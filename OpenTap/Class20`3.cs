// Decompiled with JetBrains decompiler
// Type: OpenTap.Class20`3
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace OpenTap;

internal class Class20<T, U, V> : Class19, Interface0<T, U>
{
  public TimeSpan timeSpan_0 = TimeSpan.FromSeconds(30.0);
  protected Func<T, V> func_0;
  protected Func<T, U> func_1 = (Func<T, U>) (gparam_0 => (U) (object) gparam_0);
  private readonly Dictionary<V, DateTime> dictionary_0 = new Dictionary<V, DateTime>();
  private readonly Dictionary<V, U> dictionary_1 = new Dictionary<V, U>();
  private readonly Dictionary<V, Class20<T, U, V>.Class22> dictionary_2 = new Dictionary<V, Class20<T, U, V>.Class22>();
  private readonly Dictionary<V, object> dictionary_3 = new Dictionary<V, object>();
  public Class19.Enum0 enum0_0;

  [CompilerGenerated]
  [SpecialName]
  public Func<V, object> method_0() => this.func_2;

  [CompilerGenerated]
  [SpecialName]
  public void method_1(Func<V, object> func_3) => this.func_2 = func_3;

  [CompilerGenerated]
  [SpecialName]
  public ulong? method_2() => this.nullable_0;

  [CompilerGenerated]
  [SpecialName]
  public void method_3(ulong? nullable_1) => this.nullable_0 = nullable_1;

  public Class20(Func<T, V> func_3 = null, Func<T, U> func_4 = null)
  {
    if (func_4 != null)
      this.func_1 = func_4;
    this.func_0 = func_3;
  }

  public void method_4()
  {
    do
      ;
    while (this.method_5() == Class20<T, U, V>.Enum1.const_0);
  }

  private Class20<T, U, V>.Enum1 method_5()
  {
    lock (this.dictionary_1)
    {
      V key = this.dictionary_0.Keys.smethod_7<V, DateTime>((Func<V, DateTime>) (gparam_0 => this.dictionary_0[gparam_0]));
      if ((object) key != null)
      {
        if (this.timeSpan_0 < DateTime.UtcNow - this.dictionary_0[key])
        {
          this.dictionary_0.Remove(key);
          this.dictionary_1.Remove(key);
          return Class20<T, U, V>.Enum1.const_0;
        }
        if (this.method_2().HasValue)
        {
          if ((ulong) this.dictionary_1.Count > this.method_2().Value)
          {
            this.dictionary_0.Remove(key);
            this.dictionary_1.Remove(key);
            return Class20<T, U, V>.Enum1.const_0;
          }
        }
      }
    }
    return Class20<T, U, V>.Enum1.const_1;
  }

  private V method_6(T gparam_0)
  {
    return this.func_0 != null ? this.func_0(gparam_0) : (V) (object) gparam_0;
  }

  public U this[T gparam_0] => this.Invoke(gparam_0);

  public U Invoke(T gparam_0)
  {
    V key = this.method_6(gparam_0);
    if (this.method_0() != null)
    {
      object objB = this.method_0()(key);
      lock (this.dictionary_1)
      {
        if (this.dictionary_3.ContainsKey(key))
        {
          if (!object.Equals(this.dictionary_3[key], objB))
            this.method_9(gparam_0);
        }
        else
          this.dictionary_3[key] = objB;
      }
    }
    Class20<T, U, V>.Class22 class22;
    lock (this.dictionary_1)
    {
      this.dictionary_0[key] = DateTime.UtcNow;
      if (!this.dictionary_2.TryGetValue(key, out class22))
      {
        class22 = new Class20<T, U, V>.Class22();
        this.dictionary_2[key] = class22;
      }
    }
    lock (class22)
    {
      if (class22.bool_0)
      {
        if (this.enum0_0 == Class19.Enum0.const_0)
          throw new Exception("Cyclic memorizer invoke detected.");
        return default (U);
      }
      try
      {
        class22.bool_0 = true;
        lock (this.dictionary_1)
        {
          if (this.dictionary_1.ContainsKey(key))
            return this.dictionary_1[key];
        }
        U u = this.func_1(gparam_0);
        lock (this.dictionary_1)
        {
          this.dictionary_1[key] = u;
          int num = (int) this.method_5();
        }
        return u;
      }
      finally
      {
        class22.bool_0 = false;
      }
    }
  }

  public U method_7(T gparam_0)
  {
    U u = default (U);
    V key = this.method_6(gparam_0);
    lock (this.dictionary_1)
    {
      if (!this.dictionary_1.TryGetValue(key, out u))
        return default (U);
      this.dictionary_0[key] = DateTime.UtcNow;
    }
    return u;
  }

  public void method_8(T gparam_0, U gparam_1)
  {
    V key = this.method_6(gparam_0);
    lock (this.dictionary_1)
    {
      this.dictionary_0[key] = DateTime.UtcNow;
      this.dictionary_1[key] = gparam_1;
      int num = (int) this.method_5();
    }
  }

  public void method_9(T gparam_0)
  {
    V key = this.method_6(gparam_0);
    lock (this.dictionary_1)
    {
      this.dictionary_1.Remove(key);
      this.dictionary_0.Remove(key);
      this.dictionary_3.Remove(key);
    }
  }

  public void method_10(Func<V, U, bool> func_3)
  {
    lock (this.dictionary_1)
    {
      List<V> vList = (List<V>) null;
      foreach (KeyValuePair<V, U> keyValuePair in this.dictionary_1)
      {
        if (func_3(keyValuePair.Key, keyValuePair.Value))
        {
          if (vList == null)
            vList = new List<V>();
          vList.Add(keyValuePair.Key);
        }
      }
      if (vList == null)
        return;
      foreach (V key in vList)
      {
        this.dictionary_1.Remove(key);
        this.dictionary_0.Remove(key);
      }
    }
  }

  public List<U> method_11()
  {
    lock (this.dictionary_1)
      return this.dictionary_1.Values.ToList<U>();
  }

  public List<V> method_12()
  {
    lock (this.dictionary_1)
      return this.dictionary_1.Keys.ToList<V>();
  }

  public void imethod_0()
  {
    lock (this.dictionary_1)
    {
      this.dictionary_1.Clear();
      this.dictionary_0.Clear();
      this.dictionary_3.Clear();
    }
  }

  private class Class22
  {
    public bool bool_0;
  }

  private enum Enum1
  {
    const_0,
    const_1,
  }
}
