// Decompiled with JetBrains decompiler
// Type: OpenTap.DataSerialization
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace OpenTap;

internal static class DataSerialization
{
  internal static void SerializeIData(List<IData> elements, BinaryWriter writer)
  {
    writer.Write(elements.Count);
    using (List<IData>.Enumerator enumerator1 = elements.GetEnumerator())
    {
label_2:
      while (enumerator1.MoveNext())
      {
        IData current1 = enumerator1.Current;
        writer.Write(current1 is IResultTable);
        writer.Write(elements.IndexOf(current1.Parent));
        writer.Write(current1.GetID());
        writer.Write(((IAttributedObject) current1).Name);
        writer.Write(((IAttributedObject) current1).ObjectType);
        writer.Write(((ICollection<IParameter>) current1.Parameters).Count);
        foreach (IParameter parameter in (IEnumerable<IParameter>) current1.Parameters)
        {
          writer.Write(((IAttributedObject) parameter).Name);
          writer.Write(parameter.Group);
          writer.Write(((IAttributedObject) parameter).ObjectType);
          writer.Write((int) parameter.Value.GetTypeCode());
          writer.Write(parameter.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        }
        if (current1 is IResultTable)
        {
          IResultTable iresultTable = current1 as IResultTable;
          writer.Write(iresultTable.Columns.Length);
          IResultColumn[] columns = iresultTable.Columns;
          int index = 0;
          while (true)
          {
            if (index < columns.Length)
            {
              IResultColumn iresultColumn = columns[index];
              TypeCode typeCode = Type.GetTypeCode(iresultColumn.Data.GetType().GetElementType());
              int count = 0;
              byte[] numArray = (byte[]) null;
              switch (typeCode)
              {
                case TypeCode.Object:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                  writer.Write(((IAttributedObject) iresultColumn).Name);
                  writer.Write(((IAttributedObject) iresultColumn).ObjectType);
                  writer.Write(iresultColumn.Data.Length);
                  writer.Write(count);
                  writer.Write(typeCode == TypeCode.Object ? 18 : (int) typeCode);
                  if (numArray != null)
                  {
                    writer.Write(numArray, 0, count);
                  }
                  else
                  {
                    switch (typeCode)
                    {
                      case TypeCode.Object:
                        IEnumerator enumerator2 = iresultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator2.MoveNext())
                          {
                            object current2 = enumerator2.Current;
                            if (current2 == null)
                            {
                              writer.Write(false);
                            }
                            else
                            {
                              writer.Write(true);
                              writer.Write(current2.ToString());
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
                        IEnumerator enumerator3 = iresultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator3.MoveNext())
                          {
                            object current3 = enumerator3.Current;
                            writer.Write((Decimal) current3);
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator3 is IDisposable disposable)
                            disposable.Dispose();
                        }
                      case TypeCode.DateTime:
                        IEnumerator enumerator4 = iresultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator4.MoveNext())
                          {
                            object current4 = enumerator4.Current;
                            writer.Write(((DateTime) current4).ToBinary());
                          }
                          break;
                        }
                        finally
                        {
                          if (enumerator4 is IDisposable disposable)
                            disposable.Dispose();
                        }
                      case TypeCode.String:
                        IEnumerator enumerator5 = iresultColumn.Data.GetEnumerator();
                        try
                        {
                          while (enumerator5.MoveNext())
                          {
                            object current5 = enumerator5.Current;
                            if (current5 == null)
                            {
                              writer.Write(false);
                            }
                            else
                            {
                              writer.Write(true);
                              writer.Write((string) current5);
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
                  count = Buffer.ByteLength(iresultColumn.Data);
                  numArray = new byte[count];
                  Buffer.BlockCopy(iresultColumn.Data, 0, (Array) numArray, 0, count);
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

  internal static List<IData> Deserialize(BinaryReader reader)
  {
    int num1 = reader.ReadInt32();
    List<IData> idataList = new List<IData>();
    List<int> intList = new List<int>();
    for (int index = 0; index < num1; ++index)
      idataList.Add((IData) new DataSerialization.Data());
    for (int index1 = 0; index1 < num1; ++index1)
    {
      bool flag = reader.ReadBoolean();
      intList.Add(reader.ReadInt32());
      long num2 = reader.ReadInt64();
      string name1 = reader.ReadString();
      string objectType1 = reader.ReadString();
      int num3 = reader.ReadInt32();
      List<IParameter> collection = new List<IParameter>();
      for (int index2 = 0; index2 < num3; ++index2)
      {
        string name2 = reader.ReadString();
        string group = reader.ReadString();
        string objtype = reader.ReadString();
        int num4 = reader.ReadInt32();
        string str = reader.ReadString();
        collection.Add((IParameter) new DataSerialization.ResultParameter2(group, name2, objtype, (IConvertible) Convert.ChangeType((object) str, (TypeCode) num4, (IFormatProvider) CultureInfo.InvariantCulture)));
      }
      if (flag)
      {
        int num5 = reader.ReadInt32();
        List<ResultColumn> resultColumnList = new List<ResultColumn>();
        for (int index3 = 0; index3 < num5; ++index3)
        {
          string name3 = reader.ReadString();
          string objectType2 = reader.ReadString();
          int length = reader.ReadInt32();
          int count = reader.ReadInt32();
          TypeCode typeCode = (TypeCode) reader.ReadInt32();
          Array instance = Array.CreateInstance(Utils.TypeOf(typeCode), length);
          switch (typeCode)
          {
            case TypeCode.Decimal:
              for (int index4 = 0; index4 < length; ++index4)
                ((Decimal[]) instance)[index4] = reader.ReadDecimal();
              break;
            case TypeCode.DateTime:
              for (int index5 = 0; index5 < length; ++index5)
                ((DateTime[]) instance)[index5] = DateTime.FromBinary(reader.ReadInt64());
              break;
            case TypeCode.String:
              for (int index6 = 0; index6 < length; ++index6)
              {
                if (reader.ReadBoolean())
                  ((string[]) instance)[index6] = reader.ReadString();
                else
                  ((string[]) instance)[index6] = (string) null;
              }
              break;
            default:
              Buffer.BlockCopy((Array) reader.ReadBytes(count), 0, instance, 0, count);
              break;
          }
          resultColumnList.Add((ResultColumn) new DataSerialization.ResultColumn2(name3, objectType2, instance));
        }
        idataList[index1] = (IData) new DataSerialization.ResultTable2(name1, objectType1, resultColumnList.ToArray(), intList[index1] != -1 ? idataList[intList[index1]] : (IData) null);
      }
      else
      {
        DataSerialization.Data data = idataList[index1] as DataSerialization.Data;
        data.ID = num2;
        data.Name = name1;
        data.ObjectType = objectType1;
        data.Parameters = (IParameters) new DataSerialization.Params((IEnumerable<IParameter>) collection);
      }
    }
    for (int index = 0; index < num1; ++index)
    {
      IData idata = idataList[index];
      if (intList[index] >= 0 && idata is DataSerialization.Data)
        ((DataSerialization.Data) idata).Parent = idataList[intList[index]];
    }
    return idataList;
  }

  internal class Data : IData, IAttributedObject
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public string ObjectType { get; set; }

    public IParameters Parameters { get; set; }

    public IData Parent { get; set; }

    public long GetID() => this.ID;

    public override string ToString()
    {
      return $"{this.Name} ({((ICollection<IParameter>) this.Parameters).Count})";
    }
  }

  private class Params(IEnumerable<IParameter> collection) : 
    List<IParameter>(collection),
    IParameters,
    IList<IParameter>,
    ICollection<IParameter>,
    IEnumerable<IParameter>,
    IEnumerable
  {
    public IConvertible this[string Name] => (IConvertible) null;
  }

  private class ResultTable2 : ResultTable, IAttributedObject
  {
    private string objectType;

    string IAttributedObject.ObjectType => this.objectType;

    internal ResultTable2(
      string name,
      string objectType,
      ResultColumn[] resultColumns,
      IData data)
      : base(name, resultColumns)
    {
      this.objectType = objectType;
      this.Parent = data;
    }
  }

  private class ResultColumn2 : ResultColumn, IAttributedObject
  {
    private string objectType;

    string IAttributedObject.ObjectType => this.objectType;

    public ResultColumn2(string name, string objectType, Array data)
      : base(name, data)
    {
      this.objectType = objectType;
    }
  }

  private class ResultParameter2 : ResultParameter, IAttributedObject
  {
    private string objtype;

    string IAttributedObject.ObjectType => this.objtype;

    public ResultParameter2(
      string group,
      string name,
      string objtype,
      IConvertible value,
      MetaDataAttribute metadata = null,
      int parentLevel = 0)
      : base(group, name, value, metadata, parentLevel)
    {
      this.objtype = objtype;
    }
  }
}
