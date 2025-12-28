// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.PackageManagerToolProvider
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using OpenTap.Package;
using System;
using System.IO;
using System.Threading;
using Tap.Shared;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("Package Manager", null, null, -10000.0, false, null)]
public class PackageManagerToolProvider : ToolProvider
{
  public override string FileName => Path.GetFullPath("PackageManager.exe");

  public override bool Execute() => PackageManagerToolProvider.smethod_0((string) null);

  public bool Execute(ITypeData FocusType)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PackageManagerToolProvider.Class194 class194 = new PackageManagerToolProvider.Class194();
    // ISSUE: reference to a compiler-generated field
    class194.itypeData_0 = FocusType;
    using (Mutex mutex = LockingPackageAction.GetMutex(Class11.smethod_2()))
    {
      if (mutex.WaitOne(0))
        Class2.smethod_0("tap.exe packagemanager --force");
    }
    // ISSUE: reference to a compiler-generated field
    if (class194.itypeData_0 != null)
    {
      // ISSUE: reference to a compiler-generated method
      TapThread.Start(new Action(class194.method_0));
    }
    return true;
  }

  internal static bool smethod_0(string string_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PackageManagerToolProvider.Class195 class195 = new PackageManagerToolProvider.Class195();
    // ISSUE: reference to a compiler-generated field
    class195.string_0 = string_0;
    using (Mutex mutex = LockingPackageAction.GetMutex(Class11.smethod_2()))
    {
      if (mutex.WaitOne(0))
        Class2.smethod_0("tap.exe packagemanager --force");
    }
    // ISSUE: reference to a compiler-generated field
    if (class195.string_0 != null)
    {
      // ISSUE: reference to a compiler-generated method
      TapThread.Start(new Action(class195.method_0));
    }
    return true;
  }
}
