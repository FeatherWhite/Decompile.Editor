// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GridSettingsControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Grid", null, null, 20.0, false, null)]
public class GridSettingsControlProvider : ISettingsControlProvider
{
  public Type GetControlType(Type settingsType)
  {
    if (settingsType.HasInterface<INotifyCollectionChanged>())
    {
      ComponentSettingsLayoutAttribute attribute = ReflectionDataExtensions.GetAttribute<ComponentSettingsLayoutAttribute>((IReflectionData) TypeData.FromType(settingsType));
      if (attribute != null && attribute.Mode == 1)
        return typeof (GridSettingsControl);
    }
    return (Type) null;
  }
}
