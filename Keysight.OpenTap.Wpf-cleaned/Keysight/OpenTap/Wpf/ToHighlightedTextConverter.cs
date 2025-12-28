// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ToHighlightedTextConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class ToHighlightedTextConverter : ConverterBase
{
  public readonly string DefaultText;
  public readonly Func<object, string> textConverter = new Func<object, string>(ToHighlightedTextConverter.smethod_0);

  public bool Centered { get; set; }

  private static string smethod_0(object object_0) => object_0.ToString();

  public ToHighlightedTextConverter(string defaultText) => this.DefaultText = defaultText;

  public ToHighlightedTextConverter(string defaultText, Func<object, string> formatter)
  {
    this.DefaultText = defaultText;
    this.textConverter = formatter;
  }

  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (value == null)
      return (object) new HighlightedText()
      {
        Text = this.DefaultText,
        Centered = this.Centered,
        IsDefault = true
      };
    string str = this.textConverter(value) ?? this.DefaultText;
    return (object) new HighlightedText()
    {
      Text = str,
      Centered = this.Centered,
      IsDefault = (str == this.DefaultText)
    };
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    return DependencyProperty.UnsetValue;
  }
}
