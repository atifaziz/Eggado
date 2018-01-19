using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

partial class Program
{
    static IEnumerable<string> GenerateDataSelectors()
    {
        var targz =
            from arity in Enumerable.Range(2, 15)
            let ts = from n in Enumerable.Range(1, arity)
                     select "T" + n.ToString(CultureInfo.InvariantCulture)
            select "<" + string.Join(", ", ts) + ", TResult>";

        yield return @"
#region Copyright (c) 2011 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the ""License"");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an ""AS IS"" BASIS,
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
    // " + DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture) + @"

    partial class DataReaderExtensions
    {";
        foreach (var targs in targz)
        {
            yield return @"

        public static IEnumerator<TResult> Select" + targs + @"(
            this IDataReader reader,
            Func" + targs + @" selector)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var f = reader.CreateRecordSelector<Func<IDataRecord, Func" + targs + @", TResult>>(selector);
            while (reader.Read())
                yield return f(reader, selector);
        }";
        }

        yield return @"
    }

    partial class DbCommandExtensions
    {";
        foreach (var targs in targz)
        {
            yield return @"

        public static IEnumerable<TResult> Select" + targs + @"(
            this IDbCommand command,
            Func" + targs + @" selector)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(command.ExecuteReader, r => r.Select(selector));
        }";
        }

        yield return @"
    }

    partial class DataTableExtensions
    {";
        foreach (var targs in targz)
        {
            yield return @"

        public static IEnumerable<TResult> Select" + targs + @"(
            this DataTable table,
            Func" + targs + @" selector)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Eggnumerable.From(table.CreateDataReader, r => r.Select(selector));
        }";
        }

        yield return @"
    }
}";
    }
}
