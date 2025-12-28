// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.MixedColorBrush
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xaml;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[MarkupExtensionReturnType(typeof (SolidColorBrush))]
public class MixedColorBrush : MarkupExtension
{
  public string Brush1Key { get; set; }

  public string Brush2Key { get; set; }

  public double Amount { get; set; }

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
    SolidColorBrush solidColorBrush1 = (rootObject.Contains((object) this.Brush1Key) ? rootObject[(object) this.Brush1Key] : (object) null) as SolidColorBrush;
    SolidColorBrush solidColorBrush2 = (rootObject.Contains((object) this.Brush2Key) ? rootObject[(object) this.Brush2Key] : (object) null) as SolidColorBrush;
    if (solidColorBrush1 == null)
      return (object) null;
    if (solidColorBrush2 == null)
      return (object) solidColorBrush1;
    Color color1 = solidColorBrush1.Color;
    Color color2 = solidColorBrush2.Color;
    byte[] numArray1 = new byte[4]
    {
      color1.A,
      color1.R,
      color1.G,
      color1.B
    };
    byte[] numArray2 = new byte[4]
    {
      color2.A,
      color2.R,
      color2.G,
      color2.B
    };
    for (int index = 0; index < 4; ++index)
      numArray1[index] = (byte) ((double) numArray1[index] * (1.0 - this.Amount) + (double) numArray2[index] * this.Amount);
    return (object) new SolidColorBrush(Color.FromArgb(numArray1[0], numArray1[1], numArray1[2], numArray1[3]));
  }

  private object method_1() => Application.Current.TryFindResource((object) this.Brush1Key);
}
