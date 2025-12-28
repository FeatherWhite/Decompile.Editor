// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CircularBuffer`1
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class CircularBuffer<T> : IEnumerable<T>, IEnumerable
{
  private T[] gparam_0;
  private int int_1;
  private int int_2;
  private object object_0 = new object();

  public int MaxSize { get; set; }

  public int CurrentSize => this.int_2;

  public CircularBuffer(int length)
  {
    this.MaxSize = length;
    this.gparam_0 = new T[length];
  }

  public int Count => this.int_2;

  public void Add(T item)
  {
    lock (this.object_0)
      this.method_0(item);
  }

  private void method_0(T gparam_1)
  {
    if (this.int_1 == this.int_2)
    {
      if (this.int_2 < this.MaxSize)
        ++this.int_2;
      else
        this.int_1 = 0;
    }
    this.gparam_0[this.int_1] = gparam_1;
    ++this.int_1;
  }

  public void AddRange(IEnumerable<T> items)
  {
    lock (this.object_0)
    {
      foreach (T gparam_1 in items)
        this.method_0(gparam_1);
    }
  }

  private uint method_1(int int_3)
  {
    if (int_3 < 0)
      throw new InvalidOperationException("position cannot be negative");
    if (int_3 >= this.CurrentSize)
      throw new InvalidOperationException("position cannot be larger than the current size of the buffer.");
    uint num = (uint) (int_3 + this.int_1);
    if (num >= (uint) this.CurrentSize)
      num -= (uint) this.CurrentSize;
    return num;
  }

  public T this[int position]
  {
    get
    {
      lock (this.object_0)
        return this.gparam_0[(int) this.method_1(position)];
    }
    set
    {
      lock (this.object_0)
        this.gparam_0[(int) this.method_1(position)] = value;
    }
  }

  public IEnumerator<T> GetEnumerator()
  {
    for (int position = 0; position < this.CurrentSize; ++position)
      yield return this[position];
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  internal void Clear()
  {
    lock (this.object_0)
    {
      this.int_2 = 0;
      this.int_1 = 0;
    }
  }

  internal void SetMaxSize(int maxMessages)
  {
    lock (this.object_0)
    {
      List<T> list = this.ToList<T>();
      this.MaxSize = maxMessages;
      this.Clear();
      this.gparam_0 = new T[this.MaxSize];
      this.AddRange((IEnumerable<T>) list);
    }
  }

  internal IEnumerable<T> GetRange(int index, int count)
  {
    T[] destinationArray = new T[count];
    Array.Copy((Array) this.gparam_0, index, (Array) destinationArray, 0, count);
    return (IEnumerable<T>) destinationArray;
  }

  public IDisposable Lock() => (IDisposable) new CircularBuffer<T>.mutexLock(this.object_0);

  private class mutexLock : IDisposable
  {
    private object object_0;

    public mutexLock(object lockingObject)
    {
      this.object_0 = lockingObject;
      Monitor.Enter(this.object_0);
    }

    public void Dispose()
    {
      if (this.object_0 == null)
        return;
      Monitor.Exit(this.object_0);
      this.object_0 = (object) null;
    }
  }
}
