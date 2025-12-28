// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnVisibilityDetails
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Gui;

[DebuggerDisplay("{Name}: Priority: {Priority} Width: {Width} Visible: {IsVisible}")]
public class ColumnVisibilityDetails : INotifyPropertyChanged
{
  public readonly DataGridColumn Column;

  public bool IsVisible
  {
    get => this.Column.Visibility == System.Windows.Visibility.Visible;
    set
    {
      this.Column.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
      this.State.IsVisible = value;
      this.method_0(nameof (IsVisible));
    }
  }

  public double Priority
  {
    get => this.State.Priority;
    set => this.State.Priority = value;
  }

  public double OriginalPriority => this.State.OriginalPriority;

  public bool ManuallyConfigured
  {
    get => this.State.ManuallyConfigured;
    set => this.State.ManuallyConfigured = value;
  }

  public DataGridLength Width
  {
    get
    {
      if (!((int?) this.State.Width).HasValue)
        return DataGridLength.Auto;
      int pixels = ((int?) this.State.Width).Value;
      return pixels == -1 ? DataGridLength.Auto : new DataGridLength((double) pixels);
    }
    set
    {
      int? width = (int?) this.State.Width;
      DataGridLength? nullable = width.HasValue ? new DataGridLength?((DataGridLength) (double) width.GetValueOrDefault()) : new DataGridLength?();
      DataGridLength dataGridLength = value;
      if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == dataGridLength ? 1 : 0) : 1) : 0) != 0)
        return;
      if (!value.IsAuto && !value.IsSizeToCells && !value.IsSizeToHeader)
        this.State.Width = (int?) new int?((int) value.Value);
      else
        this.State.Width = (int?) new int?();
    }
  }

  public string Name => this.State.Name;

  public event PropertyChangedEventHandler PropertyChanged;

  public ColumnState GetColumnState(string name, double originalPriority = 0.0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ColumnVisibilityDetails.Class73 class73 = new ColumnVisibilityDetails.Class73();
    // ISSUE: reference to a compiler-generated field
    class73.string_0 = name;
    // ISSUE: reference to a compiler-generated method
    ColumnState columnState = ((IEnumerable) ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState2).OfType<ColumnState>().FirstOrDefault<ColumnState>(new Func<ColumnState, bool>(class73.method_0));
    if (columnState == null)
    {
      // ISSUE: reference to a compiler-generated field
      columnState = new ColumnState(class73.string_0, originalPriority);
      ((List<object>) ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState2).Add((object) columnState);
    }
    return columnState;
  }

  public static List<ColumnState> GetColumnStates()
  {
    return ((IEnumerable) ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState2).OfType<ColumnState>().ToList<ColumnState>();
  }

  private ColumnState State { get; set; }

  public ColumnVisibilityDetails(
    DataGridColumn dataGridColumn_0,
    string Name,
    double originalPriority = 0.0)
  {
    this.State = this.GetColumnState(Name, originalPriority);
    this.Column = dataGridColumn_0;
    DependencyPropertyDescriptor.FromProperty(DataGridColumn.WidthProperty, typeof (DataGridColumn)).AddValueChanged((object) dataGridColumn_0, (EventHandler) ((sender, e) => this.Width = (sender as DataGridColumn).Width));
    if (this.Width != DataGridLength.Auto)
      dataGridColumn_0.Width = this.Width;
    dataGridColumn_0.Visibility = this.State.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
  }

  private void method_0(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_0));
  }

  public void Remove()
  {
    ((List<object>) ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState2).Remove((object) this.State);
  }
}
