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

namespace Eggado.Experimental
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public interface IDataReaderQuery<out T>
    {
        T GetResult(IDataReader reader);
    }

    public interface IDataReaderQuery<out TArgs, out T> : IDataReaderQuery<T>
    {
        TArgs Args { get; }
    }

    public struct DataReaderFieldQueryArgs
    {
        public int Ordinal { get; }
        public DataReaderFieldQueryArgs(int ordinal) => Ordinal = ordinal;
    }

    public static class Eggnumerable
    {
        public static IEnumerable<T> From<T>(Func<IDataReader> opener, IDataReaderQuery<IDataReaderQuery<T>> query)
        {
            if (opener == null) throw new ArgumentNullException(nameof(opener));
            if (query == null) throw new ArgumentNullException(nameof(query));

            using var reader = opener();
            var result = query.GetResult(reader);
            while (reader.Read())
                yield return result.GetResult(reader);
        }
    }

    public static class DataReaderQuery
    {
        public static IEnumerable<T> Read<T>(this DataTable table,
            IDataReaderQuery<IDataReaderQuery<T>> recordReader)
        {
            using var reader = table.CreateDataReader();
            var rr = recordReader.GetResult(reader);
            while (reader.Read())
                yield return rr.GetResult(reader);
        }

        public static IEnumerable<T> Read<T>(this DbCommand command,
            IDataReaderQuery<IDataReaderQuery<T>> recordReader)
        {
            using var reader = command.ExecuteReader();
            var rr = recordReader.GetResult(reader);
            while (reader.Read())
                yield return rr.GetResult(reader);
        }

        public static readonly IDataReaderQuery<int> FieldCount = Create(r => r.FieldCount);

        public static readonly IDataReaderQuery<IList<KeyValuePair<int, string>>> Names =
            from fc in FieldCount
            from fs in Enumerable.Range(0, fc)
                                 .Select(ord => from name in GetName(ord)
                                                select KeyValuePair.Create(ord, name))
                                 .Collect()
            select fs;

        public static IDataReaderQuery<DataTable> GetSchemaTable() => Create(r => r.GetSchemaTable());
        public static IDataReaderQuery<int> GetOrdinal(string name) => Create(r => r.GetOrdinal(name));

        public static IDataReaderQuery<string> GetName(int ordinal) => Create(r => r.GetName(ordinal));

        public static IDataReaderQuery<bool> IsDBNull(int ordinal) => Create(r => r.IsDBNull(ordinal));

        static IDataReaderQuery<IList<string>> X =
            from ns in Names
            from ss in ns.Where(n => n.Value.StartsWith("$")).Select(n => GetString(n.Key)).Collect()
            select ss;

        public static IDataReaderQuery<DataReaderFieldQueryArgs, object>   GetValue   (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetValue(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, char>     GetChar    (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetChar(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, byte>     GetByte    (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetByte(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, short>    GetInt16   (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetInt16(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, int>      GetInt32   (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetInt32(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, long>     GetInt64   (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetInt64(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, float>    GetFloat   (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetFloat(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, double>   GetDouble  (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetDouble(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, decimal>  GetDecimal (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetDecimal(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, DateTime> GetDateTime(int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetDateTime(arg.Ordinal));
        public static IDataReaderQuery<DataReaderFieldQueryArgs, string>   GetString  (int ordinal) => Create(new DataReaderFieldQueryArgs(ordinal), (arg, r) => r.GetString(arg.Ordinal));

        public static IDataReaderQuery<object>    Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, object>   q) => q.Nullable(null, v => v);
        public static IDataReaderQuery<char?>     Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, char>     q) => q.Nullable(default, v => (char?)v);
        public static IDataReaderQuery<byte?>     Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, byte>     q) => q.Nullable(default, v => (byte?)v);
        public static IDataReaderQuery<short?>    Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, short>    q) => q.Nullable(default, v => (short?)v);
        public static IDataReaderQuery<int?>      Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, int>      q) => q.Nullable(default, v => (int?)v);
        public static IDataReaderQuery<long?>     Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, long>     q) => q.Nullable(default, v => (long?)v);
        public static IDataReaderQuery<float?>    Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, float>    q) => q.Nullable(default, v => (float?)v);
        public static IDataReaderQuery<double?>   Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, double>   q) => q.Nullable(default, v => (double?)v);
        public static IDataReaderQuery<decimal?>  Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, decimal>  q) => q.Nullable(default, v => (decimal?)v);
        public static IDataReaderQuery<DateTime?> Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, DateTime> q) => q.Nullable(default, v => (DateTime?)v);
        public static IDataReaderQuery<string>    Nullable(this IDataReaderQuery<DataReaderFieldQueryArgs, string>   q) => q.Nullable(null, v => v);

        public static IDataReaderQuery<TResult> Nullable<T, TResult>(this IDataReaderQuery<DataReaderFieldQueryArgs, T> reader, TResult none, Func<T, TResult> some) =>
            from f in IsDBNull(reader.Args.Ordinal)
            from r in f ? Return(none) : reader.Select(some)
            select r;

        static IDataReaderQuery<TArgs, T> Create<TArgs, T>(TArgs args, Func<TArgs, IDataReader, T> f) =>
            new DelegatingDataReaderQuery<TArgs, T>(args, f);

        public static IDataReaderQuery<T> Create<T>(Func<IDataReader, T> f) =>
            new DelegatingDataReaderQuery<T>(f);

        public static IDataReaderQuery<T> Return<T>(T value) => Create(_ => value);

        public static IDataReaderQuery<TResult> Select<T, TResult>(this IDataReaderQuery<T> reader, Func<T, TResult> selector) =>
            Create(r => selector(reader.GetResult(r)));

        public static IDataReaderQuery<TResult> SelectMany<T, TResult>(this IDataReaderQuery<T> reader, Func<T, IDataReaderQuery<TResult>> selector) =>
            Create(r => selector(reader.GetResult(r)).GetResult(r));

        public static IDataReaderQuery<IList<T>> Collect<T>(this IEnumerable<IDataReaderQuery<T>> queries) =>
            Create(r => queries.Select(q => q.GetResult(r)).ToList());

        public static IDataReaderQuery<TResult>
            SelectMany<TFirst, TSecond, TResult>(this IDataReaderQuery<TFirst> first,
                                                 Func<TFirst, IDataReaderQuery<TSecond>> secondSelector,
                                                 Func<TFirst, TSecond, TResult> resultSelector) =>
            first.Select(a => secondSelector(a).Select(b => resultSelector(a, b)))
                 .SelectMany(c => c);

        sealed class DelegatingDataReaderQuery<T> : IDataReaderQuery<T>
        {
            readonly Func<IDataReader, T> _function;

            public DelegatingDataReaderQuery(Func<IDataReader, T> f) => _function = f;
            public T GetResult(IDataReader reader) => _function(reader);
        }

        sealed class DelegatingDataReaderQuery<TArgs, T> : IDataReaderQuery<TArgs, T>
        {
            readonly Func<TArgs, IDataReader, T> _function;

            public DelegatingDataReaderQuery(TArgs args, Func<TArgs, IDataReader, T> f) =>
                (Args, _function) = (args, f);

            public TArgs Args { get; }

            public T GetResult(IDataReader reader) => _function(Args, reader);
        }
    }
}
