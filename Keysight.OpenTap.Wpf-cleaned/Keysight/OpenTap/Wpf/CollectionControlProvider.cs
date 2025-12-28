// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CollectionControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class CollectionControlProvider : IControlProvider, ITapPlugin
{
  public double Order => 12.0;

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    if (annotation.Get<ICollectionAnnotation>(false, (object) null) == null)
      return (FrameworkElement) null;
    if (annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null) != null)
      return (FrameworkElement) null;
    ReadOnlyViewAnnotation onlyViewAnnotation = annotation.Get<ReadOnlyViewAnnotation>(false, (object) null);
    if (onlyViewAnnotation != null)
    {
      annotation.Remove((IAnnotation) onlyViewAnnotation);
      annotation.RemoveType<ReadOnlyMemberAnnotation>();
    }
    Button control = new Button();
    control.Content = (object) "Edit...";
    control.Padding = new Thickness(8.0, 1.0, 8.0, 1.0);
    control.Tag = (object) annotation;
    control.Click += new RoutedEventHandler(this.method_0);
    return (FrameworkElement) control;
  }

  private void method_0(object sender1, RoutedEventArgs e1)
  {
    Button button = (Button) sender1;
    AnnotationCollection tag = (AnnotationCollection) button.Tag;
    tag.Read();
    PropGridListEditorWindow listEditorWindow = new PropGridListEditorWindow(tag);
    listEditorWindow.Owner = Window.GetWindow((DependencyObject) button);
    IMemberAnnotation imemberAnnotation = tag.Get<IMemberAnnotation>(false, (object) null);
    string str;
    if (imemberAnnotation == null)
    {
      str = (string) null;
    }
    else
    {
      str = ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) imemberAnnotation.Member).Name;
      if (str != null)
        goto label_4;
    }
    str = tag.Get<IObjectValueAnnotation>(false, (object) null)?.Value?.ToString();
label_4:
    listEditorWindow.Title = str;
    listEditorWindow.Closed += (EventHandler) ((sender2, e2) =>
    {
      ((AnnotationCollection) button.Tag).Write();
      UpdateMonitor.Update((object) this, (AnnotationCollection) button.Tag);
      ((AnnotationCollection) button.Tag).Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
    });
    listEditorWindow.ShowDialog();
  }
}
