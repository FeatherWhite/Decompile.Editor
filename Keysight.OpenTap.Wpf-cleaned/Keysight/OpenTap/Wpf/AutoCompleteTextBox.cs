// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AutoCompleteTextBox
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AutoCompleteTextBox : UserControl, IComponentConnector
{
  private readonly ControlDropDown controlDropDown_0;
  private readonly ListBox listBox_0;
  public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register(nameof (Provider), typeof (IAutoCompleteProvider), typeof (AutoCompleteTextBox), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (AutoCompleteTextBox));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (AutoCompleteTextBox));
  internal AutoCompleteTextBox thisCompleteBox;
  internal ControlDropDown PART_Popup;
  internal TextBox PART_textbox;
  internal ListBox PART_ListBox;
  private bool bool_0;

  public IAutoCompleteProvider Provider
  {
    get => (IAutoCompleteProvider) this.GetValue(AutoCompleteTextBox.ProviderProperty);
    set => this.SetValue(AutoCompleteTextBox.ProviderProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(AutoCompleteTextBox.TextProperty);
    set => this.SetValue(AutoCompleteTextBox.TextProperty, (object) value);
  }

  public IEnumerable ItemsSource
  {
    get => (IEnumerable) this.GetValue(AutoCompleteTextBox.ItemsSourceProperty);
    set => this.SetValue(AutoCompleteTextBox.ItemsSourceProperty, (object) value);
  }

  public event TextChangedEventHandler TextChanged;

  public TextBox TextBox => this.PART_textbox;

  public AutoCompleteTextBox()
  {
    this.InitializeComponent();
    this.controlDropDown_0 = this.PART_Popup;
    this.listBox_0 = this.PART_ListBox;
    this.PART_textbox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
    this.PART_textbox.PreviewKeyDown += new KeyEventHandler(this.PART_textbox_PreviewKeyDown);
    this.PART_textbox.PreviewKeyUp += new KeyEventHandler(this.PART_textbox_PreviewKeyUp);
    this.listBox_0.PreviewMouseDown += new MouseButtonEventHandler(this.listBox_0_PreviewMouseDown);
    this.listBox_0.KeyDown += new KeyEventHandler(this.listBox_0_KeyDown);
    this.Provider = (IAutoCompleteProvider) new SimpleListCompletionProvider(this.ItemsSource);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != AutoCompleteTextBox.ItemsSourceProperty)
      return;
    this.Provider = (IAutoCompleteProvider) new SimpleListCompletionProvider(this.ItemsSource);
  }

  private void PART_textbox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (!this.controlDropDown_0.IsDropDownOpen)
      return;
    e.Handled = e.Key == Key.Tab || e.Key == Key.Return;
  }

  private void PART_textbox_PreviewKeyUp(object sender, KeyEventArgs e)
  {
    if (this.listBox_0 == null || this.PART_textbox == null)
      return;
    switch (e.Key)
    {
      case Key.Tab:
      case Key.Return:
        this.method_1(this.listBox_0.SelectedItem);
        break;
      case Key.Escape:
        this.Focus();
        this.method_2();
        break;
      case Key.Up:
        if (!object.Equals((object) this.PART_textbox, (object) FocusManager.GetFocusedElement(FocusManager.GetFocusScope((DependencyObject) this.PART_textbox))) || this.listBox_0.Items.Count <= 0)
          break;
        if (this.listBox_0.SelectedIndex <= 0)
          this.listBox_0.SelectedIndex = this.listBox_0.Items.Count - 1;
        else
          this.listBox_0.SelectedIndex = (this.listBox_0.SelectedIndex - 1) % this.listBox_0.Items.Count;
        this.listBox_0.ScrollIntoView(this.listBox_0.SelectedItem);
        break;
      case Key.Down:
        if (!object.Equals((object) this.PART_textbox, (object) FocusManager.GetFocusedElement(FocusManager.GetFocusScope((DependencyObject) this.PART_textbox))) || this.listBox_0.Items.Count <= 0)
          break;
        this.listBox_0.SelectedIndex = (this.listBox_0.SelectedIndex + 1) % this.listBox_0.Items.Count;
        this.listBox_0.ScrollIntoView(this.listBox_0.SelectedItem);
        break;
    }
  }

  private void method_0(string string_0)
  {
    if (this.Text == "")
    {
      this.method_2();
    }
    else
    {
      if (this.Provider == null)
        return;
      this.listBox_0.ItemsSource = (IEnumerable) this.Provider.GetSuggestions(string_0, this.PART_textbox.CaretIndex).ToArray();
      if (this.listBox_0.Items.Count == 0)
      {
        if (!this.controlDropDown_0.IsDropDownOpen)
          return;
        this.method_2();
      }
      else
      {
        this.method_3();
        if (this.listBox_0.Items.Count == 1)
          this.listBox_0.SelectedItem = this.listBox_0.Items.GetItemAt(0);
        this.listBox_0.UpdateLayout();
      }
    }
  }

  protected void OnTextChanged(object sender, TextChangedEventArgs e)
  {
    if (!(sender is TextBox textBox) || !textBox.IsKeyboardFocusWithin)
      return;
    this.method_0(this.Text ?? "");
    // ISSUE: reference to a compiler-generated field
    TextChangedEventHandler changedEventHandler0 = this.textChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, e);
  }

  private void listBox_0_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    DependencyObject parent = (DependencyObject) ((DependencyObject) e.OriginalSource).FindParent<ListBoxItem>();
    if (parent == null)
      return;
    object object_0 = this.listBox_0.ItemContainerGenerator.ItemFromContainer(parent);
    if (object_0 == null)
      return;
    this.method_1(object_0);
  }

  private void listBox_0_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return && e.Key != Key.Return && e.Key != Key.Tab)
      return;
    this.method_1(this.listBox_0.SelectedItem);
  }

  private void method_1(object object_0)
  {
    if (!this.PART_Popup.IsDropDownOpen)
      return;
    AutoCompleteItem autoCompleteItem = (AutoCompleteItem) object_0;
    this.PART_textbox.Focus();
    this.method_2();
    if (autoCompleteItem == null)
      return;
    this.PART_textbox.BeginChange();
    this.PART_textbox.SetCurrentValue(TextBox.TextProperty, (object) autoCompleteItem.FullReplacement);
    this.PART_textbox.CaretIndex = autoCompleteItem.NewCaretPosition;
    this.PART_textbox.EndChange();
    this.SetCurrentValue(AutoCompleteTextBox.TextProperty, (object) this.Text);
    // ISSUE: reference to a compiler-generated field
    TextChangedEventHandler changedEventHandler0 = this.textChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.Clear));
  }

  private void method_2()
  {
    if (!this.controlDropDown_0.IsDropDownOpen)
      return;
    this.listBox_0.SelectedItem = (object) null;
    this.controlDropDown_0.IsDropDownOpen = false;
  }

  private void method_3() => this.controlDropDown_0.IsDropDownOpen = true;

  private void method_4(object sender, RoutedEventArgs e)
  {
    this.listBox_0.ItemsSource = (IEnumerable) this.Provider.GetSuggestions("", this.PART_textbox.CaretIndex).ToArray();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/autocompletetextbox.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.thisCompleteBox = (AutoCompleteTextBox) target;
        break;
      case 2:
        this.PART_Popup = (ControlDropDown) target;
        break;
      case 3:
        this.PART_textbox = (TextBox) target;
        break;
      case 4:
        this.PART_ListBox = (ListBox) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }
}
