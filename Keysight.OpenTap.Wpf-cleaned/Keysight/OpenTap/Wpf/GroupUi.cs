// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GroupUi
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class GroupUi : BaseUi
{
  private bool isExpandedDefault;
  private readonly string groupIdentifier = "";
  private readonly string name;
  private List<BaseUi> list_0 = new List<BaseUi>();
  private bool bool_2 = true;
  private BaseUi[] baseUi_0;
  private BaseUi[] baseUi_1;

  public override string GetName() => this.name;

  public override double GetOrder()
  {
    return this.list_0.Select<BaseUi, double>((Func<BaseUi, double>) (baseUi_0 => baseUi_0.GetOrder())).DefaultIfEmpty<double>(0.0).Average();
  }

  public GroupUi(string name, string groupIdentifier, bool isExpandedDefault)
  {
    this.isExpandedDefault = isExpandedDefault;
    this.name = name;
    this.groupIdentifier = groupIdentifier;
  }

  public int Level { get; set; }

  public string Name => this.name;

  public IReadOnlyList<BaseUi> Items
  {
    get => (IReadOnlyList<BaseUi>) this.list_0;
    set
    {
      if (this.list_0 != null)
        this.list_0.ToList<BaseUi>().ForEach((Action<BaseUi>) (baseUi_2 => baseUi_2.PropertyChanged -= new PropertyChangedEventHandler(this.itemPropertyChanged)));
      this.list_0 = value.ToList<BaseUi>();
      this.list_0.ForEach((Action<BaseUi>) (baseUi_2 => baseUi_2.PropertyChanged += new PropertyChangedEventHandler(this.itemPropertyChanged)));
      this.IsVisible = !this.Items.All<BaseUi>((Func<BaseUi, bool>) (item => !item.IsVisible));
    }
  }

  public void itemPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "IsVisible"))
      return;
    this.IsVisible = !this.Items.All<BaseUi>((Func<BaseUi, bool>) (item => !item.IsVisible));
  }

  public bool IsExpanded
  {
    get
    {
      return ComponentSettings<GuiControlsSettings>.Current.PropGridItemIsExpanded(this.groupIdentifier + this.Name) ?? this.isExpandedDefault;
    }
    set
    {
      if (value == this.IsExpanded)
        return;
      ComponentSettings<GuiControlsSettings>.Current.SetPropGridCategoryState(this.groupIdentifier + this.Name, value);
      this.RaisePropertyChanged(nameof (IsExpanded));
      foreach (BaseUi baseUi in this.list_0)
        baseUi.GroupVisible = this.GroupVisible & value;
    }
  }

  public void UpdateGroupVisible()
  {
    bool isExpanded = this.IsExpanded;
    foreach (BaseUi baseUi in this.list_0)
    {
      baseUi.GroupVisible = this.GroupVisible & isExpanded;
      if (baseUi is GroupUi groupUi)
        groupUi.UpdateGroupVisible();
    }
  }

  public override bool GroupVisible
  {
    get => this.bool_2;
    set
    {
      if (this.bool_2 == value)
        return;
      bool isExpanded = this.IsExpanded;
      foreach (BaseUi baseUi in this.list_0)
        baseUi.GroupVisible = value & isExpanded;
      this.bool_2 = value;
      this.RaisePropertyChanged("CalcVisibility");
    }
  }

  public IEnumerable<BaseUi> Sequential
  {
    get
    {
      GroupUi groupUi1 = this;
      if (groupUi1.Level > 0)
        yield return (BaseUi) groupUi1;
      List<BaseUi>.Enumerator enumerator1 = groupUi1.list_0.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        BaseUi current = enumerator1.Current;
        if (current is GroupUi groupUi2)
        {
          IEnumerator<BaseUi> enumerator2 = groupUi2.Sequential.GetEnumerator();
          while (enumerator2.MoveNext())
            yield return enumerator2.Current;
          this.method_1();
          enumerator2 = (IEnumerator<BaseUi>) null;
        }
        else
          yield return current;
      }
      this.method_0();
      enumerator1 = new List<BaseUi>.Enumerator();
    }
  }

  public IEnumerable<BaseUi> SequentialFloatBottom
  {
    get
    {
      return (IEnumerable<BaseUi>) this.baseUi_0 ?? (IEnumerable<BaseUi>) (this.baseUi_0 = (BaseUi[]) this.Sequential.OfType<ItemUi>().Where<ItemUi>((Func<ItemUi, bool>) (itemUi_0 =>
      {
        GuiOptions guiOptions = itemUi_0.Annotation.Get<GuiOptions>(false, (object) null);
        return guiOptions != null && guiOptions.FloatBottom;
      })).ToArray<ItemUi>());
    }
  }

  public IEnumerable<BaseUi> SequentialNormal
  {
    get
    {
      return (IEnumerable<BaseUi>) this.baseUi_1 ?? (IEnumerable<BaseUi>) (this.baseUi_1 = this.Sequential.Where<BaseUi>((Func<BaseUi, bool>) (baseUi_0 => !(baseUi_0 is ItemUi itemUi ? itemUi.Annotation.Get<GuiOptions>(false, (object) null)?.FloatBottom : new bool?()).GetValueOrDefault())).ToArray<BaseUi>());
    }
  }
}
