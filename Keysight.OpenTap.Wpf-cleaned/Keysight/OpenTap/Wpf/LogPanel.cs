// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LogPanel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using OpenTap.Diagnostic;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class LogPanel : System.Windows.Controls.UserControl, IComponentConnector, IStyleConnector
{
  private static readonly TraceSource traceSource_0 = Log.CreateSource("Log Panel");
  private int int_0 = 20000;
  public static readonly RoutedUICommand OpenLinkCommand = new RoutedUICommand("Open Link", "OpenLink", typeof (LogPanel));
  public static readonly RoutedUICommand OpenFolderCommand = new RoutedUICommand("Open Folder", "OpenFolder", typeof (LogPanel));
  public static readonly RoutedUICommand CopyLinkCommand = new RoutedUICommand("Copy Link", "CopyLink", typeof (LogPanel));
  public static readonly RoutedUICommand ClearCommand = new RoutedUICommand("Clear Log Panel", "Clear", typeof (LogPanel), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.C, ModifierKeys.Alt)
  });
  public static readonly RoutedUICommand OpenSessionLogCommand = new RoutedUICommand("Open Session Log", "OpenSessionLog", typeof (LogPanel), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.O, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand OpenSessionLogFoldeCommand = new RoutedUICommand("Open Session Log Folder", "OpenSessionLogFolder", typeof (LogPanel), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.O, ModifierKeys.Alt)
  });
  public static readonly RoutedUICommand AnalyzeCommand = new RoutedUICommand("Analyze Session Log", "Analyze", typeof (LogPanel), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.A, ModifierKeys.Alt)
  });
  public static readonly RoutedUICommand FindCommand = new RoutedUICommand("Opens a find menu", "Find", typeof (SearchGui), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand FilterCommand = new RoutedUICommand("Opens a filter menu", "Filter", typeof (ControlDropDown), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.I, ModifierKeys.Control)
  });
  public static readonly RoutedUICommand ClosePopupCommand = new RoutedUICommand("Closes find and filter menus", "Close", typeof (SearchGui), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.Escape)
  });
  public static readonly DependencyProperty MaxMessagesProperty = DependencyProperty.Register(nameof (MaxMessages), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 20000));
  public static readonly DependencyProperty ViewErrorsProperty = DependencyProperty.Register(nameof (ViewErrors), typeof (bool), typeof (LogPanel), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ViewWarningsProperty = DependencyProperty.Register(nameof (ViewWarnings), typeof (bool), typeof (LogPanel), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ViewMessagesProperty = DependencyProperty.Register(nameof (ViewMessages), typeof (bool), typeof (LogPanel), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ViewDebugProperty = DependencyProperty.Register(nameof (ViewDebug), typeof (bool), typeof (LogPanel), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ErrorCountProperty = DependencyProperty.Register(nameof (ErrorCount), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty WarningCountProperty = DependencyProperty.Register(nameof (WarningCount), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty MessageCountProperty = DependencyProperty.Register(nameof (MessageCount), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty DebugCountProperty = DependencyProperty.Register(nameof (DebugCount), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty FilterCountProperty = DependencyProperty.Register(nameof (FilterCount), typeof (int), typeof (LogPanel), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty FilterActiveProperty = DependencyProperty.Register(nameof (FilterActive), typeof (bool), typeof (LogPanel));
  public static readonly DependencyProperty SourceFilterActiveProperty = DependencyProperty.Register(nameof (SourceFilterActive), typeof (bool), typeof (LogPanel));
  public static readonly DependencyProperty LockBottomProperty = DependencyProperty.Register(nameof (LockBottom), typeof (bool), typeof (LogPanel), new PropertyMetadata((object) true, new PropertyChangedCallback(LogPanel.smethod_1)));
  public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(nameof (FilterText), typeof (string), typeof (LogPanel));
  public static readonly DependencyProperty FilterStringProperty = DependencyProperty.Register(nameof (FilterString), typeof (string), typeof (LogPanel), new PropertyMetadata((object) ""));
  public static readonly DependencyProperty SelectedMessagesProperty = DependencyProperty.Register(nameof (SelectedMessages), typeof (IList<LogMessage>), typeof (LogPanel), new PropertyMetadata((object) null, new PropertyChangedCallback(LogPanel.smethod_0)));
  public static readonly DependencyProperty SelectedTimeSlotsProperty = DependencyProperty.Register(nameof (SelectedTimeSlots), typeof (IList<TimeSlot>), typeof (LogPanel), new PropertyMetadata((object) new List<TimeSlot>()));
  public static readonly DependencyProperty WarningBrushProperty = DependencyProperty.Register(nameof (WarningBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty ErrorBrushProperty = DependencyProperty.Register(nameof (ErrorBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(nameof (SelectedBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty DebugBrushProperty = DependencyProperty.Register(nameof (DebugBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty HyperlinkBrushProperty = DependencyProperty.Register(nameof (HyperlinkBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty TimeSlotBrushProperty = DependencyProperty.Register(nameof (TimeSlotBrush), typeof (Brush), typeof (LogPanel));
  public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.Register(nameof (ScrollOffset), typeof (double), typeof (LogPanel));
  public static readonly DependencyProperty FeaturesProperty = DependencyProperty.Register(nameof (Features), typeof (LogPanelFeatures), typeof (LogPanel), new PropertyMetadata((object) (LogPanelFeatures.Levels | LogPanelFeatures.Filter | LogPanelFeatures.AutoScroll | LogPanelFeatures.Search | LogPanelFeatures.TimingAnalyzerInterop | LogPanelFeatures.Clear | LogPanelFeatures.SessionLog)));
  private bool bool_1 = true;
  private bool bool_2;
  private bool bool_3;
  private bool bool_4;
  private bool bool_5;
  private bool bool_6 = true;
  private string string_0;
  private bool bool_7;
  private HashSet<string> hashSet_0 = new HashSet<string>();
  private readonly List<LogMessage> list_0 = new List<LogMessage>();
  private readonly object object_0 = new object();
  public CircularBuffer<LogMessage> messages = new CircularBuffer<LogMessage>(20000);
  public CircularBuffer<LogMessage> filteredmessages = new CircularBuffer<LogMessage>(20000);
  public CircularBuffer<LogMessage> activebuffer;
  private int int_1 = 10;
  private HashSet<string> hashSet_1 = new HashSet<string>();
  private bool bool_8;
  private double double_0;
  private Point point_0;
  private LogMessage logMessage_0;
  public HashSet<LogMessage> selectedMessages = new HashSet<LogMessage>();
  public double charwidth = 10.0;
  public double charheight = 10.0;
  private string string_1;
  internal LogPanel This;
  internal System.Windows.Controls.ContextMenu gridContextMenu;
  internal System.Windows.Controls.MenuItem openLinkBtn;
  internal Grid basegrid;
  internal Grid LogPanelToolBarGrid;
  internal System.Windows.Controls.CheckBox errorToggle;
  internal System.Windows.Controls.CheckBox warningToggle;
  internal System.Windows.Controls.CheckBox infoToggle;
  internal System.Windows.Controls.CheckBox debugToggle;
  internal ControlDropDown groups;
  internal ItemsControl groupsItems;
  internal ControlDropDown searchGuiPopUp;
  internal SearchGui searchGui;
  internal ControlDropDown filterPopUp;
  internal Grid filterControlDropDownGrid;
  internal System.Windows.Controls.TextBox filterInputField;
  internal TextBlock foundCount;
  internal ToggleButton autoscrollButton;
  internal Grid viewHostGrid;
  internal StackPanel viewHost;
  internal System.Windows.Controls.Primitives.ScrollBar horzbar;
  internal System.Windows.Controls.Primitives.ScrollBar vertbar;
  private bool bool_9;

  public bool IsTimeSpanEnabled { get; set; }

  public int MaxMessages
  {
    get => (int) this.GetValue(LogPanel.MaxMessagesProperty);
    set => this.SetValue(LogPanel.MaxMessagesProperty, (object) value);
  }

  public bool ViewErrors
  {
    get => (bool) this.GetValue(LogPanel.ViewErrorsProperty);
    set => this.SetValue(LogPanel.ViewErrorsProperty, (object) value);
  }

  public bool ViewWarnings
  {
    get => (bool) this.GetValue(LogPanel.ViewWarningsProperty);
    set => this.SetValue(LogPanel.ViewWarningsProperty, (object) value);
  }

  public bool ViewMessages
  {
    get => (bool) this.GetValue(LogPanel.ViewMessagesProperty);
    set => this.SetValue(LogPanel.ViewMessagesProperty, (object) value);
  }

  public bool ViewDebug
  {
    get => (bool) this.GetValue(LogPanel.ViewDebugProperty);
    set => this.SetValue(LogPanel.ViewDebugProperty, (object) value);
  }

  public int ErrorCount
  {
    get => (int) this.GetValue(LogPanel.ErrorCountProperty);
    set => this.SetValue(LogPanel.ErrorCountProperty, (object) value);
  }

  public int WarningCount
  {
    get => (int) this.GetValue(LogPanel.WarningCountProperty);
    set => this.SetValue(LogPanel.WarningCountProperty, (object) value);
  }

  public int MessageCount
  {
    get => (int) this.GetValue(LogPanel.MessageCountProperty);
    set => this.SetValue(LogPanel.MessageCountProperty, (object) value);
  }

  public int DebugCount
  {
    get => (int) this.GetValue(LogPanel.DebugCountProperty);
    set => this.SetValue(LogPanel.DebugCountProperty, (object) value);
  }

  public int FilterCount
  {
    get => (int) this.GetValue(LogPanel.FilterCountProperty);
    set => this.SetValue(LogPanel.FilterCountProperty, (object) value);
  }

  public bool FilterActive
  {
    get => (bool) this.GetValue(LogPanel.FilterActiveProperty);
    set => this.SetValue(LogPanel.FilterActiveProperty, (object) value);
  }

  public bool SourceFilterActive
  {
    get => (bool) this.GetValue(LogPanel.SourceFilterActiveProperty);
    set => this.SetValue(LogPanel.SourceFilterActiveProperty, (object) value);
  }

  public bool LockBottom
  {
    get => (bool) this.GetValue(LogPanel.LockBottomProperty);
    set => this.SetValue(LogPanel.LockBottomProperty, (object) value);
  }

  public bool FilterText
  {
    get => (bool) this.GetValue(LogPanel.FilterTextProperty);
    set => this.SetValue(LogPanel.FilterTextProperty, (object) value);
  }

  public string FilterString
  {
    get => (string) this.GetValue(LogPanel.FilterStringProperty);
    set => this.SetValue(LogPanel.FilterStringProperty, (object) value);
  }

  public IList<LogMessage> SelectedMessages
  {
    get => (IList<LogMessage>) this.GetValue(LogPanel.SelectedMessagesProperty);
    set => this.SetValue(LogPanel.SelectedMessagesProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as LogPanel).method_0((IList<LogMessage>) dependencyPropertyChangedEventArgs_0.OldValue, (IList<LogMessage>) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  private void method_0(IList<LogMessage> ilist_0, IList<LogMessage> ilist_1)
  {
    this.selectedMessages.Clear();
    if (ilist_1 != null)
    {
      foreach (LogMessage logMessage in (IEnumerable<LogMessage>) ilist_1)
        this.selectedMessages.Add(logMessage);
    }
    foreach (UIElement uiElement in this.viewHost.Children.OfType<LogTextShower>())
      uiElement.InvalidateVisual();
  }

  public IList<TimeSlot> SelectedTimeSlots
  {
    get => (IList<TimeSlot>) this.GetValue(LogPanel.SelectedTimeSlotsProperty);
    set => this.SetValue(LogPanel.SelectedTimeSlotsProperty, (object) value);
  }

  public Brush WarningBrush
  {
    get => (Brush) this.GetValue(LogPanel.WarningBrushProperty);
    set => this.SetValue(LogPanel.WarningBrushProperty, (object) value);
  }

  public Brush ErrorBrush
  {
    get => (Brush) this.GetValue(LogPanel.ErrorBrushProperty);
    set => this.SetValue(LogPanel.ErrorBrushProperty, (object) value);
  }

  public Brush SelectedBrush
  {
    get => (Brush) this.GetValue(LogPanel.SelectedBrushProperty);
    set => this.SetValue(LogPanel.SelectedBrushProperty, (object) value);
  }

  public Brush DebugBrush
  {
    get => (Brush) this.GetValue(LogPanel.DebugBrushProperty);
    set => this.SetValue(LogPanel.DebugBrushProperty, (object) value);
  }

  public Brush HyperlinkBrush
  {
    get => (Brush) this.GetValue(LogPanel.HyperlinkBrushProperty);
    set => this.SetValue(LogPanel.HyperlinkBrushProperty, (object) value);
  }

  public Brush TimeSlotBrush
  {
    get => (Brush) this.GetValue(LogPanel.TimeSlotBrushProperty);
    set => this.SetValue(LogPanel.TimeSlotBrushProperty, (object) value);
  }

  public double ScrollOffset
  {
    get => (double) this.GetValue(LogPanel.ScrollOffsetProperty);
    set => this.SetValue(LogPanel.ScrollOffsetProperty, (object) value);
  }

  public LogPanelFeatures Features
  {
    get => (LogPanelFeatures) this.GetValue(LogPanel.FeaturesProperty);
    set => this.SetValue(LogPanel.FeaturesProperty, (object) value);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    DependencyProperty property = dependencyPropertyChangedEventArgs_0.Property;
    if (property == LogPanel.FeaturesProperty)
    {
      Func<LogPanelFeatures, Visibility> func = (Func<LogPanelFeatures, Visibility>) (logPanelFeatures_0 => !this.Features.HasFlag((Enum) logPanelFeatures_0) ? Visibility.Collapsed : Visibility.Visible);
      Visibility levelsVisibility = func(LogPanelFeatures.Levels);
      ((IEnumerable<System.Windows.Controls.CheckBox>) new System.Windows.Controls.CheckBox[4]
      {
        this.errorToggle,
        this.warningToggle,
        this.infoToggle,
        this.debugToggle
      }).ToList<System.Windows.Controls.CheckBox>().ForEach((Action<System.Windows.Controls.CheckBox>) (checkBox_0 => checkBox_0.Visibility = levelsVisibility));
      this.searchGuiPopUp.Visibility = func(LogPanelFeatures.Search);
      this.filterPopUp.Visibility = func(LogPanelFeatures.Filter);
      this.autoscrollButton.Visibility = func(LogPanelFeatures.AutoScroll);
      this.groups.Visibility = func(LogPanelFeatures.Sources);
      this.LogPanelToolBarGrid.Visibility = this.Features > (LogPanelFeatures) 0 ? Visibility.Visible : Visibility.Collapsed;
      foreach (object obj in (IEnumerable) this.ContextMenu.Items)
      {
        if (obj is System.Windows.Controls.MenuItem)
        {
          System.Windows.Controls.MenuItem menuItem = (System.Windows.Controls.MenuItem) obj;
          if (menuItem.Tag != null)
          {
            LogPanelFeatures tag = (LogPanelFeatures) menuItem.Tag;
            menuItem.Visibility = func(tag);
          }
        }
      }
    }
    else if (property != LogPanel.ViewDebugProperty && property != LogPanel.FilterActiveProperty && property != LogPanel.ViewErrorsProperty && property != LogPanel.ViewMessagesProperty && property != LogPanel.ViewWarningsProperty && property != LogPanel.FilterStringProperty && property != LogPanel.SourceFilterActiveProperty && (!this.bool_1 || !this.IsLoaded))
    {
      if (property == LogPanel.MaxMessagesProperty)
      {
        this.int_0 = Math.Max(10, this.MaxMessages);
      }
      else
      {
        if (property != LogPanel.SelectedTimeSlotsProperty || this.viewHost == null)
          return;
        foreach (UIElement uiElement in this.viewHost.Children.OfType<LogTextShower>())
          uiElement.InvalidateVisual();
      }
    }
    else
    {
      this.bool_1 = false;
      bool flag = this.FilterActive || !this.ViewDebug || !this.ViewErrors || !this.ViewMessages || !this.ViewWarnings || this.SourceFilterActive;
      this.bool_7 = this.FilterActive;
      this.bool_2 = this.ViewDebug;
      this.bool_3 = this.ViewErrors;
      this.bool_4 = this.ViewMessages;
      this.bool_5 = this.ViewWarnings;
      this.string_0 = this.FilterString;
      if (flag != this.bool_6)
      {
        if (flag)
        {
          this.activebuffer = this.filteredmessages;
        }
        else
        {
          this.activebuffer = this.messages;
          this.filteredmessages.Clear();
        }
        this.searchGui.LogMessageView = (IEnumerable<LogMessage>) this.activebuffer;
        this.bool_6 = flag;
      }
      this.method_7();
      if (this.vertbar != null)
        this.vertbar.Maximum = (double) (this.activebuffer.Count - this.HostItemCount + 1);
      this.searchGui.Update();
    }
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as LogPanel).method_1();
  }

  private void method_1()
  {
    if (!this.LockBottom)
      return;
    this.method_2();
  }

  private void method_2() => this.vertbar.Value = this.vertbar.Maximum;

  public void ScrollToTop(int index) => this.method_3(0);

  private void method_3(int int_3)
  {
    if ((double) int_3 < this.vertbar.Value)
      this.vertbar.Value = (double) int_3;
    else if ((double) int_3 >= this.vertbar.Value + (double) this.viewHost.Children.Count - 2.0)
      this.vertbar.Value = Math.Min(this.vertbar.Maximum, (double) (int_3 - this.viewHost.Children.Count + 3));
    if (this.vertbar.Maximum - this.vertbar.Value > 0.0)
      this.LockBottom = false;
    else
      this.LockBottom = true;
  }

  public void ScrollIntoView(LogMessage message)
  {
    int int_3 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_0 => logMessage_0 == message));
    if (int_3 == -1)
      return;
    this.method_3(int_3);
  }

  public void ScrollIntoView(IEnumerable<LogMessage> message)
  {
    int num1 = this.activebuffer.Count;
    int num2 = 0;
    foreach (LogMessage logMessage in message)
    {
      LogMessage logMessage_0 = logMessage;
      int num3 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => logMessage_1 == logMessage_0));
      if (num3 != -1)
      {
        if (num3 < num1)
          num1 = num3;
        if (num3 > num2)
          num2 = num3;
      }
    }
    if (num2 == 0 && num1 == (int) this.vertbar.Maximum)
      return;
    int int_3_1 = num1 - 1;
    int int_3_2 = num2 + 1;
    this.method_3(int_3_1);
    this.method_3(int_3_2);
    this.method_3(int_3_1);
  }

  public void ClearLog(bool refresh = true)
  {
    this.DebugCount = 0;
    this.MessageCount = 0;
    this.ErrorCount = 0;
    this.WarningCount = 0;
    this.FilterCount = 0;
    this.string_1 = (string) null;
    this.messages.Clear();
    this.filteredmessages.Clear();
    this.selectedMessages.Clear();
    this.int_1 = 0;
    this.MaxSourceWidth = 0;
    this.searchGui.Update();
    this.vertbar.Value = 0.0;
    this.hashSet_0.Clear();
    this.method_5();
    this.method_33();
  }

  public LogPanel()
  {
    this.activebuffer = this.filteredmessages;
    this.SelectedTimeSlots = (IList<TimeSlot>) new List<TimeSlot>();
    this.HyperlinkBrush = (Brush) Brushes.Blue;
    this.InitializeComponent();
    this.ErrorBrush = (Brush) new SolidColorBrush(Color.FromRgb((byte) 233, (byte) 0, (byte) 41));
    this.WarningBrush = (Brush) new SolidColorBrush(Color.FromRgb((byte) 249, (byte) 108, (byte) 37));
    this.TimeSlotBrush = (Brush) new SolidColorBrush(Color.FromArgb((byte) 136, byte.MaxValue, (byte) 0, (byte) 0));
    this.DebugBrush = this.Foreground;
    this.Bind("Foreground", (DependencyObject) this, LogPanel.DebugBrushProperty, converter: (IValueConverter) new OpaqueColorConverter(), converter_parameter: (object) "0.5");
    this.Bind("Foreground", (DependencyObject) this, LogPanel.SelectedBrushProperty, converter: (IValueConverter) new OpaqueColorConverter(), converter_parameter: (object) "0.2");
    this.Bind(nameof (ScrollOffset), (DependencyObject) this.vertbar, RangeBase.ValueProperty, BindingMode.TwoWay);
    this.Loaded += new RoutedEventHandler(this.LogPanel_Loaded);
    this.searchGuiPopUp.OnOpened += new RoutedEventHandler(this.searchGuiPopUp_OnOpened);
    this.filterPopUp.OnOpened += new RoutedEventHandler(this.filterPopUp_OnOpened);
    this.searchGui.LogMessageView = (IEnumerable<LogMessage>) this.activebuffer;
  }

  private void horzbar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
  {
    foreach (UIElement uiElement in this.viewHost.Children.OfType<LogTextShower>())
      uiElement.InvalidateVisual();
  }

  private void vertbar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
  {
    if (((RangeBase) sender).Maximum - e.NewValue > 0.0)
      this.LockBottom = false;
    else
      this.LockBottom = true;
  }

  private void filterPopUp_OnOpened(object sender, RoutedEventArgs e)
  {
    this.searchGuiPopUp_OnOpened(sender, e);
    this.FilterActive = true;
  }

  private void searchGuiPopUp_OnOpened(object sender, RoutedEventArgs e)
  {
    if (!(sender is ControlDropDown dpObj) || !(dpObj.GetVisualChildren().FirstOrDefault<DependencyObject>((Func<DependencyObject, bool>) (dependencyObject_0 => dependencyObject_0 is NotOnTopPopup)) is NotOnTopPopup notOnTopPopup) || !((notOnTopPopup.Child as Grid).GetVisualChildren().FirstOrDefault<DependencyObject>((Func<DependencyObject, bool>) (dependencyObject_0 => dependencyObject_0 is System.Windows.Controls.TextBox)) is System.Windows.Controls.TextBox textBox))
      return;
    textBox.Focus();
    textBox.SelectAll();
  }

  private void LogPanel_Loaded(object sender, RoutedEventArgs e)
  {
    this.Loaded -= new RoutedEventHandler(this.LogPanel_Loaded);
    this.SetResourceReference(LogPanel.HyperlinkBrushProperty, (object) "Hyperlink.Static.Foreground");
    this.SetResourceReference(LogPanel.ErrorBrushProperty, (object) "failColor");
    this.SetResourceReference(LogPanel.WarningBrushProperty, (object) "warningColor");
    this.method_5();
    this.method_4();
  }

  private void method_4()
  {
    this.hashSet_1 = ComponentSettings<GuiControlsSettings>.Current.DisabledLogGroups;
    this.SourceFilterActive = this.hashSet_1.Count > 0;
  }

  private void method_5()
  {
    HashSet<string> activeLogGroups = ComponentSettings<GuiControlsSettings>.Current.DisabledLogGroups;
    List<SelectableLogGroup> list = this.hashSet_0.Select<string, SelectableLogGroup>((Func<string, SelectableLogGroup>) (string_0 => new SelectableLogGroup(activeLogGroups, string_0))).ToList<SelectableLogGroup>();
    list.Sort((Comparison<SelectableLogGroup>) ((selectableLogGroup_0, selectableLogGroup_1) => (selectableLogGroup_0.Name ?? "").CompareTo(selectableLogGroup_1.Name ?? "")));
    this.groupsItems.ItemsSource = (IEnumerable) list;
  }

  private void method_6(object sender, MouseWheelEventArgs e)
  {
    int wheelScrollLines = SystemInformation.MouseWheelScrollLines;
    this.vertbar.Value += e.Delta > 0 ? (double) -wheelScrollLines : (double) wheelScrollLines;
    this.LockBottom = this.vertbar.Value == this.vertbar.Maximum;
    e.Handled = true;
  }

  public void AddLogMessages(IEnumerable<Event> newMessages)
  {
    lock (this.object_0)
    {
      foreach (Event newMessage in newMessages)
        this.list_0.Add(new LogMessage(newMessage));
      this.AddLogMessages((IReadOnlyList<LogMessage>) this.list_0);
      this.list_0.Clear();
    }
  }

  public void AddLogMessages(IReadOnlyList<LogMessage> newMessages)
  {
    if (this.int_0 != this.messages.MaxSize)
    {
      this.messages.SetMaxSize(this.int_0);
      this.filteredmessages.SetMaxSize(this.int_0);
    }
    HashSet<string> newLogGroups = (HashSet<string>) null;
    int int_0 = 0;
    int info = 0;
    int warn = 0;
    int int_1 = 0;
    foreach (LogMessage newMessage in (IEnumerable<LogMessage>) newMessages)
    {
      LogEventType level = newMessage.Level;
      if (level <= 20)
      {
        if (level != 10)
        {
          if (level == 20)
            warn++;
        }
        else
          int_1++;
      }
      else if (level != 30)
      {
        if (level == 40)
          int_0++;
      }
      else
        info++;
      if (!this.hashSet_0.Contains(newMessage.Source))
      {
        if (newLogGroups == null)
          newLogGroups = new HashSet<string>();
        newLogGroups.Add(newMessage.Source);
      }
    }
    this.messages.AddRange((IEnumerable<LogMessage>) newMessages);
    if (this.bool_6)
    {
      foreach (LogMessage newMessage in (IEnumerable<LogMessage>) newMessages)
      {
        if (this.method_9(newMessage))
        {
          this.filteredmessages.Add(newMessage);
          if (newMessage.Source != null)
            this.MaxSourceWidth = Math.Max(this.MaxSourceWidth, newMessage.Source.Length);
          if (newMessage.Message != null)
            this.int_1 = Math.Max(this.int_1, newMessage.Message.Length);
        }
      }
    }
    else
    {
      foreach (LogMessage newMessage in (IEnumerable<LogMessage>) newMessages)
      {
        if (newMessage.Source != null)
          this.MaxSourceWidth = Math.Max(this.MaxSourceWidth, newMessage.Source.Length);
        if (newMessage.Message != null)
          this.int_1 = Math.Max(this.int_1, newMessage.Message.Length);
      }
    }
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      if (newLogGroups != null)
      {
        foreach (string str in newLogGroups)
          this.hashSet_0.Add(str);
        this.method_5();
      }
      this.DebugCount += int_0;
      this.ErrorCount += int_1;
      this.WarningCount += warn;
      this.MessageCount += info;
      this.method_33();
      this.FilterCount = this.activebuffer.Count;
      if (string.IsNullOrWhiteSpace(this.searchGui.SearchString))
        return;
      this.searchGui.Update();
    }));
  }

  private void method_7()
  {
    this.filteredmessages.Clear();
    IEnumerable<LogMessage> items;
    if (this.bool_6)
    {
      items = (IEnumerable<LogMessage>) this.messages.Where<LogMessage>(new Func<LogMessage, bool>(this.method_9)).ToList<LogMessage>();
      this.filteredmessages.AddRange(items);
      this.FilterCount = this.filteredmessages.Count;
    }
    else
      items = (IEnumerable<LogMessage>) this.messages;
    this.vertbar.Maximum = (double) Math.Max(0, this.activebuffer.Count - this.HostItemCount + 1);
    int num1 = 0;
    int num2 = 0;
    foreach (LogMessage logMessage in items)
    {
      int length1 = logMessage.Source.Length;
      int length2 = logMessage.Message.Length;
      num1 = length1 > num1 ? length1 : num1;
      num2 = length2 > num2 ? length2 : num2;
    }
    this.MaxSourceWidth = num1;
    this.int_1 = num2;
    this.method_33();
  }

  private void filterInputField_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (!this.FilterActive)
      return;
    this.method_7();
  }

  private bool method_8(LogMessage logMessage_1)
  {
    return string.IsNullOrEmpty(this.string_0) || (logMessage_1.Message.IndexOf(this.string_0, StringComparison.CurrentCultureIgnoreCase) != -1 || logMessage_1.Source.IndexOf(this.string_0, StringComparison.CurrentCultureIgnoreCase) != -1 ? 1 : (logMessage_1.FormattedTime.IndexOf(this.string_0, StringComparison.CurrentCultureIgnoreCase) != -1 ? 1 : 0)) != 0;
  }

  private bool method_9(LogMessage logMessage_1)
  {
    return this.method_10(logMessage_1.Level) && (!this.bool_7 || this.method_8(logMessage_1)) && !this.hashSet_1.Contains(logMessage_1.Source);
  }

  private bool method_10(LogEventType logEventType_0)
  {
    if (logEventType_0 <= 20)
    {
      if (logEventType_0 == 10)
        return this.bool_3;
      if (logEventType_0 == 20)
        return this.bool_5;
    }
    else
    {
      if (logEventType_0 == 30)
        return this.bool_4;
      if (logEventType_0 == 40)
        return this.bool_2;
    }
    if (((Enum) (object) logEventType_0).HasFlag((Enum) (object) (LogEventType) 30))
      return this.bool_4;
    return !((Enum) (object) logEventType_0).HasFlag((Enum) (object) (LogEventType) 40) || this.bool_2;
  }

  private void method_11(object sender, RoutedEventArgs e)
  {
    string fileName = "";
    try
    {
      fileName = Path.GetFullPath(SessionLogs.GetSessionLogFilePath());
      SessionLogs.Flush();
      Process.Start(fileName);
    }
    catch (Exception ex)
    {
      Log.Warning(LogPanel.traceSource_0, "Failed to open file '{0}'", new object[1]
      {
        (object) fileName
      });
      Log.Debug(LogPanel.traceSource_0, ex);
    }
  }

  private void method_12(object sender, RoutedEventArgs e)
  {
    try
    {
      Process.Start(Path.GetDirectoryName(Path.GetFullPath(SessionLogs.GetSessionLogFilePath())));
    }
    catch (Exception ex)
    {
      Log.Error(LogPanel.traceSource_0, "Failed to open log folder.", Array.Empty<object>());
      Log.Debug(LogPanel.traceSource_0, ex);
    }
  }

  private void method_13(object sender, RoutedEventArgs e) => this.ClearLog();

  private void method_14(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.activebuffer.Count > 0;
  }

  public void SelectedItemsToClipboard()
  {
    IEnumerable<LogMessage> logMessages = this.activebuffer.Where<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => this.selectedMessages.Contains(logMessage_1)));
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in logMessages)
      stringBuilder.AppendLine(obj.ToString());
    ClipboardHelper.CopyText(stringBuilder.ToString());
  }

  private void basegrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
  {
    bool flag = Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control);
    switch (e.Key)
    {
      case Key.End:
        this.vertbar.Value = this.vertbar.Maximum;
        this.LockBottom = true;
        goto case Key.Left;
      case Key.Home:
        this.vertbar.Value = 0.0;
        this.LockBottom = false;
        goto case Key.Left;
      case Key.Left:
      case Key.Right:
        using (IEnumerator<LogTextShower> enumerator = this.viewHost.Children.OfType<LogTextShower>().GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.InvalidateVisual();
          break;
        }
      case Key.Up:
        if (flag)
        {
          this.vertbar.Value = Math.Max(0.0, this.vertbar.Value - 1.0);
          this.LockBottom = false;
          goto case Key.Left;
        }
        if (this.logMessage_0 != null)
        {
          int num = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => logMessage_1 == this.logMessage_0));
          if (num != -1)
            this.SelectedMessages = (IList<LogMessage>) new List<LogMessage>()
            {
              {
                this.logMessage_0 = this.activebuffer[Math.Max(0, num - 1)]
              }
            };
        }
        this.ScrollIntoView(this.logMessage_0);
        goto case Key.Left;
      case Key.Down:
        if (flag)
        {
          this.vertbar.Value = Math.Min(this.vertbar.Maximum, this.vertbar.Value + 1.0);
          this.LockBottom = false;
          goto case Key.Left;
        }
        if (this.logMessage_0 != null)
        {
          int num = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => logMessage_1 == this.logMessage_0));
          if (num != -1)
            this.SelectedMessages = (IList<LogMessage>) new List<LogMessage>()
            {
              {
                this.logMessage_0 = this.activebuffer[Math.Min(this.activebuffer.Count - 1, num + 1)]
              }
            };
        }
        this.ScrollIntoView(this.logMessage_0);
        goto case Key.Left;
      case Key.A:
        if (flag)
        {
          this.SelectedMessages = (IList<LogMessage>) this.activebuffer.ToList<LogMessage>();
          goto case Key.Left;
        }
        goto case Key.Left;
      case Key.C:
        if (flag)
        {
          e.Handled = true;
          this.SelectedItemsToClipboard();
          goto case Key.Left;
        }
        goto case Key.Left;
      default:
        e.Handled = false;
        goto case Key.Left;
    }
  }

  private void method_15(object sender, RoutedEventArgs e) => this.SelectedItemsToClipboard();

  private void method_16(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.selectedMessages.Count > 0;
  }

  private void method_17(object sender, RoutedEventArgs e)
  {
    SessionLogs.Flush();
    string fullPath = Path.GetFullPath(SessionLogs.GetSessionLogFilePath());
    Type type = PluginManager.GetPlugins<ILogAnalyzer>().FirstOrDefault<Type>();
    if (type == (Type) null)
    {
      Log.Error(LogPanel.traceSource_0, "Could not find any installed Timing Analyzer.", Array.Empty<object>());
    }
    else
    {
      ILogAnalyzer instance = (ILogAnalyzer) Activator.CreateInstance(type);
      instance.ParentWindow = System.Windows.Application.Current.MainWindow;
      instance.LoadProcess((IEnumerable<string>) new string[1]
      {
        fullPath
      }, (IEnumerable<string>) null, false);
    }
  }

  private void SearchGui_ScrollTo(int absolutePosition)
  {
    this.method_3(absolutePosition);
    this.SelectedMessages = (IList<LogMessage>) new LogMessage[1]
    {
      this.activebuffer[absolutePosition]
    };
  }

  private void method_18(object sender, ExecutedRoutedEventArgs e)
  {
    this.searchGuiPopUp.IsDropDownOpen = true;
    this.filterPopUp.IsDropDownOpen = false;
    this.searchGui.SearchStringBox.Focus();
    this.searchGui.SearchStringBox.SelectAll();
  }

  private void method_19(object sender, ExecutedRoutedEventArgs e)
  {
    if (this.searchGuiPopUp.IsDropDownOpen)
    {
      this.searchGuiPopUp.IsDropDownOpen = false;
      this.searchGuiPopUp.Focus();
      Keyboard.Focus((IInputElement) this.searchGuiPopUp);
    }
    if (this.filterPopUp.IsDropDownOpen)
    {
      this.filterPopUp.IsDropDownOpen = false;
      this.filterPopUp.IsEnabled = true;
      this.filterPopUp.Focus();
      Keyboard.Focus((IInputElement) this.filterPopUp);
    }
    this.GetVisualChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (frameworkElement_0 => frameworkElement_0.Focusable))?.Focus();
  }

  private void method_20(object sender, ExecutedRoutedEventArgs e)
  {
    this.filterPopUp.IsDropDownOpen = true;
    this.searchGuiPopUp.IsDropDownOpen = false;
    this.filterPopUp_OnOpened((object) this.filterPopUp, (RoutedEventArgs) e);
  }

  private void method_21(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right)
      return;
    List<DependencyObject> results = new List<DependencyObject>();
    Point position = e.GetPosition((IInputElement) this.viewHost);
    Mouse.Capture((IInputElement) this.viewHost);
    this.viewHost.MouseLeftButtonUp += new MouseButtonEventHandler(this.viewHost_MouseLeftButtonUp);
    VisualTreeHelper.HitTest((Visual) this.viewHost, (HitTestFilterCallback) (dependencyObject_0 => HitTestFilterBehavior.Continue), (HitTestResultCallback) (result =>
    {
      results.Add(result.VisualHit);
      return HitTestResultBehavior.Continue;
    }), (HitTestParameters) new PointHitTestParameters(position));
    LogTextShower logTextShower = results.OfType<LogTextShower>().FirstOrDefault<LogTextShower>();
    if (logTextShower != null && logTextShower.Text != null)
    {
      LogMessage logMessage_0_1 = logTextShower.Text;
      bool flag1;
      if ((flag1 = this.selectedMessages.Contains(logMessage_0_1)) && e.ChangedButton == MouseButton.Right)
        return;
      int num = Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) ? 1 : 0;
      bool flag2 = Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Shift);
      if (num == 0 && !flag2)
        this.selectedMessages.Clear();
      if (flag2)
      {
        IEnumerable<LogMessage> logMessages;
        using (this.activebuffer.Lock())
        {
          int val1 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_0 => logMessage_0 == this.logMessage_0));
          int val2 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => logMessage_1 == logMessage_0_1));
          logMessages = val1 == -1 || val2 == -1 ? Enumerable.Empty<LogMessage>() : this.activebuffer.GetRange(Math.Min(val1, val2), Math.Abs(val2 - val1) + 1);
        }
        foreach (LogMessage logMessage in logMessages)
          this.selectedMessages.Add(logMessage);
      }
      else if (!flag1)
        this.selectedMessages.Add(logMessage_0_1);
      this.method_22();
      this.logMessage_0 = logMessage_0_1;
    }
    e.Handled = false;
  }

  private void viewHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.viewHost.MouseLeftButtonUp -= new MouseButtonEventHandler(this.viewHost_MouseLeftButtonUp);
    this.viewHost.ReleaseMouseCapture();
  }

  private void method_22()
  {
    this.SelectedMessages = (IList<LogMessage>) this.activebuffer.Where<LogMessage>(new Func<LogMessage, bool>(this.selectedMessages.Contains)).ToList<LogMessage>();
  }

  private void method_23(object sender, System.Windows.Input.MouseEventArgs e)
  {
    if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
    {
      Point position = e.GetPosition((IInputElement) this.viewHost);
      if (!this.bool_8)
      {
        this.double_0 = 0.0;
        this.bool_8 = true;
        this.point_0 = position;
      }
      double num1 = this.point_0.X - position.X;
      double num2 = this.point_0.Y - position.Y;
      if (position.Y < 0.0)
      {
        this.vertbar.Value += position.Y / this.LogFontSize;
        position.Y = 0.0;
      }
      if (position.Y > this.viewHostGrid.ActualHeight)
      {
        this.vertbar.Value += (position.Y - this.viewHostGrid.ActualHeight) / this.LogFontSize;
        position.Y = this.viewHostGrid.ActualHeight - 1.0;
      }
      this.double_0 += Math.Sqrt(num1 * num1 + num2 * num2);
      if (this.double_0 > 5.0)
      {
        Point point = position;
        List<DependencyObject> results = new List<DependencyObject>();
        VisualTreeHelper.HitTest((Visual) this.viewHost, (HitTestFilterCallback) (dependencyObject_0 => HitTestFilterBehavior.Continue), (HitTestResultCallback) (result =>
        {
          results.Add(result.VisualHit);
          return HitTestResultBehavior.Continue;
        }), (HitTestParameters) new PointHitTestParameters(point));
        LogTextShower logtext = results.OfType<LogTextShower>().FirstOrDefault<LogTextShower>();
        if (logtext != null)
        {
          int gparam_0 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_1 => logMessage_1 == this.logMessage_0));
          int gparam_1 = this.activebuffer.IndexWhen<LogMessage>((Func<LogMessage, bool>) (logMessage_0 => logMessage_0 == logtext.Text));
          if (gparam_0 != -1 && gparam_1 != -1)
          {
            if (gparam_0 > gparam_1)
              Utils.Swap<int>(ref gparam_0, ref gparam_1);
            this.selectedMessages.Clear();
            foreach (LogMessage logMessage in this.activebuffer.Skip<LogMessage>(gparam_0).Take<LogMessage>(gparam_1 - gparam_0 + 1))
              this.selectedMessages.Add(logMessage);
            this.method_22();
          }
        }
      }
    }
    else
      this.bool_8 = false;
    e.Handled = false;
  }

  private void This_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => this.bool_8 = false;

  private void filterInputField_Loaded(object sender, RoutedEventArgs e)
  {
    this.searchGuiPopUp.IsDropDownOpen = false;
  }

  private void basegrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    this.Focus();
    Keyboard.Focus((IInputElement) this.basegrid);
  }

  protected override void OnPreviewMouseMove(System.Windows.Input.MouseEventArgs mouseEventArgs_0)
  {
    base.OnPreviewMouseMove(mouseEventArgs_0);
    if (this.gridContextMenu.IsOpen)
      return;
    LogTextShower shower = this.viewHost.Children.OfType<LogTextShower>().FirstOrDefault<LogTextShower>((Func<LogTextShower, bool>) (logTextShower_0 => logTextShower_0.IsMouseOver));
    if (shower == null)
      return;
    GuiHelpers.GuiInvokeAsync((Action) (() => this.string_1 = shower.CheckLink(Mouse.GetPosition((IInputElement) shower))), priority: DispatcherPriority.ApplicationIdle);
  }

  private void method_24(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.string_1 != null;
  }

  private void method_25(object sender, ExecutedRoutedEventArgs e)
  {
    this.method_29(this.string_1 ?? "NULL");
  }

  private void method_26(object sender, ExecutedRoutedEventArgs e) => this.method_28(this.string_1);

  private void method_27(object sender, ExecutedRoutedEventArgs e) => this.OpenLink(this.string_1);

  private void method_28(string string_2)
  {
    if (string_2.StartsWith("vs: "))
    {
      int num = string_2.LastIndexOf(":");
      this.method_28(string_2.Substring(4, num - 4));
    }
    else
    {
      try
      {
        Process.Start(Path.GetDirectoryName(string_2));
      }
      catch (Exception ex)
      {
        QuickDialog.ShowMessage("Unable To Open Folder", $"Caught Error while opening\n{string_2}\nSee log for details.");
        Log.Debug(LogPanel.traceSource_0, ex);
      }
    }
  }

  private void method_29(string string_2)
  {
    if (string_2.StartsWith("vs: "))
    {
      int num = string_2.LastIndexOf(":");
      this.method_29(string_2.Substring(4, num - 4));
    }
    else
    {
      try
      {
        ClipboardHelper.CopyText(string_2);
      }
      catch (Exception ex)
      {
        QuickDialog.ShowMessage("Unable To Copy", $"While copying '{string_2}' to clipboard\nSee log for details.");
        Log.Debug(LogPanel.traceSource_0, ex);
      }
    }
  }

  public void OpenLink(string path)
  {
    if (path.StartsWith("vs: "))
    {
      int num = path.LastIndexOf(":");
      this.method_30(path.Substring(4, num - 4), int.Parse(path.Substring(num + 1)));
    }
    else
    {
      try
      {
        Process.Start(path);
      }
      catch (Exception ex)
      {
        QuickDialog.ShowMessage("Unable To Open", $"Caught Error while opening\n{path}\nSee log for details.");
        Log.Debug(LogPanel.traceSource_0, ex);
      }
    }
  }

  private void method_30(string string_2, int int_3)
  {
    try
    {
      FileAssociations.OpenFileAssociation(string_2, int_3);
    }
    catch (FileAssociations.FileAssociationException ex)
    {
      Log.Error(LogPanel.traceSource_0, "Cannot find file association for {0}.", new object[1]
      {
        (object) ex.Extension
      });
    }
    catch (Exception ex)
    {
      Log.Error(LogPanel.traceSource_0, "Unable to load source code link: {0}", new object[1]
      {
        (object) ex
      });
      Log.Debug(LogPanel.traceSource_0, ex);
    }
  }

  public Typeface LogFont { get; private set; }

  public DpiScale Dpi { get; private set; }

  public double LogFontSize { get; private set; }

  private void method_31(object sender, EventArgs e)
  {
    foreach (UIElement child in this.viewHost.Children)
      child.InvalidateVisual();
  }

  private void method_32(object sender, RoutedEventArgs e)
  {
    int num1 = this.SourceFilterActive ? 1 : 0;
    this.method_4();
    int num2 = this.SourceFilterActive ? 1 : 0;
    if (num1 != num2)
      return;
    this.method_7();
  }

  private void method_33()
  {
    if (this.LogFont == null)
    {
      this.LogFont = new Typeface(TextBlock.GetFontFamily((DependencyObject) this.viewHost), TextBlock.GetFontStyle((DependencyObject) this.viewHost), TextBlock.GetFontWeight((DependencyObject) this.viewHost), FontStretches.Normal);
      this.Dpi = VisualTreeHelper.GetDpi((Visual) this.viewHost);
      this.LogFontSize = TextBlock.GetFontSize((DependencyObject) this.viewHost);
      FormattedText formattedText = new FormattedText("O", CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, this.LogFont, this.LogFontSize, this.Foreground, (NumberSubstitution) null, TextFormattingMode.Display, this.Dpi.PixelsPerDip);
      this.charwidth = formattedText.Width;
      this.charheight = formattedText.Height;
    }
    if (this.LogFont == null)
      return;
    double num1 = Math.Ceiling(this.charheight);
    int num2 = (int) ((this.viewHostGrid.ActualHeight + num1 * 0.8) / num1);
    while (this.HostItemCount < num2)
    {
      UIElementCollection children = this.viewHost.Children;
      LogTextShower element = new LogTextShower(this, this.viewHost.Children.Count);
      element.Height = num1;
      children.Add((UIElement) element);
    }
    while (this.HostItemCount > num2)
      this.viewHost.Children.RemoveAt(this.viewHost.Children.Count - 1);
    this.vertbar.Maximum = (double) Math.Max(0, this.activebuffer.Count - this.HostItemCount + 1);
    if (this.LockBottom)
      this.vertbar.Value = this.vertbar.Maximum;
    this.horzbar.Maximum = (double) (this.int_1 + this.MaxSourceWidth + 15) - this.viewHost.ActualWidth / this.charwidth + 2.0;
  }

  private void viewHostGrid_SizeChanged(object sender, SizeChangedEventArgs e) => this.method_33();

  public int MaxSourceWidth { get; private set; }

  public int HostItemCount => this.viewHost.Children.Count;

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_9)
      return;
    this.bool_9 = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/logpanel.xaml", UriKind.Relative));
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
        this.This = (LogPanel) target;
        this.This.MouseLeftButtonUp += new MouseButtonEventHandler(this.This_MouseLeftButtonUp);
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_27);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_24);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_26);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_24);
        break;
      case 4:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_25);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_24);
        break;
      case 5:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_13);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_14);
        break;
      case 6:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_15);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_16);
        break;
      case 7:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_11);
        break;
      case 8:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_12);
        break;
      case 9:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_17);
        break;
      case 10:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_18);
        break;
      case 11:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_20);
        break;
      case 12:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_19);
        break;
      case 13:
        this.gridContextMenu = (System.Windows.Controls.ContextMenu) target;
        break;
      case 14:
        this.openLinkBtn = (System.Windows.Controls.MenuItem) target;
        break;
      case 15:
        this.basegrid = (Grid) target;
        this.basegrid.PreviewMouseDown += new MouseButtonEventHandler(this.basegrid_PreviewMouseDown);
        this.basegrid.KeyDown += new System.Windows.Input.KeyEventHandler(this.basegrid_KeyDown);
        break;
      case 16 /*0x10*/:
        this.LogPanelToolBarGrid = (Grid) target;
        break;
      case 17:
        this.errorToggle = (System.Windows.Controls.CheckBox) target;
        break;
      case 18:
        this.warningToggle = (System.Windows.Controls.CheckBox) target;
        break;
      case 19:
        this.infoToggle = (System.Windows.Controls.CheckBox) target;
        break;
      case 20:
        this.debugToggle = (System.Windows.Controls.CheckBox) target;
        break;
      case 21:
        this.groups = (ControlDropDown) target;
        break;
      case 22:
        this.groupsItems = (ItemsControl) target;
        break;
      case 24:
        this.searchGuiPopUp = (ControlDropDown) target;
        break;
      case 25:
        this.searchGui = (SearchGui) target;
        break;
      case 26:
        this.filterPopUp = (ControlDropDown) target;
        break;
      case 27:
        this.filterControlDropDownGrid = (Grid) target;
        break;
      case 28:
        this.filterInputField = (System.Windows.Controls.TextBox) target;
        this.filterInputField.TextChanged += new TextChangedEventHandler(this.filterInputField_TextChanged);
        this.filterInputField.Loaded += new RoutedEventHandler(this.filterInputField_Loaded);
        break;
      case 29:
        this.foundCount = (TextBlock) target;
        break;
      case 30:
        this.autoscrollButton = (ToggleButton) target;
        break;
      case 31 /*0x1F*/:
        ((UIElement) target).MouseMove += new System.Windows.Input.MouseEventHandler(this.method_23);
        ((UIElement) target).MouseDown += new MouseButtonEventHandler(this.method_21);
        ((UIElement) target).MouseWheel += new MouseWheelEventHandler(this.method_6);
        break;
      case 32 /*0x20*/:
        this.viewHostGrid = (Grid) target;
        this.viewHostGrid.SizeChanged += new SizeChangedEventHandler(this.viewHostGrid_SizeChanged);
        break;
      case 33:
        this.viewHost = (StackPanel) target;
        break;
      case 34:
        this.horzbar = (System.Windows.Controls.Primitives.ScrollBar) target;
        this.horzbar.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(this.horzbar_Scroll);
        break;
      case 35:
        this.vertbar = (System.Windows.Controls.Primitives.ScrollBar) target;
        this.vertbar.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(this.vertbar_Scroll);
        break;
      default:
        this.bool_9 = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 23)
      return;
    ((ToggleButton) target).Checked += new RoutedEventHandler(this.method_32);
    ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.method_32);
  }
}
