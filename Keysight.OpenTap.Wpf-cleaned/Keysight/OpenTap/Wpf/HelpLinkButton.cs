// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.HelpLinkButton
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class HelpLinkButton : UserControl, IComponentConnector
{
  public static DependencyProperty HelpLinkProperty = DependencyProperty.Register(nameof (HelpLink), typeof (string), typeof (HelpLinkButton));
  internal Viewbox helpViewBox;
  internal Button HelpBtn;
  private bool bool_0;

  public string HelpLink
  {
    get => (string) this.GetValue(HelpLinkButton.HelpLinkProperty);
    set => this.SetValue(HelpLinkButton.HelpLinkProperty, (object) value);
  }

  public HelpLinkButton() => this.InitializeComponent();

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != HelpLinkButton.HelpLinkProperty)
      return;
    HelpManager.SetHelpLink((DependencyObject) this.HelpBtn, dependencyPropertyChangedEventArgs_0.NewValue);
    if (dependencyPropertyChangedEventArgs_0.NewValue == null)
      this.Visibility = Visibility.Collapsed;
    else
      this.Visibility = Visibility.Visible;
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/helplinkbutton.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.HelpBtn = (Button) target;
    }
    else
      this.helpViewBox = (Viewbox) target;
  }
}
