// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.RunExplorer
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class RunExplorer
{
  internal const string string_0 = "RunExplorer.exe";
  public static OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Result navigator");

  internal static string ExePath
  {
    get
    {
      return Path.Combine(Path.GetDirectoryName(typeof (ResultsViewer).Assembly.Location), "RunExplorer.exe");
    }
  }

  public static void Open(IResultStore resultStore, Window ownerWindow)
  {
    try
    {
      string directoryName = Path.GetDirectoryName(typeof (ResultsViewer).Assembly.Location);
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.WorkingDirectory = directoryName;
      startInfo.FileName = Path.Combine(directoryName, "RunExplorer.exe");
      if (!File.Exists(startInfo.FileName))
      {
        RunExplorer.traceSource_0.Error("Could not find the Run Explorer application. This could be because the OpenTAP Results Viewer plugin is broken or missing.");
      }
      else
      {
        startInfo.Arguments = ComponentSettings.GetSaveFilePath(typeof (ResultSettings));
        startInfo.Arguments += $" --pid {Process.GetCurrentProcess().Id}";
        if (resultStore != null)
        {
          ProcessStartInfo processStartInfo = startInfo;
          processStartInfo.Arguments = $"{processStartInfo.Arguments} --store {ComponentSettings<ResultSettings>.Current.IndexOf((object) resultStore).ToString()}";
        }
        if (ownerWindow != null)
        {
          if (ownerWindow.WindowState != WindowState.Maximized)
          {
            int num1 = (int) (ownerWindow.Left + ownerWindow.ActualWidth / 2.0);
            int num2 = (int) (ownerWindow.Top + ownerWindow.ActualHeight / 2.0);
            startInfo.Arguments += $" -l {num1},{num2}";
          }
          else
          {
            Screen screen = Screen.FromHandle(new WindowInteropHelper(ownerWindow).Handle);
            int num3 = screen.WorkingArea.Left + screen.WorkingArea.Width / 2;
            int num4 = screen.WorkingArea.Top + screen.WorkingArea.Height / 2;
            startInfo.Arguments += $" -l {num3},{num4}";
          }
        }
        Process.Start(startInfo);
      }
    }
    catch (Exception ex)
    {
      RunExplorer.traceSource_0.Error("Caught error while opening Results Viewer.");
      RunExplorer.traceSource_0.Debug(ex);
    }
  }
}
