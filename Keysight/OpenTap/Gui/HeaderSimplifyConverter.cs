// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.HeaderSimplifyConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class HeaderSimplifyConverter : ConverterBase
{
  public virtual object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return (object) ((IEnumerable<string>) ((string) value ?? "").Split('\\')).Last<string>().Trim();
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
