// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ColumnDisplayNameConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ColumnDisplayNameConverter : ConverterBase
{
  public static string GetColumnName(IMemberData member)
  {
    ColumnDisplayNameAttribute displayNameAttribute = ReflectionDataExtensions.GetAttribute<ColumnDisplayNameAttribute>((IReflectionData) member) ?? new ColumnDisplayNameAttribute((string) null, 0.0, false);
    return displayNameAttribute.ColumnName != null ? displayNameAttribute.ColumnName : ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) member).GetFullName();
  }

  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    parameter = parameter ?? (object) "";
    string columnName = ColumnDisplayNameConverter.GetColumnName((IMemberData) value);
    if (!((string) parameter).Contains("compact"))
      return (object) columnName;
    return (object) ((IEnumerable<string>) columnName.Split('\\')).Last<string>().TrimStart();
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
