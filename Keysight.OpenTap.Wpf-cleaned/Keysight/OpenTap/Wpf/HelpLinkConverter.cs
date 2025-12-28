// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.HelpLinkConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class HelpLinkConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    Type member = value as Type;
    if ((object) member == null)
    {
      if (value == null)
      {
        member = (Type) null;
      }
      else
      {
        Type type = value.GetType();
        if ((object) type == null)
        {
          member = type;
        }
        else
        {
          member = type;
          goto label_7;
        }
      }
    }
    else if ((object) member != null)
      goto label_7;
    return (object) null;
label_7:
    return (object) member.GetHelpLink();
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
