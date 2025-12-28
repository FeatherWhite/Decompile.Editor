// Decompiled with JetBrains decompiler
// Type: OpenTap.Class51
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Windows;

#nullable disable
namespace OpenTap;

internal class Class51
{
  private static TraceSource traceSource_0 = Log.CreateSource("Clipboard");

  public static void smethod_0(string string_0)
  {
    try
    {
      Clipboard.SetText(string_0);
    }
    catch
    {
      try
      {
        Clipboard.SetDataObject((object) string_0);
      }
      catch (Exception ex)
      {
        Class51.traceSource_0.Error("Unable to copy data to the clipboard.");
        Class51.traceSource_0.Debug(ex);
      }
    }
  }
}
