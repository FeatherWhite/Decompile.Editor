// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FileAssociationNative
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal static class FileAssociationNative
{
  [DllImport("Shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern uint AssocQueryString(
    FileAssociationNative.AccocKind accocKind_0,
    FileAssociationNative.AssocString assocString_0,
    string string_0,
    string string_1,
    [Out] StringBuilder stringBuilder_0,
    [In, Out] ref uint uint_0);

  public static FileAssociation GetFileAssociation(string extension)
  {
    string str1 = FileAssociationNative.smethod_0(FileAssociationNative.AssocString.Command, extension);
    string str2 = FileAssociationNative.smethod_0(FileAssociationNative.AssocString.FriendlyAppName, extension);
    if (str1 == null || str2 == null)
      return (FileAssociation) null;
    return new FileAssociation()
    {
      Identifier = str2,
      File = str1
    };
  }

  private static string smethod_0(FileAssociationNative.AssocString assocString_0, string string_0)
  {
    uint uint_0 = 0;
    int num = (int) FileAssociationNative.AssocQueryString(FileAssociationNative.AccocKind.Verify, assocString_0, string_0, (string) null, (StringBuilder) null, ref uint_0);
    if (uint_0 == 0U)
      return (string) null;
    StringBuilder stringBuilder_0 = new StringBuilder((int) uint_0);
    return FileAssociationNative.AssocQueryString(FileAssociationNative.AccocKind.Verify, assocString_0, string_0, (string) null, stringBuilder_0, ref uint_0) != 0U ? (string) null : stringBuilder_0.ToString();
  }

  [Flags]
  public enum AccocKind
  {
    Init_NoRemapCLSID = 1,
    Init_ByExeName = 2,
    Open_ByExeName = Init_ByExeName, // 0x00000002
    Init_DefaultToStar = 4,
    Init_DefaultToFolder = 8,
    NoUserSettings = 16, // 0x00000010
    NoTruncate = 32, // 0x00000020
    Verify = 64, // 0x00000040
    RemapRunDll = 128, // 0x00000080
    NoFixUps = 256, // 0x00000100
    IgnoreBaseClass = 512, // 0x00000200
  }

  public enum AssocString
  {
    Command = 1,
    Executable = 2,
    FriendlyDocName = 3,
    FriendlyAppName = 4,
    NoOpen = 5,
    ShellNewValue = 6,
    DDECommand = 7,
    DDEIfExec = 8,
    DDEApplication = 9,
    DDETopic = 10, // 0x0000000A
  }
}
