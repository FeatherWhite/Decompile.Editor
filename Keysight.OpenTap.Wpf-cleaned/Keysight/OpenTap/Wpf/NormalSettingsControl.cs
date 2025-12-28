// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NormalSettingsControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NormalSettingsControl : UserControl, IComponentConnector
{
  private bool bool_0;

  public NormalSettingsControl() => this.InitializeComponent();

  private void PropGridLite_PropertyEdited(object sender, EventArgs e)
  {
    this.RaiseEvent(new RoutedEventArgs(SettingsDialog.SettingsChangedEvent, sender));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/settingscontrolproviders/normalsettingscontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target) => this.bool_0 = true;
}
