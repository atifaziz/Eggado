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

#if NETSTANDARD2_0

namespace Eggado
{
    using System;
    using System.Collections.Generic;

    static class Extensions
    {
        /// <summary>
        /// Emulates <a href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.tryadd?view=netstandard-2.1">
        /// <c>TryAdd</c></a> that was added to <see cref="Dictionary{TKey,TValue}"/>
        /// in .NET Standard 2.1.
        /// </summary>
        /// <remarks>
        /// This method is not thread-safe.
        /// </remarks>

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (dictionary.ContainsKey(key))
                return false;
            dictionary.Add(key, value);
            return true;
        }
    }

}

#endif
