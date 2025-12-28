// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Themes.LightThemeSkin
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.Ccl.Wsl.UI.Resources.Skins;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf.Themes;

public class LightThemeSkin : Skin, IComponentConnector
{
  private bool bool_0;

  public LightThemeSkin()
    : base("TapLight", SkinCategories.Supported | SkinCategories.Fragment | SkinCategories.Custom | SkinCategories.Color | SkinCategories.UserSelectable)
  {
    this.RegisterSkinRelationship(typeof (CaranuLightSkinFragment));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/themes/lightthemeskin.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target) => this.bool_0 = true;
}
