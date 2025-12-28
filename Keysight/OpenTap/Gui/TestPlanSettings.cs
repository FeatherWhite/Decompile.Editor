// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestPlanSettings
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("Test Plan Settings", "Show the settings of the test plan.", null, -10000.0, false, null)]
[HelpLink("EditorHelp.chm::/Editor Overview/Test Plan Settings Panel.html")]
public class TestPlanSettings : ITapDockPanel, ITapPlugin
{
  public FrameworkElement CreateElement(ITapDockContext context)
  {
    return (FrameworkElement) new TestPlanSettingsControl(context);
  }

  public double? DesiredWidth => new double?();

  public double? DesiredHeight => new double?();
}
