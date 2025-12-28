// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ColumnHeaderItem
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ColumnHeaderItem
{
  public string Header { get; set; }

  public string Description { get; set; }

  public ColumnHeaderToolTip ToolTip
  {
    get => new ColumnHeaderToolTip(this.FullName ?? this.Header, this.Description);
  }

  public string FullName { get; set; }
}
