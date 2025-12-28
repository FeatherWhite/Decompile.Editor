// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.PropertyColumn
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class PropertyColumn : DataGridColumn
{
  public static readonly DependencyProperty EditableProperty = DependencyProperty.RegisterAttached("Editable", typeof (bool), typeof (PropertyColumn));
  public ColumnViewProvider Provider;

  public static bool GetEditable(DependencyObject dependencyObject_0)
  {
    return (bool) dependencyObject_0.GetValue(PropertyColumn.EditableProperty);
  }

  public static void SetEditable(DependencyObject dependencyObject_0, bool value)
  {
    dependencyObject_0.SetValue(PropertyColumn.EditableProperty, (object) value);
  }

  public PropertyColumn(ColumnViewProvider provider, ColumnFilterContext columnFilterContext_0)
  {
    this.Provider = provider;
    this.Filter = new ColumnFilter(this.Provider.Name, columnFilterContext_0.CreateInstance(provider));
  }

  public PropertyColumn(ColumnViewProvider provider) => this.Provider = provider;

  public ColumnFilter Filter { get; private set; }

  protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PropertyColumn.Class136 class136 = new PropertyColumn.Class136();
    // ISSUE: reference to a compiler-generated field
    class136.dataGridCell_0 = cell;
    // ISSUE: reference to a compiler-generated field
    bool editFormExists = PropertyColumn.GetEditable((DependencyObject) class136.dataGridCell_0);
    TestStepRowItem testStepRowItem = (TestStepRowItem) dataItem;
    if (testStepRowItem.Step.IsReadOnly)
      editFormExists = false;
    if (!editFormExists)
    {
      // ISSUE: reference to a compiler-generated field
      return this.GenerateElement(class136.dataGridCell_0, dataItem);
    }
    AnnotationCollection annotation = ((TestStepRowItem) dataItem).Annotation;
    annotation.Read();
    ItemUi itemUi = this.Provider.GetItemUi(annotation, true, out editFormExists);
    // ISSUE: reference to a compiler-generated field
    class136.dataGridCell_0.SetValue(PropertyColumn.EditableProperty, (object) editFormExists);
    if (editFormExists)
    {
      // ISSUE: reference to a compiler-generated field
      class136.testPlanGrid_0 = (TestPlanGrid) null;
      // ISSUE: reference to a compiler-generated method
      itemUi.Control.Loaded += new RoutedEventHandler(class136.method_0);
      // ISSUE: reference to a compiler-generated method
      itemUi.Control.Unloaded += new RoutedEventHandler(class136.method_1);
    }
    // ISSUE: reference to a compiler-generated field
    class136.dataGridCell_0.IsEditing = editFormExists;
    Decorator editingElement = new Decorator();
    editingElement.Child = (UIElement) itemUi.Control;
    editingElement.Tag = (object) itemUi;
    // ISSUE: reference to a compiler-generated field
    editingElement.SetBinding(UIElement.IsEnabledProperty, (BindingBase) new MultiBinding()
    {
      Bindings = {
        (BindingBase) new Binding("IsReadOnly")
        {
          Source = (object) class136.dataGridCell_0
        },
        (BindingBase) new Binding("IsReadOnly")
        {
          Source = (object) testStepRowItem.Step
        },
        (BindingBase) new Binding("IsEnabled")
        {
          Source = (object) itemUi,
          Converter = (IValueConverter) new InvertBooleanConverter()
        }
      },
      Converter = (IMultiValueConverter) new NorConverter()
    });
    return (FrameworkElement) editingElement;
  }

  protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
  {
    return this.GenerateElement2((Decorator) null, cell, dataItem);
  }

  protected FrameworkElement GenerateElement2(Decorator result, DataGridCell cell, object dataItem)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PropertyColumn.Class137 class137 = new PropertyColumn.Class137();
    if (dataItem?.GetType().Name == "NamedObject")
      return (FrameworkElement) null;
    TestStepRowItem testStepRowItem = (TestStepRowItem) dataItem;
    // ISSUE: reference to a compiler-generated field
    class137.annotationCollection_0 = testStepRowItem.Annotation;
    // ISSUE: reference to a compiler-generated field
    IAccessAnnotation accessAnnotation = class137.annotationCollection_0.Get<IAccessAnnotation>();
    bool flag = accessAnnotation == null || !accessAnnotation.IsReadOnly;
    // ISSUE: reference to a compiler-generated field
    ItemUi itemUi = this.Provider.GetItemUi(class137.annotationCollection_0, false, out bool _);
    if (itemUi != null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class137.updateMonitor_0 = class137.annotationCollection_0.Get<UpdateMonitor>(true);
      // ISSUE: reference to a compiler-generated method
      itemUi.Control.Loaded += new RoutedEventHandler(class137.method_1);
      // ISSUE: reference to a compiler-generated method
      itemUi.Control.Unloaded += new RoutedEventHandler(class137.method_2);
      if (result == null)
      {
        Decorator decorator = new Decorator();
        decorator.Name = "PropertyColumnDecorator";
        result = decorator;
      }
      ControlProvider.SetSingleLineView((DependencyObject) result, true);
      result.Child = (UIElement) itemUi.Control;
      result.Tag = (object) itemUi;
      result.SetBinding(UIElement.IsEnabledProperty, (BindingBase) new MultiBinding()
      {
        Bindings = {
          (BindingBase) new Binding("IsReadOnly")
          {
            Source = (object) cell
          },
          (BindingBase) new Binding("IsReadOnly")
          {
            Source = (object) testStepRowItem.Step
          },
          (BindingBase) new Binding("IsEnabled")
          {
            Source = (object) itemUi,
            Converter = (IValueConverter) new InvertBooleanConverter()
          }
        },
        Converter = (IMultiValueConverter) new NorConverter()
      });
    }
    else
      flag = false;
    cell.SetValue(PropertyColumn.EditableProperty, (object) flag);
    return (FrameworkElement) result;
  }

  protected override object PrepareCellForEdit(
    FrameworkElement editingElement,
    RoutedEventArgs editingEventArgs)
  {
    while (editingElement is Decorator decorator)
      editingElement = decorator.Child as FrameworkElement;
    if (editingElement == null)
      return (object) null;
    FrameworkElement frameworkElement = GuiHelpers.MoveFocusTo(editingElement);
    if (editingElement is TextBoxWithHelp depObj)
    {
      TextBox visualChild1 = depObj.FindVisualChild<TextBox>();
      if (visualChild1 != null)
      {
        visualChild1.SelectAll();
        GuiHelpers.MoveFocusTo((FrameworkElement) visualChild1);
      }
      else
      {
        ComboBox visualChild2 = depObj.FindVisualChild<ComboBox>();
        if (visualChild2 != null)
          visualChild2.IsDropDownOpen = true;
      }
    }
    if (frameworkElement is TextBox textBox)
      textBox.SelectAll();
    if (frameworkElement is ComboBox comboBox)
      comboBox.IsDropDownOpen = true;
    if (frameworkElement is Button button)
    {
      button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
      GuiHelpers.FindParent<DataGrid>((DependencyObject) editingElement)?.CancelEdit();
    }
    return (object) null;
  }
}
