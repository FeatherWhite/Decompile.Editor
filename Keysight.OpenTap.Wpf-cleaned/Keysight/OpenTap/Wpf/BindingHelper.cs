// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.BindingHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class BindingHelper
{
  public static BindingExpressionBase Bind(
    this DependencyObject binder,
    string path,
    DependencyObject target,
    DependencyProperty prop,
    BindingMode bindingMode = BindingMode.Default,
    IValueConverter converter = null,
    object converter_parameter = null)
  {
    return BindingOperations.SetBinding(target, prop, (BindingBase) new Binding(path)
    {
      Source = (object) binder,
      Mode = bindingMode,
      Converter = converter,
      ConverterParameter = converter_parameter
    });
  }
}
