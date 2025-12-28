// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.BackBuffered`1
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class BackBuffered<T>
{
  private List<T> list_0 = new List<T>();
  private List<T> list_1 = new List<T>();
  private bool bool_0;
  private object object_0 = new object();

  public int Count
  {
    get => this.list_0 == null ? this.list_1.Count : this.list_1.Count + this.list_0.Count;
  }

  public T Last()
  {
    lock (this.object_0)
      return this.list_1.Any<T>() ? this.list_1.Last<T>() : this.list_0.Last<T>();
  }

  public void Add(T value)
  {
    lock (this.object_0)
      this.list_1.Add(value);
  }

  public List<T> GetBackBuffer()
  {
    if (this.bool_0)
      throw new InvalidOperationException("A backbuffer already exists.");
    lock (this.object_0)
      Class24.smethod_2<List<T>>(ref this.list_0, ref this.list_1);
    this.bool_0 = true;
    return this.list_0;
  }

  public void EndBackbuffer()
  {
    lock (this.object_0)
    {
      this.list_0.AddRange((IEnumerable<T>) this.list_1);
      this.list_1.Clear();
      Class24.smethod_2<List<T>>(ref this.list_0, ref this.list_1);
      this.bool_0 = false;
    }
  }
}
