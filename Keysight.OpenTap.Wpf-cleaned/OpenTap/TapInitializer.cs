// Decompiled with JetBrains decompiler
// Type: OpenTap.TapInitializer
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;

#nullable disable
namespace OpenTap;

internal static class TapInitializer
{
  private static readonly SimpleTapAssemblyResolver simpleTapAssemblyResolver_0 = new SimpleTapAssemblyResolver();

  internal static void Initialize()
  {
    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(TapInitializer.simpleTapAssemblyResolver_0.Resolve);
    TapInitializer.ContinueInitialization();
  }

  internal static void ContinueInitialization()
  {
    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(TapInitializer.simpleTapAssemblyResolver_0.Resolve);
    PluginManager.SearchAsync();
  }
}
