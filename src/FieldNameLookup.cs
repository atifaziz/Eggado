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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/Common/src/System/Data/Common/BasicFieldNameLookup.cs

    class BasicFieldNameLookup
    {
        // Dictionary stores the index into the _fieldNames, match via case-sensitive
        Dictionary<string, int> _fieldNameLookup;

        // original names for linear searches when exact matches fail
        readonly string[] _fieldNames;

        // By default _compareInfo is set to InvariantCulture CompareInfo
        CompareInfo _compareInfo;

        public BasicFieldNameLookup(string[] fieldNames) =>
            _fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));

        public BasicFieldNameLookup(ReadOnlyCollection<string> columnNames)
        {
            var length = columnNames.Count;
            var fieldNames = new string[length];
            for (var i = 0; i < length; ++i)
                fieldNames[i] = columnNames[i];
            _fieldNames = fieldNames;
            GenerateLookup();
        }

        public BasicFieldNameLookup(IDataReader reader)
        {
            var length = reader.FieldCount;
            var fieldNames = new string[length];
            for (var i = 0; i < length; ++i)
                fieldNames[i] = reader.GetName(i);
            _fieldNames = fieldNames;
        }

        public int GetOrdinal(string fieldName)
        {
            if (null == fieldName) throw new ArgumentNullException(nameof(fieldName));

            return IndexOf(fieldName) switch
            {
                -1 => throw new IndexOutOfRangeException(fieldName),
                var index => index
            };
        }

        public int IndexOfName(string fieldName)
        {
            if (null == _fieldNameLookup)
                GenerateLookup();

            // via case sensitive search, first match with lowest ordinal matches
            return _fieldNameLookup.TryGetValue(fieldName, out var value) ? value : -1;
        }

        public int IndexOf(string fieldName)
        {
            if (null == _fieldNameLookup)
                GenerateLookup();

            // via case sensitive search, first match with lowest ordinal matches
            if (!_fieldNameLookup.TryGetValue(fieldName, out var index))
            {
                // via case insensitive search, first match with lowest ordinal matches
                index = LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
                if (-1 == index)
                {
                    // do the slow search now (kana, width insensitive comparison)
                    index = LinearIndexOf(fieldName, CompareOptions.IgnoreKanaType
                                                   | CompareOptions.IgnoreWidth
                                                   | CompareOptions.IgnoreCase);
                }
            }

            return index;
        }

        protected virtual CompareInfo GetCompareInfo() => CultureInfo.InvariantCulture.CompareInfo;

        int LinearIndexOf(string fieldName, CompareOptions compareOptions)
        {
            _compareInfo ??= GetCompareInfo();

            var length = _fieldNames.Length;
            for (var i = 0; i < length; ++i)
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
        void GenerateLookup()
        {
            var length = _fieldNames.Length;
            var hash = new Dictionary<string, int>(length);

            // via case sensitive search, first match with lowest ordinal matches
            for (var i = length - 1; 0 <= i; --i)
            {
                var fieldName = _fieldNames[i];
                hash[fieldName] = i;
            }

            _fieldNameLookup = hash;
        }
    }

    // Source: https://github.com/dotnet/runtime/blob/33bedaf3bcc95d91dde5f09251a5972fbac5f05e/src/libraries/System.Data.Common/src/System/Data/Common/FieldNameLookup.cs

    sealed class FieldNameLookup : BasicFieldNameLookup
    {
        readonly int _defaultLocaleId;

        public FieldNameLookup(string[] fieldNames, int defaultLocaleId) : base(fieldNames) =>
            _defaultLocaleId = defaultLocaleId;

        public FieldNameLookup(ReadOnlyCollection<string> columnNames, int defaultLocaleId) :
            base(columnNames) =>
            _defaultLocaleId = defaultLocaleId;

        public FieldNameLookup(IDataReader reader, int defaultLocaleId) : base(reader) =>
            _defaultLocaleId = defaultLocaleId;

        //The compare info is specified by the server by specifying the default LocaleId.
        protected override CompareInfo GetCompareInfo() =>
            -1 != _defaultLocaleId ? CompareInfo.GetCompareInfo(_defaultLocaleId)
                                   : base.GetCompareInfo();
    }
}
