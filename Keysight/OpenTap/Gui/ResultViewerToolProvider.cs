// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ResultViewerToolProvider
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System.IO;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("Results Viewer", "Open the Results Viewer, showing the most recent test plan runs result.", null, -10000.0, false, null)]
public class ResultViewerToolProvider : ToolProvider
{
  public override string FileName => Path.GetFullPath("ResultsViewer.exe");

  public override bool Execute() => this.method_0();

  private bool method_0()
  {
    (Application.Current.MainWindow as MainWindow).ResultsOther_Click((object) this, (RoutedEventArgs) null);
    return true;
  }
}
