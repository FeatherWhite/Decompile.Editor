// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.SubGuiContext
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections.Generic;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class SubGuiContext : GuiContext, IDisposable
{
  private readonly GuiContext context;

  public SubGuiContext(GuiContext context) => this.context = context;

  public void Dispose()
  {
  }

  public override TestPlan Plan
  {
    get => this.context.Plan;
    set => this.context.Plan = value;
  }

  public override TapThread Run() => this.context.Run();

  public override TapSettings Settings => this.context.Settings;

  public virtual IList<IResultListener> ResultListeners
  {
    get => (IList<IResultListener>) this.context.ResultListeners;
  }
}
