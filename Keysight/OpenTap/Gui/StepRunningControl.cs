// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.StepRunningControl
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class StepRunningControl : FrameworkElement
{
  public static readonly DependencyProperty FlowProperty = DependencyProperty.Register(nameof (Flow), typeof (FlowViewModel), typeof (StepRunningControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty RenderActiveProperty = DependencyProperty.Register(nameof (RenderActive), typeof (bool), typeof (StepRunningControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(nameof (IsRunning), typeof (bool), typeof (StepRunningControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  private static int int_0 = 0;

  public FlowViewModel Flow
  {
    get => (FlowViewModel) this.GetValue(StepRunningControl.FlowProperty);
    set => this.SetValue(StepRunningControl.FlowProperty, (object) value);
  }

  public bool RenderActive
  {
    get => (bool) this.GetValue(StepRunningControl.RenderActiveProperty);
    set => this.SetValue(StepRunningControl.RenderActiveProperty, (object) value);
  }

  public bool IsRunning
  {
    get => (bool) this.GetValue(StepRunningControl.IsRunningProperty);
    set => this.SetValue(StepRunningControl.IsRunningProperty, (object) value);
  }

  public StepRunningControl()
  {
    this.Loaded += new RoutedEventHandler(this.StepRunningControl_Unloaded);
    this.Unloaded += new RoutedEventHandler(this.StepRunningControl_Unloaded);
  }

  private void StepRunningControl_Unloaded(object sender, RoutedEventArgs e) => this.method_0();

  private void method_0()
  {
    this.Flow?.PollUpdate();
    FlowViewModel flow = this.Flow;
    this.IsRunning = flow != null && flow.IsRunning;
    this.RenderActive = this.Flow != null && !this.Flow.Ended && this.IsVisible && this.IsLoaded;
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == StepRunningControl.RenderActiveProperty)
    {
      if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
      {
        ++StepRunningControl.int_0;
        RenderDispatch.Rendering += (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_1);
      }
      else
      {
        --StepRunningControl.int_0;
        RenderDispatch.Rendering -= (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_1);
      }
    }
    else
    {
      if (dependencyPropertyChangedEventArgs_0.Property != StepRunningControl.FlowProperty && dependencyPropertyChangedEventArgs_0.Property != UIElement.IsVisibleProperty)
        return;
      this.method_0();
    }
  }

  private void method_1(object sender, EventArgs e) => this.method_0();
}
