// Decompiled with JetBrains decompiler
// Type: OpenTap.Class4
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.IO;
using System.Runtime.CompilerServices;

#nullable disable
namespace OpenTap;

internal class Class4
{
  public static readonly Class4 class4_0 = new Class4("Windows");
  public static readonly Class4 class4_1 = new Class4("Linux");
  public static readonly Class4 class4_2 = new Class4("Unsupported");
  private static Class4 class4_3;

  public override string ToString() => this.method_0();

  [CompilerGenerated]
  [SpecialName]
  public string method_0() => this.string_0;

  private Class4(string string_1) => this.string_0 = string_1;

  private static Class4 smethod_0()
  {
    if (Path.DirectorySeparatorChar == '\\')
      return Class4.class4_0;
    return Directory.Exists("/proc/") ? Class4.class4_1 : Class4.class4_2;
  }

  [SpecialName]
  public static Class4 smethod_1()
  {
    if (Class4.class4_3 == null)
      Class4.class4_3 = Class4.smethod_0();
    return Class4.class4_3;
  }
}
