// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestPlanPlugin
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

[Display("Test Plan", "Test Plan Panel", null, -10000.0, false, null)]
[HelpLink("EditorHelp.chm::/Editor Overview/Test Plan Panel/Readme.html")]
public class TestPlanPlugin : ITapDockPanel, ITapPlugin
{
  public double? DesiredWidth => new double?(300.0);

  public double? DesiredHeight => new double?(400.0);

  public TestPlanPlugin() => Class79.smethod_0();

  public FrameworkElement CreateElement(ITapDockContext context)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanPlugin.Class186 class186 = new TestPlanPlugin.Class186();
    // ISSUE: reference to a compiler-generated field
    class186.mainWindow_0 = (MainWindow) context;
    // ISSUE: reference to a compiler-generated field
    DependencyObject parent = class186.mainWindow_0.grid_2.Parent;
    // ISSUE: reference to a compiler-generated field
    class186.mainWindow_0.grid_2.Visibility = System.Windows.Visibility.Collapsed;
    if (parent is Grid grid)
    {
      // ISSUE: reference to a compiler-generated field
      grid.Children.Remove((UIElement) class186.mainWindow_0.grid_2);
    }
    if (parent is Decorator decorator)
      decorator.Child = (UIElement) null;
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class186.method_0));
    // ISSUE: reference to a compiler-generated field
    return (FrameworkElement) class186.mainWindow_0.grid_2;
  }
}
