// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.StringSearch
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class StringSearch
{
  public static bool Contains(string input, string search, bool caseInv)
  {
    return input.IndexOf(search, 0, input.Length, caseInv ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) >= 0;
  }

  public static List<StringSearch.StringSearchResult> StringFind(
    string input,
    string search,
    bool caseInv)
  {
    List<StringSearch.StringSearchResult> stringSearchResultList = new List<StringSearch.StringSearchResult>();
    int startIndex = 0;
    while (true)
    {
      int num = input.IndexOf(search, startIndex, input.Length - startIndex, StringComparison.InvariantCultureIgnoreCase);
      if (num != -1)
      {
        stringSearchResultList.Add(new StringSearch.StringSearchResult()
        {
          Index = num,
          Pattern = search
        });
        startIndex = num + search.Length;
      }
      else
        break;
    }
    return stringSearchResultList;
  }

  public struct StringSearchResult
  {
    public int Index;
    public string Pattern;
  }
}
