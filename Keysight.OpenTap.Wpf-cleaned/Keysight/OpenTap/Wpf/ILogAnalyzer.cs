// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ILogAnalyzer
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Log Analyzer", null, null, -10000.0, false, null)]
public interface ILogAnalyzer : ITapPlugin
{
  void Load(Stream[] streams, ILogOrigin origin);

  void CompareWith(Stream[] streams, ILogOrigin origin);

  [XmlIgnore]
  Window ParentWindow { get; set; }

  void LoadProcess(IEnumerable<string> left, IEnumerable<string> right, bool memoryMappedApi);
}
