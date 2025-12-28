// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.AttachedAdorner
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class AttachedAdorner
{
  public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof (Type), typeof (AttachedAdorner), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(AttachedAdorner.smethod_0)));
  public static readonly DependencyProperty AdornerParameterProperty = DependencyProperty.RegisterAttached("AdornerParameter", typeof (object), typeof (AttachedAdorner), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  private static Dictionary<FrameworkElement, Adorner> dictionary_0 = new Dictionary<FrameworkElement, Adorner>();

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is FrameworkElement frameworkElement_0))
      return;
    if (dependencyPropertyChangedEventArgs_0.NewValue == null)
      AttachedAdorner.smethod_3(frameworkElement_0);
    else
      AttachedAdorner.smethod_1(frameworkElement_0);
  }

  public static void SetAdorner(DependencyObject element, Type value)
  {
    element.SetValue(AttachedAdorner.AdornerProperty, (object) value);
  }

  public static Type GetAdorner(DependencyObject element)
  {
    return (Type) element.GetValue(AttachedAdorner.AdornerProperty);
  }

  public static void SetAdornerParameter(DependencyObject element, object value)
  {
    element.SetValue(AttachedAdorner.AdornerProperty, value);
  }

  public static object GetAdornerParameter(DependencyObject element)
  {
    return element.GetValue(AttachedAdorner.AdornerProperty);
  }

  private static void smethod_1(FrameworkElement frameworkElement_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AttachedAdorner.Class63 class63 = new AttachedAdorner.Class63();
    // ISSUE: reference to a compiler-generated field
    class63.frameworkElement_0 = frameworkElement_0;
    // ISSUE: reference to a compiler-generated field
    AttachedAdorner.smethod_3(class63.frameworkElement_0);
    // ISSUE: reference to a compiler-generated field
    if (!class63.frameworkElement_0.IsLoaded)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      class63.frameworkElement_0.Loaded += new RoutedEventHandler(class63.method_0);
    }
    // ISSUE: reference to a compiler-generated field
    Type adorner = AttachedAdorner.GetAdorner((DependencyObject) class63.frameworkElement_0);
    if (adorner == (Type) null)
      return;
    // ISSUE: reference to a compiler-generated field
    Adorner instance = (Adorner) Activator.CreateInstance(adorner, (object) class63.frameworkElement_0);
    // ISSUE: reference to a compiler-generated field
    AdornerLayer.GetAdornerLayer((Visual) class63.frameworkElement_0).Add(instance);
    PropertyPath propertyPath = new PropertyPath((object) AttachedAdorner.AdornerParameterProperty);
    // ISSUE: reference to a compiler-generated field
    instance.SetBinding(AttachedAdorner.AdornerParameterProperty, (BindingBase) new Binding()
    {
      Path = propertyPath,
      Source = (object) class63.frameworkElement_0
    });
    // ISSUE: reference to a compiler-generated field
    AttachedAdorner.dictionary_0.Remove(class63.frameworkElement_0);
    // ISSUE: reference to a compiler-generated field
    AttachedAdorner.dictionary_0.Add(class63.frameworkElement_0, instance);
    // ISSUE: reference to a compiler-generated field
    class63.frameworkElement_0.Unloaded += new RoutedEventHandler(AttachedAdorner.smethod_2);
  }

  private static void smethod_2(object sender, RoutedEventArgs e)
  {
    AttachedAdorner.SetAdorner((DependencyObject) sender, (Type) null);
  }

  private static void smethod_3(FrameworkElement frameworkElement_0)
  {
    Adorner adorner;
    if (!AttachedAdorner.dictionary_0.TryGetValue(frameworkElement_0, out adorner))
      return;
    AdornerLayer.GetAdornerLayer((Visual) frameworkElement_0)?.Remove(adorner);
    AttachedAdorner.dictionary_0.Remove(frameworkElement_0);
  }

  private static void smethod_4(object sender, RoutedEventArgs e)
  {
    if (!(sender is FrameworkElement frameworkElement_0))
      return;
    AttachedAdorner.smethod_1(frameworkElement_0);
  }
}
