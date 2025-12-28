// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.RunExplorerToolProvider
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.IO;
using System.Linq;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("Run Explorer", "Open the Run Explorer Tool.", null, -10000.0, false, null)]
public class RunExplorerToolProvider : ToolProvider
{
  public override string FileName => Path.GetFullPath("RunExplorer.exe");

  public override bool Execute() => this.method_0();

  private bool method_0()
  {
    CallbackChannels.smethod_0(Application.Current.MainWindow as MainWindow);
    RunExplorer.Open(ComponentSettings<ResultSettings>.Current.FirstOrDefault<IResultListener>((Func<IResultListener, bool>) (iresultListener_0 => iresultListener_0 is IResultStore)) as IResultStore, (Window) (Application.Current.MainWindow as MainWindow));
    return true;
  }
}
