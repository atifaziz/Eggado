#region License, Terms and Author(s)
//
// Eggado
// Copyright (c) 2011 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Eggado
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;
    using Mannex.Collections.Generic;

    #endregion

    public static partial class DataReaderExtensions
    {
        public static IEnumerable<T> Select<T>(
            [NotNull] this IDataReader reader, 
            [NotNull] Func<IEnumerable<KeyValuePair<string, object>>, T> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            while (reader.Read())
            {
                var record = (IDataRecord) reader;
                yield return record.Select(selector);
            }
        }

        public static IEnumerable<dynamic> SelectDynamic([NotNull] this IDataReader reader)
        {
            return reader.Select(fields => fields.ToExpando());
        }

        public static T Select<T>(
            [NotNull] this IDataRecord record, 
            [NotNull] Func<IEnumerable<KeyValuePair<string, object>>, T> selector)
        {
            if (record == null) throw new ArgumentNullException("record");
            if (selector == null) throw new ArgumentNullException("selector");

            return selector(from i in Enumerable.Range(0, record.FieldCount)
                            select record.Get(i));
        }

        public static KeyValuePair<string, object> Get([NotNull] this IDataRecord record, int ordinal)
        {
            if (record == null) throw new ArgumentNullException("record");

            var name = record.GetName(ordinal);
            var value = record.IsDBNull(ordinal) ? null : record.GetValue(ordinal);
            return name.AsKeyTo(value);
        }

        public static IEnumerable<TResult> Select<T, TResult>(
            this IDataReader reader,
            Func<T, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static T CreateRecordSelector<T>(this IDataReader reader, Delegate selector)
        {
            var lambda = reader.CreateRecordSelectorLambda<T>(selector);
            return lambda.Compile();
        }

        public static Expression<T> CreateRecordSelectorLambda<T>(this IDataReader reader, Delegate selector)
        {
            var rpe = Expression.Parameter(typeof(IDataRecord), "record");
            var getters = from m in GetMappings(reader, selector)
                          select Expression.Invoke(GetValueLambda(rpe, m.Ordinal, m.SourceType, m.TargetType), rpe);
            var spe = Expression.Parameter(selector.GetType(), "selector");
            return Expression.Lambda<T>(Expression.Invoke(spe, getters), rpe, spe);
        }

        private static IEnumerable<Mapping> GetMappings(IDataRecord source, Delegate target)
        {
            return from p in target.Method.GetParameters()
                   let ordinal = source.GetOrdinal(p.Name)
                   let fieldType = source.GetFieldType(ordinal)
                   select new Mapping(ordinal, fieldType, p.ParameterType);
        }

        public static IEnumerable<T> Select<T>(this IDataReader reader)
            where T : new()
        {
            var f = reader.CreateRecordSelector<T>();
            while (reader.Read())
                yield return f(reader);
        }

        public static Func<IDataRecord, T> CreateRecordSelector<T>(this IDataReader reader)
            where T : new()
        {
            return reader.CreateRecordSelectorLambda<T>().Compile();
        }

        public static Expression<Func<IDataRecord, T>> CreateRecordSelectorLambda<T>([NotNull] this IDataReader reader) 
            where T : new()
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var record = Expression.Parameter(typeof(IDataRecord), "record");
            var obj = Expression.Variable(typeof(T));
            
            var statements = new IEnumerable<Expression>[]
            {
                new[] { Expression.Assign(obj, Expression.New(typeof(T))) },
                from m in GetMappings(reader, typeof(T))
                select Expression.Assign(
                           Expression.MakeMemberAccess(obj, m.Member), 
                           Expression.Invoke(
                               GetValueLambda(record, m.Ordinal, m.SourceType, m.TargetType), 
                               record)),
                new[] { obj },
            };
            
            var body = Expression.Block(typeof(T), new[] { obj }, statements.SelectMany(s => s));
            return Expression.Lambda<Func<IDataRecord, T>>(body, record);
        }

        private static IEnumerable<Mapping> GetMappings(IDataRecord source, Type targetType)
        {
            return from m in targetType.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, null, null)
                   let p = m.MemberType == MemberTypes.Property ? (PropertyInfo) m : null
                   let f = m.MemberType == MemberTypes.Field ? (FieldInfo) m : null
                   where (p != null && p.CanRead && p.CanWrite) 
                      || (f != null && f.Attributes.HasFlag(FieldAttributes.InitOnly))
                   let type = p != null 
                            ? p.PropertyType 
                            : f != null 
                            ? f.FieldType 
                            : null
                   let ordinal = source.GetOrdinal(m.Name)
                   let fieldType = source.GetFieldType(ordinal)
                   select new Mapping(ordinal, fieldType, type, m);
        }

        private static Expression GetValueLambda(ParameterExpression reader, int ordinal, Type fieldType, Type targetType)
        {
            var type = typeof(Func<,>).MakeGenericType(typeof(IDataRecord), targetType);
            return Expression.Lambda(type, GetValue(reader, ordinal, fieldType, targetType), reader);
        }

        private static Expression GetValue(Expression reader, int ordinal, Type sourceType, Type targetType)
        {
            Debug.Assert(targetType != null);

            bool nullable;
            var baseTargetType = targetType;

            if (targetType.IsValueType 
                && targetType.IsGenericType
                && typeof(Nullable<>) == targetType.GetGenericTypeDefinition())
            {
                nullable = true;
                baseTargetType = targetType.GetGenericArguments()[0];
            }
            else
            {
                nullable = targetType.IsClass;
            }
            
            var method = _methodByType.Find(sourceType, _dataRecordGetValueMethod);
            var result = (Expression) Expression.Call(reader, method, Expression.Constant(ordinal));

            result = Convert(result.Type == baseTargetType
                             ? result
                             : Expression.Convert(result, baseTargetType),
                             baseTargetType);

            if (targetType != baseTargetType)
                result = Expression.Convert(result, targetType);

            if (nullable)
            {
                result = Expression.Condition(
                             Expression.Call(reader, _dataRecordIsDbNullMethod, Expression.Constant(ordinal)),
                             Expression.Convert(Expression.Constant(null), targetType), result);
            }

            return result;
        }

        /// <remarks>
        /// If <paramref name="source"/> is already typed as 
        /// <paramref name="targetType"/> then <paramref name="source"/> is
        /// directly return as the resulting expression.
        /// </remarks>

        private static Expression Convert(Expression source, Type targetType)
        {
            var sourceType = source.Type;
            if (sourceType == targetType)
                return source;

            var converter = ConversionLambda.Find(sourceType, targetType);
            return converter != null 
                 ? Expression.Invoke(converter, source) 
                 : ConversionLambda.ChangeType(source, targetType);
        }

        private static readonly MethodInfo _dataRecordGetValueMethod = ReflectMethod(r => r.GetValue(0));
        private static readonly MethodInfo _dataRecordIsDbNullMethod = ReflectMethod(r => r.IsDBNull(0));

        private static readonly Dictionary<Type, MethodInfo> _methodByType = new Dictionary<Type, MethodInfo>
        {
            { typeof(bool),     ReflectMethod(r => r.GetBoolean(0))  },
            { typeof(char),     ReflectMethod(r => r.GetChar(0))     },     
            { typeof(byte),     ReflectMethod(r => r.GetByte(0))     },     
            { typeof(short),    ReflectMethod(r => r.GetInt16(0))    },    
            { typeof(int),      ReflectMethod(r => r.GetInt32(0))    },    
            { typeof(long),     ReflectMethod(r => r.GetInt64(0))    },    
            { typeof(float),    ReflectMethod(r => r.GetFloat(0))    },   
            { typeof(double),   ReflectMethod(r => r.GetDouble(0))   },   
            { typeof(decimal),  ReflectMethod(r => r.GetDecimal(0))  },  
            { typeof(DateTime), ReflectMethod(r => r.GetDateTime(0)) }, 
            { typeof(string),   ReflectMethod(r => r.GetString(0))   },
        };

        private static MethodInfo ReflectMethod<T>(Expression<Func<IDataRecord, T>> e)
        {
            return ((MethodCallExpression) e.Body).Method;
        }

        [ Serializable ]
        sealed class Mapping : IEquatable<Mapping>
        {
            public readonly int Ordinal;
            public readonly Type SourceType;
            public readonly Type TargetType;
            public readonly MemberInfo Member;

            private readonly int _hashCode;

            public Mapping(int ordinal, Type sourceType, Type targetType, MemberInfo member = null)
            {
                Debug.Assert(ordinal >= 0);
                Debug.Assert(sourceType != null);
                Debug.Assert(targetType != null);

                SourceType = sourceType;
                TargetType = targetType;
                Ordinal = ordinal;
                Member = member;
                
                _hashCode = unchecked(
                    (((ordinal * 397) 
                    ^ sourceType.GetHashCode() * 397) 
                    ^ targetType.GetHashCode() * 397) 
                    ^ (member != null ? member.GetHashCode() : 0));
            }

            public bool Equals(Mapping other)
            {
                return other != null 
                    && (ReferenceEquals(this, other) 
                        || (other.Ordinal == Ordinal 
                            && other.SourceType == SourceType
                            && other.TargetType == TargetType
                            && other.Member == Member));
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Mapping);
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }

            public override string ToString()
            {
                return string.Format(
                    @"Ordinal: {0}, SourceType: {1}, TargetType: {2}, Member: {3}", 
                    Ordinal, SourceType, TargetType, Member);
            }
        }
    }
}