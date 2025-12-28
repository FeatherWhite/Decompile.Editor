// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Themes.AllowsTransparencySkinFragment
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

public class AllowsTransparencySkinFragment : Skin, IComponentConnector
{
  private bool bool_0;

  public AllowsTransparencySkinFragment()
    : base("AllowTransparency", SkinCategories.Supported | SkinCategories.Fragment | SkinCategories.Custom | SkinCategories.UserSelectable)
  {
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/themes/allowstransparencyskinfragment.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target) => this.bool_0 = true;
}
