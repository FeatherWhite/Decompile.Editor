// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MethodControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MethodControlProvider : IControlProvider, ITapPlugin
{
  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    IMemberAnnotation imemberAnnotation_0 = annotation.Get<IMemberAnnotation>(false, (object) null);
    if (imemberAnnotation_0 == null)
      return (FrameworkElement) null;
    IMethodAnnotation method = annotation.Get<IMethodAnnotation>(false, (object) null);
    if (method == null)
      return (FrameworkElement) null;
    GuiOptions guiOptions = annotation.Get<GuiOptions>(false, (object) null);
    if (guiOptions != null)
    {
      guiOptions.FullRow = true;
      guiOptions.OverridesReadOnly = true;
    }
    Button button = new Button();
    button.Content = (object) ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) imemberAnnotation_0.Member).Name;
    button.Padding = new Thickness(8.0, 1.0, 8.0, 1.0);
    Button button_0 = button;
    button_0.Click += (RoutedEventHandler) ((sender, e) =>
    {
      try
      {
        method.Invoke();
        button_0.RaiseEvent(new RoutedEventArgs(SettingsDialog.SettingsChangedEvent, (object) button_0));
        UserInput.NotifyChanged(annotation.Source, ((IReflectionData) imemberAnnotation_0.Member).Name);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
      catch (Exception ex)
      {
        TraceSource source = Log.CreateSource("Action");
        Log.Error(source, "Error calling method " + ((IReflectionData) imemberAnnotation_0.Member).Name, Array.Empty<object>());
        Log.Debug(source, ex);
      }
    });
    Grid control = new Grid();
    ColumnDefinitionCollection columnDefinitions1 = control.ColumnDefinitions;
    ColumnDefinition columnDefinition1 = new ColumnDefinition();
    columnDefinition1.SharedSizeGroup = "PropGridLiteNameColumnSize";
    columnDefinition1.Width = GridLength.Auto;
    columnDefinitions1.Add(columnDefinition1);
    ColumnDefinitionCollection columnDefinitions2 = control.ColumnDefinitions;
    ColumnDefinition columnDefinition2 = new ColumnDefinition();
    columnDefinition2.SharedSizeGroup = "SharedButtonSize";
    columnDefinition2.Width = GridLength.Auto;
    columnDefinitions2.Add(columnDefinition2);
    control.ColumnDefinitions.Add(new ColumnDefinition()
    {
      Width = new GridLength(1.0, GridUnitType.Star)
    });
    control.Children.Add((UIElement) button_0);
    Grid.SetColumn((UIElement) button_0, 1);
    return (FrameworkElement) control;
  }

  public double Order => 15.0;
}
