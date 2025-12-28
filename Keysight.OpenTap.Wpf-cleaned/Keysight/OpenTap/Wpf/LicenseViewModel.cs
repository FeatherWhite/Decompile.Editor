// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LicenseViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Licensing;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class LicenseViewModel
{
  public string Name { get; }

  public string Version { get; }

  public string Source { get; }

  public string Status { get; }

  public LicenseViewModel(LicenseData licenseData_0)
  {
    this.Name = licenseData_0.Name;
    this.Version = licenseData_0.Version;
    this.Source = licenseData_0.Source;
    this.Status = licenseData_0.IsExpired ? "Expired" : "";
  }
}
