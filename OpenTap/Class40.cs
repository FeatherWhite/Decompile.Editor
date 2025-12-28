// Decompiled with JetBrains decompiler
// Type: OpenTap.Class40
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;

#nullable disable
namespace OpenTap;

internal class Class40
{
  public static TimeSpan smethod_0(double double_0)
  {
    return TimeSpan.FromTicks((long) (double_0 * 10000000.0));
  }
}
