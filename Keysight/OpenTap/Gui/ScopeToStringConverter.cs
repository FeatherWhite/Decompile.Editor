// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ScopeToStringConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ScopeToStringConverter : ConverterBase
{
  public virtual object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    switch (value)
    {
      case TestPlan _:
        return (object) "Test Plan (As External Parameter)";
      case ITestStep step:
        return (object) step.GetFormattedName();
      default:
        throw new Exception("invalid scope");
    }
  }

  public virtual object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
