// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ControlDropDown
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ControlDropDown : ContentControl
{
  public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(nameof (DropDownContent), typeof (UIElement), typeof (ControlDropDown), new PropertyMetadata((object) null, new PropertyChangedCallback(ControlDropDown.smethod_0)));
  public static readonly DependencyProperty DropDownContentTemplateProperty = DependencyProperty.Register(nameof (DropDownContentTemplate), typeof (DataTemplate), typeof (ControlDropDown));
  public static readonly DependencyProperty ShowGlyphProperty = DependencyProperty.Register(nameof (ShowGlyph), typeof (bool), typeof (ControlDropDown), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (ControlDropDown), new PropertyMetadata((object) false));
  public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register(nameof (PlacementTarget), typeof (UIElement), typeof (ControlDropDown));
  public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(nameof (Placement), typeof (PlacementMode), typeof (ControlDropDown));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof (VerticalOffset), typeof (double), typeof (ControlDropDown));
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof (HorizontalOffset), typeof (double), typeof (ControlDropDown));
  public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof (IsEditable), typeof (bool), typeof (ControlDropDown));
  public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register(nameof (StaysOpen), typeof (bool), typeof (ControlDropDown));
  public static readonly DependencyProperty GlyphInsideProperty = DependencyProperty.Register(nameof (GlyphInside), typeof (bool), typeof (ControlDropDown));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ControlDropDown), new PropertyMetadata((object) null, new PropertyChangedCallback(ControlDropDown.smethod_0)));
  public static readonly DependencyProperty DropDownEnabledProperty = DependencyProperty.Register(nameof (DropDownEnabled), typeof (bool), typeof (ControlDropDown), new PropertyMetadata((object) true));
  public static readonly RoutedEvent OnOpenedProperty = EventManager.RegisterRoutedEvent("OnOpened", RoutingStrategy.Bubble, typeof (RoutedEventArgs), typeof (ControlDropDown));
  private Point point_0 = new Point(0.0, 0.0);
  private bool bool_0;
  private Popup popup_0;
  private ToggleButton toggleButton_0;
  private Button button_0;

  static ControlDropDown()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ControlDropDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ControlDropDown)));
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    ControlDropDown controlDropDown = dependencyObject_0 as ControlDropDown;
    if (dependencyPropertyChangedEventArgs_0.OldValue != null)
      controlDropDown.RemoveLogicalChild(dependencyPropertyChangedEventArgs_0.OldValue);
    if (dependencyPropertyChangedEventArgs_0.NewValue == null)
      return;
    UIElement newValue = dependencyPropertyChangedEventArgs_0.NewValue as UIElement;
    if (newValue.GetParentObject() != null && newValue.GetParentObject() == controlDropDown)
      return;
    controlDropDown.AddLogicalChild(dependencyPropertyChangedEventArgs_0.NewValue);
  }

  protected override IEnumerator LogicalChildren
  {
    get
    {
      ControlDropDown controlDropDown = this;
      if (controlDropDown.DropDownContent != null)
        yield return (object) controlDropDown.DropDownContent;
      if (__nonvirtual (controlDropDown.Content) != null)
        yield return __nonvirtual (controlDropDown.Content);
      if (controlDropDown.Header != null)
        yield return controlDropDown.Header;
    }
  }

  public UIElement DropDownContent
  {
    get => (UIElement) this.GetValue(ControlDropDown.DropDownContentProperty);
    set => this.SetValue(ControlDropDown.DropDownContentProperty, (object) value);
  }

  public DataTemplate DropDownContentTemplate
  {
    get => (DataTemplate) this.GetValue(ControlDropDown.DropDownContentTemplateProperty);
    set => this.SetValue(ControlDropDown.DropDownContentTemplateProperty, (object) value);
  }

  public bool ShowGlyph
  {
    get => (bool) this.GetValue(ControlDropDown.ShowGlyphProperty);
    set => this.SetValue(ControlDropDown.ShowGlyphProperty, (object) value);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(ControlDropDown.IsDropDownOpenProperty);
    set => this.SetValue(ControlDropDown.IsDropDownOpenProperty, (object) value);
  }

  public UIElement PlacementTarget
  {
    get => (UIElement) this.GetValue(ControlDropDown.PlacementTargetProperty);
    set => this.SetValue(ControlDropDown.PlacementTargetProperty, (object) value);
  }

  public PlacementMode Placement
  {
    get => (PlacementMode) this.GetValue(ControlDropDown.PlacementProperty);
    set => this.SetValue(ControlDropDown.PlacementProperty, (object) value);
  }

  public double VerticalOffset
  {
    get => (double) this.GetValue(ControlDropDown.VerticalOffsetProperty);
    set => this.SetValue(ControlDropDown.VerticalOffsetProperty, (object) value);
  }

  public double HorizontalOffset
  {
    get => (double) this.GetValue(ControlDropDown.HorizontalOffsetProperty);
    set => this.SetValue(ControlDropDown.HorizontalOffsetProperty, (object) value);
  }

  public bool IsEditable
  {
    get => (bool) this.GetValue(ControlDropDown.IsEditableProperty);
    set => this.SetValue(ControlDropDown.IsEditableProperty, (object) value);
  }

  public bool StaysOpen
  {
    get => (bool) this.GetValue(ControlDropDown.StaysOpenProperty);
    set => this.SetValue(ControlDropDown.StaysOpenProperty, (object) value);
  }

  public bool GlyphInside
  {
    get => (bool) this.GetValue(ControlDropDown.GlyphInsideProperty);
    set => this.SetValue(ControlDropDown.GlyphInsideProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(ControlDropDown.HeaderProperty);
    set => this.SetValue(ControlDropDown.HeaderProperty, value);
  }

  public bool DropDownEnabled
  {
    get => (bool) this.GetValue(ControlDropDown.DropDownEnabledProperty);
    set => this.SetValue(ControlDropDown.DropDownEnabledProperty, (object) value);
  }

  public event RoutedEventHandler OnOpened
  {
    add => this.AddHandler(ControlDropDown.OnOpenedProperty, (Delegate) value);
    remove => this.RemoveHandler(ControlDropDown.OnOpenedProperty, (Delegate) value);
  }

  public event RoutedEventHandler GlyphClicked;

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != ControlDropDown.IsDropDownOpenProperty)
      return;
    if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
      RenderDispatch.Rendering += new EventHandler<EventArgs>(this.method_0);
    else
      RenderDispatch.Rendering -= new EventHandler<EventArgs>(this.method_0);
  }

  private void method_0(object sender, EventArgs e)
  {
    if (!this.IsLoaded || this.DropDownContent == null || this.popup_0 == null)
      return;
    Point screen = this.PointToScreen(new Point(0.0, 0.0));
    if (!(screen != this.point_0))
      return;
    this.point_0 = screen;
    double verticalOffset = this.popup_0.VerticalOffset;
    this.popup_0.VerticalOffset += 0.01;
    this.popup_0.VerticalOffset = verticalOffset;
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs mouseButtonEventArgs_0)
  {
    base.OnPreviewMouseLeftButtonDown(mouseButtonEventArgs_0);
    this.bool_0 = !this.StaysOpen && this.IsDropDownOpen;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.popup_0 != null)
      this.popup_0.Closed -= new EventHandler(this.popup_0_Closed);
    if (this.toggleButton_0 != null)
    {
      this.toggleButton_0.PreviewMouseUp -= new MouseButtonEventHandler(this.toggleButton_0_PreviewMouseUp);
      this.button_0.Click -= new RoutedEventHandler(this.button_0_Click);
    }
    try
    {
      this.popup_0 = (Popup) this.GetTemplateChild("DropDownPopup");
      this.toggleButton_0 = (ToggleButton) this.GetTemplateChild("DropDownButton");
      this.button_0 = (Button) this.GetTemplateChild("DropDownButton2");
      if (this.popup_0 == null || this.toggleButton_0 == null || this.button_0 == null)
        return;
      this.button_0.Click += new RoutedEventHandler(this.button_0_Click);
      this.toggleButton_0.PreviewMouseUp += new MouseButtonEventHandler(this.toggleButton_0_PreviewMouseUp);
      this.popup_0.Closed += new EventHandler(this.popup_0_Closed);
    }
    catch (Exception ex)
    {
    }
  }

  private void button_0_Click(object sender, RoutedEventArgs e)
  {
    if (this.bool_0)
    {
      e.Handled = true;
    }
    else
    {
      if (!this.IsDropDownOpen)
      {
        // ISSUE: reference to a compiler-generated field
        RoutedEventHandler routedEventHandler0 = this.routedEventHandler_0;
        if (routedEventHandler0 != null)
          routedEventHandler0((object) this, e);
      }
      if (e.Handled)
        return;
      this.SetCurrentValue(ControlDropDown.IsDropDownOpenProperty, (object) !this.IsDropDownOpen);
    }
  }

  private void toggleButton_0_PreviewMouseUp(object sender1, MouseButtonEventArgs e1)
  {
    NotOnTopPopup notOnTopPopup = this.GetVisualChildren().OfType<NotOnTopPopup>().FirstOrDefault<NotOnTopPopup>();
    if (notOnTopPopup == null)
      return;
    notOnTopPopup.Opened += (EventHandler) ((sender2, e2) => this.RaiseEvent(new RoutedEventArgs(ControlDropDown.OnOpenedProperty)));
  }

  private void popup_0_Closed(object sender, EventArgs e)
  {
    GuiHelpers.GuiInvokeAsync((Action) (() => this.IsDropDownOpen = false));
  }
}
