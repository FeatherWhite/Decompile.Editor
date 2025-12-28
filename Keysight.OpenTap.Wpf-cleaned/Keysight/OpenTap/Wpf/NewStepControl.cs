// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NewStepControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NewStepControl : UserControl, IComponentConnector, IStyleConnector
{
  public static readonly DependencyProperty IsAddingEnabledProperty = DependencyProperty.Register(nameof (IsAddingEnabled), typeof (bool), typeof (NewStepControl), new PropertyMetadata((object) true));
  public static readonly DependencyProperty SelectedStepProperty = DependencyProperty.Register(nameof (SelectedStep), typeof (ITestStepParent), typeof (NewStepControl), new PropertyMetadata(new PropertyChangedCallback(NewStepControl.smethod_0)));
  private static TraceSource traceSource_0 = Log.CreateSource("Add Item");
  public static RoutedUICommand AddStep = new RoutedUICommand("Add the step to the sequence.", nameof (AddStep), typeof (NewStepControl), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Return)
  });
  public static RoutedUICommand AddChildStep = new RoutedUICommand("Add the step to the sequence.", nameof (AddChildStep), typeof (NewStepControl), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Return, ModifierKeys.Shift)
  });
  public Func<bool> CanExecuteAdd;
  private readonly List<string> list_0 = new List<string>();
  private bool bool_0;
  private Point point_0;
  private NewStepControl.StepTypeViewModel stepTypeViewModel_0;
  private TreeViewItem treeViewItem_0;
  internal NewStepControl @this;
  internal Grid basegrid;
  internal TextBox FilterTextBox;
  internal TreeView tree;
  private bool bool_1;

  public bool IsAddingEnabled
  {
    get => (bool) this.GetValue(NewStepControl.IsAddingEnabledProperty);
    set => this.SetValue(NewStepControl.IsAddingEnabledProperty, (object) value);
  }

  public ITestStepParent SelectedStep
  {
    get => (ITestStepParent) this.GetValue(NewStepControl.SelectedStepProperty);
    set => this.SetValue(NewStepControl.SelectedStepProperty, (object) value);
  }

  private void method_0(ITestStepParent itestStepParent_0)
  {
    this.viewmodel.SelectedStepList = (TestStepList) null;
    this.viewmodel.ParentStepList = (TestStepList) null;
    if (itestStepParent_0 == null)
      return;
    bool flag = true;
    for (ITestStepParent parent = itestStepParent_0.Parent; parent != null; parent = parent.Parent)
    {
      if (parent.ChildTestSteps.IsReadOnly)
      {
        flag = false;
        break;
      }
    }
    int num = !flag ? 0 : (!itestStepParent_0.ChildTestSteps.IsReadOnly ? 1 : 0);
    if (itestStepParent_0.Parent != null)
    {
      this.viewmodel.ParentStepList = itestStepParent_0.Parent.ChildTestSteps;
      this.viewmodel.SelectedStepList = itestStepParent_0.ChildTestSteps;
    }
    else
      this.viewmodel.ParentStepList = itestStepParent_0.ChildTestSteps;
    this.viewmodel.updateVisibleTypes((IList<NewStepControl.StepViewModel>) this.viewmodel.AvailableTypes.ToList<NewStepControl.StepViewModel>());
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as NewStepControl).method_0(dependencyPropertyChangedEventArgs_0.NewValue as ITestStepParent);
  }

  public event Action<ITypeData> OnAddStep;

  public event Action<ITypeData> OnAddChildStep;

  public event EventHandler RaiseDragDropEvent;

  public NewStepControl(Type basetype)
    : this()
  {
    this.SetBaseType(basetype);
  }

  public NewStepControl()
  {
    this.list_0 = ComponentSettings<GuiControlsSettings>.Current.ExpandedStepCategories;
    this.InitializeComponent();
    this.CommandBindings.Add(new CommandBinding((ICommand) NewStepControl.AddStep, new ExecutedRoutedEventHandler(this.method_4), new CanExecuteRoutedEventHandler(this.method_1)));
    this.CommandBindings.Add(new CommandBinding((ICommand) NewStepControl.AddChildStep, new ExecutedRoutedEventHandler(this.method_3), new CanExecuteRoutedEventHandler(this.method_2)));
    this.Loaded += (RoutedEventHandler) ((sender, e) => this.method_14());
  }

  public void SetBaseType(Type basetype)
  {
    if (!basetype.DescendsTo(typeof (ITestStep)))
      this.basegrid.DataContext = (object) new NewStepControl.NewStepViewModel(basetype)
      {
        AddChildVisibility = Visibility.Collapsed
      };
    else
      this.basegrid.DataContext = (object) new NewStepControl.NewStepViewModel(basetype);
  }

  private NewStepControl.NewStepViewModel viewmodel
  {
    get => this.basegrid.DataContext as NewStepControl.NewStepViewModel;
  }

  private void method_1(object sender, CanExecuteRoutedEventArgs e)
  {
    if (!(e.Parameter is NewStepControl.StepTypeViewModel stepTypeViewModel1))
      stepTypeViewModel1 = this.tree.SelectedItem as NewStepControl.StepTypeViewModel;
    NewStepControl.StepTypeViewModel stepTypeViewModel2 = stepTypeViewModel1;
    e.CanExecute = stepTypeViewModel2 != null && stepTypeViewModel2.IsShown && stepTypeViewModel2.CanAddSib && this.viewmodel.CanAdd;
    if (this.CanExecuteAdd == null)
      return;
    e.CanExecute &= this.CanExecuteAdd();
  }

  private void method_2(object sender, CanExecuteRoutedEventArgs e)
  {
    if (!(e.Parameter is NewStepControl.StepTypeViewModel stepTypeViewModel1))
      stepTypeViewModel1 = this.tree.SelectedItem as NewStepControl.StepTypeViewModel;
    NewStepControl.StepTypeViewModel stepTypeViewModel2 = stepTypeViewModel1;
    e.CanExecute = stepTypeViewModel2 != null && stepTypeViewModel2.IsShown && stepTypeViewModel2.CanAddChild && this.viewmodel.CanAdd;
    if (this.CanExecuteAdd == null)
      return;
    e.CanExecute &= this.CanExecuteAdd();
  }

  private void method_3(object sender, RoutedEventArgs e)
  {
    if (!(((ExecutedRoutedEventArgs) e).Parameter is NewStepControl.StepTypeViewModel stepTypeViewModel1))
      stepTypeViewModel1 = this.tree.SelectedItem as NewStepControl.StepTypeViewModel;
    NewStepControl.StepTypeViewModel stepTypeViewModel2 = stepTypeViewModel1;
    stepTypeViewModel2.IsSelected = true;
    // ISSUE: reference to a compiler-generated field
    if (this.action_1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.action_1(stepTypeViewModel2.Type);
  }

  private void method_4(object sender, RoutedEventArgs e)
  {
    if (!(((ExecutedRoutedEventArgs) e).Parameter is NewStepControl.StepTypeViewModel stepTypeViewModel1))
      stepTypeViewModel1 = this.tree.SelectedItem as NewStepControl.StepTypeViewModel;
    NewStepControl.StepTypeViewModel stepTypeViewModel2 = stepTypeViewModel1;
    stepTypeViewModel2.IsSelected = true;
    // ISSUE: reference to a compiler-generated field
    if (this.action_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.action_0(stepTypeViewModel2.Type);
  }

  private void method_5(object sender, RoutedEventArgs e)
  {
    NewStepControl.StepTypeViewModel dataContext = (NewStepControl.StepTypeViewModel) ((FrameworkElement) sender).DataContext;
    dataContext.IsSelected = true;
    // ISSUE: reference to a compiler-generated field
    if (this.action_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.action_0(dataContext.Type);
  }

  private void method_6(object sender, RoutedEventArgs e)
  {
    NewStepControl.StepTypeViewModel dataContext = (NewStepControl.StepTypeViewModel) ((FrameworkElement) sender).DataContext;
    dataContext.IsSelected = true;
    // ISSUE: reference to a compiler-generated field
    if (this.action_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.action_1(dataContext.Type);
  }

  private void method_7(object sender, RoutedEventArgs e) => e.Handled = true;

  private void method_8(object sender, RoutedPropertyChangedEventArgs<object> e)
  {
    TreeView treeView = sender as TreeView;
    if (!(treeView.SelectedItem is NewStepControl.StepTypeViewModel))
      return;
    this.viewmodel.SelectedItem = treeView.SelectedItem as NewStepControl.StepViewModel;
  }

  private void method_9(object sender, MouseButtonEventArgs e) => this.bool_0 = false;

  private void method_10(object sender, MouseButtonEventArgs e)
  {
    (sender as TreeViewItem).IsSelected = true;
  }

  private void method_11(object sender, MouseEventArgs e)
  {
    if (!this.bool_0 || this.stepTypeViewModel_0 == null || (e.GetPosition((IInputElement) this) - this.point_0).Length < Math.Max(SystemParameters.MinimumHorizontalDragDistance, SystemParameters.MinimumVerticalDragDistance))
      return;
    this.bool_0 = false;
    this.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Delegate) (() => { }));
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler0 = this.eventHandler_0;
    if (eventHandler0 != null)
      eventHandler0((object) this, (EventArgs) e);
    int num = (int) DragDrop.DoDragDrop((DependencyObject) this, (object) new DataObject("steptype", (object) this.stepTypeViewModel_0.Type), DragDropEffects.All);
  }

  private void method_12(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is NewStepControl.StepTypeViewModel))
      return;
    if (GuiHelpers.TryFindFromPoint<Button>((UIElement) sender, e.GetPosition((IInputElement) sender)) != null)
    {
      e.Handled = false;
    }
    else
    {
      this.stepTypeViewModel_0 = (NewStepControl.StepTypeViewModel) frameworkElement.DataContext;
      (sender as TreeViewItem).IsSelected = true;
      this.bool_0 = true;
      this.point_0 = e.GetPosition((IInputElement) this);
    }
  }

  private void method_13(object sender, RoutedEventArgs e)
  {
    if (e is MouseButtonEventArgs mouseButtonEventArgs && mouseButtonEventArgs.LeftButton != MouseButtonState.Pressed)
      return;
    TreeViewItem treeViewItem = sender as TreeViewItem;
    if (!(treeViewItem.DataContext is NewStepControl.StepTypeViewModel))
      return;
    NewStepControl.StepTypeViewModel dataContext = (NewStepControl.StepTypeViewModel) treeViewItem.DataContext;
    if (dataContext.CanAddSib && this.viewmodel.CanAdd)
    {
      if (!NewStepControl.AddStep.CanExecute((object) dataContext, sender as IInputElement))
        return;
      NewStepControl.AddStep.Execute((object) dataContext, sender as IInputElement);
    }
    else
      Log.Warning(NewStepControl.traceSource_0, "Type '{0}' cannot be added here!", new object[1]
      {
        (object) ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) dataContext.Type).Name
      });
  }

  private void FilterTextBox_GotFocus(object sender, RoutedEventArgs e)
  {
    this.FilterTextBox.SelectAll();
  }

  private void method_14()
  {
    try
    {
      if (!this.IsLoaded)
        return;
      this.FilterTextBox?.Focus();
    }
    catch
    {
    }
  }

  public void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
  {
    this.treeViewItem_0 = e.OriginalSource as TreeViewItem;
  }

  private void this_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Escape && !string.IsNullOrEmpty(this.viewmodel.Filter))
    {
      this.method_14();
      this.viewmodel.Filter = "";
      e.Handled = true;
    }
    int relative_index = 0;
    if (e.Key == Key.Down)
      relative_index = 1;
    if (e.Key == Key.Up)
      relative_index = -1;
    if (relative_index != 0)
    {
      this.viewmodel.SelectRelative(relative_index);
      e.Handled = true;
      if (this.treeViewItem_0 != null)
      {
        TreeViewItem parent = this.treeViewItem_0.FindParent<TreeViewItem>();
        if (parent != null && parent.ItemContainerGenerator.Items.OfType<NewStepControl.StepTypeViewModel>().FirstOrDefault<NewStepControl.StepTypeViewModel>((Func<NewStepControl.StepTypeViewModel, bool>) (stepTypeViewModel_0 => stepTypeViewModel_0.IsShown)) == this.treeViewItem_0.DataContext)
        {
          parent.BringIntoView();
          this.UpdateLayout();
          this.treeViewItem_0.BringIntoView();
        }
        else
          this.treeViewItem_0.BringIntoView();
      }
    }
    if (e.Handled)
      return;
    this.method_14();
  }

  private void method_15(object sender, RoutedEventArgs e)
  {
    this.viewmodel.Filter = "";
    this.method_14();
  }

  private void method_16(object sender, RoutedEventArgs e)
  {
    ITypeData type = ((NewStepControl.StepTypeViewModel) ((FrameworkElement) sender).DataContext).Type;
    this.OnOpenPluginsWindowRequested((EventArgs) new NewStepControl.NewStepEventArgs()
    {
      ResourceType = type
    });
  }

  public static event EventHandler OpenPluginsWindowRequested;

  protected virtual void OnOpenPluginsWindowRequested(EventArgs eventArgs_0)
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler1 = NewStepControl.eventHandler_1;
    if (eventHandler1 == null)
      return;
    eventHandler1((object) this, eventArgs_0);
  }

  private void method_17(object sender, MouseButtonEventArgs e) => e.Handled = true;

  private void method_18(object sender, MouseButtonEventArgs e)
  {
    if ((sender as Border).Child.IsEnabled)
      return;
    e.Handled = true;
  }

  private void this_MouseDown(object sender, MouseButtonEventArgs e) => e.Handled = true;

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_1)
      return;
    this.bool_1 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/newstepcontrol.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.@this = (NewStepControl) target;
        this.@this.PreviewKeyDown += new KeyEventHandler(this.this_PreviewKeyDown);
        this.@this.MouseDown += new MouseButtonEventHandler(this.this_MouseDown);
        break;
      case 7:
        this.basegrid = (Grid) target;
        break;
      case 8:
        this.FilterTextBox = (TextBox) target;
        this.FilterTextBox.GotFocus += new RoutedEventHandler(this.FilterTextBox_GotFocus);
        break;
      case 9:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.method_15);
        break;
      case 10:
        this.tree = (TreeView) target;
        this.tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.method_8);
        break;
      default:
        this.bool_1 = true;
        break;
    }
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 2:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = Control.MouseDoubleClickEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.method_13)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseLeftButtonDownEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.method_12)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseLeftButtonUpEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.method_9)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseRightButtonUpEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.method_10)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseMoveEvent,
          Handler = (Delegate) new MouseEventHandler(this.method_11)
        });
        break;
      case 3:
        ((MenuItem) target).Click += new RoutedEventHandler(this.method_16);
        break;
      case 4:
        ((Control) target).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.method_17);
        break;
      case 5:
        ((UIElement) target).PreviewMouseDown += new MouseButtonEventHandler(this.method_18);
        break;
      case 6:
        ((Control) target).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.method_17);
        break;
      case 11:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = TreeViewItem.SelectedEvent,
          Handler = (Delegate) new RoutedEventHandler(this.TreeViewSelectedItemChanged)
        });
        break;
    }
  }

  internal class NewStepViewModel : INotifyPropertyChanged
  {
    private NewStepControl.StepCategoryViewModel stepCategoryViewModel_0;
    private NewStepControl.StepViewModel stepViewModel_0;
    private TestStepList testStepList_0;
    private TestStepList testStepList_1;
    private List<string> list_0 = new List<string>();
    private string string_0 = "";
    private bool bool_0 = true;
    private bool bool_1 = true;

    public NewStepViewModel(Type base_type)
    {
      this.stepCategoryViewModel_0 = this.method_1(TypeData.GetDerivedTypes((ITypeData) TypeData.FromType(base_type)).Where<ITypeData>((Func<ITypeData, bool>) (itypeData_0 => itypeData_0.CanCreateInstance)).Where<ITypeData>((Func<ITypeData, bool>) (stepType =>
      {
        if (stepType is TypeData typeData2)
          return typeData2.IsBrowsable;
        BrowsableAttribute attribute = ReflectionDataExtensions.GetAttribute<BrowsableAttribute>((IReflectionData) stepType);
        return attribute == null || attribute.Browsable;
      }))).GetCollectionViewModel(this);
    }

    private static List<NewStepControl.StepViewModel> smethod_0(
      IEnumerable<NewStepControl.StepViewModel> ienumerable_0)
    {
      List<NewStepControl.StepViewModel> list = ienumerable_0.ToList<NewStepControl.StepViewModel>();
      list.SortBy<NewStepControl.StepViewModel, string>((Func<NewStepControl.StepViewModel, string>) (stepViewModel_0 => stepViewModel_0.Name));
      list.SortBy<NewStepControl.StepViewModel, int>((Func<NewStepControl.StepViewModel, int>) (stepViewModel_0 => !(stepViewModel_0 is NewStepControl.StepCategoryViewModel) ? 1 : 0));
      return list;
    }

    public void updateVisibleTypes(IList<NewStepControl.StepViewModel> ilist_0)
    {
      foreach (NewStepControl.StepViewModel stepViewModel in (IEnumerable<NewStepControl.StepViewModel>) ilist_0)
      {
        if (stepViewModel is NewStepControl.StepTypeViewModel stepTypeViewModel_0)
          this.updateStepTypeViewModel(stepTypeViewModel_0);
        else if (stepViewModel is NewStepControl.StepCategoryViewModel categoryViewModel)
        {
          this.updateVisibleTypes((IList<NewStepControl.StepViewModel>) categoryViewModel.Children);
          categoryViewModel.IsShown = categoryViewModel.Children.Any<NewStepControl.StepViewModel>((Func<NewStepControl.StepViewModel, bool>) (stepViewModel_0 => stepViewModel_0.IsShown));
        }
      }
    }

    public void updateStepTypeViewModel(
      NewStepControl.StepTypeViewModel stepTypeViewModel_0)
    {
      if (this.IsFiltering)
      {
        DisplayAttribute displayAttribute = ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) stepTypeViewModel_0.Type);
        if (!this.method_0(displayAttribute.Name + displayAttribute.Description))
        {
          stepTypeViewModel_0.IsShown = false;
          return;
        }
      }
      stepTypeViewModel_0.IsShown = true;
    }

    public void SelectRelative(int relative_index)
    {
      NewStepControl.StepTypeViewModel[] array = Utils.FlattenHeirarchy<NewStepControl.StepViewModel>((IEnumerable<NewStepControl.StepViewModel>) this.stepCategoryViewModel_0.Children, (Func<NewStepControl.StepViewModel, IEnumerable<NewStepControl.StepViewModel>>) (stepViewModel_1 =>
      {
        NewStepControl.StepCategoryViewModel categoryViewModel = stepViewModel_1 as NewStepControl.StepCategoryViewModel;
        if (!stepViewModel_1.IsShown || categoryViewModel == null)
          return Enumerable.Empty<NewStepControl.StepViewModel>();
        return !this.IsFiltering && !categoryViewModel.Expanded ? Enumerable.Empty<NewStepControl.StepViewModel>() : (IEnumerable<NewStepControl.StepViewModel>) categoryViewModel.Children;
      })).Where<NewStepControl.StepViewModel>((Func<NewStepControl.StepViewModel, bool>) (step => step.IsShown)).OfType<NewStepControl.StepTypeViewModel>().ToArray<NewStepControl.StepTypeViewModel>();
      if (array.Length == 0)
        return;
      int num = -1;
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index].IsSelected)
        {
          num = index;
          break;
        }
      }
      foreach (NewStepControl.StepViewModel stepViewModel in Utils.FlattenHeirarchy<NewStepControl.StepViewModel>((IEnumerable<NewStepControl.StepViewModel>) this.stepCategoryViewModel_0.Children, (Func<NewStepControl.StepViewModel, IEnumerable<NewStepControl.StepViewModel>>) (stepvm => !(stepvm is NewStepControl.StepCategoryViewModel categoryViewModel1) ? Enumerable.Empty<NewStepControl.StepViewModel>() : (IEnumerable<NewStepControl.StepViewModel>) categoryViewModel1.Children)))
        stepViewModel.IsSelected = false;
      if (num != -1)
      {
        int index = num + relative_index;
        if (index < 0)
          index = array.Length - 1;
        if (index >= array.Length)
          index = 0;
        array[index].IsSelected = true;
      }
      else
        array[0].IsSelected = true;
    }

    private bool method_0(string string_1)
    {
      string_1 = string_1.ToUpper();
      return this.list_0.All<string>((Func<string, bool>) (string_0 => string_1.Contains(string_0)));
    }

    public IEnumerable<NewStepControl.StepViewModel> AvailableTypes
    {
      get
      {
        this.updateVisibleTypes((IList<NewStepControl.StepViewModel>) this.stepCategoryViewModel_0.Children);
        return (IEnumerable<NewStepControl.StepViewModel>) this.stepCategoryViewModel_0.Children;
      }
    }

    public NewStepControl.StepViewModel SelectedItem
    {
      get => this.stepViewModel_0;
      set
      {
        if ((this.stepViewModel_0 == value || !(value is NewStepControl.StepTypeViewModel)) && value != null)
          return;
        this.stepViewModel_0 = value;
        this.method_2("CanBeChild");
        this.method_2("CanBeSibling");
        this.method_2(nameof (SelectedItem));
      }
    }

    public bool CanBeChild => this.SelectedItem != null && this.SelectedItem.CanAddChild;

    public bool CanBeSibling => this.SelectedItem != null && this.SelectedItem.CanAddSib;

    public Visibility AddChildVisibility { get; set; }

    public TestStepList ParentStepList
    {
      get => this.testStepList_0;
      set
      {
        if (value == this.testStepList_0)
          return;
        this.testStepList_0 = value;
        this.method_2("CanBeChild");
        this.method_2("AvailableTypes");
        this.method_2("CanBeSibling");
        this.method_2(nameof (ParentStepList));
      }
    }

    public TestStepList SelectedStepList
    {
      get => this.testStepList_1;
      set
      {
        if (value == this.testStepList_1)
          return;
        this.testStepList_1 = value;
        this.method_2("AvailableTypes");
        this.method_2("CanBeChild");
        this.method_2("CanBeSibling");
        this.method_2(nameof (SelectedStepList));
      }
    }

    public string Filter
    {
      get => this.string_0;
      set
      {
        if (!(this.string_0 != value))
          return;
        this.string_0 = value;
        this.list_0 = ((IEnumerable<string>) this.Filter.ToUpper().Split(new string[1]
        {
          " "
        }, StringSplitOptions.RemoveEmptyEntries)).Distinct<string>().ToList<string>();
        this.method_2("AvailableTypes");
        this.method_2("IsFiltering");
        this.method_2(nameof (Filter));
        if (this.list_0.Count <= 0)
          return;
        NewStepControl.StepTypeViewModel stepTypeViewModel = Utils.FlattenHeirarchy<NewStepControl.StepViewModel>(this.AvailableTypes, (Func<NewStepControl.StepViewModel, IEnumerable<NewStepControl.StepViewModel>>) (stepViewModel_0 => !(stepViewModel_0 is NewStepControl.StepCategoryViewModel) ? (IEnumerable<NewStepControl.StepViewModel>) Enumerable.Empty<NewStepControl.StepTypeViewModel>() : (IEnumerable<NewStepControl.StepViewModel>) (stepViewModel_0 as NewStepControl.StepCategoryViewModel).Children)).Where<NewStepControl.StepViewModel>((Func<NewStepControl.StepViewModel, bool>) (stepViewModel_0 => stepViewModel_0.IsShown)).OfType<NewStepControl.StepTypeViewModel>().FirstOrDefault<NewStepControl.StepTypeViewModel>();
        if (stepTypeViewModel != null)
        {
          stepTypeViewModel.IsSelected = true;
          this.SelectedItem = (NewStepControl.StepViewModel) stepTypeViewModel;
        }
        else
          this.SelectedItem = (NewStepControl.StepViewModel) null;
      }
    }

    public bool IsFiltering => this.list_0.Count > 0;

    public bool CanAdd
    {
      get => this.bool_0;
      set
      {
        this.bool_0 = value;
        this.method_2(nameof (CanAdd));
      }
    }

    public bool CanAddChild
    {
      get => this.bool_1;
      set
      {
        this.bool_1 = value;
        this.method_2(nameof (CanAddChild));
      }
    }

    private NewStepControl.NewStepViewModel.StepTree method_1(IEnumerable<ITypeData> ienumerable_0)
    {
      NewStepControl.NewStepViewModel.StepTree stepTree1 = new NewStepControl.NewStepViewModel.StepTree()
      {
        Name = ""
      };
      foreach (ITypeData itypeData in ienumerable_0)
      {
        try
        {
          NewStepControl.StepTypeViewModel stepTypeViewModel = new NewStepControl.StepTypeViewModel(this)
          {
            Type = itypeData
          };
          if (stepTypeViewModel.Equals((object) this.SelectedItem))
            stepTypeViewModel.IsSelected = true;
          DisplayAttribute displayAttribute = ReflectionDataExtensions.GetAttribute<DisplayAttribute>((IReflectionData) itypeData);
          if (displayAttribute == null)
            displayAttribute = new DisplayAttribute(((IEnumerable<string>) ((IReflectionData) itypeData).Name.Split('.')).Last<string>(), (string) null, (string) null, -10000.0, false, (string[]) null);
          string[] group = displayAttribute.Group;
          stepTypeViewModel.Name = displayAttribute.Name;
          NewStepControl.NewStepViewModel.StepTree stepTree2 = stepTree1;
          for (int index = 0; index < group.Length; ++index)
            stepTree2 = stepTree2.GetSubTree(group[index]);
          stepTree2.Items.Add(stepTypeViewModel);
        }
        catch (Exception ex)
        {
          Log.Warning(NewStepControl.traceSource_0, $"Failed load step '{((IReflectionData) itypeData).Name}'.", Array.Empty<object>());
          Log.Debug(NewStepControl.traceSource_0, ex);
        }
      }
      return stepTree1;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void method_2(string string_1)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.propertyChangedEventHandler_0 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(string_1));
    }

    private class StepTree
    {
      public string Name;
      private readonly Dictionary<string, NewStepControl.NewStepViewModel.StepTree> dictionary_0 = new Dictionary<string, NewStepControl.NewStepViewModel.StepTree>();
      public readonly List<NewStepControl.StepTypeViewModel> Items = new List<NewStepControl.StepTypeViewModel>();

      public NewStepControl.NewStepViewModel.StepTree GetSubTree(string category)
      {
        if (!this.dictionary_0.ContainsKey(category))
          this.dictionary_0[category] = new NewStepControl.NewStepViewModel.StepTree()
          {
            Name = category
          };
        return this.dictionary_0[category];
      }

      public NewStepControl.StepCategoryViewModel GetCollectionViewModel(
        NewStepControl.NewStepViewModel newStepViewModel_0)
      {
        List<NewStepControl.StepViewModel> children = NewStepControl.NewStepViewModel.smethod_0(((IEnumerable<NewStepControl.StepViewModel>) this.Items).Concat<NewStepControl.StepViewModel>(this.dictionary_0.Values.Select<NewStepControl.NewStepViewModel.StepTree, NewStepControl.StepCategoryViewModel>((Func<NewStepControl.NewStepViewModel.StepTree, NewStepControl.StepCategoryViewModel>) (tree => tree.GetCollectionViewModel(newStepViewModel_0))).Cast<NewStepControl.StepViewModel>()));
        return new NewStepControl.StepCategoryViewModel(newStepViewModel_0, this.Name, children);
      }
    }
  }

  internal class StepViewModel : INotifyPropertyChanged
  {
    protected NewStepControl.NewStepViewModel newStepViewModel_0;
    private bool isSelected;
    private bool isShown = true;

    public StepViewModel(NewStepControl.NewStepViewModel newStepViewModel_1)
    {
      this.newStepViewModel_0 = newStepViewModel_1;
    }

    public string Name { get; set; }

    public NewStepControl.StepViewModel Category { get; set; }

    public virtual bool CanAddChild => true;

    public virtual bool CanAddSib => true;

    public virtual bool IsSelected
    {
      get => this.isSelected;
      set
      {
        if (value == this.isSelected)
          return;
        this.isSelected = value;
        this.raisePropertyChanged(nameof (IsSelected));
      }
    }

    public bool IsShown
    {
      get => this.isShown;
      set
      {
        if (value == this.isShown)
          return;
        this.isShown = value;
        this.raisePropertyChanged(nameof (IsShown));
      }
    }

    public virtual bool Expanded { get; set; }

    protected void raisePropertyChanged(string prop)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }

  [DebuggerDisplay("{Name}")]
  internal class StepTypeViewModel : NewStepControl.StepViewModel
  {
    private ITypeData type;

    public StepTypeViewModel(NewStepControl.NewStepViewModel newStepViewModel_1)
      : base(newStepViewModel_1)
    {
      newStepViewModel_1.PropertyChanged += new PropertyChangedEventHandler(this.Vm_PropertyChanged);
    }

    private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ParentStepList")
        this.raisePropertyChanged("CanAddSib");
      if (!(e.PropertyName == "SelectedStepList"))
        return;
      this.raisePropertyChanged("CanAddChild");
    }

    public override bool CanAddSib
    {
      get
      {
        if (this.newStepViewModel_0.ParentStepList != null)
          return this.newStepViewModel_0.ParentStepList.CanInsertType(this.Type.GetInnerType());
        return !ReflectionDataExtensions.DescendsTo(this.Type, typeof (ITestStep)) || TestStepList.AllowChild(typeof (TestPlan), this.Type.GetInnerType());
      }
    }

    public override bool CanAddChild
    {
      get
      {
        TestStepList selectedStepList = this.newStepViewModel_0.SelectedStepList;
        return selectedStepList != null && selectedStepList.CanInsertType(this.Type.GetInnerType());
      }
    }

    public ITypeData Type
    {
      get => this.type;
      set
      {
        this.type = value;
        this.ImplementingAssembly = this.type.GetInnerType().Assembly.ManifestModule.Name;
      }
    }

    public string ImplementingAssembly { get; set; }

    public string Description
    {
      get => ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) this.Type).Description;
    }

    public string HelpLink
    {
      get
      {
        return ReflectionDataExtensions.GetAttribute<HelpLinkAttribute>((IReflectionData) this.Type)?.HelpLink;
      }
    }

    public bool HasHelpLink => this.HelpLink != null;
  }

  [DebuggerDisplay("{Name}")]
  internal class StepCategoryViewModel : NewStepControl.StepViewModel
  {
    public List<NewStepControl.StepViewModel> Children { get; private set; }

    public override bool IsSelected
    {
      get => false;
      set
      {
      }
    }

    public override bool Expanded
    {
      get
      {
        return ComponentSettings<GuiControlsSettings>.Current.ExpandedStepCategories.Contains(this.Name);
      }
      set
      {
        if (!value)
          ComponentSettings<GuiControlsSettings>.Current.ExpandedStepCategories.Remove(this.Name);
        else if (!this.Expanded)
          ComponentSettings<GuiControlsSettings>.Current.ExpandedStepCategories.Add(this.Name);
        this.raisePropertyChanged(nameof (Expanded));
      }
    }

    public StepCategoryViewModel(
      NewStepControl.NewStepViewModel newStepViewModel_1,
      string name,
      List<NewStepControl.StepViewModel> children)
      : base(newStepViewModel_1)
    {
      this.Name = name;
      this.Children = children;
    }
  }

  public class NewStepEventArgs : EventArgs
  {
    public ITypeData ResourceType { get; set; }
  }
}
