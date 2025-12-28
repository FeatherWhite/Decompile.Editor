// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropertyDataGrid
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropertyDataGrid : DataGrid
{
  public static RoutedUICommand CopyRow = new RoutedUICommand("Copy Row", nameof (CopyRow), typeof (PropertyDataGrid));
  public static RoutedUICommand PasteRow = new RoutedUICommand("Paste Row", nameof (PasteRow), typeof (PropertyDataGrid));
  public static RoutedUICommand DeleteRow = new RoutedUICommand("Delete Row", nameof (DeleteRow), typeof (PropertyDataGrid));
  public static RoutedUICommand AddRow = new RoutedUICommand("Add Row", nameof (AddRow), typeof (PropertyDataGrid));
  private bool bool_0;
  private static TraceSource traceSource_0 = Log.CreateSource("GUI");

  protected override void OnCellEditEnding(
    DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs_0)
  {
    base.OnCellEditEnding(dataGridCellEditEndingEventArgs_0);
    this.RaiseOnEdited();
  }

  private void method_0()
  {
    for (int index = 0; index < this.Items.Count; ++index)
    {
      if (this.ItemContainerGenerator.ContainerFromIndex(index) is DataGridRow dpObj)
      {
        foreach (DataGridCell cell in dpObj.GetVisualChildren().OfType<DataGridCell>())
        {
          if (!cell.IsEditing && cell.Column is PropertyDataGrid.PropertyNameDataGridColumn column)
            column.ReloadCell(cell);
        }
      }
    }
    this.bool_0 = false;
  }

  internal AnnotationDataGridViewModel ViewModel { get; } = new AnnotationDataGridViewModel();

  public void LoadData(AnnotationCollection annotations)
  {
    this.ViewModel.LoadData(annotations);
    this.ItemsSource = (IEnumerable) this.ViewModel.Rows;
    this.CanUserDeleteRows = this.CanUserAddRows = this.ViewModel.IsDynamicSize;
    this.method_14();
  }

  private Dictionary<DisplayAttribute, string> method_1(IEnumerable<DisplayAttribute> ienumerable_0)
  {
    Dictionary<string, string> fullStrings = new Dictionary<string, string>();
    foreach (DisplayAttribute displayAttribute in ienumerable_0)
      fullStrings[displayAttribute.GetFullName()] = displayAttribute.Name;
label_7:
    IEnumerable<IGrouping<string, KeyValuePair<string, string>>> source = fullStrings.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (keyValuePair_0 => keyValuePair_0.Key != keyValuePair_0.Value)).GroupBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (keyValuePair_0 => keyValuePair_0.Value)).Where<IGrouping<string, KeyValuePair<string, string>>>((Func<IGrouping<string, KeyValuePair<string, string>>, bool>) (igrouping_0 => igrouping_0.Count<KeyValuePair<string, string>>() > 1));
    if (!source.Any<IGrouping<string, KeyValuePair<string, string>>>())
      return ienumerable_0.Distinct<DisplayAttribute>().ToDictionary<DisplayAttribute, DisplayAttribute, string>((Func<DisplayAttribute, DisplayAttribute>) (displayAttribute_0 => displayAttribute_0), (Func<DisplayAttribute, string>) (displayAttribute_0 => fullStrings[displayAttribute_0.GetFullName()]));
    using (IEnumerator<IGrouping<string, KeyValuePair<string, string>>> enumerator = source.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        IGrouping<string, KeyValuePair<string, string>> current = enumerator.Current;
        string key = current.Key;
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) current)
        {
          string str1 = keyValuePair.Key.Substring(0, keyValuePair.Key.Length - keyValuePair.Value.Length - 3);
          int num = str1.LastIndexOf('\\');
          string str2 = keyValuePair.Key;
          if (num != -1)
            str2 = $"{str1.Substring(num + 1)} \\ {key}";
          fullStrings[keyValuePair.Key] = str2;
        }
      }
      goto label_7;
    }
  }

  private void method_2()
  {
    Dictionary<DisplayAttribute, string> dictionary = this.method_1(this.Columns.OfType<PropertyDataGrid.PropertyNameDataGridColumn>().Where<PropertyDataGrid.PropertyNameDataGridColumn>((Func<PropertyDataGrid.PropertyNameDataGridColumn, bool>) (propertyNameDataGridColumn_0 => propertyNameDataGridColumn_0.Display != null)).Select<PropertyDataGrid.PropertyNameDataGridColumn, DisplayAttribute>((Func<PropertyDataGrid.PropertyNameDataGridColumn, DisplayAttribute>) (propertyNameDataGridColumn_0 => propertyNameDataGridColumn_0.Display)));
    foreach (PropertyDataGrid.PropertyNameDataGridColumn nameDataGridColumn in this.Columns.OfType<PropertyDataGrid.PropertyNameDataGridColumn>().Where<PropertyDataGrid.PropertyNameDataGridColumn>((Func<PropertyDataGrid.PropertyNameDataGridColumn, bool>) (propertyNameDataGridColumn_0 => propertyNameDataGridColumn_0.Display != null)))
      nameDataGridColumn.SetHeaderText(dictionary[nameDataGridColumn.Display]);
  }

  static PropertyDataGrid()
  {
    CommandManager.RegisterClassCommandBinding(typeof (PropertyDataGrid), new CommandBinding((ICommand) ApplicationCommands.Paste, new ExecutedRoutedEventHandler(PropertyDataGrid.smethod_1), new CanExecuteRoutedEventHandler(PropertyDataGrid.smethod_0)));
  }

  public PropertyDataGrid()
  {
    this.RowHeaderWidth = double.NaN;
    this.MouseDown += new MouseButtonEventHandler(this.PropertyDataGrid_MouseDown);
    GenericGui.UpdateHelpLink((object) this, (DependencyObject) this);
    CommandBinding commandBinding1 = new CommandBinding((ICommand) PropertyDataGrid.CopyRow);
    commandBinding1.Executed += new ExecutedRoutedEventHandler(this.method_10);
    commandBinding1.CanExecute += new CanExecuteRoutedEventHandler(this.method_6);
    this.CommandBindings.Add(commandBinding1);
    CommandBinding commandBinding2 = new CommandBinding((ICommand) PropertyDataGrid.PasteRow);
    commandBinding2.Executed += new ExecutedRoutedEventHandler(this.method_9);
    this.CommandBindings.Add(commandBinding2);
    CommandBinding commandBinding3 = new CommandBinding((ICommand) PropertyDataGrid.DeleteRow);
    commandBinding3.Executed += new ExecutedRoutedEventHandler(this.method_8);
    commandBinding3.CanExecute += new CanExecuteRoutedEventHandler(this.method_5);
    this.CommandBindings.Add(commandBinding3);
    CommandBinding commandBinding4 = new CommandBinding((ICommand) PropertyDataGrid.AddRow);
    commandBinding4.Executed += new ExecutedRoutedEventHandler(this.method_4);
    commandBinding4.CanExecute += new CanExecuteRoutedEventHandler(this.method_3);
    this.CommandBindings.Add(commandBinding4);
  }

  private void method_3(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.CanUserAddRows;
  }

  private void method_4(object sender, ExecutedRoutedEventArgs e) => this.method_12(true);

  private void method_5(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = !this.method_7() && this.CanUserDeleteRows;
  }

  private void method_6(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = !this.method_7();
  }

  private bool method_7()
  {
    object obj = this.SelectedItem ?? this.SelectedCells.FirstOrDefault<DataGridCellInfo>().Item;
    return obj == null || IsNewItemPlaceholderConverter.IsNewItem(obj);
  }

  private void method_8(object sender, ExecutedRoutedEventArgs e)
  {
    this.method_11((RoutedEventArgs) e);
    this.method_16();
  }

  private void method_9(object sender, ExecutedRoutedEventArgs e)
  {
    this.method_11((RoutedEventArgs) e);
    PropertyDataGrid.smethod_1(sender, e);
  }

  private void method_10(object sender, ExecutedRoutedEventArgs e)
  {
    this.method_11((RoutedEventArgs) e);
    this.OnExecutedCopy((ExecutedRoutedEventArgs) null);
  }

  private void method_11(RoutedEventArgs routedEventArgs_0)
  {
    if (this.SelectedCells == null)
      return;
    foreach (DataGridCellInfo selectedCell in (IEnumerable<DataGridCellInfo>) this.SelectedCells)
    {
      DataGridRow dataGridRow = (DataGridRow) this.ItemContainerGenerator.ContainerFromItem(selectedCell.Item);
      if (dataGridRow != null)
        dataGridRow.IsSelected = true;
    }
  }

  private void PropertyDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    this.CommitEdit();
    this.CommitEdit();
  }

  public event EventHandler<NotifyCollectionChangedEventArgs> ItemsChanged;

  protected override void OnItemsChanged(
    NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs_0)
  {
    base.OnItemsChanged(notifyCollectionChangedEventArgs_0);
    if (notifyCollectionChangedEventArgs_0.Action == NotifyCollectionChangedAction.Add && this.AutoGenerateColumns)
      this.OnAutoGeneratedColumns((EventArgs) null);
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, notifyCollectionChangedEventArgs_0);
  }

  protected override void OnLoadingRow(DataGridRowEventArgs dataGridRowEventArgs_0)
  {
    base.OnLoadingRow(dataGridRowEventArgs_0);
    dataGridRowEventArgs_0.Row.Header = (object) new PropertyDataGrid.HeaderIndexCalculator()
    {
      Row = dataGridRowEventArgs_0.Row
    };
  }

  private void method_12(bool bool_1)
  {
    if (this.ViewModel.IsComplicatedType)
    {
      Type elementType = this.ViewModel.ElementType;
      if (elementType == (Type) null)
        return;
      if (elementType.IsAbstract)
      {
        NewStepDialog newStepDialog = new NewStepDialog(elementType);
        newStepDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        newStepDialog.Owner = Window.GetWindow((DependencyObject) this);
        newStepDialog.Title = "Add New " + elementType.GetDisplayAttribute().Name;
        NewStepDialog newStepDialog_0 = newStepDialog;
        string str;
        switch (elementType.GetDisplayAttribute().Name)
        {
          case "Instrument":
            str = "EditorHelp.chm::/Configurations/Resource Configuration/Instrument Configuration/Readme.html";
            break;
          case "Dut":
            str = "EditorHelp.chm::/Configurations/Resource Configuration/DUT Configuration.html";
            break;
          case "Generic Connection":
            str = "EditorHelp.chm::/Configurations/Resource Configuration/Connection Configuration.html";
            break;
          case "Step":
            str = "EditorHelp.chm::/CreatingATestPlan/Working With Test Steps/Adding a New Step.html";
            break;
          case "Result Listener":
            str = "EditorHelp.chm::/Configurations/Result Listener Configuration/Readme.html";
            break;
          default:
            str = "EditorHelp.chm::/Configurations/Resource Configuration/Readme.html";
            break;
        }
        HelpManager.SetHelpLink((DependencyObject) newStepDialog_0, (object) str);
        newStepDialog_0.NewStepControl.OnAddStep += (Action<ITypeData>) (itypeData_0 =>
        {
          AnnotationCollection annotationCollection = this.ViewModel.AddRow(AnnotationCollection.Annotate(ReflectionDataExtensions.CreateInstance(itypeData_0), Array.Empty<IAnnotation>()));
          this.ItemsSource = (IEnumerable) this.ViewModel.Rows;
          this.SelectedItem = (object) annotationCollection;
          this.method_14();
          if (annotationCollection == null)
            return;
          this.ScrollIntoView((object) annotationCollection);
        });
        newStepDialog_0.Loaded += (RoutedEventHandler) ((sender, e) => newStepDialog_0.GetLogicalChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (frameworkElement_0 => frameworkElement_0.Focusable))?.Focus());
        GuiHelpers.GuiInvokeAsync((Action) (() => newStepDialog_0.ShowDialog()));
        return;
      }
    }
    if (!bool_1)
      return;
    AnnotationCollection annotationCollection1 = this.method_15();
    this.ItemsSource = (IEnumerable) this.ViewModel.Rows;
    this.SelectedItem = (object) annotationCollection1;
    if (annotationCollection1 == null)
      return;
    this.ScrollIntoView((object) annotationCollection1);
  }

  protected override void OnExecutedBeginEdit(ExecutedRoutedEventArgs executedRoutedEventArgs_0)
  {
    if (!((executedRoutedEventArgs_0.OriginalSource is FrameworkElement originalSource ? originalSource.DataContext : (object) null) is AnnotationCollection))
    {
      this.method_12(false);
      if (this.ViewModel.IsComplicatedType)
      {
        executedRoutedEventArgs_0.Handled = true;
        return;
      }
    }
    base.OnExecutedBeginEdit(executedRoutedEventArgs_0);
  }

  private void method_13() => this.ViewModel.PushUpdate();

  private bool method_14()
  {
    if (this.ViewModel.Columns.Count == this.Columns.Count && this.ViewModel.Columns.Select<AnnotationDataGridViewModel.ColumnData, string>((Func<AnnotationDataGridViewModel.ColumnData, string>) (columnData_0 => columnData_0.Name)).SequenceEqual<string>(this.Columns.OfType<PropertyDataGrid.PropertyNameDataGridColumn>().Select<PropertyDataGrid.PropertyNameDataGridColumn, string>((Func<PropertyDataGrid.PropertyNameDataGridColumn, string>) (propertyNameDataGridColumn_0 => propertyNameDataGridColumn_0.PropertyName))))
      return false;
    this.Columns.Clear();
    foreach (AnnotationDataGridViewModel.ColumnData column in this.ViewModel.Columns)
      this.Columns.Add((DataGridColumn) PropertyDataGrid.PropertyNameDataGridColumn.FromColumnData(column));
    this.method_2();
    return true;
  }

  private AnnotationCollection method_15()
  {
    int count = this.ViewModel.Rows.Count;
    AnnotationCollection annotationCollection = this.ViewModel.AddRow();
    this.bool_0 = true;
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      if (!this.bool_0)
        return;
      this.method_0();
    }), priority: DispatcherPriority.ContextIdle);
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      if (!this.method_14())
        return;
      GuiHelpers.GuiInvokeAsync((Action) (() =>
      {
        if (!(this.ItemContainerGenerator.ContainerFromIndex(0) is DataGridRow dpObj2))
          return;
        using (IEnumerator<DataGridCell> enumerator = dpObj2.GetVisualChildren().OfType<DataGridCell>().GetEnumerator())
        {
          if (!enumerator.MoveNext())
            return;
          DataGridCell current = enumerator.Current;
          current.Focus();
          FocusManager.SetFocusedElement((DependencyObject) current, (IInputElement) this);
        }
      }), priority: DispatcherPriority.ContextIdle);
    }));
    return annotationCollection;
  }

  protected override void OnAddingNewItem(AddingNewItemEventArgs addingNewItemEventArgs_0)
  {
    addingNewItemEventArgs_0.NewItem = (object) this.method_15();
    base.OnAddingNewItem(addingNewItemEventArgs_0);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs mouseButtonEventArgs_0)
  {
    if (this.bool_0)
      this.method_0();
    base.OnPreviewMouseLeftButtonDown(mouseButtonEventArgs_0);
  }

  protected override void OnPreviewTextInput(
    TextCompositionEventArgs textCompositionEventArgs_0)
  {
    base.OnPreviewTextInput(textCompositionEventArgs_0);
    if (textCompositionEventArgs_0.Text == "\u001B" || textCompositionEventArgs_0.Text == "\b")
      return;
    this.BeginEdit((RoutedEventArgs) textCompositionEventArgs_0);
  }

  private void method_16()
  {
    if (!this.CanUserDeleteRows)
      throw new Exception("Items cannot be deleted from the collection");
    this.CancelEdit();
    this.CancelEdit();
    IList itemsSource = this.ItemsSource as IList;
    object[] array = this.SelectedCells.Select<DataGridCellInfo, object>((Func<DataGridCellInfo, object>) (dataGridCellInfo_0 => dataGridCellInfo_0.Item)).ToArray<object>();
    int val1 = -1;
    if (itemsSource != null && !itemsSource.IsReadOnly)
    {
      IList list = (IList) itemsSource.Cast<AnnotationCollection>().ToList<AnnotationCollection>();
      foreach (object obj in array)
      {
        int index = list.IndexOf(obj);
        if (index != -1)
        {
          list.RemoveAt(index);
          val1 = index;
        }
      }
      this.ItemsSource = (IEnumerable) list;
      this.Items.Refresh();
    }
    this.ViewModel.SetRows(this.ItemsSource.Cast<AnnotationCollection>());
    this.ItemsSource = (IEnumerable) this.ViewModel.Rows;
    int index1 = Math.Min(val1, this.ViewModel.Rows.Count - 1);
    if (index1 <= -1)
      return;
    AnnotationCollection row = this.ViewModel.Rows[index1];
    this.SelectedItem = (object) row;
    this.ScrollIntoView((object) row);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs keyEventArgs_0)
  {
    base.OnPreviewKeyDown(keyEventArgs_0);
    if (keyEventArgs_0.Key == Key.Delete && (keyEventArgs_0.OriginalSource is DataGridCell || keyEventArgs_0.OriginalSource is DataGridRow) && this.CanUserDeleteRows)
      this.method_16();
    if (keyEventArgs_0.Key != Key.Space || this.SelectedCells.Count <= 1)
      return;
    AnnotationCollection[] array = this.SelectedCells.Select<DataGridCellInfo, object>((Func<DataGridCellInfo, object>) (dataGridCellInfo_0 => dataGridCellInfo_0.Item)).OfType<AnnotationCollection>().Distinct<AnnotationCollection>().ToArray<AnnotationCollection>();
    bool? nullable = new bool?();
    foreach (AnnotationCollection annotationCollection1 in array)
    {
      IEnumerable<AnnotationCollection> members = annotationCollection1.Get<IMembersAnnotation>(false, (object) null)?.Members;
      if (members != null)
      {
        AnnotationCollection annotationCollection2 = members.FirstOrDefault<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 =>
        {
          IMemberAnnotation imemberAnnotation = annotationCollection_0.Get<IMemberAnnotation>(false, (object) null);
          return (imemberAnnotation != null ? ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) imemberAnnotation.Member).Name : (string) null) == "Enabled";
        }));
        if (annotationCollection2 != null && annotationCollection2.Get<IObjectValueAnnotation>(false, (object) null)?.Value is bool flag)
        {
          if (!nullable.HasValue)
            nullable = new bool?(flag);
          annotationCollection2.Get<IObjectValueAnnotation>(false, (object) null).Value = (object) !nullable.Value;
          annotationCollection2.Write();
        }
      }
    }
    if (!nullable.HasValue)
      return;
    this.CancelEdit();
    this.CancelEdit();
    this.Items.Refresh();
    this.Focus();
    keyEventArgs_0.Handled = true;
  }

  private (int x, int y) method_17(DataGridCellInfo dataGridCellInfo_0)
  {
    return (dataGridCellInfo_0.Column.DisplayIndex, this.Items.IndexOf(dataGridCellInfo_0.Item));
  }

  protected override void OnExecutedCopy(ExecutedRoutedEventArgs args)
  {
    if (this.SelectedCells == null || this.method_7())
      return;
    HashSet<(int, int)> positions = new HashSet<(int, int)>();
    foreach (DataGridCellInfo selectedCell in (IEnumerable<DataGridCellInfo>) this.SelectedCells)
    {
      (int, int) valueTuple = this.method_17(selectedCell);
      positions.Add(valueTuple);
    }
    ClipboardHelper.CopyText(this.ViewModel.CopyToString(positions));
  }

  private static void smethod_0(object sender, CanExecuteRoutedEventArgs e)
  {
    ((PropertyDataGrid) sender).OnCanExecutePaste(e);
  }

  private static void smethod_1(object sender, ExecutedRoutedEventArgs e)
  {
    try
    {
      ((PropertyDataGrid) sender).OnExecutedPaste(e);
    }
    catch (Exception ex)
    {
      QuickDialog.ShowMessage("Paste Error", "Unable to paste data from clipboard.");
      Log.Error(PropertyDataGrid.traceSource_0, "Unable to paste data from clipboard: " + ex.Message, Array.Empty<object>());
      Log.Debug(PropertyDataGrid.traceSource_0, ex);
    }
  }

  protected virtual void OnExecutedPaste(ExecutedRoutedEventArgs args)
  {
    string text = Clipboard.GetText();
    HashSet<(int, int)> positions = new HashSet<(int, int)>();
    foreach (DataGridCellInfo selectedCell in (IEnumerable<DataGridCellInfo>) this.SelectedCells)
    {
      (int, int) valueTuple = this.method_17(selectedCell);
      positions.Add(valueTuple);
    }
    this.CommitEdit();
    this.CommitEdit();
    this.ViewModel.PasteFromString(text, positions);
    this.ItemsSource = (IEnumerable) this.ViewModel.Rows;
    this.method_14();
    this.UnselectAll();
    this.UnselectAllCells();
    this.UpdateLayout();
    this.method_13();
    if (args == null)
      return;
    args.Handled = true;
  }

  protected virtual void OnCanExecutePaste(
    CanExecuteRoutedEventArgs canExecuteRoutedEventArgs_0)
  {
    canExecuteRoutedEventArgs_0.CanExecute = Clipboard.ContainsText();
    canExecuteRoutedEventArgs_0.Handled = true;
  }

  protected override void OnAutoGeneratingColumn(
    DataGridAutoGeneratingColumnEventArgs dataGridAutoGeneratingColumnEventArgs_0)
  {
    dataGridAutoGeneratingColumnEventArgs_0.Cancel = true;
  }

  public event EventHandler OnEdited;

  public void RaiseOnEdited()
  {
    this.ViewModel.Write();
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler1 = this.eventHandler_1;
    if (eventHandler1 == null)
      return;
    eventHandler1((object) this, EventArgs.Empty);
  }

  public class PropertyNameDataGridColumn : DataGridColumn
  {
    public readonly string PropertyName;
    private readonly string string_0;
    public double Order;

    private static DataGridLength constDefaultWidth
    {
      get => new DataGridLength(0.0, DataGridLengthUnitType.Auto);
    }

    public DisplayAttribute Display { get; }

    private PropertyNameDataGridColumn(
      AnnotationDataGridViewModel.ColumnData columnData_0)
    {
      this.PropertyName = columnData_0.Name;
      this.Display = columnData_0.Display;
      this.Header = (object) new ColumnHeaderItem()
      {
        Header = this.Display.Name,
        FullName = this.Display.GetFullName(),
        Description = this.Display.Description
      };
      this.string_0 = columnData_0.Context;
      this.Width = PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.GetLength(columnData_0?.ToString() + this.string_0) ?? PropertyDataGrid.PropertyNameDataGridColumn.constDefaultWidth;
    }

    public void SetHeaderText(string text)
    {
      if (this.Header is ColumnHeaderItem header)
        this.Header = (object) new ColumnHeaderItem()
        {
          Header = text,
          FullName = header.FullName,
          Description = header.Description
        };
      if (!(this.Header is string))
        return;
      this.Header = (object) text;
    }

    protected override void OnPropertyChanged(
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
    {
      base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
      string str = this.PropertyName ?? "NULL";
      if (this.Header != null)
      {
        if (this.Header is TextBlock)
          str = ((TextBlock) this.Header).Text;
        else if (this.Header is string)
          str = (string) this.Header;
      }
      if (this.string_0 == null)
        return;
      if (dependencyPropertyChangedEventArgs_0.Property == DataGridColumn.WidthProperty)
        PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.SetLength(str + this.string_0, (DataGridLength) dependencyPropertyChangedEventArgs_0.NewValue);
      if (dependencyPropertyChangedEventArgs_0.Property != DataGridColumn.HeaderProperty)
        return;
      this.Width = PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.GetLength(str + this.string_0) ?? PropertyDataGrid.PropertyNameDataGridColumn.constDefaultWidth;
    }

    private AnnotationCollection method_0(AnnotationCollection annotationCollection_0, bool bool_0 = true)
    {
      AnnotationCollection annotationCollection = annotationCollection_0;
      IMembersAnnotation imembersAnnotation = annotationCollection.Get<IMembersAnnotation>(false, (object) null);
      if (imembersAnnotation == null)
        return annotationCollection;
      foreach (AnnotationCollection member1 in imembersAnnotation.Members)
      {
        DisplayAttribute displayAttribute = member1.Get<DisplayAttribute>(false, (object) null);
        IMemberData member2 = member1.Get<IMemberAnnotation>(false, (object) null).Member;
        if (displayAttribute.GetFullName() + ((IReflectionData) member2.TypeDescriptor).Name == this.PropertyName)
        {
          if (bool_0)
          {
            annotationCollection = member1.Clone();
            annotationCollection.Read();
            break;
          }
          annotationCollection = member1;
          break;
        }
      }
      return annotationCollection == annotationCollection_0 ? (AnnotationCollection) null : annotationCollection;
    }

    private AnnotationCollection method_1(
      DataGridCell dataGridCell_0,
      AnnotationCollection annotationCollection_0,
      bool bool_0 = true)
    {
      int index = DataGridRow.GetRowContainingElement((FrameworkElement) dataGridCell_0).GetIndex();
      if (annotationCollection_0?.ParentAnnotation == null)
        return (AnnotationCollection) null;
      IList<AnnotationCollection> rows = (this.DataGridOwner as PropertyDataGrid).ViewModel.Rows;
      return rows == null ? this.method_0(annotationCollection_0, bool_0) : this.method_0(rows[index], bool_0);
    }

    private AnnotationCollection method_2(DataGridCell dataGridCell_0)
    {
      int index = DataGridRow.GetRowContainingElement((FrameworkElement) dataGridCell_0).GetIndex();
      AnnotationDataGridViewModel viewModel = this.DataGridOwner is PropertyDataGrid dataGridOwner ? dataGridOwner.ViewModel : (AnnotationDataGridViewModel) null;
      return viewModel.Rows.Count <= index ? (AnnotationCollection) null : viewModel.Rows[index];
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
      AnnotationCollection annotation = this.method_1(cell, dataItem as AnnotationCollection);
      if (annotation == null)
        return (FrameworkElement) null;
      annotation.RemoveType<GuiOptions>();
      annotation.RemoveType<ReadOnlyViewAnnotation>();
      GuiOptions guiOptions1 = annotation.Get<GuiOptions>(false, (object) null);
      if (guiOptions1 == null)
        guiOptions1 = new GuiOptions() { FullRow = true };
      GuiOptions guiOptions2 = guiOptions1;
      annotation.Add((IAnnotation) guiOptions2);
      HelpManager.SetHelpLink((DependencyObject) cell, (object) annotation.Get<HelpLinkAttribute>(false, (object) null)?.HelpLink);
      ItemUi itemUi = GenericGui.CreateItemUi(annotation);
      itemUi.HideTitle = true;
      itemUi.Control.SourceUpdated += (EventHandler<DataTransferEventArgs>) ((sender, e) =>
      {
        AnnotationCollection objA = this.method_1(cell, dataItem as AnnotationCollection, false);
        if (objA != null && !object.Equals((object) objA, (object) annotation))
        {
          objA.Get<IObjectValueAnnotation>(false, (object) null).Value = annotation.Get<IObjectValueAnnotation>(false, (object) null).Value;
          objA.Write();
        }
        annotation?.Write();
      });
      return (FrameworkElement) new ContentPresenter()
      {
        Content = (object) itemUi
      };
    }

    protected void onEdited(AnnotationCollection annotation)
    {
      annotation?.Write();
      if (this.DataGridOwner is PropertyDataGrid dataGridOwner)
        dataGridOwner.RaiseOnEdited();
      annotation?.Read();
      annotation?.Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
    }

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyDataGrid.PropertyNameDataGridColumn.\u003C\u003Ec__DisplayClass17_0 cDisplayClass170 = new PropertyDataGrid.PropertyNameDataGridColumn.\u003C\u003Ec__DisplayClass17_0()
      {
        \u003C\u003E4__this = this,
        cell = cell,
        dataItem = dataItem
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.annotation = this.method_1(cDisplayClass170.cell, cDisplayClass170.dataItem as AnnotationCollection);
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass170.annotation == null)
        return (FrameworkElement) null;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.annotation.RemoveType<GuiOptions>();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.annotation.RemoveType<ReadOnlyViewAnnotation>();
      ReadOnlyViewAnnotation onlyViewAnnotation = new ReadOnlyViewAnnotation();
      GuiOptions guiOptions = new GuiOptions()
      {
        FullRow = true
      };
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.annotation.Add(new IAnnotation[2]
      {
        (IAnnotation) onlyViewAnnotation,
        (IAnnotation) guiOptions
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      HelpManager.SetHelpLink((DependencyObject) cDisplayClass170.cell, (object) cDisplayClass170.annotation.Get<HelpLinkAttribute>(false, (object) null)?.HelpLink);
      // ISSUE: reference to a compiler-generated field
      FrameworkElement frameworkElement = GenericGui.Create(cDisplayClass170.annotation);
      ControlProvider.SetSingleLineView((DependencyObject) frameworkElement, true);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.ctrl = new ContentControl()
      {
        Content = (object) frameworkElement
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass170.ctrl.SourceUpdated += new EventHandler<DataTransferEventArgs>(cDisplayClass170.\u003CGenerateElement\u003Eb__0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      cDisplayClass170.error = cDisplayClass170.annotation.GetAll<IErrorAnnotation>(false).ToArray<IErrorAnnotation>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass170.annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated(frameworkElement, new Action(cDisplayClass170.method_0));
      // ISSUE: reference to a compiler-generated method
      cDisplayClass170.method_0();
      // ISSUE: reference to a compiler-generated field
      return (FrameworkElement) cDisplayClass170.ctrl;
    }

    protected override object PrepareCellForEdit(
      FrameworkElement editingElement,
      RoutedEventArgs editingEventArgs)
    {
      GuiHelpers.GuiInvokeAsync((Action) (() =>
      {
        TextBox visualChild = editingElement.FindVisualChild<TextBox>();
        if (visualChild == null)
          return;
        GuiHelpers.MoveFocusTo((FrameworkElement) visualChild);
        if (editingEventArgs is TextCompositionEventArgs compositionEventArgs2)
        {
          visualChild.Text = compositionEventArgs2.Text;
          visualChild.CaretIndex = visualChild.Text.Length;
        }
        else
          visualChild.SelectAll();
      }), priority: DispatcherPriority.Loaded);
      return (object) null;
    }

    protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
    {
      base.CancelCellEdit(editingElement, uneditedValue);
      this.method_3();
    }

    private void method_3()
    {
      string str = this.PropertyName ?? "NULL";
      if (this.Header != null)
      {
        if (this.Header is TextBlock header2)
          str = header2.Text;
        else if (this.Header is string header1)
          str = header1;
      }
      PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.SetLength(str + this.string_0, this.Width);
    }

    protected override bool CommitCellEdit(FrameworkElement editingElement)
    {
      this.method_3();
      return base.CommitCellEdit(editingElement);
    }

    public override object OnCopyingCellClipboardContent(object item)
    {
      IStringValueAnnotation istringValueAnnotation = this.method_0((AnnotationCollection) item, false).Get<IStringValueAnnotation>(false, (object) null);
      return istringValueAnnotation == null ? (object) null : (object) istringValueAnnotation.Value;
    }

    public override void OnPastingCellClipboardContent(object item, object cellContent)
    {
      if (item is AnnotationCollection annotationCollection_0)
      {
        IStringValueAnnotation istringValueAnnotation = this.method_0(annotationCollection_0, false).Get<IStringValueAnnotation>(false, (object) null);
        if (istringValueAnnotation != null)
          istringValueAnnotation.Value = cellContent as string;
      }
      base.OnPastingCellClipboardContent(item, cellContent);
    }

    public void ReloadCell(DataGridCell cell)
    {
      AnnotationCollection dataItem = this.method_2(cell);
      if (dataItem == null || dataItem == cell.DataContext)
        return;
      cell.Content = (object) this.GenerateElement(cell, (object) dataItem);
      cell.DataContext = (object) dataItem;
    }

    internal static PropertyDataGrid.PropertyNameDataGridColumn FromColumnData(
      AnnotationDataGridViewModel.ColumnData column)
    {
      return new PropertyDataGrid.PropertyNameDataGridColumn(column);
    }

    private static class PersistedColumnWidth
    {
      private static Dictionary<string, double> Lengths
      {
        get => ComponentSettings<GuiControlsSettings>.Current.PropertyDataGridColumnWidth;
      }

      public static DataGridLength? GetLength(string columnName)
      {
        double pixels;
        return PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.Lengths.TryGetValue(columnName, out pixels) ? new DataGridLength?(new DataGridLength(pixels)) : new DataGridLength?();
      }

      public static void SetLength(string columnName, DataGridLength dataGridLength_0)
      {
        if (!dataGridLength_0.IsAbsolute)
          return;
        PropertyDataGrid.PropertyNameDataGridColumn.PersistedColumnWidth.Lengths[columnName] = dataGridLength_0.Value;
      }
    }
  }

  public class HeaderIndexCalculator
  {
    public DataGridRow Row;

    public int Index => this.Row.GetIndex() + 1;
  }
}
