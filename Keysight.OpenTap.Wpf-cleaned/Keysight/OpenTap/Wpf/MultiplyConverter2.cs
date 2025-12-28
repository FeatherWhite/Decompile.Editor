// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MultiplyConverter2
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MultiplyConverter2 : MultiConverterBase
{
  public override object Convert(
    object[] values,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    double num1 = 1.0;
    foreach (object obj in values)
    {
      if (obj != null)
      {
        double num2 = (double) System.Convert.ChangeType(obj, typeof (double));
        num1 *= num2;
      }
    }
    return (object) num1;
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
