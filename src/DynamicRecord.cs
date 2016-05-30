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
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Linq;
    using JetBrains.Annotations;
    using Mannex.Collections.Generic;

    #endregion

    sealed class DynamicRecord : DynamicObject, IEnumerable<KeyValuePair<string, object>>
    {
        readonly IDataRecord _record;

        internal DynamicRecord([NotNull] IDataRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            _record = record;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null) throw new ArgumentNullException("binder");
            var value = _record[binder.Name];
            result = Value(value);
            return true;
        }

        static object Value(object value)
        {
            return DBNull.Value != value ? value : null;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return from i in Enumerable.Range(1, _record.FieldCount)
                   select _record.GetName(i);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _record.Select(ps => from p in ps select p.Key.AsKeyTo(Value(p.Value))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
