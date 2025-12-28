// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GridSettingsControl
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
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class GridSettingsControl : UserControl, IComponentConnector
{
  private AnnotationCollection annotationCollection_0;
  internal PropertyDataGrid dataGrid;
  internal Button addRow;
  private bool bool_0;

  public GridSettingsControl() => this.InitializeComponent();

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != FrameworkElement.DataContextProperty)
      return;
    UpdateMonitor updateMonitor = new UpdateMonitor();
    this.annotationCollection_0 = AnnotationCollection.Annotate(dependencyPropertyChangedEventArgs_0.NewValue, new IAnnotation[1]
    {
      (IAnnotation) updateMonitor
    });
    updateMonitor.RegisterSourceUpdated((FrameworkElement) this, (Action) (() => this.DataGrid_SourceUpdated((object) this, (DataTransferEventArgs) null)));
    this.dataGrid.LoadData(this.annotationCollection_0);
  }

  private void DataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
  {
    this.RaiseEvent(new RoutedEventArgs(SettingsDialog.SettingsChangedEvent, sender));
  }

  protected void onEdited()
  {
    this.annotationCollection_0?.Read();
    this.annotationCollection_0?.Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
  }

  private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
  {
    this.onEdited();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/settingscontrolproviders/gridsettingscontrol.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.addRow = (Button) target;
    }
    else
      this.dataGrid = (PropertyDataGrid) target;
  }
}
