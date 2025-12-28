// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SettingsDialogNewSetWindow
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SettingsDialogNewSetWindow : WslDialog, IComponentConnector
{
  internal SettingsDialogNewSetWindow This;
  internal Grid grid;
  internal TextBox SettingsProfileTextBox;
  internal TextBlock errorText;
  internal Button buttonAdd;
  private bool bool_0;

  public string NewConfigSetName { get; private set; }

  public SettingsDialogNewSetWindow(string groupName, string configName = null)
  {
    this.InitializeComponent();
    this.grid.DataContext = (object) new SettingsDialogNewSetWindow.NewSettingsGroupViewModel(configName ?? "My Settings Profile", groupName);
    this.Loaded += (RoutedEventHandler) ((sender, e) =>
    {
      this.SettingsProfileTextBox.Focus();
      this.SettingsProfileTextBox.SelectAll();
    });
    int num = 259 - (Path.Combine(ComponentSettings.SettingsDirectoryRoot, groupName) + "\\\\Instruments.xml").Length;
    this.SettingsProfileTextBox.MaxLength = num > 0 ? num : throw new Exception("Cannot create profile. The path to your OpenTAP installation is so long that it is conflicting with the Windows path length limit.");
  }

  public SettingsDialogNewSetWindow(string groupName)
    : this(groupName, (string) null)
  {
  }

  private void buttonAdd_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(true);
    this.Close();
    this.NewConfigSetName = ((SettingsDialogNewSetWindow.NewSettingsGroupViewModel) this.grid.DataContext).ConfigName.Trim();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/settingsdialognewsetwindow.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.This = (SettingsDialogNewSetWindow) target;
        break;
      case 2:
        this.grid = (Grid) target;
        break;
      case 3:
        this.SettingsProfileTextBox = (TextBox) target;
        break;
      case 4:
        this.errorText = (TextBlock) target;
        break;
      case 5:
        this.buttonAdd = (Button) target;
        this.buttonAdd.Click += new RoutedEventHandler(this.buttonAdd_Click);
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }

  private class NewSettingsGroupViewModel : INotifyPropertyChanged
  {
    private string string_0;
    private static HashSet<char> hashSet_0 = ((IEnumerable<char>) Path.GetInvalidFileNameChars()).ToHashset<char>();

    public NewSettingsGroupViewModel(string configName, string groupName)
    {
      this.GroupName = groupName;
      this.ConfigName = configName;
    }

    public string ConfigName
    {
      get => this.string_0;
      set
      {
        if (this.string_0 == value)
          return;
        this.string_0 = value;
        this.method_0();
        this.method_1("");
      }
    }

    public string GroupName { get; private set; }

    public string Error { get; private set; }

    public bool HasNoError => string.IsNullOrWhiteSpace(this.Error);

    private void method_0()
    {
      if (string.IsNullOrWhiteSpace(this.ConfigName))
        this.Error = "Profile name cannot be empty.";
      else if (SettingsDialogNewSetWindow.NewSettingsGroupViewModel.smethod_0(this.ConfigName).Length > 0)
        this.Error = $"Profile name cannot contain '{SettingsDialogNewSetWindow.NewSettingsGroupViewModel.smethod_0(this.ConfigName)}'.";
      else if (Directory.Exists(Path.Combine(ComponentSettings.SettingsDirectoryRoot, this.GroupName, this.ConfigName.Trim())))
        this.Error = "Settings profile already exists.";
      else
        this.Error = "";
    }

    private static string smethod_0(string string_3)
    {
      return new string(string_3.Where<char>(new Func<char, bool>(SettingsDialogNewSetWindow.NewSettingsGroupViewModel.hashSet_0.Contains)).Distinct<char>().ToArray<char>());
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void method_1(string string_3)
    {
      // ISSUE: reference to a compiler-generated field
      PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
      if (changedEventHandler0 == null)
        return;
      changedEventHandler0((object) this, new PropertyChangedEventArgs(string_3));
    }
  }
}
