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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using ConversionKey = System.Tuple<System.RuntimeTypeHandle, System.RuntimeTypeHandle>;

    #endregion

    // This partial implementation was template-generated:
    // Mon, 30 May 2016 09:27:36 GMT

    partial class ConversionLambda
    {
        private static ConversionKey K<TInput, TOutput>()
        {
            return Tuple.Create(typeof(TInput).TypeHandle, typeof(TOutput).TypeHandle);
        }

        private static readonly IDictionary<ConversionKey, Expression> Expressions = new Dictionary<ConversionKey, Expression>
        { 
            // System.Boolean

            { K<object, bool>(), (Expression<Func<object, bool>>) (v => Convert.ToBoolean(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, bool>(), (Expression<Func<sbyte, bool>>) (v => Convert.ToBoolean(v)) },
            { K<char, bool>(), (Expression<Func<char, bool>>) (v => Convert.ToBoolean(v)) },
            { K<byte, bool>(), (Expression<Func<byte, bool>>) (v => Convert.ToBoolean(v)) },
            { K<short, bool>(), (Expression<Func<short, bool>>) (v => Convert.ToBoolean(v)) },
            { K<ushort, bool>(), (Expression<Func<ushort, bool>>) (v => Convert.ToBoolean(v)) },
            { K<int, bool>(), (Expression<Func<int, bool>>) (v => Convert.ToBoolean(v)) },
            { K<uint, bool>(), (Expression<Func<uint, bool>>) (v => Convert.ToBoolean(v)) },
            { K<long, bool>(), (Expression<Func<long, bool>>) (v => Convert.ToBoolean(v)) },
            { K<ulong, bool>(), (Expression<Func<ulong, bool>>) (v => Convert.ToBoolean(v)) },
            { K<string, bool>(), (Expression<Func<string, bool>>) (v => Convert.ToBoolean(v, CultureInfo.InvariantCulture)) },
            { K<float, bool>(), (Expression<Func<float, bool>>) (v => Convert.ToBoolean(v)) },
            { K<double, bool>(), (Expression<Func<double, bool>>) (v => Convert.ToBoolean(v)) },
            { K<decimal, bool>(), (Expression<Func<decimal, bool>>) (v => Convert.ToBoolean(v)) },
            { K<DateTime, bool>(), (Expression<Func<DateTime, bool>>) (v => Convert.ToBoolean(v)) },
 
            // System.Char

            { K<object, char>(), (Expression<Func<object, char>>) (v => Convert.ToChar(v, CultureInfo.InvariantCulture)) },
            { K<bool, char>(), (Expression<Func<bool, char>>) (v => Convert.ToChar(v)) },
            { K<sbyte, char>(), (Expression<Func<sbyte, char>>) (v => Convert.ToChar(v)) },
            { K<byte, char>(), (Expression<Func<byte, char>>) (v => Convert.ToChar(v)) },
            { K<short, char>(), (Expression<Func<short, char>>) (v => Convert.ToChar(v)) },
            { K<ushort, char>(), (Expression<Func<ushort, char>>) (v => Convert.ToChar(v)) },
            { K<int, char>(), (Expression<Func<int, char>>) (v => Convert.ToChar(v)) },
            { K<uint, char>(), (Expression<Func<uint, char>>) (v => Convert.ToChar(v)) },
            { K<long, char>(), (Expression<Func<long, char>>) (v => Convert.ToChar(v)) },
            { K<ulong, char>(), (Expression<Func<ulong, char>>) (v => Convert.ToChar(v)) },
            { K<string, char>(), (Expression<Func<string, char>>) (v => Convert.ToChar(v, CultureInfo.InvariantCulture)) },
            { K<float, char>(), (Expression<Func<float, char>>) (v => Convert.ToChar(v)) },
            { K<double, char>(), (Expression<Func<double, char>>) (v => Convert.ToChar(v)) },
            { K<decimal, char>(), (Expression<Func<decimal, char>>) (v => Convert.ToChar(v)) },
            { K<DateTime, char>(), (Expression<Func<DateTime, char>>) (v => Convert.ToChar(v)) },
 
            // System.SByte

            { K<object, sbyte>(), (Expression<Func<object, sbyte>>) (v => Convert.ToSByte(v, CultureInfo.InvariantCulture)) },
            { K<bool, sbyte>(), (Expression<Func<bool, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<char, sbyte>(), (Expression<Func<char, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<byte, sbyte>(), (Expression<Func<byte, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<short, sbyte>(), (Expression<Func<short, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<ushort, sbyte>(), (Expression<Func<ushort, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<int, sbyte>(), (Expression<Func<int, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<uint, sbyte>(), (Expression<Func<uint, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<long, sbyte>(), (Expression<Func<long, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<ulong, sbyte>(), (Expression<Func<ulong, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<float, sbyte>(), (Expression<Func<float, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<double, sbyte>(), (Expression<Func<double, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<decimal, sbyte>(), (Expression<Func<decimal, sbyte>>) (v => Convert.ToSByte(v)) },
            { K<string, sbyte>(), (Expression<Func<string, sbyte>>) (v => Convert.ToSByte(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, sbyte>(), (Expression<Func<DateTime, sbyte>>) (v => Convert.ToSByte(v)) },
 
            // System.Byte

            { K<object, byte>(), (Expression<Func<object, byte>>) (v => Convert.ToByte(v, CultureInfo.InvariantCulture)) },
            { K<bool, byte>(), (Expression<Func<bool, byte>>) (v => Convert.ToByte(v)) },
            { K<char, byte>(), (Expression<Func<char, byte>>) (v => Convert.ToByte(v)) },
            { K<sbyte, byte>(), (Expression<Func<sbyte, byte>>) (v => Convert.ToByte(v)) },
            { K<short, byte>(), (Expression<Func<short, byte>>) (v => Convert.ToByte(v)) },
            { K<ushort, byte>(), (Expression<Func<ushort, byte>>) (v => Convert.ToByte(v)) },
            { K<int, byte>(), (Expression<Func<int, byte>>) (v => Convert.ToByte(v)) },
            { K<uint, byte>(), (Expression<Func<uint, byte>>) (v => Convert.ToByte(v)) },
            { K<long, byte>(), (Expression<Func<long, byte>>) (v => Convert.ToByte(v)) },
            { K<ulong, byte>(), (Expression<Func<ulong, byte>>) (v => Convert.ToByte(v)) },
            { K<float, byte>(), (Expression<Func<float, byte>>) (v => Convert.ToByte(v)) },
            { K<double, byte>(), (Expression<Func<double, byte>>) (v => Convert.ToByte(v)) },
            { K<decimal, byte>(), (Expression<Func<decimal, byte>>) (v => Convert.ToByte(v)) },
            { K<string, byte>(), (Expression<Func<string, byte>>) (v => Convert.ToByte(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, byte>(), (Expression<Func<DateTime, byte>>) (v => Convert.ToByte(v)) },
 
            // System.Int16

            { K<object, short>(), (Expression<Func<object, short>>) (v => Convert.ToInt16(v, CultureInfo.InvariantCulture)) },
            { K<bool, short>(), (Expression<Func<bool, short>>) (v => Convert.ToInt16(v)) },
            { K<char, short>(), (Expression<Func<char, short>>) (v => Convert.ToInt16(v)) },
            { K<sbyte, short>(), (Expression<Func<sbyte, short>>) (v => Convert.ToInt16(v)) },
            { K<byte, short>(), (Expression<Func<byte, short>>) (v => Convert.ToInt16(v)) },
            { K<ushort, short>(), (Expression<Func<ushort, short>>) (v => Convert.ToInt16(v)) },
            { K<int, short>(), (Expression<Func<int, short>>) (v => Convert.ToInt16(v)) },
            { K<uint, short>(), (Expression<Func<uint, short>>) (v => Convert.ToInt16(v)) },
            { K<long, short>(), (Expression<Func<long, short>>) (v => Convert.ToInt16(v)) },
            { K<ulong, short>(), (Expression<Func<ulong, short>>) (v => Convert.ToInt16(v)) },
            { K<float, short>(), (Expression<Func<float, short>>) (v => Convert.ToInt16(v)) },
            { K<double, short>(), (Expression<Func<double, short>>) (v => Convert.ToInt16(v)) },
            { K<decimal, short>(), (Expression<Func<decimal, short>>) (v => Convert.ToInt16(v)) },
            { K<string, short>(), (Expression<Func<string, short>>) (v => Convert.ToInt16(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, short>(), (Expression<Func<DateTime, short>>) (v => Convert.ToInt16(v)) },
 
            // System.UInt16

            { K<object, ushort>(), (Expression<Func<object, ushort>>) (v => Convert.ToUInt16(v, CultureInfo.InvariantCulture)) },
            { K<bool, ushort>(), (Expression<Func<bool, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<char, ushort>(), (Expression<Func<char, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<sbyte, ushort>(), (Expression<Func<sbyte, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<byte, ushort>(), (Expression<Func<byte, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<short, ushort>(), (Expression<Func<short, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<int, ushort>(), (Expression<Func<int, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<uint, ushort>(), (Expression<Func<uint, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<long, ushort>(), (Expression<Func<long, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<ulong, ushort>(), (Expression<Func<ulong, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<float, ushort>(), (Expression<Func<float, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<double, ushort>(), (Expression<Func<double, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<decimal, ushort>(), (Expression<Func<decimal, ushort>>) (v => Convert.ToUInt16(v)) },
            { K<string, ushort>(), (Expression<Func<string, ushort>>) (v => Convert.ToUInt16(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, ushort>(), (Expression<Func<DateTime, ushort>>) (v => Convert.ToUInt16(v)) },
 
            // System.Int32

            { K<object, int>(), (Expression<Func<object, int>>) (v => Convert.ToInt32(v, CultureInfo.InvariantCulture)) },
            { K<bool, int>(), (Expression<Func<bool, int>>) (v => Convert.ToInt32(v)) },
            { K<char, int>(), (Expression<Func<char, int>>) (v => Convert.ToInt32(v)) },
            { K<sbyte, int>(), (Expression<Func<sbyte, int>>) (v => Convert.ToInt32(v)) },
            { K<byte, int>(), (Expression<Func<byte, int>>) (v => Convert.ToInt32(v)) },
            { K<short, int>(), (Expression<Func<short, int>>) (v => Convert.ToInt32(v)) },
            { K<ushort, int>(), (Expression<Func<ushort, int>>) (v => Convert.ToInt32(v)) },
            { K<uint, int>(), (Expression<Func<uint, int>>) (v => Convert.ToInt32(v)) },
            { K<long, int>(), (Expression<Func<long, int>>) (v => Convert.ToInt32(v)) },
            { K<ulong, int>(), (Expression<Func<ulong, int>>) (v => Convert.ToInt32(v)) },
            { K<float, int>(), (Expression<Func<float, int>>) (v => Convert.ToInt32(v)) },
            { K<double, int>(), (Expression<Func<double, int>>) (v => Convert.ToInt32(v)) },
            { K<decimal, int>(), (Expression<Func<decimal, int>>) (v => Convert.ToInt32(v)) },
            { K<string, int>(), (Expression<Func<string, int>>) (v => Convert.ToInt32(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, int>(), (Expression<Func<DateTime, int>>) (v => Convert.ToInt32(v)) },
 
            // System.UInt32

            { K<object, uint>(), (Expression<Func<object, uint>>) (v => Convert.ToUInt32(v, CultureInfo.InvariantCulture)) },
            { K<bool, uint>(), (Expression<Func<bool, uint>>) (v => Convert.ToUInt32(v)) },
            { K<char, uint>(), (Expression<Func<char, uint>>) (v => Convert.ToUInt32(v)) },
            { K<sbyte, uint>(), (Expression<Func<sbyte, uint>>) (v => Convert.ToUInt32(v)) },
            { K<byte, uint>(), (Expression<Func<byte, uint>>) (v => Convert.ToUInt32(v)) },
            { K<short, uint>(), (Expression<Func<short, uint>>) (v => Convert.ToUInt32(v)) },
            { K<ushort, uint>(), (Expression<Func<ushort, uint>>) (v => Convert.ToUInt32(v)) },
            { K<int, uint>(), (Expression<Func<int, uint>>) (v => Convert.ToUInt32(v)) },
            { K<long, uint>(), (Expression<Func<long, uint>>) (v => Convert.ToUInt32(v)) },
            { K<ulong, uint>(), (Expression<Func<ulong, uint>>) (v => Convert.ToUInt32(v)) },
            { K<float, uint>(), (Expression<Func<float, uint>>) (v => Convert.ToUInt32(v)) },
            { K<double, uint>(), (Expression<Func<double, uint>>) (v => Convert.ToUInt32(v)) },
            { K<decimal, uint>(), (Expression<Func<decimal, uint>>) (v => Convert.ToUInt32(v)) },
            { K<string, uint>(), (Expression<Func<string, uint>>) (v => Convert.ToUInt32(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, uint>(), (Expression<Func<DateTime, uint>>) (v => Convert.ToUInt32(v)) },
 
            // System.Int64

            { K<object, long>(), (Expression<Func<object, long>>) (v => Convert.ToInt64(v, CultureInfo.InvariantCulture)) },
            { K<bool, long>(), (Expression<Func<bool, long>>) (v => Convert.ToInt64(v)) },
            { K<char, long>(), (Expression<Func<char, long>>) (v => Convert.ToInt64(v)) },
            { K<sbyte, long>(), (Expression<Func<sbyte, long>>) (v => Convert.ToInt64(v)) },
            { K<byte, long>(), (Expression<Func<byte, long>>) (v => Convert.ToInt64(v)) },
            { K<short, long>(), (Expression<Func<short, long>>) (v => Convert.ToInt64(v)) },
            { K<ushort, long>(), (Expression<Func<ushort, long>>) (v => Convert.ToInt64(v)) },
            { K<int, long>(), (Expression<Func<int, long>>) (v => Convert.ToInt64(v)) },
            { K<uint, long>(), (Expression<Func<uint, long>>) (v => Convert.ToInt64(v)) },
            { K<ulong, long>(), (Expression<Func<ulong, long>>) (v => Convert.ToInt64(v)) },
            { K<float, long>(), (Expression<Func<float, long>>) (v => Convert.ToInt64(v)) },
            { K<double, long>(), (Expression<Func<double, long>>) (v => Convert.ToInt64(v)) },
            { K<decimal, long>(), (Expression<Func<decimal, long>>) (v => Convert.ToInt64(v)) },
            { K<string, long>(), (Expression<Func<string, long>>) (v => Convert.ToInt64(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, long>(), (Expression<Func<DateTime, long>>) (v => Convert.ToInt64(v)) },
 
            // System.UInt64

            { K<object, ulong>(), (Expression<Func<object, ulong>>) (v => Convert.ToUInt64(v, CultureInfo.InvariantCulture)) },
            { K<bool, ulong>(), (Expression<Func<bool, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<char, ulong>(), (Expression<Func<char, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<sbyte, ulong>(), (Expression<Func<sbyte, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<byte, ulong>(), (Expression<Func<byte, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<short, ulong>(), (Expression<Func<short, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<ushort, ulong>(), (Expression<Func<ushort, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<int, ulong>(), (Expression<Func<int, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<uint, ulong>(), (Expression<Func<uint, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<long, ulong>(), (Expression<Func<long, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<float, ulong>(), (Expression<Func<float, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<double, ulong>(), (Expression<Func<double, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<decimal, ulong>(), (Expression<Func<decimal, ulong>>) (v => Convert.ToUInt64(v)) },
            { K<string, ulong>(), (Expression<Func<string, ulong>>) (v => Convert.ToUInt64(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, ulong>(), (Expression<Func<DateTime, ulong>>) (v => Convert.ToUInt64(v)) },
 
            // System.Single

            { K<object, float>(), (Expression<Func<object, float>>) (v => Convert.ToSingle(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, float>(), (Expression<Func<sbyte, float>>) (v => Convert.ToSingle(v)) },
            { K<byte, float>(), (Expression<Func<byte, float>>) (v => Convert.ToSingle(v)) },
            { K<char, float>(), (Expression<Func<char, float>>) (v => Convert.ToSingle(v)) },
            { K<short, float>(), (Expression<Func<short, float>>) (v => Convert.ToSingle(v)) },
            { K<ushort, float>(), (Expression<Func<ushort, float>>) (v => Convert.ToSingle(v)) },
            { K<int, float>(), (Expression<Func<int, float>>) (v => Convert.ToSingle(v)) },
            { K<uint, float>(), (Expression<Func<uint, float>>) (v => Convert.ToSingle(v)) },
            { K<long, float>(), (Expression<Func<long, float>>) (v => Convert.ToSingle(v)) },
            { K<ulong, float>(), (Expression<Func<ulong, float>>) (v => Convert.ToSingle(v)) },
            { K<double, float>(), (Expression<Func<double, float>>) (v => Convert.ToSingle(v)) },
            { K<decimal, float>(), (Expression<Func<decimal, float>>) (v => Convert.ToSingle(v)) },
            { K<string, float>(), (Expression<Func<string, float>>) (v => Convert.ToSingle(v, CultureInfo.InvariantCulture)) },
            { K<bool, float>(), (Expression<Func<bool, float>>) (v => Convert.ToSingle(v)) },
            { K<DateTime, float>(), (Expression<Func<DateTime, float>>) (v => Convert.ToSingle(v)) },
 
            // System.Double

            { K<object, double>(), (Expression<Func<object, double>>) (v => Convert.ToDouble(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, double>(), (Expression<Func<sbyte, double>>) (v => Convert.ToDouble(v)) },
            { K<byte, double>(), (Expression<Func<byte, double>>) (v => Convert.ToDouble(v)) },
            { K<short, double>(), (Expression<Func<short, double>>) (v => Convert.ToDouble(v)) },
            { K<char, double>(), (Expression<Func<char, double>>) (v => Convert.ToDouble(v)) },
            { K<ushort, double>(), (Expression<Func<ushort, double>>) (v => Convert.ToDouble(v)) },
            { K<int, double>(), (Expression<Func<int, double>>) (v => Convert.ToDouble(v)) },
            { K<uint, double>(), (Expression<Func<uint, double>>) (v => Convert.ToDouble(v)) },
            { K<long, double>(), (Expression<Func<long, double>>) (v => Convert.ToDouble(v)) },
            { K<ulong, double>(), (Expression<Func<ulong, double>>) (v => Convert.ToDouble(v)) },
            { K<float, double>(), (Expression<Func<float, double>>) (v => Convert.ToDouble(v)) },
            { K<decimal, double>(), (Expression<Func<decimal, double>>) (v => Convert.ToDouble(v)) },
            { K<string, double>(), (Expression<Func<string, double>>) (v => Convert.ToDouble(v, CultureInfo.InvariantCulture)) },
            { K<bool, double>(), (Expression<Func<bool, double>>) (v => Convert.ToDouble(v)) },
            { K<DateTime, double>(), (Expression<Func<DateTime, double>>) (v => Convert.ToDouble(v)) },
 
            // System.Decimal

            { K<object, decimal>(), (Expression<Func<object, decimal>>) (v => Convert.ToDecimal(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, decimal>(), (Expression<Func<sbyte, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<byte, decimal>(), (Expression<Func<byte, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<char, decimal>(), (Expression<Func<char, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<short, decimal>(), (Expression<Func<short, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<ushort, decimal>(), (Expression<Func<ushort, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<int, decimal>(), (Expression<Func<int, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<uint, decimal>(), (Expression<Func<uint, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<long, decimal>(), (Expression<Func<long, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<ulong, decimal>(), (Expression<Func<ulong, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<float, decimal>(), (Expression<Func<float, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<double, decimal>(), (Expression<Func<double, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<string, decimal>(), (Expression<Func<string, decimal>>) (v => Convert.ToDecimal(v, CultureInfo.InvariantCulture)) },
            { K<bool, decimal>(), (Expression<Func<bool, decimal>>) (v => Convert.ToDecimal(v)) },
            { K<DateTime, decimal>(), (Expression<Func<DateTime, decimal>>) (v => Convert.ToDecimal(v)) },
 
            // System.DateTime

            { K<object, DateTime>(), (Expression<Func<object, DateTime>>) (v => Convert.ToDateTime(v, CultureInfo.InvariantCulture)) },
            { K<string, DateTime>(), (Expression<Func<string, DateTime>>) (v => Convert.ToDateTime(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, DateTime>(), (Expression<Func<sbyte, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<byte, DateTime>(), (Expression<Func<byte, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<short, DateTime>(), (Expression<Func<short, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<ushort, DateTime>(), (Expression<Func<ushort, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<int, DateTime>(), (Expression<Func<int, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<uint, DateTime>(), (Expression<Func<uint, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<long, DateTime>(), (Expression<Func<long, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<ulong, DateTime>(), (Expression<Func<ulong, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<bool, DateTime>(), (Expression<Func<bool, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<char, DateTime>(), (Expression<Func<char, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<float, DateTime>(), (Expression<Func<float, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<double, DateTime>(), (Expression<Func<double, DateTime>>) (v => Convert.ToDateTime(v)) },
            { K<decimal, DateTime>(), (Expression<Func<decimal, DateTime>>) (v => Convert.ToDateTime(v)) },
 
            // System.String

            { K<object, string>(), (Expression<Func<object, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<bool, string>(), (Expression<Func<bool, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<char, string>(), (Expression<Func<char, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<sbyte, string>(), (Expression<Func<sbyte, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<byte, string>(), (Expression<Func<byte, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<short, string>(), (Expression<Func<short, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<ushort, string>(), (Expression<Func<ushort, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<int, string>(), (Expression<Func<int, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<uint, string>(), (Expression<Func<uint, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<long, string>(), (Expression<Func<long, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<ulong, string>(), (Expression<Func<ulong, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<float, string>(), (Expression<Func<float, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<double, string>(), (Expression<Func<double, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<decimal, string>(), (Expression<Func<decimal, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
            { K<DateTime, string>(), (Expression<Func<DateTime, string>>) (v => Convert.ToString(v, CultureInfo.InvariantCulture)) },
        };
    }
}
