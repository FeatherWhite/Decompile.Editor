// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MultiplyConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MultiplyConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return System.Convert.ChangeType((object) ((double) System.Convert.ChangeType(value, typeof (double)) * (double) System.Convert.ChangeType(parameter, typeof (double))), targetType);
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
