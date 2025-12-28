// Decompiled with JetBrains decompiler
// Type: OpenTap.Class42
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

#nullable disable
namespace OpenTap;

internal static class Class42
{
  internal static void smethod_0(List<IData> list_0, BinaryWriter binaryWriter_0)
  {
    binaryWriter_0.Write(list_0.Count);
    using (List<IData>.Enumerator enumerator1 = list_0.GetEnumerator())
    {
label_2:
      while (enumerator1.MoveNext())
      {
        IData current1 = enumerator1.Current;
        binaryWriter_0.Write(current1 is IResultTable);
        binaryWriter_0.Write(list_0.IndexOf(current1.Parent));
        binaryWriter_0.Write(current1.GetID());
        binaryWriter_0.Write(current1.Name);
        binaryWriter_0.Write(current1.ObjectType);
        binaryWriter_0.Write(current1.Parameters.Count);
        foreach (IParameter parameter in (IEnumerable<IParameter>) current1.Parameters)
        {
          binaryWriter_0.Write(parameter.Name);
          binaryWriter_0.Write(parameter.Group);
          binaryWriter_0.Write(parameter.ObjectType);
          binaryWriter_0.Write((int) parameter.Value.GetTypeCode());
          binaryWriter_0.Write(parameter.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        }
        if (current1 is IResultTable)
        {
          IResultTable resultTable = current1 as IResultTable;
          binaryWriter_0.Write(resultTable.Columns.Length);
          IResultColumn[] columns = resultTable.Columns;
          int index = 0;
          while (true)
          {
            if (index < columns.Length)
            {
              IResultColumn resultColumn = columns[index];
              TypeCode typeCode = Type.GetTypeCode(resultColumn.Data.GetType().GetElementType());
              int count = 0;
              byte[] numArray = (byte[]) null;
              switch (typeCode)
              {
                case TypeCode.Object:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                  binaryWriter_0.Write(resultColumn.Name);
                  binaryWriter_0.Write(resultColumn.ObjectType);
                  binaryWriter_0.Write(resultColumn.Data.Length);
                  binaryWriter_0.Write(count);
                  binaryWriter_0.Write(typeCode == TypeCode.Object ? 18 : (int) typeCode);
                  if (numArray != null)
                  {
                    binaryWriter_0.Write(numArray, 0, count);
                  }
                  else
                  {
                    switch (typeCode)
                    {
                      case TypeCode.Object:
                        IEnumerator enumerator2 = resultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator2.MoveNext())
                          {
                            object current2 = enumerator2.Current;
                            if (current2 == null)
                            {
                              binaryWriter_0.Write(false);
                            }
                            else
                            {
                              binaryWriter_0.Write(true);
                              binaryWriter_0.Write(current2.ToString());
                            }
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator2 is IDisposable disposable)
                            disposable.Dispose();
                        }
                      case TypeCode.Decimal:
                        IEnumerator enumerator3 = resultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator3.MoveNext())
                          {
                            object current3 = enumerator3.Current;
                            binaryWriter_0.Write((Decimal) current3);
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator3 is IDisposable disposable)
                            disposable.Dispose();
                        }
                      case TypeCode.DateTime:
                        IEnumerator enumerator4 = resultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator4.MoveNext())
                          {
                            object current4 = enumerator4.Current;
                            binaryWriter_0.Write(((DateTime) current4).ToBinary());
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator4 is IDisposable disposable)
                            disposable.Dispose();
                        }
                      case TypeCode.String:
                        IEnumerator enumerator5 = resultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator5.MoveNext())
                          {
                            object current5 = enumerator5.Current;
                            if (current5 == null)
                            {
                              binaryWriter_0.Write(false);
                            }
                            else
                            {
                              binaryWriter_0.Write(true);
                              binaryWriter_0.Write((string) current5);
                            }
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator5 is IDisposable disposable)
                            disposable.Dispose();
                        }
                    }
                  }
                  ++index;
                  continue;
                default:
                  count = Buffer.ByteLength(resultColumn.Data);
                  numArray = new byte[count];
                  Buffer.BlockCopy(resultColumn.Data, 0, (Array) numArray, 0, count);
                  goto case TypeCode.Object;
              }
            }
            else
              goto label_2;
          }
        }
      }
    }
  }

  internal static List<IData> smethod_1(BinaryReader binaryReader_0)
  {
    int num1 = binaryReader_0.ReadInt32();
    List<IData> dataList = new List<IData>();
    List<int> intList = new List<int>();
    for (int index = 0; index < num1; ++index)
      dataList.Add((IData) new Class42.Class43());
    for (int index1 = 0; index1 < num1; ++index1)
    {
      bool flag = binaryReader_0.ReadBoolean();
      intList.Add(binaryReader_0.ReadInt32());
      long long_1 = binaryReader_0.ReadInt64();
      string str1 = binaryReader_0.ReadString();
      string string_2_1 = binaryReader_0.ReadString();
      int num2 = binaryReader_0.ReadInt32();
      List<IParameter> ienumerable_0 = new List<IParameter>();
      for (int index2 = 0; index2 < num2; ++index2)
      {
        string string_2_2 = binaryReader_0.ReadString();
        string string_1 = binaryReader_0.ReadString();
        string string_3 = binaryReader_0.ReadString();
        int num3 = binaryReader_0.ReadInt32();
        string str2 = binaryReader_0.ReadString();
        ienumerable_0.Add((IParameter) new Class42.Class47(string_1, string_2_2, string_3, (IConvertible) Convert.ChangeType((object) str2, (TypeCode) num3, (IFormatProvider) CultureInfo.InvariantCulture)));
      }
      if (flag)
      {
        int num4 = binaryReader_0.ReadInt32();
        List<ResultColumn> resultColumnList = new List<ResultColumn>();
        for (int index3 = 0; index3 < num4; ++index3)
        {
          string string_1 = binaryReader_0.ReadString();
          string string_2_3 = binaryReader_0.ReadString();
          int length = binaryReader_0.ReadInt32();
          int count = binaryReader_0.ReadInt32();
          TypeCode typeCode_0 = (TypeCode) binaryReader_0.ReadInt32();
          Array instance = Array.CreateInstance(Class24.smethod_34(typeCode_0), length);
          switch (typeCode_0)
          {
            case TypeCode.Decimal:
              for (int index4 = 0; index4 < length; ++index4)
                ((Decimal[]) instance)[index4] = binaryReader_0.ReadDecimal();
              break;
            case TypeCode.DateTime:
              for (int index5 = 0; index5 < length; ++index5)
                ((DateTime[]) instance)[index5] = DateTime.FromBinary(binaryReader_0.ReadInt64());
              break;
            case TypeCode.String:
              for (int index6 = 0; index6 < length; ++index6)
              {
                if (binaryReader_0.ReadBoolean())
                  ((string[]) instance)[index6] = binaryReader_0.ReadString();
                else
                  ((string[]) instance)[index6] = (string) null;
              }
              break;
            default:
              Buffer.BlockCopy((Array) binaryReader_0.ReadBytes(count), 0, instance, 0, count);
              break;
          }
          resultColumnList.Add((ResultColumn) new Class42.Class46(string_1, string_2_3, instance));
        }
        dataList[index1] = (IData) new Class42.Class45(str1, string_2_1, resultColumnList.ToArray(), intList[index1] != -1 ? dataList[intList[index1]] : (IData) null);
      }
      else
      {
        Class42.Class43 class43 = dataList[index1] as Class42.Class43;
        class43.method_1(long_1);
        class43.method_2(str1);
        class43.method_3(string_2_1);
        class43.method_4((IParameters) new Class42.Class44((IEnumerable<IParameter>) ienumerable_0));
      }
    }
    for (int index = 0; index < num1; ++index)
    {
      IData data = dataList[index];
      if (intList[index] >= 0 && data is Class42.Class43)
        ((Class42.Class43) data).method_5(dataList[intList[index]]);
    }
    return dataList;
  }

  internal class Class43 : IData, IAttributedObject
  {
    [CompilerGenerated]
    [SpecialName]
    public long method_0() => this.long_0;

    [CompilerGenerated]
    [SpecialName]
    public void method_1(long long_1) => this.long_0 = long_1;

    public string Name { get; }

    [CompilerGenerated]
    [SpecialName]
    public void method_2(string string_2) => this.string_0 = string_2;

    public string ObjectType { get; }

    [CompilerGenerated]
    [SpecialName]
    public void method_3(string string_2) => this.string_1 = string_2;

    public IParameters Parameters { get; }

    [CompilerGenerated]
    [SpecialName]
    public void method_4(IParameters iparameters_1) => this.iparameters_0 = iparameters_1;

    public IData Parent { get; }

    [CompilerGenerated]
    [SpecialName]
    public void method_5(IData idata_1) => this.idata_0 = idata_1;

    public long GetID() => this.method_0();

    public override string ToString() => $"{this.Name} ({this.Parameters.Count})";
  }

  private class Class44(IEnumerable<IParameter> ienumerable_0) : 
    List<IParameter>(ienumerable_0),
    IParameters,
    IList<IParameter>,
    ICollection<IParameter>,
    IEnumerable<IParameter>,
    IEnumerable
  {
    public IConvertible this[string Name] => (IConvertible) null;
  }

  private class Class45 : ResultTable, IAttributedObject
  {
    private string string_0;

    [SpecialName]
    string IAttributedObject.get_ObjectType() => this.string_0;

    internal Class45(
      string string_1,
      string string_2,
      ResultColumn[] resultColumn_0,
      IData idata_0)
      : base(string_1, resultColumn_0)
    {
      this.string_0 = string_2;
      this.Parent = idata_0;
    }
  }

  private class Class46 : ResultColumn, IAttributedObject
  {
    private string string_0;

    [SpecialName]
    string IAttributedObject.get_ObjectType() => this.string_0;

    public Class46(string string_1, string string_2, Array array_0)
      : base(string_1, array_0)
    {
      this.string_0 = string_2;
    }
  }

  private class Class47 : ResultParameter, IAttributedObject
  {
    private string string_0;

    [SpecialName]
    string IAttributedObject.get_ObjectType() => this.string_0;

    public Class47(
      string string_1,
      string string_2,
      string string_3,
      IConvertible iconvertible_0,
      MetaDataAttribute metaDataAttribute_0 = null,
      int int_0 = 0)
      : base(string_1, string_2, iconvertible_0, metaDataAttribute_0, int_0)
    {
      this.string_0 = string_3;
    }
  }
}
