// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.DisplayNameAttributeConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class DisplayNameAttributeConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (value == null)
      return (object) "";
    return (object) (value as PropertyInfo) != null ? (object) ((MemberInfo) value).GetDisplayAttribute().Name : (object) ((object) (value as Type) == null ? (MemberInfo) value.GetType() : (MemberInfo) (Type) value).GetDisplayAttribute().Name;
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
