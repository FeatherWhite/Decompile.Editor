// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ControlProviders.PasswordControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Security;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf.ControlProviders;

public class PasswordControlProvider : IControlProvider, ITapPlugin
{
  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    IMemberAnnotation imemberAnnotation = annotation.Get<IMemberAnnotation>(false, (object) null);
    if (imemberAnnotation == null)
      return (FrameworkElement) null;
    if (!ReflectionDataExtensions.DescendsTo(((IReflectionAnnotation) imemberAnnotation).ReflectionInfo, typeof (SecureString)))
      return (FrameworkElement) null;
    IObjectValueAnnotation iobjectValueAnnotation = annotation.Get<IObjectValueAnnotation>(false, (object) null);
    if (iobjectValueAnnotation == null)
      return (FrameworkElement) null;
    PasswordControl passwrd = new PasswordControl();
    passwrd.SetBinding(PasswordControl.PasswordProperty, (BindingBase) new Binding("Value")
    {
      Source = (object) iobjectValueAnnotation,
      NotifyOnSourceUpdated = true,
      UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
      Mode = BindingMode.TwoWay
    });
    annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) passwrd, (Action) (() => passwrd.GetBindingExpression(PasswordControl.PasswordProperty).UpdateTarget()));
    return (FrameworkElement) passwrd;
  }

  public double Order => 20.0;
}
