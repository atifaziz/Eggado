#region Copyright (c) 2011 Atif Aziz. All rights reserved.
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Mannex.Collections.Generic;

    #endregion

    // ReSharper disable RedundantCommaInArrayInitializer

    public static partial class DataReaderExtensions
    {
        static readonly ConcurrentDictionary<object, object> Cache = new ConcurrentDictionary<object, object>();

        public static IEnumerator<T> Select<T>(
            this IDataReader reader,
            Func<IEnumerable<KeyValuePair<string, object>>, T> selector)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            while (reader.Read())
            {
                var record = (IDataRecord) reader;
                yield return record.Select(selector);
            }
        }

        public static IEnumerator<IDataRecord> SelectRecords(this IDataReader reader)
        {
            return Select<IDataRecord>(reader, r => r);
        }

        public static IEnumerator<T> Select<T>(
            this IDataReader reader,
            Func<IDataRecord, T> selector)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            DbEnumerator e;
            // DbEnumerator may implement IDisposable in the future so type
            // type conversion is safe and intended at the runtime level.
            // ReSharper disable once SuspiciousTypeConversion.Global
            using ((e = new DbEnumerator(reader)) as IDisposable)
            while (e.MoveNext())
                yield return selector((IDataRecord) e.Current);
        }

        public static IEnumerator<dynamic> Select(this IDataReader reader)
        {
            return reader.Select(record => new DynamicRecord(record));
        }

        public static T Select<T>(
            this IDataRecord record,
            Func<IEnumerable<KeyValuePair<string, object>>, T> selector)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return selector(from i in Enumerable.Range(0, record.FieldCount)
                            select record.Get(i));
        }

        public static KeyValuePair<string, object> Get(this IDataRecord record, int ordinal)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            var name = record.GetName(ordinal);
            var value = record.IsDBNull(ordinal) ? null : record.GetValue(ordinal);
            return name.AsKeyTo(value);
        }

        public static IEnumerator<TResult> Select<T, TResult>(
            this IDataReader reader,
            Func<T, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static T CreateRecordSelector<T>(
            this IDataReader reader,
            Delegate selector)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return PageRecordSelector(GetMappings(reader, selector), selector.GetType(), (mappings, type) => CreateRecordSelectorLambdaForDelegate<T>(mappings, type).Compile());
        }

        static T PageRecordSelector<T>(Mappings mappings, Type type, Func<IEnumerable<Mapping>, Type, T> factory)
        {
            Debug.Assert(mappings != null);
            Debug.Assert(type != null);
            Debug.Assert(factory != null);

            var cache = Cache;

            var cacheKey = type;
            var cachedSelectors = (IEnumerable<KeyValuePair<Mappings, Delegate>>) cache.Find(cacheKey);
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

        public static Expression<T> CreateRecordSelectorLambda<T>(
            this IDataReader reader,
            Delegate selector)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return CreateRecordSelectorLambdaForDelegate<T>(EnumerateMappings(reader, selector), selector.GetType());
        }

        static Expression<T> CreateRecordSelectorLambdaForDelegate<T>(IEnumerable<Mapping> mappings, Type delegateType)
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

        static Mappings GetMappings(IDataRecord source, Delegate target) =>
            new Mappings(EnumerateMappings(source, target));

        static readonly string[] OrdinalParameterNames =
        {
             "_1",  "_2",  "_3",  "_4",  "_5",  "_6",  "_7",  "_8",  "_9", "_10",
            "_11", "_12", "_13", "_14", "_15", "_16", "_17", "_18", "_19", "_20",
        };

        static IEnumerable<Mapping> EnumerateMappings(IDataRecord source, Delegate target)
        {
            Debug.Assert(source != null);
            Debug.Assert(target != null);

            var parameters = target.Method.GetParameters();

            var ordinally =
                OrdinalParameterNames.Take(parameters.Length)
                                      .Zip(parameters.Select(p => p.Name), (a, b) => a.Equals(b, StringComparison.OrdinalIgnoreCase))
                                      .All(yes => yes);

            return ordinally

                 ? from p in parameters
                   let fieldType = source.GetFieldType(p.Position)
                   select new Mapping(p.Position, fieldType, p.ParameterType)

                 : from p in parameters
                   let ordinal = source.GetOrdinal(p.Name)
                   let fieldType = source.GetFieldType(ordinal)
                   select new Mapping(ordinal, fieldType, p.ParameterType);
        }

        public static IEnumerator<T> Select<T>(this IDataReader reader)
            where T : new()
        {
            var f = reader.CreateRecordSelector<T>();
            while (reader.Read())
                yield return f(reader);
        }

        public static Func<IDataRecord, T> CreateRecordSelector<T>(this IDataReader reader)
            where T : new()
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            return PageRecordSelector(GetMappings(reader, typeof(T)), typeof(T), (mappings, _) => CreateRecordSelectorLambda<T>(mappings).Compile());
        }

        public static Expression<Func<IDataRecord, T>> CreateRecordSelectorLambda<T>(this IDataReader reader)
            where T : new()
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            return CreateRecordSelectorLambda<T>(EnumerateMappings(reader, typeof(T)));
        }

        static Expression<Func<IDataRecord, T>> CreateRecordSelectorLambda<T>(IEnumerable<Mapping> mappings)
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

        static Mappings GetMappings(IDataRecord source, Type targetType) =>
            new Mappings(EnumerateMappings(source, targetType));

        static IEnumerable<Mapping> EnumerateMappings(IDataRecord source, Type targetType)
        {
            Debug.Assert(source != null);
            Debug.Assert(targetType != null);

            return from m in targetType.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, null, null)
                   let p = m.MemberType == MemberTypes.Property ? (PropertyInfo) m : null
                   let f = m.MemberType == MemberTypes.Field ? (FieldInfo) m : null
                   where (p != null && p.CanRead && p.CanWrite)
                      || (f != null && f.Attributes.HasFlag(FieldAttributes.InitOnly))
                   let type = p?.PropertyType ?? f?.FieldType
                   let ordinal = source.GetOrdinal(m.Name)
                   let fieldType = source.GetFieldType(ordinal)
                   select new Mapping(ordinal, fieldType, type, m);
        }

        static Expression GetValueLambda(ParameterExpression reader, int ordinal, Type fieldType, Type targetType)
        {
            var type = typeof(Func<,>).MakeGenericType(typeof(IDataRecord), targetType);
            return Expression.Lambda(type, GetValue(reader, ordinal, fieldType, targetType), reader);
        }

        static Expression GetValue(Expression reader, int ordinal, Type sourceType, Type targetType)
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

            var method = MethodByType.Find(sourceType, DataRecordGetValueMethod);
            // ReSharper disable once PossiblyMistakenUseOfParamsMethod
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
                             // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                             Expression.Call(reader, DataRecordIsDbNullMethod, Expression.Constant(ordinal)),
                             Expression.Convert(Expression.Constant(null), targetType), result);
            }

            return result;
        }

        /// <remarks>
        /// If <paramref name="source"/> is already typed as
        /// <paramref name="targetType"/> then <paramref name="source"/> is
        /// directly returned as the resulting expression.
        /// </remarks>

        static Expression Convert(Expression source, Type targetType)
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

        static readonly MethodInfo DataRecordGetValueMethod = ReflectMethod(r => r.GetValue(0));
        static readonly MethodInfo DataRecordIsDbNullMethod = ReflectMethod(r => r.IsDBNull(0));

        static readonly Dictionary<Type, MethodInfo> MethodByType = new Dictionary<Type, MethodInfo>
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

        static MethodInfo ReflectMethod<T>(Expression<Func<IDataRecord, T>> e)
        {
            Debug.Assert(e != null);
            return ((MethodCallExpression)e.Body).Method;
        }

        [Serializable]
        sealed class Mappings : IEquatable<Mappings>, IEnumerable<Mapping>
        {
            int? _hashCode;
            readonly IEnumerable<Mapping> _entries;

            public Mappings(IEnumerable<Mapping> mappings)
            {
                Debug.Assert(mappings != null);
                _entries = mappings.ToArray();
            }

            public bool Equals(Mappings other) =>
                other != null
                && (ReferenceEquals(this, other)
                    || other._entries.SequenceEqual(_entries));

            public override bool Equals(object obj) =>
                Equals(obj as Mappings);

            public override int GetHashCode() =>                      // ReSharper disable NonReadonlyFieldInGetHashCode
                (int) (_hashCode ?? (_hashCode = ComputeHashCode())); // ReSharper restore NonReadonlyFieldInGetHashCode

            int ComputeHashCode()
            {
                if (!_entries.Any())
                    return 0;

                unchecked
                {
                    var hashes = from m in _entries select m.GetHashCode() * 397;
                    return hashes.Aggregate((acc, h) => acc ^ h);
                }
            }

            public IEnumerator<Mapping> GetEnumerator() => _entries.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public override string ToString() =>
                string.Join(", ", from m in this select "{ " + m + " }");
        }

        [ Serializable ]
        sealed class Mapping : IEquatable<Mapping>
        {
            public readonly int Ordinal;
            public readonly Type SourceType;
            public readonly Type TargetType;
            public readonly MemberInfo Member;

            readonly int _hashCode;

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
                    ^ (member?.GetHashCode() ?? 0));
            }

            public bool Equals(Mapping other) =>
                other != null
                && (ReferenceEquals(this, other)
                    || (other.Ordinal == Ordinal
                        && other.SourceType == SourceType
                        && other.TargetType == TargetType
                        && other.Member == Member));

            public override bool Equals(object obj) => Equals(obj as Mapping);

            public override int GetHashCode() => _hashCode;

            public override string ToString()
            {
                var member = Member;
                return string.Format(
                    member == null
                    ? @"Ordinal: {0}, SourceType: {1}, TargetType: {2}"
                    : @"Ordinal: {0}, SourceType: {1}, TargetType: {2}, Member: {3}",
                    Ordinal, SourceType, TargetType,
                    member?.Name);
            }
        }
    }
}
