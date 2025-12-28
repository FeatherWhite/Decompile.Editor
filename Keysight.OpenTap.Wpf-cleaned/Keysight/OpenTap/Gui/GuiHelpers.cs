// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.GuiHelpers
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

public static class GuiHelpers
{
  public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
  {
    DependencyObject parentObject = child.GetParentObject();
    if (parentObject == null)
      return default (T);
    return parentObject is T obj ? obj : parentObject.TryFindParent<T>();
  }

  public static DependencyObject FindVisualChild(
    Func<DependencyObject, bool> acceptFunction,
    int maxDepth,
    DependencyObject currentElement)
  {
    if (acceptFunction(currentElement))
      return currentElement;
    if (maxDepth == 0 && maxDepth != -1)
      return (DependencyObject) null;
    if (currentElement is Visual)
    {
      int childrenCount = VisualTreeHelper.GetChildrenCount(currentElement);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject visualChild = GuiHelpers.FindVisualChild(acceptFunction, maxDepth - 1, VisualTreeHelper.GetChild(currentElement, childIndex));
        if (visualChild != null)
          return visualChild;
      }
    }
    return (DependencyObject) null;
  }

  public static T FindVisualChild<T>(this DependencyObject depObj, int maxDepth = -1) where T : class
  {
    return GuiHelpers.FindVisualChild((Func<DependencyObject, bool>) (dependencyObject_0 => dependencyObject_0 is T), maxDepth, depObj) as T;
  }

  public static T FindParent<T>(this DependencyObject depObj) where T : DependencyObject
  {
    for (DependencyObject parentObject = depObj.GetParentObject(); parentObject != null; parentObject = parentObject.GetParentObject())
    {
      if (parentObject is T parent)
        return parent;
    }
    return default (T);
  }

  public static T FindParent<T>(this DependencyObject depObj, Func<T, bool> selector) where T : DependencyObject
  {
    for (DependencyObject parentObject = depObj.GetParentObject(); parentObject != null; parentObject = parentObject.GetParentObject())
    {
      if (parentObject is T parent && selector(parent))
        return parent;
    }
    return default (T);
  }

  public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject dpObj)
  {
    if (dpObj != null)
    {
      int childrenCount = VisualTreeHelper.GetChildrenCount(dpObj);
      int childIndex;
      for (childIndex = 0; childIndex < childrenCount; ++childIndex)
        yield return VisualTreeHelper.GetChild(dpObj, childIndex);
      for (childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        IEnumerator<DependencyObject> enumerator = VisualTreeHelper.GetChild(dpObj, childIndex).GetVisualChildren().GetEnumerator();
        while (enumerator.MoveNext())
          yield return enumerator.Current;
        // ISSUE: reference to a compiler-generated method
        this.method_0();
        enumerator = (IEnumerator<DependencyObject>) null;
      }
    }
  }

  public static IEnumerable<DependencyObject> GetLogicalChildren(this DependencyObject dpObj)
  {
    IEnumerator<DependencyObject> enumerator1 = LogicalTreeHelper.GetChildren(dpObj).Cast<object>().Where<object>((Func<object, bool>) (object_0 => object_0 is DependencyObject)).Cast<DependencyObject>().GetEnumerator();
    while (enumerator1.MoveNext())
    {
      DependencyObject dpObj1 = enumerator1.Current;
      yield return dpObj1;
      IEnumerator<DependencyObject> enumerator2 = dpObj1.GetLogicalChildren().GetEnumerator();
      while (enumerator2.MoveNext())
        yield return enumerator2.Current;
      // ISSUE: reference to a compiler-generated method
      this.method_1();
      enumerator2 = (IEnumerator<DependencyObject>) null;
      dpObj1 = (DependencyObject) null;
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator1 = (IEnumerator<DependencyObject>) null;
  }

  public static DependencyObject GetParentObject(this DependencyObject child)
  {
    switch (child)
    {
      case null:
        return (DependencyObject) null;
      case ContentElement reference:
        DependencyObject parent1 = ContentOperations.GetParent(reference);
        if (parent1 != null)
          return parent1;
        return !(reference is FrameworkContentElement frameworkContentElement) ? (DependencyObject) null : frameworkContentElement.Parent;
      case FrameworkElement frameworkElement:
        DependencyObject parent2 = frameworkElement.Parent;
        if (parent2 != null)
          return parent2;
        break;
    }
    return VisualTreeHelper.GetParent(child);
  }

  public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
  {
    if (!(reference.InputHitTest(point) is DependencyObject child))
      return default (T);
    return child is T obj ? obj : child.TryFindParent<T>();
  }

  public static object TryFindFromPoint(UIElement reference, Point point, Type targetType)
  {
    if (!(reference.InputHitTest(point) is DependencyObject fromPoint))
      return (object) null;
    if (fromPoint.GetType() == targetType)
      return (object) fromPoint;
    return typeof (GuiHelpers).GetMethod("TryFindParent").MakeGenericMethod(targetType).Invoke((object) null, new object[1]
    {
      (object) fromPoint
    });
  }

  public static void UnfixGridRows(Grid grid, GridSplitter splitter, bool doColumns)
  {
    IList list = !doColumns ? (IList) grid.RowDefinitions : (IList) grid.ColumnDefinitions;
    int num = !doColumns ? (int) splitter.GetValue(Grid.RowProperty) : (int) splitter.GetValue(Grid.ColumnProperty);
    int index1 = num - 1;
    int index2 = num;
    switch (splitter.ResizeBehavior)
    {
      case GridResizeBehavior.CurrentAndNext:
        index1 = num;
        index2 = num + 1;
        break;
      case GridResizeBehavior.PreviousAndCurrent:
        index1 = num - 1;
        index2 = num;
        break;
      case GridResizeBehavior.PreviousAndNext:
        index1 = num - 1;
        index2 = num + 1;
        break;
    }
    object obj1 = list[index1];
    object obj2 = list[index2];
    if (doColumns)
    {
      ColumnDefinition columnDefinition1 = obj1 as ColumnDefinition;
      ColumnDefinition columnDefinition2 = obj2 as ColumnDefinition;
      columnDefinition1.MaxWidth = double.PositiveInfinity;
      columnDefinition2.MaxWidth = double.PositiveInfinity;
    }
    else
    {
      RowDefinition rowDefinition1 = obj1 as RowDefinition;
      RowDefinition rowDefinition2 = obj2 as RowDefinition;
      rowDefinition1.MaxHeight = double.PositiveInfinity;
      rowDefinition2.MaxHeight = double.PositiveInfinity;
    }
  }

  public static void FixGridRows(Grid grid, GridSplitter splitter, bool doColumns)
  {
    IList source;
    double num1;
    if (doColumns)
    {
      source = (IList) grid.ColumnDefinitions;
      num1 = grid.ActualWidth;
    }
    else
    {
      source = (IList) grid.RowDefinitions;
      num1 = grid.ActualHeight;
    }
    if (num1 == 0.0)
      return;
    int num2 = !doColumns ? (int) splitter.GetValue(Grid.RowProperty) : (int) splitter.GetValue(Grid.ColumnProperty);
    int index1 = num2 - 1;
    int index2 = num2;
    switch (splitter.ResizeBehavior)
    {
      case GridResizeBehavior.CurrentAndNext:
        index1 = num2;
        index2 = num2 + 1;
        break;
      case GridResizeBehavior.PreviousAndCurrent:
        index1 = num2 - 1;
        index2 = num2;
        break;
      case GridResizeBehavior.PreviousAndNext:
        index1 = num2 - 1;
        index2 = num2 + 1;
        break;
    }
    object cell1 = source[index1];
    object cell2 = source[index2];
    double num3 = num1 - 1.0;
    if (doColumns)
    {
      ColumnDefinition columnDefinition1 = cell1 as ColumnDefinition;
      ColumnDefinition columnDefinition2 = cell2 as ColumnDefinition;
      double num4 = num3 - source.Cast<ColumnDefinition>().Where<ColumnDefinition>((Func<ColumnDefinition, bool>) (columnDefinition_0 => columnDefinition_0 != cell1 && columnDefinition_0 != cell2)).Select<ColumnDefinition, double>((Func<ColumnDefinition, double>) (columnDefinition_0 => columnDefinition_0.ActualWidth)).Sum();
      columnDefinition1.MaxWidth = Math.Max(0.0, num4 - columnDefinition2.MinWidth);
      columnDefinition2.MaxWidth = Math.Max(0.0, num4 - columnDefinition1.MinWidth);
    }
    else
    {
      double num5 = num3 - source.Cast<RowDefinition>().Where<RowDefinition>((Func<RowDefinition, bool>) (rowDefinition_0 => rowDefinition_0 != cell1 && rowDefinition_0 != cell2)).Select<RowDefinition, double>((Func<RowDefinition, double>) (rowDefinition_0 => rowDefinition_0.ActualHeight)).Sum();
      RowDefinition rowDefinition1 = cell1 as RowDefinition;
      RowDefinition rowDefinition2 = cell2 as RowDefinition;
      rowDefinition1.MaxHeight = Math.Max(0.0, num5 - rowDefinition2.MinHeight);
      rowDefinition2.MaxHeight = Math.Max(0.0, num5 - rowDefinition1.MinHeight);
    }
  }

  public static Tuple<IEnumerable<T>, IEnumerable<T>> Split<T>(
    this IEnumerable<T> list,
    Func<T, bool> predicate)
  {
    return new Tuple<IEnumerable<T>, IEnumerable<T>>(list.Where<T>(predicate), list.Where<T>((Func<T, bool>) (gparam_0 => !predicate(gparam_0))));
  }

  public static void Each<T>(this IEnumerable<T> ienumerable_0, Action<T, int> action)
  {
    int num = 0;
    foreach (T obj in ienumerable_0)
      action(obj, num++);
  }

  public static T FindMax<T, C>(this IEnumerable<T> ienumerable, Func<T, C> selector) where C : IComparable
  {
    if (!ienumerable.Any<T>())
      return default (T);
    T max = ienumerable.FirstOrDefault<T>();
    C c1 = selector(max);
    foreach (T obj in ienumerable.Skip<T>(1))
    {
      C c2 = selector(obj);
      if (c2.CompareTo((object) c1) > 0)
      {
        max = obj;
        c1 = c2;
      }
    }
    return max;
  }

  public static T FindMin<T, C>(this IEnumerable<T> ienumerable, Func<T, C> selector) where C : IComparable
  {
    if (!ienumerable.Any<T>())
      return default (T);
    T min = ienumerable.FirstOrDefault<T>();
    C c1 = selector(min);
    foreach (T obj in ienumerable.Skip<T>(1))
    {
      C c2 = selector(obj);
      if (c2.CompareTo((object) c1) < 0)
      {
        min = obj;
        c1 = c2;
      }
    }
    return min;
  }

  private static Dispatcher smethod_0()
  {
    return Application.Current != null ? Application.Current.Dispatcher : (Dispatcher) null;
  }

  private static void smethod_1(object sender, DispatcherUnhandledExceptionEventArgs e)
  {
    if (!(e.Exception is ThreadAbortException))
      return;
    e.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
    e.Handled = true;
  }

  public static void GuiInvoke(Action action, Dispatcher dispatch = null, DispatcherPriority priority = DispatcherPriority.Normal)
  {
    try
    {
      dispatch = dispatch ?? GuiHelpers.smethod_0();
      if (dispatch == null)
      {
        try
        {
          action();
        }
        catch (InvalidOperationException ex)
        {
        }
      }
      else if (dispatch.CheckAccess())
        action();
      else
        dispatch.Invoke(action, priority);
    }
    catch (TaskCanceledException ex)
    {
      if (dispatch != null && (dispatch.HasShutdownStarted || dispatch.HasShutdownFinished))
        return;
      throw;
    }
  }

  public static void GuiInvokeAsync(
    Action action,
    Dispatcher dispatch = null,
    DispatcherPriority priority = DispatcherPriority.Normal)
  {
    dispatch = dispatch ?? GuiHelpers.smethod_0();
    if (dispatch == null)
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          action();
        }
        catch (InvalidOperationException ex)
        {
        }
      }));
    else
      dispatch.BeginInvoke((Delegate) action, priority);
  }

  public static void DeferTillSuccess<T>(
    Func<T> getter,
    Action<T> handler,
    DispatcherPriority priority = DispatcherPriority.ContextIdle,
    T failureValue = null)
  {
    T objA = getter();
    if (object.Equals((object) objA, (object) failureValue))
      GuiHelpers.GuiInvokeAsync((Action) (() => GuiHelpers.DeferTillSuccess<T>(getter, handler)), priority: priority);
    else
      handler(objA);
  }

  public static void PushDispatcherFrame()
  {
    DispatcherFrame frame = new DispatcherFrame();
    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, (Delegate) (parameter =>
    {
      frame.Continue = false;
      return (object) null;
    }), (object) null);
    Dispatcher.PushFrame(frame);
  }

  public static void WaitAsync(Action action)
  {
    Task task = Task.Run(action);
    while (!task.Wait(16 /*0x10*/))
      GuiHelpers.PushDispatcherFrame();
  }

  public static bool IsTabStop(DependencyObject dependencyObject_0)
  {
    return dependencyObject_0 is FrameworkElement frameworkElement && frameworkElement.Focusable && (bool) frameworkElement.GetValue(Control.IsTabStopProperty) && frameworkElement.IsEnabled && frameworkElement.IsVisible;
  }

  public static FrameworkElement MoveFocusTo(FrameworkElement control)
  {
    if (GuiHelpers.IsTabStop((DependencyObject) control))
    {
      control.Focus();
      return control;
    }
    FrameworkElement frameworkElement = control.GetVisualChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>(new Func<FrameworkElement, bool>(GuiHelpers.IsTabStop));
    frameworkElement?.Focus();
    return frameworkElement;
  }

  public static void OnNextEvent(
    this FrameworkElement frameworkElement_0,
    RoutedEvent routedEvent_0,
    RoutedEventHandler handler)
  {
    RoutedEventHandler routedEventHandler_0 = (RoutedEventHandler) null;
    routedEventHandler_0 = (RoutedEventHandler) ((sender, e) =>
    {
      frameworkElement_0.RemoveHandler(routedEvent_0, (Delegate) routedEventHandler_0);
      handler(sender, e);
    });
    frameworkElement_0.AddHandler(routedEvent_0, (Delegate) routedEventHandler_0);
  }

  public static void WhenLoaded(this FrameworkElement frameworkElement_0, Action loaded)
  {
    if (frameworkElement_0.IsLoaded)
      loaded();
    else
      frameworkElement_0.OnNextEvent(FrameworkElement.LoadedEvent, (RoutedEventHandler) ((sender, e) => loaded()));
  }

  public enum CallBehaviour
  {
    Blocking,
    NonBlocking,
  }
}
