// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LicenseControlViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using Keysight.OpenTap.Licensing;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class LicenseControlViewModel : INotifyPropertyChanged
{
  private LicenseViewModel[] licenseViewModel_0 = Array.Empty<LicenseViewModel>();

  public LicenseViewModel[] Licenses
  {
    get => this.licenseViewModel_0;
    set
    {
      if (value == this.licenseViewModel_0)
        return;
      this.licenseViewModel_0 = value;
      this.OnPropertyChanged(nameof (Licenses));
    }
  }

  public LicenseControlViewModel() => TapThread.Start((Action) (() => this.UpdateLicenses()), "");

  public void UpdateLicenses()
  {
    TapThread.Sleep(500);
    LicenseManager.WaitUntilIdle();
    List<LicenseViewModel> licensesVm = new List<LicenseViewModel>();
    foreach (LicenseData licenseData_0 in ((IEnumerable<LicenseData>) LicenseManager.GetAvailableLicenses()).ToArray<LicenseData>())
      licensesVm.Add(new LicenseViewModel(licenseData_0));
    foreach (IReflectionData allPlugin in PluginManager.GetAllPlugins())
      ReflectionDataExtensions.GetAttribute<LicenseAttribute>(allPlugin);
    GuiHelpers.GuiInvoke((Action) (() => this.Licenses = licensesVm.ToArray()));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    GuiHelpers.GuiInvoke((Action) (() =>
    {
      // ISSUE: reference to a compiler-generated field
      PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
      if (changedEventHandler0 == null)
        return;
      changedEventHandler0((object) this, new PropertyChangedEventArgs(propertyName));
    }));
  }
}
