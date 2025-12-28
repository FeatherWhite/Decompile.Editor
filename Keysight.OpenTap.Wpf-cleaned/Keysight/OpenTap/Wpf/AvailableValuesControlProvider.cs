// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AvailableValuesControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using MahApps.Metro.Controls;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Browsable(false)]
public class AvailableValuesControlProvider : IControlProvider, ITapPlugin
{
  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    IAvailableValuesAnnotationProxy available = annotation.Get<IAvailableValuesAnnotationProxy>(false, (object) null);
    if (available == null)
      return (FrameworkElement) null;
    if (annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null) != null)
      return (FrameworkElement) null;
    IMemberAnnotation imemberAnnotation = annotation.Get<IMemberAnnotation>(false, (object) null);
    if ((imemberAnnotation != null ? (ReflectionDataExtensions.HasAttribute<SubmitAttribute>((IReflectionData) imemberAnnotation.Member) ? 1 : 0) : 0) != 0)
    {
      WrapPanel wrapPanel1 = new WrapPanel();
      wrapPanel1.HorizontalAlignment = HorizontalAlignment.Right;
      wrapPanel1.Orientation = Orientation.Horizontal;
      WrapPanel wrapPanel = wrapPanel1;
      foreach (AnnotationCollection availableValue in available.AvailableValues)
      {
        FrameworkElement frameworkElement = GenericGui.Create(availableValue);
        Button button1 = new Button();
        button1.MinWidth = 50.0;
        button1.Content = (object) frameworkElement;
        button1.Margin = new Thickness(4.0, 2.0, 0.0, 0.0);
        button1.Padding = new Thickness(3.0, 2.0, 3.0, 2.0);
        Button button_0_1 = button1;
        ControlProvider.SetSingleLineView((DependencyObject) button_0_1, true);
        button_0_1.SetBinding(FrameworkElement.ToolTipProperty, (BindingBase) new Binding("ToolTip")
        {
          Source = (object) frameworkElement
        });
        if (available.SelectedValue == availableValue)
          button_0_1.IsDefault = true;
        if (string.Compare(availableValue.Get<IStringReadOnlyValueAnnotation>(false, (object) null)?.Value, "Cancel", StringComparison.InvariantCultureIgnoreCase) == 0)
          button_0_1.IsCancel = true;
        AnnotationCollection savedAvailableValue = availableValue;
        button_0_1.Click += (RoutedEventHandler) ((sender, e) =>
        {
          available.SelectedValue = savedAvailableValue;
          annotation.Write();
          annotation.Get<UpdateMonitor>(true, (object) null)?.Commit();
          annotation.ParentAnnotation.Read();
        });
        wrapPanel.Children.Add((UIElement) button_0_1);
        button_0_1.BorderThickness = new Thickness(2.0, 2.0, 2.0, 2.0);
        button_0_1.GotFocus += (RoutedEventHandler) ((sender, e) => button_0_1.BorderBrush = (Brush) new SolidColorBrush((Color) button_0_1.TryFindResource((object) "Control.Accent.Color.Medium")));
        button_0_1.LostFocus += (RoutedEventHandler) ((sender, e) =>
        {
          button_0_1.BorderBrush = (Brush) Brushes.Transparent;
          if (wrapPanel.IsKeyboardFocusWithin)
            return;
          SolidColorBrush resource = (SolidColorBrush) button_0_1.TryFindResource((object) "Button.Defaulted.Border");
          if (resource == null)
            return;
          Button button2 = TreeHelper.FindChildren<Button>((DependencyObject) wrapPanel, false).FirstOrDefault<Button>((Func<Button, bool>) (button_0 => button_0.IsDefault));
          if (button2 == null)
            return;
          button2.BorderBrush = (Brush) resource;
        });
      }
      return (FrameworkElement) wrapPanel;
    }
    ComboBox comboBox_0 = new ComboBox();
    ControlProvider.SetSingleLineView((DependencyObject) comboBox_0, true);
    comboBox_0.SetBinding(Selector.SelectedValueProperty, (BindingBase) new Binding("SelectedValue")
    {
      NotifyOnSourceUpdated = true
    });
    comboBox_0.SetBinding(ItemsControl.ItemsSourceProperty, (BindingBase) new Binding("AvailableValues"));
    comboBox_0.DataContext = (object) new AvailableValuesControlProvider.AvailableValueState(available);
    annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) comboBox_0, (Action) (() =>
    {
      comboBox_0.DataContext = (object) new AvailableValuesControlProvider.AvailableValueState(available);
      ((AvailableValuesControlProvider.AvailableValueState) comboBox_0.DataContext).OnBound();
    }));
    // ISSUE: reference to a compiler-generated method
    comboBox_0.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((sender, e) => this.method_0());
    return (FrameworkElement) comboBox_0;
  }

  public double Order => 17.0;

  private class AvailableValueState : INotifyPropertyChanged
  {
    private readonly IAvailableValuesAnnotationProxy proxy;

    public IEnumerable<AnnotationCollection> AvailableValues => this.proxy.AvailableValues;

    public AnnotationCollection SelectedValue
    {
      get => this.proxy.SelectedValue;
      set => this.proxy.SelectedValue = value;
    }

    public AvailableValueState(IAvailableValuesAnnotationProxy proxy) => this.proxy = proxy;

    public void OnBound() => this.method_0("SelectedValue");

    public event PropertyChangedEventHandler PropertyChanged;

    private void method_0(string string_0)
    {
      // ISSUE: reference to a compiler-generated field
      PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
      if (changedEventHandler0 == null)
        return;
      changedEventHandler0((object) this, new PropertyChangedEventArgs(string_0));
    }
  }
}
