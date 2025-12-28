// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropGridListEditorBase
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropGridListEditorBase : UserControl
{
  protected static TraceSource traceSource_0 = Log.CreateSource("Settings");
  public static readonly DependencyProperty ListProperty = DependencyProperty.Register(nameof (List), typeof (IList), typeof (PropGridListEditorBase), new PropertyMetadata(new PropertyChangedCallback(PropGridListEditorBase.smethod_0)));
  public static readonly DependencyProperty ContentEnabledProperty = DependencyProperty.Register(nameof (ContentEnabled), typeof (bool), typeof (PropGridListEditorBase), new PropertyMetadata((object) true));

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    ((PropGridListEditorBase) dependencyObject_0).OnListChanged((IList) dependencyPropertyChangedEventArgs_0.OldValue, (IList) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  protected virtual void OnListChanged(IList oldList, IList newList)
  {
  }

  public IList List
  {
    get => (IList) this.GetValue(PropGridListEditorBase.ListProperty);
    set => this.SetValue(PropGridListEditorBase.ListProperty, (object) value);
  }

  protected void onEdited()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }

  public event EventHandler<EventArgs> PropertyEdited;

  public bool ContentEnabled
  {
    get => (bool) this.GetValue(PropGridListEditorBase.ContentEnabledProperty);
    set => this.SetValue(PropGridListEditorBase.ContentEnabledProperty, (object) value);
  }

  protected virtual void OnAddItem(object newItem)
  {
  }

  private void method_0(ITypeData itypeData_0)
  {
    object instance;
    try
    {
      itypeData_0.GetMembers().ToArray<IMemberData>();
      try
      {
        instance = ReflectionDataExtensions.CreateInstance(itypeData_0);
      }
      catch (TargetInvocationException ex)
      {
        ex.RethrowInner();
        return;
      }
    }
    catch (LicenseException ex)
    {
      Log.Error(PropGridListEditorBase.traceSource_0, $"Error during the creation of '{((IReflectionData) itypeData_0).Name}': {ex.Message}", Array.Empty<object>());
      return;
    }
    catch (Exception ex)
    {
      Log.Error(PropGridListEditorBase.traceSource_0, $"Error during the creation of '{((IReflectionData) itypeData_0).Name}'", Array.Empty<object>());
      Log.Debug(PropGridListEditorBase.traceSource_0, ex);
      return;
    }
    this.List.Add(instance);
    this.OnAddItem(instance);
  }

  protected virtual void AddButton_Click(object sender1, RoutedEventArgs e1)
  {
    Type type = this.method_1();
    TypeData itypeData_0 = TypeData.FromType(type);
    if (type.IsAbstract)
    {
      NewStepDialog newStepDialog = new NewStepDialog(type);
      newStepDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      newStepDialog.Owner = Window.GetWindow((DependencyObject) this);
      newStepDialog.Title = "Add New " + type.GetDisplayAttribute().Name;
      NewStepDialog newStepDialog_0 = newStepDialog;
      string str;
      switch (type.GetDisplayAttribute().Name)
      {
        case "Instrument":
          str = "EditorHelp.chm::/Configurations/Resource Configuration/Instrument Configuration/Readme.html";
          break;
        case "Dut":
          str = "EditorHelp.chm::/Configurations/Resource Configuration/DUT Configuration.html";
          break;
        case "Generic Connection":
          str = "EditorHelp.chm::/Configurations/Resource Configuration/Connection Configuration.html";
          break;
        case "Step":
          str = "EditorHelp.chm::/CreatingATestPlan/Working With Test Steps/Adding a New Step.html";
          break;
        case "Result Listener":
          str = "EditorHelp.chm::/Configurations/Result Listener Configuration/Readme.html";
          break;
        default:
          str = "EditorHelp.chm::/Configurations/Resource Configuration/Readme.html";
          break;
      }
      HelpManager.SetHelpLink((DependencyObject) newStepDialog_0, (object) str);
      newStepDialog_0.NewStepControl.OnAddStep += new Action<ITypeData>(this.method_0);
      newStepDialog_0.Loaded += (RoutedEventHandler) ((sender2, e2) => newStepDialog_0.GetLogicalChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (frameworkElement_0 => frameworkElement_0.Focusable))?.Focus());
      newStepDialog_0.ShowDialog();
    }
    else
    {
      if (!itypeData_0.CanCreateInstance)
        return;
      this.method_0((ITypeData) itypeData_0);
    }
  }

  private Type method_1()
  {
    Type type = this.List.GetType().GetInterface("IList`1");
    return type == (Type) null ? typeof (object) : ((IEnumerable<Type>) type.GetGenericArguments()).FirstOrDefault<Type>();
  }

  public virtual IEnumerable<object> SelectedItems { get; set; }

  public virtual void OnCopy(object sender, RoutedEventArgs e)
  {
    Type type = this.List.GetType();
    IList list = (IList) this.SelectedItems.ToList<object>();
    if (type.GetConstructor(new Type[0]) != (ConstructorInfo) null)
    {
      IList instance = (IList) Activator.CreateInstance(type);
      foreach (object obj in (IEnumerable) list)
        instance.Add(obj);
      list = instance;
    }
    ClipboardHelper.CopyText(new TapSerializer().SerializeToString((object) list));
  }

  public virtual void CanCopy(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.SelectedItems.Any<object>();
  }

  public virtual void OnCut(object sender, RoutedEventArgs e)
  {
    System.Collections.Generic.List<object> list = this.SelectedItems.ToList<object>();
    this.OnCopy(sender, e);
    foreach (object obj in list)
      this.List.Remove(obj);
  }

  public void CanCut(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.List != null && ReflectionHelper.HasAttribute<FixedSettingsListAttribute>(this.List.GetType()))
      return;
    e.CanExecute = this.ContentEnabled && this.SelectedItems.Any<object>();
  }

  public void OnPaste(object sender, RoutedEventArgs e)
  {
    string text;
    try
    {
      text = Clipboard.GetText();
    }
    catch (Exception ex)
    {
      QuickDialog.ShowMessage("Paste failed.", $"Caught error during paste action. '{ex.Message}'.");
      Log.Error(PropGridListEditorBase.traceSource_0, "Caught error during paste action. '{0}'.", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(PropGridListEditorBase.traceSource_0, ex);
      return;
    }
    try
    {
      IList source = (IList) new TapSerializer().DeserializeFromString(text, (ITypeData) null, true, (string) null);
      int num = this.SelectedItems.Any<object>() ? this.SelectedItems.Select<object, int>((Func<object, int>) (object_0 => this.List.IndexOf(object_0))).Max() : this.List.Count - 1;
      if (this.List.Count == 0)
        num = -1;
      System.Collections.Generic.List<object> objectList = new System.Collections.Generic.List<object>();
      foreach (object obj in source.Cast<object>().Reverse<object>())
      {
        this.List.Insert(num + 1, obj);
        objectList.Add(obj);
      }
      this.SelectedItems = (IEnumerable<object>) objectList;
    }
    catch
    {
      QuickDialog.ShowMessage("", "Unable to parse pasted data");
    }
  }
}
