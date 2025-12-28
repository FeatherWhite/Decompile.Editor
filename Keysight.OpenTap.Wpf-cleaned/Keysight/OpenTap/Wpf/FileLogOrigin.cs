// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FileLogOrigin
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.IO;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class FileLogOrigin : ILogOrigin
{
  public string Header { get; private set; }

  public string Details { get; private set; }

  public FileLogOrigin(string[] fileNames)
  {
    this.Header = "No files";
    if (fileNames.Length > 1)
      this.Header = $"{fileNames.Length} merged files";
    else if (fileNames.Length == 1)
      this.Header = Path.GetFileName(fileNames[0]);
    this.Details = string.Join(Environment.NewLine, fileNames);
  }
}
