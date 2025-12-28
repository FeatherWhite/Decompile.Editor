// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.RenderDispatch
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class RenderDispatch
{
  private static int int_0;
  private static readonly List<EventHandler<EventArgs>> list_0 = new List<EventHandler<EventArgs>>();
  private static readonly List<EventHandler<EventArgs>> list_1 = new List<EventHandler<EventArgs>>();
  private static readonly List<EventHandler<EventArgs>> list_2 = new List<EventHandler<EventArgs>>();
  private static long long_0 = 0;

  static RenderDispatch()
  {
    CompositionTarget.Rendering += new EventHandler(RenderDispatch.smethod_1);
    RenderDispatch.RenderingSlow += new EventHandler<EventArgs>(RenderDispatch.smethod_0);
  }

  private static void smethod_0(object sender, EventArgs e)
  {
    ++RenderDispatch.int_0;
    if (RenderDispatch.int_0 % 4 != 0)
      return;
    for (int index = 0; index < RenderDispatch.list_2.Count; ++index)
    {
      if ((RenderDispatch.long_0 + (long) index) % 4L == 0L)
        RenderDispatch.list_2[index](sender, e);
    }
  }

  public static event EventHandler<EventArgs> Rendering
  {
    add => RenderDispatch.list_0.Add(value);
    remove => RenderDispatch.list_0.Remove(value);
  }

  public static event EventHandler<EventArgs> RenderingSlow
  {
    add => RenderDispatch.list_1.Add(value);
    remove => RenderDispatch.list_1.Remove(value);
  }

  public static event EventHandler<EventArgs> RenderingGlacial
  {
    add => RenderDispatch.list_2.Add(value);
    remove => RenderDispatch.list_2.Remove(value);
  }

  private static void smethod_1(object sender, EventArgs e)
  {
    ++RenderDispatch.long_0;
    for (int index = 0; index < RenderDispatch.list_1.Count; ++index)
    {
      if ((RenderDispatch.long_0 + (long) index) % 4L == 0L)
        RenderDispatch.list_1[index](sender, e);
    }
    for (int index = 0; index < RenderDispatch.list_0.Count; ++index)
      RenderDispatch.list_0[index](sender, e);
  }
}
