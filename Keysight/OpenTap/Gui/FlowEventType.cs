// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.FlowEventType
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Flags]
public enum FlowEventType
{
  Running = 1,
  Defer = 2,
  PrePlanRun = 4,
  PostPlanRun = 8,
}
