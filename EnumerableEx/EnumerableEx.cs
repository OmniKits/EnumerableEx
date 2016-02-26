﻿using System;
using System.Collections.Generic;

static partial class EnumerableEx
{
    public static IEnumerable<TResult> ZipFull<T1st, T2nd, TResult>(this IEnumerable<T1st> src1st, IEnumerable<T2nd> src2nd, Func<T1st, T2nd, TResult> selector)
    {
        if (src1st == null)
            throw new NullReferenceException();
        if (src2nd == null)
            throw new ArgumentNullException(nameof(src2nd));
        if (selector == null)
            throw new ArgumentNullException(nameof(selector));

        var coll1st = src1st as ICollection<T1st>;
        var coll2nd = src2nd as ICollection<T2nd>;
        if (coll1st != null && coll2nd != null
            && coll1st.Count != coll2nd.Count)
        {
            throw new InvalidOperationException();
        }

        var enmtr1st = src1st.GetEnumerator();
        var enmtr2nd = src2nd.GetEnumerator();
        while (enmtr1st.MoveNext())
        {
            if (!enmtr2nd.MoveNext())
                throw new InvalidOperationException();

            yield return selector(enmtr1st.Current, enmtr2nd.Current);
        }
        if (enmtr2nd.MoveNext())
            throw new InvalidOperationException();
    }

    public static bool SequenceEqual<T>(this IEnumerable<T> self, IEnumerable<T> that, Func<T, T, bool> test)
    {
        var selfCollection = self as ICollection<T>;
        var thatCollection = that as ICollection<T>;
        if (selfCollection != null && thatCollection != null
            && selfCollection.Count != thatCollection.Count)
        {
            return false;
        }

        if (test == null)
            test = EqualityComparer<T>.Default.Equals;

        var selfEnumerator = self.GetEnumerator();
        var thatEnumerator = that.GetEnumerator();
        while (selfEnumerator.MoveNext())
        {
            if (!thatEnumerator.MoveNext())
                return false;

            if (!test(selfEnumerator.Current, thatEnumerator.Current))
                return false;
        }
        return !thatEnumerator.MoveNext();
    }
}