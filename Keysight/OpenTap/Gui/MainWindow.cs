// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.MainWindow
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using EnvDTE;
using Keysight.Ccl.Wsl.UI;
using Keysight.Ccl.Wsl.UI.Dialogs;
using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Licensing;
using Keysight.OpenTap.Package.Gui;
using Keysight.OpenTap.Wpf;
using Microsoft.Win32;
using Microsoft.Windows.Shell;
using OpenTap;
using OpenTap.Diagnostic;
using OpenTap.Package;
using OpenTap.Plugins;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class MainWindow : 
  WslMainWindow,
  ITapDockContext,
  INotifyPropertyChanged,
  ITapDockContext2,
  ICommandHandler,
  IComponentConnector,
  IStyleConnector
{
  private static readonly OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Main");
  private TestPlanGridListener testPlanGridListener_0 = new TestPlanGridListener();
  public static readonly DependencyProperty PlanProperty = DependencyProperty.Register(nameof (Plan), typeof (TestPlan), typeof (MainWindow), new PropertyMetadata(new PropertyChangedCallback(MainWindow.smethod_1)));
  public static readonly DependencyProperty RepeatCountProperty = DependencyProperty.Register(nameof (RepeatCount), typeof (int), typeof (MainWindow), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty IsRepeatCountVisibleProperty = DependencyProperty.Register(nameof (IsRepeatCountVisible), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty TestPlanLockedProperty = DependencyProperty.Register(nameof (TestPlanLocked), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty SettingsLockedProperty = DependencyProperty.Register(nameof (SettingsLocked), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty TestPlanOpenedProperty = DependencyProperty.Register(nameof (TestPlanOpened), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty TestPlanIsStoppingProperty = DependencyProperty.Register(nameof (TestPlanIsStopping), typeof (bool), typeof (MainWindow), new PropertyMetadata((object) false));
  public static readonly DependencyProperty BreakPointHitProperty = DependencyProperty.Register(nameof (BreakPointHit), typeof (bool), typeof (MainWindow));
  private bool bool_0;
  public static readonly DependencyProperty SingleSteppingStartedProperty = DependencyProperty.Register(nameof (SingleSteppingStarted), typeof (bool), typeof (MainWindow), new PropertyMetadata((object) false, new PropertyChangedCallback(MainWindow.smethod_0)));
  public static readonly DependencyProperty AnyExternalTestPlanParametersProperty = DependencyProperty.Register(nameof (AnyExternalTestPlanParameters), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty TestPlanProgressMessageProperty = DependencyProperty.Register(nameof (TestPlanProgressMessage), typeof (string), typeof (MainWindow));
  public static readonly DependencyProperty GotLicenseProperty = DependencyProperty.Register(nameof (GotLicense), typeof (bool), typeof (MainWindow), new PropertyMetadata((object) false));
  private static List<MainWindow.Class83> list_0 = new List<MainWindow.Class83>();
  public static readonly DependencyProperty PlanRunningProperty = DependencyProperty.Register(nameof (PlanRunning), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty LogEnabledProperty = DependencyProperty.Register(nameof (LogEnabled), typeof (bool), typeof (MainWindow));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (double), typeof (MainWindow));
  public static readonly DependencyProperty StartupCompletedProperty = DependencyProperty.Register(nameof (StartupCompleted), typeof (bool), typeof (MainWindow));
  public static RoutedUICommand RunTestPlan = new RoutedUICommand("Run Test Plan", "Run", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F5)
  });
  public static RoutedUICommand PauseTestPlan = new RoutedUICommand("Pause Test Plan", "Pause", typeof (MainWindow));
  public static RoutedUICommand RunStep = new RoutedUICommand("Run Selected Test Steps", "Run Test Step", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F10)
  });
  public static RoutedUICommand StopTestPlan = new RoutedUICommand("Stop Test Plan Execution", "Stop", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F5, ModifierKeys.Shift)
  });
  public static RoutedUICommand InitTestPlan = new RoutedUICommand("Open/Close Test Plan Resources", "Init", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F6, ModifierKeys.Shift)
  });
  public static RoutedUICommand AddStep = new RoutedUICommand("Add Test Step", nameof (AddStep), typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.T, ModifierKeys.Control)
  });
  public static RoutedUICommand RemoveStep = new RoutedUICommand("Remove Selected Test Steps", nameof (RemoveStep), typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Delete)
  });
  public static RoutedUICommand RenameStep = new RoutedUICommand("Rename Selected Test Step", nameof (RenameStep), typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F2)
  });
  public static RoutedUICommand OpenResults = new RoutedUICommand("Open Results Viewer", "View Results", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.R, ModifierKeys.Control)
  });
  public static RoutedUICommand Undo = new RoutedUICommand(nameof (Undo), nameof (Undo), typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Z, ModifierKeys.Control)
  });
  public static RoutedUICommand Redo = new RoutedUICommand(nameof (Redo), nameof (Redo), typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Y, ModifierKeys.Control)
  });
  public static RoutedUICommand NewPlanCommand = new RoutedUICommand("Create New Test Plan", "New Test Plan", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.N, ModifierKeys.Control)
  });
  public static RoutedUICommand Import = new RoutedUICommand(nameof (Import), nameof (Import), typeof (MainWindow));
  public static RoutedUICommand Export = new RoutedUICommand(nameof (Export), nameof (Export), typeof (MainWindow));
  public static RoutedUICommand StepTestPlan = new RoutedUICommand("Single Steps through Test Plan", "Single Step", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F11)
  });
  public static RoutedUICommand AddPlan = new RoutedUICommand("Add Test Steps from Test Plan", "InsertPlan", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Shift)
  });
  public static RoutedUICommand ToggleFocusModeCommand = new RoutedUICommand(nameof (ToggleFocusModeCommand), "FocusMode", typeof (MainWindow), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Return, ModifierKeys.Alt | ModifierKeys.Shift)
  });
  public static RoutedUICommand ShowLicenseManagerCommand = new RoutedUICommand("Show License Manager", "Show License Manager", typeof (MainWindow));
  public static RoutedUICommand CheckForUpdatesCommand = new RoutedUICommand("Check For Updates", "Check For Updates", typeof (MainWindow));
  private EventTraceListener eventTraceListener_0 = new EventTraceListener();
  private byte[] byte_0;
  private readonly List<LogMessage> list_1 = new List<LogMessage>();
  private static readonly InvertBooleanConverter invertBooleanConverter_0 = new InvertBooleanConverter();
  private List<Action> list_2 = new List<Action>();
  private HashSet<string> hashSet_0 = new HashSet<string>();
  private MainWindowContext mainWindowContext_0;
  private const int int_0 = 5;
  private FileStream fileStream_0;
  private Task task_0 = (Task) Task.FromResult<int>(0);
  private readonly string string_0 = $"~last_session.{Process.GetCurrentProcess().Id}.RecoveryTapPlan";
  private const string string_1 = ".TapPlan";
  private const string string_2 = "Test Plan File|*.TapPlan";
  private string string_3;
  private MainWindow.Class84 class84_0 = new MainWindow.Class84();
  private TapThread tapThread_0;
  private int int_1;
  private int int_2;
  private bool bool_1;
  private Point? nullable_0;
  private MainWindow.Class85 class85_0;
  private MainWindow.Class85 class85_1;
  private readonly Class198<MainWindow.Class85> class198_0 = new Class198<MainWindow.Class85>();
  private TapThread tapThread_1;
  private AutoResetEvent autoResetEvent_0 = new AutoResetEvent(false);
  private AutoResetEvent autoResetEvent_1 = new AutoResetEvent(false);
  public EventHandler PlanChanged;
  private bool bool_2;
  public static readonly DependencyProperty IsPlanDirtyProperty = DependencyProperty.Register(nameof (IsPlanDirty), typeof (bool), typeof (MainWindow));
  private ManualResetEvent manualResetEvent_0;
  private bool bool_3;
  private readonly ManualResetEventSlim manualResetEventSlim_0 = new ManualResetEventSlim(true);
  private string string_4 = "hh.exe";
  private string string_5 = "ms-its:";
  private string string_6 = "Error loading help. Please ensure help files are within the same directory as OpenTAP.";
  private string string_7;
  private string string_8;
  private readonly List<IResultListener> list_3 = new List<IResultListener>();
  private DefaultUserInputInterface defaultUserInputInterface_0;
  private const string string_9 = "EditorHelp.chm::/Editor Overview/Focus Mode.html";
  internal MainWindow mainWindow_0;
  internal Grid grid_0;
  internal RowDefinition rowDefinition_0;
  internal Grid grid_1;
  internal Menu menu_0;
  internal MenuItem menuItem_0;
  internal Separator separator_0;
  internal MenuItem menuItem_1;
  internal MenuItem menuItem_2;
  internal Separator separator_1;
  internal Separator separator_2;
  internal MenuItem menuItem_3;
  internal MenuItem menuItem_4;
  internal MenuItem menuItem_5;
  internal PresetsMenuItem presetsMenuItem_0;
  internal MenuItem menuItem_6;
  internal MenuItem menuItem_7;
  internal MenuItem menuItem_8;
  internal MenuItem menuItem_9;
  internal MenuItem menuItem_10;
  internal StackPanel stackPanel_0;
  internal TextBox textBox_0;
  internal Grid grid_2;
  internal TestPlanGrid testPlanGrid_0;
  internal TextBlock textBlock_0;
  internal TextBlock textBlock_1;
  internal WelcomeScreen welcomeScreen_0;
  internal Button button_0;
  internal ControlDropDown controlDropDown_0;
  internal ToggleButton toggleButton_0;
  internal Control control_0;
  internal Decorator decorator_0;
  internal Grid grid_3;
  internal Grid grid_4;
  internal PropGrid propGrid_0;
  internal TextBlock textBlock_2;
  internal Grid grid_5;
  internal LogPanel logPanel_0;
  internal RunCompletedOverlay runCompletedOverlay_0;
  internal TapDock tapDock_0;
  internal InstrumentStatus instrumentStatus_0;
  private bool bool_4;

  public TestPlanGridListener TestPlanListener => this.testPlanGridListener_0;

  public TestPlan Plan
  {
    get => (TestPlan) this.GetValue(MainWindow.PlanProperty);
    set => this.SetValue(MainWindow.PlanProperty, (object) value);
  }

  public int RepeatCount
  {
    get => (int) this.GetValue(MainWindow.RepeatCountProperty);
    set => this.SetValue(MainWindow.RepeatCountProperty, (object) value);
  }

  public uint MaxRepeatCount
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatMaxCount;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatMaxCount = value;
  }

  public bool RepeatStopAbort
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopAbort;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopAbort = value;
  }

  public bool RepeatStopError
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopError;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopError = value;
  }

  public bool RepeatStopFail
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopFail;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopFail = value;
  }

  public bool RepeatStopPass
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopPass;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopPass = value;
  }

  public bool RepeatStopPending
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopPending;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopPending = value;
  }

  public bool RepeatStopCount
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopCount;
    set => ComponentSettings<GuiControlsSettings>.Current.MainWindowRepeatStopCount = value;
  }

  public bool IsRepeatEnabled
  {
    get => ComponentSettings<GuiControlsSettings>.Current.MainWindowIsRepeatEnabled;
    set
    {
      ComponentSettings<GuiControlsSettings>.Current.MainWindowIsRepeatEnabled = value;
      this.method_0(nameof (IsRepeatEnabled));
    }
  }

  private void method_0(string string_10)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_10));
  }

  public bool IsRepeatCountVisible
  {
    get => (bool) this.GetValue(MainWindow.IsRepeatCountVisibleProperty);
    set => this.SetValue(MainWindow.IsRepeatCountVisibleProperty, (object) value);
  }

  public bool TestPlanLocked
  {
    get => (bool) this.GetValue(MainWindow.TestPlanLockedProperty);
    set => this.SetValue(MainWindow.TestPlanLockedProperty, (object) value);
  }

  public bool SettingsLocked
  {
    get => (bool) this.GetValue(MainWindow.SettingsLockedProperty);
    set => this.SetValue(MainWindow.SettingsLockedProperty, (object) value);
  }

  public bool TestPlanOpened
  {
    get => (bool) this.GetValue(MainWindow.TestPlanOpenedProperty);
    set => this.SetValue(MainWindow.TestPlanOpenedProperty, (object) value);
  }

  public bool TestPlanIsStopping
  {
    get => (bool) this.GetValue(MainWindow.TestPlanIsStoppingProperty);
    set => this.SetValue(MainWindow.TestPlanIsStoppingProperty, (object) value);
  }

  public bool BreakPointHit
  {
    get => (bool) this.GetValue(MainWindow.BreakPointHitProperty);
    set => this.SetValue(MainWindow.BreakPointHitProperty, (object) value);
  }

  public bool SingleSteppingStarted
  {
    get => (bool) this.GetValue(MainWindow.SingleSteppingStartedProperty);
    set => this.SetValue(MainWindow.SingleSteppingStartedProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as MainWindow).bool_0 = (bool) dependencyPropertyChangedEventArgs_0.NewValue;
    CommandManager.InvalidateRequerySuggested();
  }

  public bool AnyExternalTestPlanParameters
  {
    get => (bool) this.GetValue(MainWindow.AnyExternalTestPlanParametersProperty);
    set => this.SetValue(MainWindow.AnyExternalTestPlanParametersProperty, (object) value);
  }

  public string TestPlanProgressMessage
  {
    get => (string) this.GetValue(MainWindow.TestPlanProgressMessageProperty);
    set => this.SetValue(MainWindow.TestPlanProgressMessageProperty, (object) value);
  }

  public bool GotLicense
  {
    get => (bool) this.GetValue(MainWindow.GotLicenseProperty);
    set => this.SetValue(MainWindow.GotLicenseProperty, (object) value);
  }

  internal MainWindow.Class83 method_1(string string_10)
  {
    MainWindow.Class83 class83 = new MainWindow.Class83()
    {
      String_0 = string_10
    };
    if (MainWindow.list_0.Count == 0)
      this.TestPlanProgressMessage = class83.String_0;
    MainWindow.list_0.Add(class83);
    return class83;
  }

  internal void method_2(MainWindow.Class83 class83_0)
  {
    int index = MainWindow.list_0.IndexOf(class83_0);
    MainWindow.list_0.RemoveAt(index);
    if (index != 0)
      return;
    this.TestPlanProgressMessage = MainWindow.list_0.FirstOrDefault<MainWindow.Class83>()?.String_0;
  }

  public bool PlanRunning
  {
    get => (bool) this.GetValue(MainWindow.PlanRunningProperty);
    set => this.SetValue(MainWindow.PlanRunningProperty, (object) value);
  }

  public bool LogEnabled
  {
    get => (bool) this.GetValue(MainWindow.LogEnabledProperty);
    set => this.SetValue(MainWindow.LogEnabledProperty, (object) value);
  }

  public double Duration
  {
    get => (double) this.GetValue(MainWindow.DurationProperty);
    set => this.SetValue(MainWindow.DurationProperty, (object) value);
  }

  public bool StartupCompleted
  {
    get => (bool) this.GetValue(MainWindow.StartupCompletedProperty);
    set => this.SetValue(MainWindow.StartupCompletedProperty, (object) value);
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is MainWindow mainWindow))
      return;
    mainWindow.TestPlanOpened = dependencyPropertyChangedEventArgs_0.NewValue is TestPlan newValue && newValue.IsOpen;
    mainWindow.TestPlanPath = newValue?.Path;
    mainWindow.method_0("Plan");
    mainWindow.TestPlanPath = newValue?.Path;
  }

  public static void AttemptDebugAttach()
  {
    if (!Debugger.IsAttached)
    {
      Stopwatch timer = Stopwatch.StartNew();
      MainWindow.traceSource_0.Debug("Attaching debugger.");
      int num = 4;
      DTE dte = (DTE) null;
      while (num-- > 0)
      {
        try
        {
          dte = VisualStudioHelper.GetRunningInstance().FirstOrDefault<DTE>();
          if (dte == null)
          {
            MainWindow.traceSource_0.Debug("Could not attach Visual Studio debugger. No instances found or missing user privileges.");
            break;
          }
          foreach (Process localProcess in ((_DTE) dte).Debugger.LocalProcesses)
          {
            if (localProcess.ProcessID == Process.GetCurrentProcess().Id)
            {
              localProcess.Attach();
              MainWindow.traceSource_0.Debug(timer, "Debugger attached.");
              return;
            }
          }
        }
        catch (COMException ex)
        {
          if (ex.ErrorCode == -2147417846)
          {
            MainWindow.traceSource_0.Debug("Visual Studio was busy while trying to attach. Retrying shortly.");
            Thread.Sleep(500);
          }
          else
          {
            MainWindow.traceSource_0.Debug((Exception) ex);
            Thread.Sleep(50);
          }
        }
        catch (Exception ex)
        {
          MainWindow.traceSource_0.Debug(ex);
          Thread.Sleep(50);
        }
        finally
        {
          VisualStudioHelper.ReleaseInstance(dte);
        }
      }
    }
    else
      MainWindow.traceSource_0.Debug("Debugger already attached.");
  }

  public TapSettings TapSettings { get; set; }

  public MainWindow()
  {
    this.TapSettings = new TapSettings();
    OpenTap.Log.AddListener((ILogListener) this.eventTraceListener_0);
    OpenTap.Log.Flush();
    this.method_7(App.DetachStartTraceListener());
    this.eventTraceListener_0.MessageLogged += new EventTraceListener.LogMessageDelegate(this.method_7);
    EngineSettings.LoadWorkingDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
    SystemParameters2.Current.ToString();
    if (App.CommandLineFileNameArgument != null)
      MainWindow.traceSource_0.Debug("Command line argument: {0}", (object) App.CommandLineFileNameArgument);
    MainWindow.traceSource_0.Debug("Window initial dimensions: {0:0}px, {1:0}px.", (object) ComponentSettings<GuiControlsSettings>.Current.MainWindowWidth, (object) ComponentSettings<GuiControlsSettings>.Current.MainWindowHeight);
    if (ComponentSettings<GuiControlsSettings>.Current.MainWindowState == TapWindowState.Minimized)
      ComponentSettings<GuiControlsSettings>.Current.MainWindowState = TapWindowState.Normal;
    UnhandledExceptionHandler.Load((Window) this);
    this.Closed += new EventHandler(this.MainWindow_Closed);
    if (ComponentSettings<EditorSettings>.Current.AutoAttachDebugger)
      MainWindow.AttemptDebugAttach();
    if (ComponentSettings<EditorSettings>.Current.ShowBrokenPackagesWindow)
      TapThread.Start((Action) (() =>
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainWindow.Class88 class88 = new MainWindow.Class88();
        // ISSUE: reference to a compiler-generated field
        class88.mainWindow_0 = this;
        // ISSUE: reference to a compiler-generated field
        class88.list_0 = BrokenPackagesWindow.GetBrokenPackages(false, Installation.Current.GetPackages(), (List<PackageDef>) null, TapThread.Current.AbortToken);
        // ISSUE: reference to a compiler-generated field
        if (!class88.list_0.Any<BrokenPackage>())
          return;
        // ISSUE: reference to a compiler-generated method
        GuiHelpers.GuiInvokeAsync((Action) new Action(class88.method_0), priority: DispatcherPriority.ContextIdle);
      }));
    App.task_0 = App.task_0.ContinueWith((Action<Task>) (task_0 => SessionLogs.Rename(ComponentSettings<EngineSettings>.Current.SessionLogPath.Expand())));
    Stopwatch timer1 = Stopwatch.StartNew();
    this.Plan = new TestPlan();
    this.InitializeComponent();
    MainWindow.Class83 class83_0 = this.method_1("Loading plugins...");
    MainWindow.traceSource_0.Debug(timer1, "GUI initialized.");
    timer1.Restart();
    if (ComponentSettings<GuiControlsSettings>.Current.MainWindowWidth <= SystemParameters.WorkArea.Width && ComponentSettings<GuiControlsSettings>.Current.MainWindowHeight <= SystemParameters.WorkArea.Height)
    {
      this.Width = ComponentSettings<GuiControlsSettings>.Current.MainWindowWidth;
      this.Height = ComponentSettings<GuiControlsSettings>.Current.MainWindowHeight;
    }
    else
    {
      this.Width = SystemParameters.WorkArea.Width;
      this.Height = SystemParameters.WorkArea.Height;
    }
    if (ComponentSettings<GuiControlsSettings>.Current.MainWindowTop != 0.0)
    {
      this.Top = ComponentSettings<GuiControlsSettings>.Current.MainWindowTop;
      this.Left = ComponentSettings<GuiControlsSettings>.Current.MainWindowLeft;
      this.WindowStartupLocation = WindowStartupLocation.Manual;
    }
    SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(this.method_4);
    MainWindow.traceSource_0.Debug(timer1, "Docking manager initialized.");
    Assembly executingAssembly = Assembly.GetExecutingAssembly();
    SemanticVersion result;
    this.textBox_0.Text = !SemanticVersion.TryParse(FileVersionInfo.GetVersionInfo(executingAssembly.Location).ProductVersion, out result) ? FileVersionInfo.GetVersionInfo(executingAssembly.Location).ProductVersion : (result.PreRelease == null || !result.PreRelease.StartsWith("alpha") ? result.ToString(4) : result.ToString(5));
    this.Loaded += (RoutedEventHandler) ((sender, e) => this.WindowState = (WindowState) ComponentSettings<GuiControlsSettings>.Current.MainWindowState);
    TapThread.Start((Action) (() =>
    {
      try
      {
        Class50.smethod_0();
        Class79.smethod_0();
        GuiHelpers.GuiInvoke((Action) (() => this.GotLicense = true));
      }
      catch
      {
      }
      try
      {
        TapThread.Start((Action) (() =>
        {
          TapThread.Sleep(1000);
          Keysight.OpenTap.Licensing.LicenseManager.WaitUntilIdle();
          LicenseData[] availableLicenses = Keysight.OpenTap.Licensing.LicenseManager.GetAvailableLicenses();
          List<LicenseData> licenseDataList = new List<LicenseData>();
          LicenseData[] array = ((IEnumerable<LicenseData>) availableLicenses).Where<LicenseData>((Func<LicenseData, bool>) (licenseData_0 => licenseData_0.Type.HasFlag((System.Enum) LicenseType.KAL))).Where<LicenseData>((Func<LicenseData, bool>) (licenseData_0 =>
          {
            DateTime? expirationDate = licenseData_0.ExpirationDate;
            DateTime now = DateTime.Now;
            return expirationDate.HasValue && expirationDate.GetValueOrDefault() > now;
          })).ToArray<LicenseData>();
          if (((IEnumerable<LicenseData>) array).Any<LicenseData>())
          {
            DateTime dateTime_0;
            licenseDataList.Add(GuiHelpers.FindMax<LicenseData, DateTime>((IEnumerable<LicenseData>) array, (Func<LicenseData, DateTime>) (licenseData_0 => !MainWindow.smethod_3(licenseData_0.Version, out dateTime_0) ? DateTime.MinValue : dateTime_0)));
          }
          foreach (LicenseData licenseData in licenseDataList)
          {
            DateTime dateTime_0;
            if (MainWindow.smethod_3(licenseData.Version, out dateTime_0))
            {
              TimeSpan timeSpan = dateTime_0 - DateTime.Now;
              if (timeSpan.TotalDays > 0.0 && timeSpan.TotalDays < (double) ComponentSettings<EditorSettings>.Current.SupportSubscriptionCheckDays)
                MainWindow.traceSource_0.Info($"Support subscription for '{licenseData.Name}' expires on {dateTime_0.ToLongDateString()}. (in {(int) timeSpan.TotalDays} days)");
              else if (timeSpan.TotalDays < 0.0)
              {
                DateTime? expirationDate = licenseData.ExpirationDate;
                DateTime now = DateTime.Now;
                if ((expirationDate.HasValue ? (expirationDate.GetValueOrDefault() > now ? 1 : 0) : 0) != 0)
                  MainWindow.traceSource_0.Info($"Support subscription for '{licenseData.Name}' has expired.");
              }
            }
          }
        }));
      }
      finally
      {
        GuiHelpers.GuiInvoke((Action) (() =>
        {
          if (!Class61.smethod_1())
            return;
          CommunityEditionGui.AcceptedEulaOrExit();
        }), priority: DispatcherPriority.Background);
      }
      if (Class61.smethod_0())
        FtlHelper.MockFtLicenseWarning();
      FtlHelper.Check();
    }));
    if (Class61.smethod_1())
      this.Title += " - Community Edition";
    TapThread.Start((Action) (() =>
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class89 class89 = new MainWindow.Class89();
      Stopwatch timer2 = Stopwatch.StartNew();
      // ISSUE: reference to a compiler-generated field
      class89.list_0 = new Installation(Directory.GetCurrentDirectory()).GetPackages();
      // ISSUE: reference to a compiler-generated field
      class89.list_1 = new List<PackageDef>();
      if (Class61.smethod_1())
      {
        // ISSUE: reference to a compiler-generated method
        class89.method_0(Class200.smethod_2());
      }
      else if (Dns.GetHostEntry(Dns.GetHostName()).HostName.ToLower().EndsWith("keysight.com"))
      {
        string string_0 = Class200.smethod_2(Uri.EscapeDataString(WindowsIdentity.GetCurrent().Name) + "<employee@keysight.com>");
        // ISSUE: reference to a compiler-generated method
        class89.method_0(string_0);
      }
      else if (!ComponentSettings<EditorSettings>.Current.CheckForUpdates)
        return;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      PackageManager.CheckForUpdatesAsync(new PackageManager.PackagesEvent(class89.method_1), (IPackageIdentifier[]) class89.list_0.ToArray(), new CancellationToken()).Wait();
      // ISSUE: reference to a compiler-generated field
      foreach (PackageDef packageDef1 in class89.list_1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainWindow.Class90 class90 = new MainWindow.Class90();
        // ISSUE: reference to a compiler-generated field
        class90.packageDef_0 = packageDef1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PackageDef packageDef2 = class89.list_0.FirstOrDefault<PackageDef>(new Func<PackageDef, bool>(class90.method_0));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (class90.packageDef_0.Version.Major > packageDef2.Version.Major || class90.packageDef_0.Version.Major == packageDef2.Version.Major && class90.packageDef_0.Version.Minor > packageDef2.Version.Minor)
        {
          // ISSUE: reference to a compiler-generated field
          MainWindow.traceSource_0.Info(timer2, "Latest version of '{2}' is {0}, your version is {1}. Please consider updating.", (object) class90.packageDef_0.Version.ToString(2), (object) packageDef2.Version.ToString(2), (object) packageDef2.Name);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (class90.packageDef_0.Name == "Editor" || class90.packageDef_0.Name == "Editor CE")
            GuiHelpers.GuiInvoke((Action) (() => this.textBox_0.Tag = (object) "UpdateReady"));
        }
      }
      MainWindow.traceSource_0.Debug(timer2, "Update check complete.");
    }));
    App.writeCommandLineOptions(MainWindow.traceSource_0);
    MultiBinding binding1 = new MultiBinding()
    {
      Converter = (IMultiValueConverter) new OrConverter()
    };
    binding1.Bindings.Add((BindingBase) new Binding(nameof (PlanRunning))
    {
      Source = (object) this
    });
    binding1.Bindings.Add((BindingBase) new Binding("Plan.Locked")
    {
      Source = (object) this
    });
    BindingOperations.SetBinding((DependencyObject) this, MainWindow.TestPlanLockedProperty, (BindingBase) binding1);
    MultiBinding binding2 = new MultiBinding()
    {
      Converter = (IMultiValueConverter) new OrConverter()
    };
    binding2.Bindings.Add((BindingBase) new Binding(nameof (PlanRunning))
    {
      Source = (object) this
    });
    binding2.Bindings.Add((BindingBase) new Binding(nameof (TestPlanOpened))
    {
      Source = (object) this
    });
    BindingOperations.SetBinding((DependencyObject) this, MainWindow.SettingsLockedProperty, (BindingBase) binding2);
    App.task_0 = App.task_0.ContinueWith((Action<Task>) (task_1 => this.method_10()));
    this.defaultUserInputInterface_0 = new DefaultUserInputInterface((Window) this);
    this.defaultUserInputInterface_0.OnAbortRequested += (EventHandler) ((sender, e) => this.method_40(sender, (RoutedEventArgs) null));
    UserInput.SetInterface((IUserInputInterface) this.defaultUserInputInterface_0);
    this.PlanChanged += (EventHandler) ((sender, e) => this.AnyExternalTestPlanParameters = this.Plan.ExternalParameters.Entries.Count > 0);
    NewStepControl.OpenPluginsWindowRequested += (EventHandler) new EventHandler(this.OpenPluginsWindow);
    if (App.CommandLineOptions != null && App.CommandLineOptions.method_8("enable-callbacks"))
      CallbackChannels.smethod_0(this);
    new Installation(Directory.GetCurrentDirectory()).PackageChangedEvent += (Action) (() =>
    {
      PluginManager.SearchAsync().Wait();
      GuiHelpers.GuiInvoke((Action) new Action(this.OnPluginsChanged));
    });
    TypeData.TypeCacheInvalidated += (EventHandler<TypeDataCacheInvalidatedEventArgs>) ((sender, e) => GuiHelpers.GuiInvoke((Action) new Action(this.OnPluginsChanged)));
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      this.method_88(true);
      this.Activate();
      MainWindow.traceSource_0.Debug(DateTime.Now - Process.GetCurrentProcess().StartTime, "Application startup finished.");
      Application.Current.Resources.MergedDictionaries.Add(this.Resources);
      GuiHelpers.GuiInvokeAsync((Action) new Action(this.method_89), priority: DispatcherPriority.Input);
    }), priority: DispatcherPriority.ContextIdle);
    this.testPlanGridListener_0.Grid = this.testPlanGrid_0;
    this.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
    this.Unloaded += new RoutedEventHandler(this.MainWindow_Unloaded);
    this.method_2(class83_0);
  }

  private void MainWindow_Loaded(object sender, RoutedEventArgs e)
  {
    RenderDispatch.RenderingSlow += (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_3);
    this.method_0("FocusModeEnabled");
  }

  private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
  {
    RenderDispatch.RenderingSlow -= (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.method_3);
  }

  private void method_3(object sender, EventArgs e)
  {
    this.Duration = this.testPlanGridListener_0.Duration;
  }

  private void method_4(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "WorkArea"))
      return;
    GuiHelpers.GuiInvokeAsync((Action) new Action(this.tapDock_0.EnsureWindowsInsideWorkingArea));
  }

  private void method_5()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class91 class91 = new MainWindow.Class91();
    // ISSUE: reference to a compiler-generated field
    class91.mainWindow_0 = this;
    if (this.PlanRunning)
      return;
    // ISSUE: reference to a compiler-generated field
    class91.list_0 = this.testPlanGrid_0.GetExpandedConf();
    // ISSUE: reference to a compiler-generated field
    class91.guid_0 = this.testPlanGrid_0.selectedStepIds();
    this.TestPlanProgressMessage = "Reloading Test Plan";
    // ISSUE: reference to a compiler-generated field
    class91.testPlan_0 = this.Plan;
    // ISSUE: reference to a compiler-generated method
    TapThread.Start(new Action(class91.method_0));
  }

  private void method_6()
  {
    if (this.Dispatcher.CheckAccess())
      throw new Exception("FlushUndoRedo may not be invoked in the GUI thread since this can cause a dead-lock.");
    this.autoResetEvent_1.Set();
    while (this.bool_2)
      TapThread.Sleep(50);
  }

  public void OpenPluginsWindow(object sender, EventArgs e)
  {
    new PackageManagerToolProvider().Execute(e is NewStepControl.NewStepEventArgs newStepEventArgs ? newStepEventArgs.ResourceType : (ITypeData) null);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    try
    {
      base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    }
    catch (Exception ex)
    {
      if (dependencyPropertyChangedEventArgs_0.Property.Name != "WindowChrome")
        throw;
    }
    if (dependencyPropertyChangedEventArgs_0.Property == MainWindow.SettingsLockedProperty)
      this.TapSettings.AllowEdit = !this.SettingsLocked;
    if (dependencyPropertyChangedEventArgs_0.Property == MainWindow.PlanProperty && this.Plan != null && this.Plan.Steps.Count > 0)
      this.welcomeScreen_0.Visibility = System.Windows.Visibility.Collapsed;
    if (dependencyPropertyChangedEventArgs_0.Property != MainWindow.TestPlanProgressMessageProperty || !string.IsNullOrEmpty((string) dependencyPropertyChangedEventArgs_0.NewValue))
      return;
    CommandManager.InvalidateRequerySuggested();
  }

  private void method_7(IEnumerable<Event> ienumerable_0)
  {
    foreach (Event evnt in ienumerable_0)
      this.list_1.Add(new LogMessage(evnt));
  }

  private void method_8(IEnumerable<Event> ienumerable_0)
  {
    this.logPanel_0.AddLogMessages((IEnumerable<Event>) ienumerable_0);
  }

  private void logPanel_0_Loaded(object sender, RoutedEventArgs e)
  {
    this.eventTraceListener_0.MessageLogged -= new EventTraceListener.LogMessageDelegate(this.method_7);
    this.eventTraceListener_0.MessageLogged += new EventTraceListener.LogMessageDelegate(this.method_8);
    this.logPanel_0.AddLogMessages((IReadOnlyList<LogMessage>) this.list_1);
    this.list_1.Clear();
    this.logPanel_0.Loaded -= new RoutedEventHandler(this.logPanel_0_Loaded);
  }

  private void method_9(object sender, RoutedEventArgs e)
  {
    this.Plan.Locked = !this.Plan.Locked;
    this.testPlanChanged();
    UpdateMonitor.Update((object) this, (AnnotationCollection) null);
  }

  private void MainWindow_Closed(object sender, EventArgs e)
  {
    UnhandledExceptionHandler.Unload((Window) this);
  }

  private void method_10()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class93 class93 = new MainWindow.Class93();
    // ISSUE: reference to a compiler-generated field
    class93.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class93.string_0 = App.getCommandLineArg("open");
    try
    {
      if (App.CommandLineFileNameArgument != null)
      {
        if (string.Equals(Path.GetExtension(App.CommandLineFileNameArgument), ".tapplan", StringComparison.CurrentCultureIgnoreCase))
        {
          // ISSUE: reference to a compiler-generated field
          class93.string_0 = App.CommandLineFileNameArgument;
        }
      }
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Warning("Specified filename could not be found: \"{0}\".", (object) App.CommandLineFileNameArgument);
      MainWindow.traceSource_0.Debug(ex);
    }
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvoke((Action) new Action(class93.method_0));
  }

  private void method_11(Type type_0)
  {
    try
    {
      this.Plan.Steps.Add((ITestStep) Activator.CreateInstance(type_0));
      this.testPlanChanged();
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Error("Caught exception while creating step: '{0}'", (object) ex.Message);
      MainWindow.traceSource_0.Debug(ex);
    }
  }

  private MenuItem method_12(string string_10)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class95 class95 = new MainWindow.Class95();
    // ISSUE: reference to a compiler-generated field
    class95.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class95.string_0 = string_10;
    // ISSUE: reference to a compiler-generated field
    string str = class95.string_0.Replace("_", "__");
    MenuItem menuItem = new MenuItem();
    menuItem.Header = (object) str;
    MenuItem target = menuItem;
    this.Bind("PlanRunning", (DependencyObject) target, UIElement.IsEnabledProperty, BindingMode.OneWay, (IValueConverter) MainWindow.invertBooleanConverter_0);
    // ISSUE: reference to a compiler-generated method
    target.Click += new RoutedEventHandler(class95.method_0);
    return target;
  }

  private void method_13()
  {
    int num1 = this.menuItem_0.Items.IndexOf((object) this.separator_1);
    int num2 = this.menuItem_0.Items.IndexOf((object) this.separator_2) - num1 - 1;
    for (int index = 0; index < num2; ++index)
      this.menuItem_0.Items.RemoveAt(num1 + 1);
  }

  private void method_14()
  {
    this.method_13();
    int num = this.menuItem_0.Items.IndexOf((object) this.separator_1);
    GuiControlsSettings current = ComponentSettings<GuiControlsSettings>.Current;
    ((IList<string>) current.RecentTestPlans).smethod_9<string>((Predicate<string>) (string_0 => !System.IO.File.Exists(string_0)));
    IEnumerable<MenuItem> source = ((IEnumerable<string>) current.RecentTestPlans).Select<string, MenuItem>(new Func<string, MenuItem>(this.method_12)).Reverse<MenuItem>();
    foreach (MenuItem insertItem in source)
      this.menuItem_0.Items.Insert(num + 1, (object) insertItem);
    if (source.Any<MenuItem>())
      this.separator_2.Visibility = System.Windows.Visibility.Visible;
    else
      this.separator_2.Visibility = System.Windows.Visibility.Collapsed;
  }

  private DisplayAttribute method_15(IMenuItem imenuItem_0)
  {
    if (!(imenuItem_0 is ToolMenuEntry))
      return imenuItem_0.GetType().smethod_2();
    DisplayAttribute display = ((ToolMenuEntry) imenuItem_0).GetDisplay();
    return new DisplayAttribute(display.Name, display.Description, "Tools", display.Order, display.Collapsed);
  }

  private MainWindowContext mainWindowContext
  {
    get
    {
      return this.mainWindowContext_0 ?? (this.mainWindowContext_0 = MainWindowContext.FromMainWindow(this));
    }
  }

  public MainWindowContext GuiContext => this.mainWindowContext;

  private void method_16()
  {
    IList<(IPromotedMenuItem Instance, ITypeData Type)> tupleList = MainWindow.smethod_2();
    MainWindowContext mainWindowContext = this.mainWindowContext;
    foreach ((IPromotedMenuItem Instance, ITypeData Type) tuple in (IEnumerable<(IPromotedMenuItem Instance, ITypeData Type)>) tupleList)
    {
      if (!this.hashSet_0.Contains(tuple.Type.Name))
      {
        this.hashSet_0.Add(tuple.Type.Name);
        IPromotedMenuItem instance = tuple.Instance;
        if (instance is IInitializedMenuItem initializedMenuItem)
          initializedMenuItem.Initialize((Keysight.OpenTap.Wpf.GuiContext) mainWindowContext);
        UIElement control = instance.Control;
        Grid.SetColumn(control, 2);
        this.stackPanel_0.Children.Add(control);
      }
    }
  }

  private static IList<(IPromotedMenuItem Instance, ITypeData Type)> smethod_2()
  {
    List<(IPromotedMenuItem, ITypeData)> valueTupleList = new List<(IPromotedMenuItem, ITypeData)>();
    foreach (ITypeData type in (IEnumerable<ITypeData>) TypeData.GetDerivedTypes<IPromotedMenuItem>().OrderBy<ITypeData, string>((Func<ITypeData, string>) (itypeData_0 => itypeData_0.GetDisplayAttribute().Name)).OrderBy<ITypeData, double>((Func<ITypeData, double>) (itypeData_0 => itypeData_0.GetDisplayAttribute().Order)))
    {
      try
      {
        if (type.CanCreateInstance)
        {
          IPromotedMenuItem instance = type.CreateInstance() as IPromotedMenuItem;
          valueTupleList.Add((instance, type));
        }
      }
      catch
      {
      }
    }
    return (IList<(IPromotedMenuItem, ITypeData)>) valueTupleList;
  }

  private void method_17()
  {
    this.list_2.ForEach((Action<Action>) (action_5 => action_5()));
    this.list_2.Clear();
    HashSet<object> objectSet = new HashSet<object>();
    objectSet.Add((object) null);
    List<IMenuItem> source = new List<IMenuItem>();
    foreach (Type plugin in PluginManager.GetPlugins<IMenuItem>())
    {
      if (plugin.GetConstructors().Length != 0)
      {
        try
        {
          IMenuItem instance = (IMenuItem) Activator.CreateInstance(plugin);
          if (instance is IInitializedMenuItem initializedMenuItem)
            initializedMenuItem.Initialize((Keysight.OpenTap.Wpf.GuiContext) this.mainWindowContext);
          source.Add(instance);
        }
        catch (Exception ex)
        {
          if (MainWindow.traceSource_0.smethod_27((object) plugin, "Unable to load menuitem plugin {0}", (object) plugin))
            MainWindow.traceSource_0.Debug(ex);
        }
      }
    }
    source.AddRange(ToolMenuEntry.GetToolMenuEntries());
    List<IMenuItem> list = source.OrderBy<IMenuItem, string>((Func<IMenuItem, string>) (imenuItem_0 => this.method_15(imenuItem_0).GetFullName().TrimStart('_'))).OrderBy<IMenuItem, double>((Func<IMenuItem, double>) (imenuItem_0 => this.method_15(imenuItem_0).Order)).ToList<IMenuItem>();
    HashSet<SettingsMenuItem> settingsMenuItemSet = new HashSet<SettingsMenuItem>();
    foreach (IMenuItem menuItem1 in list)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class96 class96 = new MainWindow.Class96();
      // ISSUE: reference to a compiler-generated field
      class96.imenuItem_0 = menuItem1;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class97 class97_1 = new MainWindow.Class97();
      // ISSUE: reference to a compiler-generated field
      class97_1.itemCollection_0 = this.menu_0.Items;
      // ISSUE: reference to a compiler-generated field
      class97_1.menuItem_0 = (MenuItem) null;
      // ISSUE: reference to a compiler-generated field
      DisplayAttribute displayAttribute = this.method_15(class96.imenuItem_0);
      for (int index = 0; index < displayAttribute.Group.Length; ++index)
      {
        string str = displayAttribute.Group[index];
        MenuItem newItem = (MenuItem) null;
        // ISSUE: reference to a compiler-generated field
        foreach (object obj in (IEnumerable) class97_1.itemCollection_0)
        {
          if (obj is MenuItem menuItem2 && string.Equals(menuItem2.Header.ToString().Replace("_", ""), str.Replace("_", "")))
          {
            newItem = menuItem2;
            break;
          }
        }
        if (newItem == null)
        {
          MenuItem menuItem3 = new MenuItem();
          menuItem3.Header = (object) str;
          newItem = menuItem3;
          // ISSUE: reference to a compiler-generated field
          if (class97_1.menuItem_0 is SettingsMenuItem)
          {
            // ISSUE: reference to a compiler-generated field
            settingsMenuItemSet.Add((SettingsMenuItem) class97_1.menuItem_0);
            // ISSUE: reference to a compiler-generated field
            if (!objectSet.Contains((object) class97_1.menuItem_0))
            {
              // ISSUE: reference to a compiler-generated field
              ((List<object>) ((SettingsMenuItem) class97_1.menuItem_0).AdditionalItems).Add((object) new Separator());
            }
            // ISSUE: reference to a compiler-generated field
            ((List<object>) ((SettingsMenuItem) class97_1.menuItem_0).AdditionalItems).Add((object) newItem);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!objectSet.Contains((object) class97_1.menuItem_0) && class97_1.itemCollection_0.Count != 0)
            {
              // ISSUE: reference to a compiler-generated field
              class97_1.itemCollection_0.Add((object) new Separator());
            }
            // ISSUE: reference to a compiler-generated field
            class97_1.itemCollection_0.Add((object) newItem);
          }
          // ISSUE: reference to a compiler-generated field
          objectSet.Add((object) class97_1.menuItem_0);
        }
        // ISSUE: reference to a compiler-generated field
        class97_1.itemCollection_0 = newItem.Items;
        // ISSUE: reference to a compiler-generated field
        class97_1.menuItem_0 = newItem;
      }
      // ISSUE: reference to a compiler-generated field
      IMenuItemIcon imenuItem0_1 = class96.imenuItem_0 as IMenuItemIcon;
      // ISSUE: reference to a compiler-generated field
      IMenuItemIcon2 imenuItem0_2 = class96.imenuItem_0 as IMenuItemIcon2;
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class97 class97_2 = class97_1;
      MenuItem menuItem4 = new MenuItem();
      menuItem4.Header = (object) displayAttribute.Name;
      // ISSUE: reference to a compiler-generated field
      menuItem4.DataContext = (object) class96.imenuItem_0;
      // ISSUE: reference to a compiler-generated field
      class97_2.menuItem_1 = menuItem4;
      if (imenuItem0_1 != null)
      {
        Image image = new Image();
        // ISSUE: reference to a compiler-generated field
        Binding binding = new Binding("IconSource")
        {
          Source = (object) class96.imenuItem_0
        };
        image.SetBinding(Image.SourceProperty, (BindingBase) binding);
        // ISSUE: reference to a compiler-generated field
        class97_1.menuItem_1.Icon = (object) image;
      }
      else if (imenuItem0_2 != null)
      {
        // ISSUE: reference to a compiler-generated field
        class97_1.menuItem_1.Icon = (object) new Image()
        {
          Source = imenuItem0_2.Icon
        };
      }
      // ISSUE: reference to a compiler-generated field
      if (class97_1.menuItem_0 is SettingsMenuItem)
      {
        // ISSUE: reference to a compiler-generated field
        settingsMenuItemSet.Add((SettingsMenuItem) class97_1.menuItem_0);
        // ISSUE: reference to a compiler-generated field
        if (!objectSet.Contains((object) class97_1.menuItem_0))
        {
          // ISSUE: reference to a compiler-generated field
          ((List<object>) ((SettingsMenuItem) class97_1.menuItem_0).AdditionalItems).Add((object) new Separator());
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ((List<object>) ((SettingsMenuItem) class97_1.menuItem_0).AdditionalItems).Add((object) class97_1.menuItem_1);
        // ISSUE: reference to a compiler-generated method
        this.list_2.Add(new Action(class97_1.method_0));
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!objectSet.Contains((object) class97_1.menuItem_0) && class97_1.itemCollection_0.Count != 0)
        {
          // ISSUE: reference to a compiler-generated field
          class97_1.itemCollection_0.Add((object) new Separator());
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        class97_1.itemCollection_0.Add((object) class97_1.menuItem_1);
        // ISSUE: reference to a compiler-generated method
        this.list_2.Add(new Action(class97_1.method_1));
      }
      // ISSUE: reference to a compiler-generated field
      objectSet.Add((object) class97_1.menuItem_0);
      // ISSUE: reference to a compiler-generated field
      RoutedCommand routedCommand = new RoutedCommand(displayAttribute.Name, class97_1.menuItem_1.GetType());
      // ISSUE: reference to a compiler-generated field
      if (class96.imenuItem_0 is IMenuItemShortcut imenuItem0_3)
        routedCommand.InputGestures.Add((InputGesture) imenuItem0_3.Shortcut);
      // ISSUE: reference to a compiler-generated field
      class97_1.menuItem_1.Command = (ICommand) routedCommand;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class97_1.imenuItemVisibility_0 = class96.imenuItem_0 as IMenuItemVisibility;
      // ISSUE: reference to a compiler-generated field
      if (class97_1.imenuItemVisibility_0 != null)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.CommandBindings.Add(new CommandBinding((ICommand) routedCommand, new ExecutedRoutedEventHandler(class96.method_0), new CanExecuteRoutedEventHandler(class97_1.method_2)));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.CommandBindings.Add(new CommandBinding((ICommand) routedCommand, new ExecutedRoutedEventHandler(class96.method_1)));
      }
      foreach (SettingsMenuItem settingsMenuItem in settingsMenuItemSet)
        settingsMenuItem.RearrangeItems();
    }
  }

  private void method_18(string string_10)
  {
    GuiControlsSettings current = ComponentSettings<GuiControlsSettings>.Current;
    ((List<string>) current.RecentTestPlans).Remove(string_10);
    ((List<string>) current.RecentTestPlans).Insert(0, string_10);
    if (((List<string>) current.RecentTestPlans).Count > 5)
      ((List<string>) current.RecentTestPlans).RemoveRange(5, ((List<string>) current.RecentTestPlans).Count - 5);
    this.method_14();
  }

  private void method_19(string string_10)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class98 class98 = new MainWindow.Class98();
    // ISSUE: reference to a compiler-generated field
    class98.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class98.string_0 = string_10;
    if (this.fileStream_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    class98.testPlan_0 = this.Plan;
    // ISSUE: reference to a compiler-generated method
    this.task_0.ContinueWith(new Action<Task>(class98.method_0));
  }

  private bool method_20()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class99 class99 = new MainWindow.Class99();
    // ISSUE: reference to a compiler-generated field
    class99.fileStream_0 = (FileStream) null;
    foreach (string path in (IEnumerable<string>) ((IEnumerable<string>) Directory.GetFiles(".", "~last_session.*.RecoveryTapPlan")).OrderBy<string, DateTime>(new Func<string, DateTime>(System.IO.File.GetLastWriteTimeUtc)))
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        class99.fileStream_0 = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        // ISSUE: reference to a compiler-generated field
        if (class99.fileStream_0.Length == 0L)
        {
          // ISSUE: reference to a compiler-generated field
          string name = class99.fileStream_0.Name;
          // ISSUE: reference to a compiler-generated field
          class99.fileStream_0.Close();
          System.IO.File.Delete(name);
          // ISSUE: reference to a compiler-generated field
          class99.fileStream_0 = (FileStream) null;
        }
        else
          break;
      }
      catch
      {
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (class99.fileStream_0 == null)
      return false;
    MainWindow.traceSource_0.Warning("Recovery test plan detected.");
    try
    {
      this.TestPlanProgressMessage = "Loading recovery test plan.";
      // ISSUE: reference to a compiler-generated field
      class99.recoveryFile_0 = (MainWindow.RecoveryFile) null;
      try
      {
        // ISSUE: reference to a compiler-generated method
        GuiHelpers.WaitAsync((Action) new Action(class99.method_0));
        // ISSUE: reference to a compiler-generated field
        this.TestPlanPath = class99.recoveryFile_0.FilePath;
      }
      catch
      {
        this.Plan = new TestPlan();
      }
      this.method_51();
      this.method_50();
      // ISSUE: reference to a compiler-generated field
      if (class99.recoveryFile_0 != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Plan = class99.recoveryFile_0.TestPlan;
      }
      this.testPlanChanged();
      return true;
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Error("Unable to load recovery plan");
      MainWindow.traceSource_0.Debug(ex);
    }
    finally
    {
      this.TestPlanProgressMessage = (string) null;
      // ISSUE: reference to a compiler-generated field
      class99.fileStream_0.Close();
      try
      {
        // ISSUE: reference to a compiler-generated field
        System.IO.File.Delete(class99.fileStream_0.Name);
      }
      catch
      {
      }
    }
    return false;
  }

  private void method_21(MainWindow.Class85 class85_2 = null)
  {
    this.method_19(this.TestPlanPath == null ? this.Plan.Name : this.TestPlanPath);
  }

  private void method_22()
  {
    if (this.fileStream_0 == null)
      return;
    string name = this.fileStream_0.Name;
    this.fileStream_0.Close();
    System.IO.File.Delete(name);
    this.fileStream_0 = (FileStream) null;
  }

  private TestPlan method_23(string string_10, string[] string_11, bool bool_5)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class100 class100 = new MainWindow.Class100();
    // ISSUE: reference to a compiler-generated field
    class100.mainWindow_0 = this;
    string[] collection = Array.Empty<string>();
    TapSerializer tapSerializer = new TapSerializer();
    ExternalParameterSerializer serializer = tapSerializer.GetSerializer<ExternalParameterSerializer>();
    List<string> stringList1 = new List<string>();
    if (string_11.Length != 0)
      stringList1.AddRange((IEnumerable<string>) string_11);
    if (collection.Length != 0)
      stringList1.AddRange((IEnumerable<string>) collection);
    // ISSUE: reference to a compiler-generated field
    class100.testPlan_0 = new TestPlan();
    List<string> stringList2 = new List<string>();
    OpenTap.TraceSource source1 = OpenTap.Log.CreateSource("CLI");
    foreach (string paramName in stringList1)
    {
      int length = paramName.IndexOf("=");
      if (length == -1)
      {
        try
        {
          stringList2.Add(paramName);
        }
        catch
        {
          throw new ArgumentException("Unable to read external test plan parameter {0}. Expected '=' or ExternalParameters file.", paramName);
        }
      }
      else
      {
        string key = paramName.Substring(0, length);
        string str = paramName.Substring(length + 1);
        serializer.PreloadedValues[key] = str;
      }
    }
    // ISSUE: reference to a compiler-generated field
    class100.testPlan_0 = (TestPlan) tapSerializer.DeserializeFromFile(string_10, (ITypeData) TypeData.FromType(typeof (TestPlan)));
    if (stringList2.Count > 0)
    {
      IEnumerable<IExternalTestPlanParameterImport> source2 = TypeData.FromType(typeof (IExternalTestPlanParameterImport)).DerivedTypes.Where<TypeData>((Func<TypeData, bool>) (typeData_0 => typeData_0.CanCreateInstance)).Select<TypeData, object>((Func<TypeData, object>) (typeData_0 => typeData_0.CreateInstance(Array.Empty<object>()))).OfType<IExternalTestPlanParameterImport>();
      foreach (string str in stringList2)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainWindow.Class101 class101 = new MainWindow.Class101();
        // ISSUE: reference to a compiler-generated field
        class101.string_0 = str;
        // ISSUE: reference to a compiler-generated field
        source1.Info("Loading external parameters from '{0}'.", (object) class101.string_0);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        source2.FirstOrDefault<IExternalTestPlanParameterImport>(new Func<IExternalTestPlanParameterImport, bool>(class101.method_0))?.ImportExternalParameters(class100.testPlan_0, class101.string_0);
      }
    }
    if (string_11.Length != 0)
    {
      foreach (string str in string_11)
      {
        int length = str.IndexOf("=");
        if (length != -1)
        {
          string externalParameterName = str.Substring(0, length);
          // ISSUE: reference to a compiler-generated field
          if (class100.testPlan_0.ExternalParameters.Get(externalParameterName) == null)
            source1.Warning("External parameter '{0}' does not exist in the test plan.", (object) externalParameterName);
        }
      }
    }
    if (bool_5 && tapSerializer.Errors.Any<string>())
    {
      PlanLoadedWithErrorsQuestion dataObject = new PlanLoadedWithErrorsQuestion();
      UserInput.Request((object) dataObject);
      if (dataObject.Response == PlanLoadedWithErrorsQuestion.ResponseType.SaveAs)
      {
        // ISSUE: reference to a compiler-generated method
        GuiHelpers.GuiInvoke((Action) new Action(class100.method_0));
        // ISSUE: reference to a compiler-generated field
        return class100.testPlan_0;
      }
      if (dataObject.Response == PlanLoadedWithErrorsQuestion.ResponseType.Cancel)
        return (TestPlan) null;
    }
    // ISSUE: reference to a compiler-generated field
    return class100.testPlan_0;
  }

  private void method_24(string string_10, string[] string_11, bool bool_5)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class102 class102 = new MainWindow.Class102();
    // ISSUE: reference to a compiler-generated field
    class102.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class102.string_0 = string_10;
    try
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class103 class103 = new MainWindow.Class103()
      {
        class102_0 = class102
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class103.testPlan_0 = this.method_23(class103.class102_0.string_0, string_11, bool_5);
      // ISSUE: reference to a compiler-generated field
      if (class103.testPlan_0 == null)
        return;
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.GuiInvoke((Action) new Action(class103.method_0));
    }
    finally
    {
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.GuiInvoke((Action) new Action(class102.method_0));
    }
  }

  internal void method_25(string string_10, string[] string_11, bool bool_5 = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class104 class104 = new MainWindow.Class104();
    // ISSUE: reference to a compiler-generated field
    class104.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class104.string_0 = string_10;
    // ISSUE: reference to a compiler-generated field
    class104.string_1 = string_11;
    // ISSUE: reference to a compiler-generated field
    class104.bool_0 = bool_5;
    try
    {
      // ISSUE: reference to a compiler-generated field
      this.TestPlanProgressMessage = $"Loading {class104.string_0}.";
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.WaitAsync((Action) new Action(class104.method_0));
    }
    catch (Exception ex)
    {
      if (ex.InnerException is TestPlan.PlanLoadException)
        return;
      MainWindow.traceSource_0.Error("Failed to load test plan.");
      MainWindow.traceSource_0.Debug(ex);
    }
    finally
    {
      this.TestPlanProgressMessage = (string) null;
    }
  }

  public void load(string file = null, bool manualLoad = true)
  {
    if (this.method_26() || this.PlanRunning)
      return;
    if (this.IsPlanDirty)
    {
      switch (QuickDialog.Show("Save?", $"The test plan '{this.Plan.Name}' has not been saved. Do you want to save now?", "Save", "No", "Cancel"))
      {
        case QuickDialog.DialogOption.Yes:
          this.method_29();
          break;
        case QuickDialog.DialogOption.Cancel:
          return;
      }
    }
    if (file == null)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.FileName = "plan";
      openFileDialog1.DefaultExt = ".TapPlan";
      openFileDialog1.Filter = "Test Plan File|*.TapPlan";
      openFileDialog1.InitialDirectory = this.method_86();
      OpenFileDialog openFileDialog2 = openFileDialog1;
      bool? nullable = openFileDialog2.ShowDialog();
      if (nullable.GetValueOrDefault() & nullable.HasValue)
        file = openFileDialog2.FileName;
    }
    Stopwatch timer = Stopwatch.StartNew();
    try
    {
      if (file == null)
        return;
      this.method_25(file, Array.Empty<string>(), true);
    }
    finally
    {
      MainWindow.traceSource_0.Debug(timer, "Loaded Test plan {0}", (object) file);
    }
  }

  private bool method_26()
  {
    if (!this.TestPlanOpened)
      return false;
    MainWindow.traceSource_0.Warning("Resources are open, please close them before changing the test plan.");
    return true;
  }

  private void method_27()
  {
    try
    {
      this.method_29();
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Warning("Error while saving file");
      MainWindow.traceSource_0.Debug(ex);
    }
  }

  private void method_28()
  {
    try
    {
      this.method_30();
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Warning("Error while saving file");
      MainWindow.traceSource_0.Debug(ex);
    }
  }

  public string TestPlanPath
  {
    get => this.string_3;
    set
    {
      this.string_3 = value;
      this.method_19(value);
    }
  }

  private bool method_29(TestPlan testPlan_0 = null, string string_10 = null)
  {
    string str = string_10 ?? this.TestPlanPath;
    TestPlan testPlan = testPlan_0 ?? this.Plan;
    bool flag = true;
    if (str != null)
    {
      if (System.IO.File.Exists(str))
      {
        try
        {
          Stopwatch timer = Stopwatch.StartNew();
          testPlan.Save(str);
          MainWindow.traceSource_0.Info(timer, "Saved test plan to " + str);
          goto label_7;
        }
        catch (Exception ex)
        {
          MainWindow.traceSource_0.Error("Failed to save test plan.");
          MainWindow.traceSource_0.Debug(ex);
          switch (ex)
          {
            case UnauthorizedAccessException _:
            case IOException _:
              QuickDialog.ShowMessage("File is read only.", $"File cannot be saved at:\n{str}..\n\nPlease select a different location.");
              break;
          }
          return false;
        }
      }
    }
    flag = this.method_30();
label_7:
    if (flag)
    {
      this.method_51();
      this.method_50();
    }
    return flag;
  }

  private bool method_30(TestPlan testPlan_0 = null, string string_10 = null)
  {
    if (testPlan_0 == null)
      testPlan_0 = this.Plan;
    string_10 = string_10 ?? (string.IsNullOrEmpty(testPlan_0.Name) ? "plan" : testPlan_0.Name);
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.FileName = string_10;
    saveFileDialog1.DefaultExt = ".TapPlan";
    saveFileDialog1.Filter = "Test Plan File|*.TapPlan";
    saveFileDialog1.InitialDirectory = this.method_86();
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    bool? nullable = saveFileDialog2.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return false;
    try
    {
      this.TestPlanPath = saveFileDialog2.FileName;
      if (!System.IO.File.Exists(this.TestPlanPath))
        System.IO.File.Create(this.TestPlanPath).Close();
      this.method_18(this.TestPlanPath);
      return this.method_29(testPlan_0, this.TestPlanPath);
    }
    catch (Exception ex)
    {
      MainWindow.traceSource_0.Warning("Error while saving file");
      MainWindow.traceSource_0.Debug(ex);
      return false;
    }
  }

  private void method_31(bool bool_5 = true)
  {
    if (this.method_26())
      return;
    if (bool_5 && this.IsPlanDirty)
    {
      switch (QuickDialog.Show("Save?", $"The test plan '{this.Plan.Name}' has not been saved. Do you want to save now?", "Save", "Discard", "Cancel"))
      {
        case QuickDialog.DialogOption.Yes:
          try
          {
            this.method_29();
            break;
          }
          catch (Exception ex)
          {
            MainWindow.traceSource_0.Warning("Error while saving file");
            MainWindow.traceSource_0.Debug(ex);
            return;
          }
        case QuickDialog.DialogOption.Cancel:
          return;
      }
    }
    this.Plan = new TestPlan();
    this.method_51();
    this.method_50();
    this.TestPlanPath = (string) null;
    this.method_21();
    this.testPlanChanged();
  }

  public void Run(TestPlan plan, HashSet<ITestStep> steps = null, Action onFinished = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class105 class105 = new MainWindow.Class105();
    // ISSUE: reference to a compiler-generated field
    class105.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class105.testPlan_0 = plan;
    // ISSUE: reference to a compiler-generated field
    class105.hashSet_0 = steps;
    // ISSUE: reference to a compiler-generated field
    class105.action_0 = onFinished;
    this.PlanRunning = true;
    // ISSUE: reference to a compiler-generated field
    this.TestPlanListener.Plan = class105.testPlan_0;
    // ISSUE: reference to a compiler-generated field
    class105.testPlan_0.BreakOffered += new EventHandler<BreakOfferedEventArgs>(this.method_33);
    this.TestPlanIsStopping = false;
    if (ComponentSettings<EditorSettings>.Current.ClearLog)
      this.logPanel_0.ClearLog();
    this.runCompletedOverlay_0.Verdict = Verdict.NotSet;
    // ISSUE: reference to a compiler-generated field
    class105.testPlan_0.PrintTestPlanRunSummary = true;
    // ISSUE: reference to a compiler-generated field
    class105.list_0 = ComponentSettings<ResultSettings>.Current.ToList<IResultListener>();
    // ISSUE: reference to a compiler-generated field
    class105.list_0.Add((IResultListener) this.TestPlanListener);
    // ISSUE: reference to a compiler-generated field
    class105.list_0.Add((IResultListener) this.class84_0);
    // ISSUE: reference to a compiler-generated field
    class105.list_0.AddRange((IEnumerable<IResultListener>) this.list_3);
    this.mainWindowContext.method_0();
    // ISSUE: reference to a compiler-generated method
    this.tapThread_0 = TapThread.Start(new Action(class105.method_0), "Plan Thread");
  }

  private void method_32(object sender, RoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class107 class107 = new MainWindow.Class107();
    if (this.PlanRunning)
      return;
    // ISSUE: reference to a compiler-generated field
    class107.itestStep_0 = this.testPlanGrid_0.SelectedTestSteps.ToArray<ITestStep>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class107.hashSet_0 = ((IEnumerable<ITestStep>) class107.itestStep_0).ToHashSet<ITestStep>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    HashSet<ITestStep> hashSet = class107.hashSet_0.Where<ITestStep>(new Func<ITestStep, bool>(class107.method_0)).Cast<ITestStep>().ToHashSet<ITestStep>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class107.bool_0 = ((IEnumerable<ITestStep>) class107.itestStep_0).Select<ITestStep, bool>((Func<ITestStep, bool>) (itestStep_0 => itestStep_0.Enabled)).ToArray<bool>();
    // ISSUE: reference to a compiler-generated field
    for (int index = 0; index < class107.itestStep_0.Length; ++index)
    {
      // ISSUE: reference to a compiler-generated field
      class107.itestStep_0[index].Enabled = true;
    }
    // ISSUE: reference to a compiler-generated method
    Action onFinished = new Action(class107.method_1);
    this.Run(this.Plan, hashSet, onFinished);
  }

  private void method_33(object sender, BreakOfferedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class108 class108 = new MainWindow.Class108();
    // ISSUE: reference to a compiler-generated field
    class108.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class108.breakOfferedEventArgs_0 = e;
    // ISSUE: reference to a compiler-generated field
    if (this.testPlanGrid_0.BreakPointTestSteps.Contains(class108.breakOfferedEventArgs_0.TestStepRun.TestStepId) && !this.bool_0)
    {
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.GuiInvoke((Action) new Action(class108.method_0));
    }
    if (!this.bool_0)
      return;
    // ISSUE: reference to a compiler-generated field
    class108.int_0 = this.int_2;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class108.testStepRowItem_0 = this.testPlanGrid_0.GetRowItem(class108.breakOfferedEventArgs_0.TestStepRun.TestStepId);
    // ISSUE: reference to a compiler-generated field
    if (class108.testStepRowItem_0 != null)
    {
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.GuiInvoke((Action) new Action(class108.method_1));
    }
    try
    {
      ++this.breakPointHits;
      // ISSUE: reference to a compiler-generated field
      MainWindow.traceSource_0.Info("Breaking at step '{0}'.", (object) class108.breakOfferedEventArgs_0.TestStepRun.TestStepName);
      // ISSUE: reference to a compiler-generated field
      this.testPlanGrid_0.AsyncBringIntoView(true, class108.breakOfferedEventArgs_0.TestStepRun.TestStepId);
      // ISSUE: reference to a compiler-generated method
      this.testPlanGrid_0.jumpToStepAction = new Action<Guid>(class108.method_2);
      // ISSUE: reference to a compiler-generated field
      while (class108.int_0 == this.int_2)
        TapThread.Sleep(10);
    }
    finally
    {
      this.testPlanGrid_0.jumpToStepAction = (Action<Guid>) null;
      --this.breakPointHits;
      // ISSUE: reference to a compiler-generated field
      if (class108.testStepRowItem_0 != null)
      {
        // ISSUE: reference to a compiler-generated method
        GuiHelpers.GuiInvoke((Action) new Action(class108.method_3));
      }
    }
  }

  private int breakPointHits
  {
    get => this.int_1;
    set
    {
      MainWindow.Class109 class109 = new MainWindow.Class109();
      class109.mainWindow_0 = this;
      class109.int_0 = value;
      if (class109.int_0 == this.int_1)
        return;
      this.int_1 = class109.int_0;
      GuiHelpers.GuiInvoke((Action) new Action(class109.method_0));
    }
  }

  private void method_34(object sender, RoutedEventArgs e)
  {
    if (!this.PlanRunning)
      this.method_35(sender, new RoutedEventArgs());
    if (!this.SingleSteppingStarted)
      this.SingleSteppingStarted = true;
    ++this.int_2;
  }

  private void method_35(object sender, RoutedEventArgs e)
  {
    if (this.SingleSteppingStarted)
    {
      this.SingleSteppingStarted = false;
      ++this.int_2;
    }
    else if (this.PlanRunning)
    {
      this.SingleSteppingStarted = true;
      ++this.int_2;
    }
    else
    {
      this.IsRepeatCountVisible = this.IsRepeatEnabled;
      if (this.IsRepeatEnabled)
        this.StartRepeat(this.Plan);
      else
        this.Run(this.Plan);
    }
  }

  private void method_36(object sender, RoutedEventArgs e)
  {
    if (!this.PlanRunning)
      return;
    this.SingleSteppingStarted = true;
    ++this.int_2;
  }

  private void method_37(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.PlanRunning;
  }

  public void StartRepeat(TestPlan plan)
  {
    this.RepeatCount = 1;
    this.bool_1 = true;
    this.Run(plan);
  }

  private void method_38(IEnumerable<IResultListener> ienumerable_0, string string_10 = null, int int_3 = 1)
  {
    IResultStore resultStore = ienumerable_0.FirstOrDefault<IResultListener>((Func<IResultListener, bool>) (iresultListener_0 => iresultListener_0 is IResultStore)) as IResultStore;
    string str = (string) null;
    if (string_10 != null)
      str = Path.ChangeExtension(string_10, ".TapReport");
    string templatePath = str;
    int numberOfRuns = int_3;
    ResultsViewer.Open(resultStore, (Window) this, templatePath, numberOfRuns);
  }

  private void mainWindow_0_KeyDown(object sender, KeyEventArgs e)
  {
    this.runCompletedOverlay_0.Verdict = Verdict.NotSet;
  }

  private void mainWindow_0_MouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this);
    if (position != (this.nullable_0 ?? position))
      this.runCompletedOverlay_0.Verdict = Verdict.NotSet;
    this.nullable_0 = new Point?(position);
  }

  private void method_39()
  {
    this.TestPlanIsStopping = true;
    if (!this.Plan.IsRunning)
      return;
    MainWindow.traceSource_0.Warning("User requested to end test plan execution. Aborting test plan run.");
    this.tapThread_0?.Abort();
  }

  private void method_40(object sender, RoutedEventArgs e)
  {
    this.method_39();
    this.mainWindowContext.method_4();
  }

  private void method_41(object sender, RoutedEventArgs e)
  {
    this.tapDock_0.ShowPanel(typeof (StepExplorerPlugin).smethod_2().Name);
  }

  private void method_42(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = !ComponentSettings<EditorSettings>.Current.FocusMode;
  }

  private void method_43(object sender, RoutedEventArgs e)
  {
    this.testPlanGrid_0.RemoveSteps(this.testPlanGrid_0.SelectedTestSteps);
    this.testPlanChanged();
  }

  private void method_44(object sender, RoutedEventArgs e)
  {
    DataGrid dataGrid0 = this.testPlanGrid_0.dataGrid_0;
    DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(dataGrid0.Items[dataGrid0.SelectedIndex], dataGrid0.Columns[dataGrid0.Columns.smethod_18<DataGridColumn>((Func<DataGridColumn, bool>) (dataGridColumn_0 => dataGridColumn_0.Header.ToString() == "Name"))]);
    dataGrid0.CurrentCell = dataGridCellInfo;
    dataGrid0.BeginEdit();
  }

  private void method_45(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.testPlanGrid_0.dataGrid_0.SelectedItems.Count == 1 && this.testPlanGrid_0.dataGrid_0.SelectedIndex >= 0 && !this.TestPlanLocked;
  }

  public void ReloadMainUI()
  {
    if (this.TestPlanOpened)
      return;
    this.method_5();
  }

  private void method_46(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = !this.testPlanGrid_0.TestPlan.Locked && !this.testPlanGrid_0.TestPlan.IsRunning;
  }

  private void method_47(object sender, ExecutedRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class110 class110_1 = new MainWindow.Class110();
    // ISSUE: reference to a compiler-generated field
    class110_1.mainWindow_0 = this;
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class110 class110_2 = class110_1;
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.DefaultExt = ".TapPlan";
    openFileDialog.Filter = "Test Plan File|*.TapPlan";
    // ISSUE: reference to a compiler-generated field
    class110_2.openFileDialog_0 = openFileDialog;
    // ISSUE: reference to a compiler-generated field
    bool? nullable = class110_1.openFileDialog_0.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return;
    // ISSUE: reference to a compiler-generated field
    this.TestPlanProgressMessage = "Loading " + class110_1.openFileDialog_0.FileName;
    // ISSUE: reference to a compiler-generated method
    TapThread.Start(new Action(class110_1.method_0));
  }

  private void method_48()
  {
    this.method_54(this.class198_0.method_6(this.class85_1), false);
    this.Focus();
  }

  private void method_49()
  {
    this.method_54(this.class198_0.method_7(this.class85_1), true);
    this.Focus();
  }

  private void method_50()
  {
    this.class198_0.method_10();
    this.class85_1 = this.class85_0;
  }

  private void method_51()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    Task<MainWindow.Class85> task = Task.Factory.StartNew<MainWindow.Class85>(new Func<MainWindow.Class85>(new MainWindow.Class112()
    {
      mainWindow_0 = this,
      testPlan_0 = this.Plan
    }.method_0));
    while (task.Status < TaskStatus.RanToCompletion)
      GuiHelpers.PushDispatcherFrame();
    this.class85_0 = task.Result;
    this.IsPlanDirty = false;
  }

  public void testPlanChanged()
  {
    GuiHelpers.GuiInvokeAsync((Action) (() => CommandManager.InvalidateRequerySuggested()));
    if (this.PlanChanged != null)
      this.PlanChanged((object) this, new EventArgs());
    this.autoResetEvent_0.Set();
    if (this.tapThread_1 != null)
      return;
    lock (this.autoResetEvent_0)
    {
      if (this.tapThread_1 != null)
        return;
      this.tapThread_1 = TapThread.Start((Action) (() =>
      {
        while (true)
        {
          this.autoResetEvent_0.WaitOne();
          this.bool_2 = true;
          do
            ;
          while (!this.autoResetEvent_1.WaitOne(500) && this.autoResetEvent_0.WaitOne(0));
          this.autoResetEvent_0.Reset();
          try
          {
            this.method_53();
            GuiHelpers.GuiInvokeAsync((Action) (() => CommandManager.InvalidateRequerySuggested()));
          }
          catch
          {
          }
        }
      }));
    }
  }

  private Guid[] method_52()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class113 class113 = new MainWindow.Class113();
    // ISSUE: reference to a compiler-generated field
    class113.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class113.guid_0 = Array.Empty<Guid>();
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvoke((Action) new Action(class113.method_0));
    // ISSUE: reference to a compiler-generated field
    return class113.guid_0;
  }

  private void method_53()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class114 class114 = new MainWindow.Class114();
    // ISSUE: reference to a compiler-generated field
    class114.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    class114.testPlan_0 = (TestPlan) null;
    // ISSUE: reference to a compiler-generated field
    class114.guid_0 = Array.Empty<Guid>();
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvoke((Action) new Action(class114.method_0));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class114.class85_0 = new MainWindow.Class85(class114.testPlan_0, class114.guid_0);
    // ISSUE: reference to a compiler-generated field
    if (!object.Equals((object) this.class85_1, (object) class114.class85_0))
      this.class198_0.method_5(this.class85_1);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class114.bool_0 = !class114.class85_0.Equals((object) this.class85_0);
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class114.method_1));
    // ISSUE: reference to a compiler-generated field
    this.class85_1 = class114.class85_0;
    this.bool_2 = false;
  }

  private void method_54(MainWindow.Class85 class85_2, bool bool_5)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvoke((Action) new Action(new MainWindow.Class115()
    {
      bool_0 = bool_5,
      mainWindow_0 = this,
      class85_0 = class85_2
    }.method_0));
  }

  private void method_55() => this.IsPlanDirty = !this.class85_1.Equals((object) this.class85_0);

  private void method_56(byte[] byte_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class116 class116 = new MainWindow.Class116();
    // ISSUE: reference to a compiler-generated field
    class116.byte_0 = byte_1;
    List<bool> expandedConf = this.testPlanGrid_0.GetExpandedConf();
    Guid[] guid_1 = this.testPlanGrid_0.selectedStepIds();
    try
    {
      this.TestPlanProgressMessage = "Loading test plan";
      // ISSUE: reference to a compiler-generated field
      class116.testPlan_0 = this.Plan;
      // ISSUE: reference to a compiler-generated method
      GuiHelpers.WaitAsync((Action) new Action(class116.method_0));
      // ISSUE: reference to a compiler-generated field
      this.Plan = class116.testPlan_0;
    }
    finally
    {
      this.TestPlanProgressMessage = (string) null;
    }
    this.testPlanGrid_0.SetExpandedConf(expandedConf);
    this.testPlanGrid_0.setSelectedStepIds(guid_1);
    this.method_55();
  }

  private void method_57()
  {
    ComponentSettings<GuiControlsSettings>.Current.MainWindowWidth = this.Width;
    ComponentSettings<GuiControlsSettings>.Current.MainWindowHeight = this.Height;
    ComponentSettings<GuiControlsSettings>.Current.MainWindowTop = this.Top;
    ComponentSettings<GuiControlsSettings>.Current.MainWindowLeft = this.Left;
    ComponentSettings<GuiControlsSettings>.Current.MainWindowState = (TapWindowState) this.WindowState;
    ComponentSettings<GuiControlsSettings>.Current.Save();
    this.tapDock_0.SaveLayout();
    ComponentSettings<PanelSettings>.Current.Save();
    ComponentSettings<EditorSettings>.Current.Save();
  }

  private void mainWindow_0_Closing(object sender, CancelEventArgs e)
  {
    try
    {
      if (this.PlanRunning)
      {
        if (QuickDialog.Show("Stop Test Plan?", "A test plan is running. Do you want to stop the test plan?", "Stop Plan", (string) null, "Cancel") == QuickDialog.DialogOption.Cancel)
        {
          e.Cancel = true;
          return;
        }
        this.method_39();
        Stopwatch stopwatch = Stopwatch.StartNew();
        MainWindow.Class83 class83_0 = this.method_1("Stopping test plan.");
        try
        {
          while (this.PlanRunning)
          {
            if (stopwatch.Elapsed < TimeSpan.FromSeconds(10.0))
              GuiHelpers.PushDispatcherFrame();
            else
              break;
          }
        }
        finally
        {
          this.method_2(class83_0);
        }
        if (this.PlanRunning && QuickDialog.Show("Continue?", "Unable to stop the test plan. Continue exiting?", "Exit", (string) null, "Cancel") == QuickDialog.DialogOption.Cancel)
        {
          e.Cancel = true;
          return;
        }
      }
      if (this.TestPlanOpened)
      {
        if (QuickDialog.Show("Close Resources?", "The connections to the resources are still open and must be closed before exiting", "Close", (string) null, "Cancel") == QuickDialog.DialogOption.Yes)
        {
          try
          {
            this.TestPlanOpened = false;
            this.Plan.Close();
          }
          catch (Exception ex)
          {
            MainWindow.traceSource_0.Warning("Error while closing resources.");
            MainWindow.traceSource_0.Debug(ex);
            e.Cancel = true;
          }
        }
        else
        {
          e.Cancel = true;
          return;
        }
      }
      EditorSettings current = ComponentSettings<EditorSettings>.Current;
      if (this.IsPlanDirty && current.ShowSaveBeforeExit)
      {
        bool flag;
        switch (QuickDialog.Show("Save before exit?", $"The test plan '{this.Plan.Name}' has not been saved.", ref flag, "Save", "Exit", "Cancel"))
        {
          case QuickDialog.DialogOption.Yes:
            try
            {
              current.ShowSaveBeforeExit = !flag;
              e.Cancel = !this.method_29();
              break;
            }
            catch (Exception ex)
            {
              MainWindow.traceSource_0.Warning("Error while saving plan");
              MainWindow.traceSource_0.Debug(ex);
              e.Cancel = true;
              break;
            }
          case QuickDialog.DialogOption.No:
            current.ShowSaveBeforeExit = !flag;
            break;
          case QuickDialog.DialogOption.Cancel:
            e.Cancel = true;
            return;
        }
      }
      else if (current.ShowQuitConfirmation)
      {
        bool flag;
        switch (QuickDialog.Show("Really Quit?", "Are you sure you want to quit?", ref flag, "Yes", (string) null, "Cancel"))
        {
          case QuickDialog.DialogOption.Yes:
            current.ShowQuitConfirmation = !flag;
            break;
          case QuickDialog.DialogOption.Cancel:
            e.Cancel = true;
            return;
        }
      }
      this.method_22();
    }
    finally
    {
      this.method_57();
    }
  }

  public bool IsPlanDirty
  {
    get => (bool) this.GetValue(MainWindow.IsPlanDirtyProperty);
    set => this.SetValue(MainWindow.IsPlanDirtyProperty, (object) value);
  }

  private void SettingsPanel_PropertyEdited(object sender, EventArgs e)
  {
    if (this.bool_3)
      return;
    this.testPlanChanged();
  }

  private void logPanel_0_MouseDown(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    (sender as UIElement).Focus();
  }

  private void method_58(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning;
  }

  private void method_59(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning && !this.FocusModeEnabled;
  }

  private void method_60(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning && !this.FocusModeEnabled;
  }

  private void method_61(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning && !this.TestPlanOpened;
  }

  private void method_62(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.PlanRunning;
  }

  private void method_63(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.SingleSteppingStarted)
      e.CanExecute = true;
    else
      e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning && this.Plan.EnabledSteps.Count<ITestStep>() > 0;
  }

  private void method_64(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && !this.PlanRunning && this.TestPlanProgressMessage == null && this.testPlanGrid_0.SelectedTestSteps.Any<ITestStep>();
  }

  private void method_65(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.Plan.EnabledSteps.Count > 0 && (!this.SingleSteppingStarted || this.BreakPointHit);
  }

  private void method_66(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.PlanRunning && (this.Plan.EnabledSteps.Count<ITestStep>() > 0 || this.TestPlanOpened);
  }

  private void method_67(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense && this.TestPlanProgressMessage == null && !this.TestPlanLocked && Clipboard.ContainsText() && !this.testPlanGrid_0.FilterMode;
  }

  private void method_68(object sender, CanExecuteRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class117 class117 = new MainWindow.Class117();
    this.method_63(sender, e);
    if (this.testPlanGrid_0.SelectedTestSteps.Any<ITestStep>() && !this.TestPlanLocked && !this.testPlanGrid_0.FilterMode)
    {
      // ISSUE: reference to a compiler-generated field
      class117.hashSet_0 = new HashSet<ITestStep>();
      // ISSUE: reference to a compiler-generated field
      Class24.smethod_14<ITestStep>(this.testPlanGrid_0.SelectedTestSteps, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps), (ISet<ITestStep>) class117.hashSet_0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ITestStep[] array1 = class117.hashSet_0.Where<ITestStep>(new Func<ITestStep, bool>(class117.method_0)).ToArray<ITestStep>();
      ITestStepParent[] array2 = ((IEnumerable<ITestStep>) array1).Select<ITestStep, ITestStepParent>((Func<ITestStep, ITestStepParent>) (itestStep_0 => itestStep_0.Parent)).ToArray<ITestStepParent>();
      e.CanExecute = ((IEnumerable<ITestStep>) array1).Any<ITestStep>() && !((IEnumerable<ITestStepParent>) array2).Any<ITestStepParent>((Func<ITestStepParent, bool>) (itestStepParent_0 => itestStepParent_0.ChildTestSteps.IsReadOnly));
    }
    else
      e.CanExecute = false;
  }

  private void method_69(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.class198_0.method_4() > 0 && this.TestPlanProgressMessage == null && !this.PlanRunning;
  }

  private void method_70(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.class198_0.method_3() > 0 && this.TestPlanProgressMessage == null && !this.PlanRunning;
  }

  private void method_71(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.GotLicense;
  }

  private void method_72(object sender, ExecutedRoutedEventArgs e)
  {
    this.method_31();
    this.welcomeScreen_0.Visibility = System.Windows.Visibility.Collapsed;
  }

  private void method_73(object sender, ExecutedRoutedEventArgs e) => this.load();

  private void method_74(object sender, ExecutedRoutedEventArgs e)
  {
    this.byte_0 = (byte[]) null;
    if (this.Plan != null)
    {
      try
      {
        this.manualResetEvent_0 = new ManualResetEvent(false);
        TapThread.Start((Action) (() =>
        {
          this.method_6();
          MainWindow.Class85 class851 = this.class85_1;
          byte[] numArray;
          if (class851 == null)
          {
            numArray = (byte[]) null;
          }
          else
          {
            numArray = class851.method_2();
            if (numArray != null)
              goto label_4;
          }
          numArray = this.class85_0.method_2();
label_4:
          this.byte_0 = numArray;
          this.manualResetEvent_0.Set();
        }));
      }
      catch
      {
      }
    }
    this.method_57();
    this.TapSettings.ReloadUi = (Action) new Action(this.ReloadMainUI);
    this.bool_3 = true;
    try
    {
      this.TapSettings.OpenSettings(e.Parameter);
    }
    finally
    {
      this.bool_3 = false;
    }
  }

  private void method_75(object sender, ExecutedRoutedEventArgs e) => this.method_27();

  private void method_76(object sender, ExecutedRoutedEventArgs e) => this.method_28();

  private void method_77(object sender, ExecutedRoutedEventArgs e) => this.method_48();

  private void method_78(object sender, ExecutedRoutedEventArgs e) => this.method_49();

  private void method_79(object sender, ExecutedRoutedEventArgs e) => this.Close();

  private void method_80(object sender, ExecutedRoutedEventArgs e)
  {
    this.testPlanGrid_0.DoPaste();
    this.welcomeScreen_0.Visibility = System.Windows.Visibility.Collapsed;
  }

  private void method_81(object sender, KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Return:
        this.IsRepeatEnabled = !this.IsRepeatEnabled;
        ((FrameworkElement) sender).GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        break;
      case Key.Escape:
        this.Focus();
        this.controlDropDown_0.IsDropDownOpen = false;
        break;
      case Key.Add:
      case Key.Subtract:
      case Key.OemPlus:
      case Key.OemMinus:
        e.Handled = true;
        break;
    }
  }

  private bool method_82(DragEventArgs dragEventArgs_0)
  {
    return dragEventArgs_0.Data != null && dragEventArgs_0.Data.GetDataPresent(DataFormats.FileDrop);
  }

  private void mainWindow_0_Drop(object sender, DragEventArgs e)
  {
    if (!this.method_82(e))
      return;
    string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];
    if (data.Length == 0)
      return;
    this.load(data[0]);
  }

  private void mainWindow_0_DragOver(object sender, DragEventArgs e) => this.updateCursor(e);

  private void mainWindow_0_DragEnter(object sender, DragEventArgs e) => this.updateCursor(e);

  protected void updateCursor(DragEventArgs dragEventArgs_0)
  {
    dragEventArgs_0.Effects = DragDropEffects.None;
    if (!this.method_82(dragEventArgs_0) || !(dragEventArgs_0.Data.GetData(DataFormats.FileDrop) is string[] data) || data.Length == 0 || !data[0].ToLower().EndsWith(".TapPlan".ToLower()))
      return;
    dragEventArgs_0.Effects = DragDropEffects.Copy;
  }

  private void method_83(object sender, RoutedEventArgs e)
  {
    // ISSUE: variable of a compiler-generated type
    MainWindow.Struct8 stateMachine;
    // ISSUE: reference to a compiler-generated field
    stateMachine.asyncVoidMethodBuilder_0 = AsyncVoidMethodBuilder.Create();
    // ISSUE: reference to a compiler-generated field
    stateMachine.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    stateMachine.int_0 = -1;
    // ISSUE: reference to a compiler-generated field
    stateMachine.asyncVoidMethodBuilder_0.Start<MainWindow.Struct8>(ref stateMachine);
  }

  public void MonitorPropertyExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    this.testPlanGrid_0.LoadMonitorVariable((IMemberData) e.Parameter);
  }

  public void RemoveMonitorPropertyExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    this.testPlanGrid_0.RemoveMonitorVariable((IMemberData) e.Parameter);
  }

  public void CanExecuteMonitorProperty(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = !this.testPlanGrid_0.MonitorVariableVisible((IMemberData) e.Parameter);
  }

  public void RemoveCanExecuteMonitorProperty(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.testPlanGrid_0.MonitorVariableVisible((IMemberData) e.Parameter);
  }

  private void method_84(TestPlan testPlan_0)
  {
    if (testPlan_0 == null)
      return;
    this.Plan = testPlan_0;
    this.method_51();
    this.method_50();
    this.method_55();
  }

  private List<T> method_85<T>()
  {
    ReadOnlyCollection<Type> plugins = PluginManager.GetPlugins<T>();
    List<T> objList = new List<T>();
    foreach (Type type in plugins)
    {
      try
      {
        T instance = (T) Activator.CreateInstance(type);
        objList.Add(instance);
      }
      catch (Exception ex)
      {
        MainWindow.traceSource_0.Error("Unable to load import plugin of type '{0}'.", (object) type.Name);
        MainWindow.traceSource_0.Debug(ex);
      }
    }
    return objList;
  }

  public event EventHandler PluginsChanged;

  public event PropertyChangedEventHandler PropertyChanged;

  public void OnPluginsChanged()
  {
    this.method_87();
    this.TapSettings.UpdateSettingsLists();
    this.method_17();
    this.method_16();
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, EventArgs.Empty);
  }

  private string method_86()
  {
    return this.Plan != null && this.Plan.Directory != null ? this.Plan.Directory : EngineSettings.StartupDir;
  }

  private void method_87()
  {
    bool flag1 = PluginManager.GetPlugins<ITestPlanImport>().Any<Type>();
    bool flag2 = PluginManager.GetPlugins<ITestPlanExport>().Any<Type>();
    ReadOnlyCollection<Type> plugins1 = PluginManager.GetPlugins<ITestPlanImportCustomDialog>();
    ReadOnlyCollection<Type> plugins2 = PluginManager.GetPlugins<ITestPlanExportCustomDialog>();
    this.menuItem_2.Visibility = flag2 || plugins2.Any<Type>() ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    this.menuItem_1.Visibility = flag1 || plugins1.Any<Type>() ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    this.separator_0.Visibility = flag1 | flag2 || plugins1.Any<Type>() || plugins2.Any<Type>() ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    if (plugins1.Any<Type>())
    {
      if (!flag1 && plugins1.Count == 1)
      {
        this.menuItem_1.Command = (ICommand) MainWindow.Import;
        this.menuItem_1.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Import, new ExecutedRoutedEventHandler(this.CustomDialogImportExecuted), new CanExecuteRoutedEventHandler(this.method_61)));
        this.menuItem_1.CommandParameter = (object) $"{plugins1.First<Type>().FullName}+{plugins1.First<Type>().AssemblyQualifiedName}";
      }
      else
      {
        foreach (Type memberInfo_0 in plugins1)
        {
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) memberInfo_0.smethod_2().Name;
          newItem.Command = (ICommand) MainWindow.Import;
          newItem.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Import, new ExecutedRoutedEventHandler(this.CustomDialogImportExecuted), new CanExecuteRoutedEventHandler(this.method_61)));
          newItem.CommandParameter = (object) $"{memberInfo_0.FullName}+{memberInfo_0.AssemblyQualifiedName}";
          this.menuItem_1.Items.Add((object) newItem);
        }
      }
    }
    if (flag1)
    {
      if (!plugins1.Any<Type>())
      {
        this.menuItem_1.Command = (ICommand) MainWindow.Import;
        this.menuItem_1.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Import, new ExecutedRoutedEventHandler(this.ImportExecuted), new CanExecuteRoutedEventHandler(this.method_61)));
      }
      else
      {
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) "File ...";
        newItem.Command = (ICommand) MainWindow.Import;
        newItem.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Import, new ExecutedRoutedEventHandler(this.ImportExecuted), new CanExecuteRoutedEventHandler(this.method_61)));
        this.menuItem_1.Items.Add((object) newItem);
      }
    }
    if (plugins2.Any<Type>())
    {
      if (!flag2 && plugins2.Count == 1)
      {
        this.menuItem_2.Command = (ICommand) MainWindow.Export;
        this.menuItem_2.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Export, new ExecutedRoutedEventHandler(this.CustomDialogExportExecuted), new CanExecuteRoutedEventHandler(this.method_58)));
        this.menuItem_2.CommandParameter = (object) $"{plugins2.First<Type>().FullName}+{plugins2.First<Type>().AssemblyQualifiedName}";
      }
      else
      {
        foreach (Type memberInfo_0 in plugins2)
        {
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) memberInfo_0.smethod_2().Name;
          newItem.Command = (ICommand) MainWindow.Export;
          newItem.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Export, new ExecutedRoutedEventHandler(this.CustomDialogExportExecuted), new CanExecuteRoutedEventHandler(this.method_58)));
          newItem.CommandParameter = (object) $"{memberInfo_0.FullName}+{memberInfo_0.AssemblyQualifiedName}";
          this.menuItem_2.Items.Add((object) newItem);
        }
      }
    }
    if (!flag2)
      return;
    if (!plugins2.Any<Type>())
    {
      this.menuItem_2.Command = (ICommand) MainWindow.Export;
      this.menuItem_2.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Export, new ExecutedRoutedEventHandler(this.ExportExecuted), new CanExecuteRoutedEventHandler(this.method_58)));
    }
    else
    {
      MenuItem newItem = new MenuItem();
      newItem.Header = (object) "File ...";
      newItem.Command = (ICommand) MainWindow.Export;
      newItem.CommandBindings.Add(new CommandBinding((ICommand) MainWindow.Export, new ExecutedRoutedEventHandler(this.ExportExecuted), new CanExecuteRoutedEventHandler(this.method_58)));
      this.menuItem_2.Items.Add((object) newItem);
    }
  }

  public void CustomDialogImportExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    this.method_84(this.method_85<ITestPlanImportCustomDialog>().First<ITestPlanImportCustomDialog>(new Func<ITestPlanImportCustomDialog, bool>(new MainWindow.Class120()
    {
      executedRoutedEventArgs_0 = e
    }.method_0)).ImportTestPlan());
  }

  public void CustomDialogExportExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    this.method_85<ITestPlanExportCustomDialog>().First<ITestPlanExportCustomDialog>(new Func<ITestPlanExportCustomDialog, bool>(new MainWindow.Class121()
    {
      executedRoutedEventArgs_0 = e
    }.method_0)).ExportTestPlan(this.Plan);
  }

  public void ImportExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    List<ITestPlanImport> source = this.method_85<ITestPlanImport>();
    string str = string.Join("|", source.Select<ITestPlanImport, string>((Func<ITestPlanImport, string>) (itestPlanImport_0 => string.Format("{0} ({1})|*{1}", (object) itestPlanImport_0.Name, (object) itestPlanImport_0.Extension))));
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = str;
    openFileDialog1.Title = "Import Test Plan";
    openFileDialog1.InitialDirectory = this.method_86();
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (!openFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    this.method_84(source[openFileDialog2.FilterIndex - 1].ImportTestPlan(openFileDialog2.FileName));
    this.TestPlanPath = (string) null;
  }

  public void ExportExecuted(object sender, ExecutedRoutedEventArgs e)
  {
    List<ITestPlanExport> source = this.method_85<ITestPlanExport>();
    string str = string.Join("|", source.Select<ITestPlanExport, string>((Func<ITestPlanExport, string>) (itestPlanExport_0 => string.Format("{0} ({1})|*{1}", (object) itestPlanExport_0.Name, (object) itestPlanExport_0.Extension))));
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.Filter = str;
    saveFileDialog1.Title = "Export Test Plan";
    saveFileDialog1.InitialDirectory = this.method_86();
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (!saveFileDialog2.ShowDialog().GetValueOrDefault())
      return;
    source[saveFileDialog2.FilterIndex - 1].ExportTestPlan(this.Plan, saveFileDialog2.FileName);
  }

  private void menuItem_8_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start(this.string_4, this.string_5 + (sender as FrameworkElement).Tag?.ToString());
    }
    catch
    {
      MainWindow.traceSource_0.Error(this.string_6);
    }
  }

  private void menuItem_9_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start("http://www.keysight.com/find/TAP");
    }
    catch
    {
    }
  }

  private void menuItem_10_Click(object sender, RoutedEventArgs e)
  {
    Assembly executingAssembly = Assembly.GetExecutingAssembly();
    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
    string version = $"{versionInfo.ProductVersion} {(Environment.Is64BitProcess ? (object) "(64-bit)" : (object) "(32-bit)")}";
    if (!versionInfo.ProductVersion.Contains("-"))
      version += " Official Release";
    string applicationName = "Keysight Test Automation";
    if (Class61.smethod_1())
      applicationName += " - Community Edition";
    Installation installation = new Installation(Path.GetDirectoryName(executingAssembly.Location));
    PackageDef packageDef = installation.GetPackages().FirstOrDefault<PackageDef>((Func<PackageDef, bool>) (packageDef_0 => packageDef_0.Name.StartsWith("Editor")));
    if (packageDef != null)
      version += $"\nRelease Date: {packageDef.Date:yyyy.MM.dd}";
    PackageDef openTapPackage = installation.GetOpenTapPackage();
    if (openTapPackage != null)
      version += $"\n\nBuilt on OpenTAP™ v{openTapPackage.Version}";
    AboutBox aboutBox = new AboutBox(applicationName, version, "2012-2022", new Uri("/Images/KeysightApplicationIconTAP64.png", UriKind.Relative));
    aboutBox.Commands.Remove(aboutBox.Commands.FirstOrDefault<CommandViewModel>((Func<CommandViewModel, bool>) (commandViewModel_0 => commandViewModel_0.ToolTipText == "http://www.keysight.com/go/products")));
    aboutBox.Commands.Add((CommandViewModel) new HyperlinkCommandViewModel("Support", "http://www.keysight.com/find/TAP", new Uri("http://www.keysight.com/find/TAP")));
    aboutBox.ShowModal((Window) this);
  }

  public DependencyObject ShowExternalTestPlanParameters()
  {
    return this.tapDock_0.ShowPanel(typeof (TestPlanSettings).smethod_2().Name);
  }

  private void Executed_ShowTestPlanSettings(object sender, ExecutedRoutedEventArgs e)
  {
    this.ShowExternalTestPlanParameters().FindVisualChild<FrameworkElement>()?.Focus();
  }

  private void Executed_ShowParameters(object object_0, IMemberData member)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MainWindow.Class122 class122 = new MainWindow.Class122();
    // ISSUE: reference to a compiler-generated field
    class122.object_0 = object_0;
    // ISSUE: reference to a compiler-generated field
    class122.mainWindow_0 = this;
    // ISSUE: reference to a compiler-generated field
    if (class122.object_0 == this.Plan)
    {
      // ISSUE: reference to a compiler-generated field
      class122.propGrid_0 = this.ShowExternalTestPlanParameters().FindVisualChild<PropGrid>();
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      this.testPlanGrid_0.SetSelectedItem((ITestStep) (class122.object_0 as TestStep));
      // ISSUE: reference to a compiler-generated field
      class122.propGrid_0 = this.propGrid_0;
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class122.method_0(member, (FrameworkElement) class122.propGrid_0);
  }

  public void ResultsOther_Click(object sender, RoutedEventArgs e)
  {
    string string_10 = (string) null;
    if (this.Plan != null)
      string_10 = this.Plan.Path;
    this.method_38((IEnumerable<IResultListener>) ComponentSettings<ResultSettings>.Current, string_10);
  }

  public static long GetRunID(IData data)
  {
    IParameter parameter = data.Parameters.FirstOrDefault<IParameter>((Func<IParameter, bool>) (iparameter_0 => iparameter_0.Name == "Run ID"));
    return parameter != null ? Convert.ToInt64((object) parameter.Value) : data.GetID();
  }

  private void method_88(bool bool_5)
  {
    this.method_89();
    this.tapDock_0.LoadDockingPlugins();
    this.menuItem_5.ItemsSource = this.tapDock_0.Tag as IEnumerable;
    if (bool_5)
      this.tapDock_0.LoadLayout();
    GuiHelpers.GuiInvokeAsync((Action) new Action(this.tapDock_0.EnsureWindowsInsideWorkingArea), priority: DispatcherPriority.Input);
  }

  private void method_89()
  {
    if (this.string_7 == null)
      this.string_7 = this.Title;
    if (this.string_8 == null)
      this.string_8 = (string) HelpManager.GetHelpLink((DependencyObject) this);
    this.tapDock_0.FocusMode = this.FocusModeEnabled;
    if (this.FocusModeEnabled)
    {
      this.Title = this.string_7 + " (Focus Mode)";
      HelpManager.SetHelpLink((DependencyObject) this, (object) "EditorHelp.chm::/Editor Overview/Focus Mode.html");
    }
    else
    {
      this.Title = this.string_7;
      HelpManager.SetHelpLink((DependencyObject) this, (object) this.string_8);
    }
  }

  TapThread ITapDockContext.Run()
  {
    this.Run(this.Plan);
    return this.tapThread_0;
  }

  TestPlan ITapDockContext.Plan
  {
    get => this.Plan;
    set
    {
      if (this.Plan.IsRunning)
        throw new InvalidOperationException("Cannot set the plan while it is running.");
      this.Plan = !this.Plan.IsOpen ? value : throw new InvalidOperationException("Cannot set the plan while it is open.");
    }
  }

  TapSettings ITapDockContext.Settings => this.TapSettings;

  List<IResultListener> ITapDockContext.ResultListeners => this.list_3;

  private void method_90(object sender, RoutedEventArgs e)
  {
    this.grid_1.Visibility = System.Windows.Visibility.Collapsed;
    Grid parent1 = this.grid_1.Parent as Grid;
    FrameworkElement parent2 = GuiHelpers.FindParent<FrameworkElement>((DependencyObject) (sender as FrameworkElement), (Func<FrameworkElement, bool>) (frameworkElement_0 => frameworkElement_0.Name == "ChromeModeMenuContentContainer"));
    if (parent2 != null)
      parent2.Height = double.NaN;
    parent1.Children.Remove((UIElement) this.grid_1);
    ((sender as Decorator).Child as Grid).Children.Add((UIElement) this.grid_1);
    this.grid_1.Visibility = System.Windows.Visibility.Visible;
  }

  private void method_91(Action action_0, string string_10)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(new MainWindow.Class124()
    {
      mainWindow_0 = this,
      string_0 = string_10,
      action_0 = action_0
    }.method_0), priority: DispatcherPriority.Loaded);
  }

  private void method_92(object sender, EventArgs e)
  {
    this.autoResetEvent_1.Set();
    this.method_91(new Action(this.method_6), "  ");
  }

  public IEnumerable<object> SelectedItems
  {
    get => (IEnumerable<object>) this.testPlanGrid_0.SelectedTestSteps;
    set => this.testPlanGrid_0.SetSelectedItems(value.OfType<ITestStep>());
  }

  private void method_93(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "SelectedTestSteps"))
      return;
    this.method_0("SelectedItems");
  }

  private void propGrid_0_TestPlanModified(object sender, EventArgs e)
  {
    this.testPlanChanged();
    this.testPlanGrid_0.updateView(true);
  }

  public bool CanHandleCommand(string commandName, AnnotationCollection context)
  {
    if (commandName != null)
    {
      switch (commandName.Length)
      {
        case 40:
          if (commandName == "Keysight.OpenTap.Wpf.IconNames.CopyValue")
            return context.Get<IStringReadOnlyValueAnnotation>() != null;
          break;
        case 44:
          if (commandName == "Keysight.OpenTap.Wpf.IconNames.ShowParameter")
          {
            IMemberData member = context.Get<IMemberAnnotation>().Member;
            return member != null && context.Source is ITestStep source && ItemUi.GetScope((object) source, member).member is ParameterMemberData;
          }
          break;
        case 48 /*0x30*/:
          if (commandName == "Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput")
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MainWindow.Class127 class127 = new MainWindow.Class127();
            // ISSUE: reference to a compiler-generated field
            class127.imemberData_0 = context.Get<IMemberAnnotation>().Member;
            object source = context.Source;
            // ISSUE: reference to a compiler-generated field
            class127.itestStepParent_0 = source as ITestStepParent;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            return class127.itestStepParent_0 != null && InputOutputRelation.GetRelations(class127.itestStepParent_0).FirstOrDefault<InputOutputRelation>(new Func<InputOutputRelation, bool>(class127.method_0))?.InputObject is ITestStep;
          }
          break;
        case 49:
          switch (commandName[35])
          {
            case 'A':
              if (commandName == "Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput")
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                MainWindow.Class126 class126 = new MainWindow.Class126();
                // ISSUE: reference to a compiler-generated field
                class126.imemberData_0 = context.Get<IMemberAnnotation>().Member;
                object source = context.Source;
                // ISSUE: reference to a compiler-generated field
                class126.itestStepParent_0 = source as ITestStepParent;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                return class126.itestStepParent_0 != null && InputOutputRelation.GetRelations(class126.itestStepParent_0).FirstOrDefault<InputOutputRelation>(new Func<InputOutputRelation, bool>(class126.method_0))?.OutputObject is ITestStep;
              }
              break;
            case 'I':
              if (commandName == "Keysight.OpenTap.Wpf.IconNames.ShowInTestPlanView")
              {
                IMemberData member = context.Get<IMemberAnnotation>().Member;
                return member != null && context.Source is ITestStep && !this.testPlanGrid_0.MonitorVariableVisible(member);
              }
              break;
            case 'v':
              if (commandName == "Keysight.OpenTap.Wpf.IconNames.RemoveFromPlanView")
              {
                IMemberData member = context.Get<IMemberAnnotation>().Member;
                return member != null && context.Source is ITestStep && this.testPlanGrid_0.MonitorVariableVisible(member);
              }
              break;
          }
          break;
        case 52:
          switch (commandName[31 /*0x1F*/])
          {
            case 'E':
              if (commandName == "Keysight.OpenTap.Wpf.IconNames.ExportParameterValues")
                return context.Source is TestPlan;
              break;
            case 'I':
              if (commandName == "Keysight.OpenTap.Wpf.IconNames.ImportParameterValues" && context.Source is TestPlan source1 && !source1.IsRunning)
                return !source1.Locked;
              break;
          }
          break;
      }
    }
    return false;
  }

  public static (ITestStepParent target, IParameterMemberData parameter) findParameter(
    ITestStepParent itestStepParent_0,
    IMemberData member)
  {
    (object, IMemberData) valueTuple = ((object) itestStepParent_0, member);
    while (itestStepParent_0 != null)
    {
      itestStepParent_0 = itestStepParent_0.Parent;
      foreach (IParameterMemberData parameterMemberData in TypeData.GetTypeData((object) itestStepParent_0).GetMembers().OfType<IParameterMemberData>())
      {
        if (parameterMemberData.ParameterizedMembers.Contains<(object, IMemberData)>(valueTuple))
          return (itestStepParent_0, parameterMemberData);
      }
    }
    return ((ITestStepParent) null, (IParameterMemberData) null);
  }

  public void HandleCommand(string commandName, AnnotationCollection context)
  {
    if (commandName == null)
      return;
    switch (commandName.Length)
    {
      case 44:
        int num = commandName == "Keysight.OpenTap.Wpf.IconNames.ShowParameter" ? 1 : 0;
        break;
      case 48 /*0x30*/:
        if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput"))
          break;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MainWindow.Class129 class129 = new MainWindow.Class129();
        // ISSUE: reference to a compiler-generated field
        class129.imemberData_0 = context.Get<IMemberAnnotation>().Member;
        // ISSUE: reference to a compiler-generated field
        class129.itestStepParent_0 = context.Source as ITestStepParent;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!(InputOutputRelation.GetRelations(class129.itestStepParent_0).FirstOrDefault<InputOutputRelation>(new Func<InputOutputRelation, bool>(class129.method_0))?.InputObject is ITestStep inputObject))
          break;
        this.testPlanGrid_0.SetSelectedItem(inputObject);
        break;
      case 49:
        switch (commandName[35])
        {
          case 'A':
            if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput"))
              return;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MainWindow.Class128 class128 = new MainWindow.Class128();
            // ISSUE: reference to a compiler-generated field
            class128.imemberData_0 = context.Get<IMemberAnnotation>().Member;
            // ISSUE: reference to a compiler-generated field
            class128.itestStepParent_0 = (ITestStepParent) context.Source;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.testPlanGrid_0.SetSelectedItem(InputOutputRelation.GetRelations(class128.itestStepParent_0).FirstOrDefault<InputOutputRelation>(new Func<InputOutputRelation, bool>(class128.method_0))?.OutputObject as ITestStep);
            return;
          case 'I':
            if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ShowInTestPlanView"))
              return;
            this.testPlanGrid_0.LoadMonitorVariable(context.Get<IMemberAnnotation>().Member);
            return;
          case 'v':
            if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.RemoveFromPlanView"))
              return;
            this.testPlanGrid_0.RemoveMonitorVariable(context.Get<IMemberAnnotation>().Member);
            return;
          default:
            return;
        }
      case 52:
        switch (commandName[31 /*0x1F*/])
        {
          case 'E':
            if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ExportParameterValues"))
              return;
            TestPlanSettingsControl.OnExport(this.Plan);
            return;
          case 'I':
            if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ImportParameterValues"))
              return;
            TestPlanSettingsControl.OnImport(this.Plan);
            return;
          default:
            return;
        }
    }
  }

  public void CommandHandled(string commandName, AnnotationCollection context)
  {
    switch (commandName)
    {
      case "OpenTap.IconNames.ParameterizeOnTestPlan":
      case "OpenTap.IconNames.Parameterize":
      case "Keysight.OpenTap.Wpf.IconNames.ShowParameter":
        if (!(context.Source is ITestStepParent source))
          break;
        IMemberData member = context.Get<IMemberAnnotation>()?.Member;
        if (member == null)
          break;
        (ITestStepParent target, IParameterMemberData parameter) parameter = MainWindow.findParameter(source, member);
        if (parameter.target != null)
        {
          this.Executed_ShowParameters((object) parameter.target, (IMemberData) parameter.parameter);
          break;
        }
        if (!(commandName == "Keysight.OpenTap.Wpf.IconNames.ShowParameter"))
          break;
        MainWindow.traceSource_0.Error("Unable to find parameter");
        break;
      case "OpenTap.IconNames.RemoveParameter":
      case "OpenTap.IconNames.Unparameterize":
      case "OpenTap.IconNames.EditParameter":
        this.propGrid_0.Reload();
        break;
    }
  }

  public bool FocusModeEnabled => ComponentSettings<EditorSettings>.Current.FocusMode;

  private void method_94(object sender, ExecutedRoutedEventArgs e)
  {
    if (!ComponentSettings<EditorSettings>.Current.FocusMode && !ComponentSettings<EditorSettings>.Current.FocusModeDontAskMeAgain)
    {
      MainWindow.EnterFocusModeQuestion dataObject = new MainWindow.EnterFocusModeQuestion();
      UserInput.Request((object) dataObject);
      if (dataObject.YesCancel == MainWindow.EnterFocusModeQuestion.Question.Cancel)
        return;
      if (dataObject.DontAskAgain)
        ComponentSettings<EditorSettings>.Current.FocusModeDontAskMeAgain = true;
    }
    ComponentSettings<EditorSettings>.Current.FocusMode = !ComponentSettings<EditorSettings>.Current.FocusMode;
    this.method_0("FocusModeEnabled");
    this.method_89();
    ComponentSettings<EditorSettings>.Current.Save();
    CommandManager.InvalidateRequerySuggested();
  }

  private void method_95(object sender, CanExecuteRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    PackageDef packageDef = new Installation(Class11.smethod_2()).GetPackages().FirstOrDefault<PackageDef>(new Func<PackageDef, bool>(new MainWindow.Class130()
    {
      string_0 = "Developer's System"
    }.method_0));
    e.CanExecute = packageDef != null;
  }

  private void method_96(object sender, ExecutedRoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    PackageManagerToolProvider.smethod_0(new Installation(Class11.smethod_2()).GetPackages().FirstOrDefault<PackageDef>(new Func<PackageDef, bool>(new MainWindow.Class131()
    {
      string_0 = "Developer's System"
    }.method_0)).Name);
  }

  private void method_97(object sender, ExecutedRoutedEventArgs e) => Class80.smethod_2();

  private void method_98(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = Class80.smethod_3();
  }

  private void method_99(object sender, ExecutedRoutedEventArgs e)
  {
    this.presetsMenuItem_0.RaiseEvent((RoutedEventArgs) e);
  }

  private void method_100(object sender, EventArgs e) => this.tapDock_0.SaveLayout();

  private void method_101(object sender, EventArgs e)
  {
    this.method_0("FocusModeEnabled");
    this.method_88(true);
    this.method_0("FocusModeEnabled");
    this.method_89();
    this.Focus();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_4)
      return;
    this.bool_4 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/mainwindow.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate method_102(Type type_0, string string_10)
  {
    return Delegate.CreateDelegate(type_0, (object) this, string_10);
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.mainWindow_0 = (MainWindow) target;
        this.mainWindow_0.Closing += new CancelEventHandler(this.mainWindow_0_Closing);
        this.mainWindow_0.MouseDown += new MouseButtonEventHandler(this.logPanel_0_MouseDown);
        this.mainWindow_0.Drop += new DragEventHandler(this.mainWindow_0_Drop);
        this.mainWindow_0.DragOver += new DragEventHandler(this.mainWindow_0_DragOver);
        this.mainWindow_0.DragEnter += new DragEventHandler(this.mainWindow_0_DragEnter);
        this.mainWindow_0.MouseMove += new MouseEventHandler(this.mainWindow_0_MouseMove);
        this.mainWindow_0.KeyDown += new KeyEventHandler(this.mainWindow_0_KeyDown);
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_72);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_59);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_73);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_60);
        break;
      case 4:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_75);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_71);
        break;
      case 5:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_76);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_71);
        break;
      case 6:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_69);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_77);
        break;
      case 7:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_70);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_78);
        break;
      case 8:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_69);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_77);
        break;
      case 9:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_70);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_78);
        break;
      case 10:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_79);
        break;
      case 11:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_41);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_42);
        break;
      case 12:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_67);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_80);
        break;
      case 13:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_40);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_62);
        break;
      case 14:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_63);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_35);
        break;
      case 15:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_37);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_36);
        break;
      case 16 /*0x10*/:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_66);
        break;
      case 17:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_64);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_32);
        break;
      case 18:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_34);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_65);
        break;
      case 19:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_45);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_44);
        break;
      case 20:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.MonitorPropertyExecuted);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.CanExecuteMonitorProperty);
        break;
      case 21:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.RemoveMonitorPropertyExecuted);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.RemoveCanExecuteMonitorProperty);
        break;
      case 22:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_74);
        break;
      case 23:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.Executed_ShowTestPlanSettings);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_42);
        break;
      case 24:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_46);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_47);
        break;
      case 25:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_94);
        break;
      case 26:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_99);
        break;
      case 27:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_99);
        break;
      case 28:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_99);
        break;
      case 29:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_99);
        break;
      case 30:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_99);
        break;
      case 31 /*0x1F*/:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_96);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_95);
        break;
      case 32 /*0x20*/:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_97);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_98);
        break;
      case 34:
        this.grid_0 = (Grid) target;
        break;
      case 35:
        this.rowDefinition_0 = (RowDefinition) target;
        break;
      case 36:
        this.grid_1 = (Grid) target;
        break;
      case 37:
        this.menu_0 = (Menu) target;
        break;
      case 38:
        this.menuItem_0 = (MenuItem) target;
        break;
      case 39:
        this.separator_0 = (Separator) target;
        break;
      case 40:
        this.menuItem_1 = (MenuItem) target;
        break;
      case 41:
        this.menuItem_2 = (MenuItem) target;
        break;
      case 42:
        ((MenuItem) target).Click += new RoutedEventHandler(this.method_9);
        break;
      case 43:
        ((MenuItem) target).Click += new RoutedEventHandler(this.method_9);
        break;
      case 44:
        this.separator_1 = (Separator) target;
        break;
      case 45:
        this.separator_2 = (Separator) target;
        break;
      case 46:
        this.menuItem_3 = (MenuItem) target;
        break;
      case 47:
        this.menuItem_4 = (MenuItem) target;
        break;
      case 48 /*0x30*/:
        this.menuItem_5 = (MenuItem) target;
        break;
      case 49:
        this.presetsMenuItem_0 = (PresetsMenuItem) target;
        break;
      case 50:
        this.menuItem_6 = (MenuItem) target;
        this.menuItem_6.Click += new RoutedEventHandler(this.menuItem_8_Click);
        break;
      case 51:
        this.menuItem_7 = (MenuItem) target;
        this.menuItem_7.Click += new RoutedEventHandler(this.menuItem_8_Click);
        break;
      case 52:
        this.menuItem_8 = (MenuItem) target;
        this.menuItem_8.Click += new RoutedEventHandler(this.menuItem_8_Click);
        break;
      case 53:
        this.menuItem_9 = (MenuItem) target;
        this.menuItem_9.Click += new RoutedEventHandler(this.menuItem_9_Click);
        break;
      case 54:
        this.menuItem_10 = (MenuItem) target;
        this.menuItem_10.Click += new RoutedEventHandler(this.menuItem_10_Click);
        break;
      case 55:
        this.stackPanel_0 = (StackPanel) target;
        break;
      case 56:
        this.textBox_0 = (TextBox) target;
        break;
      case 58:
        this.grid_2 = (Grid) target;
        break;
      case 59:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_68);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_43);
        break;
      case 60:
        this.testPlanGrid_0 = (TestPlanGrid) target;
        break;
      case 61:
        this.textBlock_0 = (TextBlock) target;
        break;
      case 62:
        this.textBlock_1 = (TextBlock) target;
        break;
      case 63 /*0x3F*/:
        this.welcomeScreen_0 = (WelcomeScreen) target;
        break;
      case 64 /*0x40*/:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.method_83);
        break;
      case 65:
        this.button_0 = (Button) target;
        break;
      case 66:
        this.controlDropDown_0 = (ControlDropDown) target;
        break;
      case 67:
        this.toggleButton_0 = (ToggleButton) target;
        break;
      case 68:
        ((UIElement) target).KeyDown += new KeyEventHandler(this.method_81);
        break;
      case 69:
        this.control_0 = (Control) target;
        break;
      case 70:
        this.decorator_0 = (Decorator) target;
        break;
      case 71:
        this.grid_3 = (Grid) target;
        break;
      case 72:
        this.grid_4 = (Grid) target;
        break;
      case 73:
        this.propGrid_0 = (PropGrid) target;
        this.propGrid_0.PropertyEdited += (EventHandler<EventArgs>) new EventHandler<EventArgs>(this.SettingsPanel_PropertyEdited);
        this.propGrid_0.TestPlanModified += (EventHandler) new EventHandler(this.propGrid_0_TestPlanModified);
        this.propGrid_0.MouseDown += new MouseButtonEventHandler(this.logPanel_0_MouseDown);
        break;
      case 74:
        this.textBlock_2 = (TextBlock) target;
        break;
      case 75:
        this.grid_5 = (Grid) target;
        break;
      case 76:
        this.logPanel_0 = (LogPanel) target;
        this.logPanel_0.Loaded += new RoutedEventHandler(this.logPanel_0_Loaded);
        this.logPanel_0.MouseDown += new MouseButtonEventHandler(this.logPanel_0_MouseDown);
        break;
      case 77:
        this.runCompletedOverlay_0 = (RunCompletedOverlay) target;
        break;
      case 78:
        this.tapDock_0 = (TapDock) target;
        break;
      case 79:
        this.instrumentStatus_0 = (InstrumentStatus) target;
        break;
      default:
        this.bool_4 = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 33)
    {
      if (connectionId != 57)
        return;
      ((UIElement) target).MouseDown += new MouseButtonEventHandler(this.menuItem_10_Click);
    }
    else
      ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.method_90);
  }

  [CompilerGenerated]
  internal static bool smethod_3(string string_10, out DateTime dateTime_0)
  {
    return DateTime.TryParseExact(string_10, "YYYY.MMDD", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime_0);
  }

  [CompilerGenerated]
  internal static void smethod_4()
  {
    TapThread.Sleep(1000);
    Keysight.OpenTap.Licensing.LicenseManager.WaitUntilIdle();
    LicenseData[] availableLicenses = Keysight.OpenTap.Licensing.LicenseManager.GetAvailableLicenses();
    List<LicenseData> licenseDataList = new List<LicenseData>();
    LicenseData[] array = ((IEnumerable<LicenseData>) availableLicenses).Where<LicenseData>((Func<LicenseData, bool>) (licenseData_0 => licenseData_0.Type.HasFlag((System.Enum) LicenseType.KAL))).Where<LicenseData>((Func<LicenseData, bool>) (licenseData_0 =>
    {
      DateTime? expirationDate = licenseData_0.ExpirationDate;
      DateTime now = DateTime.Now;
      return expirationDate.HasValue && expirationDate.GetValueOrDefault() > now;
    })).ToArray<LicenseData>();
    if (((IEnumerable<LicenseData>) array).Any<LicenseData>())
    {
      DateTime dateTime_0;
      licenseDataList.Add(GuiHelpers.FindMax<LicenseData, DateTime>((IEnumerable<LicenseData>) array, (Func<LicenseData, DateTime>) (licenseData_0 => !MainWindow.smethod_3(licenseData_0.Version, out dateTime_0) ? DateTime.MinValue : dateTime_0)));
    }
    foreach (LicenseData licenseData in licenseDataList)
    {
      DateTime dateTime_0;
      if (MainWindow.smethod_3(licenseData.Version, out dateTime_0))
      {
        TimeSpan timeSpan = dateTime_0 - DateTime.Now;
        if (timeSpan.TotalDays > 0.0 && timeSpan.TotalDays < (double) ComponentSettings<EditorSettings>.Current.SupportSubscriptionCheckDays)
          MainWindow.traceSource_0.Info($"Support subscription for '{licenseData.Name}' expires on {dateTime_0.ToLongDateString()}. (in {(int) timeSpan.TotalDays} days)");
        else if (timeSpan.TotalDays < 0.0)
        {
          DateTime? expirationDate = licenseData.ExpirationDate;
          DateTime now = DateTime.Now;
          if ((expirationDate.HasValue ? (expirationDate.GetValueOrDefault() > now ? 1 : 0) : 0) != 0)
            MainWindow.traceSource_0.Info($"Support subscription for '{licenseData.Name}' has expired.");
        }
      }
    }
  }

  [CompilerGenerated]
  internal static IEnumerable<IResultListener> smethod_5()
  {
    return ComponentSettings<ResultSettings>.Current.Where<IResultListener>((Func<IResultListener, bool>) (iresultListener_0 => !(iresultListener_0 is IEnabledResource enabledResource) || enabledResource.IsEnabled));
  }

  internal class Class83
  {
    public string String_0 { get; set; }
  }

  public class RecoveryFile
  {
    public string FilePath { get; set; }

    public TestPlan TestPlan { get; set; }

    public void Save(Stream stream) => new TapSerializer().Serialize(stream, (object) this);

    public static MainWindow.RecoveryFile Load(Stream stream)
    {
      MainWindow.RecoveryFile recoveryFile = new MainWindow.RecoveryFile();
      XDocument xdocument = XDocument.Load(stream);
      XElement xelement = xdocument.Element((XName) nameof (RecoveryFile)).Element((XName) "FilePath");
      recoveryFile.FilePath = xelement.Value;
      using (Stream stream1 = (Stream) new MemoryStream())
      {
        xdocument.Element((XName) nameof (RecoveryFile)).Element((XName) "TestPlan").Save(stream1);
        stream1.Seek(0L, SeekOrigin.Begin);
        recoveryFile.TestPlan = TestPlan.Load(stream1, recoveryFile.FilePath);
      }
      return recoveryFile;
    }
  }

  private class Class84 : ResultListener
  {
    public bool bool_0;

    public override void OnTestPlanRunStart(TestPlanRun planRun) => this.bool_0 = true;

    public Class84() => OpenTap.Log.RemoveSource(this.Log);
  }

  private class Class85
  {
    [CompilerGenerated]
    [SpecialName]
    public Guid[] method_0() => this.guid_0;

    [CompilerGenerated]
    [SpecialName]
    public byte[] method_2() => this.byte_0;

    public Class85(TestPlan testPlan_0, Guid[] guid_1)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MainWindow.Class85.Class86 class86 = new MainWindow.Class85.Class86();
      // ISSUE: reference to a compiler-generated field
      class86.testPlan_0 = testPlan_0;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      // ISSUE: reference to a compiler-generated field
      class86.class85_0 = this;
      // ISSUE: reference to a compiler-generated method
      this.method_1(guid_1);
      // ISSUE: reference to a compiler-generated field
      if (class86.testPlan_0 == null)
        return;
      try
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          // ISSUE: reference to a compiler-generated field
          class86.testPlan_0.Save((Stream) memoryStream);
          // ISSUE: reference to a compiler-generated method
          this.method_3(memoryStream.ToArray());
        }
      }
      catch
      {
        // ISSUE: reference to a compiler-generated method
        GuiHelpers.GuiInvoke((Action) new Action(class86.method_0));
      }
    }

    public override bool Equals(object object_0)
    {
      if (!(object_0 is MainWindow.Class85))
        return false;
      MainWindow.Class85 class85 = object_0 as MainWindow.Class85;
      return this.method_2().Length == class85.method_2().Length && ((IEnumerable<byte>) this.method_2()).SequenceEqual<byte>((IEnumerable<byte>) class85.method_2());
    }

    public override int GetHashCode()
    {
      return this.method_0().GetHashCode() ^ this.method_2().GetHashCode();
    }
  }

  [Display("Enter Focus Mode?", null, null, -10000.0, false, null)]
  [HelpLink("EditorHelp.chm::/Editor Overview/Focus Mode.html")]
  private class EnterFocusModeQuestion
  {
    [Browsable(true)]
    [Layout(LayoutMode.FullRow, 3, 1000)]
    public string Message
    {
      get
      {
        return "Entering focus mode. All floating panels will be closed.\nTo leave focus mode press Alt+Shift+Enter.\n\nAre you sure you want to continue?";
      }
    }

    [Display("Don't ask me again", null, null, -10000.0, false, null)]
    [Layout(LayoutMode.FloatBottom, 1, 1000)]
    public bool DontAskAgain { get; set; }

    [Submit]
    [Layout(LayoutMode.FullRow | LayoutMode.FloatBottom, 1, 1000)]
    public MainWindow.EnterFocusModeQuestion.Question YesCancel { get; set; } = MainWindow.EnterFocusModeQuestion.Question.Cancel;

    public enum Question
    {
      [Display("Continue", "Continue into focus mode.", null, -10000.0, false, null)] Continue,
      [Display("Cancel", "Don't enter focus mode.", null, -10000.0, false, null)] Cancel,
    }
  }
}
