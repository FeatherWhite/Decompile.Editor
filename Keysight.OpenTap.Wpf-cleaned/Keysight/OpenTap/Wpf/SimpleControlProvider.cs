// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SimpleControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SimpleControlProvider : IControlProvider, ITapPlugin
{
  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    bool flag;
    IStringReadOnlyValueAnnotation onlyValueAnnotation;
    if (flag = annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null) != null)
    {
      onlyValueAnnotation = annotation.Get<IStringReadOnlyValueAnnotation>(false, (object) null);
    }
    else
    {
      onlyValueAnnotation = (IStringReadOnlyValueAnnotation) annotation.Get<IStringValueAnnotation>(false, (object) null);
      if (onlyValueAnnotation == null)
      {
        onlyValueAnnotation = annotation.Get<IStringReadOnlyValueAnnotation>(false, (object) null);
        flag = true;
      }
    }
    string str1 = (string) null;
    if (flag && onlyValueAnnotation == null)
    {
      IObjectValueAnnotation iobjectValueAnnotation = annotation.Get<IObjectValueAnnotation>(false, (object) null);
      if (iobjectValueAnnotation != null)
        str1 = iobjectValueAnnotation.Value?.ToString();
      if (str1 == null)
        return (FrameworkElement) null;
    }
    string str2 = (string) null;
    HelpLinkAttribute helpLinkAttribute = annotation.Get<HelpLinkAttribute>(false, (object) null);
    if (helpLinkAttribute != null)
      str2 = helpLinkAttribute.HelpLink;
    Binding binding;
    if (str1 == null)
      binding = new Binding("Value")
      {
        Mode = flag ? BindingMode.OneWay : BindingMode.TwoWay,
        ValidatesOnDataErrors = true,
        Converter = (IValueConverter) null,
        Source = (object) onlyValueAnnotation,
        NotifyOnSourceUpdated = true,
        NotifyOnValidationError = true,
        ValidatesOnExceptions = true,
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
      };
    else
      binding = new Binding("")
      {
        Mode = BindingMode.OneWay,
        ValidatesOnDataErrors = true,
        Converter = (IValueConverter) null,
        Source = (object) str1,
        NotifyOnSourceUpdated = true,
        NotifyOnValidationError = true,
        ValidatesOnExceptions = true
      };
    ISuggestedValuesAnnotationProxy valuesAnnotationProxy = annotation.Get<ISuggestedValuesAnnotationProxy>(false, (object) null);
    GuiOptions guiOptions1 = annotation.Get<GuiOptions>(false, (object) null);
    int lines = guiOptions1 != null ? guiOptions1.RowHeight : 1;
    bool readOnlyBox = annotation.Get<ReadOnlyViewAnnotation>(false, (object) null) != null || annotation.Get<ReadOnlyMemberAnnotation>(false, (object) null) != null;
    TextBoxWithHelp textBox = new TextBoxWithHelp(lines, readOnlyBox, !readOnlyBox && valuesAnnotationProxy != null);
    FrameworkElement element1 = (FrameworkElement) textBox;
    if (!string.IsNullOrWhiteSpace(str2))
    {
      Grid grid = new Grid();
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1.0, GridUnitType.Star)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = GridLength.Auto
      });
      grid.Children.Add((UIElement) element1);
      HelpLinkButton element2 = new HelpLinkButton()
      {
        HelpLink = str2
      };
      Grid.SetColumn((UIElement) element2, 1);
      grid.Children.Add((UIElement) element2);
      element1 = (FrameworkElement) grid;
    }
    textBox.UpdateSource = (Action<string>) (string_0 =>
    {
      IStringValueAnnotation istringValueAnnotation = annotation.Get<IStringValueAnnotation>(false, (object) null);
      if (istringValueAnnotation != null)
      {
        if (istringValueAnnotation.Value == string_0)
          return;
        istringValueAnnotation.Value = string_0;
      }
      WriterAnnotation writerAnnotation = annotation.Get<WriterAnnotation>(false, (object) null);
      if (writerAnnotation == null)
        return;
      Action action = writerAnnotation.Action;
      if (action == null)
        return;
      action();
    });
    if (textBox.ReadOnlyBox)
    {
      GuiOptions guiOptions2 = annotation.Get<GuiOptions>(false, (object) null);
      if (guiOptions2 != null)
        guiOptions2.OverridesReadOnly = true;
      else
        annotation.Add((IAnnotation) new GuiOptions()
        {
          OverridesReadOnly = true
        });
    }
    textBox.SetBinding(FrameworkElement.DataContextProperty, (BindingBase) binding);
    if (valuesAnnotationProxy != null)
      textBox.SetBinding(TextBoxWithHelp.SuggestedValuesProperty, (BindingBase) new Binding("SuggestedValues")
      {
        Source = (object) valuesAnnotationProxy
      });
    if (str1 == null)
      annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) textBox, (Action) (() =>
      {
        if (!textBox.IsKeyboardFocusWithin)
          textBox.GetBindingExpression(FrameworkElement.DataContextProperty)?.UpdateTarget();
        textBox.GetBindingExpression(TextBoxWithHelp.SuggestedValuesProperty)?.UpdateTarget();
      }));
    return element1;
  }

  public double Order => 11.0;
}
