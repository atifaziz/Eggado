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
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;
    using Mannex.Collections.Generic;

    #endregion

    static partial class ConversionLambda
    {
        public static Expression<Func<TInput, TOutput>> Find<TInput, TOutput>() =>
            (Expression<Func<TInput, TOutput>>) Find(typeof(TInput), typeof(TOutput));

        public static Expression Find([NotNull] Type input, [NotNull] Type output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            return Find(input.TypeHandle, output.TypeHandle);
        }

        public static Expression Find(RuntimeTypeHandle input, RuntimeTypeHandle output) =>
            Expressions.Find(Tuple.Create(input, output));

        /// <summary>
        /// Generates an expression that calls <see cref="Convert.ChangeType(object,System.Type,System.IFormatProvider)"/>.
        /// </summary>

        public static Expression ChangeType(Expression value, Type targetType) =>
            Expression.Convert(
                Expression.Call(
                    ChangeTypeMethod,
                    Expression.Convert(value, typeof(object)),
                    Expression.Constant(targetType),
                    Culture),
                targetType);

        static readonly Expression Culture = ((Expression<Func<IFormatProvider>>) (() => CultureInfo.InvariantCulture)).Body;
        static readonly MethodInfo ChangeTypeMethod = ((MethodCallExpression) (((Expression<Func<object, object>>) (_ => Convert.ChangeType(_, typeof(object), null))).Body)).Method;

        public static Expression<Func<object, T>> ChangeType<T>() =>
            value => (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }
}
