// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.Argument
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap.Cli;

internal class Argument
{
  public char ShortName { get; private set; }

  public string LongName { get; private set; }

  public bool NeedsArgument { get; private set; }

  public string Value => this.Values.FirstOrDefault<string>();

  public List<string> Values { get; private set; }

  public string Description { get; set; }

  public bool IsVisible { get; set; } = true;

  public Argument(
    string longName,
    char shortName = '\0',
    bool needsArgument = true,
    string description = "",
    string defaultArg = null)
  {
    this.ShortName = shortName;
    this.LongName = longName;
    this.NeedsArgument = needsArgument;
    this.Description = description;
    this.Values = new List<string>();
    if (string.IsNullOrWhiteSpace(defaultArg))
      return;
    this.Values.Add(defaultArg);
  }

  public Argument Clone(string argument)
  {
    Argument obj = this.Clone();
    obj.Values = new List<string>() { argument };
    return obj;
  }

  public Argument Clone()
  {
    return new Argument(this.LongName, this.ShortName, this.NeedsArgument)
    {
      Values = new List<string>((IEnumerable<string>) this.Values),
      Description = this.Description
    };
  }

  public bool CompareTo(string string_2)
  {
    return string_2.Length == 1 && this.ShortName != char.MinValue ? (int) string_2[0] == (int) this.ShortName : string_2 == this.LongName;
  }
}
