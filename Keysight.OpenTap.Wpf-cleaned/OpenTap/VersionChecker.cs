// Decompiled with JetBrains decompiler
// Type: OpenTap.VersionChecker
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Diagnostics;

#nullable disable
namespace OpenTap;

internal static class VersionChecker
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("OpenTAP");

  internal static void EmitVersion()
  {
    string str = Environment.Is64BitProcess ? "64-bit" : "32-bit";
    SemanticVersion semanticVersion = PluginManager.GetOpenTapAssembly().SemanticVersion;
    TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
    Log.Info(VersionChecker.traceSource_0, timeSpan, $"OpenTAP version '{semanticVersion}' {str} initialized {DateTime.Now:MM/dd/yyyy}", Array.Empty<object>());
  }
}
