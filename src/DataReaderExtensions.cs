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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Caching;
    using JetBrains.Annotations;
    using Mannex.Collections.Generic;

    #endregion

    public static partial class DataReaderExtensions
    {
        private static readonly ObjectCache Cache = new MemoryCache("Eggado");
        
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

        public static IEnumerable<IDataRecord> SelectRecords([NotNull] this IDataReader reader)
        {
            return Select<IDataRecord>(reader, r => r);
        }

        public static IEnumerable<T> Select<T>(
            [NotNull] this IDataReader reader, 
            [NotNull] Func<IDataRecord, T> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            DbEnumerator e;
            using ((e = new DbEnumerator(reader)) as IDisposable)
            while (e.MoveNext())
                yield return selector((IDataRecord) e.Current);
        }

        public static IEnumerable<dynamic> Select([NotNull] this IDataReader reader)
        {
            return reader.Select(record => new DynamicRecord(record));
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
            [NotNull] this IDataReader reader,
            [NotNull] Func<T, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static T CreateRecordSelector<T>(
            [NotNull] this IDataReader reader, 
            [NotNull] Delegate selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            return PageRecordSelector(GetMappings(reader, selector), selector.GetType(), (mappings, type) => CreateRecordSelectorLambdaForDelegate<T>(mappings, type).Compile());
        }

        private static T PageRecordSelector<T>(Mappings mappings, Type type, Func<IEnumerable<Mapping>, Type, T> factory)
        {
            Debug.Assert(mappings != null);
            Debug.Assert(type != null);
            Debug.Assert(factory != null);

            var cache = Cache;

            var cacheKey = type.GUID.ToString();
            var cachedSelectors = (IEnumerable<KeyValuePair<Mappings, Delegate>>) cache[cacheKey];
            if (cachedSelectors != null)
            {
                var cached = cachedSelectors.FirstOrDefault(e => e.Key.GetHashCode().Equals(mappings.GetHashCode()) && e.Key.Equals(mappings));
                if (cached.Key != null)
                    return (T) (object) cached.Value;
            }

            var selector = factory(mappings, type);
            
            cache[cacheKey] = (cachedSelectors ?? Enumerable.Empty<KeyValuePair<Mappings, Delegate>>()).Concat(new[] { mappings.AsKeyTo((Delegate) (object) selector) }).ToArray();

            return selector;
        }

        static T[] ToArray<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var collection = source as ICollection<T>;
            if (collection != null)
            {
                var array = new T[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext()) return new T[0];
                var e1 = e.Current;
                if (!e.MoveNext()) return new[] { e1 };
                var e2 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2 };
                var e3 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3 };
                var e4 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4 };
                var e5 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5 };
                var e6 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6 };
                var e7 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7 };
                var e8 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8 };
                var e9 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9 };
                var e10 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
                var e11 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
                var e12 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
                var e13 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
                var e14 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
                var e15 = e.Current;
                if (!e.MoveNext()) return new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
                var e16 = e.Current;
                var array = new[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, e16 };
                var i = array.Length;
                for (; e.MoveNext(); i++)
                {
                    if (i == array.Length)
                        Array.Resize(ref array, checked(array.Length * 2));
                    array[i] = e.Current;
                }
                if (i < array.Length)
                    Array.Resize(ref array, i);
                return array;
            }
        }

        public static Expression<T> CreateRecordSelectorLambda<T>(
            [NotNull] this IDataReader reader, 
            [NotNull] Delegate selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");
            return CreateRecordSelectorLambdaForDelegate<T>(EnumerateMappings(reader, selector), selector.GetType());
        }

        private static Expression<T> CreateRecordSelectorLambdaForDelegate<T>(IEnumerable<Mapping> mappings, Type delegateType)
        {
            Debug.Assert(mappings != null);
            Debug.Assert(delegateType != null);
            Debug.Assert(typeof(Delegate).IsAssignableFrom(delegateType));
            
            var rpe = Expression.Parameter(typeof(IDataRecord), "record");
            var getters = from m in mappings
                          select Expression.Invoke(GetValueLambda(rpe, m.Ordinal, m.SourceType, m.TargetType), rpe);
            var spe = Expression.Parameter(delegateType, "selector");
            return Expression.Lambda<T>(Expression.Invoke(spe, getters), rpe, spe);
        }

        private static Mappings GetMappings(IDataRecord source, Delegate target)
        {
            return new Mappings(EnumerateMappings(source, target));
        }

        private static IEnumerable<Mapping> EnumerateMappings(IDataRecord source, Delegate target)
        {
            Debug.Assert(source != null);
            Debug.Assert(target != null);

            return from p in target.Method.GetParameters()
                   let ordinal = source.GetOrdinal(p.Name)
                   let fieldType = source.GetFieldType(ordinal)
                   select new Mapping(ordinal, fieldType, p.ParameterType);
        }

        public static IEnumerable<T> Select<T>([NotNull] this IDataReader reader)
            where T : new()
        {
            var f = reader.CreateRecordSelector<T>();
            while (reader.Read())
                yield return f(reader);
        }

        public static Func<IDataRecord, T> CreateRecordSelector<T>([NotNull] this IDataReader reader)
            where T : new()
        {
            if (reader == null) throw new ArgumentNullException("reader");
            return PageRecordSelector(GetMappings(reader, typeof(T)), typeof(T), (mappings, _) => CreateRecordSelectorLambda<T>(mappings).Compile());
        }

        public static Expression<Func<IDataRecord, T>> CreateRecordSelectorLambda<T>([NotNull] this IDataReader reader)
            where T : new()
        {
            return CreateRecordSelectorLambda<T>(EnumerateMappings(reader, typeof(T)));
        }

        private static Expression<Func<IDataRecord, T>> CreateRecordSelectorLambda<T>(IEnumerable<Mapping> mappings) 
            where T : new()
        {
            Debug.Assert(mappings != null);

            var record = Expression.Parameter(typeof(IDataRecord), "record");
            var obj = Expression.Variable(typeof(T));
            
            var statements = new IEnumerable<Expression>[]
            {
                new[] { Expression.Assign(obj, Expression.New(typeof(T))) },
                from m in mappings
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

        private static Mappings GetMappings(IDataRecord source, Type targetType)
        {
            return new Mappings(EnumerateMappings(source, targetType));
        }

        private static IEnumerable<Mapping> EnumerateMappings(IDataRecord source, Type targetType)
        {
            Debug.Assert(source != null);
            Debug.Assert(targetType != null);

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

            result = Convert(result.Type == sourceType
                             ? result
                             : Expression.Convert(result, sourceType),
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
        /// directly returned as the resulting expression.
        /// </remarks>

        private static Expression Convert(Expression source, Type targetType)
        {
            Debug.Assert(source != null);
            Debug.Assert(targetType != null);

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
            Debug.Assert(e != null);
            return ((MethodCallExpression)e.Body).Method;
        }

        [ Serializable ]
        sealed class Mappings : IEquatable<Mappings>, IEnumerable<Mapping>
        {
            private int? _hashCode;
            private readonly IEnumerable<Mapping> _entries;

            public Mappings(IEnumerable<Mapping> mappings)
            {
                Debug.Assert(mappings != null);
                _entries = mappings.ToArray();
            }

            public bool Equals(Mappings other)
            {
                return other != null 
                    && (ReferenceEquals(this, other) 
                        || other._entries.SequenceEqual(_entries));
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Mappings);
            }

            public override int GetHashCode()
            {
                return (int) (_hashCode ?? (_hashCode = ComputeHashCode()));
            }

            private int ComputeHashCode()
            {
                if (!_entries.Any())
                    return 0;
                
                unchecked
                {
                    var hashes = from m in _entries select m.GetHashCode() * 397;
                    return hashes.Aggregate((acc, h) => acc ^ h);
                }
            }

            public IEnumerator<Mapping> GetEnumerator()
            {
                return _entries.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public override string ToString()
            {
                return string.Join(", ", from m in this select "{ " + m + " }");
            }
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
                var member = Member;
                return string.Format(
                    member == null 
                    ? @"Ordinal: {0}, SourceType: {1}, TargetType: {2}"
                    : @"Ordinal: {0}, SourceType: {1}, TargetType: {2}, Member: {3}",
                    Ordinal, SourceType, TargetType, 
                    member != null ? member.Name : null);
            }
        }
    }
}