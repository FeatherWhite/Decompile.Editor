// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnFilterContext
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ColumnFilterContext
{
  public BitMatrix FilterSelection = new BitMatrix()
  {
    DefaultValue = true
  };
  public TestStepRowItem[] Items;
  private Dictionary<TestStepRowItem, int> dictionary_0 = new Dictionary<TestStepRowItem, int>();
  private Dictionary<ColumnFilterContextInstance, int> dictionary_1 = new Dictionary<ColumnFilterContextInstance, int>();

  public void Update(IEnumerable<TestStepRowItem> items, ColumnFilter[] filters)
  {
    this.Items = items.ToArray<TestStepRowItem>();
    this.dictionary_0.Clear();
    this.dictionary_1.Clear();
    this.FilterSelection.Columns = filters.Length;
    for (int index = 0; index < filters.Length; ++index)
      this.dictionary_1[filters[index].Context] = index;
    for (int index = 0; index < this.Items.Length; ++index)
      this.dictionary_0[this.Items[index]] = index;
    this.FilterSelection.Rows = this.Items.Length;
    this.FilterSelection.Clear(true);
    foreach (ColumnFilter filter in filters)
      filter.method_3();
  }

  public ColumnFilterContext.Visibility IsVisible(int index, int int_0 = -1)
  {
    if (int_0 != -1)
    {
      int num = this.FilterSelection.RowHits(index);
      if (num == this.FilterSelection.Columns)
        return ColumnFilterContext.Visibility.Visible;
      if (num == this.FilterSelection.Columns - 1)
        return this.FilterSelection[int_0, index] ? ColumnFilterContext.Visibility.Hidden : ColumnFilterContext.Visibility.HiddenButSignificant;
    }
    return !this.FilterSelection.FullRow(index) ? ColumnFilterContext.Visibility.Hidden : ColumnFilterContext.Visibility.Visible;
  }

  public bool IsSelected(int index, int int_0 = -1) => this.FilterSelection[int_0, index];

  public bool IsSelected(TestStepRowItem item, int int_0 = -1)
  {
    int index;
    if (!this.dictionary_0.TryGetValue(item, out index))
      throw new Exception("Item was not present in the dictionary");
    return this.IsSelected(index, int_0);
  }

  public ColumnFilterContext.Visibility IsVisible(TestStepRowItem item, int int_0 = -1)
  {
    int index;
    if (!this.dictionary_0.TryGetValue(item, out index))
      throw new Exception("Item was not present in the dictionary");
    return this.IsVisible(index, int_0);
  }

  public int GetFilterInstanceIndex(ColumnFilterContextInstance inst)
  {
    int filterInstanceIndex;
    if (this.dictionary_1.TryGetValue(inst, out filterInstanceIndex))
      return filterInstanceIndex;
    int columns = this.FilterSelection.Columns;
    ++this.FilterSelection.Columns;
    this.dictionary_1[inst] = columns;
    return columns;
  }

  public ColumnFilterContextInstance CreateInstance(ColumnViewProvider provider)
  {
    return new ColumnFilterContextInstance(this, provider);
  }

  public void Hide(int int_0, int column) => this.FilterSelection[column, int_0] = false;

  public void Show(int int_0, int column) => this.FilterSelection[column, int_0] = true;

  internal void method_0()
  {
    this.dictionary_0.Clear();
    this.dictionary_1.Clear();
    this.FilterSelection.Columns = 0;
    this.FilterSelection.Rows = 0;
  }

  internal int method_1(int int_0) => this.FilterSelection.ColumnHits(int_0);

  internal int method_2() => this.FilterSelection.Rows;

  public enum Visibility
  {
    Visible,
    HiddenButSignificant,
    Hidden,
  }
}
