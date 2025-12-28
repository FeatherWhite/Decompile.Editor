// Decompiled with JetBrains decompiler
// Type: OpenTap.Sequence
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap;

internal static class Sequence
{
  public static List<T> DistinctLast<T>(this IEnumerable<T> items)
  {
    Dictionary<T, int> source = new Dictionary<T, int>();
    int num = 0;
    foreach (T key in items)
    {
      source[key] = num;
      ++num;
    }
    return source.OrderBy<KeyValuePair<T, int>, int>((Func<KeyValuePair<T, int>, int>) (keyValuePair_0 => keyValuePair_0.Value)).Select<KeyValuePair<T, int>, T>((Func<KeyValuePair<T, int>, T>) (keyValuePair_0 => keyValuePair_0.Key)).ToList<T>();
  }

  public static void SortBy<T, K>(this IList<T> ilist_0, Func<T, K> keySelector) where K : IComparable<K>
  {
    T[] array = ilist_0.OrderBy<T, K>(keySelector).ToArray<T>();
    if (ilist_0 is T[] objArray)
    {
      array.CopyTo((Array) objArray, 0);
    }
    else
    {
      for (int index = 0; index < array.Length; ++index)
        ilist_0[index] = array[index];
    }
  }

  public static bool TryFind<T>(this IEnumerable<T> items, Func<T, bool> predicate, out T match)
  {
    foreach (T obj in items)
    {
      if (predicate(obj))
      {
        match = obj;
        return true;
      }
    }
    match = default (T);
    return false;
  }
}
