// Decompiled with JetBrains decompiler
// Type: OpenTap.ClipboardHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Windows;

#nullable disable
namespace OpenTap;

internal class ClipboardHelper
{
  private static TraceSource traceSource_0 = Log.CreateSource("Clipboard");

  public static void CopyText(string text)
  {
    try
    {
      Clipboard.SetText(text);
    }
    catch
    {
      try
      {
        Clipboard.SetDataObject((object) text);
      }
      catch (Exception ex)
      {
        Log.Error(ClipboardHelper.traceSource_0, "Unable to copy data to the clipboard.", Array.Empty<object>());
        Log.Debug(ClipboardHelper.traceSource_0, ex);
      }
    }
  }
}
