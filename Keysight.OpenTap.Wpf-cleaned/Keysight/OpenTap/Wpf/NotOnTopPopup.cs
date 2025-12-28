// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NotOnTopPopup
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class NotOnTopPopup : Popup
{
  public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(typeof (NotOnTopPopup), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(NotOnTopPopup.smethod_0)));
  public static DependencyProperty OpenLeftOfProperty = DependencyProperty.Register(nameof (OpenLeftOf), typeof (bool), typeof (NotOnTopPopup));
  private FrameworkElement frameworkElement_0;
  private Window window_0;
  private const int int_0 = 16 /*0x10*/;

  public bool Topmost
  {
    get => (bool) this.GetValue(NotOnTopPopup.TopmostProperty);
    set => this.SetValue(NotOnTopPopup.TopmostProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as NotOnTopPopup).method_1();
  }

  public bool OpenLeftOf
  {
    get => (bool) this.GetValue(NotOnTopPopup.OpenLeftOfProperty);
    set => this.SetValue(NotOnTopPopup.OpenLeftOfProperty, (object) value);
  }

  public static CustomPopupPlacement[] burgerMenu_placementCallback(
    Size popupSize,
    Size targetSize,
    Point offset)
  {
    return new CustomPopupPlacement[1]
    {
      new CustomPopupPlacement(new Point(-22.0, 0.0), PopupPrimaryAxis.Vertical)
    };
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != NotOnTopPopup.OpenLeftOfProperty || !(bool) dependencyPropertyChangedEventArgs_0.NewValue)
      return;
    this.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(NotOnTopPopup.burgerMenu_placementCallback);
  }

  protected override void OnOpened(EventArgs eventArgs_0)
  {
    this.method_1();
    this.frameworkElement_0 = this.Child as FrameworkElement;
    this.window_0 = Window.GetWindow((DependencyObject) this);
    if (this.frameworkElement_0 != null && this.window_0 != null)
      this.Child.PreviewMouseDown += new MouseButtonEventHandler(this.method_0);
    base.OnOpened(eventArgs_0);
  }

  protected override void OnClosed(EventArgs eventArgs_0)
  {
    base.OnClosed(eventArgs_0);
    this.window_0 = (Window) null;
    FrameworkElement frameworkElement0 = this.frameworkElement_0;
    if (frameworkElement0 != null)
      frameworkElement0.PreviewMouseDown -= new MouseButtonEventHandler(this.method_0);
    this.frameworkElement_0 = (FrameworkElement) null;
  }

  private void method_0(object sender, MouseButtonEventArgs e)
  {
    if (this.IsKeyboardFocusWithin)
      return;
    this.window_0.Activate();
  }

  private void method_1()
  {
    IntPtr handle = ((HwndSource) PresentationSource.FromVisual((Visual) this.Child)).Handle;
    NotOnTopPopup.RECT rect_0;
    if (!NotOnTopPopup.GetWindowRect(handle, out rect_0))
      return;
    NotOnTopPopup.SetWindowPos(handle, this.Topmost ? -1 : -2, rect_0.Left, rect_0.Top, (int) this.Width, (int) this.Height, 16 /*0x10*/);
  }

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetWindowRect(IntPtr intptr_0, out NotOnTopPopup.RECT rect_0);

  [DllImport("user32")]
  private static extern int SetWindowPos(
    IntPtr intptr_0,
    int int_1,
    int int_2,
    int int_3,
    int int_4,
    int int_5,
    int int_6);

  public struct RECT
  {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
  }
}
