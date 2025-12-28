// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class198`1
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class198<T>
{
  private readonly LinkedList<T> linkedList_0 = new LinkedList<T>();
  private readonly LinkedList<T> linkedList_1 = new LinkedList<T>();

  public static T smethod_0(LinkedList<T> linkedList_2)
  {
    T obj = linkedList_2.Last<T>();
    linkedList_2.RemoveLast();
    return obj;
  }

  public static void smethod_1(LinkedList<T> linkedList_2, T gparam_0)
  {
    linkedList_2.AddLast(gparam_0);
  }

  [SpecialName]
  public IEnumerable<T> method_0()
  {
    return this.linkedList_0.Concat<T>((IEnumerable<T>) this.linkedList_1);
  }

  [CompilerGenerated]
  [SpecialName]
  public int method_1() => this.int_0;

  [CompilerGenerated]
  [SpecialName]
  public void method_2(int int_1) => this.int_0 = int_1;

  [SpecialName]
  public int method_3() => this.linkedList_1.Count;

  [SpecialName]
  public int method_4() => this.linkedList_0.Count;

  public Class198() => this.method_2(1000);

  public void method_5(T gparam_0)
  {
    lock (this)
    {
      Class198<T>.smethod_1(this.linkedList_0, gparam_0);
      while (this.linkedList_0.Count > this.method_1())
        this.linkedList_0.RemoveFirst();
      this.method_8();
    }
  }

  public T method_6(T gparam_0)
  {
    lock (this)
    {
      T obj = Class198<T>.smethod_0(this.linkedList_0);
      Class198<T>.smethod_1(this.linkedList_1, gparam_0);
      return obj;
    }
  }

  public T method_7(T gparam_0)
  {
    lock (this)
    {
      T obj = Class198<T>.smethod_0(this.linkedList_1);
      Class198<T>.smethod_1(this.linkedList_0, gparam_0);
      return obj;
    }
  }

  public void method_8()
  {
    lock (this)
      this.linkedList_1.Clear();
  }

  public void method_9()
  {
    lock (this)
      this.linkedList_0.Clear();
  }

  public void method_10()
  {
    this.method_8();
    this.method_9();
  }
}
