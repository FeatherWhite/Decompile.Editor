// Decompiled with JetBrains decompiler
// Type: OpenTap.PathUtils
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace OpenTap;

internal class PathUtils
{
  private static HashSet<char> hashSet_0 = ((IEnumerable<char>) Path.GetInvalidFileNameChars()).ToHashset<char>();
  private static string string_0 = (string) null;

  public static bool IsValidFileName(string fileName)
  {
    return !string.IsNullOrEmpty(fileName) && !fileName.Any<char>(new Func<char, bool>(PathUtils.hashSet_0.Contains));
  }

  public static bool IsValidPath(string path)
  {
    if (string.IsNullOrWhiteSpace(path) || Regex.IsMatch(path.ToLower(), "http(s)?://"))
      return false;
    HashSet<char> hashset = path.ToHashset<char>();
    if (((IEnumerable<char>) Path.GetInvalidPathChars()).Intersect<char>((IEnumerable<char>) hashset).Any<char>())
      return false;
    try
    {
      FileInfo fileInfo = new FileInfo(path);
      return true;
    }
    catch
    {
      return false;
    }
  }

  public static string NormalizePath(string path)
  {
    string str = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    switch (Environment.OSVersion.Platform)
    {
      case PlatformID.Win32S:
      case PlatformID.Win32Windows:
      case PlatformID.Win32NT:
      case PlatformID.WinCE:
        return str.ToUpperInvariant();
      default:
        return str;
    }
  }

  public static bool AreEqual(string path1, string path2)
  {
    return PathUtils.NormalizePath(path1) == PathUtils.NormalizePath(path2);
  }

  public static IEnumerable<string> IterateDirectories(
    string rootPath,
    string patternMatch,
    SearchOption searchOption)
  {
    IEnumerator<string> enumerator1;
    if (searchOption == SearchOption.AllDirectories)
    {
      IEnumerable<string> strings = (IEnumerable<string>) Array.Empty<string>();
      try
      {
        strings = Directory.EnumerateDirectories(rootPath);
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      catch (PathTooLongException ex)
      {
      }
      enumerator1 = strings.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        IEnumerator<string> enumerator2 = PathUtils.IterateDirectories(enumerator1.Current, patternMatch, searchOption).GetEnumerator();
        while (enumerator2.MoveNext())
          yield return enumerator2.Current;
        // ISSUE: reference to a compiler-generated method
        this.method_1();
        enumerator2 = (IEnumerator<string>) null;
      }
      // ISSUE: reference to a compiler-generated method
      this.method_0();
      enumerator1 = (IEnumerator<string>) null;
    }
    IEnumerable<string> strings1 = (IEnumerable<string>) Array.Empty<string>();
    try
    {
      strings1 = Directory.EnumerateFiles(rootPath, patternMatch);
    }
    catch (UnauthorizedAccessException ex)
    {
    }
    enumerator1 = strings1.GetEnumerator();
    while (enumerator1.MoveNext())
      yield return enumerator1.Current;
    // ISSUE: reference to a compiler-generated method
    this.method_2();
    enumerator1 = (IEnumerator<string>) null;
  }

  private static bool smethod_0(FileStream fileStream_0, FileStream fileStream_1)
  {
    if (fileStream_0.Length != fileStream_1.Length)
      return false;
    byte[] buffer1 = new byte[4096 /*0x1000*/];
    byte[] buffer2 = new byte[4096 /*0x1000*/];
label_9:
    int num1 = fileStream_0.Read(buffer1, 0, 4096 /*0x1000*/);
    if (num1 == 0)
      return true;
    fileStream_1.Read(buffer2, 0, 4096 /*0x1000*/);
    int num2 = 512 /*0x0200*/;
    if (num1 < 4096 /*0x1000*/)
      num2 = num1 / 8 + 1;
    for (int index = 0; index < num2; ++index)
    {
      if (BitConverter.ToInt64(buffer1, index * 8) != BitConverter.ToInt64(buffer2, index * 8))
        return false;
    }
    goto label_9;
  }

  public static bool CompareFiles(string file1, string file2)
  {
    if (PathUtils.AreEqual(file1, file2))
      return true;
    try
    {
      using (FileStream fileStream_0 = File.Open(file1, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (FileStream fileStream_1 = File.Open(file2, FileMode.Open, FileAccess.Read, FileShare.Read))
          return PathUtils.smethod_0(fileStream_0, fileStream_1);
      }
    }
    catch
    {
      return false;
    }
  }

  public static string OpenTapDir
  {
    get
    {
      return PathUtils.string_0 ?? (PathUtils.string_0 = Path.GetDirectoryName(typeof (TestPlan).Assembly.Location));
    }
  }

  public class PathComparer : IEqualityComparer<string>
  {
    public bool Equals(string string_0, string string_1)
    {
      return PathUtils.NormalizePath(string_0) == PathUtils.NormalizePath(string_1);
    }

    public int GetHashCode(string string_0) => PathUtils.NormalizePath(string_0).GetHashCode();
  }
}
