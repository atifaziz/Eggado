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
    using System.Collections.Generic;
    using System.Data;

    #endregion

    // This partial implementation was template-generated:
    // Mon, 30 May 2016 09:27:49 GMT

    partial class DataReaderExtensions
    {
        public static IEnumerator<TResult> Select<T1, T2, TResult>(
            this IDataReader reader,
            Func<T1, T2, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (selector == null) throw new ArgumentNullException("selector");

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }
    }

    partial class DbCommandExtensions
    {
        public static IEnumerable<TResult> Select<T1, T2, TResult>(
            this IDbCommand command,
            Func<T1, T2, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }
    }

    partial class DataTableExtensions
    {
        public static IEnumerable<TResult> Select<T1, T2, TResult>(
            this DataTable table,
            Func<T1, T2, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, TResult>(
            this DataTable table,
            Func<T1, T2, T3, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (selector == null) throw new ArgumentNullException("selector");

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }
    }
}