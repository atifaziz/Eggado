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
    using JetBrains.Annotations;

    #endregion

    public static partial class DataTableExtensions
    {
        public static IEnumerable<T> Select<T>([NotNull] this DataTable table)
            where T : new()
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            return Eggnumerable.From(() => new DataTableReader(table), r => r.Select<T>());
        }

        public static IEnumerable<TResult> Select<T, TResult>(
            [NotNull] this DataTable table,
            [NotNull] Func<T, TResult> selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return Eggnumerable.From(() => new DataTableReader(table), r => r.Select(selector));
        }

        public static IEnumerable<dynamic> Select(
            [NotNull] this DataTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            return Eggnumerable.From(() => new DataTableReader(table), r => r.Select());
        }
    }
}