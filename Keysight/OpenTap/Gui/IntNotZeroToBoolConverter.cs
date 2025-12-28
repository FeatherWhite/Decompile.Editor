// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.IntNotZeroToBoolConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class IntNotZeroToBoolConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ((int) value != 0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
