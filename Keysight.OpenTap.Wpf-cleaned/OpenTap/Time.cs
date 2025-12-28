// Decompiled with JetBrains decompiler
// Type: OpenTap.Time
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;

#nullable disable
namespace OpenTap;

internal class Time
{
  public static TimeSpan FromSeconds(double seconds)
  {
    return TimeSpan.FromTicks((long) (seconds * 10000000.0));
  }
}
