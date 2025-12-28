// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ControlProvider
{
  public static readonly DependencyProperty StepEnabledProperty = DependencyProperty.RegisterAttached("StepEnabled", typeof (bool?), typeof (ControlProvider), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty SingleLineView = DependencyProperty.RegisterAttached("SingleLine", typeof (bool), typeof (ControlProvider), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));

  public static bool? GetStepEnabled(DependencyObject dependencyObject_0)
  {
    return (bool?) dependencyObject_0.GetValue(ControlProvider.StepEnabledProperty);
  }

  public static void SetStepEnabled(DependencyObject dependencyObject_0, bool? value)
  {
    dependencyObject_0.SetValue(ControlProvider.StepEnabledProperty, (object) value);
  }

  public static bool GetSingleLineView(DependencyObject dependencyObject_0)
  {
    return (bool) dependencyObject_0.GetValue(ControlProvider.SingleLineView);
  }

  public static void SetSingleLineView(DependencyObject dependencyObject_0, bool value)
  {
    dependencyObject_0.SetValue(ControlProvider.SingleLineView, (object) value);
  }
}
