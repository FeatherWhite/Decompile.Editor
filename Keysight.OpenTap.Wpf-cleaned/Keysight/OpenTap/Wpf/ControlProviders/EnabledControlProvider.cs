// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ControlProviders.EnabledControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf.ControlProviders;

[Browsable(false)]
public class EnabledControlProvider : IControlProvider, ITapPlugin
{
  public double Order => 18.0;

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    IEnabledValueAnnotation ienabledValueAnnotation = annotation.Get<IEnabledValueAnnotation>(false, (object) null);
    if (ienabledValueAnnotation == null)
      return (FrameworkElement) null;
    AnnotationCollection isEnabled = ienabledValueAnnotation.IsEnabled;
    AnnotationCollection annotation1 = ienabledValueAnnotation.Value;
    ReadOnlyViewAnnotation onlyViewAnnotation = annotation.Get<ReadOnlyViewAnnotation>(false, (object) null);
    if (onlyViewAnnotation != null)
    {
      annotation.Remove((IAnnotation) onlyViewAnnotation);
      ReadOnlyMemberAnnotation memberAnnotation = annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null);
      annotation.RemoveType<ReadOnlyMemberAnnotation>();
      annotation1 = annotation1.Clone();
      annotation1.Add((IAnnotation) onlyViewAnnotation);
      if (memberAnnotation != null)
        annotation1.Add((IAnnotation) memberAnnotation);
    }
    Grid control = new Grid();
    control.ColumnDefinitions.Add(new ColumnDefinition()
    {
      Width = GridLength.Auto
    });
    control.ColumnDefinitions.Add(new ColumnDefinition()
    {
      Width = new GridLength(1.0, GridUnitType.Star)
    });
    FrameworkElement element1 = GenericGui.Create(isEnabled);
    FrameworkElement element2 = GenericGui.Create(annotation1);
    element2.SetValue(Grid.ColumnProperty, (object) 1);
    control.Children.Add((UIElement) element1);
    control.Children.Add((UIElement) element2);
    return (FrameworkElement) control;
  }
}
