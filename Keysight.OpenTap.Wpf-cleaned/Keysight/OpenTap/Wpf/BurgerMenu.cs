// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.BurgerMenu
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class BurgerMenu : ContentControl
{
  public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (BurgerMenu));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IList), typeof (BurgerMenu), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof (Item), typeof (object), typeof (BurgerMenu), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(nameof (HasItems), typeof (bool), typeof (BurgerMenu));
  public static readonly DependencyProperty RequestOpenProperty = DependencyProperty.Register(nameof (RequestOpen), typeof (bool), typeof (BurgerMenu));
  private bool bool_0;
  private Task task_0;

  public DataTemplate ItemTemplate
  {
    get => (DataTemplate) this.GetValue(BurgerMenu.ItemTemplateProperty);
    set => this.SetValue(BurgerMenu.ItemTemplateProperty, (object) value);
  }

  public IList ItemsSource
  {
    get => (IList) this.GetValue(BurgerMenu.ItemsSourceProperty);
    set => this.SetValue(BurgerMenu.ItemsSourceProperty, (object) value);
  }

  public object Item
  {
    get => this.GetValue(BurgerMenu.ItemProperty);
    set => this.SetValue(BurgerMenu.ItemProperty, value);
  }

  public bool HasItems
  {
    get => (bool) this.GetValue(BurgerMenu.HasItemsProperty);
    set => this.SetValue(BurgerMenu.HasItemsProperty, (object) value);
  }

  public bool RequestOpen
  {
    get => (bool) this.GetValue(BurgerMenu.RequestOpenProperty);
    set => this.SetValue(BurgerMenu.RequestOpenProperty, (object) value);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == BurgerMenu.ItemProperty || dependencyPropertyChangedEventArgs_0.Property == BurgerMenu.ItemsSourceProperty)
      this.HasItems = this.Item != null || this.ItemsSource != null && this.ItemsSource.Count > 0;
    if (dependencyPropertyChangedEventArgs_0.Property == UIElement.IsMouseOverProperty)
    {
      if (!this.IsMouseOver)
      {
        this.RequestOpen = this.IsMouseOver;
        this.bool_0 = true;
      }
      else
      {
        if (this.task_0 != null)
          return;
        this.bool_0 = false;
        this.task_0 = Task.Run((Action) (() =>
        {
          Task.Delay(100).Wait();
          if (!this.bool_0)
            GuiHelpers.GuiInvoke((Action) (() => this.RequestOpen = true));
          this.task_0 = (Task) null;
        }));
      }
    }
    else
    {
      if (dependencyPropertyChangedEventArgs_0.Property != BurgerMenu.ItemProperty)
        return;
      if (dependencyPropertyChangedEventArgs_0.OldValue != null)
        this.RemoveLogicalChild(dependencyPropertyChangedEventArgs_0.OldValue);
      if (dependencyPropertyChangedEventArgs_0.NewValue == null)
        return;
      UIElement newValue = dependencyPropertyChangedEventArgs_0.NewValue as UIElement;
      if (newValue.GetParentObject() != null && newValue.GetParentObject() == this)
        return;
      this.AddLogicalChild(dependencyPropertyChangedEventArgs_0.NewValue);
    }
  }
}
