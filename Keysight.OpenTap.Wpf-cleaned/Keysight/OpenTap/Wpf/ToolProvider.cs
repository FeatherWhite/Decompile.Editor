// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ToolProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public abstract class ToolProvider : ITapPlugin
{
  private static TraceSource traceSource_0 = Log.CreateSource(nameof (ToolProvider));

  public abstract string FileName { get; }

  public virtual DisplayAttribute GetDisplay() => this.GetType().GetDisplayAttribute();

  public abstract bool Execute();

  public static IEnumerable<ToolProvider> GetToolProviders()
  {
    IEnumerator<Type> enumerator = PluginManager.GetPlugins<ToolProvider>().GetEnumerator();
    while (enumerator.MoveNext())
    {
      Type current = enumerator.Current;
      ToolProvider toolProvider;
      try
      {
        toolProvider = (ToolProvider) current.CreateInstance();
      }
      catch (Exception ex)
      {
        if (ToolProvider.traceSource_0.ErrorOnce((object) current, "Unable to create instance of type '{0}'", (object) ex))
          Log.Debug(ToolProvider.traceSource_0, ex);
        toolProvider = (ToolProvider) null;
      }
      if (toolProvider != null)
        yield return toolProvider;
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator = (IEnumerator<Type>) null;
  }
}
