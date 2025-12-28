// Decompiled with JetBrains decompiler
// Type: OpenTap.Class48
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Threading;

#nullable disable
namespace OpenTap;

internal class Class48 : IDisposable
{
  private Action action_0;
  private TimeSpan timeSpan_0;
  private static readonly TimeSpan timeSpan_1 = TimeSpan.FromSeconds(2.0);
  private CancellationTokenSource cancellationTokenSource_0;
  private bool bool_0;

  private Class48(TimeSpan timeSpan_2, Action action_1)
  {
    this.timeSpan_0 = timeSpan_2;
    this.action_0 = action_1;
    this.cancellationTokenSource_0 = new CancellationTokenSource(timeSpan_2);
  }

  private void method_0()
  {
    try
    {
      if (this.cancellationTokenSource_0.IsCancellationRequested)
        return;
      if (258 != WaitHandle.WaitAny(new WaitHandle[2]
      {
        this.cancellationTokenSource_0.Token.WaitHandle,
        TapThread.Current.AbortToken.WaitHandle
      }, this.timeSpan_0))
        return;
      this.action_0();
    }
    finally
    {
      this.cancellationTokenSource_0.Dispose();
      this.bool_0 = true;
    }
  }

  public static Class48 smethod_0(TimeSpan timeSpan_2, Action action_1)
  {
    Class48 class48 = new Class48(timeSpan_2, action_1);
    TapThread.Start(new Action(class48.method_0), "Timeout");
    return class48;
  }

  public static Class48 smethod_1(Action action_1)
  {
    return Class48.smethod_0(Class48.timeSpan_1, action_1);
  }

  public void method_1()
  {
    try
    {
      if (this.bool_0)
        return;
      this.cancellationTokenSource_0.Cancel();
    }
    catch (ObjectDisposedException ex)
    {
    }
  }

  public void Dispose() => this.method_1();
}
