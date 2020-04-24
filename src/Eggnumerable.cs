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
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Eggnumerable
    {
        public static IEnumerable<T> From<TCursor, T>(Func<TCursor> opener, Func<TCursor, IEnumerator<T>> selector)
        {
            if (opener == null) throw new ArgumentNullException(nameof(opener));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return _(); IEnumerable<T> _()
            {
                var cursor = opener();
                using var _ = cursor as IDisposable;
                using var e = selector(cursor);

                while (e.MoveNext())
                    yield return e.Current;
            }
        }

        #if ASYNC_STREAMS

        public static IAsyncEnumerable<T>
            FromAsync<TCursor, T>(Func<CancellationToken, Task<TCursor>> opener,
                                  Func<TCursor, CancellationToken, IAsyncEnumerator<T>> selector)
        {
            if (opener == null) throw new ArgumentNullException(nameof(opener));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return FromAsyncImpl(opener, selector);
        }

        static async IAsyncEnumerable<T>
            FromAsyncImpl<TCursor, T>(Func<CancellationToken, Task<TCursor>> opener,
                                      Func<TCursor, CancellationToken, IAsyncEnumerator<T>> selector,
                                      [EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
            if (opener == null) throw new ArgumentNullException(nameof(opener));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var cursor = await opener(cancellationToken).ConfigureAwait(false);

            using (cursor as IDisposable)
            await using ((cursor as IAsyncDisposable).ConfigureAwait(false))
            {
                var e = selector(cursor, cancellationToken);
                await using (selector(cursor, cancellationToken).ConfigureAwait(false))
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                        yield return e.Current;
                }
            }
        }

        #endif

        /// <summary>
        /// A version of <see cref="System.Linq.Enumerable.ToArray{TSource}"/> that is
        /// optimized for 16 elements.
        /// </summary>

        internal static T[] ToArray<T>(this IEnumerable<T> source)
        {
            switch (source)
            {
                case null: throw new ArgumentNullException(nameof(source));

                case ICollection<T> collection:
                {
                    var array = new T[collection.Count];
                    collection.CopyTo(array, 0);
                    return array;
                }

                default:
                {
                    using var e = source.GetEnumerator();
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
        }
    }
}
