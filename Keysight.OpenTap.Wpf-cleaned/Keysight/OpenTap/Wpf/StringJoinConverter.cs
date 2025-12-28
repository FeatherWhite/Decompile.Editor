// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.StringJoinConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class StringJoinConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return (object) string.Join(Environment.NewLine, (IEnumerable<string>) value);
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
