// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class80
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using OpenTap.Package;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class80
{
  private Class80.Enum3 enum3_0;

  [SpecialName]
  public bool method_0() => this.enum3_0 != 0;

  private string method_1()
  {
    string[] strArray = new string[2]
    {
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
    };
    string path2 = "Common Files\\Keysight\\PathWave License Manager";
    string path3 = "pwlmgr.exe";
    foreach (string path1 in strArray)
    {
      string path = Path.Combine(path1, path2, path3);
      if (File.Exists(path))
        return path;
    }
    return (string) null;
  }

  private static string smethod_0()
  {
    string[] strArray = new string[2]
    {
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
    };
    string path2 = "Agilent\\Agilent License Manager\\";
    string path3 = "KeysightLicenseManager.exe";
    foreach (string path1 in strArray)
    {
      string path = Path.Combine(path1, path2, path3);
      if (File.Exists(path))
        return path;
    }
    return (string) null;
  }

  private string method_2()
  {
    if (this.enum3_0 == Class80.Enum3.const_1)
      return this.method_1();
    return this.enum3_0 == Class80.Enum3.const_2 ? Class80.smethod_0() : (string) null;
  }

  private static Class80 smethod_1()
  {
    List<PackageDef> packages = new Installation(Class11.smethod_2()).GetPackages();
    if (packages.FirstOrDefault<PackageDef>((Func<PackageDef, bool>) (packageDef_0 => packageDef_0.Name == "PathWave License Manager")) != null)
      return new Class80()
      {
        enum3_0 = Class80.Enum3.const_1
      };
    if (packages.FirstOrDefault<PackageDef>((Func<PackageDef, bool>) (packageDef_0 => packageDef_0.Name == "Keysight Fixed License Manager")) != null)
      return new Class80()
      {
        enum3_0 = Class80.Enum3.const_2
      };
    return new Class80() { enum3_0 = Class80.Enum3.const_0 };
  }

  public static void smethod_2()
  {
    Class80 class80 = Class80.smethod_1();
    if (!class80.method_0())
      return;
    class80.method_3();
  }

  private void method_3()
  {
    string fileName = this.method_2();
    if (fileName == null)
      return;
    Process.Start(fileName);
  }

  public static bool smethod_3() => Class80.smethod_1().method_0();

  private enum Enum3
  {
    const_0,
    const_1,
    const_2,
  }
}
