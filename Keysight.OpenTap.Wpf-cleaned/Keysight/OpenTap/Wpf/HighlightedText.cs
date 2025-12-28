// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.HighlightedText
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class HighlightedText : INotifyPropertyChanged
{
  public string Text { get; set; }

  public bool Centered { get; set; }

  public event PropertyChangedEventHandler PropertyChanged;

  public bool IsDefault { get; set; }

  private void method_0(string string_1)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_1));
  }
}
