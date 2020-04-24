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

        public int FieldCount
        {
            get
            {
                return _schemaInfo.Length;
            }
        }

        public int GetValues(object[] values)
        {
            if (null == values)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var copyLen = (values.Length < _schemaInfo.Length) ? values.Length : _schemaInfo.Length;
            for (var i = 0; i < copyLen; i++)
            {
                values[i] = _values[i];
            }
            return copyLen;
        }

        public string GetName(int i)
        {
            return _schemaInfo[i].Name;
        }


        public object GetValue(int i)
        {
            return _values[i];
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public string GetDataTypeName(int i)
        {
            return _schemaInfo[i].TypeName;
        }

        public Type GetFieldType(int i)
        {
            return _schemaInfo[i].Type;
        }

        public int GetOrdinal(string name)
        {
            return _fieldNameLookup.GetOrdinal(name);
        }

        public object this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        public object this[string name]
        {
            get
            {
                return GetValue(GetOrdinal(name));
            }
        }

        public bool GetBoolean(int i)
        {
            return ((bool)_values[i]);
        }

        public byte GetByte(int i)
        {
            return ((byte)_values[i]);
        }

        public long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            var cbytes = 0;
            int ndataIndex;

            var data = (byte[])_values[i];

            cbytes = data.Length;

            // since arrays can't handle 64 bit values and this interface doesn't
            // allow chunked access to data, a dataIndex outside the rang of Int32
            // is invalid
            if (dataIndex > int.MaxValue)
            {
                throw InvalidSourceBufferIndex(cbytes, dataIndex, nameof(dataIndex));
            }

            ndataIndex = (int)dataIndex;

            // if no buffer is passed in, return the number of characters we have
            if (null == buffer)
                return cbytes;

            try
            {
                if (ndataIndex < cbytes)
                {
                    // help the user out in the case where there's less data than requested
                    if ((ndataIndex + length) > cbytes)
                        cbytes = cbytes - ndataIndex;
                    else
                        cbytes = length;
                }

                // until arrays are 64 bit, we have to do these casts
                Array.Copy(data, ndataIndex, buffer, bufferIndex, cbytes);
            }
            catch (Exception e) when (IsCatchableExceptionType(e))
            {
                cbytes = data.Length;

                if (length < 0)
                {
                    throw InvalidDataLength(length);
                }

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                {
                    throw InvalidDestinationBufferIndex(length, bufferIndex, nameof(bufferIndex));
                }

                // if bad data index, throw
                if (dataIndex < 0 || dataIndex >= cbytes)
                {
                    throw InvalidSourceBufferIndex(length, dataIndex, nameof(dataIndex));
                }

                // if there is not enough room in the buffer for data
                if (cbytes + bufferIndex > buffer.Length)
                {
                    throw InvalidBufferSizeOrIndex(cbytes, bufferIndex);
                }
            }

            return cbytes;
        }

        public char GetChar(int i) => ((string)_values[i])[0];

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
            {
                throw InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));
            }

            var ndataIndex = (int)dataIndex;


            // if no buffer is passed in, return the number of characters we have
            if (null == buffer)
            {
                return cchars;
            }

            try
            {
                if (ndataIndex < cchars)
                {
                    // help the user out in the case where there's less data than requested
                    if ((ndataIndex + length) > cchars)
                    {
                        cchars = cchars - ndataIndex;
                    }
                    else
                    {
                        cchars = length;
                    }
                }

                Array.Copy(data, ndataIndex, buffer, bufferIndex, cchars);
            }
            catch (Exception e) when (IsCatchableExceptionType(e))
            {
                cchars = data.Length;

                if (length < 0)
                {
                    throw InvalidDataLength(length);
                }

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                {
                    throw InvalidDestinationBufferIndex(buffer.Length, bufferIndex, nameof(bufferIndex));
                }

                // if bad data index, throw
                if (ndataIndex < 0 || ndataIndex >= cchars)
                {
                    throw InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));
                }

                // if there is not enough room in the buffer for data
                if (cchars + bufferIndex > buffer.Length)
                {
                    throw InvalidBufferSizeOrIndex(cchars, bufferIndex);
                }
            }

            return cchars;
        }

        static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
        {
            return new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid source buffer (size of {maxLen}) offset: {srcOffset}"), parameterName);
        }

        static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
        {
            return new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid destination buffer (size of {maxLen}) offset: {dstOffset}"), parameterName);
        }

        static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex)
        {
            return new IndexOutOfRangeException(FormattableString.Invariant($"Buffer offset '{bufferIndex}' plus the bytes available '{numBytes}' is greater than the length of the passed in buffer."));
        }

        static Exception InvalidDataLength(long length)
        {
            return new IndexOutOfRangeException(FormattableString.Invariant($"Data length '{length}' is less than 0."));
        }

        public Guid GetGuid(int i)
        {
            return ((Guid)_values[i]);
        }


        public short GetInt16(int i)
        {
            return ((short)_values[i]);
        }

        public int GetInt32(int i)
        {
            return ((int)_values[i]);
        }

        public long GetInt64(int i)
        {
            return ((long)_values[i]);
        }

        public float GetFloat(int i)
        {
            return ((float)_values[i]);
        }

        public double GetDouble(int i)
        {
            return ((double)_values[i]);
        }

        public string GetString(int i)
        {
            return ((string)_values[i]);
        }

        public decimal GetDecimal(int i)
        {
            return ((decimal)_values[i]);
        }

        public DateTime GetDateTime(int i)
        {
            return ((DateTime)_values[i]);
        }

        public bool IsDBNull(int i)
        {
            var o = _values[i];
            return (null == o || Convert.IsDBNull(o));
        }

        // only StackOverflowException & ThreadAbortException are sealed classes
        static readonly Type StackOverflowType = typeof(StackOverflowException);
        static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);
        static readonly Type ThreadAbortType = typeof(System.Threading.ThreadAbortException);
        static readonly Type NullReferenceType = typeof(NullReferenceException);
        static readonly Type AccessViolationType = typeof(AccessViolationException);
        static readonly Type SecurityType = typeof(System.Security.SecurityException);

        static bool IsCatchableExceptionType(Exception e)
        {
            // a 'catchable' exception is defined by what it is not.
            var type = e.GetType();

            return ((type != StackOverflowType) &&
                     (type != OutOfMemoryType) &&
                     (type != ThreadAbortType) &&
                     (type != NullReferenceType) &&
                     (type != AccessViolationType) &&
                     !SecurityType.IsAssignableFrom(type));
        }
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
