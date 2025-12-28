// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.UnhandledExceptionHandler
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class UnhandledExceptionHandler
{
  private static Window window_0;
  [ThreadStatic]
  private static bool bool_0;
  private static readonly TraceSource traceSource_0 = Log.CreateSource("App");

  public static void Load(Window window_1)
  {
    UnhandledExceptionHandler.window_0 = UnhandledExceptionHandler.window_0 == null ? window_1 : throw new Exception("exception handler is already loaded.");
    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler.smethod_1);
    if (Build.IsDebug)
      AppDomain.CurrentDomain.FirstChanceException += new EventHandler<FirstChanceExceptionEventArgs>(UnhandledExceptionHandler.smethod_0);
    Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(UnhandledExceptionHandler.smethod_2);
  }

  private static void smethod_0(object sender, FirstChanceExceptionEventArgs e)
  {
    if (UnhandledExceptionHandler.bool_0)
      return;
    UnhandledExceptionHandler.bool_0 = true;
    try
    {
      Log.Warning(UnhandledExceptionHandler.traceSource_0, "First chance exception: {0}", new object[1]
      {
        (object) e.Exception
      });
      Log.Debug(UnhandledExceptionHandler.traceSource_0, e.Exception);
    }
    finally
    {
      UnhandledExceptionHandler.bool_0 = false;
    }
  }

  public static void Unload(Window window_1)
  {
    UnhandledExceptionHandler.window_0 = UnhandledExceptionHandler.window_0 == window_1 ? (Window) null : throw new Exception("Window not previously loaded");
    AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(UnhandledExceptionHandler.smethod_1);
    Application.Current.DispatcherUnhandledException -= new DispatcherUnhandledExceptionEventHandler(UnhandledExceptionHandler.smethod_2);
  }

  private static void smethod_1(object sender, UnhandledExceptionEventArgs e)
  {
    if (!e.IsTerminating)
    {
      Log.Error(UnhandledExceptionHandler.traceSource_0, "Caught unexpected error.", Array.Empty<object>());
      if (!(e.ExceptionObject is Exception exceptionObject))
        return;
      Log.Debug(UnhandledExceptionHandler.traceSource_0, exceptionObject);
    }
    else
    {
      Log.Error(UnhandledExceptionHandler.traceSource_0, "Caught unexpected error. Shutting down..", Array.Empty<object>());
      if (e.ExceptionObject is Exception exceptionObject)
        Log.Debug(UnhandledExceptionHandler.traceSource_0, exceptionObject);
      SessionLogs.Flush();
      QuickDialog.ShowMessage("Error", "An unexpected exception occured, see session log for more information.");
    }
  }

  private static void smethod_2(object sender, DispatcherUnhandledExceptionEventArgs e)
  {
    e.Handled = true;
    if (e.Exception != null && e.Exception.InnerException != null && e.Exception.InnerException.Message != null && e.Exception.InnerException.Message.Contains("System.Core, Version=2.0.5.0"))
    {
      Log.Error(UnhandledExceptionHandler.traceSource_0, ".NET framework version compatibility error.", Array.Empty<object>());
      Log.Error(UnhandledExceptionHandler.traceSource_0, "Please try applying update \"Microsoft .NET Framework 4 KB2468871\" from here: http://www.microsoft.com/en-us/download/details.aspx?id=3556", Array.Empty<object>());
    }
    else
    {
      Exception exception1 = e.Exception;
      bool? nullable1;
      int num;
      if (exception1 == null)
      {
        num = 0;
      }
      else
      {
        nullable1 = exception1.Message?.Contains("Hwnd of zero is not valid");
        num = nullable1.GetValueOrDefault() & nullable1.HasValue ? 1 : 0;
      }
      if (num != 0)
        return;
      Exception exception2 = e.Exception;
      bool? nullable2;
      if (exception2 == null)
      {
        nullable1 = new bool?();
        nullable2 = nullable1;
      }
      else
      {
        string stackTrace = exception2.StackTrace;
        if (stackTrace == null)
        {
          nullable1 = new bool?();
          nullable2 = nullable1;
        }
        else
          nullable2 = new bool?(stackTrace.Contains("WindowChromeWorker._ExtendGlassFrame"));
      }
      nullable1 = nullable2;
      if (nullable1.GetValueOrDefault())
        return;
      Log.Error(UnhandledExceptionHandler.traceSource_0, "Caught unhandled GUI error.", Array.Empty<object>());
      Log.Debug(UnhandledExceptionHandler.traceSource_0, e.Exception);
      Window window0 = UnhandledExceptionHandler.window_0;
      if ((window0 != null ? (!window0.IsLoaded ? 1 : 0) : 0) == 0)
        return;
      UnhandledExceptionHandler.window_0.Close();
    }
  }
}
