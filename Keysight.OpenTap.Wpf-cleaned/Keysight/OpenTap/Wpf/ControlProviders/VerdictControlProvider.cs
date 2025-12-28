// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ControlProviders.VerdictControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf.ControlProviders;

public class VerdictControlProvider : IControlProvider, ITapPlugin
{
  private static readonly ConditionalWeakTable<ITestStep, VerdictControlProvider.VerdictBox> conditionalWeakTable_0 = new ConditionalWeakTable<ITestStep, VerdictControlProvider.VerdictBox>();

  public static void RegisterVerdict(ITestStep step, Verdict verdict)
  {
    VerdictControlProvider.conditionalWeakTable_0.GetOrCreateValue(step).Verdict = verdict;
  }

  public static Verdict GetVerdict(ITestStep step)
  {
    VerdictControlProvider.VerdictBox verdictBox;
    return VerdictControlProvider.conditionalWeakTable_0.TryGetValue(step, out verdictBox) ? verdictBox.Verdict : step.Verdict;
  }

  public double Order => 20.0;

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    if (annotation.Get<ReadOnlyViewAnnotation>(false, (object) null) == null)
      return (FrameworkElement) null;
    IMemberAnnotation imemberAnnotation = annotation.Get<IMemberAnnotation>(false, (object) null);
    if (imemberAnnotation == null || ((IReflectionData) imemberAnnotation.Member).Name != "Verdict" || !ReflectionDataExtensions.DescendsTo(((IReflectionAnnotation) imemberAnnotation).ReflectionInfo, typeof (Verdict)))
      return (FrameworkElement) null;
    UpdateMonitor updateMonitor = annotation.Get<UpdateMonitor>(true, (object) null);
    VerdictView verdictview = new VerdictView();
    VerdictView verdictView = verdictview;
    IObjectValueAnnotation iobjectValueAnnotation = annotation.Get<IObjectValueAnnotation>(false, (object) null);
    object obj;
    if (iobjectValueAnnotation == null)
    {
      obj = (object) null;
    }
    else
    {
      obj = iobjectValueAnnotation.Value;
      if (obj != null)
        goto label_7;
    }
    obj = (object) (Verdict) 0;
label_7:
    verdictView.DataContext = obj;
    updateMonitor?.RegisterSourceUpdated((FrameworkElement) verdictview, (Action) (() =>
    {
      if (!(annotation.Source is ITestStep source2))
        return;
      verdictview.DataContext = (object) VerdictControlProvider.GetVerdict(source2);
    }));
    return (FrameworkElement) verdictview;
  }

  private class VerdictBox
  {
    public Verdict Verdict;
  }
}
