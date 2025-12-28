// Decompiled with JetBrains decompiler
// Type: OpenTap.OperatingSystem
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.IO;

#nullable disable
namespace OpenTap;

internal class OperatingSystem
{
  public static readonly OperatingSystem Windows = new OperatingSystem(nameof (Windows));
  public static readonly OperatingSystem Linux = new OperatingSystem(nameof (Linux));
  public static readonly OperatingSystem Unsupported = new OperatingSystem(nameof (Unsupported));
  private static OperatingSystem operatingSystem_0;

  public override string ToString() => this.Name;

  public string Name { get; }

  private OperatingSystem(string string_1) => this.Name = string_1;

  private static OperatingSystem smethod_0()
  {
    if (Path.DirectorySeparatorChar == '\\')
      return OperatingSystem.Windows;
    return Directory.Exists("/proc/") ? OperatingSystem.Linux : OperatingSystem.Unsupported;
  }

  public static OperatingSystem Current
  {
    get
    {
      if (OperatingSystem.operatingSystem_0 == null)
        OperatingSystem.operatingSystem_0 = OperatingSystem.smethod_0();
      return OperatingSystem.operatingSystem_0;
    }
  }
}
