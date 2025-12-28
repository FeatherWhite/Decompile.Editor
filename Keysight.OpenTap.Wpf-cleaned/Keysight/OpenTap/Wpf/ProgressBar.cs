// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ProgressBar
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ProgressBar : UserControl, IComponentConnector
{
  public static readonly DependencyProperty NumeratorProperty = DependencyProperty.Register(nameof (Numerator), typeof (int), typeof (ProgressBar), new PropertyMetadata((object) 0, new PropertyChangedCallback(ProgressBar.smethod_0)));
  public static readonly DependencyProperty DenominatorProperty = DependencyProperty.Register(nameof (Denominator), typeof (int), typeof (ProgressBar), new PropertyMetadata((object) 100, new PropertyChangedCallback(ProgressBar.smethod_0)));
  internal ProgressBar @this;
  internal ColumnDefinition col1;
  internal ColumnDefinition col2;
  internal Label fracText;
  private bool bool_0;

  public int Numerator
  {
    get => (int) this.GetValue(ProgressBar.NumeratorProperty);
    set => this.SetValue(ProgressBar.NumeratorProperty, (object) value);
  }

  public int Denominator
  {
    get => (int) this.GetValue(ProgressBar.DenominatorProperty);
    set => this.SetValue(ProgressBar.DenominatorProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as ProgressBar).method_0();
  }

  private void method_0()
  {
    if (this.Numerator > this.Denominator)
      return;
    this.col1.Width = new GridLength((double) this.Numerator, GridUnitType.Star);
    this.col2.Width = new GridLength((double) (this.Denominator - this.Numerator), GridUnitType.Star);
    this.fracText.Content = (object) $"{this.Numerator} / {this.Denominator}";
  }

  public ProgressBar()
  {
    this.InitializeComponent();
    this.method_0();
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/progressbar.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.@this = (ProgressBar) target;
        break;
      case 2:
        this.col1 = (ColumnDefinition) target;
        break;
      case 3:
        this.col2 = (ColumnDefinition) target;
        break;
      case 4:
        this.fracText = (Label) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
