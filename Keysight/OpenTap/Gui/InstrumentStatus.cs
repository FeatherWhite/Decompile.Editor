// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.InstrumentStatus
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class InstrumentStatus : 
  UserControl,
  INotifyPropertyChanged,
  IComponentConnector,
  IStyleConnector
{
  private List<IList> list_0 = new List<IList>();
  private static OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Resources");
  internal InstrumentStatus instrumentStatus_0;
  private bool bool_0;

  public List<IList> ResourceLists
  {
    get => this.list_0;
    set
    {
      this.list_0 = value;
      this.method_4(nameof (ResourceLists));
    }
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (FrameworkElement.DataContextProperty != dependencyPropertyChangedEventArgs_0.Property || this.settings == null)
      return;
    this.settings.PropertyChanged += (PropertyChangedEventHandler) new PropertyChangedEventHandler(this.method_1);
    this.method_0();
  }

  private void method_0()
  {
    List<IList> listList = new List<IList>();
    foreach (ComponentSettings allSetting in (IEnumerable<ComponentSettings>) this.settings.AllSettings)
    {
      if (allSetting is IList)
      {
        Type type1 = allSetting.GetType();
        try
        {
          Type type2 = type1;
          while (!type2.FullName.StartsWith(typeof (ComponentSettingsList).FullName + "`"))
            type2 = type2.BaseType;
          if (type2.GetGenericArguments()[1].smethod_12(typeof (IResource)))
            listList.Add((IList) allSetting);
        }
        catch (Exception ex)
        {
          InstrumentStatus.traceSource_0.smethod_27((object) type1, "Unable to load resource list for {0}: '{0}'", (object) type1.FullName, (object) ex.Message);
        }
      }
    }
    this.ResourceLists = listList;
  }

  private void method_1(object sender, PropertyChangedEventArgs e) => this.method_0();

  private TapSettings settings => this.DataContext as TapSettings;

  public ICommand OpenStoreCommand { get; set; }

  public InstrumentStatus()
  {
    this.OpenStoreCommand = (ICommand) new RelayCommand((Action<object>) (object_0 => this.method_2((IResultStore) object_0)));
    this.InitializeComponent();
  }

  private void method_2(IResultStore iresultStore_0)
  {
    RunExplorer.Open(iresultStore_0, Application.Current.MainWindow);
  }

  private void method_3(IFileResultStore ifileResultStore_0)
  {
    try
    {
      string[] strArray = ifileResultStore_0.FilePath.Replace('/', '\\').Split('\\');
      if (strArray.Length == 0)
        return;
      string str = Path.GetFullPath(strArray[0]);
      if (!Directory.Exists(str))
      {
        string directoryName = Path.GetDirectoryName(str);
        if (!Directory.Exists(directoryName))
          return;
        Process.Start(directoryName);
      }
      else
      {
        for (int index = 1; index < strArray.Length; ++index)
        {
          string path = Path.Combine(str, strArray[index]);
          if (Directory.Exists(path))
            str = path;
          else
            break;
        }
        Process.Start(str);
      }
    }
    catch (Exception ex)
    {
      InstrumentStatus.traceSource_0.Error("Could not open folder '{0}'. '{1}'", (object) ifileResultStore_0.FilePath, (object) ex.Message);
      InstrumentStatus.traceSource_0.Debug(ex);
    }
  }

  private void method_4(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_0));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void loadCommandParameter(object sender, RoutedEventArgs e)
  {
    Button depObj = (Button) sender;
    ItemsControl parent = GuiHelpers.FindParent<ItemsControl>((DependencyObject) depObj);
    IList list = (IList) null;
    if (parent != null)
      list = (IList) parent.DataContext;
    depObj.CommandParameter = (object) new TapSettings.ResourceListSet()
    {
      Item = depObj.DataContext,
      List = (IList) list
    };
  }

  private void method_5(object sender, RoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    InstrumentStatus.Class77 class77 = new InstrumentStatus.Class77();
    // ISSUE: reference to a compiler-generated field
    class77.instrumentStatus_0 = this;
    // ISSUE: reference to a compiler-generated field
    class77.contextMenu_0 = sender as ContextMenu;
    // ISSUE: reference to a compiler-generated method
    Func<string, Action, MenuItem> func = new Func<string, Action, MenuItem>(class77.method_0);
    // ISSUE: reference to a compiler-generated method
    func("Configure", new Action(class77.method_1)).FontWeight = FontWeights.Bold;
    Window.GetWindow((DependencyObject) this);
    // ISSUE: reference to a compiler-generated field
    if (class77.contextMenu_0.DataContext is IResultStore)
    {
      // ISSUE: reference to a compiler-generated method
      MenuItem menuItem1 = func("Open", new Action(class77.method_2));
    }
    // ISSUE: reference to a compiler-generated field
    if (!(class77.contextMenu_0.DataContext is IFileResultStore))
      return;
    // ISSUE: reference to a compiler-generated method
    MenuItem menuItem2 = func("Open Folder", new Action(class77.method_3));
  }

  private void method_6(object sender, RoutedEventArgs e) => (sender as ContextMenu).Items.Clear();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/instrumentstatus.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      this.instrumentStatus_0 = (InstrumentStatus) target;
    else
      this.bool_0 = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 2)
      return;
    ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.method_5);
    ((FrameworkElement) target).Unloaded += new RoutedEventHandler(this.method_6);
  }
}
