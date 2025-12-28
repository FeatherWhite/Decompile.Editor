// Decompiled with JetBrains decompiler
// Type: Tap.Shared.MurMurHash3
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.IO;
using System.Text;

#nullable disable
namespace Tap.Shared;

internal static class MurMurHash3
{
  private const uint uint_0 = 144 /*0x90*/;

  public static int Hash(byte[] bytes)
  {
    using (MemoryStream memoryStream = new MemoryStream(bytes))
      return MurMurHash3.Hash((Stream) memoryStream);
  }

  public static int Hash(string uniqueString)
  {
    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(uniqueString)))
      return MurMurHash3.Hash((Stream) memoryStream);
  }

  public static int Hash(Stream stream)
  {
    uint num1 = 144 /*0x90*/;
    uint num2 = 0;
    Stream stream1 = stream;
    byte[] buffer = new byte[4];
    while (true)
    {
      int num3 = stream1.Read(buffer, 0, 4);
      if (num3 > 0)
      {
        num2 += (uint) num3;
        switch (num3 - 1)
        {
          case 0:
            uint num4 = MurMurHash3.smethod_0((uint) buffer[0] * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num4;
            continue;
          case 1:
            uint num5 = MurMurHash3.smethod_0(((uint) buffer[0] | (uint) buffer[1] << 8) * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num5;
            continue;
          case 2:
            uint num6 = MurMurHash3.smethod_0((uint) ((int) buffer[0] | (int) buffer[1] << 8 | (int) buffer[2] << 16 /*0x10*/) * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num6;
            continue;
          case 3:
            uint num7 = MurMurHash3.smethod_0((uint) ((int) buffer[0] | (int) buffer[1] << 8 | (int) buffer[2] << 16 /*0x10*/ | (int) buffer[3] << 24) * 3432918353U, (byte) 15) * 461845907U;
            num1 = (uint) ((int) MurMurHash3.smethod_0(num1 ^ num7, (byte) 13) * 5 - 430675100);
            continue;
          default:
            continue;
        }
      }
      else
        break;
    }
    return (int) MurMurHash3.smethod_1(num1 ^ num2);
  }

  private static uint smethod_0(uint uint_1, byte byte_0)
  {
    return uint_1 << (int) byte_0 | uint_1 >> 32 /*0x20*/ - (int) byte_0;
  }

  private static uint smethod_1(uint uint_1)
  {
    uint_1 ^= uint_1 >> 16 /*0x10*/;
    uint_1 *= 2246822507U;
    uint_1 ^= uint_1 >> 13;
    uint_1 *= 3266489909U;
    uint_1 ^= uint_1 >> 16 /*0x10*/;
    return uint_1;
  }
}
