// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ListSettingsControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("List", null, null, 10.0, false, null)]
public class ListSettingsControlProvider : ISettingsControlProvider
{
  public Type GetControlType(Type settingsType)
  {
    if (!settingsType.HasInterface<INotifyCollectionChanged>())
      return (Type) null;
    ComponentSettingsLayoutAttribute attribute = ReflectionDataExtensions.GetAttribute<ComponentSettingsLayoutAttribute>((IReflectionData) TypeData.FromType(settingsType));
    return attribute != null && attribute.Mode == 1 ? (Type) null : typeof (ListSettingsControl);
  }
}
