// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ContentContainer
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ContentContainer : Control
{
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (UIElement), typeof (ContentContainer));

  public UIElement Content
  {
    get => (UIElement) this.GetValue(ContentContainer.ContentProperty);
    set => this.SetValue(ContentContainer.ContentProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    ((Decorator) this.GetTemplateChild("brd")).Child = this.Content;
  }
}
