// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NormalSettingsControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Normal", null, null, 0.0, false, null)]
public class NormalSettingsControlProvider : ISettingsControlProvider
{
  public Type GetControlType(Type settingsType)
  {
    return GenericGui.GetReflectionDataFromType(settingsType).Where<MemberInfo>(new Func<MemberInfo, bool>(GenericGui.FilterDefault)).Count<MemberInfo>() == 0 ? (Type) null : typeof (NormalSettingsControl);
  }
}
