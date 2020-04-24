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
    using System.Data;
    using System.Data.Common;

    public static partial class DbCommandExtensions
    {
        public static IEnumerable<T> Select<T>(this IDbCommand command)
            where T : new()
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.From(command.ExecuteReader, r => r.Select<T>());
        }

        public static IEnumerable<dynamic> Select(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.From(command.ExecuteReader, r => r.Select());
        }

        public static IEnumerable<IDataRecord> SelectRecords(this IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.From(command.ExecuteReader, r => r.SelectRecords());
        }

        #if ASYNC_STREAMS

        public static IAsyncEnumerable<T> SelectAsync<T>(this DbCommand command)
            where T : new()
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.FromAsync(command.ExecuteReaderAsync, (r, ct) => r.SelectAsync<T>(ct));
        }

        public static IAsyncEnumerable<dynamic> SelectAsync(this DbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.FromAsync(command.ExecuteReaderAsync, (r, ct) => r.SelectAsync(ct));
        }

        public static IAsyncEnumerable<IDataRecord> SelectRecordsAsync(this DbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            return Eggnumerable.FromAsync(command.ExecuteReaderAsync, (r, ct) => r.SelectRecordsAsync(ct));
        }

        #endif
    }
}
