// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SimpleColorPicker
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SimpleColorPicker : UserControl, IComponentConnector, IStyleConnector
{
  public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof (SelectedColor), typeof (SolidColorBrush), typeof (SimpleColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(SimpleColorPicker.smethod_0)));
  public static readonly DependencyProperty AvailableColorsProperty = DependencyProperty.Register(nameof (AvailableColors), typeof (IEnumerable<SolidColorBrush>), typeof (SimpleColorPicker));
  private bool bool_0;

  public event SimpleColorPicker.SelectedColorChangedDelegate SelectedColorChanged;

  private void method_0()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.selectedColorChangedDelegate_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.selectedColorChangedDelegate_0(this, this.SelectedColor);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as SimpleColorPicker).method_0();
  }

  public IEnumerable<SolidColorBrush> AvailableColors
  {
    get => (IEnumerable<SolidColorBrush>) this.GetValue(SimpleColorPicker.AvailableColorsProperty);
    set => this.SetValue(SimpleColorPicker.AvailableColorsProperty, (object) value);
  }

  public SolidColorBrush SelectedColor
  {
    get => this.GetValue(SimpleColorPicker.SelectedColorProperty) as SolidColorBrush;
    set => this.SetValue(SimpleColorPicker.SelectedColorProperty, (object) value);
  }

  public static List<SolidColorBrush> GetBrushes()
  {
    return new List<SolidColorBrush>()
    {
      new SolidColorBrush(Color.FromRgb((byte) 0, (byte) 95, (byte) 142)),
      new SolidColorBrush(Color.FromRgb((byte) 102, (byte) 153, (byte) 51)),
      new SolidColorBrush(Color.FromRgb((byte) 102, (byte) 51, (byte) 153)),
      new SolidColorBrush(Color.FromRgb((byte) 25, (byte) 146, (byte) 185)),
      new SolidColorBrush(Color.FromRgb((byte) 221, (byte) 136, (byte) 0)),
      new SolidColorBrush(Color.FromRgb((byte) 102, (byte) 51, (byte) 51)),
      new SolidColorBrush(Color.FromRgb((byte) 81, (byte) 110, (byte) 145)),
      new SolidColorBrush(Color.FromRgb((byte) 136, (byte) 85, (byte) 0)),
      new SolidColorBrush(Color.FromRgb((byte) 51, (byte) 51, (byte) 51))
    };
  }

  public SimpleColorPicker()
  {
    this.AvailableColors = (IEnumerable<SolidColorBrush>) SimpleColorPicker.GetBrushes();
    this.InitializeComponent();
  }

  private void method_1(object sender, MouseButtonEventArgs e)
  {
    this.SelectedColor = (sender as Border).DataContext as SolidColorBrush;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/simplecolorpicker.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target) => this.bool_0 = true;

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
      return;
    ((UIElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.method_1);
  }

  public delegate void SelectedColorChangedDelegate(
    SimpleColorPicker self,
    SolidColorBrush newBrush);
}
