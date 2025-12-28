// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.IconView
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class IconView : Control
{
  public static readonly DependencyProperty IconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (IconView));

  public string IconName
  {
    get => (string) this.GetValue(IconView.IconNameProperty);
    set => this.SetValue(IconView.IconNameProperty, (object) value);
  }

  public IconView(string iconName)
    : this()
  {
    this.IconName = iconName;
  }

  public IconView() => this.Loaded += new RoutedEventHandler(this.IconView_Loaded);

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != IconView.IconNameProperty || !this.IsLoaded)
      return;
    this.method_0();
  }

  private void IconView_Loaded(object sender, RoutedEventArgs e) => this.method_0();

  private void method_0()
  {
    this.Template = Application.Current.TryFindResource((object) (this.IconName ?? "")) as ControlTemplate;
  }
}
