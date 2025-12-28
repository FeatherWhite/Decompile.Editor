// Decompiled with JetBrains decompiler
// Type: OpenTap.ShortTimeSpan
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace OpenTap;

internal struct ShortTimeSpan
{
  private ShortTimeSpan.UnitKind unitKind_0;

  public ShortTimeSpan.UnitKind Unit => this.unitKind_0;

  public double Value { get; set; }

  public double Seconds => this.Value * this.Scale;

  public static ShortTimeSpan FromSeconds(double seconds)
  {
    double num = seconds;
    ShortTimeSpan.UnitKind unitKind = ShortTimeSpan.UnitKind.Seconds;
    if (num < 1.0)
    {
      num = seconds * 1000.0;
      unitKind = ShortTimeSpan.UnitKind.Milliseconds;
      if (num < 1.0)
      {
        num = seconds * 1000000.0;
        unitKind = ShortTimeSpan.UnitKind.Microseconds;
        if (num < 1.0)
        {
          num = seconds * 1000000000.0;
          unitKind = ShortTimeSpan.UnitKind.Nanoseconds;
          if (num < 1.0)
            return new ShortTimeSpan();
        }
      }
    }
    return new ShortTimeSpan()
    {
      unitKind_0 = unitKind,
      Value = Math.Round(num, 3)
    };
  }

  public static ShortTimeSpan FromString(string string_0)
  {
    string_0 = string_0.Trim();
    ShortTimeSpan.UnitKind unitKind = ShortTimeSpan.UnitKind.Seconds;
    if (string_0.EndsWith("ms"))
      unitKind = ShortTimeSpan.UnitKind.Milliseconds;
    else if (!string_0.EndsWith("μs") && !string_0.EndsWith("us"))
    {
      if (string_0.EndsWith("ns"))
        unitKind = ShortTimeSpan.UnitKind.Nanoseconds;
      else
        string_0.EndsWith("s");
    }
    else
      unitKind = ShortTimeSpan.UnitKind.Microseconds;
    int num1 = 0;
    while (num1 < string_0.Length && (char.IsNumber(string_0[num1]) || string_0[num1] == '.'))
      ++num1;
    string_0 = string_0.Substring(0, num1);
    double num2 = double.Parse(string_0, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
    return new ShortTimeSpan()
    {
      Value = num2,
      unitKind_0 = unitKind
    };
  }

  private static string smethod_0(ShortTimeSpan.UnitKind unitKind_1)
  {
    switch (unitKind_1)
    {
      case ShortTimeSpan.UnitKind.Milliseconds:
        return "ms";
      case ShortTimeSpan.UnitKind.Microseconds:
        return "us";
      case ShortTimeSpan.UnitKind.Nanoseconds:
        return "ns";
      default:
        return "s";
    }
  }

  public double Scale
  {
    get
    {
      switch (this.unitKind_0)
      {
        case ShortTimeSpan.UnitKind.Milliseconds:
          return 0.001;
        case ShortTimeSpan.UnitKind.Microseconds:
          return 1E-06;
        case ShortTimeSpan.UnitKind.Nanoseconds:
          return 1E-09;
        default:
          return 1.0;
      }
    }
  }

  public override string ToString()
  {
    if (this.Value < 10.0)
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00} {1}", (object) (Math.Floor(this.Value * 100.0) * 0.01), (object) ShortTimeSpan.smethod_0(this.Unit));
    return this.Value < 100.0 ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0} {1}", (object) (Math.Round(this.Value * 10.0) * 0.1), (object) ShortTimeSpan.smethod_0(this.Unit)) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) Math.Floor(this.Value), (object) ShortTimeSpan.smethod_0(this.Unit));
  }

  public void ToString(StringBuilder output)
  {
    if (this.Value < 10.0)
      output.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00} {1}", (object) (Math.Floor(this.Value * 100.0) * 0.01), (object) ShortTimeSpan.smethod_0(this.Unit));
    else if (this.Value < 100.0)
      output.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0} {1}", (object) (Math.Round(this.Value * 10.0) * 0.1), (object) ShortTimeSpan.smethod_0(this.Unit));
    else
      output.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) Math.Floor(this.Value), (object) ShortTimeSpan.smethod_0(this.Unit));
  }

  public (string, string) ToStringParts()
  {
    if (this.Value < 10.0)
      return (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.00}", (object) (Math.Floor(this.Value * 100.0) * 0.01)), ShortTimeSpan.smethod_0(this.Unit));
    return this.Value < 100.0 ? (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.0}", (object) (Math.Floor(this.Value * 10.0) * 0.1)), ShortTimeSpan.smethod_0(this.Unit)) : (string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0}", (object) Math.Floor(this.Value)), ShortTimeSpan.smethod_0(this.Unit));
  }

  public enum UnitKind
  {
    Seconds,
    Milliseconds,
    Microseconds,
    Nanoseconds,
  }
}
