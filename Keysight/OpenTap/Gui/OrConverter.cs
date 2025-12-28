// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.OrConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class OrConverter : MultiConverterBase
{
  public virtual object Convert(
    object[] values,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (values.Length == 0)
      return (object) false;
    return values[0] == DependencyProperty.UnsetValue ? values[0] : (object) Enumerable.Cast<bool>(values).Any<bool>((Func<bool, bool>) (bool_0 => bool_0));
  }

  public virtual object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
