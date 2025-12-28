// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.App
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using OpenTap.Cli;
using OpenTap.Diagnostic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class App : Application
{
  public static string CommandLineFileNameArgument;
  private static readonly Class56 class56_0 = new Class56();
  internal static Task task_0;
  public static EventTraceListener StartTraceListener = new EventTraceListener();
  private static List<Event> list_0 = new List<Event>();
  private bool bool_1;

  private static OpenTap.TraceSource TraceSource_0 { get; set; }

  internal static Class59 CommandLineOptions { get; private set; }

  [Obsolete("Hub mode is no longer supported")]
  public static bool HubMode { get; }

  static App()
  {
    OpenTap.Log.AddListener((ILogListener) App.StartTraceListener);
    App.StartTraceListener.MessageLogged += new EventTraceListener.LogMessageDelegate(App.smethod_0);
    Class54.smethod_0();
    DebuggerAttacher.TryAttach();
  }

  private void App_Startup(object sender, StartupEventArgs e) => this.method_0(e);

  private void method_0(StartupEventArgs startupEventArgs_0)
  {
    App.TraceSource_0 = OpenTap.Log.CreateSource("Main");
    if (((IEnumerable<string>) startupEventArgs_0.Args).Contains<string>("--launch"))
    {
      CliActionExecutor.Execute(((IEnumerable<string>) new string[1]
      {
        "launch"
      }).Concat<string>(((IEnumerable<string>) startupEventArgs_0.Args).Where<string>((Func<string, bool>) (string_0 => string_0 != "--launch"))).ToArray<string>());
      this.Shutdown();
    }
    else
    {
      new SplashScreen(typeof (App).Assembly, "splash.png").Show(true);
      this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
      Class15.smethod_17();
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
      CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
      App.class56_0.class59_0.method_7("help", 'h', false);
      App.class56_0.class59_0.method_7("open", 'o');
      App.class56_0.class59_0.method_7("add", 'a');
      App.class56_0.class59_0.method_7("search", 's');
      App.class56_0.class59_0.method_7("enable-callbacks", bool_0: false);
      App.class56_0.class59_0.method_7("external", 'e');
      Class58 class58_1 = App.class56_0.class59_0.method_7("view-preset");
      Class58 class58_2 = App.class56_0.class59_0.method_7("focus-mode", bool_0: false);
      App.SetCommandLineArgs(startupEventArgs_0.Args);
      SessionLogs.Initialize();
      string path = App.getCommandLineArg("search");
      if (path != null)
      {
        try
        {
          path = Path.GetFullPath(path);
          if (Directory.Exists(path))
          {
            App.TraceSource_0.Debug("Also searching in '{0}'.", (object) path);
            PluginManager.DirectoriesToSearch.Add(path);
            PluginManager.SearchAsync();
          }
          else
            App.TraceSource_0.Error("Could not search '{0}'. Directory not found.", (object) path);
        }
        catch (ArgumentException ex)
        {
          App.TraceSource_0.Error("Unable to add path '{0}' to search dir.", (object) path);
          App.TraceSource_0.Debug((Exception) ex);
        }
      }
      App.task_0 = (Task) Task.Factory.StartNew<IEnumerable<ITypeData>>((Func<IEnumerable<ITypeData>>) (() => TypeData.GetDerivedTypes<ITestStep>()));
      EditorSettings.Load();
      Class58 class58_3;
      if (App.CommandLineOptions.TryGetValue(class58_1.method_2(), out class58_3))
      {
        try
        {
          ViewPreset.SelectPreset(class58_3.method_6());
        }
        catch (Exception ex)
        {
          App.TraceSource_0.Error("Unable to load preset '{0}': {1}", (object) class58_3.method_6(), (object) ex.Message);
          App.TraceSource_0.Debug(ex);
        }
      }
      if (App.CommandLineOptions.TryGetValue(class58_2.method_2(), out Class58 _))
        ComponentSettings<EditorSettings>.Current.FocusMode = true;
      if (!Class61.smethod_0())
        return;
      WpfErrorTraceListener listener = new WpfErrorTraceListener();
      PresentationTraceSources.Refresh();
      PresentationTraceSources.DataBindingSource.Listeners.Add((System.Diagnostics.TraceListener) listener);
      PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
    }
  }

  public static IEnumerable<Event> DetachStartTraceListener()
  {
    if (App.list_0 == null)
      throw new InvalidOperationException("Messages has already been handed over");
    App.StartTraceListener.MessageLogged -= new EventTraceListener.LogMessageDelegate(App.smethod_0);
    Event[] array = App.list_0.ToArray();
    App.list_0 = (List<Event>) null;
    return (IEnumerable<Event>) array;
  }

  private static void smethod_0(IEnumerable<Event> ienumerable_0)
  {
    App.list_0.AddRange(ienumerable_0);
  }

  public static void SetCommandLineArgs(params string[] args)
  {
    App.CommandLineOptions = App.class56_0.method_1(args);
    App.CommandLineFileNameArgument = ((IEnumerable<string>) App.CommandLineOptions.method_0()).LastOrDefault<string>();
    Class58 class58;
    if (!App.CommandLineOptions.TryGetValue("open", out class58))
      return;
    for (int index = 0; index < class58.method_7().Count; ++index)
      class58.method_7()[index] = Path.GetFullPath(class58.method_7()[index]);
  }

  public static string getCommandLineArg(string argName)
  {
    return App.CommandLineOptions != null && App.CommandLineOptions.method_8(argName) ? App.CommandLineOptions[argName].method_6() : (string) null;
  }

  public static void writeCommandLineOptions(OpenTap.TraceSource traceSource_1)
  {
    if (App.CommandLineOptions == null)
      return;
    string str1 = "";
    if (App.CommandLineOptions.Count > 0)
    {
      StringBuilder stringBuilder = new StringBuilder("Application started with option(s): ");
      foreach (Class58 class58 in App.CommandLineOptions.Values)
      {
        stringBuilder.AppendFormat("{0}{1}", (object) str1, (object) class58.method_2());
        str1 = ", ";
      }
      stringBuilder.Append(".");
      traceSource_1.Debug(stringBuilder.ToString());
    }
    if (App.CommandLineOptions.method_2().Count > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Unknown option(s): ");
      string str2 = "";
      foreach (string str3 in App.CommandLineOptions.method_2())
      {
        stringBuilder.AppendFormat("{0}{1}", (object) str2, (object) str3);
        str2 = ", ";
      }
      stringBuilder.Append(".");
      traceSource_1.Debug(stringBuilder.ToString());
    }
    if (App.CommandLineOptions.method_4().Count <= 0)
      return;
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append("Missing Argument(s): ");
    string str4 = "";
    foreach (Class58 class58 in App.CommandLineOptions.method_4())
    {
      stringBuilder1.AppendFormat("{0}{1}:", (object) str4, (object) class58.method_2());
      foreach (string str5 in class58.method_7())
      {
        stringBuilder1.AppendFormat("{0}{1}", (object) str4, (object) str5);
        str4 = ", ";
      }
    }
    stringBuilder1.Append(".");
    traceSource_1.Debug(stringBuilder1.ToString());
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_1)
      return;
    this.bool_1 = true;
    this.Startup += new StartupEventHandler(this.App_Startup);
    Application.LoadComponent((object) this, new Uri("/Editor;component/app.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [STAThread]
  [DebuggerNonUserCode]
  public static void Main()
  {
    App app = new App();
    app.InitializeComponent();
    app.Run();
  }
}
