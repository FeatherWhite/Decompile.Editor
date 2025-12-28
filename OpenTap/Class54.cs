// Decompiled with JetBrains decompiler
// Type: OpenTap.Class54
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;

#nullable disable
namespace OpenTap;

internal static class Class54
{
  private static readonly Class52 class52_0 = new Class52();

  internal static void smethod_0()
  {
    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Class54.class52_0.method_0);
    Class54.smethod_1();
  }

  internal static void smethod_1()
  {
    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(Class54.class52_0.method_0);
    PluginManager.SearchAsync();
  }
}
