// Decompiled with JetBrains decompiler
// Type: OpenTap.Class52
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OpenTap;

internal class Class52
{
  private Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();

  public Class52()
  {
    foreach (string path in Class5.smethod_4(Environment.GetEnvironmentVariable("OPENTAP_INIT_DIRECTORY") ?? Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>) (string_0 => string_0.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) || string_0.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))).ToList<string>())
      this.dictionary_0[Path.GetFileNameWithoutExtension(path).ToLower()] = path;
  }

  internal Assembly method_0(object object_0, ResolveEventArgs resolveEventArgs_0)
  {
    if (resolveEventArgs_0.Name.Contains(".resources"))
      return (Assembly) null;
    string lower = resolveEventArgs_0.Name.Split(',')[0].ToLower();
    string assemblyFile;
    if (this.dictionary_0.TryGetValue(lower, out assemblyFile))
      return Assembly.LoadFrom(assemblyFile);
    Console.Error.WriteLine($"Asked to resolve {lower}, but couldn't");
    return (Assembly) null;
  }
}
