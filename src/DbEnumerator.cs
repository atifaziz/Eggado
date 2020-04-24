#region Copyright (c) .NET Foundation and Contributors. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
#endregion

namespace Eggado
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/DbEnumerator.cs

    public class DbEnumerator : IEnumerator
    {
        internal IDataReader _reader;
        internal DbDataRecord _current;
        internal SchemaInfo[] _schemaInfo; // shared schema info among all the data records
        internal PropertyDescriptorCollection _descriptors; // cached property descriptors
        FieldNameLookup _fieldNameLookup;
        readonly bool _closeReader;

        // users must get enumerators off of the datareader interfaces
        public DbEnumerator(IDataReader reader)
        {
            if (null == reader)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            _reader = reader;
        }

        public DbEnumerator(IDataReader reader, bool closeReader)
        {
            if (null == reader)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            _reader = reader;
            _closeReader = closeReader;
        }

        public DbEnumerator(DbDataReader reader)
            : this((IDataReader)reader)
        {
        }

        public DbEnumerator(DbDataReader reader, bool closeReader)
            : this((IDataReader)reader, closeReader)
        {
        }

        public object Current => _current;

        public bool MoveNext()
        {
            if (null == _schemaInfo)
            {
                BuildSchemaInfo();
            }

            Debug.Assert(null != _schemaInfo && null != _descriptors, "unable to build schema information!");
            _current = null;

            if (_reader.Read())
            {
                // setup our current record
                object[] values = new object[_schemaInfo.Length];
                _reader.GetValues(values); // this.GetValues()
                _current = new DataRecordInternal(_schemaInfo, values, _descriptors, _fieldNameLookup);
                return true;
            }
            if (_closeReader)
            {
                _reader.Close();
            }
            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        void BuildSchemaInfo()
        {
            int count = _reader.FieldCount;
            string[] fieldnames = new string[count];
            for (int i = 0; i < count; ++i)
            {
                fieldnames[i] = _reader.GetName(i);
            }
            BuildSchemaTableInfoTableNames(fieldnames);

            SchemaInfo[] si = new SchemaInfo[count];
            PropertyDescriptor[] props = new PropertyDescriptor[_reader.FieldCount];
            for (int i = 0; i < si.Length; i++)
            {
                SchemaInfo s = default;
                s.name = _reader.GetName(i);
                s.type = _reader.GetFieldType(i);
                s.typeName = _reader.GetDataTypeName(i);
                props[i] = new DbColumnDescriptor(i, fieldnames[i], s.type);
                si[i] = s;
            }

            _schemaInfo = si;
            _fieldNameLookup = new FieldNameLookup(_reader, -1);
            _descriptors = new PropertyDescriptorCollection(props);

            // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/AdapterUtil.Common.cs

            // { "a", "a", "a" } -> { "a", "a1", "a2" }
            // { "a", "a", "a1" } -> { "a", "a2", "a1" }
            // { "a", "A", "a" } -> { "a", "A1", "a2" }
            // { "a", "A", "a1" } -> { "a", "A2", "a1" }

            static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
            {
                Dictionary<string, int> hash = new Dictionary<string, int>(columnNameArray.Length);

                int startIndex = columnNameArray.Length; // lowest non-unique index
                for (int i = columnNameArray.Length - 1; 0 <= i; --i)
                {
                    string columnName = columnNameArray[i];
                    if ((null != columnName) && (0 < columnName.Length))
                    {
                        columnName = columnName.ToLowerInvariant();
                        int index;
                        if (hash.TryGetValue(columnName, out index))
                        {
                            startIndex = Math.Min(startIndex, index);
                        }
                        hash[columnName] = i;
                    }
                    else
                    {
                        columnNameArray[i] = string.Empty;
                        startIndex = i;
                    }
                }
                int uniqueIndex = 1;
                for (int i = startIndex; i < columnNameArray.Length; ++i)
                {
                    string columnName = columnNameArray[i];
                    if (0 == columnName.Length)
                    {
                        // generate a unique name
                        columnNameArray[i] = "Column";
                        uniqueIndex = GenerateUniqueName(hash, ref columnNameArray[i], i, uniqueIndex);
                    }
                    else
                    {
                        columnName = columnName.ToLowerInvariant();
                        if (i != hash[columnName])
                        {
                            GenerateUniqueName(hash, ref columnNameArray[i], i, 1);
                        }
                    }
                }

                // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/AdapterUtil.Common.cs

                static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
                {
                    for (; ; ++uniqueIndex)
                    {
                        string uniqueName = columnName + uniqueIndex.ToString(CultureInfo.InvariantCulture);
                        string lowerName = uniqueName.ToLowerInvariant();
                        if (hash.TryAdd(lowerName, index))
                        {
                            columnName = uniqueName;
                            break;
                        }
                    }
                    return uniqueIndex;
                }
            }
        }

        sealed class DbColumnDescriptor : PropertyDescriptor
        {
            readonly int _ordinal;
            readonly Type _type;

            internal DbColumnDescriptor(int ordinal, string name, Type type)
                : base(name, null)
            {
                _ordinal = ordinal;
                _type = type;
            }

            public override Type ComponentType => typeof(IDataRecord);
            public override bool IsReadOnly => true;
            public override Type PropertyType => _type;
            public override bool CanResetValue(object component) => false;
            public override object GetValue(object component) => ((IDataRecord)component)[_ordinal];
            public override void ResetValue(object component) => throw new NotSupportedException();
            public override void SetValue(object component, object value) => throw new NotSupportedException();
            public override bool ShouldSerializeValue(object component) => false;
        }
    }
}
