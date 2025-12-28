// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.BoolControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Browsable(false)]
public class BoolControlProvider : IControlProvider, ITapPlugin
{
  private void method_0(object sender, EventArgs e)
  {
    CheckBox checkBox = (CheckBox) sender;
    if (!checkBox.IsLoaded)
      return;
    checkBox.GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateSource();
    if (!(checkBox.DataContext is AnnotationCollection dataContext))
      return;
    dataContext.Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
  }

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    IMemberData member = annotation.Get<IMemberAnnotation>(false, (object) null)?.Member;
    if (member != null)
    {
      if (!member.TypeDescriptor.IsA(typeof (bool)))
        return (FrameworkElement) null;
    }
    else if (!(annotation.Get<IObjectValueAnnotation>(false, (object) null).Value is bool))
      return (FrameworkElement) null;
    CheckBox checkBox1 = new CheckBox();
    checkBox1.HorizontalAlignment = HorizontalAlignment.Left;
    checkBox1.VerticalContentAlignment = VerticalAlignment.Center;
    checkBox1.HorizontalContentAlignment = HorizontalAlignment.Center;
    checkBox1.MinWidth = 23.0;
    checkBox1.MinHeight = 23.0;
    checkBox1.DataContext = (object) annotation;
    CheckBox checkBox_0 = checkBox1;
    IObjectValueAnnotation iobjectValueAnnotation = annotation.Get<IObjectValueAnnotation>(false, (object) null);
    checkBox_0.SetBinding(ToggleButton.IsCheckedProperty, (BindingBase) new Binding("Value")
    {
      Source = (object) iobjectValueAnnotation,
      NotifyOnSourceUpdated = true
    });
    checkBox_0.Checked += new RoutedEventHandler(this.method_0);
    checkBox_0.Unchecked += new RoutedEventHandler(this.method_0);
    ReadOnlyViewAnnotation onlyViewAnnotation = annotation.Get<ReadOnlyViewAnnotation>(false, (object) null);
    if (onlyViewAnnotation != null)
    {
      annotation.Remove((IAnnotation) onlyViewAnnotation);
      annotation.RemoveType<ReadOnlyMemberAnnotation>();
    }
    annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) checkBox_0, (Action) (() => checkBox_0.GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateTarget()));
    CheckBox checkBox2 = checkBox_0;
    ReadOnlyMemberAnnotation memberAnnotation = annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null);
    int num = memberAnnotation != null ? (memberAnnotation.IsReadOnly ? 1 : 0) : 1;
    checkBox2.IsEnabled = num != 0;
    if (annotation.Get<HelpLinkAttribute>(false, (object) null) == null)
      return (FrameworkElement) checkBox_0;
    Button button1 = new Button();
    button1.Background = (Brush) Brushes.Transparent;
    button1.Margin = new Thickness(-4.0, 0.0, 0.0, -1.0);
    button1.Padding = new Thickness(0.0);
    button1.BorderThickness = new Thickness(0.0);
    button1.Style = (Style) Application.Current.FindResource((object) "WslWindow.HelpButton.Style");
    button1.ToolTip = (object) "Help";
    button1.Height = 18.0;
    button1.Width = 18.0;
    button1.HorizontalAlignment = HorizontalAlignment.Left;
    button1.Command = (ICommand) ApplicationCommands.Help;
    Button button2 = button1;
    checkBox_0.Content = (object) button2;
    return (FrameworkElement) checkBox_0;
  }

  public double Order => 15.0;
}
