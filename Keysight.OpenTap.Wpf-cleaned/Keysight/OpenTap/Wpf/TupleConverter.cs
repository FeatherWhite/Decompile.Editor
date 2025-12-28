// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TupleConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class TupleConverter : MultiConverterBase
{
  public override object Convert(
    object[] values,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    switch (values.Length)
    {
      case 1:
        return (object) Tuple.Create<object>(values[0]);
      case 2:
        return (object) Tuple.Create<object, object>(values[0], values[1]);
      case 3:
        return (object) Tuple.Create<object, object, object>(values[0], values[1], values[2]);
      default:
        return (object) values;
    }
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
