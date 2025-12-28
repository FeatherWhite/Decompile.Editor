// Decompiled with JetBrains decompiler
// Type: OpenTap.Class12
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace OpenTap;

internal class Class12
{
  internal static byte[] byte_0 = new byte[3]
  {
    (byte) 239,
    (byte) 187,
    (byte) 191
  };

  private static void smethod_0(string string_0)
  {
    foreach (string file in Directory.GetFiles(string_0))
    {
      File.SetAttributes(file, FileAttributes.Normal);
      File.Delete(file);
    }
  }

  private static void smethod_1(string string_0)
  {
    foreach (string directory in Directory.GetDirectories(string_0))
      Class12.smethod_2(directory);
  }

  public static void smethod_2(string string_0)
  {
    if (string_0 == null)
      throw new ArgumentNullException("target_dir");
    try
    {
      Class12.smethod_0(string_0);
      Class12.smethod_1(string_0);
      Directory.Delete(string_0, false);
    }
    catch (Exception ex)
    {
    }
  }

  public static void smethod_3(string string_0)
  {
    if (Directory.Exists(Path.GetDirectoryName(string_0)) || string.IsNullOrWhiteSpace(Path.GetDirectoryName(string_0)))
      return;
    Directory.CreateDirectory(Path.GetDirectoryName(string_0));
  }

  public static string smethod_4()
  {
    string string_0 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    Class12.smethod_3(string_0);
    return string_0;
  }

  public static string smethod_5() => Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

  public static string smethod_6() => Class11.smethod_2();

  internal static string smethod_7(string string_0_1, string string_1)
  {
    string[] strArray1 = string_0_1.Split('\\');
    string[] strArray2 = string_1.Split('\\');
    int count = Class12.smethod_8(strArray1, strArray2);
    string str = Path.Combine(string.Join("\\", ((IEnumerable<string>) strArray1).Skip<string>(count).Select<string, string>((Func<string, string>) (string_0_2 => ".."))), string.Join("\\", ((IEnumerable<string>) strArray2).Skip<string>(count)));
    return !(str == string.Empty) ? str : ".";
  }

  private static int smethod_8(string[] string_0, string[] string_1)
  {
    int num = Math.Min(string_0.Length, string_1.Length);
    for (int index = 0; index < num; ++index)
    {
      if (string_0[index] != string_1[index])
        return index;
    }
    return num;
  }

  public static string smethod_9(string string_0)
  {
    string_0 = string_0 != null ? Path.GetFullPath(string_0) : throw new ArgumentNullException("assemblyPath");
    return FileVersionInfo.GetVersionInfo(string_0).ProductVersion;
  }

  public static void smethod_10(Stream stream_0, bool bool_0, Func<string, int, string> func_0)
  {
    Class12.Class13 class13 = new Class12.Class13(stream_0, (Encoding) new UTF8Encoding(bool_0));
    try
    {
      int num = 0;
      while (true)
      {
        string str = class13.method_1();
        if (str != null)
        {
          string string_1 = func_0(str, num++) ?? str;
          class13.method_2(string_1);
        }
        else
          break;
      }
    }
    finally
    {
      class13.method_0();
    }
  }

  public static string smethod_11(string string_0)
  {
    try
    {
      FileInfo fileInfo = new FileInfo(string_0);
    }
    catch
    {
      string path2 = Path.GetFileName(string_0);
      string path1 = Path.GetDirectoryName(string_0);
      foreach (char invalidPathChar in Path.GetInvalidPathChars())
        path1 = path1.Replace(invalidPathChar, '_');
      foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        path2 = path2.Replace(invalidFileNameChar, '_');
      string_0 = Path.Combine(path1, path2);
      FileInfo fileInfo = new FileInfo(string_0);
    }
    return string_0;
  }

  public static string smethod_12(string string_0)
  {
    if (!File.Exists(string_0))
      return string_0;
    string directoryName = Path.GetDirectoryName(string_0);
    string extension = Path.GetExtension(string_0);
    string withoutExtension = Path.GetFileNameWithoutExtension(string_0);
    for (int index = 2; index < 100000; ++index)
    {
      string path = Path.Combine(directoryName, $"{withoutExtension} ({index.ToString()}){extension}");
      if (!File.Exists(path))
        return path;
    }
    return string_0;
  }

  private class Class13
  {
    private readonly string string_0;
    private readonly Stream stream_0;
    private readonly Stream stream_1;
    private readonly StreamReader streamReader_0;
    private readonly StreamWriter streamWriter_0;

    public void method_0()
    {
      this.streamWriter_0.Flush();
      this.stream_1.Flush();
      this.stream_1.Seek(0L, SeekOrigin.Begin);
      this.stream_1.SetLength(this.stream_0.Length);
      this.stream_0.Seek(0L, SeekOrigin.Begin);
      this.stream_0.CopyTo(this.stream_1, 8192 /*0x2000*/);
      this.stream_0.Close();
      File.Delete(this.string_0);
    }

    public string method_1() => this.streamReader_0.ReadLine();

    public void method_2(string string_1) => this.streamWriter_0.WriteLine(string_1);

    public Class13(Stream stream_2, Encoding encoding_0)
    {
      this.string_0 = Class12.smethod_5();
      this.stream_0 = (Stream) File.Open(this.string_0, FileMode.Create);
      this.stream_1 = stream_2;
      this.streamReader_0 = new StreamReader(stream_2, encoding_0, false, 8192 /*0x2000*/);
      this.streamWriter_0 = new StreamWriter(this.stream_0, encoding_0, 8192 /*0x2000*/);
    }
  }
}
