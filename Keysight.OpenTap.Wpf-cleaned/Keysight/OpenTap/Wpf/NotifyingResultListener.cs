// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.NotifyingResultListener
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Display("Notifying Result Listener", "Windows specific notifying result listener. It is capable of playing sounds and blinking the application main window.", null, -10000.0, false, null)]
public class NotifyingResultListener : ResultListener
{
  private SoundPlayer soundPlayer_0;

  [Display("Blink Icon", "Enable to blink the task bar icon when the test plan completes.", null, -10000.0, false, null)]
  public bool BlinkTaskBarIconWhenTestPlanCompletes { get; set; }

  [Display("Play Sound When", "Select when to play the sound.", null, -10000.0, false, null)]
  public NotifyingResultListener.PlanStatus PlaySoundWhen { get; set; }

  [Display("Sound", "Select which sound to play. Sounds can be configured in the windows Control Panel under the Sounds category.", null, -10000.0, false, null)]
  public NotifyingResultListener.SystemSound PlaySoundType { get; set; }

  [EnabledIf("PlaySoundType", new object[] {NotifyingResultListener.SystemSound.Custom}, HideIfDisabled = true)]
  [FilePath]
  [Display("Custom Sound File", "Select a custom sound to play. This must be a .wav file.", null, 1.0, false, null)]
  public string CustomSoundFile { get; set; }

  [Display("Stop Sound When", "Configure when to stop the custom sound playback.", null, 1.0, false, null)]
  [EnabledIf("PlaySoundType", new object[] {NotifyingResultListener.SystemSound.Custom}, HideIfDisabled = true)]
  public NotifyingResultListener.PlanStatus StopSoundWhen { get; set; } = NotifyingResultListener.PlanStatus.PlanStarted;

  private void method_0()
  {
    switch (this.PlaySoundType)
    {
      case NotifyingResultListener.SystemSound.Asterisk:
        SystemSounds.Asterisk.Play();
        break;
      case NotifyingResultListener.SystemSound.Beep:
        SystemSounds.Beep.Play();
        break;
      case NotifyingResultListener.SystemSound.Exclamation:
        SystemSounds.Exclamation.Play();
        break;
      case NotifyingResultListener.SystemSound.Hand:
        SystemSounds.Hand.Play();
        break;
      case NotifyingResultListener.SystemSound.Question:
        SystemSounds.Question.Play();
        break;
      case NotifyingResultListener.SystemSound.Custom:
        if (!File.Exists(this.CustomSoundFile))
          throw new FileNotFoundException($"CustomSoundFile path not found at '{this.CustomSoundFile}'.");
        (this.soundPlayer_0 = new SoundPlayer(this.CustomSoundFile)).Play();
        break;
    }
  }

  private void method_1() => this.soundPlayer_0?.Stop();

  protected virtual void onEnabledChanged(bool oldValue, bool newValue)
  {
    base.onEnabledChanged(oldValue, newValue);
    this.method_1();
  }

  private Window method_2() => Application.Current.MainWindow;

  public virtual void OnTestStepRunStart(TestStepRun stepRun)
  {
    base.OnTestStepRunStart(stepRun);
    if (this.PlaySoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.StepStarted))
      this.method_0();
    if (!this.StopSoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.StepStarted))
      return;
    this.method_1();
  }

  public virtual void OnTestStepRunCompleted(TestStepRun stepRun)
  {
    if (this.PlaySoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.StepCompleted))
      this.method_0();
    base.OnTestStepRunCompleted(stepRun);
    if (!this.StopSoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.StepCompleted))
      return;
    this.method_1();
  }

  public virtual void OnTestPlanRunStart(TestPlanRun planRun)
  {
    base.OnTestPlanRunStart(planRun);
    if (this.StopSoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.PlanStarted))
      this.method_1();
    if (!this.PlaySoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.PlanStarted))
      return;
    this.method_0();
  }

  private void method_3()
  {
    if (Application.Current?.Dispatcher == null)
      ((Resource) this).Log.ErrorOnce((object) this, "Blink cannot be used in this context.");
    else
      GuiHelpers.GuiInvokeAsync((Action) (() =>
      {
        Window window = this.method_2();
        if (window != null)
        {
          IntPtr handle = new WindowInteropHelper(window).Handle;
          if (handle != IntPtr.Zero)
          {
            WindowUtility.Flash(handle);
            return;
          }
        }
        ((Resource) this).Log.ErrorOnce((object) this, "Blink cannot be used in this context.");
      }));
  }

  public virtual void OnTestPlanRunCompleted(TestPlanRun planRun, Stream logStream)
  {
    if (this.PlaySoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.PlanCompleted))
      this.method_0();
    base.OnTestPlanRunCompleted(planRun, logStream);
    if (this.BlinkTaskBarIconWhenTestPlanCompletes)
      this.method_3();
    if (!this.StopSoundWhen.HasFlag((Enum) NotifyingResultListener.PlanStatus.PlanCompleted))
      return;
    this.method_1();
  }

  public NotifyingResultListener() => ((Resource) this).Name = "Notify";

  public enum SystemSound
  {
    Asterisk,
    Beep,
    Exclamation,
    Hand,
    Question,
    Custom,
  }

  [Flags]
  public enum PlanStatus
  {
    [Display("Plan Started", null, null, -10000.0, false, null)] PlanStarted = 4,
    [Display("Plan Completed", null, null, -10000.0, false, null)] PlanCompleted = 8,
    [Display("Step Started", null, null, -10000.0, false, null)] StepStarted = 1,
    [Display("Step Completed", null, null, -10000.0, false, null)] StepCompleted = 2,
  }
}
