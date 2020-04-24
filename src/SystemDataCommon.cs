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
    using System.Data;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Globalization;

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/DbEnumerator.cs

    public class DbEnumerator : IEnumerator
    {
        internal IDataReader _reader;
        internal DbDataRecord _current;
        internal SchemaInfo[] _schemaInfo; // shared schema info among all the data records
        internal PropertyDescriptorCollection _descriptors; // cached property descriptors
        private FieldNameLookup _fieldNameLookup;
        private readonly bool _closeReader;

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

        private void BuildSchemaInfo()
        {
            int count = _reader.FieldCount;
            string[] fieldnames = new string[count];
            for (int i = 0; i < count; ++i)
            {
                fieldnames[i] = _reader.GetName(i);
            }
            ADP.BuildSchemaTableInfoTableNames(fieldnames);

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
        }

        private sealed class DbColumnDescriptor : PropertyDescriptor
        {
            private readonly int _ordinal;
            private readonly Type _type;

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

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/Common/src/System/Data/Common/BasicFieldNameLookup.cs

    internal class BasicFieldNameLookup
    {
        // Dictionary stores the index into the _fieldNames, match via case-sensitive
        private Dictionary<string, int> _fieldNameLookup;

        // original names for linear searches when exact matches fail
        private readonly string[] _fieldNames;

        // By default _compareInfo is set to InvariantCulture CompareInfo
        private CompareInfo _compareInfo;

        public BasicFieldNameLookup(string[] fieldNames)
        {
            if (null == fieldNames)
            {
                throw new ArgumentNullException(nameof(fieldNames));
            }
            _fieldNames = fieldNames;
        }

        public BasicFieldNameLookup(System.Collections.ObjectModel.ReadOnlyCollection<string> columnNames)
        {
            int length = columnNames.Count;
            string[] fieldNames = new string[length];
            for (int i = 0; i < length; ++i)
            {
                fieldNames[i] = columnNames[i];
            }
            _fieldNames = fieldNames;
            GenerateLookup();
        }

        public BasicFieldNameLookup(IDataReader reader)
        {
            int length = reader.FieldCount;
            string[] fieldNames = new string[length];
            for (int i = 0; i < length; ++i)
            {
                fieldNames[i] = reader.GetName(i);
            }
            _fieldNames = fieldNames;
        }

        public int GetOrdinal(string fieldName)
        {
            if (null == fieldName)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            int index = IndexOf(fieldName);
            if (-1 == index)
            {
                throw new IndexOutOfRangeException(fieldName);
            }
            return index;
        }

        public int IndexOfName(string fieldName)
        {
            if (null == _fieldNameLookup)
            {
                GenerateLookup();
            }

            int value;
            // via case sensitive search, first match with lowest ordinal matches
            return _fieldNameLookup.TryGetValue(fieldName, out value) ? value : -1;
        }

        public int IndexOf(string fieldName)
        {
            if (null == _fieldNameLookup)
            {
                GenerateLookup();
            }
            int index;
            // via case sensitive search, first match with lowest ordinal matches
            if (!_fieldNameLookup.TryGetValue(fieldName, out index))
            {
                // via case insensitive search, first match with lowest ordinal matches
                index = LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
                if (-1 == index)
                {
                    // do the slow search now (kana, width insensitive comparison)
                    index = LinearIndexOf(fieldName, ADP.DefaultCompareOptions);
                }
            }

            return index;
        }

        protected virtual CompareInfo GetCompareInfo()
        {
            return CultureInfo.InvariantCulture.CompareInfo;
        }

        private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
        {
            if (null == _compareInfo)
            {
                _compareInfo = GetCompareInfo();
            }

            int length = _fieldNames.Length;
            for (int i = 0; i < length; ++i)
            {
                if (0 == _compareInfo.Compare(fieldName, _fieldNames[i], compareOptions))
                {
                    _fieldNameLookup[fieldName] = i; // add an exact match for the future
                    return i;
                }
            }
            return -1;
        }

        // RTM common code for generating Dictionary from array of column names
        private void GenerateLookup()
        {
            int length = _fieldNames.Length;
            Dictionary<string, int> hash = new Dictionary<string, int>(length);

            // via case sensitive search, first match with lowest ordinal matches
            for (int i = length - 1; 0 <= i; --i)
            {
                string fieldName = _fieldNames[i];
                hash[fieldName] = i;
            }
            _fieldNameLookup = hash;
        }
    }

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/FieldNameLookup.cs

    internal sealed class FieldNameLookup : BasicFieldNameLookup
    {
        private readonly int _defaultLocaleID;

        public FieldNameLookup(string[] fieldNames, int defaultLocaleID) : base(fieldNames)
        {
            _defaultLocaleID = defaultLocaleID;
        }

        public FieldNameLookup(System.Collections.ObjectModel.ReadOnlyCollection<string> columnNames, int defaultLocaleID) : base(columnNames)
        {
            _defaultLocaleID = defaultLocaleID;
        }

        public FieldNameLookup(IDataReader reader, int defaultLocaleID) : base(reader)
        {
            _defaultLocaleID = defaultLocaleID;
        }

        //The compare info is specified by the server by specifying the default LocaleId.
        protected override CompareInfo GetCompareInfo()
        {
            CompareInfo compareInfo = null;
            if (-1 != _defaultLocaleID)
            {
                compareInfo = CompareInfo.GetCompareInfo(_defaultLocaleID);
            }
            if (null == compareInfo)
            {
                compareInfo = base.GetCompareInfo();
            }
            return compareInfo;
        }
    }

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/AdapterUtil.Common.cs

    static partial class ADP
    {
        public const CompareOptions DefaultCompareOptions = CompareOptions.IgnoreKanaType
                                                          | CompareOptions.IgnoreWidth
                                                          | CompareOptions.IgnoreCase;

        // { "a", "a", "a" } -> { "a", "a1", "a2" }
        // { "a", "a", "a1" } -> { "a", "a2", "a1" }
        // { "a", "A", "a" } -> { "a", "A1", "a2" }
        // { "a", "A", "a1" } -> { "a", "A2", "a1" }
        internal static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
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
        }

        private static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
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

    partial class ADP
    {
        internal static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
        {
            return new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid source buffer (size of {maxLen}) offset: {srcOffset}"), parameterName);
        }
        internal static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
        {
            return new ArgumentOutOfRangeException(FormattableString.Invariant($"Invalid destination buffer (size of {maxLen}) offset: {dstOffset}"), parameterName);
        }
        internal static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex)
        {
            return new IndexOutOfRangeException(FormattableString.Invariant($"Buffer offset '{bufferIndex}' plus the bytes available '{numBytes}' is greater than the length of the passed in buffer."));
        }
        internal static Exception InvalidDataLength(long length)
        {
            return new IndexOutOfRangeException(FormattableString.Invariant($"Data length '{length}' is less than 0."));
        }
    }

    partial class ADP
    {
        // only StackOverflowException & ThreadAbortException are sealed classes
        private static readonly Type s_stackOverflowType = typeof(StackOverflowException);
        private static readonly Type s_outOfMemoryType = typeof(OutOfMemoryException);
        private static readonly Type s_threadAbortType = typeof(System.Threading.ThreadAbortException);
        private static readonly Type s_nullReferenceType = typeof(NullReferenceException);
        private static readonly Type s_accessViolationType = typeof(AccessViolationException);
        private static readonly Type s_securityType = typeof(System.Security.SecurityException);

        internal static bool IsCatchableExceptionType(Exception e)
        {
            // a 'catchable' exception is defined by what it is not.
            Type type = e.GetType();

            return ((type != s_stackOverflowType) &&
                     (type != s_outOfMemoryType) &&
                     (type != s_threadAbortType) &&
                     (type != s_nullReferenceType) &&
                     (type != s_accessViolationType) &&
                     !s_securityType.IsAssignableFrom(type));
        }
    }

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/DataRecordInternal.cs

    internal sealed class DataRecordInternal : DbDataRecord, ICustomTypeDescriptor
    {
        private readonly SchemaInfo[] _schemaInfo;
        private readonly object[] _values;
        private PropertyDescriptorCollection _propertyDescriptors;
        private readonly FieldNameLookup _fieldNameLookup;

        // copy all runtime data information
        internal DataRecordInternal(SchemaInfo[] schemaInfo, object[] values, PropertyDescriptorCollection descriptors, FieldNameLookup fieldNameLookup)
        {
            Debug.Assert(null != schemaInfo, "invalid attempt to instantiate DataRecordInternal with null schema information");
            Debug.Assert(null != values, "invalid attempt to instantiate DataRecordInternal with null value[]");
            _schemaInfo = schemaInfo;
            _values = values;
            _propertyDescriptors = descriptors;
            _fieldNameLookup = fieldNameLookup;
        }

        public override int FieldCount
        {
            get
            {
                return _schemaInfo.Length;
            }
        }

        public override int GetValues(object[] values)
        {
            if (null == values)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int copyLen = (values.Length < _schemaInfo.Length) ? values.Length : _schemaInfo.Length;
            for (int i = 0; i < copyLen; i++)
            {
                values[i] = _values[i];
            }
            return copyLen;
        }

        public override string GetName(int i)
        {
            return _schemaInfo[i].name;
        }


        public override object GetValue(int i)
        {
            return _values[i];
        }

        public override string GetDataTypeName(int i)
        {
            return _schemaInfo[i].typeName;
        }

        public override Type GetFieldType(int i)
        {
            return _schemaInfo[i].type;
        }

        public override int GetOrdinal(string name)
        {
            return _fieldNameLookup.GetOrdinal(name);
        }

        public override object this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        public override object this[string name]
        {
            get
            {
                return GetValue(GetOrdinal(name));
            }
        }

        public override bool GetBoolean(int i)
        {
            return ((bool)_values[i]);
        }

        public override byte GetByte(int i)
        {
            return ((byte)_values[i]);
        }

        public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            int cbytes = 0;
            int ndataIndex;

            byte[] data = (byte[])_values[i];

            cbytes = data.Length;

            // since arrays can't handle 64 bit values and this interface doesn't
            // allow chunked access to data, a dataIndex outside the rang of Int32
            // is invalid
            if (dataIndex > int.MaxValue)
            {
                throw ADP.InvalidSourceBufferIndex(cbytes, dataIndex, nameof(dataIndex));
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
            catch (Exception e) when (ADP.IsCatchableExceptionType(e))
            {
                cbytes = data.Length;

                if (length < 0)
                {
                    throw ADP.InvalidDataLength(length);
                }

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                {
                    throw ADP.InvalidDestinationBufferIndex(length, bufferIndex, nameof(bufferIndex));
                }

                // if bad data index, throw
                if (dataIndex < 0 || dataIndex >= cbytes)
                {
                    throw ADP.InvalidSourceBufferIndex(length, dataIndex, nameof(dataIndex));
                }

                // if there is not enough room in the buffer for data
                if (cbytes + bufferIndex > buffer.Length)
                {
                    throw ADP.InvalidBufferSizeOrIndex(cbytes, bufferIndex);
                }
            }

            return cbytes;
        }

        public override char GetChar(int i) => ((string)_values[i])[0];

        public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            // if the object doesn't contain a char[] then the user will get an exception
            string s = (string)_values[i];

            char[] data = s.ToCharArray();

            int cchars = data.Length;

            // since arrays can't handle 64 bit values and this interface doesn't
            // allow chunked access to data, a dataIndex outside the rang of Int32
            // is invalid
            if (dataIndex > int.MaxValue)
            {
                throw ADP.InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));
            }

            int ndataIndex = (int)dataIndex;


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
            catch (Exception e) when (ADP.IsCatchableExceptionType(e))
            {
                cchars = data.Length;

                if (length < 0)
                {
                    throw ADP.InvalidDataLength(length);
                }

                // if bad buffer index, throw
                if (bufferIndex < 0 || bufferIndex >= buffer.Length)
                {
                    throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, nameof(bufferIndex));
                }

                // if bad data index, throw
                if (ndataIndex < 0 || ndataIndex >= cchars)
                {
                    throw ADP.InvalidSourceBufferIndex(cchars, dataIndex, nameof(dataIndex));
                }

                // if there is not enough room in the buffer for data
                if (cchars + bufferIndex > buffer.Length)
                {
                    throw ADP.InvalidBufferSizeOrIndex(cchars, bufferIndex);
                }
            }

            return cchars;
        }

        public override Guid GetGuid(int i)
        {
            return ((Guid)_values[i]);
        }


        public override short GetInt16(int i)
        {
            return ((short)_values[i]);
        }

        public override int GetInt32(int i)
        {
            return ((int)_values[i]);
        }

        public override long GetInt64(int i)
        {
            return ((long)_values[i]);
        }

        public override float GetFloat(int i)
        {
            return ((float)_values[i]);
        }

        public override double GetDouble(int i)
        {
            return ((double)_values[i]);
        }

        public override string GetString(int i)
        {
            return ((string)_values[i]);
        }

        public override decimal GetDecimal(int i)
        {
            return ((decimal)_values[i]);
        }

        public override DateTime GetDateTime(int i)
        {
            return ((DateTime)_values[i]);
        }

        public override bool IsDBNull(int i)
        {
            object o = _values[i];
            return (null == o || Convert.IsDBNull(o));
        }

        //
        // ICustomTypeDescriptor
        //

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }


        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (_propertyDescriptors == null)
            {
                _propertyDescriptors = new PropertyDescriptorCollection(null);
            }
            return _propertyDescriptors;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }

    // this doesn't change per record, only alloc once
    internal struct SchemaInfo
    {
        public string name;
        public string typeName;
        public Type type;
    }
}
