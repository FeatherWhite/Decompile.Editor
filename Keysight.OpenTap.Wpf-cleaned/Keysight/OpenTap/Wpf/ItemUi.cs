// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ItemUi
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ItemUi : BaseUi
{
  protected bool EnableReadOnly;
  public Action OnWrite = (Action) (() => { });
  private bool bool_2;
  private string[] string_0 = Array.Empty<string>();
  private bool bool_3 = true;
  private bool bool_4;
  private bool bool_5;
  private bool bool_7;
  private readonly List<ValidationError> list_0 = new List<ValidationError>();
  private AnnotationCollection annotation;
  private MenuUi menuUi_0;
  private MenuUi menuUi_1;
  private MenuUi menuUi_2;
  private MenuUi menuUi_3;
  private IEnumerable<AnnotationCollection> ienumerable_0;

  public override string GetName() => this.Item.DisplayName;

  private IEnumerable<MenuUi> method_0()
  {
    List<MenuUi> ilist_0 = new List<MenuUi>();
    try
    {
      MenuUi.LoadingCurrent = this;
      foreach (MenuAnnotation menuAnnotation in this.annotation.GetAll<MenuAnnotation>(false))
      {
        foreach (AnnotationCollection menuItem in menuAnnotation.MenuItems)
        {
          if (menuItem.Get<IMethodAnnotation>(false, (object) null) != null)
            ilist_0.Add(new MenuUi(menuItem, this));
        }
      }
    }
    finally
    {
      MenuUi.LoadingCurrent = (ItemUi) null;
    }
    ilist_0.SortBy<MenuUi, string>((Func<MenuUi, string>) (menuUi_0 => menuUi_0.Item.DisplayName));
    ilist_0.SortBy<MenuUi, double>((Func<MenuUi, double>) (menuUi_0 => menuUi_0.Item.Order));
    return (IEnumerable<MenuUi>) ilist_0;
  }

  public IEnumerable<MenuUi> MenuItems => this.method_0();

  public override double GetOrder() => this.Item.Order;

  private static IEnumerable<ITestStepParent> smethod_0(ITestStepParent itestStepParent_0)
  {
    for (; itestStepParent_0 != null; itestStepParent_0 = itestStepParent_0.Parent)
      yield return itestStepParent_0;
  }

  private static ITestStepParent[] smethod_1(ITestStepParent[] itestStepParent_0)
  {
    if (itestStepParent_0.Length == 0)
      return Array.Empty<ITestStepParent>();
    ITestStepParent[] array1 = ItemUi.smethod_0(itestStepParent_0[0].Parent).Reverse<ITestStepParent>().ToArray<ITestStepParent>();
    int num = array1.Length;
    for (int index1 = 1; index1 < itestStepParent_0.Length; ++index1)
    {
      ITestStepParent[] array2 = ItemUi.smethod_0(itestStepParent_0[index1].Parent).Reverse<ITestStepParent>().ToArray<ITestStepParent>();
      num = Math.Min(num, array2.Length);
      int index2 = 0;
      foreach (ITestStepParent itestStepParent in array2)
      {
        if (itestStepParent == array1[index2])
        {
          ++index2;
          if (index2 == num)
            break;
        }
        else
        {
          num = index2;
          break;
        }
      }
    }
    Array.Resize<ITestStepParent>(ref array1, num);
    Array.Reverse((Array) array1);
    return array1;
  }

  public static IEnumerable<object> GetScopes(object source)
  {
    switch (source)
    {
      case ITestStepParent[] itestStepParent_0:
        return (IEnumerable<object>) ItemUi.smethod_1(itestStepParent_0);
      case ITestStepParent itestStepParent:
        return (IEnumerable<object>) ItemUi.smethod_0(itestStepParent.Parent);
      default:
        return (IEnumerable<object>) Array.Empty<object>();
    }
  }

  public IEnumerable<object> Scopes => ItemUi.GetScopes(this.Annotation.Source);

  public object FirstScope => this.Scopes.FirstOrDefault<object>();

  public FrameworkElement Control { get; set; }

  public GenericGui.Item Item { get; set; }

  public bool HideTitle
  {
    get => this.bool_2;
    set
    {
      this.bool_2 = value;
      this.RaisePropertyChanged(nameof (HideTitle));
    }
  }

  public string[] DataErrors
  {
    get => this.string_0;
    set
    {
      if (((IEnumerable<string>) this.string_0).SequenceEqual<string>((IEnumerable<string>) value))
        return;
      this.string_0 = value;
      this.RaisePropertyChanged(nameof (DataErrors));
    }
  }

  public bool IsEnabled
  {
    get => this.bool_3;
    set
    {
      if (value == this.bool_3)
        return;
      this.bool_3 = value;
      this.RaisePropertyChanged(nameof (IsEnabled));
    }
  }

  public bool ReadOnlyStep
  {
    get => this.Annotation.Get<ReadOnlyStepAnnotation>(false, (object) null) != null;
  }

  public bool IsScoped
  {
    get => this.bool_4;
    private set
    {
      if (this.bool_4 == value)
        return;
      this.bool_4 = value;
      this.RaisePropertyChanged(nameof (IsScoped));
    }
  }

  public bool IsInputAssigned
  {
    get => this.bool_5;
    set
    {
      if (value == this.bool_5)
        return;
      this.bool_5 = value;
      this.RaisePropertyChanged(nameof (IsInputAssigned));
    }
  }

  public bool IsOutput
  {
    get
    {
      return ReflectionDataExtensions.HasAttribute<OutputAttribute>((IReflectionData) this.Item.Member);
    }
  }

  public bool IsOutputAssigned { get; set; }

  public bool IsFocused
  {
    get => this.bool_7;
    set
    {
      if (value == this.bool_7)
        return;
      this.bool_7 = value;
      this.RaisePropertyChanged(nameof (IsFocused));
    }
  }

  public bool IsScope { get; set; }

  public bool IsReferenceScope { get; set; }

  public IEnumerable<ItemUi.ScopeMember> ScopeMembers
  {
    get
    {
      return this.Item.Member is ParameterMemberData member ? (IEnumerable<ItemUi.ScopeMember>) member.ParameterizedMembers.Select<(object, IMemberData), ItemUi.ScopeMember>((Func<(object, IMemberData), ItemUi.ScopeMember>) (valueTuple_0 => new ItemUi.ScopeMember(this, valueTuple_0.Source, valueTuple_0.Member))).ToArray<ItemUi.ScopeMember>() : Enumerable.Empty<ItemUi.ScopeMember>();
    }
  }

  public static (object Scope, IMemberData member) GetScope(object owners, IMemberData member)
  {
    if (owners is object[] source)
      return ItemUi.GetScope((object[]) source.OfType<ITestStepParent>().ToArray<ITestStepParent>(), member);
    return ItemUi.GetScope((object[]) new ITestStepParent[1]
    {
      (ITestStepParent) owners
    }, member);
  }

  public static (object Scope, IMemberData member) GetScope(object[] owners, IMemberData member)
  {
    foreach (object scope in ItemUi.GetScopes((object) owners))
    {
      if (scope is ITestStepParent itestStepParent)
      {
        IParameterMemberData iparameterMemberData = TypeData.GetTypeData(scope).GetMembers().OfType<IParameterMemberData>().FirstOrDefault<IParameterMemberData>((Func<IParameterMemberData, bool>) (iparameterMemberData_0 => ((IEnumerable<object>) owners).All<object>((Func<object, bool>) (object_0 => iparameterMemberData_0.ParameterizedMembers.Contains<(object, IMemberData)>((object_0, member))))));
        if (iparameterMemberData != null)
          return ((object) itestStepParent, (IMemberData) iparameterMemberData);
      }
    }
    return ((object) null, (IMemberData) null);
  }

  public AnnotationCollection Annotation => this.annotation;

  public ItemUi(AnnotationCollection annotation, FrameworkElement control)
    : this(annotation, control, true)
  {
  }

  public ItemUi(AnnotationCollection annotation, FrameworkElement control, bool update)
  {
    this.Control = control;
    this.Item = new GenericGui.Item(annotation);
    this.annotation = annotation;
    this.IsReferenceScope = this.Item.Member is IParameterMemberData && !(this.Item.Member is ParameterMemberData);
    GuiOptions guiOptions = annotation.Get<GuiOptions>(false, (object) null);
    if (guiOptions != null)
      this.EnableReadOnly = guiOptions.OverridesReadOnly;
    if (!update)
      return;
    this.Update();
  }

  public void UpdateErrors()
  {
    if (this.annotation != null)
    {
      string[] array = ((IEnumerable) this.annotation).OfType<IErrorAnnotation>().SelectMany<IErrorAnnotation, string>((Func<IErrorAnnotation, IEnumerable<string>>) (ierrorAnnotation_0 => ierrorAnnotation_0.Errors ?? (IEnumerable<string>) Array.Empty<string>())).ToArray<string>();
      if (((IEnumerable<string>) array).SequenceEqual<string>((IEnumerable<string>) this.DataErrors))
        return;
      this.DataErrors = array;
    }
    else
      this.DataErrors = this.list_0.Select<ValidationError, string>((Func<ValidationError, string>) (validationError_0 => validationError_0.ErrorContent.ToString())).ToArray<string>();
  }

  public bool IsReadOnly { get; private set; }

  public void UpdateVisibility()
  {
    bool flag = this.annotation.GetAll<IEnabledAnnotation>(false).Select<IEnabledAnnotation, bool>((Func<IEnabledAnnotation, bool>) (ienabledAnnotation_0 => ienabledAnnotation_0.IsEnabled)).DefaultIfEmpty<bool>(true).All<bool>((Func<bool, bool>) (bool_0 => bool_0));
    IAccessAnnotation iaccessAnnotation = this.annotation.Get<IAccessAnnotation>(false, (object) null);
    GuiOptions guiOptions = this.annotation.Get<GuiOptions>(false, (object) null);
    if (iaccessAnnotation != null)
    {
      this.IsEnabled = ((flag ? 1 : 0) & (!iaccessAnnotation.IsReadOnly ? 1 : (this.EnableReadOnly ? 1 : 0))) != 0;
      this.IsReadOnly = iaccessAnnotation.IsReadOnly;
      this.IsVisible = iaccessAnnotation.IsVisible;
      if (guiOptions != null)
        goto label_5;
    }
    else
    {
      this.IsEnabled = flag;
      this.IsVisible = true;
      this.IsReadOnly = false;
      if (guiOptions != null)
        goto label_5;
    }
    int num = 0;
    goto label_6;
label_5:
    num = guiOptions.OverridesReadOnly ? 1 : 0;
label_6:
    if (num != 0)
      this.IsEnabled = flag;
    if (this.annotation.Get<ReadOnlyStepAnnotation>(false, (object) null) == null)
      return;
    this.IsEnabled = false;
  }

  private MenuUi method_1(string string_1)
  {
    MenuUi.LoadingCurrent = this;
    try
    {
      return this.menuItems.Where<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 => annotationCollection_0.Get<IconAnnotationAttribute>(false, (object) null).IconName == string_1)).Select<AnnotationCollection, MenuUi>((Func<AnnotationCollection, MenuUi>) (annotationCollection_0 => new MenuUi(annotationCollection_0, this))).FirstOrDefault<MenuUi>();
    }
    finally
    {
      MenuUi.LoadingCurrent = (ItemUi) null;
    }
  }

  public MenuUi EditParameterMenu
  {
    get => this.menuUi_0 ?? (this.menuUi_0 = this.method_1("OpenTap.IconNames.EditParameter"));
  }

  public MenuUi ShowParameterMenu
  {
    get
    {
      return this.menuUi_1 ?? (this.menuUi_1 = this.method_1("Keysight.OpenTap.Wpf.IconNames.ShowParameter"));
    }
  }

  public MenuUi ShowAssignedOutputMenu
  {
    get
    {
      return this.menuUi_2 ?? (this.menuUi_2 = this.method_1("Keysight.OpenTap.Wpf.IconNames.ShowAssignedOutput"));
    }
  }

  public MenuUi ShowAssignedInputMenu
  {
    get
    {
      return this.menuUi_3 ?? (this.menuUi_3 = this.method_1("Keysight.OpenTap.Wpf.IconNames.ShowAssignedInput"));
    }
  }

  private ITestStepParent method_2(ITestStepParent itestStepParent_0)
  {
    (ITestStepParent, IMemberData) valueTuple_0 = (itestStepParent_0, this.Item.Member);
    for (ITestStepParent parent = itestStepParent_0.Parent; parent != null; parent = parent.Parent)
    {
      if (TypeData.GetTypeData((object) parent).GetMembers().OfType<IParameterMemberData>().Any<IParameterMemberData>((Func<IParameterMemberData, bool>) (param =>
      {
        IEnumerable<(object, IMemberData)> parameterizedMembers = param.ParameterizedMembers;
        (ITestStepParent, IMemberData) valueTuple1 = valueTuple_0;
        (object, IMemberData) valueTuple2 = ((object) valueTuple1.Item1, valueTuple1.Item2);
        return parameterizedMembers.Contains<(object, IMemberData)>(valueTuple2);
      })))
        return parent;
    }
    return (ITestStepParent) null;
  }

  private IEnumerable<AnnotationCollection> menuItems
  {
    get
    {
      IEnumerable<AnnotationCollection> ienumerable0 = this.ienumerable_0;
      if (ienumerable0 != null)
        return ienumerable0;
      MenuAnnotation menuAnnotation = this.annotation.Get<MenuAnnotation>(false, (object) null);
      AnnotationCollection[] annotationCollectionArray;
      if (menuAnnotation == null)
      {
        annotationCollectionArray = (AnnotationCollection[]) null;
      }
      else
      {
        IEnumerable<AnnotationCollection> menuItems = menuAnnotation.MenuItems;
        if (menuItems == null)
        {
          annotationCollectionArray = (AnnotationCollection[]) null;
        }
        else
        {
          annotationCollectionArray = menuItems.ToArray<AnnotationCollection>();
          if (annotationCollectionArray != null)
            goto label_7;
        }
      }
      annotationCollectionArray = Array.Empty<AnnotationCollection>();
label_7:
      IEnumerable<AnnotationCollection> menuItems1 = (IEnumerable<AnnotationCollection>) annotationCollectionArray;
      this.ienumerable_0 = (IEnumerable<AnnotationCollection>) annotationCollectionArray;
      return menuItems1;
    }
  }

  public void UpdateIsExternal()
  {
    ITestStepParent[] source1 = (ITestStepParent[]) null;
    if (this.annotation.Source is ITestStepParent source3)
      source1 = new ITestStepParent[1]{ source3 };
    else if (this.annotation.Source is ITestStepParent[] source2)
      source1 = source2;
    if (source1 != null)
    {
      this.IsOutputAssigned = ((IEnumerable<ITestStepParent>) source1).Any<ITestStepParent>((Func<ITestStepParent, bool>) (itestStepParent_0 => InputOutputRelation.IsOutput(itestStepParent_0, this.Item.Member)));
      this.IsInputAssigned = ((IEnumerable<ITestStepParent>) source1).Any<ITestStepParent>((Func<ITestStepParent, bool>) (itestStepParent_0 => InputOutputRelation.IsInput(itestStepParent_0, this.Item.Member)));
      this.IsScoped = ((IEnumerable<ITestStepParent>) source1).Select<ITestStepParent, ITestStepParent>(new Func<ITestStepParent, ITestStepParent>(this.method_2)).Any<ITestStepParent>(new Func<ITestStepParent, bool>(Utils.IsNotNull<ITestStepParent>));
    }
    this.IsScope = this.Item.Member is ParameterMemberData;
  }

  internal void Update()
  {
    this.UpdateVisibility();
    this.UpdateErrors();
    this.RaisePropertyChanged("Item");
    this.IsFocused = false;
    this.UpdateIsExternal();
  }

  public override string ToString()
  {
    return $"Item {this.annotation} {(!this.IsVisible ? (object) " (hidden)" : (object) "")}";
  }

  public void Focus()
  {
    FrameworkElement frameworkElement;
    if (!this.Control.Focusable)
    {
      FrameworkElement control = this.Control;
      frameworkElement = control != null ? control.GetVisualChildren().OfType<FrameworkElement>().FirstOrDefault<FrameworkElement>((Func<FrameworkElement, bool>) (frameworkElement_0 => frameworkElement_0.Focusable)) : (FrameworkElement) null;
    }
    else
      frameworkElement = this.Control;
    FrameworkElement element = frameworkElement;
    if (element == null)
      return;
    element?.Focus();
    Keyboard.Focus((IInputElement) element);
    FocusManager.SetFocusedElement((DependencyObject) this.Control, (IInputElement) element);
    if (!(element is TextBox textBox))
      return;
    textBox.SelectAll();
  }

  public class ScopeMember
  {
    public object Scope { get; }

    public IMemberData Member { get; }

    public ItemUi Item { get; }

    public ScopeMember(ItemUi item, object scope, IMemberData member)
    {
      this.Scope = scope;
      this.Member = member;
      this.Item = item;
    }

    private string scopeName
    {
      get
      {
        object scope = this.Scope;
        switch (scope)
        {
          case ITestStep itestStep:
            return TestStepExtensions.GetFormattedName(itestStep);
          case TestPlan _:
            return "Test plan";
          case null:
            return "null";
          default:
            return scope.ToString();
        }
      }
    }

    public override string ToString()
    {
      return $"'{ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) this.Member).Name}' from '{this.scopeName}'";
    }
  }
}
