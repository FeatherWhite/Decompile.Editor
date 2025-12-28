// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MultiSelect
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MultiSelect : UserControl, IComponentConnector
{
  internal Grid grid;
  internal ComboBox selectablesBox;
  private bool bool_0;

  public MultiSelect()
  {
    this.InitializeComponent();
    this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.MultiSelect_DataContextChanged);
  }

  private void MultiSelect_DataContextChanged(object sender1, DependencyPropertyChangedEventArgs e1)
  {
    this.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.MultiSelect_DataContextChanged);
    (e1.NewValue is AnnotationCollection newValue ? newValue.Get<UpdateMonitor>(true, (object) null) : (UpdateMonitor) null)?.RegisterSourceUpdated((FrameworkElement) this, (Action) (() => this.grid.DataContext = (object) SelectableCommon.Create((AnnotationCollection) this.DataContext, this.grid.DataContext as SelectableCommon)));
    // ISSUE: reference to a compiler-generated method
    this.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((sender2, e2) => this.method_0());
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controlproviders/multiselect.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.selectablesBox = (ComboBox) target;
    }
    else
      this.grid = (Grid) target;
  }
}
