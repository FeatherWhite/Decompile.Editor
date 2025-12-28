// Decompiled with JetBrains decompiler
// Type: OpenTap.Memorizer`3
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OpenTap;

internal class Memorizer<ArgT, ResultT, MemorizerKey> : Memorizer, IMemorizer<ArgT, ResultT>
{
  public TimeSpan SoftSizeDecayTime = TimeSpan.FromSeconds(30.0);
  protected Func<ArgT, MemorizerKey> getKey;
  protected Func<ArgT, ResultT> getData = (Func<ArgT, ResultT>) (argt => (ResultT) (object) argt);
  private readonly Dictionary<MemorizerKey, DateTime> dictionary_0 = new Dictionary<MemorizerKey, DateTime>();
  private readonly Dictionary<MemorizerKey, ResultT> dictionary_1 = new Dictionary<MemorizerKey, ResultT>();
  private readonly Dictionary<MemorizerKey, Memorizer<ArgT, ResultT, MemorizerKey>.LockObject> dictionary_2 = new Dictionary<MemorizerKey, Memorizer<ArgT, ResultT, MemorizerKey>.LockObject>();
  private readonly Dictionary<MemorizerKey, object> dictionary_3 = new Dictionary<MemorizerKey, object>();
  public Memorizer.CyclicInvokeMode CylicInvokeResponse;

  public Func<MemorizerKey, object> Validator { get; set; }

  public ulong? MaxNumberOfElements { get; set; }

  public Memorizer(Func<ArgT, MemorizerKey> getKey = null, Func<ArgT, ResultT> extractData = null)
  {
    if (extractData != null)
      this.getData = extractData;
    this.getKey = getKey;
  }

  public void CheckConstraints()
  {
    do
      ;
    while (this.method_0() == Memorizer<ArgT, ResultT, MemorizerKey>.Status.Changed);
  }

  private Memorizer<ArgT, ResultT, MemorizerKey>.Status method_0()
  {
    lock (this.dictionary_1)
    {
      MemorizerKey min = this.dictionary_0.Keys.FindMin<MemorizerKey, DateTime>((Func<MemorizerKey, DateTime>) (gparam_0 => this.dictionary_0[gparam_0]));
      if ((object) min != null)
      {
        if (this.SoftSizeDecayTime < DateTime.UtcNow - this.dictionary_0[min])
        {
          this.dictionary_0.Remove(min);
          this.dictionary_1.Remove(min);
          return Memorizer<ArgT, ResultT, MemorizerKey>.Status.Changed;
        }
        if (this.MaxNumberOfElements.HasValue)
        {
          if ((ulong) this.dictionary_1.Count > this.MaxNumberOfElements.Value)
          {
            this.dictionary_0.Remove(min);
            this.dictionary_1.Remove(min);
            return Memorizer<ArgT, ResultT, MemorizerKey>.Status.Changed;
          }
        }
      }
    }
    return Memorizer<ArgT, ResultT, MemorizerKey>.Status.Unchanged;
  }

  private MemorizerKey method_1(ArgT gparam_0)
  {
    return this.getKey != null ? this.getKey(gparam_0) : (MemorizerKey) (object) gparam_0;
  }

  public ResultT this[ArgT gparam_0] => this.Invoke(gparam_0);

  public ResultT Invoke(ArgT gparam_0)
  {
    MemorizerKey key = this.method_1(gparam_0);
    if (this.Validator != null)
    {
      object objB = this.Validator(key);
      lock (this.dictionary_1)
      {
        if (this.dictionary_3.ContainsKey(key))
        {
          if (!object.Equals(this.dictionary_3[key], objB))
            this.Invalidate(gparam_0);
        }
        else
          this.dictionary_3[key] = objB;
      }
    }
    Memorizer<ArgT, ResultT, MemorizerKey>.LockObject lockObject;
    lock (this.dictionary_1)
    {
      this.dictionary_0[key] = DateTime.UtcNow;
      if (!this.dictionary_2.TryGetValue(key, out lockObject))
      {
        lockObject = new Memorizer<ArgT, ResultT, MemorizerKey>.LockObject();
        this.dictionary_2[key] = lockObject;
      }
    }
    lock (lockObject)
    {
      if (lockObject.IsLocked)
      {
        if (this.CylicInvokeResponse == Memorizer.CyclicInvokeMode.ThrowException)
          throw new Exception("Cyclic memorizer invoke detected.");
        return default (ResultT);
      }
      try
      {
        lockObject.IsLocked = true;
        lock (this.dictionary_1)
        {
          if (this.dictionary_1.ContainsKey(key))
            return this.dictionary_1[key];
        }
        ResultT resultT = this.getData(gparam_0);
        lock (this.dictionary_1)
        {
          this.dictionary_1[key] = resultT;
          int num = (int) this.method_0();
        }
        return resultT;
      }
      finally
      {
        lockObject.IsLocked = false;
      }
    }
  }

  public ResultT GetCached(ArgT gparam_0)
  {
    ResultT cached = default (ResultT);
    MemorizerKey key = this.method_1(gparam_0);
    lock (this.dictionary_1)
    {
      if (!this.dictionary_1.TryGetValue(key, out cached))
        return default (ResultT);
      this.dictionary_0[key] = DateTime.UtcNow;
    }
    return cached;
  }

  public void Add(ArgT gparam_0, ResultT value)
  {
    MemorizerKey key = this.method_1(gparam_0);
    lock (this.dictionary_1)
    {
      this.dictionary_0[key] = DateTime.UtcNow;
      this.dictionary_1[key] = value;
      int num = (int) this.method_0();
    }
  }

  public void Invalidate(ArgT value)
  {
    MemorizerKey key = this.method_1(value);
    lock (this.dictionary_1)
    {
      this.dictionary_1.Remove(key);
      this.dictionary_0.Remove(key);
      this.dictionary_3.Remove(key);
    }
  }

  public void InvalidateWhere(Func<MemorizerKey, ResultT, bool> predicate)
  {
    lock (this.dictionary_1)
    {
      List<MemorizerKey> memorizerKeyList = (List<MemorizerKey>) null;
      foreach (KeyValuePair<MemorizerKey, ResultT> keyValuePair in this.dictionary_1)
      {
        if (predicate(keyValuePair.Key, keyValuePair.Value))
        {
          if (memorizerKeyList == null)
            memorizerKeyList = new List<MemorizerKey>();
          memorizerKeyList.Add(keyValuePair.Key);
        }
      }
      if (memorizerKeyList == null)
        return;
      foreach (MemorizerKey key in memorizerKeyList)
      {
        this.dictionary_1.Remove(key);
        this.dictionary_0.Remove(key);
      }
    }
  }

  public List<ResultT> GetResults()
  {
    lock (this.dictionary_1)
      return this.dictionary_1.Values.ToList<ResultT>();
  }

  public List<MemorizerKey> GetKeys()
  {
    lock (this.dictionary_1)
      return this.dictionary_1.Keys.ToList<MemorizerKey>();
  }

  public void InvalidateAll()
  {
    lock (this.dictionary_1)
    {
      this.dictionary_1.Clear();
      this.dictionary_0.Clear();
      this.dictionary_3.Clear();
    }
  }

  private class LockObject
  {
    public bool IsLocked;
  }

  private enum Status
  {
    Changed,
    Unchanged,
  }
}
