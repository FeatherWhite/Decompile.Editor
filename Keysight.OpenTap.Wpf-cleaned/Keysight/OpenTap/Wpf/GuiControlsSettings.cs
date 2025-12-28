// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GuiControlsSettings
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Browsable(false)]
[Display("GUI Controls", null, null, -10000.0, false, null)]
public class GuiControlsSettings : ComponentSettings<GuiControlsSettings>
{
  private Dictionary<string, string> dictionary_3 = new Dictionary<string, string>();
  [XmlIgnore]
  public Dictionary<string, HashSet<string>> FilterConfigurations = new Dictionary<string, HashSet<string>>();
  private const int int_0 = 500;

  public List<string> ExpandedStepCategories { get; set; }

  public List<string> PropGridCategories { get; set; }

  public List<bool> PropGridCategoryStates { get; set; }

  [ViewPreset.Member]
  public double MainWindowHeight { get; set; }

  [ViewPreset.Member]
  public double MainWindowWidth { get; set; }

  [ViewPreset.Member]
  public double MainWindowTop { get; set; }

  [ViewPreset.Member]
  public double MainWindowLeft { get; set; }

  [ViewPreset.Member]
  public TapWindowState MainWindowState { get; set; }

  public List<string> RecentTestPlans { get; set; }

  [ViewPreset.Member]
  public string TestPlanGridColumnDetails { get; set; }

  public List<object> TestPlanColumnState { get; set; }

  [ViewPreset.Member]
  public List<object> TestPlanColumnState2 { get; set; }

  public TapSize ExternalParametersWindowSize { get; set; }

  public TapSize MultipleChoiceWindowSize { get; set; }

  [Obsolete("Use PackageManagerGui settings instead")]
  public TapSize PluginManagerWindowSize { get; set; }

  public TapSize SweepValuesWindowSize { get; set; }

  [Obsolete("The hub is deprecated")]
  public TapSize HubWindowSize { get; set; }

  public TapSize UserInputSize { get; set; }

  public Dictionary<string, TapSize> UserInputSizes { get; set; } = new Dictionary<string, TapSize>();

  public TapSize NewStepDialogWindowSize { get; set; }

  public TapSize SettingDialogWindowSize { get; set; }

  public bool TestPlanGridScrollToPlaying { get; set; }

  public Dictionary<string, double> PluginManagerColumnWidth { get; set; }

  public Dictionary<string, double> PropertyDataGridColumnWidth { get; set; } = new Dictionary<string, double>();

  public Dictionary<string, string> SelectedControlProvider
  {
    get => this.dictionary_3 ?? (this.dictionary_3 = new Dictionary<string, string>());
    set => this.dictionary_3 = value ?? new Dictionary<string, string>();
  }

  public Dictionary<string, double> GridSplitterRatios { get; set; }

  public Dictionary<string, TapSize> DialogSize { get; set; }

  public HashSet<string> DisabledLogGroups { get; set; } = new HashSet<string>();

  public List<GuiControlsSettings.FilterConfiguration> FilterConfigurationData
  {
    get
    {
      return this.FilterConfigurations.Select<KeyValuePair<string, HashSet<string>>, GuiControlsSettings.FilterConfiguration>((Func<KeyValuePair<string, HashSet<string>>, GuiControlsSettings.FilterConfiguration>) (keyValuePair_0 => new GuiControlsSettings.FilterConfiguration()
      {
        Name = keyValuePair_0.Key,
        Values = keyValuePair_0.Value
      })).ToList<GuiControlsSettings.FilterConfiguration>();
    }
    set
    {
      this.FilterConfigurations = new Dictionary<string, HashSet<string>>();
      foreach (GuiControlsSettings.FilterConfiguration filterConfiguration in value)
        this.FilterConfigurations[filterConfiguration.Name] = filterConfiguration.Values;
    }
  }

  [XmlIgnore]
  public bool MainWindowIsRepeatEnabled { get; set; }

  public bool MainWindowRepeatStopCount { get; set; }

  public bool MainWindowRepeatStopPending { get; set; }

  public bool MainWindowRepeatStopPass { get; set; }

  public bool MainWindowRepeatStopFail { get; set; }

  public bool MainWindowRepeatStopError { get; set; }

  public bool MainWindowRepeatStopAbort { get; set; }

  public uint MainWindowRepeatMaxCount { get; set; }

  public string FilePathControlLastBrowsedPath { get; set; }

  public bool PropGridItemExists(string item) => this.PropGridCategories.Contains(item);

  private void method_0()
  {
    if (this.PropGridCategoryStates.Count == this.PropGridCategories.Count)
      return;
    this.PropGridCategoryStates.Clear();
    this.PropGridCategories.Clear();
  }

  public bool? PropGridItemIsExpanded(string item)
  {
    this.method_0();
    int index = this.PropGridCategories.IndexOf(item);
    return index == -1 ? new bool?() : new bool?(this.PropGridCategoryStates[index]);
  }

  public GuiControlsSettings()
  {
    this.ExpandedStepCategories = new List<string>();
    this.PropGridCategories = new List<string>();
    this.PropGridCategoryStates = new List<bool>();
    this.TestPlanColumnState2 = new List<object>();
    this.MainWindowHeight = 768.0;
    this.MainWindowWidth = 1024.0;
    this.RecentTestPlans = new List<string>();
    this.TestPlanGridColumnDetails = "";
    this.PluginManagerColumnWidth = new Dictionary<string, double>();
    this.GridSplitterRatios = new Dictionary<string, double>();
    this.DialogSize = new Dictionary<string, TapSize>();
    this.FilterConfigurations = new Dictionary<string, HashSet<string>>();
    this.MainWindowRepeatStopError = true;
  }

  public void SetPropGridCategoryState(string string_2, bool isExpanded)
  {
    this.method_0();
    int index = this.PropGridCategories.IndexOf(string_2);
    if (index == -1)
    {
      this.PropGridCategories.Add(string_2);
      this.PropGridCategoryStates.Add(isExpanded);
      if (this.PropGridCategories.Count <= 500)
        return;
      this.PropGridCategories.RemoveRange(0, 10);
      this.PropGridCategoryStates.RemoveRange(0, 10);
    }
    else
    {
      if (this.PropGridCategoryStates[index] == isExpanded)
        return;
      this.PropGridCategoryStates[index] = isExpanded;
    }
  }

  public class FilterConfiguration
  {
    public string Name { get; set; }

    public HashSet<string> Values { get; set; }
  }
}
