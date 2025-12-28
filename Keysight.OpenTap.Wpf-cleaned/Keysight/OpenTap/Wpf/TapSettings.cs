// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TapSettings
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class TapSettings : INotifyPropertyChanged
{
  public static readonly RoutedUICommand Open = new RoutedUICommand(nameof (Open), nameof (Open), typeof (TapSettings));
  public string HelpLink;
  private bool bool_0 = true;
  private ComponentSettings[] componentSettings_0 = Array.Empty<ComponentSettings>();
  private readonly TapSettings.Dialog dialog_0;
  private bool bool_1;
  private static TraceSource traceSource_0 = Log.CreateSource("Settings");
  public Action ReloadUi;
  private HashSet<ITypeData> hashSet_0;

  public bool AllowEdit
  {
    get => this.bool_0;
    set
    {
      this.bool_0 = value;
      this.method_2(nameof (AllowEdit));
    }
  }

  public IEnumerable<ComponentSettings> AllSettings
  {
    get => (IEnumerable<ComponentSettings>) this.componentSettings_0;
  }

  protected virtual IEnumerable<ComponentSettings> getAllComponentsSettings()
  {
    IEnumerable<ITypeData> derivedTypes = TypeData.GetDerivedTypes<ComponentSettings>();
    return (IEnumerable<ComponentSettings>) ((IEnumerable<ITypeData>) (this.hashSet_0 == null ? derivedTypes.Where<ITypeData>((Func<ITypeData, bool>) (type =>
    {
      if (!type.CanCreateInstance)
        return false;
      BrowsableAttribute attribute = ReflectionDataExtensions.GetAttribute<BrowsableAttribute>((IReflectionData) type);
      return attribute == null || attribute.Browsable;
    })) : derivedTypes.Where<ITypeData>(new Func<ITypeData, bool>(this.hashSet_0.Contains))).OrderBy<ITypeData, string>((Func<ITypeData, string>) (type => ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) type).GetFullName())).ToArray<ITypeData>()).Select<ITypeData, ComponentSettings>(new Func<ITypeData, ComponentSettings>(ComponentSettings.GetCurrent)).Where<ComponentSettings>((Func<ComponentSettings, bool>) (componentSettings_0 => componentSettings_0 != null)).ToArray<ComponentSettings>();
  }

  protected virtual Window GetOwnerWindow() => Application.Current.MainWindow;

  public void UpdateSettingsLists(IEnumerable<ComponentSettings> SettingsTypes = null)
  {
    foreach (ComponentSettings allSetting in this.AllSettings)
      allSetting.CacheInvalidated -= new EventHandler(this.method_1);
    this.componentSettings_0 = SettingsTypes == null ? this.getAllComponentsSettings().ToArray<ComponentSettings>() : SettingsTypes.ToArray<ComponentSettings>();
    foreach (ComponentSettings allSetting in this.AllSettings)
      allSetting.CacheInvalidated += new EventHandler(this.method_1);
    this.method_2("AllSettings");
  }

  public void PushSettingsChanged()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_1((object) this, new EventArgs());
  }

  private void method_0(object sender, PropertyChangedEventArgs e) => this.PushSettingsChanged();

  private void method_1(object sender, EventArgs e)
  {
    if (this.bool_1)
      return;
    ComponentSettings componentSettings = (ComponentSettings) sender;
    componentSettings.CacheInvalidated -= new EventHandler(this.method_1);
    ComponentSettings[] array = this.AllSettings.ToArray<ComponentSettings>();
    string settingname = "Settings";
    int index = Array.IndexOf<ComponentSettings>(array, componentSettings);
    if (index == -1)
      throw new Exception("Unknown setting");
    array[index] = ComponentSettings.GetCurrent(componentSettings.GetType());
    this.componentSettings_0[Array.IndexOf<ComponentSettings>(this.componentSettings_0, componentSettings)] = array[index];
    array[index].CacheInvalidated += new EventHandler(this.method_1);
    GuiHelpers.GuiInvoke((Action) (() =>
    {
      this.method_2(settingname);
      this.method_2("AllSettings");
    }));
  }

  public TapSettings() => this.dialog_0 = new TapSettings.Dialog(this);

  public void OnSettingsChanged()
  {
    foreach (ComponentSettings allSetting in this.AllSettings)
    {
      try
      {
        allSetting.Save();
      }
      catch (Exception ex)
      {
        Log.Error(TapSettings.traceSource_0, "Caught error while saving settings '{0}'.", new object[1]
        {
          (object) allSetting.GetType().GetDisplayAttribute().GetFullName()
        });
        Log.Debug(TapSettings.traceSource_0, ex);
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }

  public void Revert()
  {
    this.bool_1 = true;
    try
    {
      foreach (ComponentSettings allSetting in this.AllSettings)
        allSetting.Invalidate();
    }
    finally
    {
      this.bool_1 = false;
    }
    this.UpdateSettingsLists();
    foreach (ComponentSettings allSetting in this.AllSettings)
      allSetting.Reload();
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler2 = this.eventHandler_2;
    if (eventHandler2 == null)
      return;
    eventHandler2((object) this, EventArgs.Empty);
  }

  public void Apply()
  {
    if (this.ReloadUi == null)
      return;
    GuiHelpers.GuiInvokeAsync(this.ReloadUi);
  }

  public event EventHandler SettingsChanged;

  [Obsolete("No longer used")]
  public event EventHandler SettingsMinorChanged;

  public event EventHandler SettingsReverted;

  public event PropertyChangedEventHandler PropertyChanged;

  private void method_2(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_0));
  }

  public void OpenSettings(object object_0)
  {
    this.dialog_0.OpenDialog(object_0, this.GetOwnerWindow(), (object) this.HelpLink);
  }

  public void SetSettingsDir(string newdir)
  {
    if (newdir == ComponentSettings.GetSettingsDirectory(this.dialog_0.GroupName, true))
      return;
    try
    {
      foreach (ComponentSettings allSetting in this.AllSettings)
        allSetting.Save();
    }
    catch (Exception ex)
    {
      Log.Error(TapSettings.traceSource_0, "Unable to write to settings files: {0}", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(TapSettings.traceSource_0, ex);
    }
    ComponentSettings.SetSettingsProfile(this.dialog_0.GroupName, newdir);
  }

  public void ExportBenchTo(string packageFileName)
  {
    try
    {
      foreach (ComponentSettings settings in this.dialog_0.SettingsList)
        settings.Save();
      using (FileStream fileStream1 = File.Create(packageFileName))
      {
        using (ZipArchive zipArchive = new ZipArchive((Stream) fileStream1, ZipArchiveMode.Create))
        {
          foreach (string file in Directory.GetFiles(ComponentSettings.GetSettingsDirectory(this.dialog_0.GroupName, true), "*.xml", SearchOption.AllDirectories))
          {
            try
            {
              using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(file), CompressionLevel.Fastest).Open())
              {
                using (FileStream fileStream2 = File.OpenRead(file))
                  fileStream2.CopyTo(destination);
              }
            }
            catch (Exception ex)
            {
              throw new Exception($"Unable to export settings. Could not read the file \"{Path.GetFileName(file)}\".", ex);
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      try
      {
        File.Delete(packageFileName);
      }
      catch
      {
      }
      QuickDialog.ShowMessage("An error occured while exporting", ex.Message);
      Log.Error(TapSettings.traceSource_0, ex.Message, Array.Empty<object>());
      Log.Debug(TapSettings.traceSource_0, ex);
    }
  }

  public void ImportBenchTo(string packageFileName, string toDir)
  {
    FileSystemHelper.EnsureDirectory(toDir);
    using (FileStream fileStream = File.OpenRead(packageFileName))
    {
      using (ZipArchive zipArchive = new ZipArchive((Stream) fileStream, ZipArchiveMode.Read))
      {
        foreach (ZipArchiveEntry entry in zipArchive.Entries)
        {
          if (!(entry.Name == "[Content_Types].xml") && !(entry.Name == ".rels") && !entry.FullName.StartsWith("package/services/metadata/core-properties"))
          {
            string str = Path.Combine(toDir, entry.FullName);
            FileSystemHelper.EnsureDirectory(str);
            using (FileStream destination = File.OpenWrite(str))
            {
              using (Stream stream = entry.Open())
                stream.CopyTo((Stream) destination);
            }
          }
        }
      }
    }
  }

  public void LimitTo(ITypeData[] componentSettingsTypes)
  {
    this.hashSet_0 = ((IEnumerable<ITypeData>) componentSettingsTypes).ToHashset<ITypeData>();
    this.UpdateSettingsLists();
  }

  public struct ResourceListSet
  {
    public IList List;
    public object Item;
  }

  public class Dialog : INotifyPropertyChanged
  {
    private SettingsDialog settingsDialog_0;
    private int int_0;

    public bool Profile { get; private set; }

    public string GroupName { get; private set; }

    public TapSettings Settings { get; set; }

    public int SelectedIndex
    {
      get => this.int_0;
      set
      {
        if (value == this.int_0)
          return;
        this.int_0 = value;
        this.method_1(nameof (SelectedIndex));
      }
    }

    public IEnumerable<ComponentSettings> SettingsList
    {
      get
      {
        return (IEnumerable<ComponentSettings>) this.Settings.AllSettings.Where<ComponentSettings>((Func<ComponentSettings, bool>) (componentSettings_0 => componentSettings_0.GroupName == this.GroupName)).ToList<ComponentSettings>();
      }
    }

    public Dialog(TapSettings settings)
    {
      SettingsGroupAttribute attribute = settings.GetType().GetAttribute<SettingsGroupAttribute>();
      this.GroupName = attribute == null ? "" : attribute.GroupName;
      this.Profile = attribute != null && attribute.Profile;
      this.Settings = settings;
      settings.PropertyChanged += new PropertyChangedEventHandler(this.method_0);
    }

    private void method_0(object sender, PropertyChangedEventArgs e)
    {
      int int0 = this.int_0;
      if (e.PropertyName == "AllSettings")
        this.method_1("SettingsList");
      if (int0 < 0 || int0 >= this.SettingsList.Count<ComponentSettings>())
        return;
      this.SelectedIndex = int0;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void method_1(string string_1)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.propertyChangedEventHandler_0 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_1));
    }

    public void OpenDialog(object object_0, Window owner)
    {
      this.OpenDialog(object_0, owner, (object) null);
    }

    public void OpenDialog(object object_0, Window Owner, object helpLink)
    {
      IList list1 = (IList) null;
      if (object_0 is TapSettings.ResourceListSet resourceListSet)
        list1 = resourceListSet.List;
      if (this.settingsDialog_0 == null)
      {
        string str = "Settings";
        SettingsGroupAttribute settingsGroupAttribute = object_0.GetType().GetAttribute<SettingsGroupAttribute>();
        if (settingsGroupAttribute == null && !(object_0 is ComponentSettings))
        {
          IList list2 = list1 ?? ComponentSettingsList.GetContainer(object_0.GetType());
          settingsGroupAttribute = list2 != null ? list2.GetType().GetAttribute<SettingsGroupAttribute>() : settingsGroupAttribute;
        }
        this.GroupName = settingsGroupAttribute == null ? "" : settingsGroupAttribute.GroupName;
        this.Profile = settingsGroupAttribute != null && settingsGroupAttribute.Profile;
        if (Owner.GetType().Name == "MainWindow")
          str = this.GroupName + " Settings";
        SettingsDialog settingsDialog = new SettingsDialog();
        settingsDialog.DataContext = (object) this;
        settingsDialog.Title = str;
        settingsDialog.Owner = Owner;
        settingsDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.settingsDialog_0 = settingsDialog;
        if (helpLink == null)
          HelpManager.SetHelpLink((DependencyObject) this.settingsDialog_0, this.GroupName == "Bench" ? (object) "EditorHelp.chm::/Configurations/Resource Configuration/Readme.html" : (object) "EditorHelp.chm::/Editor Overview/Main Menu/Settings Menu.html");
        else
          HelpManager.SetHelpLink((DependencyObject) this.settingsDialog_0, helpLink);
      }
      this.settingsDialog_0.PreSelectTab(object_0);
      this.settingsDialog_0.ShowDialog();
      this.settingsDialog_0 = (SettingsDialog) null;
    }
  }
}
