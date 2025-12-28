// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TestPlanStepFinder
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class TestPlanStepFinder : ITypeDataSearcherCacheInvalidated, ITypeDataSearcher
{
  private readonly string string_0 = Path.GetDirectoryName(Path.Combine(PluginManager.GetOpenTapAssembly().Location));
  private TapThread tapThread_0;
  private static readonly WorkQueue workQueue_0 = new WorkQueue((WorkQueue.Options) 0, "TestPlanDescriptions");
  private readonly object object_0 = new object();

  public IEnumerable<ITypeData> Types { get; private set; }

  private List<string> method_1()
  {
    return PathUtils.IterateDirectories(Path.Combine(this.string_0, "Packages"), "*.TapPlan", SearchOption.AllDirectories).ToList<string>();
  }

  public void Search()
  {
    List<string> stringList = this.method_1();
    List<ITypeData> itypeDataList = new List<ITypeData>();
    foreach (string str in stringList)
    {
      string testplan = str;
      if (!Path.GetFileNameWithoutExtension(testplan).StartsWith("."))
      {
        if (!Path.GetFileNameWithoutExtension(testplan).StartsWith("~last_session"))
        {
          try
          {
            using (new FileStream(Path.Combine(this.string_0, testplan), FileMode.Open))
              ;
            TestPlanReferenceTypeData testPlanReferenceTypeData = new TestPlanReferenceTypeData(FileSystemHelper.GetRelativePath(this.string_0, testplan));
            itypeDataList.Add((ITypeData) testPlanReferenceTypeData);
            TestPlanStepFinder.workQueue_0.EnqueueWork((Action) (() =>
            {
              try
              {
                using (FileStream fileStream = new FileStream(Path.Combine(this.string_0, testplan), FileMode.Open))
                {
                  string description = XDocument.Load((Stream) fileStream).Element((XName) "TestPlan").Element((XName) "OpenTap.Description")?.Value;
                  if (description == null)
                    return;
                  testPlanReferenceTypeData.UpdateDescription(description);
                }
              }
              catch
              {
              }
            }));
          }
          catch
          {
          }
        }
      }
    }
    this.Types = (IEnumerable<ITypeData>) itypeDataList;
  }

  private event EventHandler<TypeDataCacheInvalidatedEventArgs> cacheInvalidated;

  public event EventHandler<TypeDataCacheInvalidatedEventArgs> CacheInvalidated
  {
    add
    {
      lock (this.object_0)
      {
        if (this.eventHandler_0 == null)
          this.tapThread_0 = TapThread.Start((Action) (() =>
          {
            try
            {
              List<string> first = this.method_1();
              while (!TapThread.Current.AbortToken.IsCancellationRequested)
              {
                List<string> second = this.method_1();
                if (!first.SequenceEqual<string>((IEnumerable<string>) second))
                {
                  this.Search();
                  EventHandler<TypeDataCacheInvalidatedEventArgs> eventHandler0 = this.eventHandler_0;
                  if (eventHandler0 != null)
                    eventHandler0((object) this, new TypeDataCacheInvalidatedEventArgs());
                }
                first = second;
                TapThread.Sleep(5000);
              }
            }
            catch
            {
            }
          }), "");
        this.cacheInvalidated += value;
      }
    }
    remove
    {
      lock (this.object_0)
      {
        this.cacheInvalidated -= value;
        if (this.eventHandler_0 != null)
          return;
        this.tapThread_0.Abort();
      }
    }
  }
}
