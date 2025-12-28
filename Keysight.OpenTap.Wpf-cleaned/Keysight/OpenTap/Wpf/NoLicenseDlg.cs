// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NoLicenseDlg
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NoLicenseDlg : WslDialog, IComponentConnector
{
  internal static readonly TraceSource _log = Log.CreateSource("License");
  private DispatcherTimer dispatcherTimer_0;
  private DateTime dateTime_0;
  private int int_0 = 60;
  internal Hyperlink LicenseManagerHyperlink;
  internal Button RequestTrialLicensBtn;
  internal Button InstallLicenseFileBtn;
  internal Button ActivateCertificateBtn;
  internal Button setupNetworkLicense;
  internal TextBlock CountDownTxtBl;
  internal Button CloseBtn;
  private bool bool_2;

  private string KlaFilePath { get; set; }

  private string KlmFilePath { get; set; }

  public bool KlFound { get; private set; }

  public bool KlNotFound { get; private set; }

  public string HostID { get; private set; }

  public string PackageName { get; set; }

  public NoLicenseDlg(string packageName)
    : this(packageName, true)
  {
  }

  private static string smethod_0() => "";

  public NoLicenseDlg(string packageName, bool enableCountdown)
  {
    this.InitializeComponent();
    this.method_0();
    this.KlFound = File.Exists(this.KlaFilePath) && File.Exists(this.KlmFilePath);
    this.KlNotFound = !this.KlFound;
    this.dateTime_0 = DateTime.Now.AddSeconds((double) this.int_0);
    this.dispatcherTimer_0 = new DispatcherTimer()
    {
      Interval = new TimeSpan(0, 0, 1)
    };
    this.dispatcherTimer_0.Tick += new EventHandler(this.dispatcherTimer_0_Tick);
    if (enableCountdown)
      this.dispatcherTimer_0.Start();
    try
    {
      this.HostID = NoLicenseDlg.smethod_0();
    }
    catch
    {
    }
    this.PackageName = packageName;
    this.Title = this.PackageName + " License Not Found";
    this.DataContext = (object) this;
  }

  private void method_0()
  {
    string path1 = Path.Combine(NoLicenseDlg.smethod_1(), "Agilent\\Agilent License Manager");
    this.KlaFilePath = Path.Combine(path1, "KeysightLicenseActivator.exe");
    this.KlmFilePath = Path.Combine(path1, "KeysightLicenseManager.exe");
  }

  private void dispatcherTimer_0_Tick(object sender, EventArgs e)
  {
    if (DateTime.Now > this.dateTime_0)
    {
      this.dispatcherTimer_0.Stop();
      this.Close();
    }
    else
      this.CountDownTxtBl.Text = $"Application will exit in {(int) (this.dateTime_0 - DateTime.Now).TotalSeconds} seconds or upon closing this dialog.";
  }

  private static string smethod_1()
  {
    return 8 != IntPtr.Size && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")) ? Environment.GetEnvironmentVariable("ProgramFiles") : Environment.GetEnvironmentVariable("ProgramFiles(x86)");
  }

  private void RequestTrialLicensBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start(this.KlaFilePath, "/Trial /Id 224");
    }
    catch (Exception ex)
    {
      Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
      if (!(declaringType != (Type) null))
        return;
      string str = $"{declaringType.Name}|{MethodBase.GetCurrentMethod().Name}|{$"Failed to start:{this.KlaFilePath}\\n" + ex.Message}";
      Log.Error(NoLicenseDlg._log, str, new object[1]
      {
        (object) ex
      });
    }
  }

  private void ActivateCertificateBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start(this.KlaFilePath, "/Id 81");
    }
    catch (Exception ex)
    {
      Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
      if (!(declaringType != (Type) null))
        return;
      string str = $"{declaringType.Name}|{MethodBase.GetCurrentMethod().Name}|{$"Failed to start:{this.KlaFilePath}\\n" + ex.Message}";
      Log.Error(NoLicenseDlg._log, str, new object[1]
      {
        (object) ex
      });
    }
  }

  private void InstallLicenseFileBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start(this.KlmFilePath);
    }
    catch (Exception ex)
    {
      Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
      if (!(declaringType != (Type) null))
        return;
      string str = $"{declaringType.Name}|{MethodBase.GetCurrentMethod().Name}|{$"Failed to start:{this.KlmFilePath}\\n" + ex.Message}";
      Log.Error(NoLicenseDlg._log, str, new object[1]
      {
        (object) ex
      });
    }
  }

  private void CloseBtn_Click(object sender, RoutedEventArgs e) => this.Close();

  private void LicenseManagerHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
  {
    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
    e.Handled = true;
  }

  private void method_1(object sender, RoutedEventArgs e)
  {
    ClipboardHelper.CopyText(this.HostID ?? "");
  }

  private void setupNetworkLicense_Click(object sender, RoutedEventArgs e)
  {
    NetworkLicenseSetup networkLicenseSetup = new NetworkLicenseSetup();
    networkLicenseSetup.Owner = (Window) this;
    networkLicenseSetup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    this.dispatcherTimer_0.Stop();
    this.Hide();
    bool? nullable = networkLicenseSetup.ShowDialog();
    if (!(!nullable.GetValueOrDefault() & nullable.HasValue))
    {
      this.Close();
    }
    else
    {
      this.dispatcherTimer_0.Start();
      this.ShowDialog();
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_2)
      return;
    this.bool_2 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/nolicensedlg.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LicenseManagerHyperlink = (Hyperlink) target;
        this.LicenseManagerHyperlink.RequestNavigate += new RequestNavigateEventHandler(this.LicenseManagerHyperlink_RequestNavigate);
        break;
      case 2:
        this.RequestTrialLicensBtn = (Button) target;
        this.RequestTrialLicensBtn.Click += new RoutedEventHandler(this.RequestTrialLicensBtn_Click);
        break;
      case 3:
        this.InstallLicenseFileBtn = (Button) target;
        this.InstallLicenseFileBtn.Click += new RoutedEventHandler(this.InstallLicenseFileBtn_Click);
        break;
      case 4:
        this.ActivateCertificateBtn = (Button) target;
        this.ActivateCertificateBtn.Click += new RoutedEventHandler(this.ActivateCertificateBtn_Click);
        break;
      case 5:
        this.setupNetworkLicense = (Button) target;
        this.setupNetworkLicense.Click += new RoutedEventHandler(this.setupNetworkLicense_Click);
        break;
      case 6:
        ((Hyperlink) target).Click += new RoutedEventHandler(this.method_1);
        break;
      case 7:
        this.CountDownTxtBl = (TextBlock) target;
        break;
      case 8:
        this.CloseBtn = (Button) target;
        this.CloseBtn.Click += new RoutedEventHandler(this.CloseBtn_Click);
        break;
      default:
        this.bool_2 = true;
        break;
    }
  }
}
