// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LicensesPanel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Licenses", "Shows the available Keysight licenses.", null, -10000.0, false, null)]
[Browsable(false)]
internal class LicensesPanel
{
  public FrameworkElement CreateElement(ITapDockContext context)
  {
    return (FrameworkElement) new LicenseControl();
  }

  public double? DesiredWidth { get; }

  public double? DesiredHeight { get; }
}
