// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GridSplitter2
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class GridSplitter2 : GridSplitter
{
  public static readonly DependencyProperty PersistingIdProperty = DependencyProperty.Register(nameof (PersistingId), typeof (string), typeof (GridSplitter2));
  private const double double_0 = 2.0;

  static GridSplitter2()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (GridSplitter2), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (GridSplitter2)));
  }

  public string PersistingId
  {
    get => (string) this.GetValue(GridSplitter2.PersistingIdProperty);
    set => this.SetValue(GridSplitter2.PersistingIdProperty, (object) value);
  }

  public GridSplitter2()
  {
    this.Loaded += new RoutedEventHandler(this.GridSplitter2_Loaded);
    this.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
  }

  private bool isColumnSplitter => this.ActualHeight > this.ActualWidth;

  private void GridSplitter2_Loaded(object sender, RoutedEventArgs e)
  {
    this.DragCompleted += new DragCompletedEventHandler(this.GridSplitter2_DragCompleted);
    Grid parent1 = this.Parent as Grid;
    parent1.SizeChanged += new SizeChangedEventHandler(this.method_0);
    FrameworkElement parent2 = parent1.Parent as FrameworkElement;
    parent2.SizeChanged += new SizeChangedEventHandler(this.method_0);
    if (this.PersistingId == null || parent1 == null)
      return;
    double num1 = 1.0;
    if (!ComponentSettings<GuiControlsSettings>.Current.GridSplitterRatios.TryGetValue(this.PersistingId, out num1) || num1 <= 0.0)
      return;
    if (this.isColumnSplitter)
    {
      int columnIndex = Grid.GetColumn((UIElement) this);
      ColumnDefinition[] array = parent1.ColumnDefinitions.Where<ColumnDefinition>((Func<ColumnDefinition, int, bool>) ((columnDefinition_0, int_0) => int_0 == columnIndex - 1 || int_0 == columnIndex + 1)).ToArray<ColumnDefinition>();
      if (array.Length != 2)
        return;
      double num2 = ((IEnumerable<ColumnDefinition>) array).Sum<ColumnDefinition>((Func<ColumnDefinition, double>) (columnDefinition_0 => columnDefinition_0.ActualWidth));
      for (int index = 0; index < 3; ++index)
      {
        double num3 = array[0].ActualWidth / num2;
        double num4 = Math.Pow(num1 / num3, 2.0);
        array[0].Width = new GridLength(array[0].Width.Value * num4, GridUnitType.Star);
        parent2.UpdateLayout();
      }
    }
    else
    {
      int rowIndex = Grid.GetRow((UIElement) this);
      RowDefinition[] array = parent1.RowDefinitions.Where<RowDefinition>((Func<RowDefinition, int, bool>) ((rowDefinition_0, int_0) => int_0 == rowIndex - 1 || int_0 == rowIndex + 1)).ToArray<RowDefinition>();
      if (array.Length != 2)
        return;
      double num5 = ((IEnumerable<RowDefinition>) array).Sum<RowDefinition>((Func<RowDefinition, double>) (rowDefinition_0 => rowDefinition_0.ActualHeight));
      for (int index = 0; index < 3; ++index)
      {
        double num6 = array[0].ActualHeight / num5;
        double num7 = Math.Pow(num1 / num6, 2.0);
        array[0].Height = new GridLength(array[0].Height.Value * num7, GridUnitType.Star);
        parent2.UpdateLayout();
      }
    }
  }

  private void GridSplitter2_DragCompleted(object sender, DragCompletedEventArgs e)
  {
    if (!(this.Parent is Grid parent) || this.PersistingId == null)
      return;
    double num1;
    if (this.isColumnSplitter)
    {
      int columnIndex = Grid.GetColumn((UIElement) this);
      ColumnDefinition[] array = parent.ColumnDefinitions.Where<ColumnDefinition>((Func<ColumnDefinition, int, bool>) ((columnDefinition_0, int_0) => int_0 == columnIndex - 1 || int_0 == columnIndex + 1)).ToArray<ColumnDefinition>();
      if (array.Length != 2)
        return;
      double num2 = ((IEnumerable<ColumnDefinition>) array).Sum<ColumnDefinition>((Func<ColumnDefinition, double>) (columnDefinition_0 => columnDefinition_0.ActualWidth));
      num1 = array[0].ActualWidth / num2;
    }
    else
    {
      int rowIndex = Grid.GetRow((UIElement) this);
      RowDefinition[] array = parent.RowDefinitions.Where<RowDefinition>((Func<RowDefinition, int, bool>) ((rowDefinition_0, int_0) => int_0 == rowIndex - 1 || int_0 == rowIndex + 1)).ToArray<RowDefinition>();
      if (array.Length != 2)
        return;
      double num3 = ((IEnumerable<RowDefinition>) array).Sum<RowDefinition>((Func<RowDefinition, double>) (rowDefinition_0 => rowDefinition_0.ActualHeight));
      num1 = array[0].ActualHeight / num3;
    }
    ComponentSettings<GuiControlsSettings>.Current.GridSplitterRatios[this.PersistingId] = num1;
  }

  private void method_0(object sender, SizeChangedEventArgs e)
  {
    Grid parent1 = this.Parent as Grid;
    FrameworkElement parent2 = parent1.Parent as FrameworkElement;
    if (!parent2.IsVisible || !parent1.IsVisible)
      return;
    for (int index = 0; index < 2; ++index)
    {
      IEnumerable<ColumnDefinition> source = parent1.ColumnDefinitions.Where<ColumnDefinition>((Func<ColumnDefinition, bool>) (columnDefinition_0 => columnDefinition_0.Width.IsStar));
      double num1 = source.Sum<ColumnDefinition>((Func<ColumnDefinition, double>) (columnDefinition_0 => columnDefinition_0.ActualWidth));
      foreach (ColumnDefinition columnDefinition in source)
        columnDefinition.Width = new GridLength(columnDefinition.ActualWidth / num1, GridUnitType.Star);
      int columnIndex = Grid.GetColumn((UIElement) this);
      ColumnDefinition[] array = parent1.ColumnDefinitions.Where<ColumnDefinition>((Func<ColumnDefinition, int, bool>) ((columnDefinition_0, int_0) => int_0 == columnIndex - 1 || int_0 == columnIndex + 1)).ToArray<ColumnDefinition>();
      if (array.Length == 2 && parent1.ColumnDefinitions.Any<ColumnDefinition>((Func<ColumnDefinition, bool>) (columnDefinition_0 => columnDefinition_0.MinWidth >= columnDefinition_0.ActualWidth && columnDefinition_0.MinWidth > 0.0)))
      {
        Point point = parent1.TranslatePoint(new Point(0.0, 0.0), (UIElement) parent2);
        double num2 = parent1.ActualWidth + point.X - parent2.ActualWidth;
        if (num2 > 0.0)
        {
          ColumnDefinition max = ((IEnumerable<ColumnDefinition>) array).FindMax<ColumnDefinition, double>((Func<ColumnDefinition, double>) (columnDefinition_0 => columnDefinition_0.ActualWidth - columnDefinition_0.MinWidth));
          double num3 = Math.Pow((max.ActualWidth - num2) / max.ActualWidth, 2.0);
          if (num3 < 0.5)
            num3 = 0.5;
          max.Width = new GridLength(max.Width.Value * num3, GridUnitType.Star);
          parent2.UpdateLayout();
        }
        else
          break;
      }
      else
        break;
    }
    for (int index = 0; index < 2; ++index)
    {
      IEnumerable<RowDefinition> source = parent1.RowDefinitions.Where<RowDefinition>((Func<RowDefinition, bool>) (rowDefinition_0 => rowDefinition_0.Height.IsStar));
      double num4 = source.Sum<RowDefinition>((Func<RowDefinition, double>) (rowDefinition_0 => rowDefinition_0.ActualHeight));
      foreach (RowDefinition rowDefinition in source)
        rowDefinition.Height = new GridLength(rowDefinition.ActualHeight / num4, GridUnitType.Star);
      int rowIndex = Grid.GetRow((UIElement) this);
      RowDefinition[] array = parent1.RowDefinitions.Where<RowDefinition>((Func<RowDefinition, int, bool>) ((rowDefinition_0, int_0) => int_0 == rowIndex - 1 || int_0 == rowIndex + 1)).ToArray<RowDefinition>();
      if (array.Length != 2 || !parent1.RowDefinitions.Any<RowDefinition>((Func<RowDefinition, bool>) (rowDefinition_0 => rowDefinition_0.MinHeight >= rowDefinition_0.ActualHeight && rowDefinition_0.MinHeight > 0.0)))
        break;
      Point point = parent1.TranslatePoint(new Point(0.0, 0.0), (UIElement) parent2);
      double num5 = parent1.ActualHeight + point.Y - parent2.ActualHeight;
      if (num5 <= 0.0)
        break;
      RowDefinition max = ((IEnumerable<RowDefinition>) array).FindMax<RowDefinition, double>((Func<RowDefinition, double>) (rowDefinition_0 => rowDefinition_0.ActualHeight - rowDefinition_0.MinHeight));
      double num6 = Math.Pow((max.ActualHeight - num5) / max.ActualHeight, 2.0);
      if (num6 < 0.5)
        num6 = 0.5;
      max.Height = new GridLength(max.Height.Value * num6, GridUnitType.Star);
      parent2.UpdateLayout();
    }
  }
}
