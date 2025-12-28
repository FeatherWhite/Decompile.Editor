// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GuiMenuModelFactory
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class GuiMenuModelFactory : IMenuModelFactory, ITapPlugin
{
  public IMenuModel CreateModel(IMemberData member)
  {
    ItemUi loadingCurrent = MenuUi.LoadingCurrent;
    return loadingCurrent == null ? (IMenuModel) null : (IMenuModel) new GuiMenuItems(loadingCurrent.Annotation, loadingCurrent.Annotation.Get<UpdateMonitor>(true, (object) null));
  }
}
