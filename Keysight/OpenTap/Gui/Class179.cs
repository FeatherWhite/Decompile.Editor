// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class179
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal static class Class179
{
  public static List<ITestStep> smethod_0(IEnumerable<ITestStep> ienumerable_0)
  {
    List<ITestStep> testStepList = new List<ITestStep>();
    HashSet<ITestStep> hashSet = ienumerable_0.ToHashSet<ITestStep>();
    foreach (ITestStep testStep in hashSet)
    {
      ITestStepParent parent = testStep.Parent;
      testStepList.Add(testStep);
      for (; !(parent is TestPlan); parent = parent.Parent)
      {
        if (hashSet.Contains((ITestStep) parent))
        {
          testStepList.RemoveAt(testStepList.Count - 1);
          break;
        }
      }
    }
    return testStepList;
  }

  public static void smethod_1(IList<ITestStep> ilist_0, IEnumerable<ITestStep> ienumerable_0)
  {
    ILookup<string, ITestStep> lookup = Class24.smethod_13<ITestStep>((IEnumerable<ITestStep>) ilist_0, (Func<ITestStep, IEnumerable<ITestStep>>) (itestStep_0 => (IEnumerable<ITestStep>) itestStep_0.ChildTestSteps)).ToLookup<ITestStep, string, ITestStep>((Func<ITestStep, string>) (itestStep_0 => itestStep_0.Name), (Func<ITestStep, ITestStep>) (itestStep_0 => itestStep_0));
    HashSet<string> stringSet = new HashSet<string>();
    foreach (ITestStep testStep in ienumerable_0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Class179.Class181 class181 = new Class179.Class181();
      // ISSUE: reference to a compiler-generated field
      class181.itestStep_0 = testStep;
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (class181.itestStep_0.Name != class181.itestStep_0.GetFormattedName())
          continue;
      }
      catch
      {
      }
      // ISSUE: reference to a compiler-generated field
      string key = class181.itestStep_0.Name;
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (; (lookup.Contains(key) || stringSet.Contains(key)) && !lookup[key].Contains<ITestStep>(class181.itestStep_0); key = $"{class181.itestStep_0.Name} ({num})")
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (lookup[key].Count<ITestStep>(class181.func_0 ?? (class181.func_0 = new Func<ITestStep, bool>(class181.method_0))) > 0)
          ++num;
        else if (stringSet.Contains(key))
          ++num;
      }
      // ISSUE: reference to a compiler-generated field
      class181.itestStep_0.Name = key;
      stringSet.Add(key);
    }
  }

  public static void smethod_2(
    ITestStep itestStep_0,
    ITestStepParent itestStepParent_0,
    bool bool_0)
  {
    int int_0;
    if (!bool_0 && !(itestStepParent_0 is TestPlan))
    {
      int_0 = itestStepParent_0.Parent.ChildTestSteps.IndexOf(itestStepParent_0 as ITestStep) + 1;
      itestStepParent_0 = itestStepParent_0.Parent;
    }
    else
      int_0 = itestStepParent_0.ChildTestSteps.Count;
    Class179.smethod_3(itestStepParent_0, itestStep_0, int_0);
  }

  public static void smethod_3(ITestStepParent itestStepParent_0, ITestStep itestStep_0, int int_0)
  {
    if (ComponentSettings<EditorSettings>.Current.EnsureUniqueStepName)
      Class179.smethod_1((IList<ITestStep>) itestStepParent_0.ChildTestSteps, (IEnumerable<ITestStep>) new ITestStep[1]
      {
        itestStep_0
      });
    itestStepParent_0.ChildTestSteps.Insert(int_0, itestStep_0);
  }

  public static void smethod_4(
    IList<ITestStep> ilist_0,
    IList<ITestStep> ilist_1,
    ITestStep itestStep_0,
    int int_0)
  {
    if (ilist_0 != null && ilist_0.Contains(itestStep_0))
    {
      int index = ilist_0.IndexOf(itestStep_0);
      if (ilist_1.Contains(itestStep_0) && int_0 == index)
        return;
      itestStep_0.Parent = (ITestStepParent) null;
      ilist_0.RemoveAt(index);
      if (ilist_0 == ilist_1 && index < int_0)
        --int_0;
    }
    ilist_1.Insert(int_0, itestStep_0);
  }
}
