// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SearchGui
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SearchGui : UserControl, IComponentConnector
{
  public static RoutedUICommand NextCommand = new RoutedUICommand("Selects the next message", "Next", typeof (SearchGui), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F3),
    (InputGesture) new KeyGesture(Key.Return)
  });
  public static RoutedUICommand PreviousCommand = new RoutedUICommand("Selects the previous message", "Previous", typeof (SearchGui), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.F3, ModifierKeys.Shift),
    (InputGesture) new KeyGesture(Key.Return, ModifierKeys.Shift)
  });
  public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register(nameof (SearchString), typeof (string), typeof (SearchGui), new PropertyMetadata((object) "", new PropertyChangedCallback(SearchGui.smethod_1)));
  public static readonly DependencyProperty LogMessageViewProperty = DependencyProperty.Register(nameof (LogMessageView), typeof (IEnumerable<LogMessage>), typeof (SearchGui), new PropertyMetadata((object) null, new PropertyChangedCallback(SearchGui.smethod_2)));
  public static readonly DependencyProperty NumberOfMatchesProperty = DependencyProperty.Register(nameof (NumberOfMatches), typeof (int), typeof (SearchGui));
  public static readonly DependencyProperty NumberOfMessagesProperty = DependencyProperty.Register(nameof (NumberOfMessages), typeof (int), typeof (SearchGui));
  public static readonly DependencyProperty CurrentMessageProperty = DependencyProperty.Register(nameof (CurrentMessage), typeof (int), typeof (SearchGui), new PropertyMetadata((object) 0, new PropertyChangedCallback(SearchGui.smethod_0)));
  private string string_0;
  private readonly Timer timer_0 = new Timer()
  {
    Interval = 200.0,
    AutoReset = false
  };
  private List<int> list_0;
  private bool bool_1;
  internal SearchGui This;
  internal TextBox SearchStringBox;
  internal Button PreviousButton;
  internal Button NextButton;
  private bool bool_2;

  public string SearchString
  {
    get => (string) this.GetValue(SearchGui.SearchStringProperty);
    set => this.SetValue(SearchGui.SearchStringProperty, (object) value);
  }

  public IEnumerable<LogMessage> LogMessageView
  {
    get => (IEnumerable<LogMessage>) this.GetValue(SearchGui.LogMessageViewProperty);
    set => this.SetValue(SearchGui.LogMessageViewProperty, (object) value);
  }

  public int NumberOfMatches
  {
    get => (int) this.GetValue(SearchGui.NumberOfMatchesProperty);
    set => this.SetValue(SearchGui.NumberOfMatchesProperty, (object) value);
  }

  public int NumberOfMessages
  {
    get => (int) this.GetValue(SearchGui.NumberOfMessagesProperty);
    set => this.SetValue(SearchGui.NumberOfMessagesProperty, (object) value);
  }

  public int CurrentMessage
  {
    get => (int) this.GetValue(SearchGui.CurrentMessageProperty);
    set => this.SetValue(SearchGui.CurrentMessageProperty, (object) value);
  }

  public event SearchGui.ScrollToDelegate ScrollTo;

  public event EventHandler TextChanged;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as SearchGui).method_5();
  }

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as SearchGui).method_0();
  }

  private void method_0() => this.method_3();

  public bool CurrentlyVisible { get; set; }

  public string SearchText { get; set; }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == UIElement.IsVisibleProperty)
    {
      this.CurrentlyVisible = (bool) dependencyPropertyChangedEventArgs_0.NewValue;
      if (!this.CurrentlyVisible)
      {
        this.string_0 = this.SearchString;
        this.SearchString = "";
      }
      else
        this.SearchString = this.string_0;
      this.Update();
    }
    this.SearchText = this.SearchString;
  }

  private void method_1() => this.method_3();

  private static void smethod_2(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as SearchGui).method_1();
  }

  private static bool smethod_3(LogMessage logMessage_0, string string_2)
  {
    if (string.IsNullOrEmpty(string_2))
      return false;
    string_2 = string_2.ToLower();
    return StringSearch.Contains(logMessage_0.Message, string_2, true) || StringSearch.Contains(logMessage_0.Source, string_2, true) || StringSearch.Contains(logMessage_0.FormattedTime, string_2, true);
  }

  private void method_2()
  {
    this.timer_0.Stop();
    this.timer_0.Start();
  }

  private void method_3(int int_0 = 0)
  {
    if (!this.bool_1)
      return;
    this.method_2();
  }

  private void method_4()
  {
    List<int> messageEntries = new List<int>();
    int num = 0;
    IEnumerable<LogMessage> logMessages = (IEnumerable<LogMessage>) null;
    string searchString = "";
    GuiHelpers.GuiInvoke((Action) (() =>
    {
      logMessages = this.LogMessageView;
      searchString = this.SearchString;
    }));
    foreach (LogMessage logMessage_0 in logMessages)
    {
      if (SearchGui.smethod_3(logMessage_0, searchString))
        messageEntries.Add(num);
      ++num;
    }
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      this.NumberOfMessages = this.LogMessageView.Count<LogMessage>();
      this.NumberOfMatches = messageEntries.Count;
      this.list_0 = messageEntries;
      if (this.CurrentMessage > this.NumberOfMatches || this.CurrentMessage <= 0)
        this.CurrentMessage = 1;
      this.method_5();
      // ISSUE: reference to a compiler-generated field
      if (this.eventHandler_0 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.eventHandler_0((object) this, new EventArgs());
    }));
  }

  private void method_5()
  {
    if (this.CurrentMessage == 0 || this.list_0.Count <= 0)
      return;
    this.method_6(this.list_0[this.CurrentMessage - 1]);
  }

  public void Update() => this.method_2();

  public SearchGui()
  {
    this.InitializeComponent();
    this.timer_0.Elapsed += new ElapsedEventHandler(this.timer_0_Elapsed);
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.SearchGui_IsVisibleChanged);
  }

  private void SearchGui_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.bool_1 = (bool) e.NewValue;
    if (!this.bool_1)
      return;
    this.method_3();
  }

  private void timer_0_Elapsed(object sender, ElapsedEventArgs e) => this.method_4();

  private void method_6(int int_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.scrollToDelegate_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.scrollToDelegate_0(int_0);
  }

  public void GoToNextFoundMessage()
  {
    if (this.CurrentMessage + 1 <= this.NumberOfMatches)
    {
      ++this.CurrentMessage;
    }
    else
    {
      if (this.NumberOfMatches <= 0)
        return;
      this.CurrentMessage = 1;
    }
  }

  public void GoToPreviousFoundMessage()
  {
    if (this.CurrentMessage - 1 >= 1)
    {
      --this.CurrentMessage;
    }
    else
    {
      if (this.NumberOfMatches <= 0)
        return;
      this.CurrentMessage = this.NumberOfMatches;
    }
  }

  private void method_7(object sender, ExecutedRoutedEventArgs e) => this.GoToNextFoundMessage();

  private void method_8(object sender, ExecutedRoutedEventArgs e)
  {
    this.GoToPreviousFoundMessage();
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_2)
      return;
    this.bool_2 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/searchgui.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.This = (SearchGui) target;
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_7);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_8);
        break;
      case 4:
        this.SearchStringBox = (TextBox) target;
        break;
      case 5:
        this.PreviousButton = (Button) target;
        break;
      case 6:
        this.NextButton = (Button) target;
        break;
      default:
        this.bool_2 = true;
        break;
    }
  }

  public delegate void ScrollToDelegate(int absolutePosition);
}
