// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ComponentSettingsNameConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ComponentSettingsNameConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return !(value is ComponentSettings componentSettings) ? (object) null : (object) componentSettings.GetType().GetDisplayAttribute().GetFullName();
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
