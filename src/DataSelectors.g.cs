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
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    // This partial implementation was template-generated:
    // Thu, 27 Feb 2020 08:41:30 GMT

    partial class DataReaderExtensions
    {
        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T, TResult>(
                this IDataReader reader,
                Func<T, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T, TResult>(
            this IDataReader reader,
            Func<T, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T, TResult>(
            this DbDataReader reader,
            Func<T, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T, TResult>(
            this DbDataReader reader,
            Func<T, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, TResult>(
                this IDataReader reader,
                Func<T1, T2, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, TResult>(
            this IDataReader reader,
            Func<T1, T2, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, TResult>(
            this DbDataReader reader,
            Func<T1, T2, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, TResult>(
            this DbDataReader reader,
            Func<T1, T2, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }

        public static Func<IDataRecord, TResult>
            CreateRecordSelector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
                this IDataReader reader,
                Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            var f = reader.CreateRecordSelector<Func<IDataRecord, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, TResult>>(selector);
            return record => f(record, selector);
        }

        public static IEnumerator<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this IDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            var f = reader.CreateRecordSelector(selector);
            while (reader.Read())
                yield return f(reader);
        }

        #if NETSTANDARD2_1

        public static async IAsyncEnumerator<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector,
            CancellationToken cancellationToken = default)
        {
            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                yield return f(reader);
        }

        #endif

        public static async Task<TResult[]> ReadAllAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DbDataReader reader,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector) =>
            (await reader.CollectAsync(new List<TResult>(), selector).ConfigureAwait(false)).ToArray();

        public static async Task<TCollection> CollectAsync<TCollection, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DbDataReader reader, TCollection collection,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
            where TCollection : ICollection<TResult>
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector(selector);
            while (await reader.ReadAsync().ConfigureAwait(false))
                collection.Add(f(reader));
            return collection;
        }
    }

    partial class DbCommandExtensions
    {
        public static IEnumerable<TResult> Select<T, TResult>(
            this IDbCommand command,
            Func<T, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, TResult>(
            this IDbCommand command,
            Func<T1, T2, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this IDbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }
    }

    #if NETSTANDARD2_1

    partial class DbCommandExtensions
    {
        public static IAsyncEnumerable<TResult> SelectAsync<T, TResult>(
            this DbCommand command,
            Func<T, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, TResult>(
            this DbCommand command,
            Func<T1, T2, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }

        public static IAsyncEnumerable<TResult> SelectAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DbCommand command,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.FromAsync(ct => command.ExecuteReaderAsync(ct),
                                          (r, ct) => r.SelectAsync(selector, ct));
        }
    }

    #endif // NETSTANDARD2_1

    partial class DataTableExtensions
    {
        public static IEnumerable<TResult> Select<T, TResult>(
            this DataTable table,
            Func<T, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, TResult>(
            this DataTable table,
            Func<T1, T2, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, TResult>(
            this DataTable table,
            Func<T1, T2, T3, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }

        public static IEnumerable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this DataTable table,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }
    }
}
