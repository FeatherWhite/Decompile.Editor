// Decompiled with JetBrains decompiler
// Type: OpenTap.KeysightVisaDeviceDiscovery
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace OpenTap;

[Browsable(false)]
public class KeysightVisaDeviceDiscovery : IDeviceDiscovery, ITapPlugin
{
  private IEnumerable<string> method_0(string string_0)
  {
    try
    {
      RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
      try
      {
        RegistryKey registryKey2 = registryKey1.OpenSubKey(string_0, RegistryKeyPermissionCheck.ReadSubTree);
        if (registryKey2 == null)
          return (IEnumerable<string>) new List<string>();
        string[] valueNames = registryKey2.GetValueNames();
        registryKey2.Close();
        return (IEnumerable<string>) valueNames;
      }
      finally
      {
        registryKey1.Close();
      }
    }
    catch
    {
      return (IEnumerable<string>) new List<string>();
    }
  }

  private IEnumerable<string> method_1(string string_0)
  {
    return this.method_0(string_0 + "\\Devices").Concat<string>(this.method_0(string_0 + "\\VisaDevices"));
  }

  private IEnumerable<string> method_2(string string_0_1)
  {
    List<string> stringList = new List<string>();
    try
    {
      RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
      try
      {
        RegistryKey registryKey2 = registryKey1.OpenSubKey(string_0_1, RegistryKeyPermissionCheck.ReadSubTree);
        if (registryKey2 != null)
        {
          foreach (string name1 in ((IEnumerable<string>) registryKey2.GetSubKeyNames()).Where<string>((Func<string, bool>) (string_0_2 => string_0_2.StartsWith("INTF"))))
          {
            RegistryKey registryKey3 = registryKey2.OpenSubKey(name1, false);
            if (registryKey3 != null)
            {
              string VisaName = (string) registryKey3.GetValue("VisaName");
              if (VisaName != null)
                stringList.AddRange(((IEnumerable<string>) registryKey3.GetSubKeyNames()).Select<string, string>((Func<string, string>) (name => $"{VisaName}::{name}")));
              registryKey3.Close();
            }
          }
          registryKey2.Close();
        }
      }
      finally
      {
        registryKey1.Close();
      }
    }
    catch
    {
    }
    return (IEnumerable<string>) stringList;
  }

  public string[] DetectDeviceAddresses(DeviceAddressAttribute AddressType)
  {
    List<string> stringList = new List<string>()
    {
      "SOFTWARE\\Keysight\\IO Libraries Suite\\CurrentVersion",
      "SOFTWARE\\Wow6432Node\\Keysight\\IO Libraries Suite\\CurrentVersion",
      "SOFTWARE\\Keysight\\IO Libraries Suite\\CurrentVersion",
      "SOFTWARE\\Agilent\\IO Libraries\\CurrentVersion"
    };
    List<string> source = new List<string>();
    foreach (string string_0 in stringList)
    {
      try
      {
        source.AddRange(this.method_1(string_0));
      }
      catch
      {
      }
    }
    foreach (string string_0 in stringList)
    {
      try
      {
        source.AddRange(this.method_2(string_0));
      }
      catch
      {
      }
    }
    return source.Distinct<string>().ToArray<string>();
  }

  public bool CanDetect(DeviceAddressAttribute DeviceAddress)
  {
    return DeviceAddress is VisaAddressAttribute;
  }
}
