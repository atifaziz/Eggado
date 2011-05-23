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

    #endregion

    public static partial class DbCommandExtensions
    {
        public static IEnumerable<TResult> Select<T, TResult>(
            this IDbCommand command,
            Func<T, TResult> selector)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (selector == null) throw new ArgumentNullException("selector");

            using (var reader = command.ExecuteReader())
                foreach (var record in reader.Select(selector))
                    yield return record;
        }
    }
}
