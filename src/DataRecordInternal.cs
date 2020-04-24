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
    using System.Data;
    using System.Diagnostics;
    using System.Security;
    using System.Threading;

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/DataRecordInternal.cs

    sealed class DataRecordInternal : IDataRecord
    {
        readonly SchemaInfo[] _schemaInfo;
        readonly object[] _values;
        readonly FieldNameLookup _fieldNameLookup;

        // copy all runtime data information
        public DataRecordInternal(SchemaInfo[] schemaInfo, object[] values, FieldNameLookup fieldNameLookup)
        {
            Debug.Assert(null != schemaInfo, "invalid attempt to instantiate DataRecordInternal with null schema information");
            Debug.Assert(null != values, "invalid attempt to instantiate DataRecordInternal with null value[]");
            _schemaInfo = schemaInfo;
            _values = values;
            _fieldNameLookup = fieldNameLookup;
        }

        public int FieldCount => _schemaInfo.Length;

        public int GetValues(object[] values)
        {
            if (null == values) throw new ArgumentNullException(nameof(values));

            var copyLen = values.Length < _schemaInfo.Length
                        ? values.Length
                        : _schemaInfo.Length;

            for (var i = 0; i < copyLen; i++)
                values[i] = _values[i];

            return copyLen;
        }

        public string GetName(int i) => _schemaInfo[i].Name;
        public object GetValue(int i) => _values[i];
        public string GetDataTypeName(int i) => _schemaInfo[i].TypeName;
        public Type GetFieldType(int i) => _schemaInfo[i].Type;
        public int GetOrdinal(string name) => _fieldNameLookup.GetOrdinal(name);

        public object this[int i] => GetValue(i);
        public object this[string name] => GetValue(GetOrdinal(name));

        public bool IsDBNull(int i) =>
            _values[i] switch { null => true, var o => Convert.IsDBNull(o) };

        public IDataReader GetData(int i) => throw new NotSupportedException();

        public bool     GetBoolean(int i)  => (bool)_values[i];
        public byte     GetByte(int i)     => (byte)_values[i];
        public Guid     GetGuid(int i)     => (Guid)_values[i];
        public short    GetInt16(int i)    => (short)_values[i];
        public int      GetInt32(int i)    => (int)_values[i];
        public long     GetInt64(int i)    => (long)_values[i];
        public float    GetFloat(int i)    => (float)_values[i];
        public double   GetDouble(int i)   => (double)_values[i];
        public string   GetString(int i)   => (string)_values[i];
        public decimal  GetDecimal(int i)  => (decimal)_values[i];
        public DateTime GetDateTime(int i) => (DateTime)_values[i];
        public char     GetChar(int i)     => ((string)_values[i])[0];

        public long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            var data = (byte[])_values[i];
            var cbytes = data.Length;

            // since arrays can't handle 64 bit values and this interface doesn't
            // allow chunked access to data, a dataIndex outside the rang of Int32
            // is invalid
            if (dataIndex > int.MaxValue)
                throw InvalidSourceBufferIndex(cbytes, dataIndex, nameof(dataIndex));

            // if no buffer is passed in, return the number of characters we have
            if (null == buffer)
                return cbytes;

            var ndataIndex = (int)dataIndex;

            try
            {
                // help the user out in the case where there's less data than requested
                if (ndataIndex < cbytes)
                    cbytes = ndataIndex + length > cbytes ? cbytes - ndataIndex : length;

                // until arrays are 64 bit, we have to do these casts
                Array.Copy(data, ndataIndex, buffer, bufferIndex, cbytes);
            }
            catch (Exception e) when (IsCatchableExceptionType(e))
            {
                cbytes = data.Length;

                if (length < 0)
                    throw InvalidDataLength(length);

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                    throw InvalidDestinationBufferIndex(length, bufferIndex, nameof(bufferIndex));

                // if bad data index, throw
                if (dataIndex < 0 || dataIndex >= cbytes)
                    throw InvalidSourceBufferIndex(length, dataIndex, nameof(dataIndex));

                // if there is not enough room in the buffer for data
                if (cbytes + bufferIndex > buffer.Length)
                    throw InvalidBufferSizeOrIndex(cbytes, bufferIndex);
            }

            return cbytes;
        }

        public long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            // if the object doesn't contain a char[] then the user will get an exception
            var s = (string)_values[i];
            var data = s.ToCharArray();
            var cchars = data.Length;

            // since arrays can't handle 64 bit values and this interface doesn't
            // allow chunked access to data, a dataIndex outside the rang of Int32
            // is invalid
            if (dataIndex > int.MaxValue)
                throw InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));

            // if no buffer is passed in, return the number of characters we have
            if (null == buffer)
                return cchars;

            var ndataIndex = (int)dataIndex;

            try
            {
                // help the user out in the case where there's less data than requested
                if (ndataIndex < cchars)
                    cchars = ndataIndex + length > cchars ? cchars - ndataIndex : length;

                Array.Copy(data, ndataIndex, buffer, bufferIndex, cchars);
            }
            catch (Exception e) when (IsCatchableExceptionType(e))
            {
                cchars = data.Length;

                if (length < 0)
                    throw InvalidDataLength(length);

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                    throw InvalidDestinationBufferIndex(buffer.Length, bufferIndex,
                        nameof(bufferIndex));

                // if bad data index, throw
                if (ndataIndex < 0 || ndataIndex >= cchars)
                    throw InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));

                // if there is not enough room in the buffer for data
                if (cchars + bufferIndex > buffer.Length)
                    throw InvalidBufferSizeOrIndex(cchars, bufferIndex);
            }

            return cchars;
        }

        static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName) =>
            new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid source buffer (size of {maxLen}) offset: {srcOffset}"), parameterName);

        static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName) =>
            new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid destination buffer (size of {maxLen}) offset: {dstOffset}"), parameterName);

        static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex) =>
            new IndexOutOfRangeException(FormattableString.Invariant($"Buffer offset '{bufferIndex}' plus the bytes available '{numBytes}' is greater than the length of the passed in buffer."));

        static Exception InvalidDataLength(long length) =>
            new IndexOutOfRangeException(FormattableString.Invariant($"Data length '{length}' is less than 0."));

        static bool IsCatchableExceptionType(Exception e) => e switch
        {
            // a 'catchable' exception is defined by what it is not.
            // only StackOverflowException & ThreadAbortException are sealed classes

            StackOverflowException   _ => false,
            OutOfMemoryException     _ => false,
            ThreadAbortException     _ => false,
            NullReferenceException   _ => false,
            AccessViolationException _ => false,
            SecurityException        _ => false,
            _                          => true
        };
    }

    // this doesn't change per record, only alloc once

    readonly struct SchemaInfo
    {
        public readonly string Name;
        public readonly string TypeName;
        public readonly Type Type;

        public SchemaInfo(string name, string typeName, Type type) =>
            (Name, TypeName, Type) = (name, typeName, type);
    }
}
