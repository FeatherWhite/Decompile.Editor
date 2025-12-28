// Decompiled with JetBrains decompiler
// Type: OpenTap.Class5
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

#nullable disable
namespace OpenTap;

internal class Class5
{
  private static HashSet<char> hashSet_0 = ((IEnumerable<char>) Path.GetInvalidFileNameChars()).smethod_21<char>();
  private static string string_0 = (string) null;

  public static bool smethod_0(string string_1)
  {
    return !string.IsNullOrEmpty(string_1) && !string_1.Any<char>(new Func<char, bool>(Class5.hashSet_0.Contains));
  }

  public static bool smethod_1(string string_1)
  {
    if (string.IsNullOrWhiteSpace(string_1) || Regex.IsMatch(string_1.ToLower(), "http(s)?://"))
      return false;
    HashSet<char> second = string_1.smethod_21<char>();
    if (((IEnumerable<char>) Path.GetInvalidPathChars()).Intersect<char>((IEnumerable<char>) second).Any<char>())
      return false;
    try
    {
      FileInfo fileInfo = new FileInfo(string_1);
      return true;
    }
    catch
    {
      return false;
    }
  }

  public static string smethod_2(string string_1)
  {
    string str = Path.GetFullPath(string_1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
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

  public static bool smethod_3(string string_1, string string_2)
  {
    return Class5.smethod_2(string_1) == Class5.smethod_2(string_2);
  }

  public static IEnumerable<string> smethod_4(
    string string_1,
    string string_2,
    SearchOption searchOption_0)
  {
    IEnumerator<string> enumerator1;
    if (searchOption_0 == SearchOption.AllDirectories)
    {
      IEnumerable<string> strings = (IEnumerable<string>) Array.Empty<string>();
      try
      {
        strings = Directory.EnumerateDirectories(string_1);
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
        IEnumerator<string> enumerator2 = Class5.smethod_4(enumerator1.Current, string_2, searchOption_0).GetEnumerator();
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
      strings1 = Directory.EnumerateFiles(string_1, string_2);
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

  private static bool smethod_5(FileStream fileStream_0, FileStream fileStream_1)
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

  public static bool smethod_6(string string_1, string string_2)
  {
    if (Class5.smethod_3(string_1, string_2))
      return true;
    try
    {
      using (FileStream fileStream_0 = File.Open(string_1, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (FileStream fileStream_1 = File.Open(string_2, FileMode.Open, FileAccess.Read, FileShare.Read))
          return Class5.smethod_5(fileStream_0, fileStream_1);
      }
    }
    catch
    {
      return false;
    }
  }

  [SpecialName]
  public static string smethod_7()
  {
    return Class5.string_0 ?? (Class5.string_0 = Path.GetDirectoryName(typeof (TestPlan).Assembly.Location));
  }

  public class Class6 : IEqualityComparer<string>
  {
    public bool Equals(string string_0, string string_1)
    {
      return Class5.smethod_2(string_0) == Class5.smethod_2(string_1);
    }

    public int GetHashCode(string string_0) => Class5.smethod_2(string_0).GetHashCode();
  }
}
