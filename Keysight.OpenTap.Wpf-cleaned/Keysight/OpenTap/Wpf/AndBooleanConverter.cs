// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AndBooleanConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AndBooleanConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    bool flag1 = true;
    foreach (bool flag2 in values)
      flag1 &= flag2;
    return (object) flag1;
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
