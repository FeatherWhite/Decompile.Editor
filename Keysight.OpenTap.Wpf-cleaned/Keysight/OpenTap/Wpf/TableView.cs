// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TableView
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class TableView
{
  private string[] string_0;
  private AnnotationCollection[] annotationCollection_0;
  private AnnotationCollection _annotations;
  private bool bool_1;
  private TapSerializer tapSerializer_0 = new TapSerializer();

  public bool IsPluginItems { get; set; }

  private string method_0(object object_0)
  {
    if (object_0 == null)
      return (string) null;
    ITypeData typeData = TypeData.GetTypeData(object_0);
    string str;
    object obj;
    if (typeData != null && StringConvertProvider.TryGetString(object_0, ref str, (CultureInfo) null) && StringConvertProvider.TryFromString(str, typeData, (object) null, ref obj, (CultureInfo) null))
      return str;
    try
    {
      return this.tapSerializer_0.SerializeToString(object_0);
    }
    catch
    {
      return (string) null;
    }
  }

  private object method_1(string string_1, ITypeData itypeData_0)
  {
    object obj;
    if (StringConvertProvider.TryFromString(string_1, itypeData_0, (object) null, ref obj, (CultureInfo) null))
      return obj;
    try
    {
      return this.tapSerializer_0.DeserializeFromString(string_1, itypeData_0, true, (string) null);
    }
    catch
    {
      return (object) null;
    }
  }

  public string[][] GetMatrix()
  {
    string[][] matrix = new string[this.annotationCollection_0.Length + 1][];
    matrix[0] = new string[this.string_0.Length];
    for (int index = 0; index < this.string_0.Length; ++index)
      matrix[0][index] = this.string_0[index];
    if (this.bool_1)
    {
      for (int index1 = 1; index1 < matrix.Length; ++index1)
      {
        AnnotationCollection[] array = this.annotationCollection_0[index1 - 1].Get<IMembersAnnotation>(false, (object) null).Members.ToArray<AnnotationCollection>();
        matrix[index1] = new string[this.string_0.Length];
        for (int index2 = 0; index2 < this.string_0.Length; ++index2)
        {
          string name = this.string_0[index2];
          AnnotationCollection annotationCollection = ((IEnumerable<AnnotationCollection>) array).FirstOrDefault<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 =>
          {
            IMemberAnnotation imemberAnnotation = annotationCollection_0.Get<IMemberAnnotation>(false, (object) null);
            return (imemberAnnotation != null ? ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) imemberAnnotation.Member).GetFullName() : (string) null) == name;
          }));
          if (annotationCollection != null)
          {
            try
            {
              matrix[index1][index2] = this.method_0(annotationCollection.Get<IObjectValueAnnotation>(false, (object) null)?.Value);
            }
            catch
            {
            }
          }
        }
      }
    }
    else
    {
      object[] array = ((IEnumerable<AnnotationCollection>) this.annotationCollection_0).Select<AnnotationCollection, object>((Func<AnnotationCollection, object>) (annotationCollection_0 => annotationCollection_0.Get<IObjectValueAnnotation>(false, (object) null)?.Value)).ToArray<object>();
      for (int index = 1; index < matrix.Length; ++index)
        matrix[index] = new string[1]
        {
          this.method_0(array[index - 1])
        };
    }
    return matrix;
  }

  public bool SetMatrix(string[][] values, ITypeData[] types, bool breakOnResize)
  {
    ICollectionAnnotation icollectionAnnotation = this._annotations.Get<ICollectionAnnotation>(false, (object) null);
    int index1 = values.Length - 1;
    List<AnnotationCollection> list = icollectionAnnotation.AnnotatedElements.ToList<AnnotationCollection>();
    bool flag = false;
    if (list.Count > index1)
    {
      list.RemoveRange(index1, list.Count - index1);
      flag = true;
    }
    while (list.Count < index1)
    {
      if (types != null && types[list.Count + 1] != null && types[list.Count + 1].CanCreateInstance)
      {
        object instance = types[list.Count + 1].CreateInstance(Array.Empty<object>());
        list.Add(AnnotationCollection.Annotate(instance, Array.Empty<IAnnotation>()));
      }
      else
      {
        AnnotationCollection annotationCollection = icollectionAnnotation.NewElement();
        if (annotationCollection.Get<IObjectValueAnnotation>(false, (object) null).Value == null)
          throw new InvalidDataException("Unable to detect element types.");
        list.Add(annotationCollection);
      }
      flag = true;
    }
    if (flag)
    {
      icollectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) list;
      this._annotations.Write();
      this._annotations.Read();
      if (breakOnResize)
        return true;
    }
    this.annotationCollection_0 = icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
    string[] array = ((IEnumerable<string>) values[0]).Select<string, string>((Func<string, string>) (string_0 => string_0?.Trim())).ToArray<string>();
    int[] headers1to2 = new int[this.string_0.Length];
    for (int index2 = 0; index2 < this.string_0.Length; ++index2)
    {
      headers1to2[index2] = -1;
      for (int index3 = 0; index3 < array.Length; ++index3)
      {
        if (string.Compare(this.string_0[index2], array[index3]) == 0)
        {
          headers1to2[index2] = index3;
          break;
        }
      }
      if (headers1to2[index2] == -1)
      {
        for (int index4 = 0; index4 < array.Length; ++index4)
        {
          if (string.Compare(this.string_0[index2], array[index4], true) == 0)
            headers1to2[index2] = index4;
        }
      }
    }
    IEnumerable<string> strings = ((IEnumerable<string>) array).Where<string>((Func<string, int, bool>) ((string_0, int_0) => !((IEnumerable<int>) headers1to2).Contains<int>(int_0)));
    if (strings.Any<string>((Func<string, bool>) (string_0 => string_0 != null)))
      throw new InvalidDataException("Data contains unrecognizable headers: " + string.Join(", ", strings));
    for (int index5 = 1; index5 < values.Length; ++index5)
    {
      AnnotationCollection annotationCollection = this.annotationCollection_0[index5 - 1];
      if (this.bool_1)
      {
        foreach (AnnotationCollection member1 in annotationCollection.Get<IMembersAnnotation>(false, (object) null).Members)
        {
          IMemberData member = member1.Get<IMemberAnnotation>(false, (object) null)?.Member;
          if (member != null || GenericGui.FilterDefault2((IReflectionData) member))
          {
            int index6 = ((IEnumerable<string>) this.string_0).IndexWhen<string>((Func<string, bool>) (string_0 => string_0 == ((IReflectionData) member).Name || ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) member).GetFullName() == string_0));
            if (index6 != -1)
            {
              int index7 = headers1to2[index6];
              if (index7 != -1)
              {
                IObjectValueAnnotation iobjectValueAnnotation = member1.Get<IObjectValueAnnotation>(false, (object) null);
                string string_1 = values[index5][index7];
                if (string_1 != null)
                {
                  ITypeData reflectionInfo = member1.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo;
                  object obj = this.method_1(string_1, reflectionInfo);
                  if (obj != null)
                    iobjectValueAnnotation.Value = obj;
                  else if (iobjectValueAnnotation is IOwnedAnnotation iownedAnnotation)
                  {
                    iownedAnnotation.Write(member1.Source);
                    member1.Read();
                  }
                  else
                    member1.Write();
                }
              }
            }
          }
        }
      }
      else
      {
        string string_1 = values[index5][0];
        if (string_1 != null)
        {
          ITypeData reflectionInfo = annotationCollection.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo;
          IObjectValueAnnotation iobjectValueAnnotation = annotationCollection.Get<IObjectValueAnnotation>(false, (object) null);
          object obj = this.method_1(string_1, reflectionInfo);
          if (obj != null)
          {
            iobjectValueAnnotation.Value = obj;
            if (iobjectValueAnnotation is IOwnedAnnotation iownedAnnotation)
              iownedAnnotation.Write(this._annotations.Source);
            else
              annotationCollection.Write();
          }
          else
            continue;
        }
        else
          continue;
      }
      this.annotationCollection_0[index5 - 1].Write();
    }
    this._annotations.Write();
    this._annotations.Read();
    return false;
  }

  public TableView(AnnotationCollection _annotations)
    : this(_annotations, (ITypeData[]) null)
  {
  }

  public TableView(AnnotationCollection _annotations, ITypeData[] types)
  {
    this._annotations = _annotations;
    ICollectionAnnotation icollectionAnnotation = this._annotations.Get<ICollectionAnnotation>(false, (object) null);
    this.annotationCollection_0 = icollectionAnnotation != null ? icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>() : throw new Exception("Can only work on collection data");
    if (this.annotationCollection_0.Length == 0)
    {
      if (types != null)
        this.annotationCollection_0 = types[0] != null ? new AnnotationCollection[1]
        {
          AnnotationCollection.Annotate(types[0].CreateInstance(Array.Empty<object>()), Array.Empty<IAnnotation>())
        } : throw new Exception("Unable to create new elements");
      else
        this.annotationCollection_0 = new AnnotationCollection[1]
        {
          icollectionAnnotation.NewElement()
        };
      icollectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) this.annotationCollection_0;
      if (this.annotationCollection_0[0] == null)
        return;
    }
    this.bool_1 = ((IEnumerable<AnnotationCollection>) this.annotationCollection_0).Any<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 => annotationCollection_0.Get<IMembersAnnotation>(false, (object) null) != null));
    if (this.bool_1)
    {
      HashSet<string> source = new HashSet<string>();
      Dictionary<string, ITypeData> dictionary1 = new Dictionary<string, ITypeData>();
      Dictionary<string, double> orders = new Dictionary<string, double>();
      Dictionary<string, IMemberData> dictionary2 = new Dictionary<string, IMemberData>();
      foreach (AnnotationCollection annotationCollection in this.annotationCollection_0)
      {
        IMembersAnnotation imembersAnnotation = annotationCollection.Get<IMembersAnnotation>(false, (object) null);
        if (imembersAnnotation != null)
        {
          foreach (AnnotationCollection member1 in imembersAnnotation.Members)
          {
            DisplayAttribute displayAttribute = member1.Get<DisplayAttribute>(false, (object) null);
            IMemberData member2 = member1.Get<IMemberAnnotation>(false, (object) null)?.Member;
            if (member2 != null && GenericGui.FilterDefault2((IReflectionData) member2) && displayAttribute != null)
            {
              string fullName = displayAttribute.GetFullName();
              source.Add(fullName);
              dictionary1[fullName] = member1.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo;
              orders[fullName] = displayAttribute.Order;
              dictionary2[fullName] = member2;
            }
          }
        }
      }
      this.string_0 = new string[source.Count];
      IMemberData[] imemberDataArray = new IMemberData[source.Count];
      int index = 0;
      foreach (string key in source.OrderBy<string, string>((Func<string, string>) (string_0 => string_0)).OrderBy<string, double>((Func<string, double>) (string_0 => orders[string_0])).ToArray<string>())
      {
        this.string_0[index] = key;
        imemberDataArray[index] = dictionary2[key];
        ++index;
      }
    }
    else
      this.string_0 = new string[1]{ "Column" };
  }

  public static string SerializeMatrix(string[][] matrix)
  {
    StringBuilder stringBuilder = new StringBuilder();
    bool flag1 = true;
    foreach (string[] strArray in matrix)
    {
      if (!flag1)
        stringBuilder.AppendLine();
      else
        flag1 = false;
      bool flag2 = true;
      foreach (string string_1 in strArray)
      {
        if (!flag2)
          stringBuilder.Append('\t');
        else
          flag2 = false;
        string str = TableView.smethod_0(string_1);
        stringBuilder.Append(str);
      }
    }
    return stringBuilder.ToString();
  }

  public static string[][] UnserializeMatrix(string string_1)
  {
    // ISSUE: variable of a compiler-generated type
    TableView.\u003C\u003Ec__DisplayClass16_0 cDisplayClass160;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.stringBuilder_0 = new StringBuilder();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.output = new List<string[]>();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.currentLine = new List<string>();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass160.lastQuote = -1;
    for (int index = 0; index < string_1.Length; ++index)
    {
      // ISSUE: variable of a compiler-generated type
      TableView.\u003C\u003Ec__DisplayClass16_1 cDisplayClass161;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass161.char_0 = string_1[index];
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass161.char_0 == '\n')
      {
        if (TableView.smethod_4(ref cDisplayClass160))
        {
          TableView.smethod_5(ref cDisplayClass160, ref cDisplayClass161);
        }
        else
        {
          TableView.smethod_3(ref cDisplayClass160);
          TableView.smethod_2(ref cDisplayClass160);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass161.char_0 == '\t')
        {
          if (TableView.smethod_4(ref cDisplayClass160))
            TableView.smethod_5(ref cDisplayClass160, ref cDisplayClass161);
          else
            TableView.smethod_3(ref cDisplayClass160);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass161.char_0 == '"')
          {
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass160.lastQuote > -1)
            {
              if ((index + 1 < string_1.Length ? (int) string_1[index + 1] : 0) == 34)
              {
                TableView.smethod_5(ref cDisplayClass160, ref cDisplayClass161);
                ++index;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                cDisplayClass160.lastQuote = -1;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              cDisplayClass160.lastQuote = index;
            }
          }
          else
            TableView.smethod_5(ref cDisplayClass160, ref cDisplayClass161);
        }
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (cDisplayClass160.stringBuilder_0.Length > 0)
      TableView.smethod_3(ref cDisplayClass160);
    TableView.smethod_2(ref cDisplayClass160);
    // ISSUE: reference to a compiler-generated field
    return cDisplayClass160.output.ToArray();
  }

  private static string smethod_0(string string_1)
  {
    bool flag = false;
    switch (string_1)
    {
      case null:
        return "";
      case "":
        return "\"\"";
      default:
        if (string_1.Contains<char>('"'))
        {
          string_1 = string_1.Replace("\"", "\"\"");
          flag = true;
        }
        else if (string_1.Contains<char>('\t') || string_1.Contains<char>('\n'))
          flag = true;
        if (flag)
          string_1 = $"\"{string_1}\"";
        return string_1;
    }
  }

  private static string smethod_1(string string_1)
  {
    switch (string_1)
    {
      case "\"\"":
        return "";
      case "":
        return (string) null;
      default:
        return string_1.Replace("\"\"", "\"");
    }
  }

  [CompilerGenerated]
  internal static void smethod_2(
    ref TableView.\u003C\u003Ec__DisplayClass16_0 _param0)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    _param0.output.Add(_param0.currentLine.ToArray());
    // ISSUE: reference to a compiler-generated field
    _param0.currentLine.Clear();
  }

  [CompilerGenerated]
  internal static void smethod_3(
    ref TableView.\u003C\u003Ec__DisplayClass16_0 _param0)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    _param0.currentLine.Add(TableView.smethod_1(_param0.stringBuilder_0.ToString()));
    // ISSUE: reference to a compiler-generated field
    _param0.stringBuilder_0.Clear();
  }

  [CompilerGenerated]
  internal static bool smethod_4(
    ref TableView.\u003C\u003Ec__DisplayClass16_0 _param0)
  {
    // ISSUE: reference to a compiler-generated field
    return _param0.lastQuote >= 0;
  }

  [CompilerGenerated]
  internal static void smethod_5(
    ref TableView.\u003C\u003Ec__DisplayClass16_0 _param0,
    ref TableView.\u003C\u003Ec__DisplayClass16_1 _param1)
  {
    // ISSUE: reference to a compiler-generated field
    if (_param1.char_0 == '\r' && !TableView.smethod_4(ref _param0))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    _param0.stringBuilder_0.Append(_param1.char_0);
  }
}
