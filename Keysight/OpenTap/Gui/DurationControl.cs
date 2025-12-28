// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.DurationControl
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class DurationControl : FrameworkElement
{
  public static readonly DependencyProperty ProgressBarBrushProperty = DependencyProperty.Register(nameof (ProgressBarBrush), typeof (Brush), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Blue, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty OvershootBrushProperty = DependencyProperty.Register(nameof (OvershootBrush), typeof (Brush), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty FlowProperty = DependencyProperty.Register(nameof (Flow), typeof (FlowViewModel), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (object), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty Duration2Property = DependencyProperty.Register(nameof (Duration2), typeof (double), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty RenderActiveProperty = DependencyProperty.Register(nameof (RenderActive), typeof (bool), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty ExpectedDurationProperty = DependencyProperty.Register(nameof (ExpectedDuration), typeof (object), typeof (DurationControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Struct6.smethod_0(0.0)));

  public Brush ProgressBarBrush
  {
    get => (Brush) this.GetValue(DurationControl.ProgressBarBrushProperty);
    set => this.SetValue(DurationControl.ProgressBarBrushProperty, (object) value);
  }

  public Brush OvershootBrush
  {
    get => (Brush) this.GetValue(DurationControl.OvershootBrushProperty);
    set => this.SetValue(DurationControl.OvershootBrushProperty, (object) value);
  }

  public FlowViewModel Flow
  {
    get => (FlowViewModel) this.GetValue(DurationControl.FlowProperty);
    set => this.SetValue(DurationControl.FlowProperty, (object) value);
  }

  public object Duration
  {
    get => this.GetValue(DurationControl.DurationProperty);
    set => this.SetValue(DurationControl.DurationProperty, value);
  }

  public double Duration2
  {
    get => (double) this.GetValue(DurationControl.Duration2Property);
    set => this.SetValue(DurationControl.Duration2Property, (object) value);
  }

  public bool RenderActive
  {
    get => (bool) this.GetValue(DurationControl.RenderActiveProperty);
    set => this.SetValue(DurationControl.RenderActiveProperty, (object) value);
  }

  public object ExpectedDuration
  {
    get => this.GetValue(DurationControl.ExpectedDurationProperty);
    set => this.SetValue(DurationControl.ExpectedDurationProperty, value);
  }

  private void method_0()
  {
    this.Flow?.PollUpdate();
    this.RenderActive = this.Flow != null && this.IsVisible && this.IsLoaded;
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == DurationControl.RenderActiveProperty)
    {
      if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
        RenderDispatch.RenderingSlow += (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_2);
      else
        RenderDispatch.RenderingSlow -= (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_2);
    }
    else if (dependencyPropertyChangedEventArgs_0.Property != DurationControl.FlowProperty && dependencyPropertyChangedEventArgs_0.Property != UIElement.IsVisibleProperty)
    {
      DependencyProperty property = dependencyPropertyChangedEventArgs_0.Property;
    }
    else
      this.method_0();
  }

  public DurationControl()
  {
    this.Loaded += new RoutedEventHandler(this.DurationControl_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.DurationControl_Unloaded);
  }

  private void DurationControl_Unloaded(object sender, RoutedEventArgs e)
  {
    this.method_0();
    this.method_1();
  }

  private void method_1()
  {
    if (this.Flow != null)
    {
      double? duration = this.Flow.GetDuration();
      double? nullable = duration;
      double duration2 = this.Duration2;
      if (!(nullable.GetValueOrDefault() == duration2 & nullable.HasValue))
      {
        this.Duration2 = duration.GetValueOrDefault();
        this.Duration = duration.HasValue ? (object) Struct6.smethod_0(this.Duration2) : (object) null;
      }
      nullable = this.Flow.ExpectedDuration;
      if (!nullable.HasValue)
      {
        if (this.ExpectedDuration == null)
          return;
        this.ExpectedDuration = (object) null;
      }
      else
      {
        nullable = this.Flow.ExpectedDuration;
        Struct6 struct6 = Struct6.smethod_0(nullable.Value);
        if (this.ExpectedDuration is Struct6 expectedDuration)
        {
          if (expectedDuration.Equals((object) struct6))
            return;
          this.ExpectedDuration = (object) struct6;
        }
        else
          this.ExpectedDuration = (object) struct6;
      }
    }
    else
    {
      this.Duration2 = 0.0;
      this.Duration = (object) null;
    }
  }

  private void method_2(object sender, EventArgs e)
  {
    this.method_1();
    this.method_0();
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    if (this.Flow != null)
    {
      if (!this.Flow.ExpectedDuration.HasValue)
      {
        if (this.Flow.Started)
          drawingContext.DrawRectangle(this.ProgressBarBrush, new Pen(), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
      }
      else
      {
        double num1 = this.Duration2 / this.Flow.ExpectedDuration.Value;
        if (num1 > 1.0)
        {
          drawingContext.DrawRectangle(this.ProgressBarBrush, new Pen(), new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
          double num2 = 1.0 / num1;
          drawingContext.DrawRectangle(this.OvershootBrush, new Pen(), new Rect(this.ActualWidth * num2, 0.0, 2.0, this.ActualHeight));
        }
        else
          drawingContext.DrawRectangle(this.ProgressBarBrush, new Pen(), new Rect(0.0, 0.0, this.ActualWidth * num1, this.ActualHeight));
      }
    }
    this.method_0();
  }
}
