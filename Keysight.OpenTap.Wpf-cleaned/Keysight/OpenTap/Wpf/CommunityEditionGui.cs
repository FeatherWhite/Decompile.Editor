// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CommunityEditionGui
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using OpenTap.Cli;
using System;
using System.Linq;
using System.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class CommunityEditionGui
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("CE");

  public static void AcceptedEulaOrExit()
  {
    if (CommunityEdition.CheckEulaAccept())
      return;
    try
    {
      TypeData typeData = TypeData.FromType(typeof (ICliAction)).DerivedTypes.FirstOrDefault<TypeData>((Func<TypeData, bool>) (typeData_0 => typeData_0.Display?.Name == "communityeditionpopup"));
      if (!((typeData != null ? ReflectionDataExtensions.CreateInstance((ITypeData) typeData) : (object) null) is ICliAction instance))
        return;
      instance.Execute(CancellationToken.None);
    }
    catch (Exception ex)
    {
      Log.Error(CommunityEditionGui.traceSource_0, "Unable to show EULA dialog.", Array.Empty<object>());
      Log.Debug(CommunityEditionGui.traceSource_0, ex);
    }
    finally
    {
      if (!CommunityEdition.CheckEulaAccept())
      {
        Log.Error(CommunityEditionGui.traceSource_0, "Community Edition EULA not accepted. Exiting.", Array.Empty<object>());
        Environment.Exit(0);
      }
      Log.Debug(CommunityEditionGui.traceSource_0, "Community Edition EULA accepted.", Array.Empty<object>());
    }
  }
}
