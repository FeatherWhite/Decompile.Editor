// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GuiContext
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public abstract class GuiContext : ITapDockContext
{
  public virtual TapThread Run() => throw new NotImplementedException();

  public virtual TestPlan Plan { get; set; }

  public virtual TapSettings Settings { get; }

  public virtual IList<IResultListener> ResultListeners { get; }

  List<IResultListener> ITapDockContext.ResultListeners
  {
    get => this.ResultListeners.ToList<IResultListener>();
  }

  internal virtual event EventHandler OnRunStart;

  internal virtual event EventHandler OnRunCompleted;

  internal virtual event EventHandler OnConnect;

  internal virtual event EventHandler OnDisconnect;

  internal virtual event EventHandler OnAbort;

  internal virtual void RaiseOnRunStart()
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler0 = this.eventHandler_0;
    if (eventHandler0 == null)
      return;
    eventHandler0((object) this, EventArgs.Empty);
  }

  internal virtual void RaiseOnRunCompleted()
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler1 = this.eventHandler_1;
    if (eventHandler1 == null)
      return;
    eventHandler1((object) this, EventArgs.Empty);
  }

  internal virtual void RaiseOnConnect()
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler2 = this.eventHandler_2;
    if (eventHandler2 == null)
      return;
    eventHandler2((object) this, EventArgs.Empty);
  }

  internal virtual void RaiseOnDisconnect()
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler3 = this.eventHandler_3;
    if (eventHandler3 == null)
      return;
    eventHandler3((object) this, EventArgs.Empty);
  }

  internal virtual void RaiseOnAbort()
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler4 = this.eventHandler_4;
    if (eventHandler4 == null)
      return;
    eventHandler4((object) this, EventArgs.Empty);
  }
}
