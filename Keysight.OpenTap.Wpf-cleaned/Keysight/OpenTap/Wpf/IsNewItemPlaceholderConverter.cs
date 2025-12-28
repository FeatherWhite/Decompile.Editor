// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.IsNewItemPlaceholderConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class IsNewItemPlaceholderConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) IsNewItemPlaceholderConverter.IsNewItem(value);
  }

  public static bool IsNewItem(object value) => value?.GetType()?.Name == "NamedObject";

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
