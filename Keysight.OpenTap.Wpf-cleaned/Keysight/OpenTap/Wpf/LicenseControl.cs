// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LicenseControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class LicenseControl : UserControl, IComponentConnector
{
  private bool bool_0;

  public LicenseControl()
  {
    this.DataContext = (object) new LicenseControlViewModel();
    this.InitializeComponent();
  }

  private void method_0(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

  private void method_1(object sender, ExecutedRoutedEventArgs e)
  {
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/licensecontrol.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_0);
      ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_1);
    }
    else
      this.bool_0 = true;
  }
}
