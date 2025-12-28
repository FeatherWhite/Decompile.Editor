// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MenuUi
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MenuUi : ItemUi
{
  [ThreadStatic]
  internal static ItemUi LoadingCurrent;
  public bool OverrideReload;
  public readonly ItemUi RootItem;

  public string IconName
  {
    get => this.Annotation.Get<IconAnnotationAttribute>(false, (object) null)?.IconName;
  }

  public void InvokeCommand()
  {
    this.Annotation.Get<IMethodAnnotation>(false, (object) null).Invoke();
    this.RootItem?.Annotation.Get<UpdateMonitor>(true, (object) null)?.CommandHandled(this.IconName, this.RootItem.Annotation);
  }

  public MenuUi(AnnotationCollection annotation, ItemUi item)
    : base(annotation, (FrameworkElement) null, false)
  {
    GenericGui.Item obj = this.Item;
    bool? nullable;
    if (obj == null)
    {
      nullable = new bool?();
    }
    else
    {
      IMemberData member = obj.Member;
      nullable = member != null ? new bool?(ReflectionDataExtensions.HasAttribute<OverrideReloadAttribute>((IReflectionData) member)) : new bool?();
    }
    this.OverrideReload = nullable.GetValueOrDefault();
    this.RootItem = item;
    this.EnableReadOnly = true;
    this.Update();
  }
}
