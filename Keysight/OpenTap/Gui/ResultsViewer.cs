// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ResultsViewer
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

public class ResultsViewer
{
  internal const string string_0 = "ResultsViewer.exe";
  public static OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Result navigator");

  internal static string ExePath
  {
    get
    {
      return Path.Combine(Path.GetDirectoryName(typeof (ResultsViewer).Assembly.Location), "ResultsViewer.exe");
    }
  }

  public static void Open(
    IResultStore resultStore,
    Window ownerWindow,
    string templatePath,
    int numberOfRuns)
  {
    try
    {
      string directoryName = Path.GetDirectoryName(typeof (ResultsViewer).Assembly.Location);
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.WorkingDirectory = directoryName;
      startInfo.FileName = Path.Combine(directoryName, "ResultsViewer.exe");
      if (!File.Exists(startInfo.FileName))
      {
        ResultsViewer.traceSource_0.Error("Could not find any installed Results Viewer.");
      }
      else
      {
        startInfo.Arguments = ComponentSettings.GetSaveFilePath(typeof (ResultSettings));
        if (resultStore != null)
        {
          ProcessStartInfo processStartInfo1 = startInfo;
          processStartInfo1.Arguments = $"{processStartInfo1.Arguments} --store {ComponentSettings<ResultSettings>.Current.IndexOf((object) resultStore).ToString()}";
          ProcessStartInfo processStartInfo2 = startInfo;
          processStartInfo2.Arguments = $"{processStartInfo2.Arguments} --load-latest {numberOfRuns.ToString()}";
        }
        if (!string.IsNullOrEmpty(templatePath))
        {
          ProcessStartInfo processStartInfo = startInfo;
          processStartInfo.Arguments = $"{processStartInfo.Arguments} --template \"{templatePath}\"";
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
      ResultsViewer.traceSource_0.Error("Caught error while opening Results Viewer.");
      ResultsViewer.traceSource_0.Debug(ex);
    }
  }
}
