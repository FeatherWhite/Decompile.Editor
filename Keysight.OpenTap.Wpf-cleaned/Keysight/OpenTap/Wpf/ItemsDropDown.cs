// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ItemsDropDown
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ItemsDropDown : ControlDropDown, IMultiItemSelector
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof (bool), typeof (ItemsDropDown));
  public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (ItemsDropDown));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IList), typeof (ItemsDropDown), new PropertyMetadata((object) null, new PropertyChangedCallback(ItemsDropDown.smethod_1)));
  private readonly List<object> list_0 = new List<object>();
  public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof (SelectedItems), typeof (IList), typeof (ItemsDropDown), new PropertyMetadata((object) null, new PropertyChangedCallback(ItemsDropDown.smethod_2)));
  private bool bool_1;
  private bool bool_2;

  public static bool GetIsSelected(DependencyObject target)
  {
    return (bool) target.GetValue(ItemsDropDown.IsSelectedProperty);
  }

  public static void SetIsSelected(DependencyObject target, bool value)
  {
    target.SetValue(ItemsDropDown.IsSelectedProperty, (object) value);
  }

  public DataTemplate ItemTemplate
  {
    get => (DataTemplate) this.GetValue(ItemsDropDown.ItemTemplateProperty);
    set => this.SetValue(ItemsDropDown.ItemTemplateProperty, (object) value);
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ItemsDropDown).method_8(dependencyPropertyChangedEventArgs_0.OldValue as IList, dependencyPropertyChangedEventArgs_0.NewValue as IList);
  }

  public IList<DependencyPropertyDescriptor> GetAttachedProperties(
    DependencyObject dependencyObject_0)
  {
    List<DependencyPropertyDescriptor> attachedProperties = new List<DependencyPropertyDescriptor>();
    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties((object) dependencyObject_0, new Attribute[1]
    {
      (Attribute) new PropertyFilterAttribute(PropertyFilterOptions.All)
    }))
    {
      DependencyPropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(property);
      if (propertyDescriptor != null && propertyDescriptor.IsAttached)
        attachedProperties.Add(propertyDescriptor);
    }
    return (IList<DependencyPropertyDescriptor>) attachedProperties;
  }

  public static DependencyPropertyDescriptor GetAttachedPropertyDescriptor(
    DependencyObject dependencyObject_0,
    string name)
  {
    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties((object) dependencyObject_0, new Attribute[1]
    {
      (Attribute) new PropertyFilterAttribute(PropertyFilterOptions.All)
    }))
    {
      if (property.Name == name)
      {
        DependencyPropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(property);
        if (propertyDescriptor != null && propertyDescriptor.IsAttached)
          return propertyDescriptor;
      }
    }
    return (DependencyPropertyDescriptor) null;
  }

  private void method_3(object object_0)
  {
    if (!(object_0 is DependencyObject component))
      return;
    DependencyPropertyDescriptor.FromProperty(ItemsDropDown.IsSelectedProperty, object_0.GetType()).AddValueChanged((object) component, new EventHandler(this.method_4));
    this.list_0.Add(object_0);
  }

  private void method_4(object sender, EventArgs e)
  {
    if (this.bool_2)
      return;
    int num = (bool) (sender as DependencyObject).GetValue(ItemsDropDown.IsSelectedProperty) ? 1 : 0;
    List<object> list = (this.SelectedItems ?? (IList) new List<object>()).Cast<object>().ToList<object>();
    if (num != 0)
      list.Add(sender);
    else
      list.Remove(sender);
    this.bool_1 = true;
    this.SelectedItems = (IList) list;
    this.bool_1 = false;
  }

  private void method_5(object object_0)
  {
    if (!(object_0 is DependencyObject))
      return;
    DependencyPropertyDescriptor.FromProperty(ItemsDropDown.IsSelectedProperty, object_0.GetType())?.RemoveValueChanged(object_0, new EventHandler(this.method_4));
    this.list_0.Remove(object_0);
  }

  private void method_6(IList ilist_0)
  {
    foreach (object object_0 in (IEnumerable) ilist_0)
      this.method_3(object_0);
  }

  private void method_7(IList ilist_0)
  {
    foreach (object object_0 in (IEnumerable) ilist_0)
      this.method_5(object_0);
  }

  private void method_8(IList ilist_0, IList ilist_1)
  {
    if (ilist_0 != null)
      this.method_7(ilist_0);
    if (ilist_1 == null)
      return;
    this.method_6(ilist_1);
  }

  public IList ItemsSource
  {
    get => (IList) this.GetValue(ItemsDropDown.ItemsSourceProperty);
    set => this.SetValue(ItemsDropDown.ItemsSourceProperty, (object) value);
  }

  private static void smethod_2(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ItemsDropDown).method_9();
  }

  private void method_9()
  {
    if (!this.bool_1)
    {
      this.bool_2 = true;
      if (this.ItemsSource != null && this.SelectedItems != null)
        this.ItemsSource.Cast<object>().Where<object>((Func<object, bool>) (item => item is DependencyObject)).Evaluate<object>((Action<object>) (object_0 => (object_0 as DependencyObject).SetValue(ItemsDropDown.IsSelectedProperty, (object) this.SelectedItems.Contains(object_0))));
      this.bool_2 = false;
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (this.eventHandler_0 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.eventHandler_0((object) this, new EventArgs());
    }
  }

  public event EventHandler SelectionChanged;

  public IList SelectedItems
  {
    get => (IList) this.GetValue(ItemsDropDown.SelectedItemsProperty);
    set => this.SetValue(ItemsDropDown.SelectedItemsProperty, (object) value);
  }
}
