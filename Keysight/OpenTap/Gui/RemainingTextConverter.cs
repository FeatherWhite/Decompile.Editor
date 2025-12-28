// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.RemainingTextConverter
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class RemainingTextConverter : IMultiValueConverter
{
  public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(parameter is string str1))
      str1 = "";
    string str2 = str1;
    Struct6 struct6_1;
    double num1;
    bool flag;
    try
    {
      struct6_1 = Struct6.smethod_0((double) value[0]);
      num1 = System.Convert.ToDouble(value[1]);
      flag = (bool) value[2];
    }
    catch (Exception ex)
    {
      return (object) "";
    }
    if (flag)
    {
      if (num1 == 0.0)
        return (object) $"{struct6_1}";
      Struct6 struct6_2 = Struct6.smethod_0(Math.Abs(num1 - struct6_1.method_3()));
      int num2 = Math.Sign(num1 - struct6_1.method_3());
      return str2.Equals("slim") ? (num2 > 0 ? (object) $"-{struct6_2}" : (object) $"+{struct6_2}") : (num2 > 0 ? (object) $"{struct6_2} remaining" : (object) $"{struct6_2} overtime");
    }
    string str3 = "Completed in";
    if (object.Equals(((IEnumerable<object>) value).ElementAtOrDefault<object>(3), (object) Verdict.Aborted))
      str3 = "Aborted after";
    if (object.Equals(((IEnumerable<object>) value).ElementAtOrDefault<object>(3), (object) Verdict.Error))
      str3 = "Ended after";
    Struct6.smethod_0(Math.Abs(num1 - struct6_1.method_3()));
    int num3 = Math.Sign(num1 - struct6_1.method_3());
    return num1 == 0.0 ? (str2.Equals("slim") ? (object) $"{struct6_1}" : (object) string.Format("{1} {0}", (object) struct6_1, (object) str3)) : (str2.Equals("slim") ? (num3 > 0 ? (object) $"{struct6_1} (-{(num1 - struct6_1.method_3()) / num1 * 100.0:0} %)" : (object) $"{struct6_1} (+{(struct6_1.method_3() - num1) / num1 * 100.0:0} %)") : (num3 > 0 ? (object) string.Format("{2} {0:0.00} ({1:0}% faster than average)", (object) struct6_1, (object) ((num1 - struct6_1.method_3()) / num1 * 100.0), (object) str3) : (object) string.Format("{2} {0:0.00} ({1:0}% slower than average)", (object) struct6_1, (object) ((struct6_1.method_3() - num1) / num1 * 100.0), (object) str3)));
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
