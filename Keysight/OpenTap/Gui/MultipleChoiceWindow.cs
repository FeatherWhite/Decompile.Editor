// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.MultipleChoiceWindow
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.Ccl.Wsl.UI;
using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class MultipleChoiceWindow : WslDialog, IComponentConnector, IStyleConnector
{
  internal TextBlock textBlock_0;
  internal ItemsControl itemsControl_0;
  private bool bool_0;

  public object SelectedItem { get; private set; }

  public string Message { get; private set; }

  public MultipleChoiceWindow(IEnumerable items, string message)
  {
    this.InitializeComponent();
    this.itemsControl_0.ItemsSource = items;
    this.textBlock_0.Text = message;
    GuiControlsSettings current = ComponentSettings<GuiControlsSettings>.Current;
    if (current.MultipleChoiceWindowSize == null)
      return;
    this.SizeToContent = SizeToContent.Manual;
    this.Width = current.MultipleChoiceWindowSize.Width;
    this.Height = current.MultipleChoiceWindowSize.Height;
  }

  private void method_0(object sender, RoutedEventArgs e)
  {
    this.SelectedItem = ((FrameworkElement) sender).DataContext;
    this.Close();
  }

  private void method_1(object sender, RoutedEventArgs e)
  {
    this.SizeChanged += new SizeChangedEventHandler(this.MultipleChoiceWindow_SizeChanged);
    GuiControlsSettings current = ComponentSettings<GuiControlsSettings>.Current;
    Window mainWindow = Application.Current.MainWindow;
    if (current.MultipleChoiceWindowSize == null)
    {
      bool flag;
      if (flag = this.ActualWidth > mainWindow.Width / 2.0)
      {
        this.Width = mainWindow.Width / 2.0;
        this.SizeToContent = SizeToContent.Height;
      }
      if (this.ActualHeight > mainWindow.Height / 2.0)
      {
        this.Height = mainWindow.Height / 2.0;
        this.SizeToContent = flag ? SizeToContent.Manual : SizeToContent.Width;
      }
    }
    this.Left = mainWindow.Left + mainWindow.ActualWidth / 2.0 - this.ActualWidth / 2.0;
    this.Top = mainWindow.Top + mainWindow.ActualHeight / 2.0 - this.ActualHeight / 2.0;
  }

  private void MultipleChoiceWindow_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ComponentSettings<GuiControlsSettings>.Current.MultipleChoiceWindowSize = new TapSize()
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
    Application.LoadComponent((object) this, new Uri("/Editor;component/multiplechoicewindow.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.method_1);
        break;
      case 2:
        this.textBlock_0 = (TextBlock) target;
        break;
      case 3:
        this.itemsControl_0 = (ItemsControl) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 4)
      return;
    ((ButtonBase) target).Click += new RoutedEventHandler(this.method_0);
  }
}
