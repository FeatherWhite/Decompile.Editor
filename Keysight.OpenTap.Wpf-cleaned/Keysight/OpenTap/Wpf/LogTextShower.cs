// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LogTextShower
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class LogTextShower : FrameworkElement
{
  private readonly int index;
  private readonly LogPanel panel;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (LogMessage), typeof (LogTextShower), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  private LogMessage logMessage_0;
  private static readonly Regex regex_0 = new Regex(" in (.+):line ([0-9]+)", RegexOptions.Compiled);

  public LogTextShower(LogPanel panel, int index)
  {
    this.index = index;
    this.Height = 20.0;
    this.panel = panel;
  }

  private LogMessage method_0()
  {
    int position = (int) this.panel.vertbar.Value + this.index;
    return this.panel.activebuffer.Count <= position ? (LogMessage) null : this.panel.activebuffer[position];
  }

  public LogMessage Text
  {
    get => (LogMessage) this.GetValue(LogTextShower.TextProperty);
    set => this.SetValue(LogTextShower.TextProperty, (object) value);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == UIElement.IsVisibleProperty)
    {
      if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
        RenderDispatch.Rendering += new EventHandler<EventArgs>(this.method_1);
      else
        RenderDispatch.Rendering -= new EventHandler<EventArgs>(this.method_1);
    }
    else
    {
      if (dependencyPropertyChangedEventArgs_0.Property != UIElement.IsMouseOverProperty)
        return;
      this.InvalidateVisual();
    }
  }

  private void method_1(object sender, EventArgs e)
  {
    if (this.logMessage_0 == this.method_0())
      this.Text = this.logMessage_0;
    this.logMessage_0 = this.method_0();
  }

  protected override void OnMouseMove(MouseEventArgs mouseEventArgs_0)
  {
    this.method_2(mouseEventArgs_0);
    base.OnMouseMove(mouseEventArgs_0);
  }

  protected override void OnMouseLeave(MouseEventArgs mouseEventArgs_0)
  {
    base.OnMouseLeave(mouseEventArgs_0);
    this.Cursor = (Cursor) null;
  }

  protected override void OnMouseDown(MouseButtonEventArgs mouseButtonEventArgs_0)
  {
    base.OnMouseDown(mouseButtonEventArgs_0);
    if (mouseButtonEventArgs_0.ChangedButton != MouseButton.Left)
      return;
    string path = this.CheckLink(mouseButtonEventArgs_0.GetPosition((IInputElement) this));
    if (path == null)
      return;
    this.panel.OpenLink(path);
  }

  private void method_2(MouseEventArgs mouseEventArgs_0)
  {
    string str = (string) null;
    if (mouseEventArgs_0 != null)
      str = this.CheckLink(mouseEventArgs_0.GetPosition((IInputElement) this));
    if (str == null)
      this.Cursor = (Cursor) null;
    else
      this.Cursor = Cursors.Hand;
  }

  public string CheckLink(Point point_0)
  {
    if (this.Text == null)
      return (string) null;
    if (this.Text.Message == null || this.Text.Source == null)
      return (string) null;
    double num1 = -this.panel.charwidth * this.panel.horzbar.Value;
    object obj = (object) null;
    string str = this.Text.Message;
    string string_0 = (string) null;
    int num2 = 0;
    if (str.Length < 300)
    {
      Match match = LogTextShower.regex_0.Match(str);
      if (match.Success)
      {
        string_0 = match.Groups[1].Value;
        num2 = int.Parse(match.Groups[2].Value);
        int length = str.IndexOf(" in ");
        str = str.Substring(0, length);
      }
      obj = StringToLogControl.SplitHyperLinks(str);
    }
    double num3 = num1 + this.method_6(this.Text.FormattedTime) + this.panel.charwidth * 2.0 + this.method_6(this.Text.Source) + this.panel.charwidth * (double) (2 + this.panel.MaxSourceWidth - this.Text.Source.Length);
    switch (obj)
    {
      case null:
      case string _:
        num3 += this.method_6(str);
        break;
      case IEnumerable source:
        using (IEnumerator<object> enumerator = source.Cast<object>().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            if (current is string)
            {
              num3 += this.method_6((string) current);
            }
            else
            {
              HyperlinkText hyperlinkText = current as HyperlinkText;
              double num4 = num3;
              num3 += this.method_6(hyperlinkText.Text);
              if (point_0.X >= num4 && point_0.X <= num3)
                return hyperlinkText.Text;
            }
          }
          break;
        }
    }
    if (string_0 != null)
    {
      double num5 = num3 + this.panel.charwidth;
      double num6 = num5;
      double num7 = num5 + this.method_6("line " + num2.ToString()) + this.panel.charwidth + this.method_6(" in ") + this.panel.charwidth + this.method_6(string_0);
      if (point_0.X > num6 && point_0.X < num7)
        return $"vs: {string_0}:{num2.ToString()}";
    }
    return (string) null;
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    // ISSUE: variable of a compiler-generated type
    LogTextShower.\u003C\u003Ec__DisplayClass16_0 cDisplayClass160;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.drawingContext = drawingContext;
    // ISSUE: reference to a compiler-generated field
    base.OnRender(cDisplayClass160.drawingContext);
    if (this.Text == null || this.Text.Message == null)
      return;
    if (this.panel.selectedMessages.Contains(this.Text))
    {
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.drawingContext.DrawRectangle(this.panel.SelectedBrush, new Pen(), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
    }
    else if (this.IsMouseOver)
    {
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.drawingContext.PushOpacity(0.5);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.drawingContext.DrawRectangle(this.panel.SelectedBrush, new Pen(), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.drawingContext.Pop();
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      cDisplayClass160.drawingContext.DrawRectangle((Brush) Brushes.Transparent, new Pen(), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
    }
    Brush brush_0 = this.Text.Level != 10 ? (this.Text.Level != 20 ? (this.Text.Level != 40 ? this.panel.Foreground : this.panel.DebugBrush) : this.panel.WarningBrush) : this.panel.ErrorBrush;
    double num1 = -this.panel.charwidth * this.panel.horzbar.Value;
    if (this.panel.IsTimeSpanEnabled)
    {
      foreach (TimeSlot selectedTimeSlot in (IEnumerable<TimeSlot>) this.panel.SelectedTimeSlots)
      {
        if (this.Text.Time > selectedTimeSlot.Start && this.Text.Time < selectedTimeSlot.Stop)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass160.drawingContext.DrawRectangle(this.panel.TimeSlotBrush, new Pen(), new Rect(num1, 0.0, 3.0, this.ActualHeight));
          break;
        }
      }
      num1 += 3.0;
    }
    object obj = (object) null;
    string str = this.Text.Message.Replace("\r", "").Replace("\n", " ");
    string string_0_1 = (string) null;
    int num2 = 0;
    if (str.Length < 300)
    {
      Match match = LogTextShower.regex_0.Match(str);
      if (match.Success)
      {
        string_0_1 = match.Groups[1].Value;
        num2 = int.Parse(match.Groups[2].Value);
        int length = str.IndexOf(" in ");
        str = str.Substring(0, length);
      }
      obj = StringToLogControl.SplitHyperLinks(str);
    }
    if (this.Text.Source != null)
    {
      string formattedTime = this.Text.FormattedTime;
      // ISSUE: reference to a compiler-generated method
      this.method_7(num1, formattedTime, ref cDisplayClass160);
      // ISSUE: reference to a compiler-generated field
      double num3 = num1 + this.method_4(cDisplayClass160.drawingContext, this.Text.FormattedTime, new Point(num1, 0.0), brush_0) + this.panel.charwidth * 2.0;
      // ISSUE: reference to a compiler-generated method
      this.method_7(num3, this.Text.Source, ref cDisplayClass160);
      // ISSUE: reference to a compiler-generated field
      num1 = num3 + this.method_4(cDisplayClass160.drawingContext, this.Text.Source, new Point(num3, 0.0), brush_0) + this.panel.charwidth * (double) (2 + this.panel.MaxSourceWidth - this.Text.Source.Length);
    }
    switch (obj)
    {
      case null:
      case string _:
        // ISSUE: reference to a compiler-generated method
        this.method_7(num1, str, ref cDisplayClass160);
        // ISSUE: reference to a compiler-generated field
        num1 += this.method_4(cDisplayClass160.drawingContext, str, new Point(num1, 0.0), brush_0);
        break;
      case IEnumerable source:
        using (IEnumerator<object> enumerator = source.Cast<object>().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            if (current is string)
            {
              // ISSUE: reference to a compiler-generated method
              this.method_7(num1, (string) current, ref cDisplayClass160);
              // ISSUE: reference to a compiler-generated field
              num1 += this.method_4(cDisplayClass160.drawingContext, (string) current, new Point(num1, 0.0), brush_0);
            }
            else
            {
              HyperlinkText hyperlinkText = current as HyperlinkText;
              // ISSUE: reference to a compiler-generated method
              this.method_7(num1, hyperlinkText.Text, ref cDisplayClass160);
              // ISSUE: reference to a compiler-generated field
              num1 += this.method_4(cDisplayClass160.drawingContext, hyperlinkText.Text, new Point(num1, 0.0), this.panel.HyperlinkBrush);
            }
          }
          break;
        }
    }
    if (string_0_1 == null)
      return;
    double num4 = num1 + this.panel.charwidth;
    string string_0_2 = "line " + num2.ToString();
    // ISSUE: reference to a compiler-generated method
    this.method_7(num4, string_0_2, ref cDisplayClass160);
    // ISSUE: reference to a compiler-generated field
    double x = num4 + this.method_4(cDisplayClass160.drawingContext, string_0_2, new Point(num4, 0.0), this.panel.HyperlinkBrush) + this.panel.charwidth;
    // ISSUE: reference to a compiler-generated field
    double num5 = x + this.method_4(cDisplayClass160.drawingContext, "in", new Point(x, 0.0), this.panel.HyperlinkBrush) + this.panel.charwidth;
    // ISSUE: reference to a compiler-generated method
    this.method_7(num5, string_0_1, ref cDisplayClass160);
    // ISSUE: reference to a compiler-generated field
    double num6 = num5 + this.method_4(cDisplayClass160.drawingContext, string_0_1, new Point(num5, 0.0), this.panel.HyperlinkBrush);
  }

  private IEnumerable<StringSearch.StringSearchResult> method_3(string string_0)
  {
    string searchText = this.panel.searchGui.SearchText;
    return !string.IsNullOrEmpty(searchText) ? (IEnumerable<StringSearch.StringSearchResult>) StringSearch.StringFind(string_0, searchText, true) : Enumerable.Empty<StringSearch.StringSearchResult>();
  }

  private double method_4(
    DrawingContext drawingContext_0,
    string string_0,
    Point point_0,
    Brush brush_0)
  {
    int startIndex = Math.Max(0, (int) (-point_0.X / this.panel.charwidth));
    int length = (int) Math.Min((double) (string_0.Length - startIndex), this.ActualWidth / this.panel.charwidth);
    if (length <= 0)
      return (double) string_0.Length * this.panel.charwidth;
    point_0.X += (double) startIndex * this.panel.charwidth;
    string_0 = string_0.Substring(startIndex, length);
    FormattedText formattedText = new FormattedText(string_0, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.panel.LogFont, this.panel.LogFontSize, brush_0, (NumberSubstitution) null, TextFormattingMode.Display, this.panel.Dpi.PixelsPerDip);
    formattedText.MaxLineCount = 1;
    drawingContext_0.DrawText(formattedText, point_0);
    return formattedText.WidthIncludingTrailingWhitespace + (double) startIndex * this.panel.charwidth;
  }

  private double method_5(DrawingContext drawingContext_0, Point point_0, int int_0, int int_1)
  {
    if (int_1 - int_0 <= 0)
      return (double) int_1 * this.panel.charwidth;
    drawingContext_0.PushOpacity(0.5);
    drawingContext_0.DrawRectangle(this.panel.HyperlinkBrush, new Pen(), new Rect(new Point(point_0.X + (double) int_0 * this.panel.charwidth, point_0.Y - this.ActualHeight), new Point(point_0.X + (double) int_1 * this.panel.charwidth, point_0.Y)));
    drawingContext_0.Pop();
    return (double) (int_1 - int_0) * this.panel.charwidth;
  }

  private double method_6(string string_0) => (double) string_0.Length * this.panel.charwidth;
}
