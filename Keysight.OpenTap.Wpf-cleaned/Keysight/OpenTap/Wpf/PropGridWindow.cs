// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropGridWindow
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropGridWindow : WslDialog, IComponentConnector
{
  private readonly object object_0;
  private readonly ITypeData itypeData_0;
  internal PropGrid propgrid;
  internal Button button;
  private bool bool_0;

  public IEnumerable<BaseUi> Groups => (IEnumerable<BaseUi>) this.propgrid.MainGroup.Items;

  public PropGridWindow(object object_1)
  {
    this.object_0 = object_1;
    this.InitializeComponent();
    this.propgrid.ViewRefreshed += new RoutedEventHandler(this.propgrid_ViewRefreshed);
    this.itypeData_0 = TypeData.GetTypeData(object_1);
    this.Loaded += new RoutedEventHandler(this.PropGridWindow_Loaded);
    string str1 = (string) null;
    try
    {
      if (object_1 is IDisplayAnnotation idisplayAnnotation)
        str1 = idisplayAnnotation.Name;
      if (str1 == null)
      {
        DisplayAttribute displayAttribute = ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) this.itypeData_0);
        if (displayAttribute != null && !((Attribute) displayAttribute).IsDefaultAttribute())
          str1 = displayAttribute.Name;
      }
      if (str1 == null)
      {
        if (this.itypeData_0.GetMember("Name")?.GetValue(object_1) is string str2)
          str1 = str2;
      }
    }
    catch
    {
    }
    if (str1 != null)
      this.Title = str1;
    HelpLinkAttribute attribute = ReflectionDataExtensions.GetAttribute<HelpLinkAttribute>((IReflectionData) this.itypeData_0);
    if (attribute == null)
      return;
    this.HasHelpButton = true;
    HelpManager.SetHelpLink((DependencyObject) this, (object) attribute.HelpLink);
  }

  private void PropGridWindow_Loaded(object sender, RoutedEventArgs e)
  {
    TapSize tapSize;
    if (ComponentSettings<GuiControlsSettings>.Current.UserInputSizes.TryGetValue(((IReflectionData) this.itypeData_0).Name, out tapSize))
    {
      this.Width = tapSize.Width;
      this.Height = tapSize.Height;
      this.SizeToContent = SizeToContent.Manual;
      if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height) && !double.IsNaN(this.Owner.Width) && !double.IsNaN(this.Owner.Height))
      {
        this.Left = this.Owner.Left + (this.Owner.Width - this.Width) / 2.0;
        this.Top = this.Owner.Top + (this.Owner.Height - this.Height) / 2.0;
      }
      this.SizeChanged += new SizeChangedEventHandler(this.PropGridWindow_SizeChanged);
    }
    else
      GuiHelpers.GuiInvokeAsync((Action) (() =>
      {
        ComponentSettings<GuiControlsSettings>.Current.UserInputSizes[((IReflectionData) this.itypeData_0).Name] = new TapSize()
        {
          Width = this.ActualWidth,
          Height = this.ActualHeight
        };
        this.Left = this.Owner.Left + (this.Owner.ActualWidth - this.ActualWidth) / 2.0;
        this.Top = this.Owner.Top + (this.Owner.ActualHeight - this.ActualHeight) / 2.0;
        this.SizeChanged += new SizeChangedEventHandler(this.PropGridWindow_SizeChanged);
      }), priority: DispatcherPriority.ContextIdle);
  }

  private void PropGridWindow_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    ComponentSettings<GuiControlsSettings>.Current.UserInputSizes[((IReflectionData) this.itypeData_0).Name] = new TapSize()
    {
      Width = e.NewSize.Width,
      Height = e.NewSize.Height
    };
    this.SizeToContent = SizeToContent.Manual;
  }

  private void propgrid_ViewRefreshed(object sender, EventArgs e)
  {
    if (this.object_0 == null)
      return;
    if (this.Groups.FirstOrDefault<BaseUi>() != null)
      this.propgrid.ItemUis.FirstOrDefault<ItemUi>()?.Control?.Focus();
    if (this.propgrid.MainGroup.Sequential.OfType<ItemUi>().Any<ItemUi>((Func<ItemUi, bool>) (itemUi_0 => itemUi_0.IsVisible && ReflectionDataExtensions.HasAttribute<SubmitAttribute>((IReflectionData) itemUi_0.Item.Member))))
      return;
    this.button.Visibility = Visibility.Visible;
  }

  private void button_Click(object sender, RoutedEventArgs e) => this.Close();

  private void method_0(object sender, RoutedEventArgs e)
  {
    if (this.object_0 == null)
      return;
    PropGrid propGrid = (PropGrid) sender;
    propGrid.DataContext = this.object_0;
    propGrid.OnCommit = (Action) (() => this.Close());
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/propgridwindow.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
      {
        this.bool_0 = true;
      }
      else
      {
        this.button = (Button) target;
        this.button.Click += new RoutedEventHandler(this.button_Click);
      }
    }
    else
      this.propgrid = (PropGrid) target;
  }
}
