// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.Class56
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap.Cli;

internal class Class56
{
  public Class59 class59_0 = new Class59();

  private Class56.Struct7 method_0(string string_0, Class59 class59_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Class56.Class57 class57 = new Class56.Class57();
    // ISSUE: reference to a compiler-generated field
    class57.string_0 = string_0;
    Class56.Struct7 struct7 = new Class56.Struct7()
    {
      bool_0 = false
    };
    int startIndex = 0;
    // ISSUE: reference to a compiler-generated field
    if (class57.string_0.StartsWith("--"))
    {
      startIndex = 2;
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (class57.string_0.StartsWith("-"))
        startIndex = 1;
    }
    if (startIndex == 0)
      return struct7;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class57.string_0 = class57.string_0.Substring(startIndex);
    // ISSUE: reference to a compiler-generated field
    int length = class57.string_0.IndexOf('=');
    if (length > 0)
    {
      // ISSUE: reference to a compiler-generated field
      struct7.string_0 = class57.string_0.Substring(length + 1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class57.string_0 = class57.string_0.Substring(0, length);
    }
    // ISSUE: reference to a compiler-generated method
    struct7.class58_0 = class59_1.Where<KeyValuePair<string, Class58>>(new Func<KeyValuePair<string, Class58>, bool>(class57.method_0)).FirstOrDefault<KeyValuePair<string, Class58>>().Value;
    if (struct7.class58_0 == null)
    {
      // ISSUE: reference to a compiler-generated method
      struct7.class58_0 = this.class59_0.Where<KeyValuePair<string, Class58>>(new Func<KeyValuePair<string, Class58>, bool>(class57.method_1)).FirstOrDefault<KeyValuePair<string, Class58>>().Value;
    }
    if (struct7.class58_0 == null)
      struct7.bool_0 = true;
    else
      struct7.class58_0 = struct7.class58_0.method_14();
    return struct7;
  }

  public Class59 method_1(string[] string_0)
  {
    Class59 class59_1 = this.class59_0.method_14();
    List<string> list = ((IEnumerable<string>) class59_1.method_0()).ToList<string>();
    for (int index = 0; index < string_0.Length; ++index)
    {
      string string_0_1 = string_0[index];
      Class56.Struct7 struct7 = this.method_0(string_0_1, class59_1);
      Class58 class580 = struct7.class58_0;
      if (class580 == null)
      {
        if (!struct7.bool_0)
          list.Add(string_0_1);
        else
          class59_1.method_2().Add(string_0_1);
      }
      else
      {
        if (class580.method_4())
        {
          if (struct7.string_0 != null)
            class580.method_7().Add(struct7.string_0);
          else if (index + 1 < string_0.Length)
          {
            class580.method_7().Add(string_0[++index]);
          }
          else
          {
            class59_1.method_4().Add(class580);
            continue;
          }
        }
        else if (struct7.string_0 != null)
          class580.method_7().Add(struct7.string_0);
        class59_1.method_6(class580);
      }
    }
    class59_1.method_1(list.ToArray());
    return class59_1;
  }

  private struct Struct7
  {
    public Class58 class58_0;
    public bool bool_0;
    public string string_0;
  }
}
