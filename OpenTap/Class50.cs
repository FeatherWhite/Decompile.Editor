// Decompiled with JetBrains decompiler
// Type: OpenTap.Class50
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Diagnostics;

#nullable disable
namespace OpenTap;

internal static class Class50
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("OpenTAP");

  internal static void smethod_0()
  {
    string str = Environment.Is64BitProcess ? "64-bit" : "32-bit";
    SemanticVersion semanticVersion = PluginManager.GetOpenTapAssembly().SemanticVersion;
    TimeSpan elapsed = DateTime.Now - Process.GetCurrentProcess().StartTime;
    Class50.traceSource_0.Info(elapsed, $"OpenTAP version '{semanticVersion}' {str} initialized {DateTime.Now:MM/dd/yyyy}");
  }
}
