// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.Class58
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace OpenTap.Cli;

internal class Class58
{
  [CompilerGenerated]
  private bool bool_1 = true;

  [CompilerGenerated]
  [SpecialName]
  public char method_0() => this.char_0;

  [CompilerGenerated]
  [SpecialName]
  public string method_2() => this.string_0;

  [CompilerGenerated]
  [SpecialName]
  public bool method_4() => this.bool_0;

  [SpecialName]
  public string method_6() => this.method_7().FirstOrDefault<string>();

  [CompilerGenerated]
  [SpecialName]
  public List<string> method_7() => this.list_0;

  [CompilerGenerated]
  [SpecialName]
  public string method_9() => this.string_1;

  [CompilerGenerated]
  [SpecialName]
  public void method_10(string string_2) => this.string_1 = string_2;

  [CompilerGenerated]
  [SpecialName]
  public bool method_11() => this.bool_1;

  [CompilerGenerated]
  [SpecialName]
  public void method_12(bool bool_2) => this.bool_1 = bool_2;

  public Class58(string string_2, char char_1 = '\0', bool bool_2 = true, string string_3 = "", string string_4 = null)
  {
    // ISSUE: reference to a compiler-generated method
    this.method_1(char_1);
    // ISSUE: reference to a compiler-generated method
    this.method_3(string_2);
    // ISSUE: reference to a compiler-generated method
    this.method_5(bool_2);
    this.method_10(string_3);
    // ISSUE: reference to a compiler-generated method
    this.method_8(new List<string>());
    if (string.IsNullOrWhiteSpace(string_4))
      return;
    this.method_7().Add(string_4);
  }

  public Class58 method_13(string string_2)
  {
    Class58 class58 = this.method_14();
    // ISSUE: reference to a compiler-generated method
    class58.method_8(new List<string>() { string_2 });
    return class58;
  }

  public Class58 method_14()
  {
    Class58 class58 = new Class58(this.method_2(), this.method_0(), this.method_4());
    // ISSUE: reference to a compiler-generated method
    class58.method_8(new List<string>((IEnumerable<string>) this.method_7()));
    class58.method_10(this.method_9());
    return class58;
  }

  public bool method_15(string string_2)
  {
    return string_2.Length == 1 && this.method_0() != char.MinValue ? (int) string_2[0] == (int) this.method_0() : string_2 == this.method_2();
  }
}
