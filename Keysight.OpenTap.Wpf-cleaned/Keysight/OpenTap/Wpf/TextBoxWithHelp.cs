// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TextBoxWithHelp
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[DebuggerDisplay("TextBoxWithHelp: {Text}")]
public class TextBoxWithHelp : ContentControl
{
  private TextBox textBox_0;
  private AutoCompleteTextBox autoCompleteTextBox_0;
  private SimpleTextLine simpleTextLine_0;
  public static readonly DependencyProperty SuggestedValuesProperty = DependencyProperty.Register(nameof (SuggestedValues), typeof (IEnumerable), typeof (TextBoxWithHelp));
  private int lines;
  private bool suggestedValues;
  public Action<string> UpdateSource;
  private bool bool_1;
  private string string_1;
  private int int_0 = -1;

  public bool ReadOnlyBox { get; set; }

  public string Text
  {
    get
    {
      TextBox textBox0 = this.textBox_0;
      string text1;
      if (textBox0 == null)
      {
        text1 = (string) null;
      }
      else
      {
        text1 = textBox0.Text;
        if (text1 != null)
          return text1;
      }
      AutoCompleteTextBox completeTextBox0 = this.autoCompleteTextBox_0;
      string text2;
      if (completeTextBox0 == null)
      {
        text2 = (string) null;
      }
      else
      {
        text2 = completeTextBox0.Text;
        if (text2 != null)
          return text2;
      }
      return this.simpleTextLine_0?.Text;
    }
  }

  public IEnumerable SuggestedValues
  {
    get => (IEnumerable) this.GetValue(TextBoxWithHelp.SuggestedValuesProperty);
    set => this.SetValue(TextBoxWithHelp.SuggestedValuesProperty, (object) value);
  }

  public string HelpLink { get; }

  public TextBoxWithHelp(int lines, bool readOnlyBox, bool suggestedValues)
  {
    this.ReadOnlyBox = readOnlyBox;
    this.suggestedValues = suggestedValues;
    this.Loaded += new RoutedEventHandler(this.TextBoxWithHelp_Loaded);
    this.lines = lines;
    this.Focusable = false;
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == TextBoxWithHelp.SuggestedValuesProperty && this.autoCompleteTextBox_0 != null)
      this.method_0();
    if (dependencyPropertyChangedEventArgs_0.Property != FrameworkElement.DataContextProperty || object.Equals(dependencyPropertyChangedEventArgs_0.NewValue, dependencyPropertyChangedEventArgs_0.OldValue) || !(dependencyPropertyChangedEventArgs_0.NewValue is string newValue))
      return;
    TextBox textBox0 = this.textBox_0;
    if ((textBox0 != null ? (textBox0.IsFocused ? 1 : 0) : 0) != 0)
      return;
    this.method_1(newValue, false);
  }

  private void method_0()
  {
    AutoCompleteTextBox completeTextBox0 = this.autoCompleteTextBox_0;
    IEnumerable suggestedValues = this.SuggestedValues;
    string[] strArray;
    if (suggestedValues == null)
    {
      strArray = (string[]) null;
    }
    else
    {
      strArray = suggestedValues.OfType<AnnotationCollection>().Select<AnnotationCollection, string>((Func<AnnotationCollection, string>) (annotation => annotation.Get<IStringValueAnnotation>(false, (object) null).Value ?? "")).ToArray<string>();
      if (strArray != null)
        goto label_4;
    }
    strArray = Array.Empty<string>();
label_4:
    completeTextBox0.ItemsSource = (IEnumerable) strArray;
  }

  private void TextBoxWithHelp_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.suggestedValues)
    {
      AutoCompleteTextBox autoCompleteTextBox1 = new AutoCompleteTextBox();
      if (!(this.DataContext is string str))
        str = "";
      autoCompleteTextBox1.Text = str;
      AutoCompleteTextBox autoCompleteTextBox2 = autoCompleteTextBox1;
      this.autoCompleteTextBox_0 = autoCompleteTextBox1;
      this.Content = (object) autoCompleteTextBox2;
      this.autoCompleteTextBox_0.TextChanged += new TextChangedEventHandler(this.textBox_0_TextChanged);
      this.textBox_0 = this.autoCompleteTextBox_0.TextBox;
      this.method_0();
    }
    else if ((bool) this.GetValue(ControlProvider.SingleLineView) && this.ReadOnlyBox)
    {
      SimpleTextLine simpleTextLine1 = new SimpleTextLine();
      simpleTextLine1.Text = this.DataContext as string;
      simpleTextLine1.VerticalAlignment = VerticalAlignment.Center;
      simpleTextLine1.Margin = new Thickness(2.0, 0.0, 0.0, 0.0);
      simpleTextLine1.Focusable = false;
      SimpleTextLine simpleTextLine2 = simpleTextLine1;
      this.simpleTextLine_0 = simpleTextLine1;
      this.Content = (object) simpleTextLine2;
    }
    else
    {
      TextBox textBox1 = new TextBox();
      textBox1.TextWrapping = (bool) this.GetValue(ControlProvider.SingleLineView) ? TextWrapping.NoWrap : TextWrapping.Wrap;
      textBox1.AcceptsTab = !this.ReadOnlyBox && this.lines > 1;
      textBox1.Focusable = true;
      textBox1.IsReadOnly = this.ReadOnlyBox;
      TextBox textBox2 = textBox1;
      this.textBox_0 = textBox1;
      this.Content = (object) textBox2;
      this.textBox_0.TextChanged += new TextChangedEventHandler(this.textBox_0_TextChanged);
      this.textBox_0.SelectionChanged += new RoutedEventHandler(this.textBox_0_SelectionChanged);
    }
    if (this.textBox_0 != null)
    {
      this.textBox_0.LostFocus += new RoutedEventHandler(this.textBox_0_LostFocus);
      this.textBox_0.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.textBox_0_GotKeyboardFocus);
      this.textBox_0.PreviewKeyDown += new KeyEventHandler(this.textBox_0_PreviewKeyDown);
      this.textBox_0.VerticalContentAlignment = VerticalAlignment.Top;
      if (this.lines > 1)
      {
        this.textBox_0.TextWrapping = TextWrapping.Wrap;
        this.textBox_0.AcceptsReturn = !this.ReadOnlyBox;
        this.textBox_0.AcceptsTab = !this.ReadOnlyBox;
        this.textBox_0.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        this.textBox_0.Padding = new Thickness(2.0, 2.0, 0.0, 0.0);
      }
      else
      {
        this.textBox_0.TextWrapping = TextWrapping.NoWrap;
        this.textBox_0.VerticalContentAlignment = VerticalAlignment.Center;
        this.textBox_0.VerticalAlignment = VerticalAlignment.Stretch;
        this.textBox_0.Padding = new Thickness(2.0, 0.0, 0.0, 0.0);
      }
      if (this.ReadOnlyBox)
      {
        this.textBox_0.IsReadOnly = true;
        this.textBox_0.Background = (Brush) null;
        this.textBox_0.BorderThickness = new Thickness(0.0);
      }
    }
    if (!(this.DataContext is string string_2))
      string_2 = string.Empty;
    this.method_1(string_2, true);
  }

  private void textBox_0_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this.DataContext == null && this.GetBindingExpression(FrameworkElement.DataContextProperty) == null || this.IsKeyboardFocusWithin)
      return;
    this.method_1(this.DataContext as string, false);
    this.string_1 = (string) null;
    this.int_0 = -1;
  }

  private void textBox_0_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
  {
    if (this.textBox_0 == null || this.string_1 != null)
      return;
    this.string_1 = this.textBox_0.Text;
  }

  private void method_1(string string_2, bool bool_2)
  {
    this.bool_1 = bool_2;
    if (this.textBox_0 != null)
      this.textBox_0.Text = string_2;
    if (this.autoCompleteTextBox_0 != null)
      this.autoCompleteTextBox_0.Text = string_2;
    if (this.simpleTextLine_0 != null)
      this.simpleTextLine_0.Text = string_2;
    this.bool_1 = false;
  }

  private void method_2(object sender, KeyboardFocusChangedEventArgs e)
  {
    if (this.DataContext == null && this.GetBindingExpression(FrameworkElement.DataContextProperty) == null || this.IsKeyboardFocusWithin)
      return;
    this.method_1(this.DataContext as string, false);
    this.string_1 = (string) null;
    this.int_0 = -1;
  }

  private void textBox_0_TextChanged(object sender, TextChangedEventArgs e)
  {
    string text = this.textBox_0.Text;
    if (!this.IsEnabled || this.ReadOnlyBox || this.bool_1)
      return;
    if (this.UpdateSource != null)
      this.UpdateSource(text);
    this.SetCurrentValue(FrameworkElement.DataContextProperty, (object) text);
  }

  private void textBox_0_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Escape)
      return;
    this.method_1(this.string_1, false);
  }

  private void textBox_0_SelectionChanged(object sender, RoutedEventArgs e)
  {
    if (this.int_0 != -1 || !this.textBox_0.IsKeyboardFocused)
      return;
    this.int_0 = this.textBox_0.CaretIndex;
  }

  public void SelectText()
  {
    if (this.textBox_0 == null)
      return;
    this.textBox_0.SelectAll();
  }
}
