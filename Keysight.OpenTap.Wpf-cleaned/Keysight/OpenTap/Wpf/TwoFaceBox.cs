// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TwoFaceBox
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class TwoFaceBox : Control
{
  public static readonly DependencyProperty FocusedTextProperty = DependencyProperty.Register(nameof (FocusedText), typeof (object), typeof (TwoFaceBox), new PropertyMetadata((object) null, new PropertyChangedCallback(TwoFaceBox.smethod_0)));
  public static readonly DependencyProperty IsFocusedTextFocusedProperty = DependencyProperty.Register(nameof (IsFocusedTextFocused), typeof (bool), typeof (TwoFaceBox));
  public static readonly DependencyProperty UnfocusedContentProperty = DependencyProperty.Register(nameof (UnfocusedContent), typeof (UIElement), typeof (TwoFaceBox));
  private TextBox textBox_0;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    TwoFaceBox twoFaceBox = dependencyObject_0 as TwoFaceBox;
    if (dependencyPropertyChangedEventArgs_0.OldValue != null)
      twoFaceBox.RemoveLogicalChild(dependencyPropertyChangedEventArgs_0.OldValue);
    if (dependencyPropertyChangedEventArgs_0.NewValue == null)
      return;
    twoFaceBox.AddLogicalChild(dependencyPropertyChangedEventArgs_0.NewValue);
  }

  protected override IEnumerator LogicalChildren
  {
    get
    {
      yield return this.FocusedText;
      yield return (object) this.UnfocusedContent;
    }
  }

  public object FocusedText
  {
    get => this.GetValue(TwoFaceBox.FocusedTextProperty);
    set => this.SetValue(TwoFaceBox.FocusedTextProperty, value);
  }

  public bool IsFocusedTextFocused
  {
    get => (bool) this.GetValue(TwoFaceBox.IsFocusedTextFocusedProperty);
    set => this.SetValue(TwoFaceBox.IsFocusedTextFocusedProperty, (object) value);
  }

  public UIElement UnfocusedContent
  {
    get => (UIElement) this.GetValue(TwoFaceBox.UnfocusedContentProperty);
    set => this.SetValue(TwoFaceBox.UnfocusedContentProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.textBox_0 = (TextBox) this.GetTemplateChild("PART_textbox");
    this.textBox_0.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.textBox_0_LostKeyboardFocus);
    this.textBox_0.KeyDown += new KeyEventHandler(this.textBox_0_KeyDown);
    this.textBox_0.ContextMenu = (ContextMenu) null;
    (this.GetTemplateChild("PART_contentContainer") as ContentContainer).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.method_0);
  }

  private void textBox_0_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    this.textBox_0.GetBindingExpression(TextBox.TextProperty).UpdateSource();
    FocusManager.SetFocusedElement((DependencyObject) this.textBox_0, (IInputElement) null);
    Window.GetWindow((DependencyObject) this.textBox_0)?.Focus();
  }

  private void textBox_0_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
  {
    PropGridListEditor parent = (sender as TextBox).FindParent<PropGridListEditor>();
    if (parent != null && parent.ContextMenu.IsOpen)
      return;
    this.IsFocusedTextFocused = false;
  }

  private void method_0(object sender, MouseButtonEventArgs e)
  {
    this.IsFocusedTextFocused = true;
    e.Handled = true;
    this.textBox_0.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.textBox_0_GotKeyboardFocus);
    this.textBox_0.Focus();
    Keyboard.Focus((IInputElement) this.textBox_0);
  }

  private void textBox_0_GotKeyboardFocus(object sender, EventArgs e)
  {
    this.textBox_0.LostMouseCapture -= new MouseEventHandler(this.textBox_0_GotKeyboardFocus);
    this.textBox_0.SelectAll();
  }
}
