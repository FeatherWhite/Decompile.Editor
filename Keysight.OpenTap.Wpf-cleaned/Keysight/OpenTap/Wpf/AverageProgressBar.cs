// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AverageProgressBar
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AverageProgressBar : Control
{
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (AverageProgressBar), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(AverageProgressBar.smethod_0)));
  public static readonly DependencyProperty AverageProperty = DependencyProperty.Register(nameof (Average), typeof (double?), typeof (AverageProgressBar), (PropertyMetadata) new UIPropertyMetadata((object) 1.0, new PropertyChangedCallback(AverageProgressBar.smethod_1)));
  public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (AverageProgressBar), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(AverageProgressBar.smethod_2)));
  private FrameworkElement frameworkElement_0;
  private FrameworkElement frameworkElement_1;
  private FrameworkElement frameworkElement_2;
  private FrameworkElement frameworkElement_3;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is AverageProgressBar averageProgressBar))
      return;
    averageProgressBar.OnValueChanged((double) dependencyPropertyChangedEventArgs_0.OldValue, (double) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  public double Value
  {
    get => (double) this.GetValue(AverageProgressBar.ValueProperty);
    set => this.SetValue(AverageProgressBar.ValueProperty, (object) value);
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is AverageProgressBar averageProgressBar))
      return;
    double? nullable = (double?) dependencyPropertyChangedEventArgs_0.OldValue;
    double valueOrDefault1 = nullable.GetValueOrDefault();
    nullable = (double?) dependencyPropertyChangedEventArgs_0.NewValue;
    double valueOrDefault2 = nullable.GetValueOrDefault();
    averageProgressBar.OnAverageChanged(valueOrDefault1, valueOrDefault2);
  }

  public double? Average
  {
    get => (double?) this.GetValue(AverageProgressBar.AverageProperty);
    set => this.SetValue(AverageProgressBar.AverageProperty, (object) value);
  }

  private static void smethod_2(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is AverageProgressBar averageProgressBar))
      return;
    averageProgressBar.OnIsIndeterminateChanged((bool) dependencyPropertyChangedEventArgs_0.OldValue, (bool) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  public bool IsIndeterminate
  {
    get => (bool) this.GetValue(AverageProgressBar.IsIndeterminateProperty);
    set => this.SetValue(AverageProgressBar.IsIndeterminateProperty, (object) value);
  }

  static AverageProgressBar()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AverageProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AverageProgressBar)));
  }

  public AverageProgressBar()
  {
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.AverageProgressBar_IsVisibleChanged);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.frameworkElement_0 = this.GetTemplateChild("PART_Track") as FrameworkElement;
    this.frameworkElement_1 = this.GetTemplateChild("PART_Average") as FrameworkElement;
    this.frameworkElement_2 = this.GetTemplateChild("PART_Indicator") as FrameworkElement;
    this.frameworkElement_3 = this.GetTemplateChild("PART_GlowRect") as FrameworkElement;
    this.ClipToBounds = true;
  }

  private void method_0()
  {
    if (this.frameworkElement_0 == null || this.frameworkElement_2 == null)
      return;
    double num1 = this.Average.GetValueOrDefault();
    double num2 = this.Value;
    if (num1 < 0.0)
      num1 = num2 != 0.0 ? 0.0 : 0.1;
    if (num2 >= num1)
    {
      double d = num1 / num2 * this.frameworkElement_0.ActualWidth;
      if (double.IsNaN(d))
        d = 0.0;
      FrameworkElement frameworkElement1 = this.frameworkElement_1;
      double left = this.frameworkElement_0.Margin.Left + d;
      Thickness margin = this.frameworkElement_1.Margin;
      double top = margin.Top;
      margin = this.frameworkElement_1.Margin;
      double bottom = margin.Bottom;
      Thickness thickness = new Thickness(left, top, 0.0, bottom);
      frameworkElement1.Margin = thickness;
      this.frameworkElement_2.Width = this.frameworkElement_0.ActualWidth;
    }
    else
    {
      this.frameworkElement_1.Margin = new Thickness(this.frameworkElement_0.Margin.Left + this.frameworkElement_0.ActualWidth - 1.0, this.frameworkElement_1.Margin.Top, 0.0, this.frameworkElement_1.Margin.Bottom);
      this.frameworkElement_2.Width = (this.IsIndeterminate ? 1.0 : num2 / num1) * this.frameworkElement_0.ActualWidth;
    }
  }

  protected virtual void OnAverageChanged(double oldValue, double newValue)
  {
    this.method_0();
    this.method_1();
  }

  protected virtual void OnIsIndeterminateChanged(bool oldValue, bool newValue) => this.method_1();

  protected virtual void OnValueChanged(double oldValue, double newValue) => this.method_0();

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.method_0();
    this.method_1();
  }

  private void AverageProgressBar_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    GuiHelpers.GuiInvokeAsync(new Action(this.method_1), priority: DispatcherPriority.Render);
  }

  private void method_1()
  {
    if (this.frameworkElement_3 == null)
      return;
    if (this.IsVisible && this.frameworkElement_3.Width > 0.0 && this.frameworkElement_2.Width > 0.0)
    {
      double left1 = this.frameworkElement_2.Width + this.frameworkElement_3.Width;
      double left2 = -1.0 * this.frameworkElement_3.Width;
      TimeSpan timeSpan1 = TimeSpan.FromSeconds((double) (int) (left1 - left2) / 200.0);
      TimeSpan timeSpan2 = TimeSpan.FromSeconds(1.0);
      Thickness margin = this.frameworkElement_3.Margin;
      TimeSpan timeSpan3;
      if (margin.Left > left2)
      {
        margin = this.frameworkElement_3.Margin;
        if (margin.Left < left1 - 1.0)
        {
          margin = this.frameworkElement_3.Margin;
          timeSpan3 = TimeSpan.FromSeconds(-1.0 * (margin.Left - left2) / 200.0);
          goto label_6;
        }
      }
      timeSpan3 = TimeSpan.Zero;
label_6:
      ThicknessAnimationUsingKeyFrames animation = new ThicknessAnimationUsingKeyFrames();
      animation.BeginTime = new TimeSpan?(timeSpan3);
      animation.Duration = new Duration(timeSpan1 + timeSpan2);
      animation.RepeatBehavior = RepeatBehavior.Forever;
      animation.KeyFrames.Add((ThicknessKeyFrame) new LinearThicknessKeyFrame(new Thickness(left2, 0.0, 0.0, 0.0), (KeyTime) TimeSpan.FromSeconds(0.0)));
      animation.KeyFrames.Add((ThicknessKeyFrame) new LinearThicknessKeyFrame(new Thickness(left1, 0.0, 0.0, 0.0), (KeyTime) timeSpan1));
      this.frameworkElement_3.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) animation);
    }
    else
      this.frameworkElement_3.BeginAnimation(FrameworkElement.MarginProperty, (AnimationTimeline) null);
  }
}
