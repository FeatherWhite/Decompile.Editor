// Decompiled with JetBrains decompiler
// Type: Tap.Shared.Class1
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.IO;
using System.Text;

#nullable disable
namespace Tap.Shared;

internal static class Class1
{
  private const uint uint_0 = 144 /*0x90*/;

  public static int smethod_0(byte[] byte_0)
  {
    using (MemoryStream stream_0 = new MemoryStream(byte_0))
      return Class1.smethod_2((Stream) stream_0);
  }

  public static int smethod_1(string string_0)
  {
    using (MemoryStream stream_0 = new MemoryStream(Encoding.UTF8.GetBytes(string_0)))
      return Class1.smethod_2((Stream) stream_0);
  }

  public static int smethod_2(Stream stream_0)
  {
    uint num1 = 144 /*0x90*/;
    uint num2 = 0;
    Stream stream = stream_0;
    byte[] buffer = new byte[4];
    while (true)
    {
      int num3 = stream.Read(buffer, 0, 4);
      if (num3 > 0)
      {
        num2 += (uint) num3;
        switch (num3 - 1)
        {
          case 0:
            uint num4 = Class1.smethod_3((uint) buffer[0] * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num4;
            continue;
          case 1:
            uint num5 = Class1.smethod_3(((uint) buffer[0] | (uint) buffer[1] << 8) * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num5;
            continue;
          case 2:
            uint num6 = Class1.smethod_3((uint) ((int) buffer[0] | (int) buffer[1] << 8 | (int) buffer[2] << 16 /*0x10*/) * 3432918353U, (byte) 15) * 461845907U;
            num1 ^= num6;
            continue;
          case 3:
            uint num7 = Class1.smethod_3((uint) ((int) buffer[0] | (int) buffer[1] << 8 | (int) buffer[2] << 16 /*0x10*/ | (int) buffer[3] << 24) * 3432918353U, (byte) 15) * 461845907U;
            num1 = (uint) ((int) Class1.smethod_3(num1 ^ num7, (byte) 13) * 5 - 430675100);
            continue;
          default:
            continue;
        }
      }
      else
        break;
    }
    return (int) Class1.smethod_4(num1 ^ num2);
  }

  private static uint smethod_3(uint uint_1, byte byte_0)
  {
    return uint_1 << (int) byte_0 | uint_1 >> 32 /*0x20*/ - (int) byte_0;
  }

  private static uint smethod_4(uint uint_1)
  {
    uint_1 ^= uint_1 >> 16 /*0x10*/;
    uint_1 *= 2246822507U;
    uint_1 ^= uint_1 >> 13;
    uint_1 *= 3266489909U;
    uint_1 ^= uint_1 >> 16 /*0x10*/;
    return uint_1;
  }
}
