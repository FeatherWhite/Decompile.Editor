// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.StringToLogControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class StringToLogControl
{
  private static readonly Regex regex_0 = new Regex("(?<path>[a-zA-Z]:\\\\(?:[^<>|?*<\":>\\+\\[\\]/]+(?!:)))", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
  private static readonly Regex regex_1 = new Regex("((?:https:|http:|ftp:|file:/)//[/\\w\\-_\\.%:]+(?::\\d+)?(?:[\\?\\w\\.\\=\\-\\&\\#\\@\\%\\$\\/\\!]*)?)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

  private static List<StringToLogControl.matchItem> smethod_0(Regex regex_2, Match match_0)
  {
    List<StringToLogControl.matchItem> matchItemList = new List<StringToLogControl.matchItem>();
    for (; match_0.Success; match_0 = match_0.NextMatch())
      matchItemList.Add(new StringToLogControl.matchItem(regex_2, match_0));
    return matchItemList;
  }

  private static string smethod_1(string string_0)
  {
    IEnumerable<string> strings = string_0.SplitPreserve('\\', ' ', '\'', '"');
    StringBuilder stringBuilder = new StringBuilder(string_0.Length);
    string str1 = (string) null;
    foreach (string str2 in strings)
    {
      stringBuilder.Append(str2);
      string path = stringBuilder.ToString();
      if (Directory.Exists(path) || File.Exists(path))
        str1 = path;
    }
    return str1;
  }

  private static List<StringToLogControl.matchItem> smethod_2(string string_0)
  {
    List<StringToLogControl.matchItem> matchItemList1 = StringToLogControl.smethod_0(StringToLogControl.regex_0, StringToLogControl.regex_0.Match(string_0));
    List<StringToLogControl.matchItem> source = new List<StringToLogControl.matchItem>();
    for (int index = 0; index < matchItemList1.Count; ++index)
    {
      StringToLogControl.matchItem matchItem = matchItemList1[index];
      string str = StringToLogControl.smethod_1(string_0.Substring(matchItem.Position, matchItem.Length));
      if (str != null)
      {
        matchItem.Length = str.Length;
        source.Add(matchItem);
      }
    }
    StringToLogControl.matchItem[] array = source.ToList<StringToLogControl.matchItem>().Concat<StringToLogControl.matchItem>((IEnumerable<StringToLogControl.matchItem>) StringToLogControl.smethod_0(StringToLogControl.regex_1, StringToLogControl.regex_1.Match(string_0))).Where<StringToLogControl.matchItem>((Func<StringToLogControl.matchItem, bool>) (match => match.Length > 3)).OrderBy<StringToLogControl.matchItem, int>((Func<StringToLogControl.matchItem, int>) (match => match.Position)).ToArray<StringToLogControl.matchItem>();
    List<StringToLogControl.matchItem> matchItemList2 = new List<StringToLogControl.matchItem>();
    for (int index = 0; index < array.Length; ++index)
    {
      StringToLogControl.matchItem matchItem1 = array[index];
      int num1 = matchItem1.Position + matchItem1.Length;
      bool flag = true;
      if (index + 1 < array.Length)
      {
        StringToLogControl.matchItem matchItem2 = array[index + 1];
        while (matchItem2.Position < num1 && flag && index + 1 < array.Length)
        {
          if (matchItem1.Regex == StringToLogControl.regex_1 && matchItem2.Regex == StringToLogControl.regex_0 && matchItem2.Position - matchItem1.Position == 8)
          {
            int num2 = matchItem2.Position + matchItem2.Length;
            matchItem1.Length = num2 - matchItem1.Position;
            ++index;
            if (index + 1 < array.Length)
              matchItem2 = array[index + 1];
          }
          else
            flag = false;
        }
      }
      if (flag)
        matchItemList2.Add(matchItem1);
    }
    return matchItemList2;
  }

  private static List<StringToLogControl.splitItem> smethod_3(string string_0)
  {
    StringToLogControl.matchItem[] array = StringToLogControl.smethod_2(string_0).ToArray();
    if (array.Length == 0)
      return new List<StringToLogControl.splitItem>()
      {
        new StringToLogControl.splitItem()
        {
          Result = string_0,
          Kind = StringToLogControl.splitKind.String
        }
      };
    List<StringToLogControl.splitItem> source = new List<StringToLogControl.splitItem>();
    int startIndex = 0;
    foreach (StringToLogControl.matchItem matchItem1 in array)
    {
      StringToLogControl.splitItem splitItem = new StringToLogControl.splitItem();
      StringToLogControl.matchItem matchItem2 = matchItem1;
      string str1 = string_0.Substring(startIndex, matchItem2.Position - startIndex);
      string str2 = string_0.Substring(matchItem2.Position, matchItem2.Length);
      int num = str2.Reverse<char>().IndexWhen<char>((Func<char, bool>) (char_0 => !char.IsWhiteSpace(char_0)));
      if (num > 0)
      {
        matchItem2.Length -= num;
        str2 = string_0.Substring(matchItem2.Position, matchItem2.Length);
      }
      if (str1.Length > 0)
      {
        splitItem.Result = str1;
        source.Add(splitItem);
      }
      startIndex = matchItem2.Position + matchItem2.Length;
      string uriString = matchItem2.Regex == StringToLogControl.regex_0 ? StringToLogControl.smethod_1(str2) : str2;
      splitItem.Result = uriString;
      if (uriString != null)
      {
        try
        {
          splitItem.Uri = new Uri(uriString);
          splitItem.Kind = StringToLogControl.splitKind.Uri;
        }
        catch (Exception ex)
        {
          splitItem.Kind = StringToLogControl.splitKind.Local;
        }
        source.Add(splitItem);
      }
    }
    string str = string_0.Substring(startIndex);
    StringToLogControl.splitItem splitItem1;
    if (str.Length > 0)
    {
      List<StringToLogControl.splitItem> splitItemList = source;
      splitItem1 = new StringToLogControl.splitItem();
      splitItem1.Result = str;
      StringToLogControl.splitItem splitItem2 = splitItem1;
      splitItemList.Add(splitItem2);
    }
    if (!source.All<StringToLogControl.splitItem>((Func<StringToLogControl.splitItem, bool>) (item => item.Kind == StringToLogControl.splitKind.String)))
      return source;
    List<StringToLogControl.splitItem> splitItemList1 = new List<StringToLogControl.splitItem>();
    splitItem1 = new StringToLogControl.splitItem();
    splitItem1.Result = string_0;
    splitItemList1.Add(splitItem1);
    return splitItemList1;
  }

  private static object smethod_4(string string_0)
  {
    List<StringToLogControl.splitItem> splitItemList = StringToLogControl.smethod_3(string_0);
    if (splitItemList.Count == 0)
      return (object) string_0;
    List<object> source = new List<object>();
    foreach (StringToLogControl.splitItem splitItem in splitItemList)
    {
      if (splitItem.Kind == StringToLogControl.splitKind.Uri)
      {
        try
        {
          HyperlinkText hyperlinkText = new HyperlinkText()
          {
            Text = splitItem.Result
          };
          source.Add((object) hyperlinkText);
        }
        catch (Exception ex)
        {
          source.Add((object) string_0);
        }
      }
      else
        source.Add((object) splitItem.Result);
    }
    return source.All<object>((Func<object, bool>) (item => item is string)) ? (object) string_0 : (object) source;
  }

  private static bool smethod_5(string string_0)
  {
    if (string_0.Length >= 300 || !string_0.Contains(":"))
      return false;
    return string_0.Contains("\\") || string_0.Contains("/");
  }

  public static FractionMatcher.FractionMatch? IsProgressMessage(string string_0)
  {
    return FractionMatcher.TryMatchFraction(string_0);
  }

  public static object SplitHyperLinks(string string_0)
  {
    return !StringToLogControl.smethod_5(string_0) ? (object) string_0 : StringToLogControl.smethod_4(string_0);
  }

  private struct matchItem
  {
    public int Position;
    public int Length;
    public Regex Regex;

    public matchItem(Regex regex, Match match)
    {
      this.Regex = regex;
      this.Position = match.Index;
      this.Length = match.Length;
    }
  }

  private enum splitKind
  {
    String,
    Uri,
    Local,
  }

  private struct splitItem
  {
    public StringToLogControl.splitKind Kind;
    public string Result;
    public Uri Uri;
  }
}
