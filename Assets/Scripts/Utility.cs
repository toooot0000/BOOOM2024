using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility{
    /// <summary>
    /// equivalent to [from, to], both inclusive
    /// </summary>
    /// <param name="from">Inclusive! </param>
    /// <param name="to"> Inclusive! </param>
    /// <returns></returns>
    public static IEnumerable<int> To(this int from, int to){
        if (from < to){
            while (from <= to){
                yield return from++;
            }
        } else{
            while (from >= to){
                yield return from--;
            }
        }
    }

    public static IEnumerable<Vector2Int> Offset(this IEnumerable<Vector2Int> ori, Vector2Int offset){
        return ori.Select(p => p + offset);
    }

    public static IEnumerable<(int, T)> Enumerated<T>(this IEnumerable<T> ori){
        return ori.Select(((arg1, i) => (i, arg1)));
    }
}