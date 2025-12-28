// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.CommunityEdition
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

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

internal static class CommunityEdition
{
  private static int int_0 = 3;
  private static string string_0 = "SHA1";
  private static int int_1 = 32 /*0x20*/;
  private static byte[] byte_0 = Encoding.ASCII.GetBytes("dqg63928163gdg19");
  private static byte[] byte_1 = Encoding.ASCII.GetBytes("dk38s6ghqjclth6u");

  public static string Encrypt(string input, string password)
  {
    byte[] bytes1 = Encoding.UTF8.GetBytes(input);
    byte[] array;
    using (Aes aes = Aes.Create())
    {
      byte[] bytes2 = new PasswordDeriveBytes(password, CommunityEdition.byte_1, CommunityEdition.string_0, CommunityEdition.int_0).GetBytes(CommunityEdition.int_1);
      aes.Mode = CipherMode.ECB;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ICryptoTransform encryptor = aes.CreateEncryptor(bytes2, CommunityEdition.byte_0))
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

  public static string GetGenericUpdateId()
  {
    PhysicalAddress physicalAddress = ((IEnumerable<NetworkInterface>) NetworkInterface.GetAllNetworkInterfaces()).Where<NetworkInterface>((Func<NetworkInterface, bool>) (networkInterface_0 => networkInterface_0.OperationalStatus == OperationalStatus.Up)).Select<NetworkInterface, PhysicalAddress>((Func<NetworkInterface, PhysicalAddress>) (networkInterface_0 => networkInterface_0.GetPhysicalAddress())).FirstOrDefault<PhysicalAddress>();
    byte[] numArray = new byte[8];
    physicalAddress?.GetAddressBytes().CopyTo((Array) numArray, 0);
    string uniqueString = BitConverter.ToString(numArray).Replace("-", string.Empty);
    string exeDir = ExecutorClient.ExeDir;
    return $"{MurMurHash3.Hash(uniqueString):X8}{MurMurHash3.Hash(exeDir):X8}";
  }

  public static string GetUpdateId(string string_1 = null)
  {
    if (string_1 == null)
      string_1 = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Keysight", false).OpenSubKey("Test Automation", false).GetValue("EULA Accept") as string;
    return CommunityEdition.GetGenericUpdateId() + CommunityEdition.Encrypt(string_1, "dk38dfhs.2mv91eh38dht");
  }

  public static bool CheckEulaAccept()
  {
    RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Keysight", false);
    if (registryKey1 == null)
      return false;
    RegistryKey registryKey2 = registryKey1.OpenSubKey("Test Automation", false);
    return registryKey2 != null && registryKey2.GetValue("EULA Accept") is string;
  }
}
