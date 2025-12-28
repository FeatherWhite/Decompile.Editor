// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.OpaqueColorConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class OpaqueColorConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    double num = double.Parse(parameter.ToString(), (IFormatProvider) culture);
    if (value == null)
      return (object) null;
    Color color = ((SolidColorBrush) value).Color;
    color.A = (byte) ((double) color.A * num);
    return (object) new SolidColorBrush(color);
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
