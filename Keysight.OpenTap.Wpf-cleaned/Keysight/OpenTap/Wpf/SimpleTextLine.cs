// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SimpleTextLine
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SimpleTextLine : FrameworkElement
{
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (SimpleTextLine), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
  private Typeface typeface_0;
  private DpiScale dpiScale_0;
  private double double_0;

  public string Text
  {
    get => (string) this.GetValue(SimpleTextLine.TextProperty);
    set => this.SetValue(SimpleTextLine.TextProperty, (object) value);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    if (string.IsNullOrWhiteSpace(this.Text))
      return;
    this.method_1(drawingContext, this.Text);
  }

  private void method_0()
  {
    if (this.typeface_0 != null)
      return;
    FontFamily fontFamily = TextElement.GetFontFamily((DependencyObject) this);
    this.double_0 = TextElement.GetFontSize((DependencyObject) this);
    this.typeface_0 = new Typeface(fontFamily, TextElement.GetFontStyle((DependencyObject) this), TextElement.GetFontWeight((DependencyObject) this), FontStretches.Normal);
    this.dpiScale_0 = VisualTreeHelper.GetDpi((Visual) this);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    this.method_0();
    Brush foreground = TextElement.GetForeground((DependencyObject) this);
    FormattedText formattedText = new FormattedText(this.Text ?? "999ms", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.typeface_0, this.double_0, foreground, (NumberSubstitution) null, TextFormattingMode.Ideal, this.dpiScale_0.PixelsPerDip)
    {
      MaxLineCount = 1,
      Trimming = TextTrimming.None
    };
    return new Size(formattedText.WidthIncludingTrailingWhitespace + 1.0, formattedText.Height);
  }

  private void method_1(DrawingContext drawingContext_0, string string_0)
  {
    Brush foreground = TextElement.GetForeground((DependencyObject) this);
    this.method_0();
    FormattedText formattedText = new FormattedText(string_0, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.typeface_0, this.double_0, foreground, (NumberSubstitution) null, TextFormattingMode.Ideal, this.dpiScale_0.PixelsPerDip)
    {
      MaxLineCount = 1,
      MaxTextWidth = this.ActualWidth,
      Trimming = TextTrimming.CharacterEllipsis
    };
    drawingContext_0.DrawText(formattedText, new Point(0.0, 0.0));
  }
}
