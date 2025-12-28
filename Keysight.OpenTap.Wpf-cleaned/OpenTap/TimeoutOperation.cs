// Decompiled with JetBrains decompiler
// Type: OpenTap.TimeoutOperation
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Threading;

#nullable disable
namespace OpenTap;

internal class TimeoutOperation : IDisposable
{
  private Action action_0;
  private TimeSpan timeSpan_0;
  private static readonly TimeSpan timeSpan_1 = TimeSpan.FromSeconds(2.0);
  private CancellationTokenSource cancellationTokenSource_0;
  private bool bool_0;

  private TimeoutOperation(TimeSpan timeSpan_2, Action action_1)
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

  public static TimeoutOperation Create(TimeSpan timeout, Action actionOnTimeout)
  {
    TimeoutOperation timeoutOperation = new TimeoutOperation(timeout, actionOnTimeout);
    TapThread.Start(new Action(timeoutOperation.method_0), "Timeout");
    return timeoutOperation;
  }

  public static TimeoutOperation Create(Action actionOnTimeout)
  {
    return TimeoutOperation.Create(TimeoutOperation.timeSpan_1, actionOnTimeout);
  }

  public void Cancel()
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

  public void Dispose() => this.Cancel();
}
