// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropGrid
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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropGrid : Control, ICommandHandler, IMemberDataItemFocus
{
  public static RoutedUICommand MonitorProperty = new RoutedUICommand("Monitor Property", "Monitor", typeof (PropGrid));
  public static RoutedUICommand RemoveMonitorProperty = new RoutedUICommand("RemoveMonitor Property", "RemoveMonitor", typeof (PropGrid));
  public static RoutedUICommand ActivateMenuCommand = new RoutedUICommand("Activate Menu Command", nameof (ActivateMenuCommand), typeof (PropGrid));
  public static RoutedEvent ViewRefreshedEvent = EventManager.RegisterRoutedEvent("ViewRefreshed", RoutingStrategy.Direct, typeof (RoutedEventHandler), typeof (PropGrid));
  public static readonly DependencyProperty DebugModeProperty = DependencyProperty.Register(nameof (DebugMode), typeof (bool), typeof (PropGrid), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty IsShowHeadersProperty = DependencyProperty.Register(nameof (IsShowHeaders), typeof (bool), typeof (PropGrid), (PropertyMetadata) new UIPropertyMetadata((object) true));
  public static readonly DependencyProperty ContentEnabledProperty = DependencyProperty.Register(nameof (ContentEnabled), typeof (bool), typeof (PropGrid), new PropertyMetadata((object) true));
  public static readonly DependencyProperty UndoEnabledProperty = DependencyProperty.Register(nameof (UndoEnabled), typeof (bool), typeof (PropGrid));
  public static readonly DependencyProperty ProcessingProperty = DependencyProperty.Register(nameof (Processing), typeof (bool), typeof (PropGrid));
  public static readonly DependencyProperty MainGroupProperty = DependencyProperty.Register(nameof (MainGroup), typeof (GroupUi), typeof (PropGrid));
  public static readonly DependencyProperty ContextTemplateProperty = DependencyProperty.Register(nameof (ContextTemplate), typeof (DataTemplate), typeof (PropGrid));
  public Action OnCommit = (Action) (() => { });
  private UpdateMonitor updateMonitor_0;
  private ICommandHandler icommandHandler_0;
  private int int_0;
  private AnnotationCollection annotationCollection_0;
  private TapThread tapThread_0;

  public bool DebugMode
  {
    get => (bool) this.GetValue(PropGrid.DebugModeProperty);
    set => this.SetValue(PropGrid.DebugModeProperty, (object) value);
  }

  public bool IsShowHeaders
  {
    get => (bool) this.GetValue(PropGrid.IsShowHeadersProperty);
    set => this.SetValue(PropGrid.IsShowHeadersProperty, (object) value);
  }

  public bool ContentEnabled
  {
    get => (bool) this.GetValue(PropGrid.ContentEnabledProperty);
    set => this.SetValue(PropGrid.ContentEnabledProperty, (object) value);
  }

  public bool UndoEnabled
  {
    get => (bool) this.GetValue(PropGrid.UndoEnabledProperty);
    set => this.SetValue(PropGrid.UndoEnabledProperty, (object) value);
  }

  public bool Processing
  {
    get => (bool) this.GetValue(PropGrid.ProcessingProperty);
    set => this.SetValue(PropGrid.ProcessingProperty, (object) value);
  }

  public GroupUi MainGroup
  {
    get => (GroupUi) this.GetValue(PropGrid.MainGroupProperty);
    set => this.SetValue(PropGrid.MainGroupProperty, (object) value);
  }

  public DataTemplate ContextTemplate
  {
    get => (DataTemplate) this.GetValue(PropGrid.ContextTemplateProperty);
    set => this.SetValue(PropGrid.ContextTemplateProperty, (object) value);
  }

  public IEnumerable<ItemUi> ItemUis
  {
    get
    {
      GroupUi mainGroup = this.MainGroup;
      IEnumerable<ItemUi> itemUis;
      if (mainGroup == null)
      {
        itemUis = (IEnumerable<ItemUi>) null;
      }
      else
      {
        itemUis = mainGroup.Sequential.OfType<ItemUi>();
        if (itemUis != null)
          return itemUis;
      }
      return (IEnumerable<ItemUi>) Array.Empty<ItemUi>();
    }
  }

  public void RefreshBindings()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_1 != null)
    {
      // ISSUE: reference to a compiler-generated field
      this.eventHandler_1((object) this, new EventArgs());
    }
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_2 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_2((object) this, new EventArgs());
  }

  public event PropGrid.FilterPropertyHandler FilterProperty;

  public event RoutedEventHandler ViewRefreshed
  {
    add => this.AddHandler(PropGrid.ViewRefreshedEvent, (Delegate) value);
    remove => this.RemoveHandler(PropGrid.ViewRefreshedEvent, (Delegate) value);
  }

  public event EventHandler TestPlanModified;

  public bool MultiSelectEnabled { get; set; }

  public event EventHandler<EventArgs> PropertyEdited;

  private event EventHandler<EventArgs> propertyEditedInternal;

  protected virtual void OnPropertyEdited(FrameworkElement control)
  {
    if (!control.IsLoaded || this.DataContext == null)
      return;
    this.RefreshBindings();
    this.annotationCollection_0?.Read();
    this.updateMonitor_0?.PushUpdate();
    foreach (ItemUi itemUi in this.ItemUis)
      itemUi.Update();
  }

  protected override void OnKeyDown(KeyEventArgs keyEventArgs_0)
  {
    if (keyEventArgs_0.Key == Key.Return && Keyboard.FocusedElement is FrameworkElement focusedElement)
    {
      keyEventArgs_0.Handled = true;
      focusedElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }
    base.OnKeyDown(keyEventArgs_0);
  }

  public void Reload() => this.onContentChanged(this.DataContext, this.DataContext);

  protected void onContentChanged(object oldContent, object newContent)
  {
    if (oldContent is INotifyCollectionChanged collectionChanged1)
      collectionChanged1.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.method_2);
    if (newContent is INotifyCollectionChanged collectionChanged2)
      collectionChanged2.CollectionChanged += new NotifyCollectionChangedEventHandler(this.method_2);
    if (this.MultiSelectEnabled && newContent is IEnumerable)
    {
      IEnumerable<object> source = ((IEnumerable) newContent).Cast<object>();
      if (source.Take<object>(2).Count<object>() == 1)
        newContent = source.FirstOrDefault<object>();
    }
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_2 = (EventHandler<EventArgs>) null;
    this.method_3(newContent ?? new object());
  }

  public PropGrid()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) PropGrid.ActivateMenuCommand, new ExecutedRoutedEventHandler(this.method_0)));
    this.MouseDown += new MouseButtonEventHandler(this.PropGrid_MouseDown);
    this.Loaded += (RoutedEventHandler) ((sender, e) =>
    {
      UpdateMonitor.Events += new EventHandler<UpdateEventArgs>(this.method_1);
      this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.PropGrid_DataContextChanged);
      this.onContentChanged((object) null, this.DataContext);
      this.icommandHandler_0 = this.LookupParent<ICommandHandler>();
    });
    this.Unloaded += (RoutedEventHandler) ((sender, e) =>
    {
      UpdateMonitor.Events -= new EventHandler<UpdateEventArgs>(this.method_1);
      this.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.PropGrid_DataContextChanged);
    });
  }

  private void method_0(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(e.Parameter is MenuUi parameter))
      return;
    parameter.InvokeCommand();
    if (!parameter.OverrideReload)
      this.ReloadView(parameter);
    else
      this.OnPropertyEdited(parameter.RootItem.Control);
  }

  private void PropGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.onContentChanged(e.OldValue, e.NewValue);
  }

  private void method_1(object sender, UpdateEventArgs e)
  {
    if (sender == this)
      return;
    this.OnPropertyEdited((FrameworkElement) this);
  }

  private void PropGrid_MouseDown(object sender, MouseButtonEventArgs e)
  {
    e.Handled = true;
    this.Focus();
    FocusManager.SetFocusedElement((DependencyObject) Window.GetWindow((DependencyObject) this), (IInputElement) this);
  }

  private void method_2(object sender, NotifyCollectionChangedEventArgs e)
  {
    int _delayIdx = ++this.int_0;
    Utils.Delay(100, (Action) (() =>
    {
      if (_delayIdx != this.int_0)
        return;
      GuiHelpers.GuiInvoke((Action) (() => this.onContentChanged(sender, sender)));
    }));
  }

  public Func<AnnotationCollection, bool> AnnotationFilter { get; set; } = (Func<AnnotationCollection, bool>) (_ => true);

  private void method_3(object object_0)
  {
    if (typeof (object) == object_0.GetType() && !this.ItemUis.Any<ItemUi>())
      return;
    if (object_0 is IEnumerable source && source.Cast<object>().ToList<object>().Count == 1 && this.MultiSelectEnabled)
      object_0 = source.Cast<object>().FirstOrDefault<object>();
    if (this.tapThread_0 != null && this.tapThread_0.Status != 2)
      this.tapThread_0.Abort();
    ManualResetEvent manualResetEvent_0 = new ManualResetEvent(false);
    this.tapThread_0 = TapThread.Start((Action) (() =>
    {
      // ISSUE: reference to a compiler-generated method
      this.method_0();
      manualResetEvent_0.Set();
    }), "AnnotateUISelection");
    manualResetEvent_0.WaitOne(5);
  }

  private void method_4(object object_0, ItemUi itemUi_0)
  {
    if (!string.IsNullOrEmpty(itemUi_0.Item.HelpLink))
    {
      HelpManager.SetHelpLink((DependencyObject) itemUi_0.Control, (object) itemUi_0.Item.HelpLink);
    }
    else
    {
      Type type = object_0.GetType();
      if (!ReflectionHelper.HasAttribute<HelpLinkAttribute>(type))
        return;
      HelpLinkAttribute attribute = type.GetAttribute<HelpLinkAttribute>();
      HelpManager.SetHelpLink((DependencyObject) itemUi_0.Control, (object) attribute.HelpLink);
    }
  }

  static PropGrid()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PropGrid), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PropGrid)));
  }

  public void ReloadView(MenuUi menu)
  {
    menu.Annotation.Write();
    this.Reload();
    UpdateMonitor.Update((object) this, menu.Annotation);
    this.OnPropertyEdited((FrameworkElement) this);
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler0 = this.eventHandler_0;
    if (eventHandler0 == null)
      return;
    eventHandler0((object) this, new EventArgs());
  }

  public bool CanHandleCommand(string commandName, AnnotationCollection context)
  {
    ICommandHandler icommandHandler0 = this.icommandHandler_0;
    return icommandHandler0 != null && icommandHandler0.CanHandleCommand(commandName, context);
  }

  public void HandleCommand(string commandName, AnnotationCollection context)
  {
    this.icommandHandler_0?.HandleCommand(commandName, context);
  }

  public void CommandHandled(string commandName, AnnotationCollection context)
  {
    this.icommandHandler_0?.CommandHandled(commandName, context);
  }

  public void Focus(IMemberData item)
  {
    GuiHelpers.GuiInvokeAsync((Action) (() => this.ItemUis.FirstOrDefault<ItemUi>((Func<ItemUi, bool>) (itemUi_0 => itemUi_0.Item.Member == item))?.Focus()), priority: DispatcherPriority.Input);
  }

  public delegate bool FilterPropertyHandler(IReflectionData property);
}
