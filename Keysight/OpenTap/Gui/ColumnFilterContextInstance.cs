// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnFilterContextInstance
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ColumnFilterContextInstance
{
  private ColumnFilterContext columnFilterContext_0;
  private ColumnViewProvider view;
  private Dictionary<object, string> dictionary_0 = new Dictionary<object, string>();

  public TestStepRowItem[] Items => this.columnFilterContext_0.Items;

  private int columnIndex => this.columnFilterContext_0.GetFilterInstanceIndex(this);

  public int Count()
  {
    return this.columnFilterContext_0.method_2() - this.columnFilterContext_0.method_1(this.columnIndex);
  }

  public ColumnFilterContextInstance(
    ColumnFilterContext columnFilterContext_1,
    ColumnViewProvider view)
  {
    this.columnFilterContext_0 = columnFilterContext_1;
    this.view = view;
  }

  private string method_0(TestStepRowItem testStepRowItem_0)
  {
    object key = this.view.GetValue((object) testStepRowItem_0.Step) ?? (object) "";
    if (key is string str1)
      return str1;
    string str2;
    if (this.dictionary_0.TryGetValue(key, out str2))
      return str2;
    string str3;
    if (!StringConvertProvider.TryGetString(key, out str3))
      return (string) null;
    this.dictionary_0[key] = str3;
    return str3;
  }

  public ColumnFilterContextInstance.ColumnValues[] getUniqueStringValues()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ColumnFilterContextInstance.Class68 class68 = new ColumnFilterContextInstance.Class68();
    if (this.Items == null)
      return Array.Empty<ColumnFilterContextInstance.ColumnValues>();
    HashSet<string> stringSet1 = new HashSet<string>();
    HashSet<string> stringSet2 = new HashSet<string>();
    HashSet<string> stringSet3 = new HashSet<string>();
    // ISSUE: reference to a compiler-generated field
    class68.hashSet_0 = new HashSet<string>();
    int columnIndex = this.columnIndex;
    List<string> stringList = new List<string>();
    for (int index = 0; index < this.Items.Length; ++index)
    {
      TestStepRowItem testStepRowItem_0 = this.Items[index];
      ColumnFilterContext.Visibility visibility = this.columnFilterContext_0.IsVisible(testStepRowItem_0, columnIndex);
      string str = this.method_0(testStepRowItem_0) ?? "";
      switch (visibility)
      {
        case ColumnFilterContext.Visibility.Visible:
          stringSet1.Add(str);
          break;
        case ColumnFilterContext.Visibility.HiddenButSignificant:
          stringSet3.Add(str);
          break;
        case ColumnFilterContext.Visibility.Hidden:
          stringSet2.Add(str);
          break;
      }
      if (!this.columnFilterContext_0.IsSelected(testStepRowItem_0, columnIndex))
      {
        // ISSUE: reference to a compiler-generated field
        class68.hashSet_0.Add(str);
      }
    }
    Dictionary<string, ColumnFilterContext.Visibility> source = new Dictionary<string, ColumnFilterContext.Visibility>();
    foreach (string key in stringSet2)
      source[key] = ColumnFilterContext.Visibility.Hidden;
    foreach (string key in stringSet3)
      source[key] = ColumnFilterContext.Visibility.HiddenButSignificant;
    foreach (string key in stringSet1)
      source[key] = ColumnFilterContext.Visibility.Visible;
    // ISSUE: reference to a compiler-generated field
    class68.hashSet_1 = new HashSet<string>();
    HashSet<string> stringSet4;
    if (((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations).TryGetValue(this.view.Name, out stringSet4))
    {
      foreach (string key in stringSet4)
      {
        if (!source.ContainsKey(key))
        {
          source[key] = ColumnFilterContext.Visibility.Hidden;
          // ISSUE: reference to a compiler-generated field
          class68.hashSet_1.Add(key);
        }
        // ISSUE: reference to a compiler-generated field
        class68.hashSet_0.Add(key);
      }
    }
    // ISSUE: reference to a compiler-generated method
    return source.Select<KeyValuePair<string, ColumnFilterContext.Visibility>, ColumnFilterContextInstance.ColumnValues>(new Func<KeyValuePair<string, ColumnFilterContext.Visibility>, ColumnFilterContextInstance.ColumnValues>(class68.method_0)).ToArray<ColumnFilterContextInstance.ColumnValues>();
  }

  private void method_1(string string_0, bool bool_0)
  {
    HashSet<string> stringSet;
    if (!((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations).TryGetValue(this.view.Name, out stringSet))
    {
      stringSet = new HashSet<string>();
      ((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations)[this.view.Name] = stringSet;
    }
    if (!bool_0)
    {
      stringSet.Add(string_0);
    }
    else
    {
      stringSet.Remove(string_0);
      if (stringSet.Count == 0)
        ((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations).Remove(this.view.Name);
    }
    int columnIndex = this.columnIndex;
    Dictionary<object, string> dictionary = new Dictionary<object, string>();
    for (int int_0 = 0; int_0 < this.Items.Length; ++int_0)
    {
      object key = this.view.GetValue((object) this.Items[int_0].Step) ?? (object) "";
      if (!(key is string str) && !dictionary.TryGetValue(key, out str))
      {
        str = StringConvertProvider.GetString(key);
        dictionary[key] = str;
      }
      if (str == string_0)
      {
        if (bool_0)
          this.columnFilterContext_0.Show(int_0, columnIndex);
        else
          this.columnFilterContext_0.Hide(int_0, columnIndex);
      }
    }
  }

  public void Hide(string value) => this.method_1(value, false);

  public void Show(string value) => this.method_1(value, true);

  public void All(bool state)
  {
    int columnIndex = this.columnIndex;
    for (int int_0 = 0; int_0 < this.Items.Length; ++int_0)
    {
      if (state)
        this.columnFilterContext_0.Show(int_0, columnIndex);
      else
        this.columnFilterContext_0.Hide(int_0, columnIndex);
    }
    if (state)
      ((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations).Remove(this.view.Name);
    else
      ((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations)[this.view.Name] = new HashSet<string>(((IEnumerable<ColumnFilterContextInstance.ColumnValues>) this.getUniqueStringValues()).Select<ColumnFilterContextInstance.ColumnValues, string>((Func<ColumnFilterContextInstance.ColumnValues, string>) (columnValues_0 => columnValues_0.Value)));
  }

  public void ApplyState()
  {
    int columnIndex = this.columnIndex;
    HashSet<string> stringSet;
    if (!((Dictionary<string, HashSet<string>>) ComponentSettings<GuiControlsSettings>.Current.FilterConfigurations).TryGetValue(this.view.Name, out stringSet))
      return;
    for (int int_0 = 0; int_0 < this.Items.Length; ++int_0)
    {
      string str = this.method_0(this.Items[int_0]);
      if (stringSet.Contains(str))
        this.columnFilterContext_0.Hide(int_0, columnIndex);
    }
  }

  public struct ColumnValues
  {
    public string Value;
    public ColumnFilterContext.Visibility Significant;
    public bool IsSelected;
    public bool SavedOnly;
  }
}
