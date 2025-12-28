// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.WpfErrorTraceListener
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.Diagnostics;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class WpfErrorTraceListener : DefaultTraceListener
{
  private static TraceSource traceSource_0 = Log.CreateSource("WPF");

  public override void TraceEvent(
    TraceEventCache eventCache,
    string source,
    TraceEventType eventType,
    int int_0,
    string format,
    params object[] args)
  {
    Log.Debug(WpfErrorTraceListener.traceSource_0, format, args);
  }
}
