// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FileAssociations
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal static class FileAssociations
{
  private static readonly Regex regex_0 = new Regex("(?:^|\\s)(\"(?:[^\"]+|\"\")*\"|[\\s]*)", RegexOptions.Compiled);

  public static string[] splitSpaces(string input)
  {
    List<string> stringList = new List<string>();
    foreach (Capture match in FileAssociations.regex_0.Matches(input))
    {
      string str = match.Value;
      if (str.Length == 0)
        stringList.Add("");
      stringList.Add(str.TrimStart(',').Trim().Trim('"').Trim());
    }
    return stringList.ToArray();
  }

  public static void OpenFileAssociation(string file, int line)
  {
    string extension = Path.GetExtension(file);
    FileAssociation fileAssociation = FileAssociationNative.GetFileAssociation(extension);
    if (fileAssociation == null)
      throw new FileAssociations.FileAssociationException(extension);
    if (string.Equals(extension, ".cs", StringComparison.InvariantCultureIgnoreCase))
    {
      if (fileAssociation.Identifier.StartsWith("Visual Studio Code", StringComparison.InvariantCultureIgnoreCase))
        Process.Start(Environment.ExpandEnvironmentVariables(FileAssociations.splitSpaces(fileAssociation.File)[0]), $"-g {file}:{line.ToString()}");
      else if (fileAssociation.Identifier.StartsWith("JetBrains Rider", StringComparison.InvariantCultureIgnoreCase))
        Process.Start(Environment.ExpandEnvironmentVariables(FileAssociations.splitSpaces(fileAssociation.File)[0]), $"{file}:{line.ToString()}");
      else if (fileAssociation.Identifier.StartsWith("Microsoft Visual Studio", StringComparison.InvariantCultureIgnoreCase))
        VisualStudioHelper.OpenHyperlinkInVs(file, line);
      else
        Process.Start(file);
    }
    else
      Process.Start(file);
  }

  public class FileAssociationException : Exception
  {
    public string Extension { get; set; }

    public FileAssociationException(string extension) => this.Extension = extension;

    public override string ToString() => "Cannot find file association for " + this.Extension;
  }
}
