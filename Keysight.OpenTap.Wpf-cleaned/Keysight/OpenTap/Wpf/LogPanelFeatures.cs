// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LogPanelFeatures
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Flags]
public enum LogPanelFeatures
{
  Levels = 1,
  Filter = 2,
  AutoScroll = 4,
  Search = 8,
  TimingAnalyzerInterop = 16, // 0x00000010
  Clear = 32, // 0x00000020
  SessionLog = 64, // 0x00000040
  Sources = 128, // 0x00000080
}
