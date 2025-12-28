// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PasswordControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PasswordControl : UserControl, IComponentConnector
{
  public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof (Password), typeof (SecureString), typeof (PasswordControl));
  internal PasswordControl This;
  internal PasswordBox Pass;
  private bool bool_0;

  public PasswordControl() => this.InitializeComponent();

  public SecureString Password
  {
    get => (SecureString) this.GetValue(PasswordControl.PasswordProperty);
    set => this.SetValue(PasswordControl.PasswordProperty, (object) value);
  }

  private void Pass_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
  {
    this.SetCurrentValue(PasswordControl.PasswordProperty, (object) this.Pass.SecurePassword);
    this.GetBindingExpression(PasswordControl.PasswordProperty).UpdateSource();
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != PasswordControl.PasswordProperty)
      return;
    this.Pass.Password = this.Password.ConvertToUnsecureString();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controlproviders/passwordcontrol.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
      {
        this.bool_0 = true;
      }
      else
      {
        this.Pass = (PasswordBox) target;
        this.Pass.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.Pass_LostKeyboardFocus);
      }
    }
    else
      this.This = (PasswordControl) target;
  }
}
