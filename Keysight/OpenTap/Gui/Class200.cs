// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class200
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Microsoft.Win32;
using OpenTap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Tap.Shared;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal static class Class200
{
  private static int int_0 = 3;
  private static string string_0 = "SHA1";
  private static int int_1 = 32 /*0x20*/;
  private static byte[] byte_0 = Encoding.ASCII.GetBytes("dqg63928163gdg19");
  private static byte[] byte_1 = Encoding.ASCII.GetBytes("dk38s6ghqjclth6u");

  public static string smethod_0(string string_1, string string_2)
  {
    byte[] bytes1 = Encoding.UTF8.GetBytes(string_1);
    byte[] array;
    using (Aes aes = Aes.Create())
    {
      byte[] bytes2 = new PasswordDeriveBytes(string_2, Class200.byte_1, Class200.string_0, Class200.int_0).GetBytes(Class200.int_1);
      aes.Mode = CipherMode.ECB;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ICryptoTransform encryptor = aes.CreateEncryptor(bytes2, Class200.byte_0))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            cryptoStream.Write(bytes1, 0, bytes1.Length);
            cryptoStream.FlushFinalBlock();
            array = memoryStream.ToArray();
          }
        }
      }
      aes.Clear();
    }
    return BitConverter.ToString(array).Replace("-", string.Empty);
  }

  public static string smethod_1()
  {
    PhysicalAddress physicalAddress = ((IEnumerable<NetworkInterface>) NetworkInterface.GetAllNetworkInterfaces()).Where<NetworkInterface>((Func<NetworkInterface, bool>) (networkInterface_0 => networkInterface_0.OperationalStatus == OperationalStatus.Up)).Select<NetworkInterface, PhysicalAddress>((Func<NetworkInterface, PhysicalAddress>) (networkInterface_0 => networkInterface_0.GetPhysicalAddress())).FirstOrDefault<PhysicalAddress>();
    byte[] numArray = new byte[8];
    physicalAddress?.GetAddressBytes().CopyTo((Array) numArray, 0);
    string string_0_1 = BitConverter.ToString(numArray).Replace("-", string.Empty);
    string string_0_2 = Class11.smethod_2();
    return $"{Class1.smethod_1(string_0_1):X8}{Class1.smethod_1(string_0_2):X8}";
  }

  public static string smethod_2(string string_1 = null)
  {
    if (string_1 == null)
      string_1 = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Keysight", false).OpenSubKey("Test Automation", false).GetValue("EULA Accept") as string;
    return Class200.smethod_1() + Class200.smethod_0(string_1, "dk38dfhs.2mv91eh38dht");
  }

  public static bool smethod_3()
  {
    RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Keysight", false);
    if (registryKey1 == null)
      return false;
    RegistryKey registryKey2 = registryKey1.OpenSubKey("Test Automation", false);
    return registryKey2 != null && registryKey2.GetValue("EULA Accept") is string;
  }
}
