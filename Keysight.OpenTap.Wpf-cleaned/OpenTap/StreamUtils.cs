// Decompiled with JetBrains decompiler
// Type: OpenTap.StreamUtils
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.IO;
using System.IO.Compression;

#nullable disable
namespace OpenTap;

internal static class StreamUtils
{
  public static byte[] CompressStreamToBlob(Stream stream)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      GZipStream destination = new GZipStream((Stream) memoryStream, CompressionMode.Compress);
      stream.CopyTo((Stream) destination);
      destination.Close();
      return memoryStream.ToArray();
    }
  }
}
