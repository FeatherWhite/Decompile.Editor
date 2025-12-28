// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ComboBoxMultiSelector
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ComboBoxMultiSelector : Selector, IMultiItemSelector
{
  public static readonly DependencyProperty BoxTemplateProperty = DependencyProperty.Register(nameof (BoxTemplate), typeof (DataTemplate), typeof (ComboBoxMultiSelector));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (ComboBoxMultiSelector));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ComboBoxMultiSelector));
  public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof (SelectedItems), typeof (IList), typeof (ComboBoxMultiSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(ComboBoxMultiSelector.smethod_0)));
  private static readonly DependencyPropertyKey dependencyPropertyKey_0 = DependencyProperty.RegisterReadOnly(nameof (Selectables), typeof (IList), typeof (ComboBoxMultiSelector), new PropertyMetadata());
  public static readonly DependencyProperty SelectablesProperty = ComboBoxMultiSelector.dependencyPropertyKey_0.DependencyProperty;
  private IMultiItemSelector imultiItemSelector_0;
  private bool bool_0;

  public DataTemplate BoxTemplate
  {
    get => (DataTemplate) this.GetValue(ComboBoxMultiSelector.BoxTemplateProperty);
    set => this.SetValue(ComboBoxMultiSelector.BoxTemplateProperty, (object) value);
  }

  public object Content
  {
    get => this.GetValue(ComboBoxMultiSelector.ContentProperty);
    set => this.SetValue(ComboBoxMultiSelector.ContentProperty, value);
  }

  public object Header
  {
    get => this.GetValue(ComboBoxMultiSelector.HeaderProperty);
    set => this.SetValue(ComboBoxMultiSelector.HeaderProperty, value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ComboBoxMultiSelector).method_0();
  }

  private void method_0()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 != null)
    {
      // ISSUE: reference to a compiler-generated field
      this.eventHandler_0((object) this, new EventArgs());
    }
    if (this.bool_0 || this.imultiItemSelector_0 == null)
      return;
    this.imultiItemSelector_0.SelectedItems = this.method_2((IEnumerable) this.SelectedItems);
  }

  private Selectable method_1(object object_0)
  {
    if (this.imultiItemSelector_0 != null && this.imultiItemSelector_0.ItemsSource != null)
    {
      Selectable selectable1 = this.imultiItemSelector_0.ItemsSource.Cast<Selectable>().ToList<Selectable>().FirstOrDefault<Selectable>((Func<Selectable, bool>) (selectable => selectable.Value.Equals(object_0)));
      if (selectable1 != null)
        return selectable1;
      return new Selectable() { Value = object_0 };
    }
    return new Selectable() { Value = object_0 };
  }

  private IList method_2(IEnumerable ienumerable_0)
  {
    return ienumerable_0 == null ? (IList) new List<object>() : (IList) ienumerable_0.Cast<object>().Select<object, Selectable>(new Func<object, Selectable>(this.method_1)).ToList<Selectable>();
  }

  private IList method_3(IEnumerable ienumerable_0)
  {
    return ienumerable_0 == null ? (IList) new List<object>() : (IList) ienumerable_0.Cast<Selectable>().Select<Selectable, object>((Func<Selectable, object>) (item => item.Value)).ToList<object>();
  }

  public IList SelectedItems
  {
    get => (IList) this.GetValue(ComboBoxMultiSelector.SelectedItemsProperty);
    set => this.SetValue(ComboBoxMultiSelector.SelectedItemsProperty, (object) value);
  }

  public IList Selectables
  {
    get => (IList) this.GetValue(ComboBoxMultiSelector.SelectablesProperty);
    set => this.SetValue(ComboBoxMultiSelector.dependencyPropertyKey_0, (object) value);
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnItemsSourceChanged(oldValue, newValue);
    this.method_4();
  }

  private void method_4()
  {
    this.Selectables = this.method_2(this.ItemsSource);
    if (this.imultiItemSelector_0 == null)
      return;
    this.imultiItemSelector_0.SelectedItems = this.Selectables;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.imultiItemSelector_0 = this.GetTemplateChild("PART_dropdown") as IMultiItemSelector;
    if (this.imultiItemSelector_0 != null)
      this.imultiItemSelector_0.SelectionChanged += new EventHandler(this.imultiItemSelector_0_SelectionChanged);
    this.method_4();
  }

  private void imultiItemSelector_0_SelectionChanged(object sender, EventArgs e)
  {
    this.bool_0 = true;
    this.SelectedItems = this.method_3((IEnumerable) this.imultiItemSelector_0.SelectedItems);
    this.bool_0 = false;
  }

  public event EventHandler SelectedItemsChanged;

  IList IMultiItemSelector.ItemsSource
  {
    get => (IList) this.GetValue(ItemsControl.ItemsSourceProperty);
    set => this.SetValue(ItemsControl.ItemsSourceProperty, (object) value);
  }

  event EventHandler IMultiItemSelector.SelectionChanged
  {
    add => this.SelectedItemsChanged += value;
    remove => this.SelectedItemsChanged -= value;
  }
}
