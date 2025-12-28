// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.StepExplorerPlugin
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

[HelpLink("EditorHelp.chm::/CreatingATestPlan/Working with Test Steps/Adding a New Step.html")]
[Display("Test Steps", "Shows the available steps.", null, -10000.0, false, null)]
public class StepExplorerPlugin : ITapDockPanel, ITapPlugin
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("StepExplorer");

  public double? DesiredWidth => new double?(300.0);

  public double? DesiredHeight => new double?(400.0);

  public FrameworkElement CreateElement(ITapDockContext context)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    StepExplorerPlugin.Class139 class139_1 = new StepExplorerPlugin.Class139();
    // ISSUE: reference to a compiler-generated field
    class139_1.mainWindow_0 = (MainWindow) context;
    // ISSUE: variable of a compiler-generated type
    StepExplorerPlugin.Class139 class139_2 = class139_1;
    NewStepControl newStepControl = new NewStepControl((Type) typeof (ITestStep));
    newStepControl.Margin = new Thickness(0.0, 0.0, 0.0, 2.0);
    // ISSUE: reference to a compiler-generated field
    class139_2.newStepControl_0 = newStepControl;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class139_1.mainWindow_0.Bind("GotLicense", (DependencyObject) class139_1.newStepControl_0, UIElement.IsEnabledProperty);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class139_1.mainWindow_0.testPlanGrid_0.Bind("SelectedTestStep", (DependencyObject) class139_1.newStepControl_0, NewStepControl.SelectedStepProperty);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class139_1.newStepControl_0.CanExecuteAdd = (Func<bool>) new Func<bool>(class139_1.method_2);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class139_1.newStepControl_0.OnAddStep += (Action<ITypeData>) new Action<ITypeData>(class139_1.method_3);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class139_1.newStepControl_0.RaiseDragDropEvent += (EventHandler) new EventHandler(class139_1.method_0);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class139_1.newStepControl_0.OnAddChildStep += (Action<ITypeData>) new Action<ITypeData>(class139_1.method_4);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class139_1.mainWindow_0.PluginsChanged += new EventHandler(class139_1.method_5);
    // ISSUE: reference to a compiler-generated field
    return (FrameworkElement) class139_1.newStepControl_0;
  }
}
