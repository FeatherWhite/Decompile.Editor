// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.FlowViewModel
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class FlowViewModel
{
  public BackBuffered<FlowEventType> FlowTypes = new BackBuffered<FlowEventType>();
  public BackBuffered<long> FlowStarts = new BackBuffered<long>();
  public BackBuffered<long> FlowStops = new BackBuffered<long>();
  public long InitTimeStamp;
  public double FinalDuration;
  public bool Ended;
  public bool Started;
  public long LastStepStart;
  public long LastStepStop;
  public readonly Guid StepId;
  public readonly ITestStep Step;
  private ulong ulong_0;
  private ulong ulong_1;
  public Action pushUpdate;
  private bool bool_1;
  private int int_0 = 10;

  public double? ExpectedDuration { get; set; }

  public bool IsRunning { get; set; }

  public TimeSpan StepDuration { get; set; }

  public void PollUpdate()
  {
    if (this.ulong_1 >= this.ulong_0)
      return;
    this.ulong_1 = this.ulong_0;
    if (this.pushUpdate == null)
      return;
    this.pushUpdate();
  }

  public void PushUpdate() => ++this.ulong_0;

  public FlowViewModel(ITestStep step)
  {
    this.FinalDuration = 0.0;
    this.Step = step;
    this.StepId = step.Id;
  }

  public double? GetDuration()
  {
    if (this.FlowStarts.Count == 0)
      return new double?();
    return this.LastStepStart > this.LastStepStop ? new double?(TestPlanGridListener.GetTimeSinceStamp(this.LastStepStart).TotalSeconds) : new double?(this.StepDuration.TotalSeconds);
  }

  public void DoUpdate()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    FlowViewModel.Class183 class183_1 = new FlowViewModel.Class183();
    // ISSUE: reference to a compiler-generated field
    class183_1.flowViewModel_0 = this;
    if (this.bool_1 || this.FlowStops.Count < this.int_0 + 100)
      return;
    this.bool_1 = true;
    TimeSpan timeSpan = TestPlanGridListener.GetTimeSinceStamp(this.InitTimeStamp);
    double totalSeconds = timeSpan.TotalSeconds;
    // ISSUE: variable of a compiler-generated type
    FlowViewModel.Class183 class183_2 = class183_1;
    timeSpan = TimeSpan.FromSeconds(totalSeconds * 0.02);
    long ticks = timeSpan.Ticks;
    // ISSUE: reference to a compiler-generated field
    class183_2.long_0 = ticks;
    // ISSUE: reference to a compiler-generated method
    GuiHelpers.GuiInvokeAsync((Action) new Action(class183_1.method_0), priority: DispatcherPriority.Render);
  }

  public void updateBuffers(long minTicks)
  {
    List<FlowEventType> backBuffer1 = this.FlowTypes.GetBackBuffer();
    List<long> backBuffer2 = this.FlowStarts.GetBackBuffer();
    List<long> backBuffer3 = this.FlowStops.GetBackBuffer();
    try
    {
      int index1 = 1;
      int count1 = backBuffer3.Count;
      for (int index2 = 1; index2 < count1; ++index2)
      {
        if (backBuffer3[index1 - 1] + minTicks > backBuffer2[index2] && index2 != count1 - 1 && backBuffer1[index1 - 1] == backBuffer1[index2])
          backBuffer3[index1 - 1] = backBuffer3[index2];
        else if (backBuffer3[index1 - 1] + minTicks > backBuffer2[index2] && index2 != count1 - 1 && backBuffer3[index1 - 1] - backBuffer2[index1 - 1] < minTicks)
        {
          backBuffer1[index1 - 1] |= backBuffer1[index2];
          backBuffer3[index1 - 1] = backBuffer3[index2];
        }
        else if (backBuffer3[index1 - 1] + minTicks > backBuffer2[index2] && index2 != count1 - 1 && backBuffer3[index2] - backBuffer2[index2] < minTicks)
        {
          backBuffer1[index1 - 1] |= backBuffer1[index2];
          backBuffer3[index1 - 1] = backBuffer3[index2];
        }
        else
        {
          backBuffer1[index1] = backBuffer1[index2];
          backBuffer2[index1] = backBuffer2[index2];
          backBuffer3[index1] = backBuffer3[index2];
          ++index1;
        }
      }
      int count2 = backBuffer3.Count;
      backBuffer3.RemoveRange(index1, count2 - index1);
      backBuffer2.RemoveRange(index1, count2 - index1);
      backBuffer1.RemoveRange(index1, count2 - index1);
    }
    catch
    {
    }
    finally
    {
      this.bool_1 = false;
      this.int_0 = backBuffer3.Count;
      this.FlowTypes.EndBackbuffer();
      this.FlowStarts.EndBackbuffer();
      this.FlowStops.EndBackbuffer();
    }
  }
}
