namespace Eggado
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using JetBrains.Annotations;

    #endregion

    static class Expando
    {
        public static dynamic ToExpando(
            [NotNull] this IEnumerable<KeyValuePair<string, object>> pairs)
        {
            return pairs.ToExpando(e => e.Key, e => e.Value);
        }

        public static dynamic ToExpando<T>(
            [NotNull] this IEnumerable<T> source, 
            [NotNull] Func<T, string> keyFunc, 
            [NotNull] Func<T, object> valueFunc)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keyFunc == null) throw new ArgumentNullException("keyFunc");
            if (valueFunc == null) throw new ArgumentNullException("valueFunc");

            var expando = new ExpandoObject();
            var dict = (IDictionary<string, object>)expando;
            foreach (var item in source)
                dict.Add(keyFunc(item), valueFunc(item));
            return expando;
        }
    }
}