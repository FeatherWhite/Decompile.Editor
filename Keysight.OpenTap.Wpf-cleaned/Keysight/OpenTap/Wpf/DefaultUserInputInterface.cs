// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.DefaultUserInputInterface
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class DefaultUserInputInterface : IUserInterface, IUserInputInterface
{
  private readonly NotifyQueue notifyQueue_0 = new NotifyQueue((Action) (() => UpdateMonitor.Update((object) null, (AnnotationCollection) null)));

  public event EventHandler OnAbortRequested;

  public Window Owner { get; set; }

  public DefaultUserInputInterface(Window owner) => this.Owner = owner;

  public void NotifyChanged(object object_0, string property)
  {
    this.notifyQueue_0.NotifyChanged(object_0, property);
  }

  public void RequestUserInput(object dataObject, TimeSpan timeout, bool modal)
  {
    CancellationToken sourceToken = TapThread.Current.AbortToken;
    bool done = false;
    ManualResetEventSlim inputDialogExited = new ManualResetEventSlim(false);
    Stopwatch stopwatch_0 = Stopwatch.StartNew();
    GuiHelpers.GuiInvokeAsync((Action) (() =>
    {
      Window window_0;
      if (dataObject is IEnumerable)
        window_0 = (Window) new PropGridListEditorWindow(dataObject)
        {
          Owner = this.Owner,
          WindowStartupLocation = WindowStartupLocation.CenterOwner,
          HasCloseButton = true,
          SizeToContent = SizeToContent.WidthAndHeight
        };
      else
        window_0 = (Window) new PropGridWindow(dataObject)
        {
          Owner = this.Owner,
          WindowStartupLocation = WindowStartupLocation.CenterOwner,
          HasCloseButton = false,
          SizeToContent = SizeToContent.WidthAndHeight
        };
      window_0.Loaded += (RoutedEventHandler) ((sender, e) =>
      {
        if (this.Owner.WindowState != WindowState.Maximized)
          return;
        DpiScale dpi = VisualTreeHelper.GetDpi((Visual) this.Owner);
        Point screen3 = this.Owner.PointToScreen(new Point(0.0, 0.0));
        screen3.X /= dpi.DpiScaleX;
        screen3.Y /= dpi.DpiScaleY;
        Point screen4 = window_0.PointToScreen(new Point(0.0, 0.0));
        screen4.X /= dpi.DpiScaleX;
        screen4.Y /= dpi.DpiScaleY;
        Point point3 = new Point(screen4.X - screen3.X, screen4.Y - screen3.Y);
        double num3 = this.Owner.ActualWidth / 2.0 - window_0.ActualWidth / 2.0;
        double num4 = this.Owner.ActualHeight / 2.0 - window_0.ActualHeight / 2.0;
        Point point4 = new Point(num3 - point3.X, num4 - point3.Y);
        window_0.Top += point4.Y;
        window_0.Left += point4.X;
      });
      window_0.Closed += (EventHandler) ((sender, e) => done = true);
      window_0.KeyDown += new KeyEventHandler(this.PlatformInteractionDialog_KeyDown);
      // ISSUE: variable of a compiler-generated type
      DefaultUserInputInterface.\u003C\u003Ec__DisplayClass10_1 cDisplayClass101;
      window_0.Loaded += (RoutedEventHandler) ((sender, e) =>
      {
        PropGridWindow propGridWindow_0 = window_0 as PropGridWindow;
        // ISSUE: reference to a compiler-generated method
        if (propGridWindow_0 == null || this.method_0(propGridWindow_0.propgrid))
          return;
        // ISSUE: reference to a compiler-generated method
        propGridWindow_0.propgrid.OnNextEvent(PropGrid.ViewRefreshedEvent, (RoutedEventHandler) ((sender2, e2) => cDisplayClass101.method_0(propGridWindow_0.propgrid)));
      });
      TapThread.Start((Action) (() =>
      {
        try
        {
          while (!done && stopwatch_0.Elapsed < timeout && !sourceToken.IsCancellationRequested)
            TapThread.Sleep(50);
        }
        finally
        {
          GuiHelpers.GuiInvokeAsync((Action) (() =>
          {
            if (!window_0.IsLoaded)
              return;
            window_0.Close();
          }));
        }
      }), "");
      if (modal)
      {
        window_0.ShowDialog();
      }
      else
      {
        NativeMouseCapture.ReleaseAnyCapture();
        window_0.Show();
        while (!done)
          GuiHelpers.PushDispatcherFrame();
      }
      inputDialogExited.Set();
    }));
    bool flag = Application.Current.Dispatcher.CheckAccess();
    while (!inputDialogExited.Wait(0) && stopwatch_0.Elapsed < timeout)
    {
      if (flag)
        GuiHelpers.PushDispatcherFrame();
      else
        TapThread.Sleep(10);
    }
    if (!inputDialogExited.Wait(0))
    {
      done = true;
      throw new TimeoutException("User input timed out");
    }
  }

  public void PlatformInteractionDialog_KeyDown(object sender, KeyEventArgs e)
  {
    if (!e.KeyboardDevice.Modifiers.HasFlag((Enum) ModifierKeys.Shift) || e.Key != Key.F5)
      return;
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler0 = this.eventHandler_0;
    if (eventHandler0 == null)
      return;
    eventHandler0((object) this, (EventArgs) e);
  }
}
