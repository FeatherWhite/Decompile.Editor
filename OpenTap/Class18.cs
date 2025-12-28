// Decompiled with JetBrains decompiler
// Type: OpenTap.Class18
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.IO;
using System.IO.Compression;

#nullable disable
namespace OpenTap;

internal static class Class18
{
  public static byte[] smethod_0(Stream stream_0)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      GZipStream destination = new GZipStream((Stream) memoryStream, CompressionMode.Compress);
      stream_0.CopyTo((Stream) destination);
      destination.Close();
      return memoryStream.ToArray();
    }
  }
}
