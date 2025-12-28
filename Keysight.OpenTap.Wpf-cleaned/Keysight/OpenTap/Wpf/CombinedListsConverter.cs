// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CombinedListsConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class CombinedListsConverter : MultiConverterBase
{
  public override object Convert(
    object[] values,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return (object) values.OfType<object>().SelectMany<object, object>((Func<object, IEnumerable<object>>) (object_0 =>
    {
      if (object_0 is IEnumerable source2)
        return source2.Cast<object>();
      return (IEnumerable<object>) new object[1]{ object_0 };
    })).ToArray<object>();
  }

  public override object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
