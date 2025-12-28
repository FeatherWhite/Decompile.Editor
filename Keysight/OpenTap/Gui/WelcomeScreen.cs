// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.WelcomeScreen
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class WelcomeScreen : UserControl, IComponentConnector, IStyleConnector
{
  private static OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource(nameof (WelcomeScreen));
  internal ScrollViewer scrollViewer_0;
  internal StackPanel stackPanel_0;
  internal StackPanel stackPanel_1;
  private bool bool_0;

  public WelcomeScreen()
  {
    this.InitializeComponent();
    if (!ComponentSettings<EditorSettings>.Current.ShowWelcomeScreen)
    {
      this.Visibility = System.Windows.Visibility.Collapsed;
    }
    else
    {
      IEnumerable<TextBlock> source = ((IEnumerable<string>) ComponentSettings<GuiControlsSettings>.Current.RecentTestPlans).Select<string, TextBlock>(new Func<string, TextBlock>(this.method_0));
      if (source.Count<TextBlock>() > 0)
      {
        ((IEnumerable<TextBlock>) source.Take<TextBlock>(5)).Each<TextBlock>((Action<TextBlock, int>) ((textBlock_0, int_0) => this.stackPanel_1.Children.Add((UIElement) textBlock_0)));
      }
      else
      {
        TextBlock textBlock = new TextBlock();
        textBlock.Margin = new Thickness(0.0, 5.0, 0.0, 0.0);
        TextBlock element = textBlock;
        Hyperlink hyperlink = new Hyperlink();
        hyperlink.Inlines.Add("New Test Plan");
        hyperlink.Click += (RoutedEventHandler) ((sender, e) => this.Visibility = System.Windows.Visibility.Collapsed);
        element.Inlines.Add((Inline) hyperlink);
        this.stackPanel_1.Children.Add((UIElement) element);
      }
      if (!Directory.Exists(Path.GetFullPath("Packages\\SDK")))
        return;
      this.stackPanel_0.Visibility = System.Windows.Visibility.Visible;
    }
  }

  private TextBlock method_0(string string_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    WelcomeScreen.Class199 class199 = new WelcomeScreen.Class199();
    // ISSUE: reference to a compiler-generated field
    class199.string_0 = string_0;
    TextBlock textBlock = new TextBlock();
    textBlock.Margin = new Thickness(0.0, 5.0, 0.0, 0.0);
    Hyperlink hyperlink = new Hyperlink();
    // ISSUE: reference to a compiler-generated field
    hyperlink.Inlines.Add(new FileInfo(class199.string_0).Name);
    // ISSUE: reference to a compiler-generated field
    class199.mainWindow_0 = (MainWindow) Application.Current.MainWindow;
    // ISSUE: reference to a compiler-generated method
    hyperlink.Click += new RoutedEventHandler(class199.method_0);
    textBlock.Inlines.Add((Inline) hyperlink);
    return textBlock;
  }

  private void method_1(object sender, RoutedEventArgs e) => this.Visibility = System.Windows.Visibility.Collapsed;

  private void method_2(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    Hyperlink hyperlink = sender as Hyperlink;
    try
    {
      hyperlink.DoClick();
    }
    catch (Exception ex)
    {
      WelcomeScreen.traceSource_0.Error("Unable to open link '{0}'.", (object) hyperlink.NavigateUri);
      WelcomeScreen.traceSource_0.Info("The error was '{0}'.", (object) ex.Message);
      WelcomeScreen.traceSource_0.Debug(ex);
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/welcomescreen.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  internal Delegate method_3(Type type_0, string string_0)
  {
    return Delegate.CreateDelegate(type_0, (object) this, string_0);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 2:
        this.scrollViewer_0 = (ScrollViewer) target;
        break;
      case 3:
        this.stackPanel_0 = (StackPanel) target;
        break;
      case 4:
        this.stackPanel_1 = (StackPanel) target;
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.method_1);
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
      return;
    ((ContentElement) target).PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.method_2);
  }
}
