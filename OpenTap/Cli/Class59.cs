// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.Class59
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace OpenTap.Cli;

internal class Class59 : Dictionary<string, Class58>
{
  [CompilerGenerated]
  [SpecialName]
  public string[] method_0() => this.string_0;

  [CompilerGenerated]
  [SpecialName]
  public void method_1(string[] string_1) => this.string_0 = string_1;

  [CompilerGenerated]
  [SpecialName]
  public List<string> method_2() => this.list_0;

  [CompilerGenerated]
  [SpecialName]
  public void method_3(List<string> list_2) => this.list_0 = list_2;

  [CompilerGenerated]
  [SpecialName]
  public List<Class58> method_4() => this.list_1;

  [CompilerGenerated]
  [SpecialName]
  public void method_5(List<Class58> list_2) => this.list_1 = list_2;

  public Class59()
  {
    this.method_1(new string[0]);
    this.method_3(new List<string>());
    this.method_5(new List<Class58>());
  }

  public Class58 method_6(Class58 class58_0)
  {
    this[class58_0.method_2()] = class58_0;
    return class58_0;
  }

  public Class58 method_7(
    string string_1,
    char char_0 = '\0',
    bool bool_0 = true,
    string string_2 = "",
    string string_3 = null)
  {
    return this.method_6(new Class58(string_1, char_0, bool_0, string_2, string_3));
  }

  public bool method_8(string string_1) => this.ContainsKey(string_1);

  public Class58 method_9(string string_1)
  {
    return this.method_8(string_1) ? this[string_1] : (Class58) null;
  }

  public string method_10(string string_1) => this[string_1].method_6();

  public string method_11(string string_1, string string_2 = null)
  {
    Class58 class58 = this.method_9(string_1);
    return class58 != null ? class58.method_6() : string_2;
  }

  public IEnumerable<string> method_12(string string_1)
  {
    Class58 class58;
    return this.TryGetValue(string_1, out class58) ? (IEnumerable<string>) class58.method_7() : (IEnumerable<string>) Array.Empty<string>();
  }

  public Class58 method_13(string string_1, Class59 class59_0)
  {
    if (!class59_0.method_8(string_1))
      return (Class58) null;
    this[string_1] = class59_0[string_1].method_14();
    return this[string_1];
  }

  public Class59 method_14()
  {
    Class59 class59_1 = new Class59();
    class59_1.method_1(this.method_0());
    Class59 class59_2 = class59_1;
    foreach (string key in this.Keys)
    {
      if (this[key].method_6() != null)
        class59_2[key] = this[key];
    }
    return class59_2;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, Class58> keyValuePair in (Dictionary<string, Class58>) this)
    {
      Class58 class58 = keyValuePair.Value;
      string str1 = "--" + class58.method_2();
      if (class58.method_0() != char.MinValue)
        str1 = $"-{class58.method_0()}, {str1}";
      string str2 = "  " + str1;
      string str3 = class58.method_9();
      char[] chArray = new char[1]{ '\n' };
      foreach (string str4 in str3.Split(chArray))
      {
        string str5 = str2 + new string(' ', 22 - str2.Length) + str4;
        stringBuilder.AppendLine(str5);
        str2 = "";
      }
    }
    return stringBuilder.ToString();
  }
}
