// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.EditorSettings
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using Keysight.OpenTap.Wpf.Themes;
using OpenTap;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Editor", "Editor Settings", null, -10000.0, false, null)]
public class EditorSettings : ComponentSettings<EditorSettings>
{
  private EditorSettings.LogVisibilities logVisibilities_0;
  private bool bool_2;
  private bool bool_3 = true;
  private bool bool_4 = true;
  private bool bool_5 = true;
  private bool bool_6 = true;
  private int int_1 = 10;
  private int int_2 = 20000;
  private bool bool_10;
  private static FileSystemWatcher fileSystemWatcher_0;
  private static readonly TraceSource traceSource_0 = Log.CreateSource("GUI");

  [Display("Show Welcome Screen at Startup", "Show a welcome screen with help to get you started using Editor", "General", 4.0, false, null)]
  public bool ShowWelcomeScreen { get; set; }

  [Display("Check for Updates at Startup", "Checks for updates at startup, if available a message will be provided\n in the log panel and a warning icon next to the Editor version will appear.", "General", 1.0, false, null)]
  public bool CheckForUpdates { get; set; }

  [Browsable(false)]
  [Display("Log Content", "Specify which messages are shown in the log.", "General", 1.0, false, null)]
  public EditorSettings.LogVisibilities LogVisibility
  {
    get => this.logVisibilities_0;
    set => this.logVisibilities_0 = value;
  }

  [XmlIgnore]
  [ViewPreset.Member]
  public bool ViewDebug
  {
    get => this.LogVisibility.HasFlag((Enum) EditorSettings.LogVisibilities.ViewDebug);
    set
    {
      this.LogVisibility = this.LogVisibility.SetFlag<EditorSettings.LogVisibilities>(EditorSettings.LogVisibilities.ViewDebug, value);
    }
  }

  [ViewPreset.Member]
  [XmlIgnore]
  public bool ViewMessages
  {
    get => this.LogVisibility.HasFlag((Enum) EditorSettings.LogVisibilities.ViewMessages);
    set
    {
      this.LogVisibility = this.LogVisibility.SetFlag<EditorSettings.LogVisibilities>(EditorSettings.LogVisibilities.ViewMessages, value);
    }
  }

  [ViewPreset.Member]
  [XmlIgnore]
  public bool ViewWarnings
  {
    get => this.LogVisibility.HasFlag((Enum) EditorSettings.LogVisibilities.ViewWarnings);
    set
    {
      this.LogVisibility = this.LogVisibility.SetFlag<EditorSettings.LogVisibilities>(EditorSettings.LogVisibilities.ViewWarnings, value);
    }
  }

  [ViewPreset.Member]
  [XmlIgnore]
  public bool ViewErrors
  {
    get => this.LogVisibility.HasFlag((Enum) EditorSettings.LogVisibilities.ViewErrors);
    set
    {
      this.LogVisibility = this.LogVisibility.SetFlag<EditorSettings.LogVisibilities>(EditorSettings.LogVisibilities.ViewErrors, value);
    }
  }

  [Display("Color Theme", "The application color theme.", "General", 1.0, false, null)]
  [ViewPreset.Member]
  public TapSkins.Theme ColorTheme
  {
    get => TapSkins.SelectedTheme;
    set => TapSkins.SelectedTheme = value;
  }

  public bool ResultsViewerInstalled
  {
    get
    {
      return PluginManager.GetSearcher().Assemblies.Any<AssemblyData>((Func<AssemblyData, bool>) (assemblyData_0 => assemblyData_0.Name.Contains("ResultsViewer")));
    }
  }

  [Display("Show Results Viewer", "The Results Viewer opens when the test plan completes.", "General", 2.0, false, null)]
  [ViewPreset.Member]
  [EnabledIf("ResultsViewerInstalled", new object[] {})]
  public bool ResultPopup
  {
    get => this.bool_2;
    set
    {
      if (this.bool_2 == value)
        return;
      this.bool_2 = value;
      ((ValidatingObject) this).OnPropertyChanged(nameof (ResultPopup));
    }
  }

  [EnabledIf("ShowGuiLog", new object[] {true})]
  [ViewPreset.Member]
  [Display("Clear On Run", "When the test plan starts, clear the log panel.\r\nThe cleared messages will still be in the log file.", "Log Panel", 3.0, false, null)]
  public bool ClearLog
  {
    get => this.bool_3;
    set => this.bool_3 = value;
  }

  [Display("Show \"Really Quit?\" Dialog", "When exiting Editor, show the 'Really Quit?' dialog.", "General", 1.0, false, null)]
  public bool ShowQuitConfirmation
  {
    get => this.bool_4;
    set
    {
      if (this.ShowQuitConfirmation == value)
        return;
      this.bool_4 = value;
      ((ValidatingObject) this).OnPropertyChanged(nameof (ShowQuitConfirmation));
    }
  }

  [Browsable(false)]
  [DefaultValue(30)]
  public int SupportSubscriptionCheckDays { get; set; } = 30;

  [Display("Show \"Save Before Exit?\" Dialog", "When exiting Editor, show the 'Save Before Exit?' dialog.", "General", 1.0, false, null)]
  public bool ShowSaveBeforeExit
  {
    get => this.bool_5;
    set => this.bool_5 = value;
  }

  [Display("Enable", "Show log messages in the log panel.\r\nDisabling this can improve performance.", "Log Panel", 2.9, false, null)]
  [ViewPreset.Member]
  [Browsable(false)]
  public bool ShowGuiLog
  {
    get => this.bool_6;
    set => this.bool_6 = true;
  }

  [Display("Ensure Unique Step Name", " This ensures that steps added to the test plan will get unique names.", "General", 4.0, false, null)]
  public bool EnsureUniqueStepName { get; set; } = true;

  [Display("Show \"Package Dependency Warning\" Dialog", "At startup check for package dependency issues.", "General", 1.0, false, null)]
  public bool ShowBrokenPackagesWindow { get; set; }

  [Display("Duration Estimation Avg. Count", "Number of recent test plan runs used for estimating test duration.", "General", 1.0, false, null)]
  public int EstimationWindowLength
  {
    get => this.int_1;
    set => this.int_1 = value;
  }

  [Display("Message Limit", "The maximum number of messages in the log panel.\nIf exceeded, messages will be removed from the beginning of the log panel.\nDefault: 20000 messages.", "Log Panel", 3.0, false, null)]
  [EnabledIf("ShowGuiLog", new object[] {true})]
  public int LogPanelBufferSize
  {
    get => this.int_2;
    set
    {
      if (value > 134217727 /*0x07FFFFFF*/)
        throw new Exception("Message Limit is too large.");
      if (this.int_2 == value)
        return;
      this.int_2 = value;
      ((ValidatingObject) this).OnPropertyChanged(nameof (LogPanelBufferSize));
    }
  }

  [Obsolete("This setting is no longer used.")]
  [Display("Show Source Code Links", "Show stack trace links in log panel.", "Debug", 4.0, false, null)]
  [Browsable(false)]
  public bool ShowCodeLinksInLog { set; get; }

  [Display("Show Property Name/Type in Tooltips", "Show the name and type of properties in tooltips.", "Debug", 4.0, false, null)]
  public bool ShowPropertyInTooltip
  {
    get => this.bool_10;
    set => this.bool_10 = value;
  }

  [Display("Auto Attach Debugger", "If a debugger is not already attached, try to attach the debugger of an already open Visual Studio when the GUI launches.", "Debug", 4.0, false, null)]
  public bool AutoAttachDebugger { get; set; }

  [Browsable(false)]
  [ViewPreset.Member]
  [Display("Focus mode", "Focus mode. Optimizes the user interface for currently selected tasks.", null, -10000.0, false, null)]
  public bool FocusMode { get; set; }

  [Display("Focus Mode Dont Ask", "Warn when enabling focus mode?", null, -10000.0, false, null)]
  [Browsable(false)]
  public bool FocusModeDontAskMeAgain { get; set; }

  public EditorSettings()
  {
    this.ShowBrokenPackagesWindow = true;
    this.ShowPropertyInTooltip = false;
    this.ShowWelcomeScreen = true;
    this.LogVisibility = EditorSettings.LogVisibilities.ViewErrors | EditorSettings.LogVisibilities.ViewWarnings | EditorSettings.LogVisibilities.ViewMessages;
    // ISSUE: method pointer
    ((ValidatingObject) this).Rules.Add(new IsValidDelegateDefinition((object) this, __methodptr(method_0)), "Message Limit must be a positive number.", nameof (LogPanelBufferSize));
    // ISSUE: method pointer
    ((ValidatingObject) this).Rules.Add(new IsValidDelegateDefinition((object) this, __methodptr(method_1)), "Duration estimation average count must be a positive number and greater than 0", nameof (EstimationWindowLength));
  }

  static EditorSettings()
  {
    try
    {
      ComponentSettings.EnsureSettingsDirectoryExists("", false);
    }
    catch
    {
      return;
    }
    EditorSettings.fileSystemWatcher_0 = new FileSystemWatcher(ComponentSettings.SettingsDirectoryRoot, "*.xml");
    EditorSettings.fileSystemWatcher_0.Changed += new FileSystemEventHandler(EditorSettings.smethod_0);
    EditorSettings.fileSystemWatcher_0.NotifyFilter = NotifyFilters.LastWrite;
    EditorSettings.fileSystemWatcher_0.EnableRaisingEvents = true;
  }

  private static void smethod_0(object sender, FileSystemEventArgs e)
  {
    try
    {
      EditorSettings.smethod_1(e);
    }
    catch (TypeLoadException ex)
    {
      EditorSettings.fileSystemWatcher_0.Changed -= new FileSystemEventHandler(EditorSettings.smethod_0);
      EditorSettings.fileSystemWatcher_0.Dispose();
    }
  }

  private static void smethod_1(FileSystemEventArgs fileSystemEventArgs_0)
  {
    string saveFilePath = ComponentSettings.GetSaveFilePath(typeof (EditorSettings));
    if (string.Compare(Path.GetFileName(fileSystemEventArgs_0.FullPath), Path.GetFileName(saveFilePath), true) != 0)
      return;
    GuiHelpers.GuiInvoke((Action) (() =>
    {
      try
      {
        ComponentSettings<EditorSettings>.Current.ColorTheme = ((EditorSettings) new TapSerializer().DeserializeFromFile(fileSystemEventArgs_0.FullPath, (ITypeData) null, true)).ColorTheme;
      }
      catch
      {
      }
    }));
  }

  public static void Load()
  {
    try
    {
      ComponentSettings<EditorSettings>.Current.ColorTheme.ToString();
    }
    catch (Exception ex)
    {
      Log.Error(EditorSettings.traceSource_0, "Unable to load themes {0}", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(EditorSettings.traceSource_0, ex);
    }
    TapSkins.Initialize();
  }

  [Flags]
  public enum LogVisibilities
  {
    ViewErrors = 1,
    ViewWarnings = 2,
    ViewMessages = 4,
    ViewDebug = 8,
  }
}
