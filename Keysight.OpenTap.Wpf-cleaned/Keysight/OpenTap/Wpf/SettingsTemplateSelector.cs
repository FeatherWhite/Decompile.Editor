// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SettingsTemplateSelector
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SettingsTemplateSelector : DataTemplateSelector
{
  private static TraceSource traceSource_0 = Log.CreateSource("Settings");
  private static Dictionary<Type, Type> dictionary_0 = new Dictionary<Type, Type>();

  public static void SetPreferedSettingsProvider(Type settingsType, Type prefered)
  {
    SettingsTemplateSelector.dictionary_0[settingsType] = prefered;
  }

  public static Type GetPreferedProvider(Type type)
  {
    Type preferedProvider = (Type) null;
    SettingsTemplateSelector.dictionary_0.TryGetValue(type, out preferedProvider);
    return preferedProvider;
  }

  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    Type itemtype = item.GetType();
    Type preferedProvider = SettingsTemplateSelector.GetPreferedProvider(itemtype);
    Type type = (Type) null;
    if (preferedProvider != (Type) null)
    {
      ISettingsControlProvider instance = SettingsDialog.CreateInstance(preferedProvider);
      if (instance != null)
        type = instance.GetControlType(itemtype);
    }
    if (type == (Type) null)
      type = SettingsDialog.GetSettingsControlProviders().Select<ISettingsControlProvider, Type>((Func<ISettingsControlProvider, Type>) (isettingsControlProvider_0 => isettingsControlProvider_0.GetControlType(itemtype))).FirstOrDefault<Type>((Func<Type, bool>) (type_0 => type_0 != (Type) null));
    if (type == (Type) null)
      type = typeof (NormalSettingsControl);
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(type);
    frameworkElementFactory.SetBinding(FrameworkElement.DataContextProperty, (BindingBase) new Binding()
    {
      ValidatesOnDataErrors = true,
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
      NotifyOnValidationError = true
    });
    DataTemplate dataTemplate = new DataTemplate();
    dataTemplate.VisualTree = frameworkElementFactory;
    return dataTemplate;
  }
}
