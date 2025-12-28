// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.FlowControl
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class FlowControl : FrameworkElement
{
  public static readonly DependencyProperty FlowProperty = DependencyProperty.Register(nameof (Flow), typeof (FlowViewModel), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DetailBrushProperty = DependencyProperty.Register(nameof (DetailBrush), typeof (Brush), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty DeferBrushProperty = DependencyProperty.Register(nameof (DeferBrush), typeof (Brush), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Yellow, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty PrePlanRunBrushProperty = DependencyProperty.Register(nameof (PrePlanRunBrush), typeof (Brush), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Green, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty PostPlanRunBrushProperty = DependencyProperty.Register(nameof (PostPlanRunBrush), typeof (Brush), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Blue, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty RenderActiveProperty = DependencyProperty.Register(nameof (RenderActive), typeof (bool), typeof (FlowControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  private Pen pen_0;

  public FlowViewModel Flow
  {
    get => (FlowViewModel) this.GetValue(FlowControl.FlowProperty);
    set => this.SetValue(FlowControl.FlowProperty, (object) value);
  }

  public Brush DetailBrush
  {
    get => (Brush) this.GetValue(FlowControl.DetailBrushProperty);
    set => this.SetValue(FlowControl.DetailBrushProperty, (object) value);
  }

  public Brush DeferBrush
  {
    get => (Brush) this.GetValue(FlowControl.DeferBrushProperty);
    set => this.SetValue(FlowControl.DeferBrushProperty, (object) value);
  }

  public Brush PrePlanRunBrush
  {
    get => (Brush) this.GetValue(FlowControl.PrePlanRunBrushProperty);
    set => this.SetValue(FlowControl.PrePlanRunBrushProperty, (object) value);
  }

  public Brush PostPlanRunBrush
  {
    get => (Brush) this.GetValue(FlowControl.PostPlanRunBrushProperty);
    set => this.SetValue(FlowControl.PostPlanRunBrushProperty, (object) value);
  }

  public bool RenderActive
  {
    get => (bool) this.GetValue(FlowControl.RenderActiveProperty);
    set => this.SetValue(FlowControl.RenderActiveProperty, (object) value);
  }

  public FlowControl()
  {
    this.HorizontalAlignment = HorizontalAlignment.Stretch;
    this.Loaded += new RoutedEventHandler(this.FlowControl_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.FlowControl_Unloaded);
  }

  private void FlowControl_Unloaded(object sender, RoutedEventArgs e) => this.method_0();

  private void method_0()
  {
    this.Flow?.PollUpdate();
    this.RenderActive = this.Flow != null && !this.Flow.Ended && this.IsVisible && this.IsLoaded;
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == FlowControl.RenderActiveProperty)
    {
      if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
        RenderDispatch.Rendering += (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_1);
      else
        RenderDispatch.Rendering -= (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_1);
    }
    else
    {
      if (dependencyPropertyChangedEventArgs_0.Property != FlowControl.FlowProperty && dependencyPropertyChangedEventArgs_0.Property != UIElement.IsVisibleProperty)
        return;
      this.method_0();
    }
  }

  private void method_1(object sender, EventArgs e)
  {
    this.method_0();
    this.InvalidateVisual();
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    if (this.Flow == null)
      return;
    double num1;
    TimeSpan timeSpan;
    if (this.Flow.FinalDuration > 0.0)
    {
      num1 = this.Flow.FinalDuration;
    }
    else
    {
      timeSpan = TestPlanGridListener.GetTimeSinceStamp(this.Flow.InitTimeStamp);
      num1 = timeSpan.TotalSeconds;
    }
    double num2 = this.ActualWidth / (num1 * 1.02);
    List<FlowEventType> backBuffer1 = this.Flow.FlowTypes.GetBackBuffer();
    List<long> backBuffer2 = this.Flow.FlowStarts.GetBackBuffer();
    List<long> backBuffer3 = this.Flow.FlowStops.GetBackBuffer();
    try
    {
      for (int index = 0; index < backBuffer3.Count; ++index)
      {
        FlowEventType flowEventType_0 = backBuffer1[index];
        long num3 = backBuffer2[index];
        long endStamp = backBuffer3[index];
        timeSpan = TestPlanGridListener.GetTimeSpanFromStamps(this.Flow.InitTimeStamp, num3);
        double val2 = timeSpan.TotalSeconds * num2;
        timeSpan = TestPlanGridListener.GetTimeSpanFromStamps(num3, endStamp);
        double width = timeSpan.TotalSeconds * num2;
        if (width < 1.0)
        {
          if (flowEventType_0 == FlowEventType.Running)
            width = 1.0;
          else
            continue;
        }
        double x = Math.Min(this.ActualWidth - width, val2);
        drawingContext.DrawRectangle(this.method_2(flowEventType_0), this.pen_0 ?? (this.pen_0 = new Pen()), new Rect(x, 0.0, width, this.ActualHeight));
      }
      if (backBuffer3.Count >= backBuffer2.Count)
        return;
      FlowEventType flowEventType_0_1 = backBuffer1.Last<FlowEventType>();
      long num4 = backBuffer2.Last<long>();
      long timestamp = Stopwatch.GetTimestamp();
      timeSpan = TestPlanGridListener.GetTimeSpanFromStamps(this.Flow.InitTimeStamp, num4);
      double x1 = timeSpan.TotalSeconds * num2;
      timeSpan = TestPlanGridListener.GetTimeSpanFromStamps(num4, timestamp);
      double val1 = timeSpan.TotalSeconds * num2;
      drawingContext.DrawRectangle(this.method_2(flowEventType_0_1), this.pen_0 ?? (this.pen_0 = new Pen()), new Rect(x1, 0.0, Math.Max(val1, 1.0), this.ActualHeight));
    }
    catch
    {
    }
    finally
    {
      this.Flow.FlowTypes.EndBackbuffer();
      this.Flow.FlowStarts.EndBackbuffer();
      this.Flow.FlowStops.EndBackbuffer();
    }
  }

  private Brush method_2(FlowEventType flowEventType_0)
  {
    if (flowEventType_0.HasFlag((Enum) FlowEventType.Running))
      return this.DetailBrush;
    if (flowEventType_0.HasFlag((Enum) FlowEventType.Defer))
      return this.DeferBrush;
    if (flowEventType_0.HasFlag((Enum) FlowEventType.PrePlanRun))
      return this.PrePlanRunBrush;
    return flowEventType_0.HasFlag((Enum) FlowEventType.PostPlanRun) ? this.PostPlanRunBrush : this.DetailBrush;
  }
}
