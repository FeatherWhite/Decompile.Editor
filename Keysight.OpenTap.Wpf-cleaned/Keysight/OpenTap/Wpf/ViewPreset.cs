// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ViewPreset
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ViewPreset
{
  public static string PresetDir = System.IO.Path.Combine(ComponentSettings.SettingsDirectoryRoot, "Presets");
  private static readonly TraceSource traceSource_0 = Log.CreateSource("Presets");

  public ViewPreset(string presetName) => this.PresetName = presetName;

  public string PresetName { get; set; } = "Default";

  private bool FileExists => File.Exists(this.Path);

  public string Path
  {
    get
    {
      return System.IO.Path.Combine(ComponentSettings.SettingsDirectoryRoot, "Presets", this.PresetName + ".xml");
    }
  }

  public void Save()
  {
    Directory.CreateDirectory(ViewPreset.PresetDir);
    File.WriteAllText(this.Path, ViewPreset.smethod_1());
  }

  public void Load() => ViewPreset.smethod_0(File.ReadAllText(this.Path));

  private static void smethod_0(string string_1)
  {
    using (List<ViewPreset.SettingsGroup>.Enumerator enumerator = ((List<ViewPreset.SettingsGroup>) new TapSerializer().DeserializeFromString(string_1, (ITypeData) TypeData.FromType(typeof (List<ViewPreset.SettingsGroup>)), true, (string) null)).GetEnumerator())
    {
label_2:
      while (enumerator.MoveNext())
      {
        ViewPreset.SettingsGroup current1 = enumerator.Current;
        ITypeData typeData = TypeData.GetTypeData(current1.Name);
        if (typeData != null)
        {
          ComponentSettings current2 = ComponentSettings.GetCurrent(typeData);
          if (current2 != null)
          {
            ViewPreset.SettingsTuple[] settings = current1.Settings;
            int index = 0;
            while (true)
            {
              if (index < settings.Length)
              {
                ViewPreset.SettingsTuple settingsTuple = settings[index];
                IMemberData member = typeData.GetMember(settingsTuple.Name);
                if (member != null)
                {
                  if (ReflectionDataExtensions.HasAttribute<ViewPreset.Member>((IReflectionData) member))
                  {
                    try
                    {
                      member.SetValue((object) current2, settingsTuple.Value);
                    }
                    catch
                    {
                    }
                  }
                }
                ++index;
              }
              else
                goto label_2;
            }
          }
        }
      }
    }
  }

  private static string smethod_1()
  {
    IEnumerable<ITypeData> derivedTypes = TypeData.GetDerivedTypes<ComponentSettings>();
    List<ViewPreset.SettingsGroup> settingsGroupList = new List<ViewPreset.SettingsGroup>();
    foreach (ITypeData itypeData in derivedTypes)
    {
      IMemberData[] array = itypeData.GetMembers().Where<IMemberData>((Func<IMemberData, bool>) (imemberData_0 => ReflectionDataExtensions.HasAttribute<ViewPreset.Member>((IReflectionData) imemberData_0))).ToArray<IMemberData>();
      if (array.Length != 0)
      {
        ComponentSettings current = ComponentSettings.GetCurrent(itypeData);
        if (current != null)
        {
          ViewPreset.SettingsGroup settingsGroup = new ViewPreset.SettingsGroup();
          settingsGroup.Name = ((IReflectionData) itypeData).Name;
          List<ViewPreset.SettingsTuple> settingsTupleList = new List<ViewPreset.SettingsTuple>();
          foreach (IMemberData imemberData in array)
          {
            try
            {
              object obj = imemberData.GetValue((object) current);
              settingsTupleList.Add(new ViewPreset.SettingsTuple()
              {
                Name = ((IReflectionData) imemberData).Name,
                Value = obj
              });
            }
            catch
            {
            }
          }
          settingsGroup.Settings = settingsTupleList.ToArray();
          settingsGroupList.Add(settingsGroup);
        }
      }
    }
    return new TapSerializer().SerializeToString((object) settingsGroupList);
  }

  public static void SaveDefaultProfile(Action onSave)
  {
    ViewPreset viewPreset = new ViewPreset("Default");
    if (viewPreset.FileExists)
      return;
    onSave();
    viewPreset.Save();
  }

  public static void SelectPreset(string layoutName) => new ViewPreset(layoutName).Load();

  public static string[] ListPresets()
  {
    try
    {
      return Directory.EnumerateFiles(ViewPreset.PresetDir, "*.xml").Select<string, string>(new Func<string, string>(System.IO.Path.GetFileNameWithoutExtension)).OrderBy<string, int>((Func<string, int>) (string_0 => !(string_0 == "Default") ? 1 : 0)).ToArray<string>();
    }
    catch
    {
      return Array.Empty<string>();
    }
  }

  private static List<ViewPreset.PresetEntry> smethod_2()
  {
    List<ViewPreset.PresetEntry> entries = new List<ViewPreset.PresetEntry>();
    int num = 0;
    foreach (string listPreset in ViewPreset.ListPresets())
    {
      entries.Add(new ViewPreset.PresetEntry(entries)
      {
        Name = listPreset,
        InitialName = listPreset
      });
      ++num;
    }
    return entries;
  }

  public static void EditPresets()
  {
    ViewPreset.PresetEntries source = new ViewPreset.PresetEntries((IList<ViewPreset.PresetEntry>) ViewPreset.smethod_2());
    foreach (ViewPreset.PresetEntry presetEntry in (ReadOnlyCollection<ViewPreset.PresetEntry>) source)
      presetEntry.ShowDelete = true;
    UserInput.Request((object) source, true);
    string[] array = source.Select<ViewPreset.PresetEntry, string>((Func<ViewPreset.PresetEntry, string>) (presetEntry_0 => presetEntry_0.Error)).Where<string>((Func<string, bool>) (string_0 => !string.IsNullOrEmpty(string_0))).ToArray<string>();
    if (array.Length != 0)
    {
      Log.Error(ViewPreset.traceSource_0, "Unable to save preset configuration due to errors: {0}", new object[1]
      {
        (object) string.Join(" ", array)
      });
    }
    else
    {
      foreach (ViewPreset.PresetEntry presetEntry in (ReadOnlyCollection<ViewPreset.PresetEntry>) source)
      {
        if (!presetEntry.IsDefault && string.IsNullOrEmpty(presetEntry.Error))
        {
          if (presetEntry.Delete)
            presetEntry.asViewPreset.method_0();
          else if (presetEntry.InitialName != presetEntry.Name)
          {
            try
            {
              File.Move(new ViewPreset(presetEntry.InitialName).Path, presetEntry.asViewPreset.Path);
            }
            catch (Exception ex)
            {
              Log.Error(ViewPreset.traceSource_0, "Unable to rename preset: {0}", new object[1]
              {
                (object) ex.Message
              });
              Log.Debug(ViewPreset.traceSource_0, ex);
            }
          }
        }
      }
    }
  }

  private void method_0() => File.Delete(this.Path);

  public static void SaveAs()
  {
    List<ViewPreset.PresetEntry> entries = ViewPreset.smethod_2();
    ViewPreset.PresetEntryQuestion presetEntryQuestion1 = new ViewPreset.PresetEntryQuestion(entries);
    presetEntryQuestion1.Name = "";
    ViewPreset.PresetEntryQuestion presetEntryQuestion2 = presetEntryQuestion1;
    entries.Add((ViewPreset.PresetEntry) presetEntryQuestion2);
    UserInput.Request((object) presetEntryQuestion2, true);
    if (presetEntryQuestion2.Response == ViewPreset.PresetQuestionResponses.Cancel)
      return;
    presetEntryQuestion2.Name = presetEntryQuestion2.Name.Trim();
    if (string.IsNullOrWhiteSpace(presetEntryQuestion2.Name))
      return;
    if (!string.IsNullOrWhiteSpace(presetEntryQuestion2.Error))
      return;
    try
    {
      presetEntryQuestion2.asViewPreset.Save();
    }
    catch (Exception ex)
    {
      Log.Error(ViewPreset.traceSource_0, "Unable to save preset: {0}", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(ViewPreset.traceSource_0, ex);
    }
  }

  public class Member : Attribute
  {
  }

  [DebuggerDisplay("Settings Group: ({Name}: {Settings.Length} elements)")]
  private class SettingsGroup
  {
    public string Name { get; set; }

    public ViewPreset.SettingsTuple[] Settings { get; set; }
  }

  [DebuggerDisplay("Settings Tuple ({Name}: {Value})")]
  private class SettingsTuple
  {
    public string Name { get; set; }

    public object Value { get; set; }
  }

  private enum PresetQuestionResponses
  {
    [Display("Save Preset", null, null, -10000.0, false, null)] Save,
    Cancel,
  }

  [Display("Select a name for the preset.", null, null, -10000.0, false, null)]
  private class PresetEntry : ValidatingObject
  {
    public bool ShowDelete { get; internal set; }

    public bool IsDefault => this.InitialName == "Default";

    public bool IsInitialName => this.InitialName == this.Name;

    [EnabledIf("IsDefault", new object[] {false})]
    [Display("Name", null, null, -1.0, false, null)]
    public string Name { get; set; }

    [Display("Delete?", "Delete this preset?", null, 1.0, false, null)]
    [EnabledIf("ShowDelete", new object[] {}, HideIfDisabled = true)]
    [EnabledIf("IsDefault", new object[] {false})]
    public bool Delete { get; set; }

    [AnnotationIgnore]
    public string InitialName { get; set; }

    public ViewPreset asViewPreset => new ViewPreset(this.Name);

    private static bool smethod_0(string string_2, string string_3)
    {
      string a;
      if (string_2 == null)
      {
        a = (string) null;
      }
      else
      {
        a = string_2.Trim();
        if (a != null)
          goto label_4;
      }
      a = "";
label_4:
      string b;
      if (string_3 == null)
      {
        b = (string) null;
      }
      else
      {
        b = string_3.Trim();
        if (b != null)
          goto label_8;
      }
      b = "";
label_8:
      return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }

    public PresetEntry(List<ViewPreset.PresetEntry> entries)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ViewPreset.PresetEntry.\u003C\u003Ec__DisplayClass23_0 cDisplayClass230 = new ViewPreset.PresetEntry.\u003C\u003Ec__DisplayClass23_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass230.entries = entries;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass230.\u003C\u003E4__this = this;
      // ISSUE: method pointer
      this.Rules.Add(new IsValidDelegateDefinition((object) cDisplayClass230, __methodptr(\u003C\u002Ector\u003Eb__0)), "Name cannot be 'Default'", nameof (Name));
      // ISSUE: method pointer
      this.Rules.Add(new IsValidDelegateDefinition((object) cDisplayClass230, __methodptr(\u003C\u002Ector\u003Eb__1)), "Cannot rename to the same name as another preset.", nameof (Name));
      // ISSUE: method pointer
      this.Rules.Add(new IsValidDelegateDefinition((object) cDisplayClass230, __methodptr(\u003C\u002Ector\u003Eb__2)), "Preset name contains invalid filename characters.", nameof (Name));
      // ISSUE: method pointer
      this.Rules.Add(new IsValidDelegateDefinition((object) cDisplayClass230, __methodptr(\u003C\u002Ector\u003Eb__3)), "Cannot both delete and rename a preset.", nameof (Delete));
      // ISSUE: method pointer
      this.Rules.Add(new IsValidDelegateDefinition((object) cDisplayClass230, __methodptr(\u003C\u002Ector\u003Eb__4)), "Name cannot be empty.", nameof (Name));
    }
  }

  [Display("Select a name for the preset.", null, null, -10000.0, false, null)]
  private class PresetEntryQuestion(List<ViewPreset.PresetEntry> entries) : ViewPreset.PresetEntry(entries)
  {
    [Submit]
    [Layout]
    public ViewPreset.PresetQuestionResponses Response { get; set; }
  }

  [Display("Edit Preset Configuration", "Add, delete or configure view presets.", null, -10000.0, false, null)]
  [ImportExportExclude]
  private class PresetEntries(IList<ViewPreset.PresetEntry> list) : 
    ReadOnlyCollection<ViewPreset.PresetEntry>(list)
  {
  }
}
