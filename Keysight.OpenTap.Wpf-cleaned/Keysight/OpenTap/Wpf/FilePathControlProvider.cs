// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FilePathControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Browsable(false)]
public class FilePathControlProvider : IControlProvider, ITapPlugin
{
  private bool bool_0;

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    if (this.bool_0)
      return (FrameworkElement) null;
    DirectoryPathAttribute attr1 = (DirectoryPathAttribute) null;
    FilePathAttribute attr2 = annotation.Get<FilePathAttribute>(false, (object) null);
    if (attr2 == null)
    {
      attr1 = annotation.Get<DirectoryPathAttribute>(false, (object) null);
      if (attr1 == null)
      {
        IReflectionAnnotation ireflectionAnnotation = annotation.Get<IReflectionAnnotation>(false, (object) null);
        if ((ireflectionAnnotation != null ? (ReflectionDataExtensions.DescendsTo(ireflectionAnnotation.ReflectionInfo, typeof (string)) ? 1 : 0) : 0) != 0)
        {
          AnnotationCollection parentAnnotation = annotation.ParentAnnotation;
          if (parentAnnotation != null)
          {
            attr2 = parentAnnotation.Get<FilePathAttribute>(false, (object) null);
            if (attr2 == null)
              attr1 = parentAnnotation.Get<DirectoryPathAttribute>(false, (object) null);
          }
        }
        if (attr2 == null && attr1 == null)
          return (FrameworkElement) null;
      }
    }
    FilePathControlProvider.ValueModel valueModel = new FilePathControlProvider.ValueModel(annotation);
    if (!valueModel.Valid)
      return (FrameworkElement) null;
    if (valueModel.IsList && attr1 != null)
      return (FrameworkElement) null;
    this.bool_0 = true;
    try
    {
      AnnotationCollection annotationCollection = annotation.Clone();
      annotationCollection.RemoveType<HelpLinkAttribute>();
      FrameworkElement element1 = GenericGui.LoadDependencyContainer(annotationCollection);
      FilePathControl filePathControl_0 = attr1 == null ? new FilePathControl(attr2, valueModel.IsList) : new FilePathControl(attr1);
      filePathControl_0.SetBinding(FilePathControl.PathsProperty, (BindingBase) new Binding("Value")
      {
        Source = (object) valueModel,
        Mode = BindingMode.TwoWay,
        NotifyOnSourceUpdated = true
      });
      IStringExampleValueAnnotation exampleValueAnnotation = annotation.Get<IStringExampleValueAnnotation>(false, (object) null);
      if (exampleValueAnnotation != null)
        filePathControl_0.SetBinding(FilePathControl.PathExampleProperty, (BindingBase) new Binding("Example")
        {
          Source = (object) exampleValueAnnotation,
          Mode = BindingMode.OneWay,
          NotifyOnSourceUpdated = true
        });
      annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) filePathControl_0, (Action) (() =>
      {
        filePathControl_0.GetBindingExpression(FilePathControl.PathExampleProperty)?.UpdateTarget();
        filePathControl_0.GetBindingExpression(FilePathControl.PathProperty)?.UpdateTarget();
        filePathControl_0.GetBindingExpression(FilePathControl.PathsProperty)?.UpdateTarget();
      }));
      Grid control = new Grid();
      control.Children.Add((UIElement) element1);
      control.Children.Add((UIElement) filePathControl_0);
      Grid.SetColumn((UIElement) filePathControl_0, 1);
      control.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1.0, GridUnitType.Star)
      });
      control.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = GridLength.Auto
      });
      HelpLinkAttribute helpLinkAttribute = annotation.Get<HelpLinkAttribute>(false, (object) null);
      if (helpLinkAttribute != null)
      {
        control.ColumnDefinitions.Add(new ColumnDefinition()
        {
          Width = GridLength.Auto
        });
        HelpLinkButton element2 = new HelpLinkButton()
        {
          HelpLink = helpLinkAttribute.HelpLink
        };
        Grid.SetColumn((UIElement) element2, 2);
        control.Children.Add((UIElement) element2);
      }
      return (FrameworkElement) control;
    }
    finally
    {
      this.bool_0 = false;
    }
  }

  public double Order => 20.0;

  private class ValueModel : ValidatingObject
  {
    private readonly IStringValueAnnotation istringValueAnnotation_0;
    private readonly ICollectionAnnotation icollectionAnnotation_0;

    public object Value
    {
      set
      {
        if (this.IsList)
          this.icollectionAnnotation_0.AnnotatedElements = ((IEnumerable<string>) (string[]) value).Select<string, AnnotationCollection>((Func<string, AnnotationCollection>) (string_0 =>
          {
            AnnotationCollection annotationCollection = this.icollectionAnnotation_0.NewElement();
            annotationCollection.Get<IObjectValueAnnotation>(false, (object) null).Value = (object) string_0;
            return annotationCollection;
          }));
        else
          this.istringValueAnnotation_0.Value = ((string[]) value)[0];
      }
      get
      {
        if (this.IsList)
          return (object) this.icollectionAnnotation_0.AnnotatedElements.Select<AnnotationCollection, object>((Func<AnnotationCollection, object>) (annotationCollection_0 => annotationCollection_0.Get<IObjectValueAnnotation>(false, (object) null).Value)).OfType<string>().ToArray<string>();
        return (object) new string[1]
        {
          this.istringValueAnnotation_0.Value
        };
      }
    }

    public bool IsList { get; }

    public bool Valid { get; }

    public ValueModel(AnnotationCollection annotation)
    {
      this.IsList = ReflectionDataExtensions.DescendsTo(annotation.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo, typeof (ICollection<string>));
      this.istringValueAnnotation_0 = annotation.Get<IStringValueAnnotation>(false, (object) null);
      if (this.istringValueAnnotation_0 == null && !this.IsList)
        return;
      this.icollectionAnnotation_0 = annotation.Get<ICollectionAnnotation>(false, (object) null);
      if (this.IsList && this.icollectionAnnotation_0 == null)
        return;
      this.Valid = true;
    }
  }
}
