// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.InteractiveBackground
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class InteractiveBackground : UserControl, IComponentConnector
{
  private const int int_0 = 20;
  private const double double_0 = 7.0;
  private const double double_1 = 300.0;
  public List<Ellipse> ellipses = new List<Ellipse>();
  private Random random_0 = new Random();
  private Timer timer_0;
  private Stopwatch stopwatch_0 = Stopwatch.StartNew();
  private Dictionary<InteractiveBackground.LineItem, Line> dictionary_2 = new Dictionary<InteractiveBackground.LineItem, Line>();
  internal ItemsControl canvas;
  private bool bool_0;

  public InteractiveBackground()
  {
    this.position = new Dictionary<Shape, Vector>();
    this.velocity = new Dictionary<Shape, Vector>();
    this.stuff = new ObservableCollection<Shape>();
    this.InitializeComponent();
    this.DataContext = (object) this;
    this.Loaded += new RoutedEventHandler(this.InteractiveBackground_Loaded);
    this.timer_0 = new Timer() { Interval = 33.0 };
    this.timer_0.Elapsed += new ElapsedEventHandler(this.timer_0_Elapsed);
    this.timer_0.AutoReset = true;
  }

  private void InteractiveBackground_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.IsVisible)
      this.timer_0.Start();
    Ellipse ellipse = new Ellipse();
    Canvas.SetLeft((UIElement) ellipse, 10.0);
    Canvas.SetTop((UIElement) ellipse, 40.0);
    this.stuff.Add((Shape) ellipse);
    this.position[(Shape) ellipse] = new Vector(0.0, 0.0);
    this.ellipses.Add(ellipse);
    foreach (int num in Enumerable.Range(0, 20))
      this.method_0();
    for (int index = 0; index < 100; ++index)
      this.method_1(10.0);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != UIElement.IsVisibleProperty)
      return;
    if ((bool) dependencyPropertyChangedEventArgs_0.NewValue)
      this.timer_0.Start();
    else
      this.timer_0.Stop();
  }

  public ObservableCollection<Shape> stuff { get; set; }

  public Dictionary<Shape, Vector> position { get; set; }

  public Dictionary<Shape, Vector> velocity { get; set; }

  private void method_0()
  {
    Ellipse key = new Ellipse();
    this.stuff.Add((Shape) key);
    this.position[(Shape) key] = new Vector(0.0, 0.0);
    double num = this.random_0.NextDouble() * Math.PI * 2.0;
    this.velocity[(Shape) key] = new Vector(Math.Cos(num), Math.Sin(num)) * 7.0;
    this.ellipses.Add(key);
  }

  private void method_1(double double_2)
  {
    double actualWidth = this.canvas.ActualWidth;
    double actualHeight = this.canvas.ActualHeight;
    foreach (Ellipse ellipse in this.stuff.OfType<Ellipse>())
    {
      if (this.velocity.ContainsKey((Shape) ellipse))
      {
        Vector vector1 = this.position[(Shape) ellipse];
        Vector vector2 = this.velocity[(Shape) ellipse];
        Vector vector3 = vector1 + vector2 * double_2;
        if (vector3.X < 0.0 || vector3.X >= actualWidth)
        {
          vector2 = new Vector(vector2.X * -1.0, vector2.Y);
          vector3.X = Math.Max(0.0, Math.Min(vector3.X, actualWidth));
        }
        if (vector3.Y < 0.0 || vector3.Y > actualHeight)
        {
          vector2 = new Vector(vector2.X, vector2.Y * -1.0);
          vector3.Y = Math.Max(0.0, Math.Min(vector3.Y, actualHeight));
        }
        Canvas.SetLeft((UIElement) ellipse, vector3.X);
        Canvas.SetTop((UIElement) ellipse, vector3.Y);
        this.position[(Shape) ellipse] = vector3;
        this.velocity[(Shape) ellipse] = vector2;
      }
    }
  }

  private void timer_0_Elapsed(object sender, ElapsedEventArgs e)
  {
    double delta = this.stopwatch_0.Elapsed.TotalSeconds;
    this.stopwatch_0.Restart();
    this.Dispatcher.Invoke((Action) (() =>
    {
      this.method_1(delta);
      this.method_2();
    }));
  }

  private void method_2()
  {
    Vector vector1;
    for (int index1 = 0; index1 < this.ellipses.Count - 1; ++index1)
    {
      Ellipse ellipsis1 = this.ellipses[index1];
      Vector vector2 = this.position[(Shape) ellipsis1];
      for (int index2 = index1 + 1; index2 < this.ellipses.Count; ++index2)
      {
        Ellipse ellipsis2 = this.ellipses[index2];
        InteractiveBackground.LineItem key = new InteractiveBackground.LineItem()
        {
          ellipse_0 = ellipsis1,
          ellipse_1 = ellipsis2
        };
        Vector vector3 = this.position[(Shape) ellipsis2];
        vector1 = vector2 - vector3;
        if (vector1.Length < 300.0)
        {
          if (!this.dictionary_2.ContainsKey(key))
          {
            this.dictionary_2.Add(key, new Line());
            this.stuff.Add((Shape) this.dictionary_2[key]);
          }
        }
        else if (this.dictionary_2.ContainsKey(key))
        {
          this.stuff.Remove((Shape) this.dictionary_2[key]);
          this.dictionary_2.Remove(key);
        }
      }
    }
    foreach (KeyValuePair<InteractiveBackground.LineItem, Line> keyValuePair in this.dictionary_2)
    {
      Vector vector4 = this.position[(Shape) keyValuePair.Key.ellipse_0];
      Vector vector5 = this.position[(Shape) keyValuePair.Key.ellipse_1];
      vector1 = vector4 - vector5;
      double length = vector1.Length;
      keyValuePair.Value.Opacity = 1.0 - length / 300.0;
      Line line = keyValuePair.Value;
      line.X1 = vector4.X;
      line.Y1 = vector4.Y;
      line.X2 = vector5.X;
      line.Y2 = vector5.Y;
    }
  }

  private void canvas_MouseMove(object sender, MouseEventArgs e)
  {
    Dictionary<Shape, Vector> position1 = this.position;
    Shape key = this.stuff[0];
    Point position2 = e.GetPosition((IInputElement) this);
    double x = position2.X;
    position2 = e.GetPosition((IInputElement) this);
    double y = position2.Y;
    Vector vector = new Vector(x, y);
    position1[key] = vector;
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/interactivebackground.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      this.canvas = (ItemsControl) target;
      this.canvas.MouseMove += new MouseEventHandler(this.canvas_MouseMove);
    }
    else
      this.bool_0 = true;
  }

  private struct LineItem
  {
    public Ellipse ellipse_0;
    public Ellipse ellipse_1;
  }
}
