// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.MainWindowContext
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System.Collections.Generic;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class MainWindowContext : GuiContext
{
  private MainWindow mainWindow_0;

  public override TestPlan Plan
  {
    get => this.mainWindow_0.Plan;
    set => this.mainWindow_0.Plan = value;
  }

  public override TapThread Run() => ((ITapDockContext) this.mainWindow_0).Run();

  public override TapSettings Settings => this.mainWindow_0.TapSettings;

  public virtual IList<IResultListener> ResultListeners
  {
    get => (IList<IResultListener>) ((ITapDockContext) this.mainWindow_0).ResultListeners;
  }

  public static MainWindowContext FromMainWindow(MainWindow window)
  {
    return new MainWindowContext() { mainWindow_0 = window };
  }

  private MainWindowContext()
  {
  }

  internal void method_0()
  {
  }

  internal void method_1()
  {
  }

  internal void method_2()
  {
  }

  internal void method_3()
  {
  }

  internal void method_4()
  {
  }

  public GuiContext GetSubContext() => (GuiContext) this;
}
