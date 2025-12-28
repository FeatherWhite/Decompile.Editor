// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.VisibilityToBoolConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class VisibilityToBoolConverter : ConverterBase
{
  private VisibilityConverter visibilityConverter_0 = new VisibilityConverter();

  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return this.visibilityConverter_0.ConvertBack(value, targetType, parameter, culture);
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return this.visibilityConverter_0.Convert(value, targetType, parameter, culture);
  }
}
