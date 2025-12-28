// Decompiled with JetBrains decompiler
// Type: OpenTap.FileSystemHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace OpenTap;

internal class FileSystemHelper
{
  internal static byte[] ByteOrderMark = new byte[3]
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
      FileSystemHelper.DeleteDirectory(directory);
  }

  public static void DeleteDirectory(string target_dir)
  {
    if (target_dir == null)
      throw new ArgumentNullException(nameof (target_dir));
    try
    {
      FileSystemHelper.smethod_0(target_dir);
      FileSystemHelper.smethod_1(target_dir);
      Directory.Delete(target_dir, false);
    }
    catch (Exception ex)
    {
    }
  }

  public static void EnsureDirectory(string filePath)
  {
    if (Directory.Exists(Path.GetDirectoryName(filePath)) || string.IsNullOrWhiteSpace(Path.GetDirectoryName(filePath)))
      return;
    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
  }

  public static string CreateTempDirectory()
  {
    string filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    FileSystemHelper.EnsureDirectory(filePath);
    return filePath;
  }

  public static string CreateTempFile()
  {
    return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
  }

  public static string GetCurrentInstallationDirectory() => ExecutorClient.ExeDir;

  internal static string GetRelativePath(string baseDirectory, string endDirectory)
  {
    string[] strArray1 = baseDirectory.Split('\\');
    string[] strArray2 = endDirectory.Split('\\');
    int count = FileSystemHelper.smethod_2(strArray1, strArray2);
    string str = Path.Combine(string.Join("\\", ((IEnumerable<string>) strArray1).Skip<string>(count).Select<string, string>((Func<string, string>) (string_0 => ".."))), string.Join("\\", ((IEnumerable<string>) strArray2).Skip<string>(count)));
    return !(str == string.Empty) ? str : ".";
  }

  private static int smethod_2(string[] string_0, string[] string_1)
  {
    int num = Math.Min(string_0.Length, string_1.Length);
    for (int index = 0; index < num; ++index)
    {
      if (string_0[index] != string_1[index])
        return index;
    }
    return num;
  }

  public static string GetAssemblyVersion(string assemblyPath)
  {
    assemblyPath = assemblyPath != null ? Path.GetFullPath(assemblyPath) : throw new ArgumentNullException(nameof (assemblyPath));
    return FileVersionInfo.GetVersionInfo(assemblyPath).ProductVersion;
  }

  public static void ModifyLines(Stream stream, bool pushBom, Func<string, int, string> modify)
  {
    FileSystemHelper.LineModifier lineModifier = new FileSystemHelper.LineModifier(stream, (Encoding) new UTF8Encoding(pushBom));
    try
    {
      int num = 0;
      while (true)
      {
        string str = lineModifier.ReadLine();
        if (str != null)
        {
          string line = modify(str, num++) ?? str;
          lineModifier.WriteLine(line);
        }
        else
          break;
      }
    }
    finally
    {
      lineModifier.Close();
    }
  }

  public static string EscapeBadPathChars(string path)
  {
    try
    {
      FileInfo fileInfo = new FileInfo(path);
    }
    catch
    {
      string path2 = Path.GetFileName(path);
      string path1 = Path.GetDirectoryName(path);
      foreach (char invalidPathChar in Path.GetInvalidPathChars())
        path1 = path1.Replace(invalidPathChar, '_');
      foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        path2 = path2.Replace(invalidFileNameChar, '_');
      path = Path.Combine(path1, path2);
      FileInfo fileInfo = new FileInfo(path);
    }
    return path;
  }

  public static string CreateUniqueFileName(string path)
  {
    if (!File.Exists(path))
      return path;
    string directoryName = Path.GetDirectoryName(path);
    string extension = Path.GetExtension(path);
    string withoutExtension = Path.GetFileNameWithoutExtension(path);
    for (int index = 2; index < 100000; ++index)
    {
      string path1 = Path.Combine(directoryName, $"{withoutExtension} ({index.ToString()}){extension}");
      if (!File.Exists(path1))
        return path1;
    }
    return path;
  }

  private class LineModifier
  {
    private readonly string string_0;
    private readonly Stream stream_0;
    private readonly Stream file;
    private readonly StreamReader streamReader_0;
    private readonly StreamWriter streamWriter_0;

    public void Close()
    {
      this.streamWriter_0.Flush();
      this.file.Flush();
      this.file.Seek(0L, SeekOrigin.Begin);
      this.file.SetLength(this.stream_0.Length);
      this.stream_0.Seek(0L, SeekOrigin.Begin);
      this.stream_0.CopyTo(this.file, 8192 /*0x2000*/);
      this.stream_0.Close();
      File.Delete(this.string_0);
    }

    public string ReadLine() => this.streamReader_0.ReadLine();

    public void WriteLine(string line) => this.streamWriter_0.WriteLine(line);

    public LineModifier(Stream file, Encoding encoding)
    {
      this.string_0 = FileSystemHelper.CreateTempFile();
      this.stream_0 = (Stream) File.Open(this.string_0, FileMode.Create);
      this.file = file;
      this.streamReader_0 = new StreamReader(file, encoding, false, 8192 /*0x2000*/);
      this.streamWriter_0 = new StreamWriter(this.stream_0, encoding, 8192 /*0x2000*/);
    }
  }
}
