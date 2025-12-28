// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.StepSettingsPlugin
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Gui;

[HelpLink("EditorHelp.chm::/Editor Overview/Step Settings Panel.html")]
[Display("Test Step Settings", "Step Settings Panel", null, -10000.0, false, null)]
public class StepSettingsPlugin : ITapDockPanel, ITapPlugin
{
  public double? DesiredWidth => new double?(300.0);

  public double? DesiredHeight => new double?(400.0);

  public FrameworkElement CreateElement(ITapDockContext context)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    StepSettingsPlugin.Class141 class141 = new StepSettingsPlugin.Class141();
    // ISSUE: reference to a compiler-generated field
    class141.mainWindow_0 = (MainWindow) context;
    // ISSUE: reference to a compiler-generated field
    DependencyObject parent = class141.mainWindow_0.grid_3.Parent;
    // ISSUE: reference to a compiler-generated field
    class141.mainWindow_0.grid_3.Visibility = System.Windows.Visibility.Collapsed;
    if (parent is Grid grid)
    {
      // ISSUE: reference to a compiler-generated field
      grid.Children.Remove((UIElement) class141.mainWindow_0.grid_3);
    }
    if (parent is Decorator decorator)
      decorator.Child = (UIElement) null;
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class141.method_0));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class141.mainWindow_0.Bind("GotLicense", (DependencyObject) class141.mainWindow_0.grid_3, UIElement.IsEnabledProperty);
    // ISSUE: reference to a compiler-generated field
    return (FrameworkElement) class141.mainWindow_0.grid_3;
  }
}
