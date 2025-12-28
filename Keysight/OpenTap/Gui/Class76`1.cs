// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class76`1
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class76<T>
{
  [CompilerGenerated]
  [SpecialName]
  public List<T> method_0() => this.list_0;

  [CompilerGenerated]
  [SpecialName]
  public IList<T> method_2() => this.ilist_0;

  [CompilerGenerated]
  [SpecialName]
  public Func<T, IList<T>> method_4() => this.func_0;

  public Class76(List<T> list_1, IList<T> ilist_1, Func<T, IList<T>> func_1)
  {
    // ISSUE: reference to a compiler-generated method
    this.method_1(list_1);
    // ISSUE: reference to a compiler-generated method
    this.method_3(ilist_1);
    // ISSUE: reference to a compiler-generated method
    this.method_5(func_1);
    this.method_6();
  }

  public void method_6()
  {
    Class24.smethod_13<T>((IEnumerable<T>) this.method_2(), (Func<T, IEnumerable<T>>) this.method_4(), list_0: this.method_0());
  }
}
