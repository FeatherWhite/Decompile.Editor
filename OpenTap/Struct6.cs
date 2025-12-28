// Decompiled with JetBrains decompiler
// Type: OpenTap.Struct6
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace OpenTap;

internal struct Struct6
{
  private Struct6.Enum2 enum2_0;

  [SpecialName]
  public Struct6.Enum2 method_0() => this.enum2_0;

  [CompilerGenerated]
  [SpecialName]
  public double method_1() => this.double_0;

  [CompilerGenerated]
  [SpecialName]
  public void method_2(double double_1) => this.double_0 = double_1;

  [SpecialName]
  public double method_3() => this.method_1() * this.method_4();

  public static Struct6 smethod_0(double double_1)
  {
    double num = double_1;
    Struct6.Enum2 enum2 = Struct6.Enum2.const_0;
    if (num < 1.0)
    {
      num = double_1 * 1000.0;
      enum2 = Struct6.Enum2.const_1;
      if (num < 1.0)
      {
        num = double_1 * 1000000.0;
        enum2 = Struct6.Enum2.const_2;
        if (num < 1.0)
        {
          num = double_1 * 1000000000.0;
          enum2 = Struct6.Enum2.const_3;
          if (num < 1.0)
            return new Struct6();
        }
      }
    }
    Struct6 struct6 = new Struct6();
    struct6.enum2_0 = enum2;
    struct6.method_2(Math.Round(num, 3));
    return struct6;
  }

  public static Struct6 smethod_1(string string_0)
  {
    string_0 = string_0.Trim();
    Struct6.Enum2 enum2 = Struct6.Enum2.const_0;
    if (string_0.EndsWith("ms"))
      enum2 = Struct6.Enum2.const_1;
    else if (!string_0.EndsWith("μs") && !string_0.EndsWith("us"))
    {
      if (string_0.EndsWith("ns"))
        enum2 = Struct6.Enum2.const_3;
      else
        string_0.EndsWith("s");
    }
    else
      enum2 = Struct6.Enum2.const_2;
    int num = 0;
    while (num < string_0.Length && (char.IsNumber(string_0[num]) || string_0[num] == '.'))
      ++num;
    string_0 = string_0.Substring(0, num);
    double double_1 = double.Parse(string_0, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
    Struct6 struct6 = new Struct6();
    struct6.method_2(double_1);
    struct6.enum2_0 = enum2;
    return struct6;
  }

  private static string smethod_2(Struct6.Enum2 enum2_1)
  {
    switch (enum2_1)
    {
      case Struct6.Enum2.const_1:
        return "ms";
      case Struct6.Enum2.const_2:
        return "us";
      case Struct6.Enum2.const_3:
        return "ns";
      default:
        return "s";
    }
  }

  [SpecialName]
  public double method_4()
  {
    switch (this.enum2_0)
    {
      case Struct6.Enum2.const_1:
        return 0.001;
      case Struct6.Enum2.const_2:
        return 1E-06;
      case Struct6.Enum2.const_3:
        return 1E-09;
      default:
        return 1.0;
    }
  }

  public override string ToString()
  {
    if (this.method_1() < 10.0)
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00} {1}", (object) (Math.Floor(this.method_1() * 100.0) * 0.01), (object) Struct6.smethod_2(this.method_0()));
    return this.method_1() < 100.0 ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0} {1}", (object) (Math.Round(this.method_1() * 10.0) * 0.1), (object) Struct6.smethod_2(this.method_0())) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) Math.Floor(this.method_1()), (object) Struct6.smethod_2(this.method_0()));
  }

  public void method_5(StringBuilder stringBuilder_0)
  {
    if (this.method_1() < 10.0)
      stringBuilder_0.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00} {1}", (object) (Math.Floor(this.method_1() * 100.0) * 0.01), (object) Struct6.smethod_2(this.method_0()));
    else if (this.method_1() < 100.0)
      stringBuilder_0.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0} {1}", (object) (Math.Round(this.method_1() * 10.0) * 0.1), (object) Struct6.smethod_2(this.method_0()));
    else
      stringBuilder_0.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) Math.Floor(this.method_1()), (object) Struct6.smethod_2(this.method_0()));
  }

  public (string, string) method_6()
  {
    if (this.method_1() < 10.0)
      return (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00}", (object) (Math.Floor(this.method_1() * 100.0) * 0.01)), Struct6.smethod_2(this.method_0()));
    return this.method_1() < 100.0 ? (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0}", (object) (Math.Floor(this.method_1() * 10.0) * 0.1)), Struct6.smethod_2(this.method_0())) : (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0}", (object) Math.Floor(this.method_1())), Struct6.smethod_2(this.method_0()));
  }

  public enum Enum2
  {
    const_0,
    const_1,
    const_2,
    const_3,
  }
}
