// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.DurationToProgressStateConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shell;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class DurationToProgressStateConverter : IMultiValueConverter
{
  public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value[0] == DependencyProperty.UnsetValue)
      return (object) TaskbarItemProgressState.None;
    bool flag = (bool) value[2];
    double num1;
    try
    {
      num1 = System.Convert.ToDouble(value[0]);
    }
    catch
    {
      return (object) TaskbarItemProgressState.None;
    }
    double num2 = System.Convert.ToDouble(value[1]);
    if (flag)
    {
      if (num2 == 0.0)
        return (object) TaskbarItemProgressState.Indeterminate;
      return num2 - num1 > 0.0 ? (object) TaskbarItemProgressState.Normal : (object) TaskbarItemProgressState.Indeterminate;
    }
    if (value[3] is Verdict verdict)
    {
      switch (verdict)
      {
        case Verdict.Pass:
          return (object) TaskbarItemProgressState.Normal;
        case Verdict.Inconclusive:
          return (object) TaskbarItemProgressState.Paused;
        case Verdict.Fail:
        case Verdict.Aborted:
        case Verdict.Error:
          return (object) TaskbarItemProgressState.Error;
      }
    }
    return (object) TaskbarItemProgressState.None;
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
