// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.DataGridExtensions
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public static class DataGridExtensions
{
  internal static MethodInfo methodInfo_0 = typeof (DataGrid).GetMethod("BeginUpdateSelectedItems", BindingFlags.Instance | BindingFlags.NonPublic);
  internal static MethodInfo methodInfo_1 = typeof (DataGrid).GetMethod("EndUpdateSelectedItems", BindingFlags.Instance | BindingFlags.NonPublic);

  public static void SetSelectedItems(this DataGrid grid, IEnumerable items)
  {
    DataGridExtensions.methodInfo_0.Invoke((object) grid, Array.Empty<object>());
    grid.SelectedItems.Clear();
    foreach (object obj in items)
      grid.SelectedItems.Add(obj);
    DataGridExtensions.methodInfo_1.Invoke((object) grid, Array.Empty<object>());
  }

  public static T GetFirstVisualChild<T>(DependencyObject depObj) where T : DependencyObject
  {
    if (depObj != null)
    {
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(depObj); ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(depObj, childIndex);
        if (child != null && child is T firstVisualChild1)
          return firstVisualChild1;
        T firstVisualChild2 = DataGridExtensions.GetFirstVisualChild<T>(child);
        if ((object) firstVisualChild2 != null)
          return firstVisualChild2;
      }
    }
    return default (T);
  }
}
