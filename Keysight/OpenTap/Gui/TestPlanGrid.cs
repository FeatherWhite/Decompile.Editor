// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestPlanGrid
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.Ccl.Wsl.UI.Interfaces;
using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Wpf;
using OpenTap;
using OpenTap.Plugins;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class TestPlanGrid : UserControl, INotifyPropertyChanged, IComponentConnector, IStyleConnector
{
  private static readonly OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Main");
  public static readonly DependencyProperty TestPlanProperty = DependencyProperty.Register(nameof (TestPlan), typeof (TestPlan), typeof (TestPlanGrid), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(TestPlanGrid.smethod_0)));
  public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof (ReadOnly), typeof (bool), typeof (TestPlanGrid));
  public static readonly DependencyProperty ColumnVisibilityProperty = DependencyProperty.Register(nameof (ColumnVisibility), typeof (ObservableCollection<ColumnVisibilityDetails>), typeof (TestPlanGrid));
  public static readonly DependencyProperty ScrollToPlayingProperty = DependencyProperty.Register(nameof (ScrollToPlaying), typeof (bool), typeof (TestPlanGrid));
  public static readonly DependencyProperty FilterModeProperty = DependencyProperty.Register(nameof (FilterMode), typeof (bool), typeof (TestPlanGrid));
  public static readonly DependencyProperty TotalStepCountProperty = DependencyProperty.Register(nameof (TotalStepCount), typeof (int), typeof (TestPlanGrid));
  public static readonly DependencyProperty VisibleStepCountProperty = DependencyProperty.Register(nameof (VisibleStepCount), typeof (int), typeof (TestPlanGrid));
  public static readonly DependencyProperty FilterSettingsChangedProperty = DependencyProperty.Register(nameof (FilterSettingsChanged), typeof (bool), typeof (TestPlanGrid));
  public readonly List<TestStepRowItem> flattenedRowItems = new List<TestStepRowItem>();
  private readonly ObservableCollection<TestStepRowItem> observableCollection_0 = new ObservableCollection<TestStepRowItem>();
  public static readonly RoutedUICommand Paste = new RoutedUICommand(nameof (Paste), nameof (Paste), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.V, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand Copy = new RoutedUICommand(nameof (Copy), nameof (Copy), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.C, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand ExpandAll = new RoutedUICommand("Expand All", nameof (ExpandAll), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.E, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand CollapseAll = new RoutedUICommand("Collapse All", nameof (CollapseAll), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift)
  });
  public static readonly RoutedUICommand ToggleBreakpoint = new RoutedUICommand("Toggle Breakpoint", nameof (ToggleBreakpoint), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F9)
  });
  public static readonly RoutedUICommand JumpTo = new RoutedUICommand("Jump To Step", nameof (JumpTo), typeof (TestPlanGrid));
  public static readonly RoutedUICommand SelectAll = new RoutedUICommand("Select All", nameof (SelectAll), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.A, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand SelectTop = new RoutedUICommand("Select Top", nameof (SelectTop), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Home)
  });
  public static readonly RoutedUICommand SelectBottom = new RoutedUICommand("Select Bottom", nameof (SelectBottom), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.End)
  });
  public static readonly RoutedUICommand SelectAllAbove = new RoutedUICommand("Select All Above", nameof (SelectAllAbove), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Home, ModifierKeys.Control | ModifierKeys.Shift),
    (InputGesture) new KeyGesture(Key.Home, ModifierKeys.Shift)
  });
  public static readonly RoutedUICommand SelectAllBelow = new RoutedUICommand("Select All Below", nameof (SelectAllBelow), typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.End, ModifierKeys.Control | ModifierKeys.Shift),
    (InputGesture) new KeyGesture(Key.End, ModifierKeys.Shift)
  });
  public static readonly RoutedCommand ShowStepHelp = (RoutedCommand) new RoutedUICommand("Show Help", "Show Step Help", typeof (TestPlanGrid));
  public static readonly RoutedCommand ToggleTestStep = (RoutedCommand) new RoutedUICommand("Toggle Test Steps", "Toggle Test Steps", typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Space)
  });
  public static readonly RoutedCommand ToggleAllTestSteps = (RoutedCommand) new RoutedUICommand("Toggle All Test Steps", "Toggle All Test Steps", typeof (TestPlanGrid));
  public static readonly RoutedCommand FilterModeToggle = (RoutedCommand) new RoutedUICommand("Toggle Filter Mode", "Toggle Filter Mode", typeof (TestPlanGrid), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Shift)
  });
  private bool bool_0;
  private readonly Class76<TestStepRowItem> class76_0;
  private CollectionViewSource collectionViewSource_0;
  private ColumnFilterContext columnFilterContext_0 = new ColumnFilterContext();
  private readonly HashSet<ColumnViewProvider> hashSet_0 = new HashSet<ColumnViewProvider>();
  public readonly HashSet<Guid> BreakPointTestSteps = new HashSet<Guid>();
  private bool bool_1;
  private bool bool_2;
  private ITestStep itestStep_0;
  private bool bool_3;
  private bool bool_4;
  private TestPlanGrid.Class151 class151_0;
  private bool bool_5;
  private TestPlanGrid.Class153 class153_0;
  private DispatcherTimer dispatcherTimer_0;
  private double double_0;
  public bool IsEditing;
  public Action<Guid> jumpToStepAction;
  private Guid[] guid_0;
  private DispatcherTimer dispatcherTimer_1;
  private bool bool_6;
  internal TestPlanGrid testPlanGrid_0;
  internal Canvas canvas_0;
  internal Canvas canvas_1;
  internal Canvas canvas_2;
  internal Canvas canvas_3;
  internal Border border_0;
  internal TextBlock textBlock_0;
  internal MenuItem menuItem_0;
  internal ToggleButton toggleButton_0;
  internal ToggleButton toggleButton_1;
  internal DataGrid dataGrid_0;
  internal DataGridTemplateColumn dataGridTemplateColumn_0;
  private bool bool_7;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as TestPlanGrid).OnTestPlanChanged((TestPlan) dependencyPropertyChangedEventArgs_0.OldValue, (TestPlan) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  public TestPlan TestPlan
  {
    get => (TestPlan) this.GetValue(TestPlanGrid.TestPlanProperty);
    set => this.SetValue(TestPlanGrid.TestPlanProperty, (object) value);
  }

  public IEnumerable<ITestStep> SelectedTestSteps
  {
    get
    {
      return (IEnumerable<ITestStep>) this.flattenedRowItems.Where<TestStepRowItem>(new Func<TestStepRowItem, bool>(this.dataGrid_0.SelectedItems.OfType<TestStepRowItem>().ToHashSet<TestStepRowItem>().Contains)).Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)).ToArray<ITestStep>();
    }
    private set
    {
      this.bool_3 = true;
      try
      {
        HashSet<TestStepRowItem> hashSet = this.GetRowItems(value).ToHashSet<TestStepRowItem>();
        if (this.dataGrid_0.SelectedItems.OfType<TestStepRowItem>().ToHashSet<TestStepRowItem>().SetEquals((IEnumerable<TestStepRowItem>) hashSet))
          return;
        this.dataGrid_0.SetSelectedItems((IEnumerable) hashSet);
        if (this.dataGrid_0.IsKeyboardFocusWithin)
          this.method_11(this.dataGrid_0.SelectedItem as TestStepRowItem, this.method_10());
        this.bool_3 = false;
        this.dataGrid_0_SelectionChanged((object) null, (SelectionChangedEventArgs) null);
      }
      finally
      {
        this.bool_3 = false;
      }
    }
  }

  public ITestStep SelectedTestStep => this.SelectedTestSteps.FirstOrDefault<ITestStep>();

  public bool ReadOnly
  {
    get => (bool) this.GetValue(TestPlanGrid.ReadOnlyProperty);
    set => this.SetValue(TestPlanGrid.ReadOnlyProperty, (object) value);
  }

  public ObservableCollection<ColumnVisibilityDetails> ColumnVisibility
  {
    get
    {
      return (ObservableCollection<ColumnVisibilityDetails>) this.GetValue(TestPlanGrid.ColumnVisibilityProperty);
    }
    set => this.SetValue(TestPlanGrid.ColumnVisibilityProperty, (object) value);
  }

  public IEnumerable<TestPlanGrid.ExtraColumn> extraColumns
  {
    get
    {
      TestPlanGrid testPlanGrid = this;
      IMemberData[] array = ((IEnumerable<IMemberData>) ((IEnumerable<ITypeData>) testPlanGrid.flattenedRowItems.Take<TestStepRowItem>(1000).Select<TestStepRowItem, ITypeData>((Func<TestStepRowItem, ITypeData>) (testStepRowItem_0 => TypeData.GetTypeData((object) testStepRowItem_0.Step))).Distinct<ITypeData>().ToArray<ITypeData>()).SelectMany<ITypeData, IMemberData>((Func<ITypeData, IEnumerable<IMemberData>>) (itypeData_0 => itypeData_0.GetMembers())).Where<IMemberData>(new Func<IMemberData, bool>(GenericGui.FilterDefault2)).Where<IMemberData>(new Func<IMemberData, bool>(new TestPlanGrid.Class172()
      {
        hashSet_0 = testPlanGrid.ColumnVisibility.Select<ColumnVisibilityDetails, string>((Func<ColumnVisibilityDetails, string>) (columnVisibilityDetails_0 => columnVisibilityDetails_0.Name)).ToHashSet<string>()
      }.method_0)).ToArray<IMemberData>()).OrderBy<IMemberData, string>(new Func<IMemberData, string>(ColumnViewProvider.GetColumnName)).ToArray<IMemberData>();
      HashSet<string> stringSet = new HashSet<string>();
      IMemberData[] memberDataArray = array;
      for (int index = 0; index < memberDataArray.Length; ++index)
      {
        IMemberData member = memberDataArray[index];
        string columnName = ColumnViewProvider.GetColumnName(member);
        if (stringSet.Contains(columnName))
          continue;
        stringSet.Add(columnName);
        yield return new TestPlanGrid.ExtraColumn()
        {
          Name = columnName,
          IsSelected = false,
          Member = member,
          grid = testPlanGrid
        };
      }
      memberDataArray = (IMemberData[]) null;
    }
  }

  public IEnumerable<TestPlanGrid.ExtraColumn> ExtraColumns
  {
    get
    {
      return (IEnumerable<TestPlanGrid.ExtraColumn>) this.extraColumns.ToArray<TestPlanGrid.ExtraColumn>();
    }
  }

  public bool ScrollToPlaying
  {
    get => (bool) this.GetValue(TestPlanGrid.ScrollToPlayingProperty);
    set => this.SetValue(TestPlanGrid.ScrollToPlayingProperty, (object) value);
  }

  public bool FilterMode
  {
    get => (bool) this.GetValue(TestPlanGrid.FilterModeProperty);
    set => this.SetValue(TestPlanGrid.FilterModeProperty, (object) value);
  }

  public int TotalStepCount
  {
    get => (int) this.GetValue(TestPlanGrid.TotalStepCountProperty);
    set => this.SetValue(TestPlanGrid.TotalStepCountProperty, (object) value);
  }

  public int VisibleStepCount
  {
    get => (int) this.GetValue(TestPlanGrid.VisibleStepCountProperty);
    set => this.SetValue(TestPlanGrid.VisibleStepCountProperty, (object) value);
  }

  public bool FilterSettingsChanged
  {
    get => (bool) this.GetValue(TestPlanGrid.FilterSettingsChangedProperty);
    set => this.SetValue(TestPlanGrid.FilterSettingsChangedProperty, (object) value);
  }

  public event TestPlanGrid.TestPlanChangedDelegate TestPlanChanged;

  public event PropertyChangedEventHandler PropertyChanged;

  private void method_0(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_0));
  }

  private void method_1(object sender, EventArgs e)
  {
    this.method_2();
    if (this.bool_0)
      return;
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      this.bool_0 = true;
      try
      {
        this.dataGrid_0.CommitEdit(DataGridEditingUnit.Row, true);
      }
      finally
      {
        this.bool_0 = false;
      }
    }), priority: DispatcherPriority.ContextIdle);
  }

  private void method_2()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.testPlanChangedDelegate_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.testPlanChangedDelegate_0();
  }

  private void method_3(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.SelectedTestSteps != null && this.SelectedTestSteps.Any<ITestStep>();
  }

  public IEnumerable<TestStepRowItem> View
  {
    get => this.collectionViewSource_0.View.Cast<TestStepRowItem>();
  }

  public bool viewFilter(object object_0)
  {
    if (!(object_0 is TestStepRowItem testStepRowItem))
      return true;
    if (this.FilterMode)
      return this.columnFilterContext_0.IsVisible(testStepRowItem) == ColumnFilterContext.Visibility.Visible;
    return testStepRowItem.IsVisible;
  }

  public TestPlanGrid()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class173 class173 = new TestPlanGrid.Class173();
    this.ColumnVisibility = new ObservableCollection<ColumnVisibilityDetails>();
    this.InitializeComponent();
    double originalPriority = 0.0;
    foreach (DataGridColumn column in (Collection<DataGridColumn>) this.dataGrid_0.Columns)
    {
      ColumnVisibilityDetails visibilityDetails = new ColumnVisibilityDetails(column, column.Header.ToString(), originalPriority);
      this.ColumnVisibility.Add(visibilityDetails);
      if (visibilityDetails.Priority == originalPriority)
        visibilityDetails.Priority += 0.01;
      originalPriority = visibilityDetails.Priority;
      visibilityDetails.ManuallyConfigured = true;
    }
    // ISSUE: reference to a compiler-generated field
    class173.hashSet_0 = this.ColumnVisibility.Select<ColumnVisibilityDetails, string>((Func<ColumnVisibilityDetails, string>) (columnVisibilityDetails_0 => columnVisibilityDetails_0.Name)).ToHashSet<string>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    foreach (ColumnState columnState in ColumnVisibilityDetails.GetColumnStates().Where<ColumnState>(class173.func_0 ?? (class173.func_0 = new Func<ColumnState, bool>(class173.method_0))))
      this.method_6(new ColumnViewProvider()
      {
        Name = columnState.Name,
        Priority = columnState.Priority
      });
    List<object> testPlanColumnState = ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState;
    object obj;
    if (testPlanColumnState == null)
    {
      obj = (object) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      obj = (object) ((IEnumerable) testPlanColumnState).OfType<ColumnState>().Where<ColumnState>(new Func<ColumnState, bool>(class173.method_1));
      if (obj != null)
        goto label_20;
    }
    obj = (object) new ColumnState[0];
label_20:
    foreach (ColumnState columnState in (IEnumerable<ColumnState>) obj)
      this.method_6(new ColumnViewProvider()
      {
        Name = TestPlanGrid.smethod_1(columnState.Name),
        Priority = columnState.Priority
      });
    ComponentSettings<GuiControlsSettings>.Current.TestPlanColumnState = (List<object>) new List<object>();
    this.method_5((ITypeData) TypeData.FromType(typeof (TestStep)), true);
    this.collectionViewSource_0 = new CollectionViewSource()
    {
      Source = (object) this.flattenedRowItems
    };
    this.collectionViewSource_0.View.Filter = new Predicate<object>(this.viewFilter);
    this.dataGrid_0.ItemsSource = (IEnumerable) this.collectionViewSource_0.View;
    this.class76_0 = new Class76<TestStepRowItem>(this.flattenedRowItems, (IList<TestStepRowItem>) this.observableCollection_0, (Func<TestStepRowItem, IList<TestStepRowItem>>) (testStepRowItem_0 => (IList<TestStepRowItem>) testStepRowItem_0.VisibleChildItems));
    ColumnVisibilityDetails[] array = this.ColumnVisibility.OrderBy<ColumnVisibilityDetails, double>((Func<ColumnVisibilityDetails, double>) (columnVisibilityDetails_0 => columnVisibilityDetails_0.Priority)).ToArray<ColumnVisibilityDetails>();
    for (int index = 0; index < array.Length; ++index)
    {
      ColumnVisibilityDetails visibilityDetails = array[index];
      double gparam_1 = index != 0 ? array[index - 1].Priority + 0.001 : double.MinValue;
      double gparam_2 = index != array.Length - 1 ? array[index + 1].Priority - 0.001 : double.MaxValue;
      double priority = visibilityDetails.Priority;
      visibilityDetails.Priority = visibilityDetails.OriginalPriority.smethod_3<double>(gparam_1, gparam_2);
    }
    this.method_42();
    this.ScrollToPlaying = ComponentSettings<GuiControlsSettings>.Current.TestPlanGridScrollToPlaying;
    this.TestPlanChanged += new TestPlanGrid.TestPlanChangedDelegate(this.method_4);
  }

  private void method_4()
  {
    if (!this.dataGrid_0.IsLoaded)
      return;
    foreach (DataGridRow dataGridRow in ((IEnumerable) this.dataGrid_0.GetVisualChildren()).OfType<DataGridRow>())
    {
      object dataContext = dataGridRow.DataContext;
      dataGridRow.DataContext = (object) null;
      dataGridRow.DataContext = dataContext;
    }
  }

  private void method_5(ITypeData itypeData_0, bool bool_8 = false)
  {
    foreach (ColumnViewProvider columnViewProvider_0 in ColumnViewProvider.GetFor(itypeData_0))
      this.method_6(columnViewProvider_0, bool_8);
  }

  private void method_6(ColumnViewProvider columnViewProvider_0, bool bool_8 = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class174 class174 = new TestPlanGrid.Class174();
    // ISSUE: reference to a compiler-generated field
    class174.testPlanGrid_0 = this;
    // ISSUE: reference to a compiler-generated field
    class174.columnViewProvider_0 = columnViewProvider_0;
    // ISSUE: reference to a compiler-generated field
    if (this.hashSet_0.Contains(class174.columnViewProvider_0))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class174.propertyColumn_0 = new PropertyColumn(class174.columnViewProvider_0, this.columnFilterContext_0);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class174.propertyColumn_0.Filter.FilterChanged += new EventHandler<EventArgs>(class174.method_0);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class174.propertyColumn_0.Header = (object) class174.columnViewProvider_0.Name;
    // ISSUE: reference to a compiler-generated field
    this.hashSet_0.Add(class174.columnViewProvider_0);
    // ISSUE: reference to a compiler-generated field
    this.dataGrid_0.Columns.Add((DataGridColumn) class174.propertyColumn_0);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    class174.columnVisibilityDetails_0 = new ColumnVisibilityDetails((DataGridColumn) class174.propertyColumn_0, class174.columnViewProvider_0.Name, class174.columnViewProvider_0.Priority)
    {
      Priority = class174.columnViewProvider_0.Priority
    };
    if (bool_8)
    {
      // ISSUE: reference to a compiler-generated field
      class174.columnVisibilityDetails_0.ManuallyConfigured = true;
    }
    // ISSUE: reference to a compiler-generated field
    this.ColumnVisibility.Add(class174.columnVisibilityDetails_0);
    this.method_42();
    // ISSUE: reference to a compiler-generated field
    if (string.IsNullOrWhiteSpace(class174.columnViewProvider_0.Name))
    {
      // ISSUE: reference to a compiler-generated field
      class174.propertyColumn_0.CanUserResize = false;
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class174.columnVisibilityDetails_0.PropertyChanged += new PropertyChangedEventHandler(class174.method_1);
  }

  public ITestStep AddStepToPlan(ITypeData stepType, ITestStepParent relativeTo, bool child)
  {
    Action action_0;
    ITestStep itestStep_0 = this.method_28(stepType, out action_0);
    if (itestStep_0 == null)
      return (ITestStep) null;
    using (this.delayUpdate())
    {
      Class179.smethod_2(itestStep_0, relativeTo ?? (ITestStepParent) this.TestPlan, child);
      action_0();
    }
    return itestStep_0;
  }

  public void RemoveSteps(IEnumerable<ITestStep> steps)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class175 class175 = new TestPlanGrid.Class175();
    // ISSUE: reference to a compiler-generated field
    class175.testStepRowItem_0 = this.dataGrid_0.SelectedItems.Cast<TestStepRowItem>().ToArray<TestStepRowItem>();
    int num1 = 0;
    // ISSUE: reference to a compiler-generated field
    HashSet<TestStepRowItem> hashSet = ((IEnumerable<TestStepRowItem>) class175.testStepRowItem_0).ToHashSet<TestStepRowItem>();
    ItemCollection items = this.dataGrid_0.Items;
    for (int index = 0; index < items.Count; ++index)
    {
      if (hashSet.Contains((TestStepRowItem) items[index]))
        num1 = index;
    }
    int num2 = num1;
    this.SetSelectedItems(Enumerable.Empty<ITestStep>());
    int int_0 = this.method_10();
    using (this.delayUpdate())
      this.TestPlan.ChildTestSteps.RemoveItems(steps);
    this.method_23();
    this.dataGrid_0_SelectionChanged((object) null, (SelectionChangedEventArgs) null);
    // ISSUE: reference to a compiler-generated field
    TestStepRowItem testStepRowItem_0 = this.dataGrid_0.Items.Cast<TestStepRowItem>().ElementAtOrDefault<TestStepRowItem>(num2 + 1 - class175.testStepRowItem_0.Length);
    if (testStepRowItem_0 != null)
    {
      while (!testStepRowItem_0.IsVisible && testStepRowItem_0.Parent != null)
        testStepRowItem_0 = testStepRowItem_0.Parent;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      testStepRowItem_0 = this.dataGrid_0.Items.Cast<TestStepRowItem>().LastOrDefault<TestStepRowItem>(new Func<TestStepRowItem, bool>(class175.method_0));
    }
    this.method_2();
    this.dataGrid_0.Focus();
    if (testStepRowItem_0 == null)
      return;
    this.SetSelectedItems((IEnumerable<ITestStep>) new ITestStep[1]
    {
      testStepRowItem_0.Step
    }, false);
    if (int_0 <= -1)
      return;
    this.method_11(testStepRowItem_0, int_0);
  }

  private void method_7(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.flattenedRowItems.Any<TestStepRowItem>();
  }

  private void method_8(object sender, ExecutedRoutedEventArgs e)
  {
    this.SetSelectedItems((IEnumerable<ITestStep>) this.dataGrid_0.Items.OfType<TestStepRowItem>().Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)).ToArray<ITestStep>(), false);
  }

  private void method_9(object sender, ExecutedRoutedEventArgs e)
  {
    int int_0 = this.method_10();
    TestStepRowItem testStepRowItem_0_1 = (TestStepRowItem) null;
    switch ((e.Command as RoutedUICommand).Name)
    {
      case "SelectTop":
        testStepRowItem_0_1 = this.flattenedRowItems.FirstOrDefault<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsVisible));
        if (testStepRowItem_0_1 == null)
          return;
        this.SetSelectedItem(testStepRowItem_0_1.Step);
        break;
      case "SelectBottom":
        testStepRowItem_0_1 = this.flattenedRowItems.LastOrDefault<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsVisible));
        if (testStepRowItem_0_1 == null)
          return;
        this.SetSelectedItem(testStepRowItem_0_1.Step);
        break;
      case "SelectAllAbove":
        if (this.dataGrid_0.SelectedIndex == -1)
          return;
        IEnumerable<TestStepRowItem> source1 = this.flattenedRowItems.Take<TestStepRowItem>(this.flattenedRowItems.smethod_18<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.Step == (this.dataGrid_0.SelectedItem as TestStepRowItem).Step)) + 1);
        this.dataGrid_0.SelectedIndex = 0;
        this.SetSelectedItems(source1.Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)), false);
        testStepRowItem_0_1 = source1.FirstOrDefault<TestStepRowItem>();
        break;
      case "SelectAllBelow":
        if (this.dataGrid_0.SelectedIndex == -1)
          return;
        IEnumerable<TestStepRowItem> source2 = this.flattenedRowItems.Skip<TestStepRowItem>(this.flattenedRowItems.smethod_18<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.Step == (this.dataGrid_0.SelectedItem as TestStepRowItem).Step)));
        this.dataGrid_0.SelectedIndex = this.flattenedRowItems.Count - 1;
        this.SetSelectedItems(source2.Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)), false);
        testStepRowItem_0_1 = source2.LastOrDefault<TestStepRowItem>();
        break;
    }
    this.dataGrid_0.ScrollIntoView((object) testStepRowItem_0_1);
    if (testStepRowItem_0_1 == null)
      return;
    this.method_11(testStepRowItem_0_1, int_0);
  }

  private int method_10()
  {
    if (!(Keyboard.FocusedElement is DataGridCell))
      return 0;
    DataGridCellsPresenter parent = GuiHelpers.FindParent<DataGridCellsPresenter>((DependencyObject) (Keyboard.FocusedElement as DataGridCell));
    return parent == null ? 0 : parent.ItemContainerGenerator.IndexFromContainer((DependencyObject) (Keyboard.FocusedElement as DataGridCell));
  }

  private void method_11(TestStepRowItem testStepRowItem_0, int int_0, int int_1 = 0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class176 class176 = new TestPlanGrid.Class176();
    // ISSUE: reference to a compiler-generated field
    class176.testPlanGrid_0 = this;
    // ISSUE: reference to a compiler-generated field
    class176.testStepRowItem_0 = testStepRowItem_0;
    // ISSUE: reference to a compiler-generated field
    class176.int_0 = int_0;
    // ISSUE: reference to a compiler-generated field
    class176.int_1 = int_1;
    // ISSUE: reference to a compiler-generated field
    if (class176.int_1 > 4)
      return;
    // ISSUE: reference to a compiler-generated field
    DataGridRow dpObj = (DataGridRow) this.dataGrid_0.ItemContainerGenerator.ContainerFromItem((object) class176.testStepRowItem_0);
    if (dpObj == null)
    {
      // ISSUE: reference to a compiler-generated method
      this.dataGrid_0.LayoutUpdated += new EventHandler(class176.method_0);
    }
    else
    {
      DataGridCellsPresenter gridCellsPresenter = ((IEnumerable) dpObj.GetVisualChildren()).OfType<DataGridCellsPresenter>().FirstOrDefault<DataGridCellsPresenter>();
      if (gridCellsPresenter == null)
      {
        this.dataGrid_0.ScrollIntoView((object) dpObj);
        gridCellsPresenter = ((IEnumerable) dpObj.GetVisualChildren()).OfType<DataGridCellsPresenter>().FirstOrDefault<DataGridCellsPresenter>();
        if (gridCellsPresenter == null)
        {
          // ISSUE: reference to a compiler-generated method
          this.dataGrid_0.LayoutUpdated += new EventHandler(class176.method_1);
          return;
        }
      }
      // ISSUE: reference to a compiler-generated field
      Keyboard.Focus((IInputElement) gridCellsPresenter.ItemContainerGenerator.ContainerFromIndex(class176.int_0));
    }
  }

  private void method_12(bool bool_8)
  {
    this.dataGrid_0.ItemsSource = (IEnumerable) null;
    try
    {
      foreach (TestStepRowItem flattenedRowItem in this.flattenedRowItems)
        flattenedRowItem.IsExpanded = bool_8;
    }
    finally
    {
      this.dataGrid_0.ItemsSource = (IEnumerable) this.collectionViewSource_0.View;
    }
    this.updateView();
  }

  private void method_13(object sender, RoutedEventArgs e) => this.method_12(true);

  private void method_14(object sender, RoutedEventArgs e) => this.method_12(false);

  private IEnumerable<TestStepRowItem> method_15(object object_0, RoutedEventArgs routedEventArgs_0)
  {
    if (!(routedEventArgs_0.OriginalSource is DataGridRow))
      return this.dataGrid_0.SelectedItems.Cast<TestStepRowItem>();
    return (IEnumerable<TestStepRowItem>) new TestStepRowItem[1]
    {
      (TestStepRowItem) ((FrameworkElement) routedEventArgs_0.OriginalSource).DataContext
    };
  }

  private void method_16()
  {
    if (this.BreakPointTestSteps.Count == 0)
      return;
    foreach (TestStepRowItem flattenedRowItem in this.flattenedRowItems)
      flattenedRowItem.IsBreakpoint = this.BreakPointTestSteps.Contains(flattenedRowItem.Step.Id);
  }

  private void method_17(object sender, RoutedEventArgs e)
  {
    IEnumerable<TestStepRowItem> source = this.method_15(sender, e);
    TestStepRowItem testStepRowItem1 = source.FirstOrDefault<TestStepRowItem>();
    bool flag = false;
    if (testStepRowItem1 != null)
      flag = !testStepRowItem1.IsBreakpoint;
    foreach (TestStepRowItem testStepRowItem2 in source)
    {
      testStepRowItem2.IsBreakpoint = flag;
      if (testStepRowItem2.IsBreakpoint)
        this.BreakPointTestSteps.Add(testStepRowItem2.Step.Id);
      else
        this.BreakPointTestSteps.Remove(testStepRowItem2.Step.Id);
    }
    e.Handled = true;
    this.dataGrid_0.Focus();
  }

  private void method_18(object sender, RoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class177 class177 = new TestPlanGrid.Class177();
    List<ITestStep> list = this.SelectedTestSteps.ToList<ITestStep>();
    // ISSUE: reference to a compiler-generated field
    class177.hashSet_0 = Class24.smethod_13<ITestStep>(list.SelectMany<ITestStep, ITestStep>((Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)), (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).ToHashSet<ITestStep>();
    // ISSUE: reference to a compiler-generated method
    Class51.smethod_0(new TapSerializer().SerializeToString((object) list.Where<ITestStep>(new Func<ITestStep, bool>(class177.method_0)).ToArray<ITestStep>()));
  }

  private ITestStepParent method_19(ITestStepParent itestStepParent_0)
  {
    if (itestStepParent_0 is TestPlan)
      return (ITestStepParent) null;
    TestStepRowItem testStepRowItem = this.method_21((ITestStep) itestStepParent_0);
    if (testStepRowItem == null)
      return (ITestStepParent) null;
    return testStepRowItem.Parent == null ? (ITestStepParent) this.TestPlan : (ITestStepParent) testStepRowItem.Parent.Step;
  }

  public void DoPaste(string text = null)
  {
    if (this.ReadOnly)
      return;
    if (this.TestPlan.Locked)
      TestPlanGrid.traceSource_0.Info("Cannot paste steps into locked test plan.");
    else if (this.TestPlan.IsRunning)
      TestPlanGrid.traceSource_0.Info("Cannot paste steps into running test plan.");
    else if (this.FilterMode)
    {
      TestPlanGrid.traceSource_0.Info("Cannot paste steps while in filter mode.");
    }
    else
    {
      try
      {
        if (text == null)
          text = Clipboard.GetText();
        TapSerializer tapSerializer = new TapSerializer();
        tapSerializer.GetSerializer<TestStepSerializer>().AddKnownStepHeirarchy((ITestStepParent) this.TestPlan);
        object ienumerable_0 = tapSerializer.DeserializeFromString(text, path: this.TestPlan.Path) ?? tapSerializer.DeserializeFromString(text, (ITypeData) TypeData.FromType(typeof (TestPlan)), path: this.TestPlan.Path);
        TestPlan testPlan = ienumerable_0 as TestPlan;
        TestStepList testStepList;
        if (testPlan != null)
        {
          testStepList = testPlan.ChildTestSteps;
        }
        else
        {
          switch (ienumerable_0)
          {
            case ITestStep _:
              testStepList = new TestStepList()
              {
                EnforceNestingRulesOnInsert = false
              };
              testStepList.Add((ITestStep) ienumerable_0);
              break;
            case IEnumerable<ITestStep> _:
              testStepList = new TestStepList()
              {
                EnforceNestingRulesOnInsert = false
              };
              testStepList.smethod_20<ITestStep>((IEnumerable<ITestStep>) ienumerable_0);
              break;
            default:
              TestPlanGrid.traceSource_0.Info("Cannot deserialize plan.");
              return;
          }
        }
        ITestStep step = this.GetRowItems(this.SelectedTestSteps).LastOrDefault<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsVisible))?.Step;
        ITestStepParent testStepParent = step == null ? (ITestStepParent) this.TestPlan : this.method_19((ITestStepParent) step);
        TestStepList childTestSteps = testStepParent.ChildTestSteps;
        if (childTestSteps.IsReadOnly)
        {
          TestPlanGrid.traceSource_0.Info("Cannot paste steps into a read-only step or test plan.");
          return;
        }
        int num = step == null ? childTestSteps.Count : childTestSteps.IndexOf(step) + 1;
        try
        {
          if (ComponentSettings<EditorSettings>.Current.EnsureUniqueStepName)
            Class179.smethod_1((IList<ITestStep>) this.TestPlan.ChildTestSteps, testStepList.RecursivelyGetAllTestSteps(TestStepSearch.All));
          foreach (ITestStep testStep1 in (Collection<ITestStep>) testStepList)
          {
            Type type = testStepParent.GetType();
            ITestStep testStep2 = testStep1;
            if (!TestStepList.AllowChild(type, testStep2.GetType()))
            {
              if (testStepParent is TestPlan)
              {
                TestPlanGrid.traceSource_0.Error("Cannot add a step of type {0} as a root step in the test plan.", (object) testStep2.GetType().Name);
                return;
              }
              TestPlanGrid.traceSource_0.Error("Cannot add a step of type {0} to a {1}", (object) testStep2.GetType().Name, (object) type.Name);
              return;
            }
          }
          using (this.delayUpdate())
          {
            foreach (ITestStep testStep in (Collection<ITestStep>) testStepList)
              childTestSteps.Insert(num++, testStep);
          }
          IEnumerable<ITestStep> allTestSteps = testStepList.RecursivelyGetAllTestSteps(TestStepSearch.All);
          if (testPlan != null)
          {
            foreach (ITestStep testStep in allTestSteps)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              TestPlanGrid.Class156 class156 = new TestPlanGrid.Class156();
              // ISSUE: reference to a compiler-generated field
              class156.itestStep_0 = testStep;
              // ISSUE: reference to a compiler-generated method
              foreach (ExternalParameter externalParameter in testPlan.ExternalParameters.Entries.Where<ExternalParameter>(new Func<ExternalParameter, bool>(class156.method_0)))
              {
                // ISSUE: reference to a compiler-generated field
                foreach (IMemberData property in externalParameter.GetProperties(class156.itestStep_0))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.TestPlan.ExternalParameters.Add(class156.itestStep_0, property, externalParameter.Name);
                }
              }
            }
          }
          else
          {
            foreach (ExternalParameterSerializer.ExternalParamData externalParamData in tapSerializer.GetSerializer<ExternalParameterSerializer>().UnusedExternalParamData)
              this.TestPlan.ExternalParameters.Add(externalParamData.Object, externalParamData.Property, externalParamData.Name);
          }
          this.SetSelectedItems((IEnumerable<ITestStep>) testStepList, false);
        }
        catch (ArgumentOutOfRangeException ex)
        {
          TestPlanGrid.traceSource_0.Debug((Exception) ex);
        }
        catch (ArgumentException ex)
        {
          TestPlanGrid.traceSource_0.Error(ex.Message);
        }
      }
      catch (XmlException ex)
      {
        TestPlanGrid.traceSource_0.Error("Cannot paste contents of clipboard due to parsing error.");
      }
      // ISSUE: reference to a compiler-generated field
      this.testPlanChangedDelegate_0();
      // ISSUE: reference to a compiler-generated field
      EventHandler eventHandler0 = this.eventHandler_0;
      if (eventHandler0 == null)
        return;
      eventHandler0((object) this, EventArgs.Empty);
    }
  }

  private void method_20(object sender, RoutedEventArgs e) => this.DoPaste();

  public event EventHandler OnPasted;

  public Guid[] selectedStepIds()
  {
    return this.SelectedTestSteps.Select<ITestStep, Guid>((Func<ITestStep, Guid>) (itestStep_0 => itestStep_0.Id)).ToArray<Guid>();
  }

  public void setSelectedStepIds(Guid[] guid_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    this.SelectedTestSteps = Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) this.TestPlan.ChildTestSteps, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).Where<ITestStep>(new Func<ITestStep, bool>(new TestPlanGrid.Class157()
    {
      hashSet_0 = ((IEnumerable<Guid>) guid_1).ToHashSet<Guid>()
    }.method_0));
  }

  public List<bool> GetExpandedConf()
  {
    return this.iterateDepthFirst().Select<TestStepRowItem, bool>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsExpanded)).ToList<bool>();
  }

  public void SetExpandedConf(List<bool> conf)
  {
    this.iterateDepthFirst().Zip<TestStepRowItem, bool, bool>((IEnumerable<bool>) conf, (Func<TestStepRowItem, bool, bool>) ((testStepRowItem_0, bool_0) =>
    {
      testStepRowItem_0.IsExpanded = bool_0;
      return true;
    })).ToList<bool>();
    this.updateView();
  }

  public IEnumerable<TestStepRowItem> iterateDepthFirst(IEnumerable<TestStepRowItem> steps = null)
  {
    return (IEnumerable<TestStepRowItem>) Class24.smethod_13<TestStepRowItem>(steps ?? (IEnumerable<TestStepRowItem>) this.observableCollection_0, (Func<TestStepRowItem, IEnumerable<TestStepRowItem>>) (testStepRowItem_0 => (IEnumerable<TestStepRowItem>) testStepRowItem_0.AllChildItems));
  }

  public IEnumerable<TestStepRowItem> GetRowItems(IEnumerable<ITestStep> steps)
  {
    return this.GetRowItems(steps.Select<ITestStep, Guid>((Func<ITestStep, Guid>) (itestStep_0 => itestStep_0.Id)));
  }

  public IEnumerable<TestStepRowItem> GetRowItems(IEnumerable<Guid> steps)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    return this.class76_0.method_0().Where<TestStepRowItem>(new Func<TestStepRowItem, bool>(new TestPlanGrid.Class158()
    {
      hashSet_0 = steps.ToHashSet<Guid>()
    }.method_0));
  }

  private TestStepRowItem method_21(ITestStep itestStep_1) => this.GetRowItem(itestStep_1.Id);

  public TestStepRowItem GetRowItem(Guid stepId)
  {
    return this.GetRowItems((IEnumerable<Guid>) new Guid[1]
    {
      stepId
    }).FirstOrDefault<TestStepRowItem>();
  }

  protected virtual void OnTestPlanChanged(TestPlan oldValue, TestPlan newValue)
  {
    if (this.FilterMode)
      this.FilterMode = false;
    this.dataGrid_0.UnselectAll();
    this.dataGrid_0.UnselectAllCells();
    if (oldValue != null)
    {
      oldValue.Steps.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.method_24);
      oldValue.Steps.ChildStepsChanged -= new TestStepList.ChildStepsChangedDelegate(this.method_22);
    }
    if (newValue != null)
    {
      newValue.Steps.CollectionChanged += new NotifyCollectionChangedEventHandler(this.method_24);
      newValue.Steps.ChildStepsChanged += new TestStepList.ChildStepsChangedDelegate(this.method_22);
    }
    this.observableCollection_0.Clear();
    if (newValue != null)
    {
      using (this.delayUpdate())
      {
        foreach (ITestStep step in (Collection<ITestStep>) newValue.Steps)
          this.method_26(step);
      }
    }
    if (newValue != null)
      Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) newValue.ChildTestSteps, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).Select<ITestStep, ITypeData>((Func<ITestStep, ITypeData>) (itestStep_0 => TypeData.GetTypeData((object) itestStep_0))).Distinct<ITypeData>().ToList<ITypeData>().ForEach((Action<ITypeData>) (itypeData_0 => this.method_5(itypeData_0)));
    this.method_23();
  }

  public IDisposable delayUpdate()
  {
    if (this.bool_1)
      return (IDisposable) new Class182((Action) (() => { }));
    this.bool_1 = true;
    return (IDisposable) new Class182((Action) (() =>
    {
      this.bool_1 = false;
      this.updateView();
      this.method_2();
    }));
  }

  public void updateView(bool saveSelection = false)
  {
    this.dataGrid_0.CancelEdit();
    this.dataGrid_0.CancelEdit();
    this.class76_0.method_6();
    if (saveSelection)
    {
      ITestStep[] array = this.SelectedTestSteps.ToArray<ITestStep>();
      this.collectionViewSource_0.View.Refresh();
      this.SelectedTestSteps = (IEnumerable<ITestStep>) array;
    }
    else
      this.collectionViewSource_0.View.Refresh();
    this.method_16();
    this.TotalStepCount = this.flattenedRowItems.Count;
    this.VisibleStepCount = this.dataGrid_0.Items.Count;
    this.method_0("ExtraColumns");
    if (!this.FilterMode)
      return;
    bool flag = false;
    foreach (PropertyColumn propertyColumn in this.dataGrid_0.Columns.OfType<PropertyColumn>())
    {
      if (propertyColumn.Filter.Count != 0)
        flag = true;
    }
    this.FilterSettingsChanged = flag;
  }

  private void method_22(
    TestStepList testStepList_0,
    TestStepList.ChildStepsChangedAction childStepsChangedAction_0,
    ITestStep itestStep_1,
    int int_0)
  {
    if (this.bool_1 || this.bool_2)
      return;
    this.bool_2 = true;
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      if (this.FilterMode)
        this.FilterMode = false;
      this.bool_2 = false;
      this.updateView();
      this.method_2();
      this.method_0("SelectedTestSteps");
    }));
  }

  private void method_23()
  {
    HashSet<string> hashSet = ((IEnumerable<ITypeData>) Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) this.TestPlan.ChildTestSteps, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).Select<ITestStep, ITypeData>(new Func<ITestStep, ITypeData>(TypeData.GetTypeData)).ToArray<ITypeData>()).Distinct<ITypeData>().SelectMany<ITypeData, IMemberData>((Func<ITypeData, IEnumerable<IMemberData>>) (itypeData_0 => itypeData_0.GetMembers())).Distinct<IMemberData>().Where<IMemberData>((Func<IMemberData, bool>) (imemberData_0 => imemberData_0.HasAttribute<ColumnDisplayNameAttribute>())).Select<IMemberData, string>(new Func<IMemberData, string>(ColumnViewProvider.GetColumnName)).ToHashSet<string>();
    foreach (ColumnViewProvider columnViewProvider in this.hashSet_0.ToArray<ColumnViewProvider>())
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TestPlanGrid.Class159 class159 = new TestPlanGrid.Class159();
      // ISSUE: reference to a compiler-generated field
      class159.columnViewProvider_0 = columnViewProvider;
      // ISSUE: reference to a compiler-generated method
      ColumnVisibilityDetails visibilityDetails = this.ColumnVisibility.FirstOrDefault<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class159.method_0));
      // ISSUE: reference to a compiler-generated field
      if ((visibilityDetails != null ? (visibilityDetails.ManuallyConfigured ? 1 : 0) : 1) == 0 && !hashSet.Contains(class159.columnViewProvider_0.Name))
      {
        // ISSUE: reference to a compiler-generated field
        this.hashSet_0.Remove(class159.columnViewProvider_0);
        this.ColumnVisibility.Remove(visibilityDetails);
        this.dataGrid_0.Columns.Remove(visibilityDetails.Column);
      }
    }
  }

  private void method_24(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.OldItems != null)
    {
      foreach (ITestStep testStep in e.OldItems.Cast<ITestStep>().ToList<ITestStep>())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        this.observableCollection_0.Remove(this.observableCollection_0.FirstOrDefault<TestStepRowItem>(new Func<TestStepRowItem, bool>(new TestPlanGrid.Class160()
        {
          itestStep_0 = testStep
        }.method_0)));
      }
    }
    if (e.NewItems != null)
    {
      List<ITestStep> list = e.NewItems.Cast<ITestStep>().ToList<ITestStep>();
      int newStartingIndex = e.NewStartingIndex;
      for (int index = 0; index < list.Count; ++index)
      {
        TestStepRowItem testStepRowItem = new TestStepRowItem(list[list.Count - index - 1], (TestStepRowItem) null);
        this.observableCollection_0.Insert(newStartingIndex, testStepRowItem);
      }
      foreach (ITypeData itypeData_0 in list.Select<ITestStep, ITypeData>((Func<ITestStep, ITypeData>) (itestStep_0 => TypeData.GetTypeData((object) itestStep_0))).Distinct<ITypeData>())
        this.method_5(itypeData_0);
    }
    if (e.Action != NotifyCollectionChangedAction.Reset)
      return;
    this.observableCollection_0.Clear();
  }

  private void method_25(ITestStep itestStep_1, int int_0, TestStepRowItem testStepRowItem_0 = null)
  {
    TestStepRowItem testStepRowItem = new TestStepRowItem(itestStep_1, testStepRowItem_0);
    if (testStepRowItem_0 != null)
      testStepRowItem_0.InsertChildStep(testStepRowItem, int_0);
    else
      this.observableCollection_0.Insert(int_0, testStepRowItem);
  }

  private void method_26(ITestStep itestStep_1)
  {
    this.method_25(itestStep_1, this.observableCollection_0.Count);
  }

  private void dataGrid_0_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.bool_3 || this.bool_4)
      return;
    if (e != null && e.RemovedItems != null)
      this.itestStep_0 = e.RemovedItems.OfType<TestStepRowItem>().Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)).FirstOrDefault<ITestStep>();
    if (this.dataGrid_0.SelectedItems != null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TestPlanGrid.Class161 class161 = new TestPlanGrid.Class161();
      // ISSUE: reference to a compiler-generated field
      class161.hashSet_0 = this.SelectedTestSteps.ToHashSet<ITestStep>();
      try
      {
        this.bool_4 = true;
        // ISSUE: reference to a compiler-generated method
        this.dataGrid_0.SelectedItems.smethod_10(new Predicate<object>(class161.method_0));
      }
      catch
      {
        this.dataGrid_0.UnselectAll();
      }
      finally
      {
        this.bool_4 = false;
      }
    }
    this.method_0("SelectedTestSteps");
    this.method_0("SelectedTestStep");
  }

  public void SetSelectedItems(IEnumerable<ITestStep> steps, bool ChangeExpanded = true, bool show = true)
  {
    TestStepRowItem[] array = this.GetRowItems(steps).ToArray<TestStepRowItem>();
    if (ChangeExpanded)
    {
      foreach (TestStepRowItem hash in ((IEnumerable<TestStepRowItem>) array).SelectMany<TestStepRowItem, TestStepRowItem>((Func<TestStepRowItem, IEnumerable<TestStepRowItem>>) (testStepRowItem_0 => testStepRowItem_0.AllParents)).ToHashSet<TestStepRowItem>())
        hash.IsExpanded = true;
      this.updateView();
    }
    this.updateView();
    if (show)
    {
      if (array.Length == 1)
        this.AsyncBringIntoView(true, ((IEnumerable<TestStepRowItem>) array).First<TestStepRowItem>().Step.Id);
      else if (array.Length > 1)
        this.AsyncBringIntoView(true, ((IEnumerable<TestStepRowItem>) array).First<TestStepRowItem>().Step.Id, ((IEnumerable<TestStepRowItem>) array).Last<TestStepRowItem>().Step.Id);
    }
    this.SelectedTestSteps = steps;
  }

  public void SetSelectedItem(ITestStep step)
  {
    this.SetSelectedItems((IEnumerable<ITestStep>) new ITestStep[1]
    {
      step
    });
  }

  private void method_27(ITestStep itestStep_1)
  {
    foreach (ITestStep step in Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) new ITestStep[1]
    {
      itestStep_1
    }, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps), true))
    {
      foreach (IMemberData member in TypeData.GetTypeData((object) step).GetMembers())
      {
        ExternalParameterAttribute attribute = member.GetAttribute<ExternalParameterAttribute>();
        if (attribute != null)
        {
          ExternalParameter externalParameter = this.TestPlan.ExternalParameters.Add(step, member, attribute.Name);
          if (externalParameter != null)
            externalParameter.Value = externalParameter.Value;
        }
      }
    }
  }

  private ITestStep method_28(ITypeData itypeData_0, out Action action_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class162 class162 = new TestPlanGrid.Class162();
    // ISSUE: reference to a compiler-generated field
    class162.testPlanGrid_0 = this;
    try
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        class162.itestStep_0 = (ITestStep) itypeData_0.CreateInstance();
        // ISSUE: reference to a compiler-generated method
        action_0 = new Action(class162.method_0);
        // ISSUE: reference to a compiler-generated field
        return class162.itestStep_0;
      }
      catch (TargetInvocationException ex)
      {
        if (!(ex.InnerException is LicenseException))
          throw ex;
        TestPlanGrid.traceSource_0.Error("Could not create '{0}' step: {1}", (object) itypeData_0.GetDisplayAttribute().Name, (object) ex.InnerException.Message);
      }
    }
    catch (Exception ex)
    {
      TestPlanGrid.traceSource_0.Error("Caught exception while instantiating test step of type '{0}'.", (object) itypeData_0);
      TestPlanGrid.traceSource_0.Debug(ex);
    }
    action_0 = (Action) (() => { });
    return (ITestStep) null;
  }

  protected override void OnMouseRightButtonDown(MouseButtonEventArgs mouseButtonEventArgs_0)
  {
    if (GuiHelpers.TryFindFromPoint<DataGridRow>((UIElement) this, mouseButtonEventArgs_0.GetPosition((IInputElement) this.dataGrid_0)) == null && GuiHelpers.TryFindFromPoint<DataGridColumnHeader>((UIElement) this, mouseButtonEventArgs_0.GetPosition((IInputElement) this.dataGrid_0)) == null)
      this.SetSelectedItems(Enumerable.Empty<ITestStep>());
    base.OnMouseRightButtonDown(mouseButtonEventArgs_0);
  }

  private void dataGrid_0_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class163 class163 = new TestPlanGrid.Class163();
    // ISSUE: reference to a compiler-generated field
    class163.testPlanGrid_0 = this;
    // ISSUE: reference to a compiler-generated field
    class163.object_0 = sender;
    this.class151_0 = (TestPlanGrid.Class151) null;
    DependencyObject child = Keyboard.FocusedElement as DependencyObject;
    while (child != null)
    {
      child = child.GetParentObject();
      if (child is Popup)
        return;
      if (child == this)
        break;
    }
    // ISSUE: reference to a compiler-generated field
    if (this.ReadOnly || !this.AllowDrop || GuiHelpers.TryFindFromPoint<ScrollBar>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0)) != null)
      return;
    // ISSUE: reference to a compiler-generated field
    ErrorAdorner fromPoint1 = GuiHelpers.TryFindFromPoint<ErrorAdorner>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0));
    if (fromPoint1 != null)
    {
      ITestStep step = (fromPoint1.AdornedElement is DataGridRow adornedElement ? adornedElement.DataContext : (object) null) is TestStepRowItem dataContext ? dataContext.Step : (ITestStep) null;
      if (this.SelectedTestSteps.Contains<ITestStep>(step))
        return;
      this.SetSelectedItem(step);
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class163.dataGridRow_0 = GuiHelpers.TryFindFromPoint<DataGridRow>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0));
      // ISSUE: reference to a compiler-generated field
      if (class163.dataGridRow_0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        if (GuiHelpers.TryFindFromPoint<DataGridColumnHeader>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0)) != null)
          return;
        this.SetSelectedItems(Enumerable.Empty<ITestStep>());
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (GuiHelpers.TryFindFromPoint<ToggleButton>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0)) != null || GuiHelpers.TryFindFromPoint<TextBoxBase>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0)) != null || GuiHelpers.TryFindFromPoint<ButtonBase>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0)) != null)
          return;
        // ISSUE: reference to a compiler-generated field
        DataGridCell fromPoint2 = GuiHelpers.TryFindFromPoint<DataGridCell>((UIElement) class163.object_0, e.GetPosition((IInputElement) this.dataGrid_0));
        if ((fromPoint2 != null ? (fromPoint2.IsEditing ? 1 : 0) : 0) != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        class163.iinputElement_0 = Keyboard.FocusedElement;
        // ISSUE: reference to a compiler-generated field
        class163.mouseEventHandler_0 = (MouseEventHandler) null;
        // ISSUE: reference to a compiler-generated field
        class163.mouseButtonEventHandler_0 = (MouseButtonEventHandler) null;
        // ISSUE: reference to a compiler-generated field
        class163.point_0 = e.GetPosition((IInputElement) this);
        // ISSUE: reference to a compiler-generated field
        class163.bool_0 = false;
        ITestStep[] array = this.SelectedTestSteps.ToArray<ITestStep>();
        // ISSUE: reference to a compiler-generated field
        class163.itestStep_0 = Class179.smethod_0((IEnumerable<ITestStep>) array).ToArray();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        class163.mouseEventHandler_0 = new MouseEventHandler(class163.method_1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        class163.mouseButtonEventHandler_0 = new MouseButtonEventHandler(class163.method_2);
        // ISSUE: reference to a compiler-generated field
        this.dataGrid_0.PreviewMouseLeftButtonUp += class163.mouseButtonEventHandler_0;
        // ISSUE: reference to a compiler-generated field
        this.dataGrid_0.PreviewMouseMove += class163.mouseEventHandler_0;
        e.Handled = true;
      }
    }
  }

  private void dataGrid_0_DragLeave(object sender, DragEventArgs e)
  {
    this.bool_5 = true;
    Class24.smethod_24(100, (Action) (() => GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      if (!this.bool_5)
        return;
      this.bool_5 = false;
      this.method_29((TestPlanGrid.Class153) null);
    }))));
  }

  private void method_29(TestPlanGrid.Class153 class153_1)
  {
    this.class153_0 = class153_1;
    this.method_32(class153_1);
  }

  private void method_30()
  {
    this.dataGrid_0.MouseMove -= new MouseEventHandler(this.dataGrid_0_MouseMove);
    Mouse.Capture((IInputElement) null);
    this.method_36();
    if (this.class151_0 != null && this.class153_0 != null)
    {
      TestPlan testPlan = this.TestPlan;
      if ((testPlan != null ? (!testPlan.IsRunning ? 1 : 0) : 0) != 0)
      {
        Action action_0 = (Action) null;
        ITestStep[] ienumerable_0 = this.class151_0.itestStep_0;
        if (ienumerable_0 == null)
        {
          ITestStep testStep = this.method_28(((IEnumerable<ITypeData>) this.class151_0.itypeData_0).First<ITypeData>(), out action_0);
          if (testStep == null)
            return;
          ienumerable_0 = new ITestStep[1]{ testStep };
        }
        int num1 = 0;
        foreach (ITestStep testStep in ienumerable_0)
        {
          ITestStepParent testStepParent = this.method_19((ITestStepParent) testStep);
          TestStepList childTestSteps = testStepParent == null ? (TestStepList) null : testStepParent.ChildTestSteps;
          int num2 = 0;
          TestStepList testStepList = (TestStepList) null;
          if (this.class153_0.method_2() == TestPlanGrid.HoverStates.FirstChild)
          {
            testStepList = this.class153_0.method_0().ChildTestSteps;
            if (this.class153_0.method_0() is ITestStep)
              this.method_21(this.class153_0.method_0() as ITestStep).IsExpanded = true;
          }
          else if (this.class153_0.method_2() == TestPlanGrid.HoverStates.NextSibling)
          {
            testStepList = this.method_19(this.class153_0.method_0()).ChildTestSteps;
            num2 = testStepList.IndexOf((ITestStep) this.class153_0.method_0()) + 1;
          }
          if (testStepList != null)
          {
            if (ComponentSettings<EditorSettings>.Current.EnsureUniqueStepName)
              Class179.smethod_1((IList<ITestStep>) testStepList, (IEnumerable<ITestStep>) new ITestStep[1]
              {
                testStep
              });
            this.method_38((IList<ITestStep>) childTestSteps, (IList<ITestStep>) testStepList, testStep, num2 + num1++);
          }
        }
        if (action_0 != null)
          action_0();
        this.updateView();
        this.SelectedTestSteps = (IEnumerable<ITestStep>) Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) ienumerable_0, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).ToArray();
      }
    }
    this.method_29((TestPlanGrid.Class153) null);
    ((IEnumerable<TestStepRowItem>) this.dataGrid_0.Items.Cast<TestStepRowItem>()).Each<TestStepRowItem>((Action<TestStepRowItem, int>) ((testStepRowItem_0, int_0) => testStepRowItem_0.IsDraggedOver = false));
  }

  private TestPlanGrid.Class153 method_31(Point point_0)
  {
    if (this.TestPlan.IsRunning || this.TestPlan.Locked)
      return (TestPlanGrid.Class153) null;
    if (point_0.Y <= 0.0)
      point_0.Y = 0.0;
    DataGridRow fromPoint = GuiHelpers.TryFindFromPoint<DataGridRow>((UIElement) this.dataGrid_0, point_0);
    if (fromPoint == null)
    {
      if (GuiHelpers.TryFindFromPoint<DataGridColumnHeader>((UIElement) this.dataGrid_0, point_0) != null)
        return (TestPlanGrid.Class153) null;
      return this.TestPlan.ChildTestSteps.Count == 0 ? new TestPlanGrid.Class153((ITestStepParent) this.TestPlan, TestPlanGrid.HoverStates.FirstChild) : new TestPlanGrid.Class153((ITestStepParent) this.TestPlan.ChildTestSteps.Last<ITestStep>(), TestPlanGrid.HoverStates.NextSibling);
    }
    Point point = this.dataGrid_0.TranslatePoint(point_0, (UIElement) fromPoint);
    ITestStep step = ((TestStepRowItem) fromPoint.Item).Step;
    ITypeData itypeData_0 = ((IEnumerable<ITypeData>) this.class151_0.itypeData_0).FirstOrDefault<ITypeData>();
    double num1 = itypeData_0 == null || !TestStepList.AllowChild(step.GetType(), itypeData_0.smethod_0()) ? 0.5 : 1.0 / 3.0;
    if (point.Y < fromPoint.ActualHeight * num1)
    {
      TestStepList childTestSteps = this.method_19((ITestStepParent) step).ChildTestSteps;
      int num2 = childTestSteps.IndexOf(step);
      return num2 == 0 ? new TestPlanGrid.Class153(this.method_19((ITestStepParent) step), TestPlanGrid.HoverStates.FirstChild) : new TestPlanGrid.Class153((ITestStepParent) childTestSteps[num2 - 1], TestPlanGrid.HoverStates.NextSibling);
    }
    return point.Y < fromPoint.ActualHeight * (1.0 - num1) ? new TestPlanGrid.Class153((ITestStepParent) step, TestPlanGrid.HoverStates.FirstChild) : new TestPlanGrid.Class153((ITestStepParent) step, TestPlanGrid.HoverStates.NextSibling);
  }

  private void method_32(TestPlanGrid.Class153 class153_1)
  {
    this.canvas_2.Visibility = System.Windows.Visibility.Collapsed;
    this.canvas_3.Visibility = System.Windows.Visibility.Collapsed;
    this.border_0.Height = 0.0;
    if (class153_1 == null)
    {
      this.canvas_1.Visibility = System.Windows.Visibility.Collapsed;
      this.textBlock_0.Visibility = System.Windows.Visibility.Collapsed;
      Mouse.OverrideCursor = (Cursor) null;
    }
    else
    {
      this.textBlock_0.Visibility = System.Windows.Visibility.Visible;
      if (class153_1.method_2() != TestPlanGrid.HoverStates.NextSibling && !(class153_1.method_0() is TestPlan))
      {
        if (class153_1.method_2() == TestPlanGrid.HoverStates.FirstChild)
          this.canvas_2.Visibility = System.Windows.Visibility.Visible;
      }
      else
        this.canvas_3.Visibility = System.Windows.Visibility.Visible;
      if (class153_1.method_2() != TestPlanGrid.HoverStates.FirstChild && class153_1.method_2() != TestPlanGrid.HoverStates.NextSibling)
      {
        this.canvas_1.Visibility = System.Windows.Visibility.Collapsed;
      }
      else
      {
        this.canvas_1.Visibility = System.Windows.Visibility.Visible;
        if (class153_1.method_0() is TestPlan)
        {
          Canvas.SetTop((UIElement) this.canvas_1, this.dataGrid_0.FindVisualChild<DataGridRowsPresenter>().TranslatePoint(new Point(), (UIElement) this.dataGrid_0).Y);
          Canvas.SetLeft((UIElement) this.canvas_1, 12.0);
        }
        else
        {
          TestStepRowItem testStepRowItem = this.method_21((ITestStep) class153_1.method_0());
          int num1 = testStepRowItem.IsExpanded ? Class24.smethod_13<TestStepRowItem>((IEnumerable<TestStepRowItem>) testStepRowItem.VisibleChildItems, (Func<TestStepRowItem, IEnumerable<TestStepRowItem>>) (testStepRowItem_0 => !testStepRowItem_0.IsExpanded ? Enumerable.Empty<TestStepRowItem>() : (IEnumerable<TestStepRowItem>) testStepRowItem_0.VisibleChildItems)).Count<TestStepRowItem>() : 0;
          DataGridRow dataGridRow = (DataGridRow) this.dataGrid_0.ItemContainerGenerator.ContainerFromItem((object) testStepRowItem);
          if (dataGridRow == null)
            return;
          Point point = dataGridRow.TranslatePoint(new Point(), (UIElement) this.canvas_0);
          int num2 = -1;
          for (ITestStepParent itestStepParent_0 = (ITestStepParent) ((TestStepRowItem) dataGridRow.Item).Step; itestStepParent_0 != null; itestStepParent_0 = this.method_19(itestStepParent_0))
            ++num2;
          Canvas.SetTop((UIElement) this.canvas_1, point.Y + dataGridRow.ActualHeight * (class153_1.method_2() == TestPlanGrid.HoverStates.NextSibling ? 1.0 + (double) num1 : 0.5) + 2.0);
          Canvas.SetLeft((UIElement) this.canvas_1, point.X + (double) (num2 * 13));
          Canvas.SetTop((UIElement) this.border_0, point.Y + dataGridRow.ActualHeight - 4.0);
          Canvas.SetLeft((UIElement) this.border_0, point.X + (double) (num2 * 13) - 7.5);
          this.border_0.Height = Math.Max(0.0, class153_1.method_2() != TestPlanGrid.HoverStates.NextSibling ? 0.0 : dataGridRow.ActualHeight * (double) num1 + 4.0);
        }
      }
    }
  }

  private TestPlanGrid.Class153 method_33(
    TestPlanGrid.Class153 class153_1,
    TestPlanGrid.Class151 class151_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class164 class164 = new TestPlanGrid.Class164();
    if (class153_1 == null)
      return new TestPlanGrid.Class153((ITestStepParent) null, TestPlanGrid.HoverStates.CannotDropHere);
    if (class151_1.itestStep_0 != null)
    {
      if (((IEnumerable<ITestStep>) class151_1.itestStep_0).Any<ITestStep>((Func<ITestStep, bool>) (itestStep_0 => itestStep_0.IsReadOnly)))
        return new TestPlanGrid.Class153(class153_1.method_0(), TestPlanGrid.HoverStates.CannotDropHere);
      for (ITestStepParent itestStepParent_0 = class153_1.method_0(); itestStepParent_0 != null; itestStepParent_0 = this.method_19(itestStepParent_0))
      {
        foreach (ITestStep testStep in class151_1.itestStep_0)
        {
          if (itestStepParent_0 == testStep)
            return new TestPlanGrid.Class153(class153_1.method_0(), TestPlanGrid.HoverStates.CannotDropHere);
        }
      }
    }
    // ISSUE: reference to a compiler-generated field
    class164.testStepList_0 = (TestStepList) null;
    if (class153_1.method_2() == TestPlanGrid.HoverStates.FirstChild)
    {
      // ISSUE: reference to a compiler-generated field
      class164.testStepList_0 = class153_1.method_0().ChildTestSteps;
    }
    else
    {
      if (class153_1.method_2() != TestPlanGrid.HoverStates.NextSibling)
        return class153_1;
      // ISSUE: reference to a compiler-generated field
      class164.testStepList_0 = this.method_19(class153_1.method_0()).ChildTestSteps;
    }
    // ISSUE: reference to a compiler-generated method
    return ((IEnumerable<ITypeData>) this.class151_0.itypeData_0).All<ITypeData>(new Func<ITypeData, bool>(class164.method_0)) ? class153_1 : new TestPlanGrid.Class153(class153_1.method_0(), TestPlanGrid.HoverStates.CannotDropHere);
  }

  private void method_34(Point point_0)
  {
    if (this.class151_0 == null)
      return;
    Canvas.SetTop((UIElement) this.textBlock_0, point_0.Y);
    Canvas.SetLeft((UIElement) this.textBlock_0, point_0.X);
    this.textBlock_0.Text = this.class151_0.ToString();
    TestPlanGrid.Class153 class153_1 = this.method_33(this.method_31(point_0) ?? this.class153_0, this.class151_0);
    this.method_29(class153_1);
    TestStepRowItem[] array = this.dataGrid_0.Items.Cast<TestStepRowItem>().ToArray<TestStepRowItem>();
    ((IEnumerable<TestStepRowItem>) array).Each<TestStepRowItem>((Action<TestStepRowItem, int>) ((testStepRowItem_0, int_0) => testStepRowItem_0.IsDraggedOver = false));
    if (class153_1 == null || !(class153_1.method_0() is ITestStep))
      return;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class165 class165 = new TestPlanGrid.Class165();
    // ISSUE: reference to a compiler-generated field
    class165.itestStep_0 = class153_1.method_0() as ITestStep;
    // ISSUE: reference to a compiler-generated field
    if (TypeData.GetTypeData((object) class165.itestStep_0).GetAttributes<AllowAnyChildAttribute>().Any<AllowAnyChildAttribute>() && class153_1.method_2() != TestPlanGrid.HoverStates.NextSibling)
    {
      // ISSUE: reference to a compiler-generated method
      ((IEnumerable<TestStepRowItem>) array).FirstOrDefault<TestStepRowItem>(new Func<TestStepRowItem, bool>(class165.method_0)).IsDraggedOver = true;
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (class165.itestStep_0.Parent is ITestStep)
      {
        // ISSUE: reference to a compiler-generated method
        ((IEnumerable<TestStepRowItem>) array).FirstOrDefault<TestStepRowItem>(new Func<TestStepRowItem, bool>(class165.method_1)).IsDraggedOver = true;
      }
      else
        ((IEnumerable<TestStepRowItem>) array).Each<TestStepRowItem>((Action<TestStepRowItem, int>) ((testStepRowItem_0, int_0) => testStepRowItem_0.IsDraggedOver = false));
    }
  }

  private void method_35(object object_0, TestPlanGrid.Class151 class151_1)
  {
    Mouse.Capture((IInputElement) this.dataGrid_0);
    this.dataGrid_0.MouseMove += new MouseEventHandler(this.dataGrid_0_MouseMove);
    this.dataGrid_0.MouseUp += (MouseButtonEventHandler) ((sender, e) => this.method_30());
    this.bool_5 = false;
    this.class151_0 = class151_1;
    if (this.class151_0 != null && !((IEnumerable<ITypeData>) this.class151_0.itypeData_0).Any<ITypeData>((Func<ITypeData, bool>) (itypeData_0 => !itypeData_0.DescendsTo(typeof (ITestStep)))))
      return;
    this.method_29((TestPlanGrid.Class153) null);
    this.class151_0 = (TestPlanGrid.Class151) null;
  }

  private void dataGrid_0_MouseMove(object sender, MouseEventArgs e)
  {
    this.method_34(e.GetPosition((IInputElement) this.dataGrid_0));
    this.method_37();
  }

  private void method_36()
  {
    if (this.dispatcherTimer_0 == null)
      return;
    this.dispatcherTimer_0.Stop();
    this.dispatcherTimer_0 = (DispatcherTimer) null;
  }

  private void method_37()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class166 class166 = new TestPlanGrid.Class166();
    // ISSUE: reference to a compiler-generated field
    class166.testPlanGrid_0 = this;
    // ISSUE: reference to a compiler-generated field
    class166.scrollViewer_0 = this.dataGrid_0.FindVisualChild<ScrollViewer>();
    // ISSUE: reference to a compiler-generated field
    if (class166.scrollViewer_0 == null)
      return;
    double num1 = 25.0;
    double y = Mouse.GetPosition((IInputElement) this.dataGrid_0).Y;
    double num2 = 0.0;
    if (y > this.dataGrid_0.ActualHeight - num1)
      num2 = y - this.dataGrid_0.ActualHeight + num1;
    else if (y < num1)
      num2 = y - num1;
    this.double_0 = num2 / 15.0;
    if (num2 != 0.0 && this.dispatcherTimer_0 == null)
    {
      this.dispatcherTimer_0 = new DispatcherTimer()
      {
        Interval = TimeSpan.FromMilliseconds(50.0)
      };
      // ISSUE: reference to a compiler-generated method
      this.dispatcherTimer_0.Tick += new EventHandler(class166.method_1);
      this.dispatcherTimer_0.Start();
    }
    else
    {
      if (num2 != 0.0 || this.dispatcherTimer_0 == null)
        return;
      this.method_36();
    }
  }

  private void dataGrid_0_DragOver(object sender, DragEventArgs e)
  {
    this.method_34(e.GetPosition((IInputElement) this.dataGrid_0));
  }

  private void dataGrid_0_Drop(object sender, DragEventArgs e) => this.method_30();

  private void method_38(
    IList<ITestStep> ilist_0,
    IList<ITestStep> ilist_1,
    ITestStep itestStep_1,
    int int_0)
  {
    using (this.delayUpdate())
    {
      TestStepRowItem rowItem = this.GetRowItem(itestStep_1.Id);
      if (rowItem != null)
      {
        List<Tuple<ITestStep, bool, TestStepRowItem>> list = rowItem.GetAllChildRowItems().Select<TestStepRowItem, Tuple<ITestStep, bool, TestStepRowItem>>((Func<TestStepRowItem, Tuple<ITestStep, bool, TestStepRowItem>>) (testStepRowItem_0 => new Tuple<ITestStep, bool, TestStepRowItem>(testStepRowItem_0.Step, testStepRowItem_0.IsExpanded, testStepRowItem_0))).ToList<Tuple<ITestStep, bool, TestStepRowItem>>();
        Class179.smethod_4(ilist_0, ilist_1, itestStep_1, int_0);
        this.method_2();
        ILookup<Guid, Tuple<ITestStep, bool, TestStepRowItem>> lookup = list.ToLookup<Tuple<ITestStep, bool, TestStepRowItem>, Guid>((Func<Tuple<ITestStep, bool, TestStepRowItem>, Guid>) (tuple_0 => tuple_0.Item1.Id));
        foreach (TestStepRowItem testStepRowItem in this.class76_0.method_0())
        {
          if (lookup.Contains(testStepRowItem.Step.Id))
            testStepRowItem.IsExpanded = lookup[testStepRowItem.Step.Id].First<Tuple<ITestStep, bool, TestStepRowItem>>().Item2;
        }
      }
      else
      {
        Class179.smethod_4(ilist_0, ilist_1, itestStep_1, int_0);
        this.method_2();
      }
    }
  }

  private void method_39(object sender, EventArgs e) => this.method_2();

  public void LoadMonitorVariable(IMemberData member)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class167 class167 = new TestPlanGrid.Class167();
    // ISSUE: reference to a compiler-generated field
    class167.columnViewProvider_0 = ColumnViewProvider.GetFor(member);
    // ISSUE: reference to a compiler-generated field
    if (!this.hashSet_0.Contains(class167.columnViewProvider_0))
    {
      // ISSUE: reference to a compiler-generated field
      this.method_6(class167.columnViewProvider_0);
    }
    // ISSUE: reference to a compiler-generated method
    ColumnVisibilityDetails visibilityDetails = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class167.method_0));
    visibilityDetails.IsVisible = true;
    visibilityDetails.ManuallyConfigured = true;
    this.method_42();
  }

  public void RemoveMonitorVariable(IMemberData member)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    ColumnVisibilityDetails visibilityDetails = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(new TestPlanGrid.Class168()
    {
      columnViewProvider_0 = ColumnViewProvider.GetFor(member)
    }.method_0));
    visibilityDetails.IsVisible = false;
    if (!visibilityDetails.ManuallyConfigured)
      visibilityDetails.ManuallyConfigured = true;
    this.method_42();
  }

  public bool MonitorVariableVisible(IMemberData member)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    ColumnVisibilityDetails visibilityDetails = this.ColumnVisibility.FirstOrDefault<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(new TestPlanGrid.Class169()
    {
      columnViewProvider_0 = ColumnViewProvider.GetFor(member)
    }.method_0));
    return visibilityDetails != null && visibilityDetails.IsVisible;
  }

  private void method_40(DataGridColumn dataGridColumn_0_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class170 class170 = new TestPlanGrid.Class170();
    // ISSUE: reference to a compiler-generated field
    class170.dataGridColumn_0 = dataGridColumn_0_1;
    // ISSUE: reference to a compiler-generated method
    ColumnVisibilityDetails visibilityDetails1 = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class170.method_0));
    // ISSUE: reference to a compiler-generated field
    if (class170.dataGridColumn_0.DisplayIndex == -1)
    {
      this.method_42();
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      class170.list_0 = this.dataGrid_0.Columns.OrderBy<DataGridColumn, int>((Func<DataGridColumn, int>) (dataGridColumn_0_2 => dataGridColumn_0_2.DisplayIndex)).ToList<DataGridColumn>();
      int count = this.ColumnVisibility.Count;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class170.int_0 = class170.list_0.IndexOf(visibilityDetails1.Column);
      // ISSUE: reference to a compiler-generated field
      if (class170.int_0 == 0 && count > 1)
      {
        // ISSUE: reference to a compiler-generated method
        ColumnVisibilityDetails visibilityDetails2 = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class170.method_1));
        if (visibilityDetails1.Priority >= visibilityDetails2.Priority)
          visibilityDetails1.Priority = visibilityDetails2.Priority - 0.001;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (class170.int_0 == count - 1 && class170.int_0 > 0)
      {
        // ISSUE: reference to a compiler-generated method
        ColumnVisibilityDetails visibilityDetails3 = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class170.method_2));
        if (visibilityDetails1.Priority <= visibilityDetails3.Priority)
          visibilityDetails1.Priority = visibilityDetails3.Priority + 0.001;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (class170.int_0 < count - 1 && class170.int_0 > 0)
      {
        // ISSUE: reference to a compiler-generated method
        ColumnVisibilityDetails visibilityDetails4 = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class170.method_3));
        // ISSUE: reference to a compiler-generated method
        ColumnVisibilityDetails visibilityDetails5 = this.ColumnVisibility.First<ColumnVisibilityDetails>(new Func<ColumnVisibilityDetails, bool>(class170.method_4));
        if (visibilityDetails1.Priority <= visibilityDetails5.Priority || visibilityDetails1.Priority >= visibilityDetails4.Priority)
          visibilityDetails1.Priority = (visibilityDetails4.Priority + visibilityDetails5.Priority) * 0.5;
      }
      this.method_42();
    }
  }

  private void method_41(object sender, DataGridColumnEventArgs e) => this.method_40(e.Column);

  private void method_42()
  {
    int num = 0;
    foreach (ColumnVisibilityDetails visibilityDetails in (IEnumerable<ColumnVisibilityDetails>) this.ColumnVisibility.OrderBy<ColumnVisibilityDetails, double>((Func<ColumnVisibilityDetails, double>) (columnVisibilityDetails_0 => columnVisibilityDetails_0.Priority)))
      visibilityDetails.Column.DisplayIndex = num++;
  }

  private void testPlanGrid_0_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Handled || this.IsEditing)
      return;
    if (Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) && e.Key == Key.C)
    {
      this.method_18((object) this, (RoutedEventArgs) null);
      e.Handled = true;
    }
    else if (Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) && e.Key == Key.V)
    {
      this.DoPaste();
      e.Handled = true;
    }
    else
    {
      if (((IEnumerable<Key>) new Key[8]
      {
        Key.Delete,
        Key.LeftCtrl,
        Key.RightCtrl,
        Key.Home,
        Key.End,
        Key.Space,
        Key.LeftAlt,
        Key.RightAlt
      }).Any<Key>(new Func<Key, bool>(Keyboard.IsKeyDown)))
        return;
      if (((IEnumerable<Key>) new Key[12]
      {
        Key.Down,
        Key.Up,
        Key.Right,
        Key.Left,
        Key.Next,
        Key.Prior,
        Key.LeftCtrl,
        Key.RightCtrl,
        Key.LeftShift,
        Key.RightShift,
        Key.F9,
        Key.Apps
      }).Contains<Key>(e.Key))
        return;
      this.dataGrid_0.BeginEdit();
    }
  }

  private void method_43(object sender, RoutedEventArgs e)
  {
    DependencyObject focusedElement = (DependencyObject) Keyboard.FocusedElement;
    DataGridRow parent = GuiHelpers.TryFindParent<DataGridRow>(focusedElement);
    if ((GuiHelpers.TryFindParent<DataGridCell>(focusedElement) ?? focusedElement as DataGridCell) != null)
      return;
    DataGridCellsPresenter gridCellsPresenter = ((IEnumerable) parent.GetVisualChildren()).OfType<DataGridCellsPresenter>().FirstOrDefault<DataGridCellsPresenter>();
    if (gridCellsPresenter == null)
    {
      this.dataGrid_0.ScrollIntoView(parent.Item);
      gridCellsPresenter = ((IEnumerable) parent.GetVisualChildren()).OfType<DataGridCellsPresenter>().FirstOrDefault<DataGridCellsPresenter>();
    }
    Keyboard.Focus((IInputElement) gridCellsPresenter.ItemContainerGenerator.ContainerFromIndex(0));
  }

  internal void method_44(TestStepRowItem testStepRowItem_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGrid.Class171 class171 = new TestPlanGrid.Class171();
    // ISSUE: reference to a compiler-generated field
    class171.testStepRowItem_0 = testStepRowItem_0;
    // ISSUE: reference to a compiler-generated field
    if (!class171.testStepRowItem_0.IsVisible)
      return;
    // ISSUE: reference to a compiler-generated method
    int offset = this.View.smethod_18<TestStepRowItem>(new Func<TestStepRowItem, bool>(class171.method_0));
    if (offset == -1)
      return;
    ScrollViewer visualChild = this.dataGrid_0.FindVisualChild<ScrollViewer>();
    if (visualChild == null)
      return;
    double verticalOffset = visualChild.VerticalOffset;
    double viewportHeight = visualChild.ViewportHeight;
    if ((double) offset < verticalOffset)
    {
      visualChild.ScrollToVerticalOffset((double) offset);
    }
    else
    {
      if ((double) offset <= verticalOffset + viewportHeight - 1.0)
        return;
      visualChild.ScrollToVerticalOffset((double) offset - (viewportHeight - 1.0));
    }
  }

  private void method_45()
  {
    double rowHeaderWidth = this.dataGrid_0.RowHeaderWidth;
    this.dataGrid_0.RowHeaderWidth = 0.0;
    this.dataGrid_0.RowHeaderWidth = rowHeaderWidth;
  }

  private void dataGrid_0_Loaded(object sender1, RoutedEventArgs e1)
  {
    GuiHelpers.DeferTillSuccess<ScrollViewer>((Func<ScrollViewer>) (() => this.dataGrid_0.FindVisualChild<ScrollViewer>()), (Action<ScrollViewer>) (scrollViewer_0 => DependencyPropertyDescriptor.FromProperty(ScrollViewer.ExtentHeightProperty, typeof (ScrollViewer)).AddValueChanged((object) scrollViewer_0, (EventHandler) ((sender2, e2) => this.method_45()))));
  }

  private void dataGrid_0_IsKeyboardFocusWithinChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private void method_46(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.flattenedRowItems.Count > 0 && this.flattenedRowItems.Any<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsExpanded)) && this.flattenedRowItems.Any<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.GetParents().Count<TestStepRowItem>() > 0));
  }

  private void method_47(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.flattenedRowItems.Count > 0 && this.flattenedRowItems.Any<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => !testStepRowItem_0.IsVisible));
  }

  private void method_48(object sender, RequestBringIntoViewEventArgs e)
  {
    if (!(e.OriginalSource is DataGridCell originalSource))
      return;
    e.Handled = true;
    DataGridRow parent = GuiHelpers.FindParent<DataGridRow>((DependencyObject) originalSource);
    if (parent == null)
      return;
    this.dataGrid_0.ScrollIntoView(parent.Item);
  }

  private void method_49(object sender, ExecutedRoutedEventArgs e)
  {
    if (this.jumpToStepAction == null || !(e.OriginalSource is DataGridRow originalSource))
      return;
    this.jumpToStepAction((originalSource.DataContext as TestStepRowItem).Step.Id);
  }

  private void method_50(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.jumpToStepAction == null)
    {
      e.CanExecute = false;
    }
    else
    {
      if (!(e.OriginalSource is DataGridRow originalSource))
        return;
      TestStepRowItem dataContext = originalSource.DataContext as TestStepRowItem;
      bool flag = false;
      IEnumerable<TestStepRowItem> source;
      if (dataContext.Parent == null)
      {
        source = (IEnumerable<TestStepRowItem>) this.observableCollection_0;
        flag = true;
      }
      else
      {
        source = (IEnumerable<TestStepRowItem>) dataContext.Parent.AllChildItems;
        TestStepRun stepRun = dataContext.Parent.Step.StepRun;
        if (stepRun != null)
          flag = stepRun.SupportsJumpTo;
      }
      if (source.FirstOrDefault<TestStepRowItem>((Func<TestStepRowItem, bool>) (testStepRowItem_0 => testStepRowItem_0.IsAtBreak)) == null)
        e.CanExecute = false;
      else
        e.CanExecute = flag;
    }
  }

  public void AsyncBringIntoView(params Guid[] stepId) => this.AsyncBringIntoView(false, stepId);

  public void AsyncBringIntoView(bool force, params Guid[] stepId)
  {
    if (!force && !this.bool_6)
      return;
    if (this.dispatcherTimer_1 == null)
    {
      this.dispatcherTimer_1 = new DispatcherTimer(DispatcherPriority.Input, this.Dispatcher)
      {
        Interval = TimeSpan.FromMilliseconds(100.0)
      };
      this.dispatcherTimer_1.Tick += new EventHandler(this.dispatcherTimer_1_Tick);
      this.dispatcherTimer_1.Start();
    }
    this.guid_0 = stepId;
  }

  private void dispatcherTimer_1_Tick(object sender, EventArgs e)
  {
    if (this.guid_0 == null)
      return;
    Guid[] guid0 = this.guid_0;
    foreach (TestStepRowItem rowItem in this.GetRowItems((IEnumerable<Guid>) guid0))
    {
      TestStepRowItem testStepRowItem_0 = rowItem;
      while (testStepRowItem_0 != null && !testStepRowItem_0.IsVisible)
        testStepRowItem_0 = testStepRowItem_0.Parent;
      if (testStepRowItem_0 != null)
        this.method_44(testStepRowItem_0);
    }
    Interlocked.CompareExchange<Guid[]>(ref this.guid_0, (Guid[]) null, guid0);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == TestPlanGrid.ScrollToPlayingProperty)
    {
      this.bool_6 = (bool) dependencyPropertyChangedEventArgs_0.NewValue;
      ComponentSettings<GuiControlsSettings>.Current.TestPlanGridScrollToPlaying = this.bool_6;
    }
    if (dependencyPropertyChangedEventArgs_0.Property != TestPlanGrid.FilterModeProperty)
      return;
    this.updateView();
  }

  private void method_51(object sender, RoutedEventArgs e)
  {
    MenuItem menuItem = (MenuItem) sender;
    if (!menuItem.IsLoaded)
      return;
    ColumnVisibilityDetails dataContext = (ColumnVisibilityDetails) menuItem.DataContext;
    dataContext.ManuallyConfigured = true;
    dataContext.IsVisible = menuItem.IsChecked;
  }

  private void method_52(object sender, CanExecuteRoutedEventArgs e)
  {
    CanExecuteRoutedEventArgs executeRoutedEventArgs = e;
    ITestStep selectedTestStep = this.SelectedTestStep;
    int num = (selectedTestStep != null ? selectedTestStep.GetType().smethod_4() : (string) null) != null ? 1 : 0;
    executeRoutedEventArgs.CanExecute = num != 0;
  }

  private void method_53(object sender, ExecutedRoutedEventArgs e)
  {
    IShowHelp helpProvider = HelpManager.Instance.HelpProvider;
    ITestStep selectedTestStep = this.SelectedTestStep;
    string helpSpec = selectedTestStep != null ? selectedTestStep.GetType().smethod_4() : (string) null;
    helpProvider.ShowHelp((object) helpSpec);
  }

  private void method_54(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.dataGrid_0.Items.IsEmpty)
      e.CanExecute = false;
    else
      e.CanExecute = !this.ReadOnly;
  }

  private void method_55(object sender, ExecutedRoutedEventArgs e)
  {
    IEnumerable<ITestStep> array = (IEnumerable<ITestStep>) (e.Command != TestPlanGrid.ToggleAllTestSteps ? this.SelectedTestSteps : (IEnumerable<ITestStep>) Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) this.TestPlan.ChildTestSteps, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => !itestStep_0.IsReadOnly ? (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps : (IEnumerable<ITestStep>) Array.Empty<ITestStep>()))).ToArray<ITestStep>();
    bool flag = array.Any<ITestStep>((Func<ITestStep, bool>) (itestStep_0 => !itestStep_0.Enabled));
    foreach (ITestStep testStep in array)
    {
      if (!testStep.IsReadOnly)
        testStep.Enabled = flag;
    }
    this.updateView(true);
    this.Focus();
  }

  private void method_56(object sender, RoutedEventArgs e)
  {
    if (e.OriginalSource is CheckBox originalSource && originalSource.DataContext is TestStepRowItem dataContext)
      this.SetSelectedItems((IEnumerable<ITestStep>) new ITestStep[1]
      {
        dataContext.Step
      }, false);
    this.updateView();
  }

  private void dataGrid_0_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
  {
    if (Keyboard.FocusedElement != this.dataGrid_0 || !(this.dataGrid_0.SelectedItem is TestStepRowItem selectedItem))
      return;
    this.method_11(selectedItem, 0);
  }

  private void method_57(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (!(bool) e.NewValue)
      return;
    (sender as ItemsControl).GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
  }

  private void method_58(object sender, RoutedEventArgs e)
  {
    foreach (DataGridColumn column in (Collection<DataGridColumn>) this.dataGrid_0.Columns)
    {
      if (column is PropertyColumn propertyColumn)
        propertyColumn.Filter.method_2();
    }
    e.Handled = true;
  }

  private void method_59(object sender, ExecutedRoutedEventArgs e)
  {
    if (!this.FilterMode)
      this.columnFilterContext_0.Update((IEnumerable<TestStepRowItem>) this.flattenedRowItems, this.dataGrid_0.Columns.OfType<PropertyColumn>().Select<PropertyColumn, ColumnFilter>((Func<PropertyColumn, ColumnFilter>) (propertyColumn_0 => propertyColumn_0.Filter)).ToArray<ColumnFilter>());
    else
      this.columnFilterContext_0.method_0();
    this.FilterMode = !this.FilterMode;
    this.updateView();
  }

  private void menuItem_0_Click(object sender, RoutedEventArgs e) => this.method_0("ExtraColumns");

  private void menuItem_0_Loaded(object sender, RoutedEventArgs e)
  {
    ((ItemsControl) sender).ItemContainerStyleSelector = (StyleSelector) new TestPlanGrid.Class154();
  }

  protected override void OnPreviewKeyDown(KeyEventArgs keyEventArgs_0)
  {
    if (keyEventArgs_0.Key == Key.F2)
    {
      MainWindow.RenameStep.Execute((object) this, (IInputElement) this);
      keyEventArgs_0.Handled = true;
    }
    base.OnPreviewKeyDown(keyEventArgs_0);
  }

  private void method_60(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.TotalStepCount > 0;
  }

  private void dataGrid_0_DragEnter(object sender, DragEventArgs e)
  {
    this.class151_0 = TestPlanGrid.Class151.smethod_0(e);
    if (this.class151_0 != null && !((IEnumerable<ITypeData>) this.class151_0.itypeData_0).Any<ITypeData>((Func<ITypeData, bool>) (itypeData_0 => !itypeData_0.DescendsTo(typeof (ITestStep)))))
      return;
    this.method_29((TestPlanGrid.Class153) null);
    this.class151_0 = (TestPlanGrid.Class151) null;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_7)
      return;
    this.bool_7 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/testplangrid.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.testPlanGrid_0 = (TestPlanGrid) target;
        this.testPlanGrid_0.PreviewKeyDown += new KeyEventHandler(this.testPlanGrid_0_PreviewKeyDown);
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_13);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_47);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_14);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_46);
        break;
      case 4:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_18);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_3);
        break;
      case 5:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_8);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 6:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 7:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 8:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 9:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 10:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_52);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_53);
        break;
      case 11:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_54);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_55);
        break;
      case 12:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_54);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_55);
        break;
      case 13:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_60);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_59);
        break;
      case 14:
        this.canvas_0 = (Canvas) target;
        break;
      case 15:
        this.canvas_1 = (Canvas) target;
        break;
      case 16 /*0x10*/:
        this.canvas_2 = (Canvas) target;
        break;
      case 17:
        this.canvas_3 = (Canvas) target;
        break;
      case 18:
        this.border_0 = (Border) target;
        break;
      case 19:
        this.textBlock_0 = (TextBlock) target;
        break;
      case 21:
        this.menuItem_0 = (MenuItem) target;
        this.menuItem_0.Click += new RoutedEventHandler(this.menuItem_0_Click);
        this.menuItem_0.Loaded += new RoutedEventHandler(this.menuItem_0_Loaded);
        break;
      case 22:
        this.toggleButton_0 = (ToggleButton) target;
        break;
      case 23:
        this.toggleButton_1 = (ToggleButton) target;
        break;
      case 24:
        this.dataGrid_0 = (DataGrid) target;
        this.dataGrid_0.DragOver += new DragEventHandler(this.dataGrid_0_DragOver);
        this.dataGrid_0.DragLeave += new DragEventHandler(this.dataGrid_0_DragLeave);
        this.dataGrid_0.DragEnter += new DragEventHandler(this.dataGrid_0_DragEnter);
        this.dataGrid_0.Drop += new DragEventHandler(this.dataGrid_0_Drop);
        this.dataGrid_0.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.dataGrid_0_PreviewMouseLeftButtonDown);
        this.dataGrid_0.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(this.method_1);
        this.dataGrid_0.SelectionChanged += new SelectionChangedEventHandler(this.dataGrid_0_SelectionChanged);
        this.dataGrid_0.IsKeyboardFocusWithinChanged += new DependencyPropertyChangedEventHandler(this.dataGrid_0_IsKeyboardFocusWithinChanged);
        this.dataGrid_0.ColumnReordered += new EventHandler<DataGridColumnEventArgs>(this.method_41);
        this.dataGrid_0.Loaded += new RoutedEventHandler(this.dataGrid_0_Loaded);
        this.dataGrid_0.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.dataGrid_0_GotKeyboardFocus);
        break;
      case 25:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_13);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_47);
        break;
      case 26:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_14);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_46);
        break;
      case 27:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_17);
        break;
      case 28:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 29:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 30:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 31 /*0x1F*/:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_9);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_7);
        break;
      case 32 /*0x20*/:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_49);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_50);
        break;
      case 33:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_52);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_53);
        break;
      case 36:
        this.dataGridTemplateColumn_0 = (DataGridTemplateColumn) target;
        break;
      default:
        this.bool_7 = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 20:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.method_58);
        break;
      case 34:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = MenuItem.CheckedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.method_51)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = MenuItem.UncheckedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.method_51)
        });
        break;
      case 35:
        ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.method_57);
        break;
      case 37:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = FrameworkElement.RequestBringIntoViewEvent,
          Handler = (Delegate) new RequestBringIntoViewEventHandler(this.method_48)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.GotKeyboardFocusEvent,
          Handler = (Delegate) new KeyboardFocusChangedEventHandler(this.method_43)
        });
        break;
      case 38:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.method_56);
        break;
    }
  }

  [CompilerGenerated]
  internal static string smethod_1(string string_0)
  {
    switch (string_0)
    {
      case "Step Name":
        return "Name";
      case "Step Type":
        return "Type";
      default:
        return string_0;
    }
  }

  public class ExtraColumn
  {
    public bool isSelected;
    public TestPlanGrid grid;

    public string Name { get; set; }

    public bool IsSelected
    {
      get => this.isSelected;
      set
      {
        this.isSelected = value;
        if (!this.isSelected)
          return;
        this.grid.LoadMonitorVariable(this.Member);
        this.grid.updateView();
      }
    }

    public IMemberData Member { get; set; }
  }

  public delegate void TestPlanChangedDelegate();

  private class Class151
  {
    public ITestStep[] itestStep_0;
    public ITypeData[] itypeData_0;

    public Class151(ITestStep[] itestStep_1)
    {
      this.itestStep_0 = itestStep_1;
      this.itypeData_0 = ((IEnumerable<ITestStep>) itestStep_1).Select<ITestStep, ITypeData>((Func<ITestStep, ITypeData>) (itestStep_0 => TypeData.GetTypeData((object) itestStep_0))).Distinct<ITypeData>().ToArray<ITypeData>();
    }

    public Class151(ITypeData itypeData_1)
    {
      this.itypeData_0 = new ITypeData[1]{ itypeData_1 };
    }

    public override string ToString()
    {
      return this.itestStep_0 != null ? string.Join(Environment.NewLine, ((IEnumerable<ITestStep>) this.itestStep_0).Select<ITestStep, string>((Func<ITestStep, string>) (itestStep_0 => itestStep_0.Name))) : string.Join(Environment.NewLine, ((IEnumerable<ITypeData>) this.itypeData_0).Select<ITypeData, string>((Func<ITypeData, string>) (itypeData_0 => itypeData_0.Name)));
    }

    public static TestPlanGrid.Class151 smethod_0(DragEventArgs dragEventArgs_0)
    {
      try
      {
        object obj = dragEventArgs_0.Data.GetData("step") ?? dragEventArgs_0.Data.GetData("steptype");
        switch (obj)
        {
          case ITypeData _:
            return new TestPlanGrid.Class151((ITypeData) obj);
          case ITestStep[] _:
            return new TestPlanGrid.Class151((ITestStep[]) obj);
        }
      }
      catch (COMException ex)
      {
      }
      return (TestPlanGrid.Class151) null;
    }
  }

  private enum HoverStates
  {
    FirstChild,
    NextSibling,
    CannotDropHere,
  }

  private class Class153
  {
    [CompilerGenerated]
    [SpecialName]
    public ITestStepParent method_0() => this.itestStepParent_0;

    [CompilerGenerated]
    [SpecialName]
    public TestPlanGrid.HoverStates method_2() => this.hoverStates_0;

    public Class153(ITestStepParent itestStepParent_1, TestPlanGrid.HoverStates hoverStates_1)
    {
      // ISSUE: reference to a compiler-generated method
      this.method_1(itestStepParent_1);
      // ISSUE: reference to a compiler-generated method
      this.method_3(hoverStates_1);
    }

    public override string ToString() => $"{this.method_2()} {this.method_0()}";
  }

  private class Class154 : StyleSelector
  {
    private string method_0(object object_0)
    {
      switch (object_0)
      {
        case TestPlanGrid.ExtraColumn _:
          return "ExtraColumnStyle";
        case ColumnVisibilityDetails _:
          return "ColumnVisibilityStyle";
        default:
          return "DefaultStyle";
      }
    }

    public override Style SelectStyle(object item, DependencyObject container)
    {
      return (container as MenuItem).FindResource((object) this.method_0(item)) as Style;
    }
  }
}
