// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.InlineContent
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class InlineContent : TextBlock
{
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (InlineContent), new PropertyMetadata((object) null, new PropertyChangedCallback(InlineContent.smethod_0)));
  public static readonly DependencyProperty AdditionalObjectsProperty = DependencyProperty.Register(nameof (AdditionalObjects), typeof (ObservableCollection<object>), typeof (InlineContent), new PropertyMetadata((object) null, new PropertyChangedCallback(InlineContent.smethod_1)));
  private HashSet<object> hashSet_0 = new HashSet<object>();

  public object Content
  {
    get => this.GetValue(InlineContent.ContentProperty);
    set => this.SetValue(InlineContent.ContentProperty, value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as InlineContent).method_3();
  }

  public ObservableCollection<object> AdditionalObjects
  {
    get => (ObservableCollection<object>) this.GetValue(InlineContent.AdditionalObjectsProperty);
    set => this.SetValue(InlineContent.AdditionalObjectsProperty, (object) value);
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
  }

  private void method_0()
  {
    if (this.AdditionalObjects == null)
      return;
    foreach (object additionalObject in (Collection<object>) this.AdditionalObjects)
      this.method_1(additionalObject);
  }

  private void method_1(object object_0)
  {
    if (this.hashSet_0.Contains(object_0))
      return;
    this.hashSet_0.Add(object_0);
    if (((IEnumerable<object>) this.Inlines).Contains<object>(object_0))
      return;
    if (object_0 is UIElement uiElement)
    {
      uiElement.IsHitTestVisible = false;
      if (this.GetLogicalChildren().Contains<DependencyObject>((DependencyObject) uiElement))
        this.RemoveLogicalChild((object) uiElement);
      if (VisualTreeHelper.GetParent((DependencyObject) uiElement) is ContainerVisual parent)
        parent.Children.Remove((Visual) uiElement);
      this.Inlines.Add(uiElement);
    }
    if (object_0 is Inline inline)
      this.Inlines.Add(inline);
    if (object_0 is string text)
      this.Inlines.Add(text);
    if (!(object_0 is IEnumerable<object> objects))
      return;
    foreach (object object_0_1 in objects)
      this.method_1(object_0_1);
  }

  private void method_2(object object_0)
  {
  }

  private void method_3()
  {
    this.Inlines.Clear();
    this.hashSet_0.Clear();
    this.method_1(this.Content);
    this.method_0();
  }

  public InlineContent()
  {
    this.AdditionalObjects = new ObservableCollection<object>();
    this.AdditionalObjects.CollectionChanged += new NotifyCollectionChangedEventHandler(this.method_4);
  }

  private void method_4(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        e.NewItems.Cast<object>().ToList<object>().ForEach(new Action<object>(this.method_1));
        break;
      case NotifyCollectionChangedAction.Remove:
        e.OldItems.Cast<object>().ToList<object>().ForEach(new Action<object>(this.method_2));
        break;
    }
  }
}
