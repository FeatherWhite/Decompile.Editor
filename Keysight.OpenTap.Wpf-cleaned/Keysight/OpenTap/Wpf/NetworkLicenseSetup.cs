// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NetworkLicenseSetup
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NetworkLicenseSetup : WslDialog, IComponentConnector
{
  internal static readonly TraceSource _log = Log.CreateSource("License");
  internal TextBox LicenseServerName;
  internal Button CloseBtn;
  private bool bool_0;

  public NetworkLicenseSetup()
  {
    this.InitializeComponent();
    this.LicenseServerName.Text = Environment.GetEnvironmentVariable("LM_LICENSE_FILE", EnvironmentVariableTarget.User);
  }

  private void CloseBtn_Click(object sender, RoutedEventArgs e)
  {
    this.CloseBtn.IsEnabled = false;
    this.CloseBtn.Content = (object) "Setting server.";
    Task.Run((Action) (() => GuiHelpers.GuiInvoke((Action) (() =>
    {
      try
      {
        Environment.SetEnvironmentVariable("LM_LICENSE_FILE", this.LicenseServerName.Text, EnvironmentVariableTarget.User);
      }
      finally
      {
        this.Close();
      }
    }), priority: DispatcherPriority.Background)));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/networklicensesetup.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LicenseServerName = (TextBox) target;
        break;
      case 2:
        this.CloseBtn = (Button) target;
        this.CloseBtn.Click += new RoutedEventHandler(this.CloseBtn_Click);
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CloseBtn_Click);
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
