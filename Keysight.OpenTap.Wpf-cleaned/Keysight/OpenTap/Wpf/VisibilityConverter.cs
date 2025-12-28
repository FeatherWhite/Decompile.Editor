// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.VisibilityConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class VisibilityConverter : ConverterBase
{
  protected Visibility getFalseState(object parameter)
  {
    return parameter == null ? Visibility.Collapsed : (Visibility) parameter;
  }

  protected Visibility getTrueState(object parameter)
  {
    return this.getFalseState(parameter) == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
  }

  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (!(value is bool flag))
      flag = !(value is string) ? value != null : !string.IsNullOrEmpty((string) value);
    return (object) (Visibility) (flag ? (int) this.getTrueState(parameter) : (int) this.getFalseState(parameter));
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return (object) ((Visibility) value == this.getTrueState(parameter));
  }
}
