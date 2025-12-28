// Decompiled with JetBrains decompiler
// Type: OpenTap.ResultStoreExtensions
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;

#nullable disable
namespace OpenTap;

internal static class ResultStoreExtensions
{
  public static double? EnsurePositiveAverageDurationForStep(
    this IResultStore store,
    TraceSource traceSource_0,
    TestStepRun testStepRun,
    int estimatedWindowLength)
  {
    double? nullable1 = new double?();
    double? nullable2;
    try
    {
      TimeSpan? averageDuration = store.GetAverageDuration(testStepRun, estimatedWindowLength);
      ref TimeSpan? local = ref averageDuration;
      nullable2 = local.HasValue ? new double?(local.GetValueOrDefault().TotalSeconds) : new double?();
      if (nullable2.HasValue)
      {
        double? nullable3 = nullable2;
        double num = 0.0;
        if (nullable3.GetValueOrDefault() < num & nullable3.HasValue)
          nullable2 = new double?();
      }
    }
    catch (Exception ex)
    {
      if (traceSource_0.ErrorOnce((object) testStepRun, "Unable to Get Average Duration, because: '{0}'", (object) ex))
        Log.Debug(traceSource_0, ex);
      nullable2 = new double?();
    }
    return nullable2;
  }

  public static double? EnsurePositiveAverageDurationForPlan(
    this IResultStore store,
    TraceSource traceSource_0,
    TestPlanRun testPlanRun,
    int estimatedWindowLength)
  {
    double? nullable1 = new double?();
    double? nullable2;
    try
    {
      TimeSpan? averageDuration = store.GetAverageDuration(testPlanRun, estimatedWindowLength);
      ref TimeSpan? local = ref averageDuration;
      nullable2 = local.HasValue ? new double?(local.GetValueOrDefault().TotalSeconds) : new double?();
      if (nullable2.HasValue)
      {
        double? nullable3 = nullable2;
        double num = 0.0;
        if (nullable3.GetValueOrDefault() < num & nullable3.HasValue)
          nullable2 = new double?();
      }
    }
    catch (Exception ex)
    {
      if (traceSource_0.ErrorOnce((object) testPlanRun, "Unable to Get Average Duration, because: '{0}'", (object) ex))
        Log.Debug(traceSource_0, ex);
      nullable2 = new double?();
    }
    return nullable2;
  }
}
