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
    using System.Globalization;

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
}
