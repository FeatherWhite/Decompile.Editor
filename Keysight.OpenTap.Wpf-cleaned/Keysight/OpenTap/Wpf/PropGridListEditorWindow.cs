// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropGridListEditorWindow
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Microsoft.Win32;
using OpenTap;
using OpenTap.Plugins.BasicSteps;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropGridListEditorWindow : WslDialog, IComponentConnector
{
  private readonly AnnotationCollection annotations;
  private Utils.ObjectState objectState_0;
  public bool CanCancel;
  private static TraceSource traceSource_0 = Log.CreateSource("List Editor");
  internal PropGridListEditorWindow This;
  internal Menu menu;
  internal PropertyDataGrid dataGrid;
  internal Button addRow;
  internal StackPanel okCancelArea;
  internal Button OkButton;
  internal Button CancelButton;
  private bool bool_0;

  public PropGridListEditorWindow(object object_0)
  {
    this.annotations = AnnotationCollection.Annotate(object_0, Array.Empty<IAnnotation>());
    try
    {
      this.objectState_0 = this.annotations.SaveState();
    }
    catch
    {
    }
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.PropGridListEditorWindow_Loaded);
    this.Closing += new CancelEventHandler(this.PropGridListEditorWindow_Closing);
    IDisplayAnnotation idisplayAnnotation = this.annotations.Get<IDisplayAnnotation>(false, (object) null);
    string str;
    if (idisplayAnnotation == null)
    {
      str = (string) null;
    }
    else
    {
      str = idisplayAnnotation.Name;
      if (str != null)
        goto label_7;
    }
    str = "User Input Request";
label_7:
    this.Title = str;
    this.ToolTip = (object) this.annotations.Get<IDisplayAnnotation>(false, (object) null)?.Description;
    IReflectionAnnotation ireflectionAnnotation = this.annotations.Get<IReflectionAnnotation>(false, (object) null);
    int num;
    if (ireflectionAnnotation == null)
    {
      num = 0;
    }
    else
    {
      ITypeData reflectionInfo = ireflectionAnnotation.ReflectionInfo;
      bool? nullable1;
      if (reflectionInfo == null)
      {
        nullable1 = new bool?();
      }
      else
      {
        IEnumerable<object> attributes = ((IReflectionData) reflectionInfo).Attributes;
        nullable1 = attributes != null ? new bool?(attributes.OfType<ImportExportExcludeAttribute>().Any<ImportExportExcludeAttribute>()) : new bool?();
      }
      bool? nullable2 = nullable1;
      num = nullable2.GetValueOrDefault() & nullable2.HasValue ? 1 : 0;
    }
    if (num != 0)
      this.menu.Visibility = Visibility.Collapsed;
    this.okCancelArea.Visibility = this.objectState_0 != null ? Visibility.Visible : Visibility.Collapsed;
  }

  public PropGridListEditorWindow(AnnotationCollection annotations)
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.PropGridListEditorWindow_Loaded);
    this.Closing += new CancelEventHandler(this.PropGridListEditorWindow_Closing);
    this.annotations = annotations;
  }

  private void PropGridListEditorWindow_Closing(object sender, CancelEventArgs e)
  {
    if (this.dataGrid.ItemsSource == null || this.Title == null)
      return;
    ComponentSettings<GuiControlsSettings>.Current.DialogSize[this.Title] = new TapSize()
    {
      Width = this.Width,
      Height = this.Height
    };
  }

  private void PropGridListEditorWindow_Loaded(object sender, RoutedEventArgs e) => this.method_0();

  public void OnListChanged(IList oldValue, IList newValue)
  {
    if (newValue == null)
      return;
    this.method_0();
  }

  private void method_0()
  {
    if (this.annotations.Get<UpdateMonitor>(true, (object) null) == null)
      this.annotations.Add((IAnnotation) new UpdateMonitor());
    this.dataGrid.LoadData(this.annotations);
    string title = this.Title;
    if (title == null || !ComponentSettings<GuiControlsSettings>.Current.DialogSize.ContainsKey(title))
      return;
    TapSize tapSize = ComponentSettings<GuiControlsSettings>.Current.DialogSize[title];
    if (tapSize == null)
      return;
    this.Width = tapSize.Width;
    this.Height = tapSize.Height;
  }

  public event EventHandler<EventArgs> PropertyEdited;

  protected void onEdited()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 != null)
    {
      // ISSUE: reference to a compiler-generated field
      this.eventHandler_0((object) this, EventArgs.Empty);
    }
    this.annotations.Write();
    this.annotations.Read();
    this.annotations.Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
  }

  private void dataGrid_ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.onEdited();
  }

  private List<T> method_1<T>()
  {
    IOrderedEnumerable<Type> orderedEnumerable = PluginManager.GetPlugins<T>().OrderByDescending<Type, double>((Func<Type, double>) (type_0 => type_0.GetDisplayAttribute().Order));
    List<T> objList = new List<T>();
    foreach (Type type in (IEnumerable<Type>) orderedEnumerable)
      objList.Add((T) Activator.CreateInstance(type));
    return objList;
  }

  private void method_2(object sender, RoutedEventArgs e)
  {
    this.dataGrid.CancelEdit();
    this.dataGrid.CancelEdit();
    List<ITableImport> list = this.method_1<ITableImport>().GroupBy<ITableImport, string>((Func<ITableImport, string>) (itableImport_0 => itableImport_0.Extension)).Select<IGrouping<string, ITableImport>, ITableImport>((Func<IGrouping<string, ITableImport>, ITableImport>) (igrouping_0 => igrouping_0.First<ITableImport>())).ToList<ITableImport>();
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = string.Join("|", list.Select<ITableImport, string>((Func<ITableImport, string>) (itableImport_0 => string.Format("{0} (*{1})|*{1}", (object) itableImport_0.Name, (object) itableImport_0.Extension))));
    openFileDialog1.FilterIndex = 1;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    bool? nullable = openFileDialog2.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return;
    Stopwatch stopwatch = Stopwatch.StartNew();
    string fileName = openFileDialog2.FileName;
    try
    {
      string[][] values = list[openFileDialog2.FilterIndex - 1].ImportTableValues(fileName);
      if (new TableView(this.annotations).SetMatrix(values, (ITypeData[]) null, true))
      {
        this.dataGrid.LoadData(this.annotations);
        new TableView(this.annotations).SetMatrix(values, (ITypeData[]) null, false);
      }
      this.annotations.Write();
      this.dataGrid.LoadData(this.annotations);
    }
    catch (Exception ex)
    {
      this.annotations.Read();
      Log.Error(PropGridListEditorWindow.traceSource_0, "Error while importing '{1}': {0}", new object[2]
      {
        (object) ex.Message,
        (object) fileName
      });
      return;
    }
    Log.Info(PropGridListEditorWindow.traceSource_0, stopwatch, "Values imported successfully from {0}", new object[1]
    {
      (object) fileName
    });
  }

  private void method_3(object sender, RoutedEventArgs e)
  {
    List<ITableExport> list = this.method_1<ITableExport>().GroupBy<ITableExport, string>((Func<ITableExport, string>) (itableExport_0 => itableExport_0.Extension)).Select<IGrouping<string, ITableExport>, ITableExport>((Func<IGrouping<string, ITableExport>, ITableExport>) (igrouping_0 => igrouping_0.First<ITableExport>())).ToList<ITableExport>();
    IDisplayAnnotation idisplayAnnotation = this.annotations.Get<IDisplayAnnotation>(false, (object) null);
    string str;
    if (idisplayAnnotation == null)
    {
      str = (string) null;
    }
    else
    {
      str = idisplayAnnotation.Name;
      if (str != null)
        goto label_4;
    }
    str = this.annotations.ToString();
label_4:
    string fileName1 = str;
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.Filter = string.Join("|", list.Select<ITableExport, string>((Func<ITableExport, string>) (itableExport_0 => string.Format("{0} (*{1})|*{1}", (object) itableExport_0.Name, (object) itableExport_0.Extension))));
    saveFileDialog1.FilterIndex = 1;
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (PathUtils.IsValidFileName(fileName1))
      saveFileDialog2.FileName = fileName1 + ".csv";
    bool? nullable = saveFileDialog2.ShowDialog();
    if (!(nullable.GetValueOrDefault() & nullable.HasValue))
      return;
    Stopwatch stopwatch = Stopwatch.StartNew();
    string fileName2 = saveFileDialog2.FileName;
    try
    {
      string[][] matrix = new TableView(this.annotations).GetMatrix();
      list[saveFileDialog2.FilterIndex - 1].ExportTableValues(matrix, fileName2);
    }
    catch (Exception ex)
    {
      Log.Error(PropGridListEditorWindow.traceSource_0, "{0}: {1}", new object[2]
      {
        (object) ex.Message,
        (object) fileName2
      });
      return;
    }
    Log.Info(PropGridListEditorWindow.traceSource_0, stopwatch, "Values exported successfully to {0}", new object[1]
    {
      (object) fileName2
    });
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.annotations.LoadState(this.objectState_0);
    this.annotations.Write();
    this.annotations.Read();
    this.Close();
  }

  private void OkButton_Click(object sender, RoutedEventArgs e)
  {
    this.annotations.Write();
    this.Close();
  }

  private void method_4(object sender, EventArgs e) => this.onEdited();

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/propgridlisteditorwindow.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.This = (PropGridListEditorWindow) target;
        break;
      case 2:
        this.menu = (Menu) target;
        break;
      case 3:
        ((MenuItem) target).Click += new RoutedEventHandler(this.method_2);
        break;
      case 4:
        ((MenuItem) target).Click += new RoutedEventHandler(this.method_3);
        break;
      case 5:
        this.dataGrid = (PropertyDataGrid) target;
        break;
      case 6:
        this.addRow = (Button) target;
        break;
      case 7:
        this.okCancelArea = (StackPanel) target;
        break;
      case 8:
        this.OkButton = (Button) target;
        this.OkButton.Click += new RoutedEventHandler(this.OkButton_Click);
        break;
      case 9:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
