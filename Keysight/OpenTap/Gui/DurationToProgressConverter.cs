// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.DurationToProgressConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class DurationToProgressConverter : IMultiValueConverter
{
  public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value[0] == DependencyProperty.UnsetValue)
      return (object) 0.0;
    double num1;
    double num2;
    bool flag;
    try
    {
      num1 = System.Convert.ToDouble(value[0]);
      num2 = System.Convert.ToDouble(value[1]);
      flag = (bool) value[2];
    }
    catch (Exception ex)
    {
      return (object) 0.0;
    }
    if (!flag)
      return (object) 1.0;
    if (num2 == 0.0)
      return (object) 0.0;
    return num2 - num1 > 0.0 && num1 > 0.0 ? (object) (num1 / num2) : (object) 1.0;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
