// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.RunCompletedOverlay
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class RunCompletedOverlay : UserControl, IComponentConnector
{
  public static readonly DependencyProperty VerdictProperty = DependencyProperty.Register(nameof (Verdict), typeof (Verdict), typeof (RunCompletedOverlay), (PropertyMetadata) new UIPropertyMetadata((object) Verdict.NotSet, new PropertyChangedCallback(RunCompletedOverlay.smethod_0)));
  internal Border border_0;
  internal TextBlock textBlock_0;
  private bool bool_0;

  private static void smethod_0(
    DependencyObject dependencyObject_0,
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    if (!(dependencyObject_0 is RunCompletedOverlay completedOverlay))
      return;
    completedOverlay.OnVerdictChanged((Verdict) dependencyPropertyChangedEventArgs_0.OldValue, (Verdict) dependencyPropertyChangedEventArgs_0.NewValue);
  }

  public Verdict Verdict
  {
    get => (Verdict) this.GetValue(RunCompletedOverlay.VerdictProperty);
    set => this.SetValue(RunCompletedOverlay.VerdictProperty, (object) value);
  }

  public RunCompletedOverlay()
  {
    this.InitializeComponent();
    this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeOut"]);
  }

  protected virtual void OnVerdictChanged(Verdict oldValue, Verdict newValue)
  {
    switch (newValue)
    {
      case Verdict.NotSet:
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeOut"], HandoffBehavior.Compose);
        break;
      case Verdict.Pass:
        this.textBlock_0.Foreground = (Brush) this.Resources[(object) "passColor"];
        this.textBlock_0.Text = "Passed";
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeIn"]);
        break;
      case Verdict.Inconclusive:
        this.textBlock_0.Foreground = (Brush) this.Resources[(object) "warningColor"];
        this.textBlock_0.Text = "Inconclusive";
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeIn"]);
        break;
      case Verdict.Fail:
        this.textBlock_0.Foreground = (Brush) this.Resources[(object) "failColor"];
        this.textBlock_0.Text = "Failed";
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeIn"]);
        break;
      case Verdict.Aborted:
        this.textBlock_0.Foreground = (Brush) this.Resources[(object) "Tap.Wait.Brush"];
        this.textBlock_0.Text = "Aborted";
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeIn"]);
        break;
      case Verdict.Error:
        this.textBlock_0.Foreground = (Brush) this.Resources[(object) "failColor"];
        this.textBlock_0.Text = "Error";
        this.BeginStoryboard((Storyboard) this.Resources[(object) "FadeIn"]);
        break;
    }
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    Application.LoadComponent((object) this, new Uri("/Editor;component/runcompletedoverlay.xaml", UriKind.Relative));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.textBlock_0 = (TextBlock) target;
    }
    else
      this.border_0 = (Border) target;
  }
}
