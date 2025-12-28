// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.PlanLoadedWithErrorsQuestion
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("Plan Loaded With Errors.", null, null, -10000.0, false, null)]
internal class PlanLoadedWithErrorsQuestion
{
  [Layout(LayoutMode.FullRow | LayoutMode.FloatBottom, 1, 1000)]
  [Submit]
  public PlanLoadedWithErrorsQuestion.ResponseType Response { get; set; } = PlanLoadedWithErrorsQuestion.ResponseType.Continue;

  [Browsable(true)]
  [Display("Message", null, null, 0.0, false, null)]
  [Layout(LayoutMode.FullRow, 2, 1000)]
  public string Message
  {
    get
    {
      return "Plan was loaded with errors, continuing might cause loss of data.\nPlease refer to the log for information.";
    }
  }

  public enum ResponseType
  {
    [Display("Cancel", "Cancel loading the test plan, returning to what was before.", null, 2.0, false, null)] Cancel,
    [Display("Save As", "Save the loaded test plan in a new file.", null, 1.0, false, null)] SaveAs,
    [Display("Continue", "Keep the test plan as loaded.", null, 0.0, false, null)] Continue,
  }
}
