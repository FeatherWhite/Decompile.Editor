// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NewStepDialog
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NewStepDialog : WslDialog, IComponentConnector
{
  internal NewStepControl newStepControl;
  internal Button CloseBtn;
  private bool bool_0;

  public NewStepControl NewStepControl => this.newStepControl;

  public NewStepDialog(Type type_0)
  {
    this.InitializeComponent();
    TapSize dialogWindowSize = ComponentSettings<GuiControlsSettings>.Current.NewStepDialogWindowSize;
    if (dialogWindowSize != null)
    {
      this.Height = dialogWindowSize.Height;
      this.Width = dialogWindowSize.Width;
    }
    this.SizeChanged += new SizeChangedEventHandler(this.NewStepDialog_SizeChanged);
    this.newStepControl.SetBaseType(type_0);
  }

  private void NewStepDialog_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ComponentSettings<GuiControlsSettings>.Current.NewStepDialogWindowSize = new TapSize()
    {
      Width = e.NewSize.Width,
      Height = e.NewSize.Height
    };
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/newstepdialog.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.CloseBtn = (Button) target;
    }
    else
      this.newStepControl = (NewStepControl) target;
  }
}
