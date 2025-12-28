// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CallbackBindings
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class CallbackBindings
{
  public static readonly DependencyProperty CallbacksProperty = DependencyProperty.RegisterAttached("Callbacks", typeof (CallbackCollection), typeof (CallbackBindings));

  public static CallbackCollection GetCallbacks(DependencyObject dependencyObject_0)
  {
    return (CallbackCollection) dependencyObject_0.GetValue(CallbackBindings.CallbacksProperty);
  }

  public static void SetCallbacks(DependencyObject dependencyObject_0, object value)
  {
    dependencyObject_0.SetValue(CallbackBindings.CallbacksProperty, value);
  }
}
