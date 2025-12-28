// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Alias
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class Alias : MarkupExtension
{
  public string ResourceKey { get; set; }

  public Alias()
  {
  }

  public Alias(string resourceKey) => this.ResourceKey = resourceKey;

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    return this.method_0(serviceProvider) ?? this.method_1();
  }

  private object method_0(IServiceProvider iserviceProvider_0)
  {
    IRootObjectProvider service = (IRootObjectProvider) iserviceProvider_0.GetService(typeof (IRootObjectProvider));
    if (service == null)
      return (object) null;
    if (!(service.RootObject is IDictionary rootObject))
      return (object) null;
    return !rootObject.Contains((object) this.ResourceKey) ? (object) null : rootObject[(object) this.ResourceKey];
  }

  private object method_1() => Application.Current.TryFindResource((object) this.ResourceKey);
}
