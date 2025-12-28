// Decompiled with JetBrains decompiler
// Type: OpenTap.Cli.ArgumentCollection
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace OpenTap.Cli;

internal class ArgumentCollection : Dictionary<string, OpenTap.Cli.Argument>
{
  public string[] UnnamedArguments { get; set; }

  public List<string> UnknownsOptions { get; set; }

  public List<OpenTap.Cli.Argument> MissingArguments { get; set; }

  public ArgumentCollection()
  {
    this.UnnamedArguments = new string[0];
    this.UnknownsOptions = new List<string>();
    this.MissingArguments = new List<OpenTap.Cli.Argument>();
  }

  public OpenTap.Cli.Argument Add(OpenTap.Cli.Argument option)
  {
    this[option.LongName] = option;
    return option;
  }

  public OpenTap.Cli.Argument Add(
    string longName,
    char shortName = '\0',
    bool needsArgument = true,
    string description = "",
    string defaultArgument = null)
  {
    return this.Add(new OpenTap.Cli.Argument(longName, shortName, needsArgument, description, defaultArgument));
  }

  public bool Contains(string optionName) => this.ContainsKey(optionName);

  public OpenTap.Cli.Argument GetOrDefault(string optionLongName)
  {
    return this.Contains(optionLongName) ? this[optionLongName] : (OpenTap.Cli.Argument) null;
  }

  public string Argument(string optionLongName) => this[optionLongName].Value;

  public string GetArgumentOrDefault(string optionLongName, string string_1 = null)
  {
    OpenTap.Cli.Argument orDefault = this.GetOrDefault(optionLongName);
    return orDefault != null ? orDefault.Value : string_1;
  }

  public IEnumerable<string> GetArgumentValues(string optionLongName)
  {
    OpenTap.Cli.Argument obj;
    return this.TryGetValue(optionLongName, out obj) ? (IEnumerable<string>) obj.Values : (IEnumerable<string>) Array.Empty<string>();
  }

  public OpenTap.Cli.Argument TakeOption(string optionName, ArgumentCollection from)
  {
    if (!from.Contains(optionName))
      return (OpenTap.Cli.Argument) null;
    this[optionName] = from[optionName].Clone();
    return this[optionName];
  }

  public ArgumentCollection CreateDefault()
  {
    ArgumentCollection argumentCollection = new ArgumentCollection()
    {
      UnnamedArguments = this.UnnamedArguments
    };
    foreach (string key in this.Keys)
    {
      if (this[key].Value != null)
        argumentCollection[key] = this[key];
    }
    return argumentCollection;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, OpenTap.Cli.Argument> keyValuePair in (Dictionary<string, OpenTap.Cli.Argument>) this)
    {
      OpenTap.Cli.Argument obj = keyValuePair.Value;
      string str1 = "--" + obj.LongName;
      if (obj.ShortName != char.MinValue)
        str1 = $"-{obj.ShortName}, {str1}";
      string str2 = "  " + str1;
      string description = obj.Description;
      char[] chArray = new char[1]{ '\n' };
      foreach (string str3 in description.Split(chArray))
      {
        string str4 = str2 + new string(' ', 22 - str2.Length) + str3;
        stringBuilder.AppendLine(str4);
        str2 = "";
      }
    }
    return stringBuilder.ToString();
  }
}
