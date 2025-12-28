// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NoLicenseDlg2
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.OpenTap.Licensing;
using Microsoft.Win32;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NoLicenseDlg2 : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ExceptionProperty = DependencyProperty.Register(nameof (Exception), typeof (LicenseException), typeof (NoLicenseDlg2));
  private static readonly TraceSource traceSource_0 = Log.CreateSource("License");
  internal Grid baseGrid;
  internal Button InstallPWLMButton;
  internal Button OpenPWLMButton;
  internal Button RequestTrialLicensBtn;
  private bool bool_0;

  public LicenseException Exception
  {
    get => (LicenseException) this.GetValue(NoLicenseDlg2.ExceptionProperty);
    set => this.SetValue(NoLicenseDlg2.ExceptionProperty, (object) value);
  }

  internal NoLicenseDlgViewModel ViewModel { get; set; }

  public NoLicenseDlg2()
  {
    this.ViewModel = new NoLicenseDlgViewModel();
    this.InitializeComponent();
    this.baseGrid.DataContext = (object) this.ViewModel;
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (dependencyPropertyChangedEventArgs_0.Property == NoLicenseDlg2.ExceptionProperty)
      this.ViewModel.Exception = (LicenseException) dependencyPropertyChangedEventArgs_0.NewValue;
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
  }

  private void RequestTrialLicensBtn_Click(object sender, RoutedEventArgs e)
  {
    if (this.Exception.Feature.Contains("Test Automation Editor"))
      Process.Start("https://ksm.software.keysight.com/ASM/External/TrialLicense.aspx?ProdNum=KS8400b-TRL");
    else
      Process.Start("https://edadocs.software.keysight.com/kkbopen/where-can-i-find-free-trials-589310263.html?jmpid=zzfindfree_trials");
  }

  private void InstallPWLMButton_Click(object sender, RoutedEventArgs e)
  {
    string str = Path.Combine(ExecutorClient.ExeDir, "PackageManager.exe");
    if (!File.Exists(str))
    {
      Log.Error(NoLicenseDlg2.traceSource_0, "Package Manager not found. You likely have a broken install.", Array.Empty<object>());
      throw new System.Exception();
    }
    try
    {
      Process.Start(str, "--install \"PathWave License Manager\"");
    }
    catch (System.Exception ex)
    {
      Log.Error(NoLicenseDlg2.traceSource_0, "Failed installing PathWave License Manager.", Array.Empty<object>());
      Log.Debug(NoLicenseDlg2.traceSource_0, ex);
    }
  }

  private string method_0()
  {
    string path1 = "C:\\Program Files\\Common Files\\Keysight\\PathWave License Manager\\pwlmgr.exe";
    if (File.Exists(path1))
      return path1;
    string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
    using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey(name))
    {
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName))
        {
          if ("Keysight PathWave License Manager".Equals(registryKey2.GetValue("DisplayName") as string, StringComparison.Ordinal))
          {
            string path2 = Path.Combine(registryKey2.GetValue("InstallLocation") as string, "pwlmgr.exe");
            if (File.Exists(path2))
              return path2;
          }
        }
      }
    }
    throw new System.Exception("PathWave License Manager not found.");
  }

  private void OpenPWLMButton_Click(object sender, RoutedEventArgs e)
  {
    if (!this.ViewModel.PWLMInstalled)
      return;
    try
    {
      Process.Start(this.method_0());
    }
    catch (System.Exception ex)
    {
      Log.Error(NoLicenseDlg2.traceSource_0, "PathWave License Manager executable not found.", Array.Empty<object>());
      Log.Debug(NoLicenseDlg2.traceSource_0, ex);
    }
  }

  public static void Show(LicenseException exception, bool exit = true)
  {
    WslDialog wslDialog = new WslDialog();
    wslDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    wslDialog.Width = 500.0;
    wslDialog.Height = 500.0;
    wslDialog.Title = "No license found for " + exception.Feature;
    wslDialog.Content = (object) new NoLicenseDlg2()
    {
      Exception = exception
    };
    wslDialog.ShowDialog();
    if (!exit)
      return;
    Environment.Exit(1);
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/nolicensedlg2.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.baseGrid = (Grid) target;
        break;
      case 2:
        this.InstallPWLMButton = (Button) target;
        this.InstallPWLMButton.Click += new RoutedEventHandler(this.InstallPWLMButton_Click);
        break;
      case 3:
        this.OpenPWLMButton = (Button) target;
        this.OpenPWLMButton.Click += new RoutedEventHandler(this.OpenPWLMButton_Click);
        break;
      case 4:
        this.RequestTrialLicensBtn = (Button) target;
        this.RequestTrialLicensBtn.Click += new RoutedEventHandler(this.RequestTrialLicensBtn_Click);
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
