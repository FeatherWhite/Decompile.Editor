// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AndConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AndConverter : MultiConverterBase
{
  public override object Convert(
    object[] values,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (values == null)
      return (object) false;
    if (values.Length < 1)
      return (object) false;
    return !((IEnumerable<object>) values).All<object>((Func<object, bool>) (object_0 => object_0 is bool)) ? (object) false : (object) ((IEnumerable<object>) values).All<object>((Func<object, bool>) (object_0 => (bool) object_0));
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
