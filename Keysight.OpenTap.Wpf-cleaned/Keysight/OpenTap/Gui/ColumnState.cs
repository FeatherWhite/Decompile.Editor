// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnState
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ColumnState
{
  public string Name { get; set; }

  public bool IsVisible { get; set; }

  public double Priority { get; set; }

  public double OriginalPriority { get; set; }

  public int? Width { get; set; }

  public bool ManuallyConfigured { get; set; }

  public ColumnState(string name, double originalPriority)
    : this()
  {
    this.Name = name;
    this.OriginalPriority = originalPriority;
    this.Priority = originalPriority;
  }

  public ColumnState() => this.IsVisible = true;
}
