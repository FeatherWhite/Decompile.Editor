// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.DefaultStyle
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.OpenTap.Gui;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class DefaultStyle : ResourceDictionary, IComponentConnector, IStyleConnector
{
  private bool bool_0;

  private void method_0(object sender, RoutedEventArgs e)
  {
    DependencyObject reference;
    for (reference = sender as DependencyObject; !(reference is Button); reference = VisualTreeHelper.GetChild(reference, 0))
    {
      if (VisualTreeHelper.GetChildrenCount(reference) <= 0)
        return;
    }
    (reference as Button).SetResourceReference(Control.TemplateProperty, (object) "SelectAllButtonTemplate");
  }

  public void dialogLoaded(object sender, EventArgs e)
  {
    FrameworkElement frameworkElement = ((DependencyObject) sender).GetVisualChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (child => child.Name == "ChromeModeMenuContentContainer"));
    if (frameworkElement != null)
      frameworkElement.Margin = new Thickness(0.0);
    WslMainWindow wslMainWindow = sender as WslMainWindow;
    Border binder = wslMainWindow.GetVisualChildren().OfType<Border>().FirstOrDefault<Border>((Func<Border, bool>) (border_0 => border_0.Name == "OuterBorder"));
    if (binder != null)
      binder.Bind("BorderThickness", (DependencyObject) wslMainWindow, Control.BorderThicknessProperty, BindingMode.OneWay);
    ContentPresenter contentPresenter = wslMainWindow.GetVisualChildren().OfType<ContentPresenter>().FirstOrDefault<ContentPresenter>((Func<ContentPresenter, bool>) (contentPresenter_0 => contentPresenter_0.Name == "NoTitleBarMenuContentContainer"));
    if (contentPresenter == null)
      return;
    contentPresenter.Margin = new Thickness(0.0);
  }

  public void dialogHandleEsc(object sender, KeyEventArgs e)
  {
    WslDialog dpObj = (WslDialog) sender;
    if (e.Key != Key.Escape)
      return;
    dpObj.GetVisualChildren().OfType<Button>().FirstOrDefault<Button>((Func<Button, bool>) (button => button.IsCancel))?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
  }

  private void method_1(object sender, DependencyPropertyChangedEventArgs e)
  {
    Grid grid = sender as Grid;
    if (e.NewValue == null)
      grid.Opacity = 0.0;
    else
      grid.BeginStoryboard((Storyboard) this[(object) "Tap.FadeIn"], HandoffBehavior.Compose);
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/defaultstyle.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target) => this.bool_0 = true;

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = FrameworkElement.LoadedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.dialogLoaded)
        });
        break;
      case 2:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = FrameworkElement.LoadedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.dialogLoaded)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.KeyDownEvent,
          Handler = (Delegate) new KeyEventHandler(this.dialogHandleEsc)
        });
        break;
      case 3:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = FrameworkElement.LoadedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.method_0)
        });
        break;
      case 4:
        ((FrameworkElement) target).DataContextChanged += new DependencyPropertyChangedEventHandler(this.method_1);
        break;
    }
  }
}
