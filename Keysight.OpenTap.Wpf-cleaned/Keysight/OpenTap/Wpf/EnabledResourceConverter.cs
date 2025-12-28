// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.EnabledResourceConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class EnabledResourceConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    switch (value)
    {
      case IEnabledResource resource1:
        return (object) new EnabledResourceViewModel(resource1);
      case IResource resource2:
        return (object) new ResourceViewModel(resource2);
      default:
        return value;
    }
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
