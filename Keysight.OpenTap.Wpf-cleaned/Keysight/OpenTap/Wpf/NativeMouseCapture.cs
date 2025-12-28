// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NativeMouseCapture
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class NativeMouseCapture
{
  public static void ReleaseAnyCapture()
  {
    if (!(NativeMouseCapture.GetCapture() != IntPtr.Zero))
      return;
    NativeMouseCapture.ReleaseCapture();
  }

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern IntPtr GetCapture();

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool ReleaseCapture();
}
