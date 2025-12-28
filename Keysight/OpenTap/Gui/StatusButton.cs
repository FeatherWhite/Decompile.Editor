// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.StatusButton
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class StatusButton : Button
{
  public static readonly RoutedEvent ActivityEvent = EventManager.RegisterRoutedEvent("Activity", RoutingStrategy.Direct, typeof (RoutedEventHandler), typeof (StatusButton));
  private Stopwatch stopwatch_0 = Stopwatch.StartNew();

  public event RoutedEventHandler Activity
  {
    add => this.AddHandler(StatusButton.ActivityEvent, (Delegate) value);
    remove => this.RemoveHandler(StatusButton.ActivityEvent, (Delegate) value);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != FrameworkElement.DataContextProperty)
      return;
    if (dependencyPropertyChangedEventArgs_0.OldValue is INotifyActivity oldValue)
      oldValue.Activity -= new EventHandler<EventArgs>(this.method_0);
    if (!(dependencyPropertyChangedEventArgs_0.NewValue is INotifyActivity newValue))
      return;
    newValue.Activity += new EventHandler<EventArgs>(this.method_0);
  }

  private void method_0(object sender, EventArgs e)
  {
    if (this.stopwatch_0.ElapsedMilliseconds <= 100L)
      return;
    this.stopwatch_0.Restart();
    GuiHelpers.GuiInvokeAsync((Action) (() => this.RaiseEvent(new RoutedEventArgs(StatusButton.ActivityEvent))));
  }
}
