// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.PanelSettings
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Browsable(false)]
[Display("GUI Panels", null, null, -10000.0, false, null)]
public class PanelSettings : ComponentSettings<PanelSettings>
{
  [ViewPreset.Member]
  public string Layout { get; set; }

  [Obsolete("Use Layout instead")]
  public string LayoutXml { get; set; }

  [Obsolete("This has been replaced by improvements to Layout")]
  public string[] VisiblePanels { get; set; }
}
