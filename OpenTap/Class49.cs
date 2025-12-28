// Decompiled with JetBrains decompiler
// Type: OpenTap.Class49
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;

#nullable disable
namespace OpenTap;

internal static class Class49
{
  public static double? smethod_0(
    this IResultStore iresultStore_0,
    TraceSource traceSource_0,
    TestStepRun testStepRun_0,
    int int_0)
  {
    double? nullable1 = new double?();
    double? nullable2;
    try
    {
      TimeSpan? averageDuration = iresultStore_0.GetAverageDuration(testStepRun_0, int_0);
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
      if (traceSource_0.smethod_27((object) testStepRun_0, "Unable to Get Average Duration, because: '{0}'", (object) ex))
        traceSource_0.Debug(ex);
      nullable2 = new double?();
    }
    return nullable2;
  }

  public static double? smethod_1(
    this IResultStore iresultStore_0,
    TraceSource traceSource_0,
    TestPlanRun testPlanRun_0,
    int int_0)
  {
    double? nullable1 = new double?();
    double? nullable2;
    try
    {
      TimeSpan? averageDuration = iresultStore_0.GetAverageDuration(testPlanRun_0, int_0);
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
      if (traceSource_0.smethod_27((object) testPlanRun_0, "Unable to Get Average Duration, because: '{0}'", (object) ex))
        traceSource_0.Debug(ex);
      nullable2 = new double?();
    }
    return nullable2;
  }
}
