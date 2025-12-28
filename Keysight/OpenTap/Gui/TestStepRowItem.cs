// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestStepRowItem
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class TestStepRowItem : DependencyObject
{
  public readonly TestStepRowItem Parent;
  public readonly List<TestStepRowItem> VisibleChildItems;
  public readonly IList<TestStepRowItem> AllChildItems;
  private readonly ITestStep itestStep_0;
  public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (TestStepRowItem), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(TestStepRowItem.smethod_0)));
  public static readonly DependencyProperty FirstChildProperty = DependencyProperty.Register(nameof (FirstChild), typeof (bool), typeof (TestStepRowItem), new PropertyMetadata((object) false));
  private static readonly DependencyPropertyKey dependencyPropertyKey_0 = DependencyProperty.RegisterReadOnly(nameof (IsVisible), typeof (bool), typeof (TestStepRowItem), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsVisibleProperty = TestStepRowItem.dependencyPropertyKey_0.DependencyProperty;
  public static readonly DependencyProperty HasChildrenProperty = DependencyProperty.Register(nameof (HasChildren), typeof (bool), typeof (TestStepRowItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty IsDraggedOverProperty = DependencyProperty.Register(nameof (IsDraggedOver), typeof (bool), typeof (TestStepRowItem));
  public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(nameof (Enabled), typeof (bool), typeof (TestStepRowItem), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(TestStepRowItem.smethod_1)));
  public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof (IsEnabled), typeof (bool), typeof (TestStepRowItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TestStepRowItem.smethod_2)));
  public static readonly DependencyProperty IsAtBreakProperty = DependencyProperty.Register(nameof (IsAtBreak), typeof (bool), typeof (TestStepRowItem));
  public static readonly DependencyProperty IsBreakpointProperty = DependencyProperty.Register(nameof (IsBreakpoint), typeof (bool), typeof (TestStepRowItem));
  private AnnotationCollection annotationCollection_0;
  private static readonly ConditionalWeakTable<TestPlan, UpdateMonitor> conditionalWeakTable_0 = new ConditionalWeakTable<TestPlan, UpdateMonitor>();
  private static TraceSource traceSource_0 = Log.CreateSource("TestStep");
  public static readonly DependencyProperty FlowProperty = DependencyProperty.Register(nameof (Flow), typeof (FlowViewModel), typeof (TestStepRowItem));

  public event EventHandler RowChanged;

  protected void OnRowChanged(EventArgs eventArgs_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, eventArgs_0);
  }

  public ITestStep Step => this.itestStep_0;

  public void InsertChildStep(TestStepRowItem item, int index)
  {
    this.VisibleChildItems.Insert(index, item);
    this.HasChildren = true;
  }

  public bool IsExpanded
  {
    get => (bool) this.GetValue(TestStepRowItem.IsExpandedProperty);
    set => this.SetValue(TestStepRowItem.IsExpandedProperty, (object) value);
  }

  private void method_0(bool bool_0)
  {
    this.IsVisible = ((bool_0 ? 1 : 0) & (this.Parent == null ? 1 : (!this.Parent.IsVisible ? 0 : (this.Parent.IsExpanded ? 1 : 0)))) != 0;
    foreach (TestStepRowItem allChildItem in (IEnumerable<TestStepRowItem>) this.AllChildItems)
      allChildItem.method_0(bool_0 & this.IsExpanded);
  }

  public bool FirstChild
  {
    get => (bool) this.GetValue(TestStepRowItem.FirstChildProperty);
    set => this.SetValue(TestStepRowItem.FirstChildProperty, (object) value);
  }

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    foreach (TestStepRowItem allChildItem in (IEnumerable<TestStepRowItem>) (dependencyObject_0 as TestStepRowItem).AllChildItems)
      allChildItem.method_0((bool) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  public bool IsVisible
  {
    get => (bool) this.GetValue(TestStepRowItem.IsVisibleProperty);
    private set => this.SetValue(TestStepRowItem.dependencyPropertyKey_0, (object) value);
  }

  public bool HasChildren
  {
    get => (bool) this.GetValue(TestStepRowItem.HasChildrenProperty);
    set => this.SetValue(TestStepRowItem.HasChildrenProperty, (object) value);
  }

  public bool IsDraggedOver
  {
    get => (bool) this.GetValue(TestStepRowItem.IsDraggedOverProperty);
    set => this.SetValue(TestStepRowItem.IsDraggedOverProperty, (object) value);
  }

  public bool Enabled
  {
    get => (bool) this.GetValue(TestStepRowItem.EnabledProperty);
    set => this.SetValue(TestStepRowItem.EnabledProperty, (object) value);
  }

  public string Error => this.method_3();

  private static void smethod_1(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as TestStepRowItem).method_1();
  }

  private void method_1()
  {
    this.Step.Enabled = this.Enabled;
    this.IsEnabled = ((this.Enabled ? 1 : 0) & (this.Parent == null ? 1 : (this.Parent.IsEnabled ? 1 : 0))) != 0;
  }

  public bool IsEnabled
  {
    get => (bool) this.GetValue(TestStepRowItem.IsEnabledProperty);
    set => this.SetValue(TestStepRowItem.IsEnabledProperty, (object) value);
  }

  private void method_2()
  {
    foreach (TestStepRowItem visibleChildItem in this.VisibleChildItems)
      visibleChildItem.IsEnabled = this.IsEnabled & visibleChildItem.Enabled;
  }

  private static void smethod_2(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    (dependencyObject_0 as TestStepRowItem).method_2();
  }

  public bool IsEndPoint
  {
    get
    {
      TestStepList childTestSteps = this.Step.Parent.ChildTestSteps;
      return childTestSteps.Count != 0 && childTestSteps[childTestSteps.Count - 1] == this.Step;
    }
  }

  public bool IsAtBreak
  {
    get => (bool) this.GetValue(TestStepRowItem.IsAtBreakProperty);
    set => this.SetValue(TestStepRowItem.IsAtBreakProperty, (object) value);
  }

  public bool IsBreakpoint
  {
    get => (bool) this.GetValue(TestStepRowItem.IsBreakpointProperty);
    set => this.SetValue(TestStepRowItem.IsBreakpointProperty, (object) value);
  }

  public IEnumerable<TestStepRowItem> AllParents
  {
    get
    {
      TestStepRowItem[] parents = (TestStepRowItem[]) this.GetParents();
      Array.Reverse((Array) parents);
      return (IEnumerable<TestStepRowItem>) parents;
    }
  }

  public IEnumerable<TestStepRowItem> GetParents()
  {
    TestStepRowItem parent1 = this.Parent;
    int length = 0;
    while (parent1 != null)
    {
      parent1 = parent1.Parent;
      ++length;
    }
    if (length == 0)
      return (IEnumerable<TestStepRowItem>) Array.Empty<TestStepRowItem>();
    TestStepRowItem[] parents = new TestStepRowItem[length];
    TestStepRowItem parent2 = this.Parent;
    int index = 0;
    while (parent2 != null)
    {
      parents[index] = parent2;
      parent2 = parent2.Parent;
      ++index;
    }
    return (IEnumerable<TestStepRowItem>) parents;
  }

  public AnnotationCollection Annotation
  {
    get
    {
      if (this.annotationCollection_0 == null)
        this.annotationCollection_0 = AnnotationCollection.Annotate((object) this.Step, (IAnnotation) TestStepRowItem.conditionalWeakTable_0.GetValue(TestStepExtensions.GetParent<TestPlan>(this.Step), (ConditionalWeakTable<TestPlan, UpdateMonitor>.CreateValueCallback) (testPlan_0 => new UpdateMonitor()
        {
          Owner = (object) testPlan_0
        })), (IAnnotation) new GuiOptions()
        {
          FullRow = true,
          GridMode = true
        });
      return this.annotationCollection_0;
    }
  }

  public TestStepRowItem(ITestStep step, TestStepRowItem _parentItem)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestStepRowItem.Class191 class191 = new TestStepRowItem.Class191();
    // ISSUE: reference to a compiler-generated field
    class191.itestStep_0 = step;
    // ISSUE: explicit constructor call
    base.\u002Ector();
    // ISSUE: reference to a compiler-generated field
    class191.testStepRowItem_0 = this;
    this.Parent = _parentItem;
    // ISSUE: reference to a compiler-generated field
    this.itestStep_0 = class191.itestStep_0;
    // ISSUE: reference to a compiler-generated field
    this.Enabled = class191.itestStep_0.Enabled;
    this.method_0(true);
    this.method_1();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class191.itestStep_0.PropertyChanged += new PropertyChangedEventHandler(class191.method_0);
    this.Step.ChildTestSteps.CollectionChanged += new NotifyCollectionChangedEventHandler(this.method_7);
    // ISSUE: reference to a compiler-generated method
    this.Step.ChildTestSteps.ToList<ITestStep>().ForEach(new Action<ITestStep>(class191.method_2));
  }

  private string method_3()
  {
    ITestStep step = this.Step;
    try
    {
      string error1 = step.Error;
      if (error1 != null)
      {
        StringBuilder stringBuilder = new StringBuilder(error1.Length);
        ITypeData typeData = TypeData.GetTypeData((object) step);
        bool flag = true;
        foreach (ValidationRule rule in (Collection<ValidationRule>) step.Rules)
        {
          if (!rule.IsValid())
          {
            IMemberData member = typeData.GetMember(rule.PropertyName);
            if (member != null && EnabledIfAttribute.IsEnabled(member, (object) step))
            {
              if (!flag)
                stringBuilder.AppendLine();
              else
                flag = false;
              stringBuilder.Append(rule.ErrorMessage);
            }
          }
        }
        if (stringBuilder.Length != 0)
          return stringBuilder.ToString();
        if (!this.IsExpanded)
        {
          if (this.IsVisible)
          {
            if (this.AllChildItems.Any<TestStepRowItem>())
            {
              List<TestStepRowItem> list = this.GetAllChildRowItems().Skip<TestStepRowItem>(1).ToList<TestStepRowItem>();
              List<string> stringList = new List<string>();
              foreach (TestStepRowItem testStepRowItem in list)
              {
                string error2 = testStepRowItem.Error;
                if (!string.IsNullOrEmpty(error2))
                  stringList.Add($"{testStepRowItem.Step.Name}: {error2}");
              }
              if (stringList.Any<string>())
                return "One or more child steps have errors:\n" + string.Join("\n", (IEnumerable<string>) stringList);
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      if (TestStepRowItem.traceSource_0.smethod_27((object) this, "Caught exception while getting validation error: {0}", (object) ex.Message))
        TestStepRowItem.traceSource_0.Debug(ex);
      return $"Error: {ex.Message}";
    }
    return "";
  }

  private void method_4(ITestStep itestStep_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    TestStepRowItem testStepRowItem = this.AllChildItems.FirstOrDefault<TestStepRowItem>(new Func<TestStepRowItem, bool>(new TestStepRowItem.Class192()
    {
      itestStep_0 = itestStep_1
    }.method_0));
    this.AllChildItems.First<TestStepRowItem>().FirstChild = false;
    this.AllChildItems.Remove(testStepRowItem);
    if (this.AllChildItems.Count > 0)
      this.AllChildItems.First<TestStepRowItem>().FirstChild = true;
    testStepRowItem.RowChanged -= new EventHandler(this.method_6);
    this.VisibleChildItems.Remove(testStepRowItem);
    this.HasChildren = this.AllChildItems.Count > 0;
    if (!this.IsExpanded || this.HasChildren)
      return;
    this.IsExpanded = false;
  }

  private void method_5(ITestStep itestStep_1, int int_0 = -1)
  {
    if (int_0 == -1)
      int_0 = this.AllChildItems.Count;
    TestStepRowItem testStepRowItem = new TestStepRowItem(itestStep_1, this);
    testStepRowItem.RowChanged += new EventHandler(this.method_6);
    if (this.AllChildItems.Count > 0 && int_0 == 0)
      this.AllChildItems.First<TestStepRowItem>().FirstChild = false;
    if (int_0 == 0)
      testStepRowItem.FirstChild = true;
    this.AllChildItems.Insert(int_0, testStepRowItem);
    this.VisibleChildItems.Insert(int_0, testStepRowItem);
    this.HasChildren = true;
  }

  private void method_6(object sender, EventArgs e) => this.OnRowChanged(new EventArgs());

  private void method_7(object sender, NotifyCollectionChangedEventArgs e)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestStepRowItem.Class193 class193 = new TestStepRowItem.Class193();
    // ISSUE: reference to a compiler-generated field
    class193.testStepRowItem_0 = this;
    if (e.OldItems != null)
      e.OldItems.Cast<ITestStep>().ToList<ITestStep>().ForEach(new Action<ITestStep>(this.method_4));
    if (e.NewItems != null)
    {
      List<ITestStep> list = e.NewItems.Cast<ITestStep>().ToList<ITestStep>();
      int newStartingIndex = e.NewStartingIndex;
      // ISSUE: reference to a compiler-generated field
      class193.int_0 = newStartingIndex;
      // ISSUE: reference to a compiler-generated method
      Action<ITestStep> action = new Action<ITestStep>(class193.method_0);
      list.ForEach(action);
    }
    if (e.Action != NotifyCollectionChangedAction.Reset)
      return;
    foreach (ITestStep itestStep_1 in this.AllChildItems.Select<TestStepRowItem, ITestStep>((Func<TestStepRowItem, ITestStep>) (testStepRowItem_0 => testStepRowItem_0.Step)).ToArray<ITestStep>())
      this.method_4(itestStep_1);
  }

  public FlowViewModel Flow
  {
    get => (FlowViewModel) this.GetValue(TestStepRowItem.FlowProperty);
    set => this.SetValue(TestStepRowItem.FlowProperty, (object) value);
  }

  public IEnumerable<TestStepRowItem> GetAllChildRowItems()
  {
    return (IEnumerable<TestStepRowItem>) Class24.smethod_13<TestStepRowItem>((IEnumerable<TestStepRowItem>) new TestStepRowItem[1]
    {
      this
    }, (Func<TestStepRowItem, IEnumerable<TestStepRowItem>>) (testStepRowItem_0 => (IEnumerable<TestStepRowItem>) testStepRowItem_0.AllChildItems));
  }

  public override string ToString() => $"Row: {this.Step}";

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != TestStepRowItem.FlowProperty || !(dependencyPropertyChangedEventArgs_0.NewValue is FlowViewModel newValue))
      return;
    newValue.pushUpdate = (Action) (() =>
    {
      if (this.annotationCollection_0 == null)
        return;
      this.annotationCollection_0.Read((object) this.Step);
      this.annotationCollection_0.Get<UpdateMonitor>(true).PushUpdate();
    });
  }
}
