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
    using System.Threading;
    using System.Threading.Tasks;

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/DbEnumerator.cs

    public partial class DbEnumerator : IEnumerator
    {
        readonly DbDataReader _reader;
        readonly CancellationToken _cancellationToken;
        SchemaInfo[] _schemaInfo; // shared schema info among all the data records
        FieldNameLookup _fieldNameLookup;
        readonly bool _closeReader;

        // users must get enumerators off of the datareader interfaces
        public DbEnumerator(DbDataReader reader, CancellationToken cancellationToken = default) :
            this(reader, false, cancellationToken) {}

        public DbEnumerator(DbDataReader reader, bool closeReader, CancellationToken cancellationToken = default)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _closeReader = closeReader;
            _cancellationToken = cancellationToken;
        }

        object IEnumerator.Current => Current;

        public IDataRecord Current { get; private set; }

        void OnMoveNext()
        {
            if (null == _schemaInfo)
                BuildSchemaInfo();

            Debug.Assert(null != _schemaInfo, "unable to build schema information!");

            Current = null;
        }

        IDataRecord ReadRecord()
        {
            var values = new object[_schemaInfo.Length];
            _reader.GetValues(values); // this.GetValues()
            return new DataRecordInternal(_schemaInfo, values, _fieldNameLookup);
        }

        public bool MoveNext()
        {
            OnMoveNext();

            if (!_reader.Read())
            {
                if (_closeReader)
                    _reader.Close();
                return false;
            }

            Current = ReadRecord();
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Reset() => throw new NotSupportedException();

        void BuildSchemaInfo()
        {
            var count = _reader.FieldCount;
            var fieldNames = new string[count];
            for (var i = 0; i < count; ++i)
                fieldNames[i] = _reader.GetName(i);

            BuildSchemaTableInfoTableNames(fieldNames);

            var si = new SchemaInfo[count];
            for (var i = 0; i < si.Length; i++)
            {
                si[i] = new SchemaInfo(_reader.GetName(i),
                                       _reader.GetDataTypeName(i),
                                       _reader.GetFieldType(i));
            }

            _schemaInfo = si;
            _fieldNameLookup = new FieldNameLookup(_reader);

            // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/AdapterUtil.Common.cs

            // { "a", "a", "a" } -> { "a", "a1", "a2" }
            // { "a", "a", "a1" } -> { "a", "a2", "a1" }
            // { "a", "A", "a" } -> { "a", "A1", "a2" }
            // { "a", "A", "a1" } -> { "a", "A2", "a1" }

            static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
            {
                var hash = new Dictionary<string, int>(columnNameArray.Length);

                var startIndex = columnNameArray.Length; // lowest non-unique index
                for (var i = columnNameArray.Length - 1; 0 <= i; --i)
                {
                    var columnName = columnNameArray[i];
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        columnName = columnName.ToLowerInvariant();
                        if (hash.TryGetValue(columnName, out var index))
                            startIndex = Math.Min(startIndex, index);
                        hash[columnName] = i;
                    }
                    else
                    {
                        columnNameArray[i] = string.Empty;
                        startIndex = i;
                    }
                }

                var uniqueIndex = 1;
                for (var i = startIndex; i < columnNameArray.Length; ++i)
                {
                    var columnName = columnNameArray[i];
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
                            GenerateUniqueName(hash, ref columnNameArray[i], i, 1);
                    }
                }

                // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/AdapterUtil.Common.cs

                static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
                {
                    for (; ; ++uniqueIndex)
                    {
                        var uniqueName = columnName + uniqueIndex.ToString(CultureInfo.InvariantCulture);
                        var lowerName = uniqueName.ToLowerInvariant();
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
    }

    #if NETSTANDARD2_1

    partial class DbEnumerator : IAsyncEnumerator<IDataRecord>
    {
        public async ValueTask<bool> MoveNextAsync()
        {
            OnMoveNext();

            if (!await _reader.ReadAsync(_cancellationToken).ConfigureAwait(false))
            {
                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            Current = ReadRecord();
            return true;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_closeReader)
                return;
            await _reader.CloseAsync().ConfigureAwait(false);
        }
    }

    #endif // NETSTANDARD2_1
}
