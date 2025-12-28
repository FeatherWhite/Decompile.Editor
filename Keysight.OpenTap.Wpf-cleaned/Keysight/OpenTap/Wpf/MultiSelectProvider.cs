// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MultiSelectProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class MultiSelectProvider : IControlProvider, ITapPlugin
{
  public double Order => 18.0;

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    if (annotation.Get<IMultiSelectAnnotationProxy>(false, (object) null) == null)
      return (FrameworkElement) null;
    if (annotation.Get<IAvailableValuesAnnotationProxy>(false, (object) null) == null)
      return (FrameworkElement) null;
    if (annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null) != null)
      return (FrameworkElement) null;
    MultiSelect control = new MultiSelect();
    control.DataContext = (object) annotation;
    return (FrameworkElement) control;
  }
}
