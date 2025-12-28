// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.EnumerableComparer`1
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class EnumerableComparer<T> : IComparer<IEnumerable<T>>
{
  private IComparer<T> icomparer_0;

  public EnumerableComparer() => this.icomparer_0 = (IComparer<T>) Comparer<T>.Default;

  public EnumerableComparer(IComparer<T> comparer) => this.icomparer_0 = comparer;

  public int Compare(IEnumerable<T> ienumerable_0, IEnumerable<T> ienumerable_1)
  {
    using (IEnumerator<T> enumerator1 = ienumerable_0.GetEnumerator())
    {
      using (IEnumerator<T> enumerator2 = ienumerable_1.GetEnumerator())
      {
        int num;
        do
        {
          bool flag1 = enumerator1.MoveNext();
          bool flag2 = enumerator2.MoveNext();
          if (flag1 | flag2)
          {
            if (flag1)
            {
              if (flag2)
                num = this.icomparer_0.Compare(enumerator1.Current, enumerator2.Current);
              else
                goto label_8;
            }
            else
              goto label_7;
          }
          else
            goto label_6;
        }
        while (num == 0);
        goto label_9;
label_6:
        return 0;
label_7:
        return -1;
label_8:
        return 1;
label_9:
        return num;
      }
    }
  }

  public static IEnumerable<string> NaturalSort(IEnumerable<string> strings)
  {
    Func<string, object> convert = (Func<string, object>) (string_0 =>
    {
      try
      {
        return (object) int.Parse(string_0);
      }
      catch
      {
        return (object) string_0;
      }
    });
    return (IEnumerable<string>) strings.OrderBy<string, IEnumerable<object>>((Func<string, IEnumerable<object>>) (string_0 => ((IEnumerable<string>) Regex.Split(string_0.Replace(" ", ""), "([0-9]+)")).Select<string, object>(convert)), (IComparer<IEnumerable<object>>) new EnumerableComparer<object>());
  }
}
