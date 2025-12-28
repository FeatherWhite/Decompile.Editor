// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.NotifyQueue
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class NotifyQueue
{
  private readonly WorkQueue workQueue_0;
  private bool bool_0;
  private readonly object object_0 = new object();
  private readonly Action process;

  public NotifyQueue(Action process)
  {
    this.workQueue_0 = new WorkQueue((WorkQueue.Options) 0, "NotifyChanged");
    this.process = process;
  }

  public void NotifyChanged(object object_1, string property)
  {
    if (!UpdateMonitor.GetIsConnected(object_1) || this.bool_0)
      return;
    lock (this.object_0)
    {
      if (this.bool_0)
        return;
      this.bool_0 = true;
      this.workQueue_0.EnqueueWork((Action) (() =>
      {
        try
        {
          TapThread.Sleep(20);
        }
        finally
        {
          lock (this.object_0)
            this.bool_0 = false;
        }
        GuiHelpers.GuiInvoke(this.process);
      }));
    }
  }
}
