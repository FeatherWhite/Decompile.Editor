// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class134
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System.Runtime.CompilerServices;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class134
{
  private readonly ITapDockPanel itapDockPanel_0;
  private readonly ITapDockContext2 itapDockContext2_0;

  public Class134(ITapDockPanel itapDockPanel_1, ITapDockContext2 itapDockContext2_1)
  {
    this.itapDockPanel_0 = itapDockPanel_1;
    this.itapDockContext2_0 = itapDockContext2_1;
  }

  public FrameworkElement method_0(GuiContext guiContext_0)
  {
    return this.itapDockPanel_0.CreateElement((ITapDockContext) this.itapDockContext2_0);
  }

  [SpecialName]
  public double? method_1() => (double?) this.itapDockPanel_0.DesiredWidth;

  [SpecialName]
  public double? method_2() => (double?) this.itapDockPanel_0.DesiredHeight;

  [SpecialName]
  public bool method_3() => false;
}
