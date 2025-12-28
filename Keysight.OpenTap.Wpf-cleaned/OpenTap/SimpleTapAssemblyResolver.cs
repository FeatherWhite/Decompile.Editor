// Decompiled with JetBrains decompiler
// Type: OpenTap.SimpleTapAssemblyResolver
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OpenTap;

internal class SimpleTapAssemblyResolver
{
  private Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();

  public SimpleTapAssemblyResolver()
  {
    foreach (string path in PathUtils.IterateDirectories(Environment.GetEnvironmentVariable("OPENTAP_INIT_DIRECTORY") ?? Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>) (string_0 => string_0.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) || string_0.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))).ToList<string>())
      this.dictionary_0[Path.GetFileNameWithoutExtension(path).ToLower()] = path;
  }

  internal Assembly Resolve(object sender, ResolveEventArgs args)
  {
    if (args.Name.Contains(".resources"))
      return (Assembly) null;
    string lower = args.Name.Split(',')[0].ToLower();
    string assemblyFile;
    if (this.dictionary_0.TryGetValue(lower, out assemblyFile))
      return Assembly.LoadFrom(assemblyFile);
    Console.Error.WriteLine($"Asked to resolve {lower}, but couldn't");
    return (Assembly) null;
  }
}
