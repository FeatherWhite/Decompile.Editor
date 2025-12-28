// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestPlanGridListener
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using Keysight.OpenTap.Wpf.ControlProviders;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Browsable(false)]
public class TestPlanGridListener : 
  ResultListener,
  IExecutionListener,
  IResultListener,
  IResource,
  INotifyPropertyChanged,
  ITapPlugin
{
  public TestPlanGrid Grid;
  private readonly List<FlowViewModel> list_0 = new List<FlowViewModel>();
  private readonly Dictionary<Guid, FlowViewModel> dictionary_0 = new Dictionary<Guid, FlowViewModel>();
  public TestPlan Plan;
  private long long_0;
  private double? nullable_0;
  private bool bool_0;
  private bool bool_1;
  private Verdict verdict_0;
  private double? nullable_1;
  private IResultStore iresultStore_0;
  private object object_0 = new object();
  private Dictionary<FlowViewModel, TestStepRun> dictionary_1 = new Dictionary<FlowViewModel, TestStepRun>();

  public double? AverageDuration
  {
    get => this.nullable_0;
    set
    {
      double? nullable0 = this.nullable_0;
      double? nullable = value;
      if (nullable0.GetValueOrDefault() == nullable.GetValueOrDefault() & nullable0.HasValue == nullable.HasValue)
        return;
      this.nullable_0 = value;
      this.OnPropertyChanged(nameof (AverageDuration));
    }
  }

  public bool IsRunning
  {
    get => this.bool_0;
    set
    {
      if (this.bool_0 == value)
        return;
      this.bool_0 = value;
      this.OnPropertyChanged(nameof (IsRunning));
    }
  }

  public bool AverageDurationKnown
  {
    get => this.bool_1;
    set
    {
      if (this.bool_1 == value)
        return;
      this.bool_1 = value;
      this.OnPropertyChanged(nameof (AverageDurationKnown));
    }
  }

  public double Duration
  {
    get
    {
      return this.long_0 == 0L ? 0.0 : this.nullable_1 ?? TestPlanGridListener.GetTimeSinceStamp(this.long_0).TotalSeconds;
    }
  }

  public Verdict Verdict
  {
    get => this.verdict_0;
    set
    {
      if (this.verdict_0 == value)
        return;
      this.verdict_0 = value;
      this.OnPropertyChanged(nameof (Verdict));
    }
  }

  public TestPlanGridListener() => OpenTap.Log.RemoveSource(this.Log);

  private void method_0(
    TestStepList testStepList_0,
    TestPlanRun testPlanRun_0,
    IResultStore iresultStore_1,
    ILookup<Guid, TestStepRowItem> ilookup_0,
    bool bool_2 = true)
  {
    foreach (ITestStep step in (Collection<ITestStep>) testStepList_0)
    {
      bool flag = step.Enabled & bool_2;
      TestStepRowItem testStepRowItem = ilookup_0[step.Id].FirstOrDefault<TestStepRowItem>();
      if (testStepRowItem != null)
      {
        VerdictControlProvider.RegisterVerdict(step, Verdict.NotSet);
        step.OnPropertyChanged("Verdict");
        if (flag)
        {
          FlowViewModel flowViewModel = new FlowViewModel(step)
          {
            InitTimeStamp = this.long_0
          };
          testStepRowItem.Flow = flowViewModel;
          this.dictionary_0[step.Id] = flowViewModel;
          this.list_0.Add(flowViewModel);
          if (step.Enabled && iresultStore_1 != null)
            flowViewModel.ExpectedDuration = iresultStore_1.smethod_0(this.Log, new TestStepRun(step, testPlanRun_0.Id), ComponentSettings<EditorSettings>.Current.EstimationWindowLength);
        }
        else
          testStepRowItem.Flow = (FlowViewModel) null;
        this.method_0(step.ChildTestSteps, testPlanRun_0, iresultStore_1, ilookup_0, step.Enabled & flag);
      }
    }
  }

  public static TimeSpan GetTimeSpanFromStamps(long startStamp, long endStamp)
  {
    return Class40.smethod_0((double) (endStamp - startStamp) / (double) Stopwatch.Frequency);
  }

  public static TimeSpan GetTimeSinceStamp(long startStamp)
  {
    return Class40.smethod_0((double) (Stopwatch.GetTimestamp() - startStamp) / (double) Stopwatch.Frequency);
  }

  public void PushStart()
  {
    this.long_0 = Stopwatch.GetTimestamp();
    this.iresultStore_0 = (IResultStore) null;
    this.AverageDuration = new double?(0.0);
    this.nullable_1 = new double?();
    this.list_0.Clear();
    this.dictionary_0.Clear();
    this.IsRunning = false;
    this.AverageDurationKnown = false;
    this.Verdict = Verdict.NotSet;
  }

  public override void OnTestPlanRunStart(TestPlanRun planRun)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanGridListener.Class185 class185 = new TestPlanGridListener.Class185();
    // ISSUE: reference to a compiler-generated field
    class185.testPlanGridListener_0 = this;
    // ISSUE: reference to a compiler-generated field
    class185.testPlanRun_0 = planRun;
    this.PushStart();
    this.IsRunning = true;
    // ISSUE: reference to a compiler-generated field
    this.long_0 = class185.testPlanRun_0.StartTimeStamp;
    // ISSUE: reference to a compiler-generated field
    base.OnTestPlanRunStart(class185.testPlanRun_0);
    foreach (IResultStore iresultStore_0 in ComponentSettings<ResultSettings>.Current.OfType<IResultStore>().Where<IResultStore>((Func<IResultStore, bool>) (iresultStore_0 => iresultStore_0.IsConnected)))
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        class185.testPlanRun_0.WaitForResourcesOpened(TapThread.Current.AbortToken, (IResource) iresultStore_0);
        // ISSUE: reference to a compiler-generated field
        this.AverageDuration = iresultStore_0.smethod_1(this.Log, class185.testPlanRun_0, ComponentSettings<EditorSettings>.Current.EstimationWindowLength);
        this.AverageDurationKnown = this.AverageDuration.HasValue;
        this.iresultStore_0 = iresultStore_0;
        break;
      }
      catch
      {
      }
    }
    // ISSUE: reference to a compiler-generated field
    class185.testPlan_0 = this.Plan;
    // ISSUE: reference to a compiler-generated field
    class185.ilookup_0 = this.Grid.iterateDepthFirst().ToLookup<TestStepRowItem, Guid>((Func<TestStepRowItem, Guid>) (testStepRowItem_0 => testStepRowItem_0.Step.Id));
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvoke((Action) new Action(class185.method_0));
    this.nullable_1 = new double?();
    // ISSUE: reference to a compiler-generated method
    TapThread.Start(new Action(class185.method_1));
  }

  public void OnTestStepExecutionChanged(
    Guid stepId,
    TestStepRun stepRun,
    StepState newState,
    long changeTime)
  {
    FlowViewModel key = (FlowViewModel) null;
    this.dictionary_0.TryGetValue(stepId, out key);
    if (key == null)
      return;
    switch (newState)
    {
      case StepState.Idle:
        lock (this.object_0)
          this.dictionary_1.Remove(key);
        key.FlowStops.Add(changeTime);
        key.DoUpdate();
        key.IsRunning = false;
        break;
      case StepState.PrePlanRun:
        key.FlowTypes.Add(FlowEventType.PrePlanRun);
        key.FlowStarts.Add(changeTime);
        key.IsRunning = true;
        break;
      case StepState.Running:
        if (this.iresultStore_0 != null && this.iresultStore_0.IsConnected)
        {
          lock (this.object_0)
            this.dictionary_1[key] = stepRun;
        }
        key.FlowTypes.Add(FlowEventType.Running);
        key.FlowStarts.Add(changeTime);
        key.IsRunning = true;
        key.Started = true;
        key.LastStepStart = changeTime;
        break;
      case StepState.Deferred:
        key.FlowStops.Add(changeTime);
        key.FlowTypes.Add(FlowEventType.Defer);
        key.FlowStarts.Add(changeTime);
        break;
      case StepState.PostPlanRun:
        key.FlowTypes.Add(FlowEventType.PostPlanRun);
        key.FlowStarts.Add(changeTime);
        key.IsRunning = true;
        break;
    }
    key.PushUpdate();
    if (newState != StepState.Running || stepRun == null)
      return;
    this.Grid.AsyncBringIntoView(stepRun.TestStepId);
  }

  public override void OnTestStepRunCompleted(TestStepRun stepRun)
  {
    base.OnTestStepRunCompleted(stepRun);
    FlowViewModel flowViewModel;
    if (this.dictionary_0.TryGetValue(stepRun.TestStepId, out flowViewModel))
    {
      flowViewModel.LastStepStop = stepRun.StartTimeStamp + (long) ((double) Stopwatch.Frequency * stepRun.Duration.TotalSeconds);
      flowViewModel.StepDuration = stepRun.Duration;
    }
    ITestStep step = this.Plan.Steps.GetStep(stepRun.TestStepId);
    if (step == null)
      return;
    VerdictControlProvider.RegisterVerdict(step, stepRun.Verdict);
    step.OnPropertyChanged("");
  }

  public override void OnTestPlanRunCompleted(TestPlanRun planRun, Stream logStream)
  {
    base.OnTestPlanRunCompleted(planRun, logStream);
    this.nullable_1 = new double?(planRun.Duration.TotalSeconds);
    foreach (FlowViewModel flowViewModel in this.list_0)
    {
      flowViewModel.FinalDuration = planRun.Duration.TotalSeconds;
      flowViewModel.Ended = true;
    }
    this.IsRunning = false;
    this.Verdict = planRun.Verdict;
  }

  internal void method_1(bool bool_2, double double_0)
  {
    this.IsRunning = false;
    if (!this.nullable_1.HasValue)
      this.nullable_1 = new double?(double_0);
    if (!bool_2)
      return;
    this.Verdict = Verdict.Aborted;
  }
}
