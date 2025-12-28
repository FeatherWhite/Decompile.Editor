// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.WpfHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class WpfHelper
{
  public static T LookupParent<T>(this DependencyObject item)
  {
    for (item = VisualTreeHelper.GetParent(item); item != null; item = !(item is FrameworkElement frameworkElement) || frameworkElement.Parent == null ? (!(item is Popup popup) || popup.PlacementTarget == null ? VisualTreeHelper.GetParent(item) : (DependencyObject) popup.PlacementTarget) : frameworkElement.Parent)
    {
      if (item is T)
        return item as T;
    }
    return default (T);
  }
}
