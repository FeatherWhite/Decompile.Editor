// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.AeroTheme
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using AvalonDock.Themes;
using System;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class AeroTheme : Theme
{
  public static readonly DependencyProperty OverrideTitleProperty = DependencyProperty.RegisterAttached("OverrideTitle", typeof (FrameworkElement), typeof (AeroTheme), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));

  public static void SetOverrideTitle(UIElement element, FrameworkElement value)
  {
    element.SetValue(AeroTheme.OverrideTitleProperty, (object) value);
  }

  public static FrameworkElement GetOverrideTitle(UIElement element)
  {
    return (FrameworkElement) element.GetValue(AeroTheme.OverrideTitleProperty);
  }

  public override Uri GetResourceUri()
  {
    return new Uri("/Editor;component/AvalonDockTheme/AvalonDockTheme.xaml", UriKind.Relative);
  }
}
