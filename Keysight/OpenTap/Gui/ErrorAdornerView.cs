// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ErrorAdornerView
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ErrorAdornerView : UserControl, IComponentConnector
{
  internal Decorator decorator_0;
  internal Decorator decorator_1;
  internal Rectangle rectangle_0;
  private bool bool_0;

  public ErrorAdornerView()
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.ErrorAdornerView_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ErrorAdornerView_Unloaded);
  }

  private void ErrorAdornerView_Unloaded(object sender, RoutedEventArgs e)
  {
    UpdateMonitor.Events -= (EventHandler<UpdateEventArgs>) new EventHandler<UpdateEventArgs>(this.method_1);
  }

  private void ErrorAdornerView_Loaded(object sender, RoutedEventArgs e)
  {
    UpdateMonitor.Events += (EventHandler<UpdateEventArgs>) new EventHandler<UpdateEventArgs>(this.method_1);
  }

  private void method_0()
  {
    this.decorator_1.DataContext = (object) (this.decorator_0.DataContext as TestStepRowItem)?.Error;
  }

  private void method_1(object sender, UpdateEventArgs e) => this.method_0();

  private void decorator_0_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.method_0();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/erroradornerview.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.decorator_0 = (Decorator) target;
        this.decorator_0.DataContextChanged += new DependencyPropertyChangedEventHandler(this.decorator_0_DataContextChanged);
        break;
      case 2:
        this.decorator_1 = (Decorator) target;
        break;
      case 3:
        this.rectangle_0 = (Rectangle) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
