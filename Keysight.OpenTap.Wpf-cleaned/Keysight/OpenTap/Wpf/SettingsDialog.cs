// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SettingsDialog
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SettingsDialog : WslDialog, IComponentConnector, IStyleConnector
{
  internal static HashSet<Type> previouslyFailedPlugins = new HashSet<Type>();
  public static RoutedEvent SettingsChangedEvent = EventManager.RegisterRoutedEvent("SettingsChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SettingsDialog));
  private bool bool_1;
  private object object_1;
  private bool bool_2;
  private const string string_0 = ".TapSettings";
  private static TraceSource traceSource_0 = Log.CreateSource("Settings Dialog");
  internal SettingsDialog @this;
  internal System.Windows.Controls.ComboBox configSetSelector;
  internal System.Windows.Controls.Button buttonDelete;
  internal System.Windows.Controls.TabControl settingsTabs;
  internal System.Windows.Controls.Button OkButton;
  internal System.Windows.Controls.Button CancelButton;
  private bool bool_3;

  internal static ISettingsControlProvider CreateInstance(Type type_0)
  {
    try
    {
      return (ISettingsControlProvider) type_0.CreateInstance();
    }
    catch (TargetInvocationException ex)
    {
      if (!(ex.InnerException is LicenseException))
        throw ex;
      SettingsDialog.previouslyFailedPlugins.Add(type_0);
      Log.Warning(SettingsDialog.traceSource_0, "Could not create '{0}' settings template: {1}", new object[2]
      {
        (object) type_0.GetDisplayAttribute().Name,
        (object) ex.InnerException.Message
      });
      return (ISettingsControlProvider) null;
    }
    catch (Exception ex)
    {
      Log.Error(SettingsDialog.traceSource_0, "Exception while creating settings panel '{0}': {1}", new object[2]
      {
        (object) type_0.GetDisplayAttribute().Name,
        (object) ex.Message
      });
      Log.Debug(SettingsDialog.traceSource_0, ex);
      return (ISettingsControlProvider) null;
    }
  }

  internal static IEnumerable<ISettingsControlProvider> GetSettingsControlProviders()
  {
    return PluginManager.GetPlugins<ISettingsControlProvider>().OrderByDescending<Type, double>((Func<Type, double>) (type_0 => type_0.GetDisplayAttribute().Order)).Select<Type, ISettingsControlProvider>(new Func<Type, ISettingsControlProvider>(SettingsDialog.CreateInstance)).Where<ISettingsControlProvider>((Func<ISettingsControlProvider, bool>) (isettingsControlProvider_0 => isettingsControlProvider_0 != null));
  }

  public IList<SettingsDialog.ConfigurationSetItem> ConfigurationSets { get; set; }

  private bool ChangeHasHappened { get; set; }

  private void method_0()
  {
    this.ChangeHasHappened = true;
    this.tapSettings.Settings.PushSettingsChanged();
  }

  private object PreSelected { get; set; }

  public void PreSelectTab(object object_2) => this.PreSelected = object_2;

  public void FindTab(ComponentSettings componentSettings)
  {
    if (this.tapSettings == null || componentSettings == null)
      return;
    Type type_0 = componentSettings.GetType();
    componentSettings = this.tapSettings.SettingsList.FirstOrDefault<ComponentSettings>((Func<ComponentSettings, bool>) (componentSettings_0 => componentSettings_0.GetType() == type_0));
    this.settingsTabs.SelectedItem = (object) componentSettings;
  }

  public async void FindTab(object object_2)
  {
    SettingsDialog depObj = this;
    if (object_2 is ComponentSettings)
    {
      depObj.FindTab((ComponentSettings) object_2);
    }
    else
    {
      IList list = (IList) null;
      if (object_2 is TapSettings.ResourceListSet resourceListSet)
      {
        list = resourceListSet.List;
        object_2 = resourceListSet.Item;
      }
      if (list == null)
      {
        list = ComponentSettingsList.GetContainer(object_2.GetType());
        if (list == null)
          return;
        Type type_0 = list.GetType();
        list = (IList) depObj.tapSettings.SettingsList.FirstOrDefault<ComponentSettings>((Func<ComponentSettings, bool>) (componentSettings_0 => componentSettings_0.GetType() == type_0));
      }
      if (list == null)
        return;
      if (depObj.IsLoaded)
      {
        depObj.FindTab((ComponentSettings) list);
        PropGridListEditor propGridListEditor = (PropGridListEditor) null;
        int num = 0;
        while (num++ < 10 && ((propGridListEditor = depObj.FindVisualChild<PropGridListEditor>()) == null || propGridListEditor.DataContext != list))
        {
          TaskAwaiter awaiter = Task.Delay(20).GetAwaiter();
          if (awaiter.IsCompleted)
          {
            awaiter.GetResult();
          }
          else
          {
            // ISSUE: explicit reference operation
            // ISSUE: reference to a compiler-generated field
            (^this).\u003C\u003E1__state = 0;
            TaskAwaiter taskAwaiter = awaiter;
            // ISSUE: explicit reference operation
            // ISSUE: reference to a compiler-generated field
            (^this).\u003C\u003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDialog.\u003CFindTab\u003Ed__22>(ref awaiter, this);
            return;
          }
        }
        propGridListEditor?.SelectObject(object_2);
        depObj.Activate();
        propGridListEditor = (PropGridListEditor) null;
      }
      else
      {
        depObj.FindTab((ComponentSettings) list);
        depObj.object_1 = object_2;
        depObj.Loaded += new RoutedEventHandler(depObj.method_1);
      }
      list = (IList) null;
    }
  }

  private void method_1(object sender, RoutedEventArgs e)
  {
    this.Loaded -= new RoutedEventHandler(this.method_1);
    this.FindVisualChild<PropGridListEditor>()?.SelectObject(this.object_1);
    this.Activate();
  }

  private TapSettings.Dialog tapSettings => this.DataContext as TapSettings.Dialog;

  public SettingsDialog()
  {
    this.ConfigurationSets = (IList<SettingsDialog.ConfigurationSetItem>) new ObservableCollection<SettingsDialog.ConfigurationSetItem>();
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.SettingsDialog_Loaded);
    TapSize dialogWindowSize = ComponentSettings<GuiControlsSettings>.Current.SettingDialogWindowSize;
    if (dialogWindowSize != null)
    {
      this.Height = dialogWindowSize.Height;
      this.Width = dialogWindowSize.Width;
    }
    this.AddHandler(SettingsDialog.SettingsChangedEvent, (Delegate) new RoutedEventHandler(this.method_12));
  }

  private void SettingsDialog_Loaded(object sender, RoutedEventArgs e)
  {
    this.Loaded -= new RoutedEventHandler(this.SettingsDialog_Loaded);
    if (this.tapSettings.Profile)
      this.method_2();
    this.FindTab(this.PreSelected);
  }

  private void method_2()
  {
    IList<SettingsDialog.ConfigurationSetItem> configurationSets = this.ConfigurationSets;
    configurationSets.Clear();
    DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ComponentSettings.SettingsDirectoryRoot, this.tapSettings.GroupName));
    string str = (string) null;
    try
    {
      ComponentSettings.EnsureSettingsDirectoryExists(this.tapSettings.GroupName, true);
      str = ComponentSettings.GetSettingsDirectory(this.tapSettings.GroupName, true);
    }
    catch (Exception ex)
    {
      Log.Error(SettingsDialog.traceSource_0, "Error occured while loading settings profiles.", Array.Empty<object>());
      Log.Debug(SettingsDialog.traceSource_0, ex);
    }
    SettingsDialog.ConfigurationSetItem configurationSetItem1 = (SettingsDialog.ConfigurationSetItem) null;
    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
    {
      SettingsDialog.ConfigurationSetItem configurationSetItem2 = new SettingsDialog.ConfigurationSetItem()
      {
        Name = directory.Name
      };
      configurationSetItem2.Selected += new SettingsDialog.ConfigurationSetItem.SelectionDelegate(this.method_8);
      configurationSetItem2.Directory = directory.FullName;
      configurationSets.Add(configurationSetItem2);
      if (configurationSetItem2.Directory == str)
        configurationSetItem1 = configurationSetItem2;
    }
    this.buttonDelete.IsEnabled = configurationSets.Count > 1;
    SettingsDialog.ConfigurationSetItem configurationSetItem3 = new SettingsDialog.ConfigurationSetItem()
    {
      Name = "<New settings profile>"
    };
    configurationSetItem3.Selected += (SettingsDialog.ConfigurationSetItem.SelectionDelegate) (configurationSetItem_0 => this.method_3());
    configurationSets.Add(configurationSetItem3);
    if (configurationSetItem1 == null)
      this.configSetSelector.SelectedItem = (object) this.ConfigurationSets.FirstOrDefault<SettingsDialog.ConfigurationSetItem>();
    else
      this.configSetSelector.SelectedItem = (object) configurationSetItem1;
  }

  private bool method_3()
  {
    SettingsDialogNewSetWindow dialogNewSetWindow1 = new SettingsDialogNewSetWindow(this.tapSettings.GroupName);
    dialogNewSetWindow1.Owner = (Window) this;
    SettingsDialogNewSetWindow dialogNewSetWindow2 = dialogNewSetWindow1;
    bool? nullable = dialogNewSetWindow2.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return false;
    string settingsDirectory = ComponentSettings.GetSettingsDirectory(this.tapSettings.GroupName, true);
    this.tapSettings.Settings.SetSettingsDir(Path.Combine(ComponentSettings.SettingsDirectoryRoot, this.tapSettings.GroupName, dialogNewSetWindow2.NewConfigSetName));
    try
    {
      ComponentSettings.EnsureSettingsDirectoryExists(this.tapSettings.GroupName, true);
    }
    catch (Exception ex)
    {
      if (!(ex is DirectoryNotFoundException))
      {
        Log.Error(SettingsDialog.traceSource_0, $"Error occured while changing settings profile for '{this.tapSettings.GroupName}': '{ex.Message}'", Array.Empty<object>());
        Log.Debug(SettingsDialog.traceSource_0, ex);
      }
      this.tapSettings.Settings.SetSettingsDir(settingsDirectory);
      return false;
    }
    this.method_2();
    return true;
  }

  private void method_4()
  {
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.FileName = (this.configSetSelector.SelectedItem as SettingsDialog.ConfigurationSetItem).Name;
    saveFileDialog1.DefaultExt = ".TapSettings";
    saveFileDialog1.Filter = string.Format("Settings Profile ({0})|*{0}", (object) ".TapSettings");
    saveFileDialog1.Title = "Export Settings Profile";
    using (SaveFileDialog saveFileDialog2 = saveFileDialog1)
    {
      if (saveFileDialog2.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      this.tapSettings.Settings.ExportBenchTo(saveFileDialog2.FileName);
    }
  }

  private void method_5()
  {
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.DefaultExt = ".TapSettings";
    openFileDialog1.Filter = string.Format("Settings Profile ({0})|*{0}", (object) ".TapSettings");
    openFileDialog1.Title = "Import Settings Profile";
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      return;
    string fileName = openFileDialog2.FileName;
    SettingsDialogNewSetWindow dialogNewSetWindow1 = new SettingsDialogNewSetWindow(this.tapSettings.GroupName, Path.GetFileNameWithoutExtension(fileName));
    dialogNewSetWindow1.Owner = (Window) this;
    SettingsDialogNewSetWindow dialogNewSetWindow2 = dialogNewSetWindow1;
    bool? nullable = dialogNewSetWindow2.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return;
    string fullPath = Path.GetFullPath(Path.Combine(ComponentSettings.SettingsDirectoryRoot, this.tapSettings.GroupName ?? "", dialogNewSetWindow2.NewConfigSetName));
    this.tapSettings.Settings.ImportBenchTo(fileName, fullPath);
    this.tapSettings.Settings.SetSettingsDir(fullPath);
    this.method_2();
  }

  private void method_6()
  {
    if (this.ConfigurationSets.Count <= 2 || QuickDialog.Show("Delete Settings Profile", $"Are you sure you want to delete the settings profile {(this.configSetSelector.SelectedItem as SettingsDialog.ConfigurationSetItem).Name}?", "Delete") != QuickDialog.DialogOption.Yes)
      return;
    this.method_7();
  }

  private void method_7()
  {
    string dirToDelete = ComponentSettings.GetSettingsDirectory(this.tapSettings.GroupName, true);
    this.configSetSelector.SelectedItem = (object) this.ConfigurationSets.FirstOrDefault<SettingsDialog.ConfigurationSetItem>((Func<SettingsDialog.ConfigurationSetItem, bool>) (configurationSetItem_0 => configurationSetItem_0.Directory != dirToDelete));
    FileSystemHelper.DeleteDirectory(dirToDelete);
    this.method_2();
  }

  private bool method_8(
    SettingsDialog.ConfigurationSetItem configurationSetItem_0)
  {
    this.tapSettings.Settings.SetSettingsDir(configurationSetItem_0.Directory);
    return true;
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

  private void OkButton_Click(object sender, RoutedEventArgs e)
  {
    if (this.ChangeHasHappened)
      this.bool_1 = true;
    this.Close();
  }

  protected override void OnClosed(EventArgs eventArgs_0)
  {
    if (this.bool_1 && this.tapSettings.Settings.AllowEdit)
      this.tapSettings.Settings.OnSettingsChanged();
    else if (this.ChangeHasHappened && this.tapSettings.Settings.AllowEdit)
      this.tapSettings.Settings.Revert();
    if (this.ChangeHasHappened)
      this.tapSettings.Settings.Apply();
    base.OnClosed(eventArgs_0);
  }

  private void configSetSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!(((Selector) sender).SelectedItem is SettingsDialog.ConfigurationSetItem selectedItem))
      return;
    int selectedIndex = this.settingsTabs.SelectedIndex;
    bool flag = selectedItem.Select();
    if (this.bool_2 & flag)
      this.method_0();
    this.bool_2 = true;
    if (!flag)
      (sender as System.Windows.Controls.ComboBox).SelectedItem = e.RemovedItems[0];
    this.settingsTabs.SelectedIndex = selectedIndex;
  }

  private void this_Closing(object sender, CancelEventArgs e)
  {
  }

  private void method_9(object sender, RoutedEventArgs e)
  {
    FocusManager.SetFocusedElement((DependencyObject) this, (IInputElement) this);
    this.method_3();
  }

  private void method_10(object sender, RoutedEventArgs e) => this.method_5();

  private void method_11(object sender, RoutedEventArgs e) => this.method_4();

  private void buttonDelete_Click(object sender, RoutedEventArgs e) => this.method_6();

  private void method_12(object sender, RoutedEventArgs e) => this.method_0();

  private PropGridListEditor method_13()
  {
    string name = "PART_SelectedContentHost";
    ContentPresenter dpObj = this.settingsTabs.GetVisualChildren().OfType<ContentPresenter>().FirstOrDefault<ContentPresenter>((Func<ContentPresenter, bool>) (item => item.Name == name));
    return dpObj == null ? (PropGridListEditor) null : dpObj.GetVisualChildren().OfType<PropGridListEditor>().FirstOrDefault<PropGridListEditor>();
  }

  private void method_14(object sender, CanExecuteRoutedEventArgs e)
  {
    this.method_13()?.canExecuteDelete(sender, e);
  }

  private void method_15(object sender, CanExecuteRoutedEventArgs e)
  {
    this.method_13()?.canExecutePaste(sender, e);
  }

  private void method_16(object sender, CanExecuteRoutedEventArgs e)
  {
    this.method_13()?.CanCopy(sender, e);
  }

  private void method_17(object sender, CanExecuteRoutedEventArgs e)
  {
    this.method_13()?.CanCut(sender, e);
  }

  private void method_18(object sender, RoutedEventArgs e) => this.method_13()?.OnCut(sender, e);

  private void method_19(object sender, RoutedEventArgs e) => this.method_13()?.OnCopy(sender, e);

  private void method_20(object sender, RoutedEventArgs e) => this.method_13()?.OnPaste(sender, e);

  private void method_21(object sender, RoutedEventArgs e)
  {
    this.method_13()?.OnDeleteListItem(sender, e);
  }

  private void this_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ComponentSettings<GuiControlsSettings>.Current.SettingDialogWindowSize = new TapSize()
    {
      Width = e.NewSize.Width,
      Height = e.NewSize.Height
    };
  }

  private void method_22(object sender, RoutedEventArgs e)
  {
    System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox) sender;
    object dataContext = comboBox.DataContext;
    List<Type> typeList = new List<Type>();
    Type type = dataContext.GetType();
    foreach (ISettingsControlProvider settingsControlProvider in SettingsDialog.GetSettingsControlProviders())
    {
      if (settingsControlProvider.GetControlType(type) != (Type) null)
        typeList.Add(settingsControlProvider.GetType());
    }
    if (typeList.Count <= 1)
      comboBox.Visibility = Visibility.Collapsed;
    else
      comboBox.ItemsSource = (IEnumerable) typeList;
    int num = 0;
    string str = (string) null;
    if (ComponentSettings<GuiControlsSettings>.Current.SelectedControlProvider.ContainsKey(type.Name))
      str = ComponentSettings<GuiControlsSettings>.Current.SelectedControlProvider[type.Name];
    foreach (MemberInfo memberInfo in typeList)
    {
      if (!(memberInfo.Name == str))
      {
        ++num;
      }
      else
      {
        comboBox.SelectedIndex = num;
        break;
      }
    }
  }

  private void method_23(object sender, SelectionChangedEventArgs e)
  {
    Type type = ((FrameworkElement) sender).DataContext.GetType();
    Type addedItem = (Type) e.AddedItems[0];
    if (!(SettingsTemplateSelector.GetPreferedProvider(type) != addedItem))
      return;
    SettingsTemplateSelector.SetPreferedSettingsProvider(type, addedItem);
    this.settingsTabs.ContentTemplateSelector = (DataTemplateSelector) new SettingsTemplateSelector();
    this.settingsTabs.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
    ComponentSettings<GuiControlsSettings>.Current.SelectedControlProvider[type.Name] = addedItem.Name;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_3)
      return;
    this.bool_3 = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/settingsdialog.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.@this = (SettingsDialog) target;
        this.@this.Closing += new CancelEventHandler(this.this_Closing);
        this.@this.SizeChanged += new SizeChangedEventHandler(this.this_SizeChanged);
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_19);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_16);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_20);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_15);
        break;
      case 4:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_18);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_17);
        break;
      case 5:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_21);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_14);
        break;
      case 6:
        this.configSetSelector = (System.Windows.Controls.ComboBox) target;
        this.configSetSelector.SelectionChanged += new SelectionChangedEventHandler(this.configSetSelector_SelectionChanged);
        break;
      case 7:
        ((System.Windows.Controls.Primitives.ButtonBase) target).Click += new RoutedEventHandler(this.method_9);
        break;
      case 8:
        this.buttonDelete = (System.Windows.Controls.Button) target;
        this.buttonDelete.Click += new RoutedEventHandler(this.buttonDelete_Click);
        break;
      case 9:
        ((System.Windows.Controls.Primitives.ButtonBase) target).Click += new RoutedEventHandler(this.method_10);
        break;
      case 10:
        ((System.Windows.Controls.Primitives.ButtonBase) target).Click += new RoutedEventHandler(this.method_11);
        break;
      case 11:
        this.settingsTabs = (System.Windows.Controls.TabControl) target;
        break;
      case 13:
        this.OkButton = (System.Windows.Controls.Button) target;
        this.OkButton.Click += new RoutedEventHandler(this.OkButton_Click);
        break;
      case 14:
        this.CancelButton = (System.Windows.Controls.Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this.bool_3 = true;
        break;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 12)
      return;
    ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.method_22);
    ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.method_23);
  }

  public class ConfigurationSetItem
  {
    public event SettingsDialog.ConfigurationSetItem.SelectionDelegate Selected;

    public bool Select() => this.Selected == null || this.Selected(this);

    public string Name { get; set; }

    public string Directory { get; set; }

    public delegate bool SelectionDelegate(SettingsDialog.ConfigurationSetItem selectedItem);
  }
}
