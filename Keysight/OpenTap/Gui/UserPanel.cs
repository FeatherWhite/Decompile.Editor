// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.UserPanel
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class UserPanel : ValidatingObject
{
  private readonly WeakReference<object> content = new WeakReference<object>((object) null);
  internal ITapDockPanel Panel;
  private readonly ITypeData panelType;

  public string Title { get; set; }

  public string ToolTip { get; set; }

  public string ContentId { get; set; }

  public string IconSource { get; set; }

  public string HelpLink { get; set; }

  public double? DesiredWidth
  {
    get
    {
      ITapDockPanel panel = this.Panel;
      return panel == null ? new double?() : (double?) panel.DesiredWidth;
    }
  }

  public double? DesiredHeight
  {
    get
    {
      ITapDockPanel panel = this.Panel;
      return panel == null ? new double?() : (double?) panel.DesiredHeight;
    }
  }

  public GuiContext Context { get; set; }

  public MainWindow LegacyContext { get; set; }

  public object Content
  {
    get
    {
      object target;
      if (this.content.TryGetTarget(out target))
        return target;
      Decorator decorator_0 = new Decorator()
      {
        Child = (UIElement) new ContentPresenter()
        {
          Content = (object) new IndeterminateProgress()
          {
            Message = "Checking License..."
          }
        }
      };
      TapThread.Start((Action) (() =>
      {
        try
        {
          if (this.Panel == null)
          {
            try
            {
              Type type = this.panelType.smethod_0();
              if (!(type == typeof (LogPanelPlugin)) && !(type == typeof (StepSettingsPlugin)) && !(type == typeof (TestPlanSettings)) && !(type == typeof (StepExplorerPlugin)) && !(type == typeof (TestPlanPlugin)))
                Class79.smethod_1();
              this.Panel = (ITapDockPanel) this.panelType.CreateInstance();
            }
            catch (TargetInvocationException ex)
            {
              throw ex.smethod_29();
            }
          }
          GuiHelpers.GuiInvoke((Action) (() =>
          {
            try
            {
              decorator_0.Child = (UIElement) this.Panel.CreateElement((ITapDockContext) this.LegacyContext);
            }
            catch (Exception ex)
            {
              decorator_0.Child = (UIElement) new ContentPresenter()
              {
                Content = (object) ex
              };
            }
          }));
        }
        catch (Exception ex)
        {
          GuiHelpers.GuiInvoke((Action) (() => decorator_0.Child = (UIElement) new ContentPresenter()
          {
            Content = (object) ex
          }));
        }
      }));
      this.content.SetTarget((object) decorator_0);
      return (object) decorator_0;
    }
  }

  internal UserPanel(ITypeData panelType, string title, GuiContext dockContext, MainWindow window)
  {
    this.panelType = panelType;
    this.LegacyContext = window;
    this.Context = dockContext;
    this.Title = title;
  }
}
