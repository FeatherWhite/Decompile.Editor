// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Selectable
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class Selectable : DependencyObject
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (Selectable), new PropertyMetadata((object) false, new PropertyChangedCallback(Selectable.smethod_0)));

  public object Value { get; set; }

  public bool IsSelected
  {
    get => (bool) this.GetValue(Selectable.IsSelectedProperty);
    set => this.SetValue(Selectable.IsSelectedProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as Selectable).method_0();
  }

  private void method_0()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }

  public event EventHandler IsSelectedChanged;
}
