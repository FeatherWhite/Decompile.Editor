// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnFilter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ColumnFilter : DependencyObject, INotifyPropertyChanged
{
  public ColumnFilterContextInstance Context;
  public static readonly DependencyProperty AllProperty = DependencyProperty.Register(nameof (All), typeof (bool), typeof (ColumnFilter), new PropertyMetadata((object) true));
  public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof (Count), typeof (int), typeof (ColumnFilter));

  public void OnFilterChanged()
  {
    this.Count = this.Context.Count();
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }

  public event EventHandler<EventArgs> FilterChanged;

  public event PropertyChangedEventHandler PropertyChanged;

  private void method_0(string string_1)
  {
    // ISSUE: reference to a compiler-generated field
    PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, new PropertyChangedEventArgs(string_1));
  }

  private List<ColumnFilter.Selectable> method_1()
  {
    ColumnFilterContextInstance.ColumnValues[] uniqueStringValues = this.Context.getUniqueStringValues();
    HashSet<object> objectSet = new HashSet<object>();
    List<ColumnFilter.Selectable> source = new List<ColumnFilter.Selectable>();
    source.Add((ColumnFilter.Selectable) new ColumnFilter.AllItems(this, ((IEnumerable<ColumnFilterContextInstance.ColumnValues>) uniqueStringValues).All<ColumnFilterContextInstance.ColumnValues>((Func<ColumnFilterContextInstance.ColumnValues, bool>) (columnValues_0 => columnValues_0.IsSelected))));
    foreach (ColumnFilterContextInstance.ColumnValues columnValues in (IEnumerable<ColumnFilterContextInstance.ColumnValues>) ((IEnumerable<ColumnFilterContextInstance.ColumnValues>) uniqueStringValues).OrderBy<ColumnFilterContextInstance.ColumnValues, string>((Func<ColumnFilterContextInstance.ColumnValues, string>) (columnValues_0 => columnValues_0.Value)))
      source.Add((ColumnFilter.Selectable) new ColumnFilter.SelectableItem(columnValues.Value, this, columnValues.Significant != ColumnFilterContext.Visibility.Hidden, columnValues.IsSelected, columnValues.SavedOnly));
    return source.OrderByDescending<ColumnFilter.Selectable, bool>((Func<ColumnFilter.Selectable, bool>) (selectable_0 => selectable_0.IsSelectable)).ToList<ColumnFilter.Selectable>();
  }

  public void ReloadValues()
  {
  }

  public void UpdateFilter() => this.method_0("UniqueValues");

  public void OtherFiltersChanged()
  {
  }

  public List<ColumnFilter.Selectable> UniqueValues => this.method_1();

  public bool All
  {
    get => (bool) this.GetValue(ColumnFilter.AllProperty);
    set => this.SetValue(ColumnFilter.AllProperty, (object) value);
  }

  public int Count
  {
    get => (int) this.GetValue(ColumnFilter.CountProperty);
    set => this.SetValue(ColumnFilter.CountProperty, (object) value);
  }

  public string Name { get; set; }

  public ColumnFilter(string name, ColumnFilterContextInstance context)
  {
    this.Context = context;
    this.Name = name;
    this.OnFilterChanged();
  }

  internal void method_2()
  {
    this.Context.All(true);
    this.OnFilterChanged();
  }

  internal void method_3()
  {
    this.Context.ApplyState();
    this.Count = this.Context.Count();
    this.All = false;
  }

  public interface Selectable
  {
    bool IsSelected { get; set; }

    string Name { get; }

    bool IsSelectable { get; }

    bool OnlySaved { get; }
  }

  private class SelectableItem : ColumnFilter.Selectable
  {
    private ColumnFilter column;
    private bool isSelected;

    public string Name { get; set; }

    public SelectableItem(
      string name,
      ColumnFilter column,
      bool siginificant,
      bool isSelected,
      bool onlySaved)
    {
      this.Name = name;
      this.column = column;
      this.IsSelectable = siginificant;
      this.isSelected = isSelected;
      this.OnlySaved = onlySaved;
    }

    public void Select()
    {
      this.column.Context.Show(this.Name);
      this.isSelected = true;
    }

    public void Deselect()
    {
      this.column.Context.Hide(this.Name);
      this.isSelected = false;
    }

    public bool IsSelected
    {
      get => this.isSelected;
      set
      {
        if (value)
          this.Select();
        else
          this.Deselect();
        this.column.UpdateFilter();
        this.column.OnFilterChanged();
      }
    }

    public bool IsSelectable { get; set; }

    public bool OnlySaved { get; set; }
  }

  private class AllItems : ColumnFilter.Selectable
  {
    private ColumnFilter column;
    private bool isSelected = true;

    public AllItems(ColumnFilter column, bool initialState)
    {
      this.column = column;
      this.isSelected = initialState;
    }

    public void Deselect()
    {
      this.column.Context.All(false);
      this.column.UpdateFilter();
    }

    public void Select()
    {
      this.column.Context.All(true);
      this.column.UpdateFilter();
    }

    public bool IsSelected
    {
      get => this.isSelected;
      set
      {
        if (value)
          this.Select();
        else
          this.Deselect();
        this.isSelected = !this.isSelected;
        this.column.OnFilterChanged();
      }
    }

    public string Name => "All";

    public bool IsSelectable => true;

    public bool OnlySaved => false;
  }
}
