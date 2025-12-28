// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NoLicenseDlgViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using Keysight.OpenTap.Licensing;
using OpenTap.Package;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class NoLicenseDlgViewModel : INotifyPropertyChanged
{
  private LicenseException licenseException_0 = new LicenseException("", "", "1.0");
  private static PackageDef packageDef_0;

  public LicenseException Exception
  {
    get => this.licenseException_0;
    set
    {
      if (this.licenseException_0 == value)
        return;
      this.licenseException_0 = value;
      this.OnPropertyChanged(nameof (Exception));
    }
  }

  public bool PwlmCandidate => IntPtr.Size == 8;

  public bool PWLMInstalled => this.PwlmCandidate && NoLicenseDlgViewModel.packageDef_0 != null;

  public bool SuggestPlwmInstall
  {
    get => this.PwlmCandidate && NoLicenseDlgViewModel.packageDef_0 == null;
  }

  public bool NotPwlmCandidate => !this.PwlmCandidate;

  public string HostID { get; private set; }

  public string ExceptionString
  {
    get => $"{this.Exception.Feature}: {this.Exception.License} v{this.Exception.MinVersion}";
  }

  private void method_0()
  {
    Installation.Current.PackageChangedEvent += (Action) (() =>
    {
      PackageDef pwlmPkg = Installation.Current.GetPackages().FirstOrDefault<PackageDef>((Func<PackageDef, bool>) (packageDef_0 => string.Equals(((PackageIdentifier) packageDef_0).Name, "PathWave License Manager", StringComparison.Ordinal)));
      if (pwlmPkg != null)
      {
        GuiHelpers.GuiInvoke((Action) (() => NoLicenseDlgViewModel.packageDef_0 = pwlmPkg));
        Installation.Current.PackageChangedEvent -= (Action) (() =>
        {
          // ISSUE: unable to decompile the method.
        });
      }
      this.OnPropertyChanged("PWLMInstalled");
    });
    // ISSUE: reference to a compiler-generated method
    this.method_1();
  }

  public NoLicenseDlgViewModel() => this.method_0();

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    // ISSUE: reference to a compiler-generated field
    PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
