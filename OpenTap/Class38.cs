// Decompiled with JetBrains decompiler
// Type: OpenTap.Class38
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap;

internal static class Class38
{
  public static List<T> smethod_0<T>(this IEnumerable<T> ienumerable_0)
  {
    Dictionary<T, int> source = new Dictionary<T, int>();
    int num = 0;
    foreach (T key in ienumerable_0)
    {
      source[key] = num;
      ++num;
    }
    return source.OrderBy<KeyValuePair<T, int>, int>((Func<KeyValuePair<T, int>, int>) (keyValuePair_0 => keyValuePair_0.Value)).Select<KeyValuePair<T, int>, T>((Func<KeyValuePair<T, int>, T>) (keyValuePair_0 => keyValuePair_0.Key)).ToList<T>();
  }

  public static void smethod_1<T, U>(this IList<T> ilist_0, Func<T, U> func_0) where U : IComparable<U>
  {
    T[] array = ilist_0.OrderBy<T, U>(func_0).ToArray<T>();
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

  public static bool smethod_2<T>(
    this IEnumerable<T> ienumerable_0,
    Func<T, bool> func_0,
    out T gparam_0)
  {
    foreach (T obj in ienumerable_0)
    {
      if (func_0(obj))
      {
        gparam_0 = obj;
        return true;
      }
    }
    gparam_0 = default (T);
    return false;
  }
}
