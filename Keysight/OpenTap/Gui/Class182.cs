// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class182
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class182 : IDisposable
{
  private Action action_0;

  public Class182(Action action_1) => this.action_0 = action_1;

  public void Dispose() => this.action_0();
}
