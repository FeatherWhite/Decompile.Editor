// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AutoCompleteItem
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AutoCompleteItem
{
  public string Value { get; set; }

  public string FullReplacement { get; set; }

  public int NewCaretPosition { get; set; }

  public AutoCompleteItem()
  {
  }

  public AutoCompleteItem(string simple)
  {
    this.Value = simple;
    this.FullReplacement = simple;
    this.NewCaretPosition = simple.Length;
  }
}
