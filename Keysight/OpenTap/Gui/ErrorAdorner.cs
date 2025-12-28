// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ErrorAdorner
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ErrorAdorner : Adorner
{
  private ErrorAdornerView errorAdornerView_0;
  private const int int_0 = 20;
  private bool bool_0;

  public ErrorAdorner(UIElement elem)
    : base(elem)
  {
    if (!(elem is FrameworkElement frameworkElement))
      return;
    ErrorAdornerView errorAdornerView = new ErrorAdornerView();
    errorAdornerView.DataContext = frameworkElement.DataContext;
    this.errorAdornerView_0 = errorAdornerView;
    this.AddLogicalChild((object) this.errorAdornerView_0);
    this.AddVisualChild((Visual) this.errorAdornerView_0);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    return new Size(20.0, this.AdornedElement.DesiredSize.Height);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ErrorAdorner.Class74 class74 = new ErrorAdorner.Class74();
    // ISSUE: reference to a compiler-generated field
    class74.errorAdorner_0 = this;
    Rect rect = new Rect(this.AdornedElement.RenderSize);
    if (this.AdornedElement is DataGridRow adornedElement)
    {
      // ISSUE: reference to a compiler-generated field
      class74.scrollViewer_0 = GuiHelpers.TryFindParent<ScrollViewer>((DependencyObject) adornedElement);
      // ISSUE: reference to a compiler-generated field
      if (class74.scrollViewer_0 != null)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (class74.scrollViewer_0.ComputedVerticalScrollBarVisibility != System.Windows.Visibility.Collapsed)
          num = 20;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        rect.Width = class74.scrollViewer_0.ActualWidth - (double) num + class74.scrollViewer_0.HorizontalOffset;
        if (!this.bool_0)
        {
          // ISSUE: reference to a compiler-generated method
          this.Loaded += new RoutedEventHandler(class74.method_0);
          // ISSUE: reference to a compiler-generated method
          this.Unloaded += new RoutedEventHandler(class74.method_1);
          this.bool_0 = true;
        }
      }
    }
    double x = rect.Right - 20.0;
    double top = rect.Top;
    double bottom = rect.Bottom;
    this.errorAdornerView_0.Arrange(new Rect(x, 0.0, finalSize.Width, finalSize.Height));
    return this.errorAdornerView_0.RenderSize;
  }

  private void method_0(object sender, ScrollChangedEventArgs e) => this.InvalidateArrange();

  private void method_1(object sender, SizeChangedEventArgs e) => this.InvalidateArrange();

  protected override Visual GetVisualChild(int index) => (Visual) this.errorAdornerView_0;

  protected override int VisualChildrenCount => 1;

  protected override IEnumerator LogicalChildren
  {
    get
    {
      if (this.errorAdornerView_0 != null)
        yield return (object) this.errorAdornerView_0;
    }
  }
}
