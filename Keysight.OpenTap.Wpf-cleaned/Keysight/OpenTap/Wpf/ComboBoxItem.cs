// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ComboBoxItem
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ComboBoxItem : INotifyPropertyChanged
{
  public string Name { get; private set; }

  public string TypeName { get; private set; }

  public object Value { get; private set; }

  public ComboBoxItem(object value)
  {
    this.Value = value;
    this.Name = !(value is Enum) ? value.ToString() : value.GetType().GetMember(value.ToString())[0].GetDisplayAttribute().Name;
    this.TypeName = (string) null;
    if (!(value is INotifyPropertyChanged notifyPropertyChanged))
      return;
    notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(this.ipc_PropertyChanged);
  }

  public void UpdateName()
  {
    this.Name = this.Value.ToString();
    this.OnPropertyChanged("Name");
  }

  private void ipc_PropertyChanged(object sender, PropertyChangedEventArgs e) => this.UpdateName();

  public static IEnumerable<ComboBoxItem> ToComboboxItemList(IEnumerable list)
  {
    List<ComboBoxItem> comboboxItemList = new List<ComboBoxItem>();
    string str = (string) null;
    bool flag = false;
    foreach (object obj in list)
    {
      comboboxItemList.Add(new ComboBoxItem(obj));
      string name = obj.GetType().Name;
      if (str == null)
        str = name;
      if (str != name)
      {
        flag = true;
        if (str.Length > name.Length)
          str = name;
      }
    }
    if (flag)
    {
      foreach (ComboBoxItem comboBoxItem in comboboxItemList)
        comboBoxItem.TypeName = comboBoxItem.Value.GetType().GetDisplayAttribute().GetFullName();
    }
    return (IEnumerable<ComboBoxItem>) comboboxItemList;
  }

  private void OnPropertyChanged(string string_0)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(string_0));
  }

  public event PropertyChangedEventHandler PropertyChanged;
}
