// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SimpleListCompletionProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class SimpleListCompletionProvider : IAutoCompleteProvider
{
  private readonly IEnumerable ienumerable_0;

  public SimpleListCompletionProvider(IEnumerable list)
  {
    this.ienumerable_0 = list ?? (IEnumerable) Array.Empty<string>();
  }

  public List<AutoCompleteItem> GetSuggestions(string text, int position)
  {
    text = text ?? "";
    IEnumerable ienumerable0 = this.ienumerable_0;
    object source;
    if (ienumerable0 == null)
    {
      source = (object) null;
    }
    else
    {
      source = (object) ienumerable0.Cast<object>().Select<object, string>((Func<object, string>) (object_0 => object_0.ToString()));
      if (source != null)
        goto label_4;
    }
    source = (object) Array.Empty<string>();
label_4:
    return ((IEnumerable<string>) source).Where<string>((Func<string, bool>) (string_0 => string_0.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0 && string_0 != text)).Select<string, AutoCompleteItem>((Func<string, AutoCompleteItem>) (string_0 => new AutoCompleteItem(string_0))).ToList<AutoCompleteItem>();
  }
}
