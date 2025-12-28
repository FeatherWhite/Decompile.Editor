// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TapDock
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class TapDock : System.Windows.Controls.UserControl, IComponentConnector
{
  private bool bool_0;
  private static readonly OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("Panels");
  private readonly List<Class133> list_0 = new List<Class133>();
  internal DockingManager dockingManager_0;
  private bool bool_1;

  public TapDock()
  {
    int num = System.Windows.Application.Current.MainWindow.IsActive ? 1 : 0;
    this.InitializeComponent();
    this.Tag = (object) new List<System.Windows.Controls.MenuItem>();
    if (num != 0)
      this.Loaded += new RoutedEventHandler(this.TapDock_Loaded);
    ViewPreset.SaveDefaultProfile((Action) (() => this.SaveLayout()));
  }

  public bool FocusMode
  {
    get => this.bool_0;
    set
    {
      this.bool_0 = value;
      foreach (LayoutAnchorable layoutAnchorable in this.dockingManager_0.smethod_1())
      {
        if (value && layoutAnchorable.IsFloating)
          layoutAnchorable.Hide();
        layoutAnchorable.CanHide = !value;
        layoutAnchorable.CanFloat = !value;
      }
    }
  }

  private void TapDock_Loaded(object sender, RoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TapDock.Class148 class148 = new TapDock.Class148();
    // ISSUE: reference to a compiler-generated field
    class148.window_0 = Window.GetWindow((DependencyObject) this);
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class148.method_0));
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class148.method_1), priority: DispatcherPriority.ContextIdle);
  }

  private bool method_0(string string_0, string string_1)
  {
    if (!string_1.StartsWith(string_0 + " "))
      return false;
    int result = 0;
    return int.TryParse(string_1.Substring(string_0.Length + 1), out result);
  }

  public void LoadDockingPlugins()
  {
    this.list_0.ForEach((Action<Class133>) (class133_0 => class133_0.Dispose()));
    this.list_0.Clear();
    List<System.Windows.Controls.MenuItem> menuItemList = new List<System.Windows.Controls.MenuItem>();
    List<System.Windows.Controls.MenuItem> source1 = new List<System.Windows.Controls.MenuItem>();
    MainWindow parent = GuiHelpers.FindParent<MainWindow>((DependencyObject) this);
    Type[] array = PluginManager.GetPlugins<ITapDockPanel>().OrderBy<Type, string>((Func<Type, string>) (type_0 => type_0.smethod_2().GetFullName())).ToArray<Type>();
    ReadOnlyCollection<Type> plugins = PluginManager.GetPlugins<ITapDockMultiPanel>();
    bool flag = false;
    Dictionary<string, LayoutAnchorable> source2 = new Dictionary<string, LayoutAnchorable>();
    foreach (LayoutAnchorable layoutAnchorable in this.dockingManager_0.smethod_1())
    {
      layoutAnchorable.Title = TapDock.smethod_0(layoutAnchorable.Title);
      if (source2.ContainsKey(layoutAnchorable.ContentId))
        flag = true;
      source2[layoutAnchorable.ContentId] = layoutAnchorable;
    }
    foreach (Type type in plugins)
    {
      ITapDockMultiPanel instance = (ITapDockMultiPanel) Activator.CreateInstance(type);
      string str = instance.GetType().smethod_4() ?? "EditorHelp.chm";
      string name = type.smethod_2().Name;
      System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
      menuItem.Click += new RoutedEventHandler(this.method_4);
      menuItem.Header = (object) name;
      menuItem.DataContext = (object) type;
      foreach (KeyValuePair<string, LayoutAnchorable> keyValuePair in source2.ToArray<KeyValuePair<string, LayoutAnchorable>>())
      {
        if (this.method_0(name, keyValuePair.Key))
        {
          LayoutAnchorable layoutAnchorable = keyValuePair.Value;
          HelpManager.SetHelpLink((DependencyObject) layoutAnchorable, (object) str);
          if (!(layoutAnchorable.Content is ContentControl contentControl))
            contentControl = (layoutAnchorable.Content = (object) new ContentControl()) as ContentControl;
          FrameworkElement element = instance.CreateElement((ITapDockContext) parent);
          contentControl.Content = (object) element;
          element.Visibility = System.Windows.Visibility.Visible;
          source2.Remove(keyValuePair.Key);
          System.Windows.Controls.MenuItem newItem = new System.Windows.Controls.MenuItem();
          newItem.DataContext = (object) layoutAnchorable;
          newItem.Click += new RoutedEventHandler(this.method_4);
          menuItem.Items.Add((object) newItem);
        }
      }
      menuItemList.Add(menuItem);
    }
    foreach (IGrouping<string, Type> source3 in (IEnumerable<IGrouping<string, Type>>) ((IEnumerable<Type>) array).ToLookup<Type, string>((Func<Type, string>) (type_0 => type_0.smethod_2().Name)))
    {
      if (source3.Count<Type>() > 1)
      {
        TapDock.traceSource_0.Warning("Multiple plugin types has the same display name, only one will be shown in panels.");
        TapDock.traceSource_0.Info("Affected plugin types:");
        foreach (Type type in (IEnumerable<Type>) source3)
          TapDock.traceSource_0.Info("   {0}", (object) type.FullName);
      }
    }
    foreach (Type type in array)
    {
      try
      {
        DisplayAttribute displayAttribute = type.smethod_2();
        System.Windows.Controls.MenuItem menuItem1 = (System.Windows.Controls.MenuItem) null;
        foreach (string str in displayAttribute.Group)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          TapDock.Class149 class149 = new TapDock.Class149();
          // ISSUE: reference to a compiler-generated field
          class149.string_0 = str;
          // ISSUE: reference to a compiler-generated method
          System.Windows.Controls.MenuItem newItem = source1.FirstOrDefault<System.Windows.Controls.MenuItem>(new Func<System.Windows.Controls.MenuItem, bool>(class149.method_0));
          if (newItem == null)
          {
            System.Windows.Controls.MenuItem menuItem2 = new System.Windows.Controls.MenuItem();
            // ISSUE: reference to a compiler-generated field
            menuItem2.Header = (object) class149.string_0;
            newItem = menuItem2;
            source1.Add(newItem);
            if (menuItem1 == null)
              menuItemList.Add(newItem);
            else
              menuItem1.Items.Add((object) newItem);
          }
          menuItem1 = newItem;
        }
        Class133 class133 = new Class133((ITypeData) TypeData.FromType(type), parent.GuiContext.GetSubContext());
        this.list_0.Add(class133);
        System.Windows.Controls.MenuItem menuItem3 = new System.Windows.Controls.MenuItem();
        menuItem3.DataContext = (object) class133;
        menuItem3.IsCheckable = true;
        menuItem3.Header = (object) displayAttribute.Name;
        System.Windows.Controls.MenuItem newItem1 = menuItem3;
        newItem1.Loaded += new RoutedEventHandler(this.method_1);
        newItem1.Click += new RoutedEventHandler(this.method_4);
        if (menuItem1 != null)
          menuItem1.Items.Add((object) newItem1);
        else
          menuItemList.Add(newItem1);
      }
      catch
      {
      }
    }
    this.Tag = (object) menuItemList;
    if (flag)
      TapDock.traceSource_0.Warning("Multiple panels with the same name exists.");
    foreach (LayoutAnchorable layoutAnchorable in this.dockingManager_0.smethod_1().OfType<LayoutAnchorable>())
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TapDock.Class150 class150 = new TapDock.Class150();
      // ISSUE: reference to a compiler-generated field
      class150.layoutAnchorable_0 = layoutAnchorable;
      // ISSUE: reference to a compiler-generated field
      if (class150.layoutAnchorable_0.Content == null)
      {
        // ISSUE: reference to a compiler-generated method
        Class133 class133 = this.list_0.FirstOrDefault<Class133>(new Func<Class133, bool>(class150.method_0));
        // ISSUE: reference to a compiler-generated field
        class150.layoutAnchorable_0.Content = (object) class133.method_0(parent);
      }
    }
  }

  private void method_1(object sender, RoutedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TapDock.Class143 class143 = new TapDock.Class143();
    object dataContext1 = sender is System.Windows.Controls.MenuItem menuItem1 ? menuItem1.DataContext : (object) null;
    // ISSUE: reference to a compiler-generated field
    class143.class133_0 = dataContext1 as Class133;
    // ISSUE: reference to a compiler-generated field
    if (class143.class133_0 != null)
    {
      System.Windows.Controls.MenuItem menuItem2 = menuItem1;
      // ISSUE: reference to a compiler-generated method
      LayoutAnchorable layoutAnchorable = this.dockingManager_0.smethod_1().FirstOrDefault<LayoutAnchorable>(new Func<LayoutAnchorable, bool>(class143.method_0));
      int num = layoutAnchorable != null ? (!layoutAnchorable.IsHidden ? 1 : 0) : 0;
      menuItem2.IsChecked = num != 0;
    }
    ITapDockMultiPanel dataContext2 = menuItem1?.DataContext as ITapDockMultiPanel;
  }

  private DockingManager dock2 => this.dockingManager_0;

  private bool method_2(string string_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    LayoutAnchorable layoutAnchorable = this.dock2.smethod_1().FirstOrDefault<LayoutAnchorable>(new Func<LayoutAnchorable, bool>(new TapDock.Class144()
    {
      string_0 = string_0
    }.method_0));
    layoutAnchorable?.Hide();
    return layoutAnchorable != null;
  }

  private DependencyObject method_3(string string_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TapDock.Class145 class145 = new TapDock.Class145();
    // ISSUE: reference to a compiler-generated field
    class145.string_0 = string_0;
    // ISSUE: reference to a compiler-generated method
    Class133 class133 = this.list_0.FirstOrDefault<Class133>(new Func<Class133, bool>(class145.method_0));
    LayoutAnchorable gparam_0;
    // ISSUE: reference to a compiler-generated method
    if (this.dock2.smethod_1().smethod_2<LayoutAnchorable>(new Func<LayoutAnchorable, bool>(class145.method_1), out gparam_0))
    {
      gparam_0.Show();
      return (gparam_0.Content is UserPanel content ? content.Content : (object) null) as DependencyObject;
    }
    MainWindow window = (MainWindow) Window.GetWindow((DependencyObject) this);
    UserPanel userPanel = class133.method_0(window);
    LayoutAnchorable contentModel = new LayoutAnchorable();
    contentModel.Content = (object) userPanel;
    contentModel.FloatingHeight = userPanel.DesiredHeight ?? 300.0;
    double? nullable = userPanel.DesiredWidth;
    contentModel.FloatingWidth = nullable ?? 300.0;
    double num1 = window.Left + window.Width / 2.0;
    nullable = userPanel.DesiredWidth;
    double num2 = (nullable ?? 300.0) / 2.0;
    contentModel.FloatingLeft = num1 - num2;
    double num3 = window.Top + window.Height / 2.0;
    nullable = userPanel.DesiredHeight;
    double num4 = (nullable ?? 300.0) / 2.0;
    contentModel.FloatingTop = num3 - num4;
    LayoutFloatingWindowControl floatingWindow = this.dock2.CreateFloatingWindow((LayoutContent) contentModel, false);
    floatingWindow.MinHeight = 200.0;
    floatingWindow.MinWidth = 300.0;
    floatingWindow.Background = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Black;
    floatingWindow.Show();
    return (DependencyObject) floatingWindow;
  }

  private void method_4(object sender, RoutedEventArgs e)
  {
    if (!(sender is System.Windows.Controls.MenuItem menuItem) || !(menuItem.DataContext is Class133 dataContext))
      return;
    if (!menuItem.IsChecked)
      this.method_2(dataContext.String_0);
    else
      this.method_3(dataContext.String_0);
  }

  public void SaveLayout()
  {
    ComponentSettings<PanelSettings>.Current.Layout = this.dock2.smethod_0();
  }

  public void LoadLayout()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TapDock.Class146 class146 = new TapDock.Class146();
    // ISSUE: reference to a compiler-generated field
    class146.tapDock_0 = this;
    try
    {
      if (string.IsNullOrWhiteSpace(ComponentSettings<PanelSettings>.Current.Layout))
        return;
      // ISSUE: reference to a compiler-generated field
      class146.mainWindow_0 = (MainWindow) Window.GetWindow((DependencyObject) this);
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ComponentSettings<PanelSettings>.Current.Layout)))
      {
        XmlLayoutSerializer layoutSerializer = new XmlLayoutSerializer(this.dockingManager_0);
        // ISSUE: reference to a compiler-generated method
        layoutSerializer.LayoutSerializationCallback += new EventHandler<LayoutSerializationCallbackEventArgs>(class146.method_0);
        try
        {
          layoutSerializer.Deserialize((Stream) memoryStream);
        }
        catch (Exception ex)
        {
          TapDock.traceSource_0.Error("Unable to load panel layout.");
          TapDock.traceSource_0.Debug(ex);
        }
      }
    }
    catch
    {
    }
  }

  public void EnsureWindowsInsideWorkingArea()
  {
    foreach (Window window in ((IEnumerable<Window>) this.dockingManager_0.FloatingWindows).Append<Window>(Window.GetWindow((DependencyObject) this)))
    {
      PresentationSource presentationSource = PresentationSource.FromVisual((Visual) window);
      if (presentationSource != null)
      {
        Matrix transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        Vector vector1 = new Vector(window.Left, window.Top);
        Vector vector2 = new Vector(window.Left + window.Width, window.Top + window.Height);
        Vector vector3 = transformToDevice.Transform(vector2);
        Vector vector4 = transformToDevice.Transform(vector1);
        bool flag = false;
        Rectangle rect = new Rectangle((int) vector4.X, (int) vector4.Y, (int) (vector3.X - vector4.X), (int) (vector3.Y - vector4.Y));
        foreach (Screen allScreen in Screen.AllScreens)
        {
          if (allScreen.WorkingArea.IntersectsWith(rect))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
          Vector vector5 = new Vector((double) workingArea.Left, (double) workingArea.Top);
          Vector vector6 = presentationSource.CompositionTarget.TransformFromDevice.Transform(vector5);
          window.Left = vector6.X;
          window.Top = vector6.Y;
        }
      }
    }
  }

  public DependencyObject ShowPanel(string panelName) => this.method_3(panelName);

  protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs keyEventArgs_0)
  {
    if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && keyEventArgs_0.IsDown && keyEventArgs_0.Key == Key.Tab)
      keyEventArgs_0.Handled = true;
    base.OnPreviewKeyDown(keyEventArgs_0);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_1)
      return;
    this.bool_1 = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/Editor;component/tapdock.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      this.dockingManager_0 = (DockingManager) target;
    else
      this.bool_1 = true;
  }

  [CompilerGenerated]
  internal static string smethod_0(string string_0)
  {
    switch (string_0)
    {
      case "Steps":
        return "Test Steps";
      case "Step Settings":
        return "Test Step Settings";
      default:
        return string_0;
    }
  }
}
