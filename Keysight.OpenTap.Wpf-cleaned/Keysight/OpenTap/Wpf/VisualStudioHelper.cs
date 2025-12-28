// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.VisualStudioHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using EnvDTE;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class VisualStudioHelper
{
  [DllImport("ole32.dll")]
  private static extern void CreateBindCtx(int int_0, out IBindCtx ibindCtx_0);

  [DllImport("ole32.dll")]
  private static extern int GetRunningObjectTable(
    int int_0,
    out IRunningObjectTable irunningObjectTable_0);

  public static IEnumerable<DTE> GetRunningInstance()
  {
    IRunningObjectTable irunningObjectTable_0;
    if (VisualStudioHelper.GetRunningObjectTable(0, out irunningObjectTable_0) == 0)
    {
      IEnumMoniker ppenumMoniker;
      irunningObjectTable_0.EnumRunning(out ppenumMoniker);
      IntPtr zero = IntPtr.Zero;
      IMoniker[] rgelt = new IMoniker[1];
      while (ppenumMoniker.Next(1, rgelt, zero) == 0)
      {
        IBindCtx ibindCtx_0;
        VisualStudioHelper.CreateBindCtx(0, out ibindCtx_0);
        string ppszDisplayName;
        rgelt[0].GetDisplayName(ibindCtx_0, (IMoniker) null, out ppszDisplayName);
        Console.WriteLine("Display Name: {0}", (object) ppszDisplayName);
        if (ppszDisplayName.StartsWith("!VisualStudio"))
        {
          object ppunkObject;
          irunningObjectTable_0.GetObject(rgelt[0], out ppunkObject);
          yield return ppunkObject as DTE;
        }
      }
      rgelt = (IMoniker[]) null;
    }
  }

  public static DTE GetNewInstance()
  {
    try
    {
      return (DTE) Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE"));
    }
    catch (Exception ex1)
    {
      TraceSource source = Log.CreateSource("VSHelper");
      Log.Debug(source, ex1);
      try
      {
        return (DTE) Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE.12.0"));
      }
      catch (Exception ex2)
      {
        Log.Debug(source, ex2);
        return (DTE) null;
      }
    }
  }

  public static void ReleaseInstance(DTE instance)
  {
    try
    {
      if (instance == null)
        return;
      Marshal.ReleaseComObject((object) instance);
    }
    catch (Exception ex)
    {
      Log.Debug(Log.CreateSource("VSHelper"), ex);
    }
  }

  private static string smethod_0(DTE dte_0)
  {
    string str1;
    switch (((_DTE) dte_0).Version)
    {
      case "12.0":
        str1 = "2013";
        break;
      case "11.0":
        str1 = "2012";
        break;
      case "10.0":
        str1 = "2010";
        break;
      case "14.0":
        str1 = "2015";
        break;
      default:
        str1 = ((_DTE) dte_0).Version;
        break;
    }
    string str2 = Path.GetFileName(((_Solution) ((_DTE) dte_0).Solution).FullName);
    if (string.IsNullOrEmpty(str2))
      str2 = "No Solution Open";
    return $"Open in VS {str1} ({str2})";
  }

  public static void OpenHyperlinkInVs(string path, int line)
  {
    IEnumerable<DTE> dtes = VisualStudioHelper.GetRunningInstance();
    DTE dte_0_1;
    if (dtes.Any<DTE>())
    {
      if (dtes.Count<DTE>() > 1)
      {
        ContextMenu contextMenu = new ContextMenu();
        foreach (DTE dte_0_2 in dtes)
        {
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) VisualStudioHelper.smethod_0(dte_0_2);
          newItem.Click += (RoutedEventHandler) ((sender, e) => VisualStudioHelper.smethod_1(path, line, dtes.First<DTE>((Func<DTE, bool>) (dte_0 => VisualStudioHelper.smethod_0(dte_0) == (string) ((HeaderedItemsControl) sender).Header))));
          contextMenu.Items.Add((object) newItem);
        }
        contextMenu.IsOpen = true;
        return;
      }
      dte_0_1 = dtes.First<DTE>();
    }
    else
      dte_0_1 = VisualStudioHelper.GetNewInstance();
    VisualStudioHelper.smethod_1(path, line, dte_0_1);
  }

  private static void smethod_1(string string_0, int int_0, DTE dte_0)
  {
    int num = 10;
    while (num-- > 0)
    {
      if (dte_0 != null)
      {
        try
        {
          ((_DTE) dte_0).MainWindow.Visible = true;
          ((_DTE) dte_0).UserControl = true;
          ((_DTE) dte_0).ItemOperations.OpenFile(string_0, "{00000000-0000-0000-0000-000000000000}");
          (((_DTE) dte_0).ActiveDocument.Selection as TextSelection).GotoLine(int_0, true);
          return;
        }
        catch (Exception ex)
        {
          Thread.Sleep(400);
        }
      }
      else
        break;
    }
    try
    {
      Process.Start(string_0);
    }
    catch
    {
    }
  }
}
