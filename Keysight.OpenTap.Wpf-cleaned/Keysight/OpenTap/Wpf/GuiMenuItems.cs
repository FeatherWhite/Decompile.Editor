// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GuiMenuItems
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class GuiMenuItems : IMenuModel
{
  private readonly AnnotationCollection annotation;
  private readonly UpdateMonitor updateMonitor_0;

  object[] IMenuModel.Source { get; set; }

  public GuiMenuItems(AnnotationCollection annotation, UpdateMonitor update)
  {
    this.annotation = annotation;
    this.updateMonitor_0 = update ?? new UpdateMonitor();
  }

  public bool CanShowParameter
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowParameter", this.annotation);
    }
  }

  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ShowParameter")]
  [Display("Show Parameter", "Show the parameter controlling this setting.", null, -10000.0, false, null)]
  [EnabledIf("CanShowParameter", new object[] {true}, HideIfDisabled = true)]
  [Browsable(true)]
  public void ShowParameter()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowParameter", this.annotation);
  }

  public bool CanShowInTestPlan
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowInTestPlanView", this.annotation);
    }
  }

  [Browsable(true)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ShowInTestPlanView")]
  [EnabledIf("CanShowInTestPlan", new object[] {true}, HideIfDisabled = true)]
  [Display("Show In Test Plan View", "Show this setting in the test plan view.", null, -4.0, false, null)]
  public void ShowInTestPlan()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowInTestPlanView", this.annotation);
  }

  public bool CanRemoveFromTestPlan
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.RemoveFromPlanView", this.annotation);
    }
  }

  [Browsable(true)]
  [Display("Remove from Test Plan View", "Remove this setting from the test plan view.", null, -4.0, false, null)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.RemoveFromPlanView")]
  [EnabledIf("CanRemoveFromTestPlan", new object[] {true}, HideIfDisabled = true)]
  public void RemoveFromPlanView()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.RemoveFromPlanView", this.annotation);
  }

  public bool CanShowAssignedOutput
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput", this.annotation);
    }
  }

  [Display("Show Assigned Output", "This setting is controlled by an output and cannot be configured directly.", null, -10000.0, false, null)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput")]
  [EnabledIf("CanShowAssignedOutput", new object[] {true}, HideIfDisabled = true)]
  [Browsable(true)]
  public void ShowAssignedOutput()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput", this.annotation);
  }

  public bool CanShowAssignedInput
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput", this.annotation);
    }
  }

  [Display("Show Assigned Input", "This output is controlling one or more inputs.", null, -10000.0, false, null)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput")]
  [EnabledIf("CanShowAssignedInput", new object[] {true}, HideIfDisabled = true)]
  [Browsable(true)]
  public void ShowAssignedInput()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput", this.annotation);
  }

  public bool CanExportParameterValues
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ExportParameterValues", this.annotation);
    }
  }

  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ExportParameterValues")]
  [Browsable(true)]
  [Display("Export Parameters Values", "Export Parameter Values to a File.", null, -10000.0, false, null)]
  [EnabledIf("CanExportParameterValues", new object[] {true}, HideIfDisabled = true)]
  public void ExportParametersValues()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ExportParameterValues", this.annotation);
  }

  public bool CanImportParameterValues
  {
    get
    {
      return this.updateMonitor_0.CanHandleCommand("Keysight.OpenTap.Wpf.IconNames.ImportParameterValues", this.annotation);
    }
  }

  [EnabledIf("CanImportParameterValues", new object[] {true}, HideIfDisabled = true)]
  [Display("Import Parameters Values", "Import Parameter Values to a File.", null, -10000.0, false, null)]
  [Browsable(true)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.ImportParameterValues")]
  public void ImportParametersValues()
  {
    this.updateMonitor_0.HandleCommand("Keysight.OpenTap.Wpf.IconNames.ImportParameterValues", this.annotation);
  }

  public bool CanCopyValue
  {
    get
    {
      return !string.IsNullOrEmpty(this.annotation.Get<IStringReadOnlyValueAnnotation>(false, (object) null)?.Value);
    }
  }

  [Display("Copy Value", "Copy this setting's value to the clipboard.", null, -5.0, false, null)]
  [EnabledIf("CanCopyValue", new object[] {true}, HideIfDisabled = true)]
  [OverrideReload]
  [Browsable(true)]
  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.CopyValue")]
  public void CopyValue()
  {
    string data = this.annotation.Get<IStringReadOnlyValueAnnotation>(false, (object) null)?.Value;
    if (data == null)
      return;
    try
    {
      Clipboard.SetDataObject((object) data);
    }
    catch
    {
    }
  }

  public bool CanPasteValue
  {
    get
    {
      try
      {
        int num;
        if (this.annotation.Get<IStringValueAnnotation>(false, (object) null) != null)
        {
          IEnabledAnnotation ienabledAnnotation = this.annotation.Get<IEnabledAnnotation>(false, (object) null);
          if ((ienabledAnnotation != null ? (ienabledAnnotation.IsEnabled ? 1 : 0) : 1) != 0 && this.annotation.Get<IMemberAnnotation>(false, (object) null)?.Member?.Writable.GetValueOrDefault())
          {
            IAccessAnnotation iaccessAnnotation = this.annotation.Get<IAccessAnnotation>(false, (object) null);
            if ((iaccessAnnotation != null ? (!iaccessAnnotation.IsReadOnly ? 1 : 0) : 1) != 0)
            {
              num = !this.method_0(this.annotation.Source) ? 1 : 0;
              goto label_5;
            }
          }
        }
        num = 0;
label_5:
        return num != 0;
      }
      catch
      {
        return false;
      }
    }
  }

  private bool method_0(object object_1)
  {
    if (object_1 is ITestStepParent itestStepParent)
    {
      if (object_1 is TestStep testStep && testStep.IsReadOnly)
        return true;
      while (itestStepParent.Parent != null)
        itestStepParent = itestStepParent.Parent;
      if (itestStepParent is TestPlan testPlan)
        return testPlan.IsRunning || testPlan.Locked;
    }
    return false;
  }

  [IconAnnotation("Keysight.OpenTap.Wpf.IconNames.PasteValue")]
  [Browsable(true)]
  [EnabledIf("CanPasteValue", new object[] {true}, HideIfDisabled = true)]
  [Display("Paste Value", "Paste a value from the clipboard to this setting.", null, -5.0, false, null)]
  public void PasteValue()
  {
    try
    {
      string text = Clipboard.GetText();
      if (string.IsNullOrEmpty(text))
        return;
      this.annotation.Get<IStringValueAnnotation>(false, (object) null).Value = text;
      this.annotation.Write();
    }
    catch (Exception ex)
    {
      Log.Debug(Log.CreateSource("Test"), ex);
    }
  }
}
