// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PropGridListEditor
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class PropGridListEditor : PropGridListEditorBase, IComponentConnector, IStyleConnector
{
  public static readonly DependencyProperty UndoEnabledProperty = DependencyProperty.Register(nameof (UndoEnabled), typeof (bool), typeof (PropGridListEditor));
  private IResource iresource_0;
  internal PropGridListEditor This;
  internal ListBox lstBox;
  internal StackPanel buttonPanel;
  internal PropGrid propertyGrid;
  internal TextBlock clickPlusTextBlock;
  private bool bool_0;

  public bool UndoEnabled
  {
    get => (bool) this.GetValue(PropGridListEditor.UndoEnabledProperty);
    set => this.SetValue(PropGridListEditor.UndoEnabledProperty, (object) value);
  }

  public void canExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
  {
    if (e.OriginalSource is TextBox originalSource && originalSource.FindParent<TwoFaceBox>() != null || this.List != null && OpenTap.ReflectionHelper.HasAttribute<FixedSettingsListAttribute>(this.List.GetType()) || this.lstBox == null)
      return;
    e.CanExecute = this.lstBox.SelectedItems.Count > 0 && this.ContentEnabled;
  }

  public void canExecutePaste(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.List != null && OpenTap.ReflectionHelper.HasAttribute<FixedSettingsListAttribute>(this.List.GetType()))
      return;
    if (this.ContentEnabled)
    {
      try
      {
        string text = Clipboard.GetText();
        e.CanExecute = text != null;
      }
      catch
      {
        e.CanExecute = false;
      }
    }
    else
      e.CanExecute = false;
  }

  public PropGridListEditor()
  {
    this.InitializeComponent();
    this.lstBox.DataContextChanged += (DependencyPropertyChangedEventHandler) ((sender, e) => this.lstBox.SelectedIndex = 0);
  }

  protected override void OnListChanged(IList oldList, IList newList)
  {
    this.clickPlusTextBlock.Visibility = newList == null ? Visibility.Visible : (newList.Count == 0 ? Visibility.Visible : Visibility.Collapsed);
  }

  public override void OnCut(object sender, RoutedEventArgs e)
  {
    int val2 = this.List.IndexOf(this.lstBox.SelectedItems.Cast<object>().ToList<object>().First<object>());
    base.OnCut(sender, e);
    this.forceUpdateListBinding();
    if (this.lstBox.Items.Count > 0)
      this.lstBox.SelectedIndex = Math.Min(Math.Max(0, val2), this.List.Count - 1);
    this.propertyGrid.Focus();
  }

  internal void forceUpdateListBinding()
  {
    if (this.List != null)
    {
      foreach (IValidatingObject ivalidatingObject in this.List.OfType<IValidatingObject>())
        ivalidatingObject.OnPropertyChanged("ShortName");
    }
    this.onEdited();
    this.clickPlusTextBlock.Visibility = this.List == null ? Visibility.Visible : (this.List.Count == 0 ? Visibility.Visible : Visibility.Collapsed);
  }

  public override IEnumerable<object> SelectedItems
  {
    get => this.lstBox.SelectedItems.Cast<object>();
    set
    {
      this.forceUpdateListBinding();
      this.lstBox.Focus();
      this.lstBox.SelectedItems.Clear();
      foreach (object obj in value)
        this.lstBox.SelectedItems.Add(obj);
    }
  }

  public void SelectObject(object object_0) => this.lstBox.SelectedItem = object_0;

  public void OnDeleteListItem(object sender, RoutedEventArgs e)
  {
    System.Collections.Generic.List<object> list = this.lstBox.SelectedItems.Cast<object>().ToList<object>();
    int val2 = this.List.IndexOf(list.First<object>());
    foreach (object obj in list)
      this.List.Remove(obj);
    this.forceUpdateListBinding();
    this.lstBox.SelectedIndex = Math.Min(Math.Max(0, val2), this.List.Count - 1);
    this.propertyGrid.Focus();
  }

  protected override void OnAddItem(object newInstance)
  {
    this.lstBox.SelectedItems.Clear();
    this.forceUpdateListBinding();
    this.lstBox.SelectedItems.Add(newInstance);
  }

  private void propGrid_PropertyEdited(object sender, EventArgs e) => this.onEdited();

  private void method_3(object object_0)
  {
    TwoFaceBox twoFaceBox = object_0 as TwoFaceBox;
    if (string.IsNullOrWhiteSpace(twoFaceBox.FocusedText.ToString()))
      twoFaceBox.FocusedText = twoFaceBox.Tag;
    else
      this.onEdited();
  }

  public void TwoFaceBox_LostFocus(object sender, RoutedEventArgs e) => this.method_3(sender);

  public void TwoFaceBox_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    this.method_3(sender);
  }

  public void TwoFaceBox_GotFocus(object sender, RoutedEventArgs e)
  {
    TwoFaceBox twoFaceBox = sender as TwoFaceBox;
    twoFaceBox.Tag = twoFaceBox.FocusedText;
    this.iresource_0 = twoFaceBox.DataContext as IResource;
  }

  public void TwoFaceBox_Unloaded(object sender, RoutedEventArgs e)
  {
    IResource iresource0 = this.iresource_0;
    TwoFaceBox twoFaceBox = sender as TwoFaceBox;
    if (iresource0 == null || !string.IsNullOrWhiteSpace(iresource0.Name))
      return;
    iresource0.Name = (string) twoFaceBox.Tag;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controls/propgridlisteditor.xaml", UriKind.Relative));
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
    switch (connectionId)
    {
      case 1:
        this.This = (PropGridListEditor) target;
        break;
      case 2:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.canExecutePaste);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(((PropGridListEditorBase) this).OnPaste);
        break;
      case 3:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(((PropGridListEditorBase) this).CanCopy);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(((PropGridListEditorBase) this).OnCopy);
        break;
      case 4:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(((PropGridListEditorBase) this).CanCut);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(((PropGridListEditorBase) this).OnCut);
        break;
      case 5:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.canExecuteDelete);
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.OnDeleteListItem);
        break;
      case 7:
        this.lstBox = (ListBox) target;
        break;
      case 8:
        this.buttonPanel = (StackPanel) target;
        break;
      case 9:
        ((ButtonBase) target).Click += new RoutedEventHandler(((PropGridListEditorBase) this).AddButton_Click);
        break;
      case 10:
        this.propertyGrid = (PropGrid) target;
        break;
      case 11:
        this.clickPlusTextBlock = (TextBlock) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerNonUserCode]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 6)
      return;
    ((ToggleButton) target).Checked += new RoutedEventHandler(this.propGrid_PropertyEdited);
    ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.propGrid_PropertyEdited);
  }
}
