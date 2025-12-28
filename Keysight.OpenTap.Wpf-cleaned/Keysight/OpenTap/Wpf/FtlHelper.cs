// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FtlHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Licensing;
using Keysight.OpenTap.Licensing.Providers;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class FtlHelper
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("Licensing");

  private static Func<string, DateTime> smethod_0()
  {
    IEnumerable<LicenseConfig> cachedLicenses = LicenseProviderFtl.GetCachedLicenses();
    Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
    foreach (LicenseConfig licenseConfig in cachedLicenses)
      dict[licenseConfig.Feature] = licenseConfig.Expiration;
    DateTime dateTime;
    return (Func<string, DateTime>) (feature => dict.TryGetValue(feature, out dateTime) ? dateTime : DateTime.MinValue);
  }

  private static Func<string, DateTime> smethod_1()
  {
    try
    {
      return FtlHelper.smethod_0();
    }
    catch
    {
      return (Func<string, DateTime>) (string_0 => DateTime.MinValue);
    }
  }

  public static void Check()
  {
    string environmentVariable = Environment.GetEnvironmentVariable("TAP_FT_MODE_LICENSES");
    if (environmentVariable != null)
    {
      int length = environmentVariable.IndexOf(":");
      int result;
      if (length == -1 || !int.TryParse(environmentVariable.Substring(0, length), out result))
        return;
      if (result != Process.GetCurrentProcess().Id)
        return;
      try
      {
        File.WriteAllBytes(".ftl_marker", Array.Empty<byte>());
      }
      catch
      {
      }
      Func<string, DateTime> func = FtlHelper.smethod_1();
      Log.Warning(FtlHelper.traceSource_0, "Failed to check out licenses: {0}. Continuing in fault tolerant mode.", new object[1]
      {
        (object) environmentVariable.Substring(length + 1)
      });
      string str1 = environmentVariable.Substring(length + 1);
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        DateTime dateTime = func(str2);
        if (dateTime > DateTime.MinValue && dateTime < DateTime.Now.AddYears(100))
          Log.Warning(FtlHelper.traceSource_0, "   Fault tolerant mode for {0} will expire at {1}.", new object[2]
          {
            (object) str2,
            (object) dateTime.ToLocalTime().ToString("dd MMMM yyyy HH':'mm':'ss")
          });
      }
    }
    else
    {
      if (!File.Exists(".ftl_marker"))
        return;
      try
      {
        File.Delete(".ftl_marker");
        Log.Warning(FtlHelper.traceSource_0, "   Fault tolerant mode exited.", Array.Empty<object>());
      }
      catch
      {
      }
    }
  }

  public static void MockFtLicenseWarning()
  {
    Environment.SetEnvironmentVariable("TAP_FT_MODE_LICENSES", $"{Process.GetCurrentProcess().Id}:TAP_Editor,TAP_Engine");
  }
}
