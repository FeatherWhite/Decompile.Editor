// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FractionMatcher
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class FractionMatcher
{
  private static Regex regex_0 = new Regex("(\\[(?<num>[0-9]+)/(?<denom>[0-9]+)\\])", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

  public static FractionMatcher.FractionMatch? TryMatchFraction(string string_0)
  {
    Match match = FractionMatcher.regex_0.Match(string_0);
    if (!match.Success)
      return new FractionMatcher.FractionMatch?();
    return new FractionMatcher.FractionMatch?(new FractionMatcher.FractionMatch()
    {
      Fraction = new FractionMatcher.Fraction()
      {
        Numerator = int.Parse(match.Groups["num"].Value),
        Denominator = int.Parse(match.Groups["denom"].Value)
      },
      Start = match.Index,
      Length = match.Length
    });
  }

  public struct Fraction
  {
    public int Numerator;
    public int Denominator;
  }

  public struct FractionMatch
  {
    public FractionMatcher.Fraction Fraction;
    public int Start;
    public int Length;
  }
}
