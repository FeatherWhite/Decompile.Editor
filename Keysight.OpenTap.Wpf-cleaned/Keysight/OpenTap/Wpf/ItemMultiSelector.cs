// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ItemMultiSelector
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ItemMultiSelector : Control, IMultiItemSelector
{
  public static DependencyProperty ItemContentTemplateProperty = DependencyProperty.Register(nameof (ItemContentTemplate), typeof (DataTemplate), typeof (ItemMultiSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(ItemMultiSelector.smethod_0)));
  public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof (SelectedItems), typeof (IList), typeof (ItemMultiSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(ItemMultiSelector.smethod_0)));
  public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IList), typeof (ItemMultiSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(ItemMultiSelector.smethod_1)));
  public static DependencyProperty ItemsProperty = DependencyProperty.Register(nameof (Items), typeof (IList<Selectable>), typeof (ItemMultiSelector), new PropertyMetadata((PropertyChangedCallback) null));
  private bool bool_0;

  public DataTemplate ItemContentTemplate
  {
    get => this.GetValue(ItemMultiSelector.ItemContentTemplateProperty) as DataTemplate;
    set => this.SetValue(ItemMultiSelector.ItemContentTemplateProperty, (object) value);
  }

  public IList SelectedItems
  {
    get => this.GetValue(ItemMultiSelector.SelectedItemsProperty) as IList;
    set => this.SetValue(ItemMultiSelector.SelectedItemsProperty, (object) value);
  }

  public IList ItemsSource
  {
    get => this.GetValue(ItemMultiSelector.ItemsSourceProperty) as IList;
    set => this.SetValue(ItemMultiSelector.ItemsSourceProperty, (object) value);
  }

  public IList<Selectable> Items
  {
    get => this.GetValue(ItemMultiSelector.ItemsProperty) as IList<Selectable>;
    set => this.SetValue(ItemMultiSelector.ItemsProperty, (object) value);
  }

  public event EventHandler SelectionChanged;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ItemMultiSelector).method_1();
  }

  private void method_0()
  {
    this.bool_0 = true;
    if (this.Items != null && this.SelectedItems != null)
    {
      foreach (Selectable selectable in (IEnumerable<Selectable>) this.Items)
        selectable.IsSelected = this.SelectedItems.Contains(selectable.Value);
    }
    this.bool_0 = false;
  }

  private void method_1()
  {
    if (this.bool_0)
      return;
    this.method_0();
    this.bool_0 = false;
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ItemMultiSelector).method_2((IList) dependencyPropertyChangedEventArgs_0.OldValue, (IList) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  private void method_2(IList ilist_0, IList ilist_1)
  {
    if (this.Items != null)
      this.Items.Evaluate<Selectable>((Action<Selectable>) (selectable_0 => selectable_0.IsSelectedChanged -= new EventHandler(this.method_3)));
    if (ilist_1 != null)
    {
      this.Items = (IList<Selectable>) ilist_1.Cast<object>().Select<object, Selectable>((Func<object, Selectable>) (item => new Selectable()
      {
        Value = item
      })).ToList<Selectable>();
      this.Items.Evaluate<Selectable>((Action<Selectable>) (selectable_0 => selectable_0.IsSelectedChanged += new EventHandler(this.method_3)));
    }
    this.method_0();
  }

  private void method_3(object sender, EventArgs e)
  {
    if (this.bool_0)
      return;
    Selectable selectable = sender as Selectable;
    IEnumerable<object> source = this.SelectedItems.Cast<object>();
    if (selectable.IsSelected)
    {
      if (!this.SelectedItems.Contains(selectable.Value))
        source = source.Append<object>(selectable.Value);
    }
    else
      source = source.Where<object>((Func<object, bool>) (item => item != selectable.Value));
    this.SelectedItems = (IList) source.ToList<object>();
  }
}
