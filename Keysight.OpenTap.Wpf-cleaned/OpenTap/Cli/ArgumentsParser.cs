// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.ArgumentsParser
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap.Cli;

internal class ArgumentsParser
{
  public ArgumentCollection AllOptions = new ArgumentCollection();

  private ArgumentsParser.optionFindResult method_0(
    string string_0,
    ArgumentCollection argumentCollection_0)
  {
    ArgumentsParser.optionFindResult optionFindResult = new ArgumentsParser.optionFindResult()
    {
      IsUnknown = false
    };
    int startIndex = 0;
    if (string_0.StartsWith("--"))
      startIndex = 2;
    else if (string_0.StartsWith("-"))
      startIndex = 1;
    if (startIndex == 0)
      return optionFindResult;
    string_0 = string_0.Substring(startIndex);
    int length = string_0.IndexOf('=');
    if (length > 0)
    {
      optionFindResult.InlineArg = string_0.Substring(length + 1);
      string_0 = string_0.Substring(0, length);
    }
    optionFindResult.FoundOption = argumentCollection_0.Where<KeyValuePair<string, Argument>>((Func<KeyValuePair<string, Argument>, bool>) (keyValuePair_0 => keyValuePair_0.Value.CompareTo(string_0))).FirstOrDefault<KeyValuePair<string, Argument>>().Value;
    if (optionFindResult.FoundOption == null)
      optionFindResult.FoundOption = this.AllOptions.Where<KeyValuePair<string, Argument>>((Func<KeyValuePair<string, Argument>, bool>) (keyValuePair_0 => keyValuePair_0.Value.CompareTo(string_0))).FirstOrDefault<KeyValuePair<string, Argument>>().Value;
    if (optionFindResult.FoundOption == null)
      optionFindResult.IsUnknown = true;
    else
      optionFindResult.FoundOption = optionFindResult.FoundOption.Clone();
    return optionFindResult;
  }

  public ArgumentCollection Parse(string[] rawArgs)
  {
    ArgumentCollection argumentCollection_0 = this.AllOptions.CreateDefault();
    List<string> list = ((IEnumerable<string>) argumentCollection_0.UnnamedArguments).ToList<string>();
    for (int index = 0; index < rawArgs.Length; ++index)
    {
      string rawArg = rawArgs[index];
      ArgumentsParser.optionFindResult optionFindResult = this.method_0(rawArg, argumentCollection_0);
      Argument foundOption = optionFindResult.FoundOption;
      if (foundOption == null)
      {
        if (!optionFindResult.IsUnknown)
          list.Add(rawArg);
        else
          argumentCollection_0.UnknownsOptions.Add(rawArg);
      }
      else
      {
        if (foundOption.NeedsArgument)
        {
          if (optionFindResult.InlineArg != null)
            foundOption.Values.Add(optionFindResult.InlineArg);
          else if (index + 1 < rawArgs.Length)
          {
            foundOption.Values.Add(rawArgs[++index]);
          }
          else
          {
            argumentCollection_0.MissingArguments.Add(foundOption);
            continue;
          }
        }
        else if (optionFindResult.InlineArg != null)
          foundOption.Values.Add(optionFindResult.InlineArg);
        argumentCollection_0.Add(foundOption);
      }
    }
    argumentCollection_0.UnnamedArguments = list.ToArray();
    return argumentCollection_0;
  }

  private struct optionFindResult
  {
    public Argument FoundOption;
    public bool IsUnknown;
    public string InlineArg;
  }
}
