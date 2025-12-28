// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.WindowUtility
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class WindowUtility
{
  private const uint uint_0 = 0;
  private const uint uint_1 = 1;
  private const uint uint_2 = 2;
  private const uint uint_3 = 3;
  private const uint uint_4 = 4;
  private const uint uint_5 = 12;

  [DllImport("user32.dll")]
  private static extern bool FlashWindowEx(ref WindowUtility.FLASHWINFO flashwinfo_0);

  private static WindowUtility.FLASHWINFO smethod_0(IntPtr intptr_0)
  {
    WindowUtility.FLASHWINFO structure = new WindowUtility.FLASHWINFO();
    structure.size = Convert.ToUInt32(Marshal.SizeOf<WindowUtility.FLASHWINFO>(structure));
    structure.hwnd = intptr_0;
    structure.count = uint.MaxValue;
    structure.flags = 15U;
    structure.timeout = 0U;
    return structure;
  }

  public static void Flash(IntPtr hwnd)
  {
    WindowUtility.FLASHWINFO flashwinfo_0 = WindowUtility.smethod_0(hwnd);
    WindowUtility.FlashWindowEx(ref flashwinfo_0);
  }

  private struct FLASHWINFO
  {
    public uint size;
    public IntPtr hwnd;
    public uint flags;
    public uint count;
    public uint timeout;
  }
}
