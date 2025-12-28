// Decompiled with JetBrains decompiler
// Type: XamlGeneratedNamespace.GeneratedInternalTypeHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Markup;

#nullable disable
namespace XamlGeneratedNamespace;

[EditorBrowsable(EditorBrowsableState.Never)]
[GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
[DebuggerNonUserCode]
public sealed class GeneratedInternalTypeHelper : InternalTypeHelper
{
  protected override object CreateInstance(Type type, CultureInfo culture)
  {
    return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, (object[]) null, culture);
  }

  protected override object GetPropertyValue(
    PropertyInfo propertyInfo,
    object target,
    CultureInfo culture)
  {
    return propertyInfo.GetValue(target, BindingFlags.Default, (Binder) null, (object[]) null, culture);
  }

  protected override void SetPropertyValue(
    PropertyInfo propertyInfo,
    object target,
    object value,
    CultureInfo culture)
  {
    propertyInfo.SetValue(target, value, BindingFlags.Default, (Binder) null, (object[]) null, culture);
  }

  protected override Delegate CreateDelegate(Type delegateType, object target, string handler)
  {
    return (Delegate) target.GetType().InvokeMember("_CreateDelegate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, target, new object[2]
    {
      (object) delegateType,
      (object) handler
    }, (CultureInfo) null);
  }

  protected override void AddEventHandler(EventInfo eventInfo, object target, Delegate handler)
  {
    eventInfo.AddEventHandler(target, handler);
  }
}
