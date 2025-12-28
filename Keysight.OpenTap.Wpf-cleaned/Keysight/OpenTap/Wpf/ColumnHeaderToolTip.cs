// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ColumnHeaderToolTip
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ColumnHeaderToolTip
{
  public string Name { get; }

  public string Description { get; }

  public ColumnHeaderToolTip(string name, string description)
  {
    this.Name = name ?? "";
    this.Description = description ?? "";
  }
}
