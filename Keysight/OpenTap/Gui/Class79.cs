// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class79
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Licensing;
using OpenTap;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal static class Class79
{
  private static object object_0;
  private static object object_1;

  [License("TAP_Editor|KS8400A", null, "Test Automation Editor", LicenseType.NotSpecified)]
  public static void smethod_0()
  {
    if (!Class61.smethod_0())
      return;
    Class79.smethod_2(ref Class79.object_0, "CheckLicense");
  }

  [License("TAP_Editor|KS8400A|TAP_Engine|KS8000A", null, "Test Automation Shell", LicenseType.NotSpecified)]
  public static void smethod_1()
  {
    if (!Class61.smethod_0())
      return;
    Class79.smethod_2(ref Class79.object_1, "CheckLicenseShell");
  }

  private static void smethod_2(ref object object_2, [CallerMemberName] string string_0 = null)
  {
    MethodInfo method = typeof (Class79).GetMethod(string_0);
    LicenseAttribute licenseAttribute = (object) method != null ? method.smethod_7<LicenseAttribute>() : (LicenseAttribute) null;
    if (licenseAttribute == null)
      return;
    if (object_2 == null)
      object_2 = LicenseManager.CheckLicenses(licenseAttribute.LicenseString, licenseAttribute.MinVersion);
    if (!LicenseManager.VerifyLicense(object_2))
      throw new LicenseException(licenseAttribute.LicenseString, licenseAttribute.Feature);
  }
}
