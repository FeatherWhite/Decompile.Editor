// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ItemDescription
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ItemDescription
{
  public Func<string> Description { get; set; }

  public Func<string> ValueDescription { get; set; }

  public override string ToString()
  {
    Func<string> description = this.Description;
    string str1 = description != null ? description() : (string) null;
    if (str1 != null)
    {
      Func<string> valueDescription = this.ValueDescription;
      string str2 = valueDescription != null ? valueDescription() : (string) null;
      return str2 != null ? $"{str1}\n{str2}" : str1;
    }
    Func<string> valueDescription1 = this.ValueDescription;
    return (valueDescription1 != null ? valueDescription1() : (string) null) ?? (string) null;
  }
}
