// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.BitMatrix
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class BitMatrix
{
  public bool DefaultValue;
  private int int_0;
  private int int_1;
  private int int_2;
  private int int_3;
  private uint[][] uint_0 = new uint[0][];

  public int Columns
  {
    get => this.int_2;
    set => this.int_2 = value;
  }

  public int Rows
  {
    get => this.int_3;
    set => this.int_3 = value;
  }

  public void Clear(bool state = false)
  {
    this.method_0();
    foreach (uint[] numArray in this.uint_0)
    {
      if (state)
      {
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = uint.MaxValue;
      }
      else
        Array.Clear((Array) numArray, 0, numArray.Length);
    }
  }

  public bool this[int int_4, int int_5]
  {
    get
    {
      this.method_0();
      this.method_1(int_4, int_5);
      int index1 = int_4 / 32 /*0x20*/;
      uint num = (uint) (1 << int_4 - index1 * 32 /*0x20*/);
      int index2 = int_5;
      return (this.uint_0[index1][index2] & num) > 0U;
    }
    set
    {
      this.method_0();
      this.method_1(int_4, int_5);
      int index1 = int_4 / 32 /*0x20*/;
      uint num = (uint) (1 << int_4 - index1 * 32 /*0x20*/);
      int index2 = int_5;
      if (value)
        this.uint_0[index1][index2] = this.uint_0[index1][index2] | num;
      else
        this.uint_0[index1][index2] = this.uint_0[index1][index2] & ~num;
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int int_5 = 0; int_5 < this.Rows; ++int_5)
    {
      for (int int_4 = 0; int_4 < this.Columns; ++int_4)
      {
        if (int_4 > 0)
          stringBuilder.Append(" ");
        stringBuilder.Append(this[int_4, int_5] ? 1 : 0);
      }
      stringBuilder.AppendLine();
    }
    return stringBuilder.ToString();
  }

  private void method_0()
  {
    if (this.int_2 == this.int_0 && this.int_3 == this.int_1)
      return;
    int newSize1 = (this.int_2 - 1) / 32 /*0x20*/ + 1;
    int newSize2 = this.int_3 - 1 + 1;
    if (this.uint_0.Length != newSize1)
    {
      int length = this.uint_0.Length;
      Array.Resize<uint[]>(ref this.uint_0, newSize1);
      for (int index = length; index < newSize1; ++index)
        this.uint_0[index] = new uint[newSize2];
    }
    for (int index = 0; index < newSize1; ++index)
    {
      if (this.uint_0[index].Length != newSize2)
        Array.Resize<uint>(ref this.uint_0[index], newSize2);
    }
    int int0 = this.int_0;
    int int1 = this.int_1;
    this.int_0 = this.int_2;
    this.int_1 = this.int_3;
    for (int int_4 = int0; int_4 < this.int_0; ++int_4)
    {
      for (int int_5 = 0; int_5 < this.int_1; ++int_5)
        this[int_4, int_5] = this.DefaultValue;
    }
    for (int int_5 = int1; int_5 < this.int_1; ++int_5)
    {
      for (int int_4 = 0; int_4 < this.int_0; ++int_4)
        this[int_4, int_5] = this.DefaultValue;
    }
  }

  private void method_1(int int_4, int int_5)
  {
    if (int_4 < 0 || int_4 >= this.Columns)
      throw new ArgumentOutOfRangeException("x");
    if (int_5 < 0 || int_5 >= this.Rows)
      throw new ArgumentOutOfRangeException("y");
  }

  public bool FullRow(int int_4)
  {
    this.method_0();
    if (this.uint_0.Length == 0)
      return true;
    for (int index = 0; index < this.uint_0.Length - 1; ++index)
    {
      if (this.uint_0[index][int_4] != uint.MaxValue)
        return false;
    }
    int num1 = this.Columns % 32 /*0x20*/;
    uint num2 = (uint) ((1 << num1) - 1);
    if (num1 == 0 && this.Columns > 0)
      num2 = uint.MaxValue;
    return ((int) this.uint_0[this.uint_0.Length - 1][int_4] & (int) num2) == (int) num2;
  }

  public int RowHits(int int_4)
  {
    this.method_0();
    int num = 0;
    int columns = this.Columns;
    for (int index = 0; index < this.uint_0.Length - 1; ++index)
    {
      uint uint_1 = this.uint_0[index][int_4];
      if (uint_1 == uint.MaxValue)
        num += 32 /*0x20*/;
      else
        num += BitMatrix.smethod_0(uint_1, 32 /*0x20*/);
      columns -= 32 /*0x20*/;
    }
    return num + BitMatrix.smethod_0(this.uint_0[this.uint_0.Length - 1][int_4], columns);
  }

  public int ColumnHits(int columnIndex)
  {
    this.method_0();
    int index = columnIndex / 32 /*0x20*/;
    uint num1 = (uint) (1 << columnIndex - index * 32 /*0x20*/);
    int num2 = 0;
    foreach (uint num3 in this.uint_0[index])
    {
      if ((num3 & num1) > 0U)
        ++num2;
    }
    return num2;
  }

  [CompilerGenerated]
  internal static int smethod_0(uint uint_1, int int_4)
  {
    int num = 0;
    for (uint index = uint_1; index != 0U && int_4 > 0; --int_4)
    {
      if ((1 & (int) index) != 0)
        ++num;
      index >>= 1;
    }
    return num;
  }
}
