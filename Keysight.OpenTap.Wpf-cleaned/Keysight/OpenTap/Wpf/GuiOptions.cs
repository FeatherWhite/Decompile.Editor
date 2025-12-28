// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GuiOptions
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class GuiOptions : IAnnotation
{
  public bool FullRow { get; set; }

  public bool FloatBottom { get; set; }

  public bool OverridesReadOnly { get; set; }

  public bool GridMode { get; set; }

  public int RowHeight { get; set; } = 1;

  public int MaxRowHeight { get; set; } = 1000;

  public GuiOptions(LayoutAttribute layout = null)
  {
    if (layout == null)
      return;
    this.RowHeight = layout.RowHeight;
    this.FullRow = ((Enum) (object) layout.Mode).HasFlag((Enum) (object) (LayoutMode) 2);
    this.FloatBottom = ((Enum) (object) layout.Mode).HasFlag((Enum) (object) (LayoutMode) 4);
    this.MaxRowHeight = layout.MaxRowHeight;
  }
}
