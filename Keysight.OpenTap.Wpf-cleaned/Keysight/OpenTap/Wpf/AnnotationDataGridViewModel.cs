// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AnnotationDataGridViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class AnnotationDataGridViewModel
{
  private AnnotationCollection annotationCollection_0;

  public string Context { get; private set; }

  public List<AnnotationDataGridViewModel.ColumnData> Columns { get; private set; } = new List<AnnotationDataGridViewModel.ColumnData>();

  public IList<AnnotationCollection> Rows { get; private set; }

  public bool IsDynamicSize => !this.IsFixedSize;

  public bool IsFixedSize { get; private set; }

  public bool IsComplicatedType { get; private set; }

  public Type ElementType { get; private set; }

  public void LoadData(AnnotationCollection data)
  {
    this.annotationCollection_0 = data;
    this.Context = this.annotationCollection_0.ToString();
    IFixedSizeCollectionAnnotation collectionAnnotation = this.annotationCollection_0.Get<IFixedSizeCollectionAnnotation>(false, (object) null);
    this.IsFixedSize = (collectionAnnotation != null ? (collectionAnnotation.IsFixedSize ? 1 : 0) : 0) != 0 || this.annotationCollection_0.Get<IObjectValueAnnotation>(false, (object) null)?.Value == null;
    Type type1;
    if (!(this.annotationCollection_0.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo is TypeData reflectionInfo))
    {
      type1 = (Type) null;
    }
    else
    {
      Type type2 = reflectionInfo.Type;
      type1 = (object) type2 != null ? type2.GetEnumerableElementType() : (Type) null;
    }
    this.ElementType = type1;
    ICollectionAnnotation icollectionAnnotation = this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null);
    if (icollectionAnnotation == null)
      throw new Exception("Can only work on collection data");
    this.Columns = this.method_0();
    if (this.IsDynamicSize)
      this.Rows = (IList<AnnotationCollection>) icollectionAnnotation.AnnotatedElements.ToList<AnnotationCollection>();
    else
      this.Rows = (IList<AnnotationCollection>) icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
  }

  private List<AnnotationDataGridViewModel.ColumnData> method_0(bool bool_2 = true)
  {
    List<AnnotationDataGridViewModel.ColumnData> columnDataList = new List<AnnotationDataGridViewModel.ColumnData>();
    ICollectionAnnotation icollectionAnnotation = this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null);
    AnnotationCollection[] source1 = icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
    bool flag = false;
    if (source1.Length == 0)
    {
      if (!bool_2)
        return columnDataList;
      flag = true;
      source1 = new AnnotationCollection[1]
      {
        icollectionAnnotation.NewElement()
      };
      icollectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) source1;
      if (source1[0] == null)
        return columnDataList;
    }
    if (((IEnumerable<AnnotationCollection>) source1).Any<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 => annotationCollection_0.Get<IMembersAnnotation>(false, (object) null) != null)))
    {
      AnnotationCollection annotationCollection1 = icollectionAnnotation.NewElement();
      ITypeData reflectionInfo = annotationCollection1.Get<IReflectionAnnotation>(false, (object) null).ReflectionInfo;
      if (reflectionInfo != null)
      {
        if (!reflectionInfo.CanCreateInstance && annotationCollection1.Get<IObjectValueAnnotation>(false, (object) null)?.Value == null)
          this.IsComplicatedType = true;
        TypeData typeData_0 = reflectionInfo as TypeData;
        if (typeData_0 != null && PluginManager.GetPlugins(typeData_0.Type).Count<Type>((Func<Type, bool>) (type_0 => type_0 != typeData_0.Type)) > 0)
          this.IsComplicatedType = true;
      }
      HashSet<string> source2 = new HashSet<string>();
      Dictionary<string, DisplayAttribute> realNames = new Dictionary<string, DisplayAttribute>();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (AnnotationCollection annotationCollection2 in source1)
      {
        IMembersAnnotation imembersAnnotation = annotationCollection2.Get<IMembersAnnotation>(false, (object) null);
        if (imembersAnnotation != null)
        {
          foreach (AnnotationCollection member1 in imembersAnnotation.Members)
          {
            DisplayAttribute displayAttribute = member1.Get<DisplayAttribute>(false, (object) null);
            IMemberData member2 = member1.Get<IMemberAnnotation>(false, (object) null).Member;
            if (GenericGui.FilterDefault2((IReflectionData) member2) && displayAttribute != null)
            {
              string key = displayAttribute.GetFullName() + ((IReflectionData) member2.TypeDescriptor).Name;
              source2.Add(key);
              realNames[key] = displayAttribute;
              dictionary[key] = member1.ToString();
            }
          }
        }
      }
      foreach (string key in source2.OrderBy<string, string>((Func<string, string>) (string_0 => string_0)).OrderBy<string, double>((Func<string, double>) (string_0 => realNames[string_0].Order)).ToArray<string>())
        columnDataList.Add(new AnnotationDataGridViewModel.ColumnData()
        {
          Name = key,
          Display = realNames[key],
          Context = dictionary[key]
        });
    }
    else
    {
      DisplayAttribute displayAttribute = this.annotationCollection_0.Get<DisplayAttribute>(false, (object) null);
      string str1;
      if (displayAttribute == null)
      {
        str1 = (string) null;
      }
      else
      {
        str1 = displayAttribute.Name;
        if (str1 != null)
          goto label_30;
      }
      str1 = "";
label_30:
      string str2 = str1;
      AnnotationDataGridViewModel.ColumnData columnData = new AnnotationDataGridViewModel.ColumnData()
      {
        Name = str2,
        Display = this.annotationCollection_0.Get<DisplayAttribute>(false, (object) null),
        Context = this.annotationCollection_0.ToString()
      };
      columnDataList.Add(columnData);
    }
    if (flag)
      this.annotationCollection_0.Read();
    return columnDataList;
  }

  public AnnotationCollection CreateElement(AnnotationCollection element = null)
  {
    return this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null).NewElement();
  }

  public AnnotationCollection AddRow(AnnotationCollection newRow = null)
  {
    ICollectionAnnotation icollectionAnnotation = this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null);
    AnnotationCollection element = icollectionAnnotation.NewElement();
    if (newRow != null)
      element.Get<IObjectValueAnnotation>(false, (object) null).Value = newRow.Get<IObjectValueAnnotation>(false, (object) null).Value;
    this.SetRows((IEnumerable<AnnotationCollection>) icollectionAnnotation.AnnotatedElements.Append<AnnotationCollection>(element).ToArray<AnnotationCollection>(), true);
    this.UpdateColumns();
    return this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null).AnnotatedElements.LastOrDefault<AnnotationCollection>();
  }

  public void PushUpdate()
  {
    this.annotationCollection_0?.Get<UpdateMonitor>(true, (object) null)?.PushUpdate();
  }

  public void SetRows(IEnumerable<AnnotationCollection> cast, bool addingItem = false)
  {
    ICollectionAnnotation icollectionAnnotation = this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null);
    if (cast != null)
      icollectionAnnotation.AnnotatedElements = cast;
    this.annotationCollection_0.Write();
    if (addingItem)
      icollectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) Array.Empty<AnnotationCollection>();
    this.annotationCollection_0.Read();
    this.PushUpdate();
    if (this.IsDynamicSize)
      this.Rows = (IList<AnnotationCollection>) icollectionAnnotation.AnnotatedElements.ToList<AnnotationCollection>();
    else
      this.Rows = (IList<AnnotationCollection>) icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
  }

  public void Write() => this.SetRows((IEnumerable<AnnotationCollection>) this.Rows);

  public string CopyToString(HashSet<(int X, int Y)> positions)
  {
    string[][] matrix = new TableView(this.annotationCollection_0).GetMatrix();
    HashSet<int> source1 = new HashSet<int>();
    HashSet<int> source2 = new HashSet<int>();
    for (int index1 = 1; index1 < matrix.Length; ++index1)
    {
      string[] strArray = matrix[index1];
      for (int index2 = 0; index2 < strArray.Length; ++index2)
      {
        if (positions.Contains((index2, index1 - 1)))
        {
          source1.Add(index1);
          source2.Add(index2);
        }
        else
          matrix[index1][index2] = (string) null;
      }
    }
    if (source1.Count == 0)
      return (string) null;
    int sourceIndex1 = source1.Min();
    int num = source1.Max() - sourceIndex1 + 1;
    Array.Copy((Array) matrix, sourceIndex1, (Array) matrix, 0, num);
    Array.Resize<string[]>(ref matrix, num);
    int sourceIndex2 = source2.Min();
    int index3 = source2.Max() - sourceIndex2 + 1;
    object[] array1 = this.annotationCollection_0.Get<ICollectionAnnotation>(false, (object) null).AnnotatedElements.Select<AnnotationCollection, object>((Func<AnnotationCollection, object>) (annotationCollection_0 => annotationCollection_0.Get<IObjectValueAnnotation>(false, (object) null).Value)).ToArray<object>();
    for (int index4 = 0; index4 < num; ++index4)
    {
      string[] array2 = matrix[index4];
      Array.Copy((Array) array2, sourceIndex2, (Array) array2, 0, index3);
      if (this.IsComplicatedType)
      {
        Array.Resize<string>(ref array2, index3 + 1);
        ITypeData typeData = TypeData.GetTypeData(array1[index4 + sourceIndex1 - 1]);
        if (typeData.CanCreateInstance)
          array2[index3] = ((IReflectionData) typeData).Name;
      }
      else
        Array.Resize<string>(ref array2, index3);
      matrix[index4] = array2;
    }
    return TableView.SerializeMatrix(matrix);
  }

  public void PasteFromString(string rawData, HashSet<(int X, int Y)> positions)
  {
    string[][] source = TableView.UnserializeMatrix(rawData);
    if (source.Length == 0)
      return;
    ITypeData[] itypeDataArray = (ITypeData[]) null;
    if (this.IsComplicatedType)
    {
      itypeDataArray = ((IEnumerable<string[]>) source).Select<string[], ITypeData>((Func<string[], ITypeData>) (string_0 => TypeData.GetTypeData(((IEnumerable<string>) string_0).LastOrDefault<string>()))).ToArray<ITypeData>();
      if (((IEnumerable<ITypeData>) itypeDataArray).Any<ITypeData>((Func<ITypeData, bool>) (itypeData_0 => itypeData_0 != null)))
      {
        for (int index = 0; index < source.Length; ++index)
        {
          string[] array = source[index];
          Array.Resize<string>(ref array, array.Length - 1);
          source[index] = array;
        }
      }
    }
    int num1 = positions.Select<(int, int), int>((Func<(int, int), int>) (valueTuple_0 => valueTuple_0.X)).DefaultIfEmpty<int>(0).Min();
    int num2 = positions.Select<(int, int), int>((Func<(int, int), int>) (valueTuple_0 => valueTuple_0.Y)).DefaultIfEmpty<int>(0).Min() + 1;
    TableView tableView = new TableView(this.annotationCollection_0, itypeDataArray);
    string[][] matrix = tableView.GetMatrix();
    int length1 = Math.Max(source.Length + num2, matrix.Length);
    int length2 = Math.Max(source[0].Length + num1, matrix[0].Length);
    string[][] values = new string[length1][];
    ITypeData[] types = new ITypeData[length1];
    for (int index = 0; index < values.Length; ++index)
      values[index] = new string[length2];
    for (int index = 0; index < matrix[0].Length; ++index)
      values[0][index] = matrix[0][index];
    for (int index1 = 0; index1 < source.Length; ++index1)
    {
      string[] strArray1 = values[index1 + num2];
      string[] strArray2 = source[index1];
      if (itypeDataArray != null)
        types[index1 + num2] = itypeDataArray[index1];
      for (int index2 = 0; index2 < strArray2.Length; ++index2)
        strArray1[index2 + num1] = strArray2[index2];
    }
    if (tableView.SetMatrix(values, types, true))
      tableView.SetMatrix(values, types, false);
    this.SetRows((IEnumerable<AnnotationCollection>) null, true);
    this.method_1((IEnumerable<AnnotationDataGridViewModel.ColumnData>) this.method_0(false));
  }

  private void method_1(
    IEnumerable<AnnotationDataGridViewModel.ColumnData> ienumerable_0)
  {
    if (this.Columns.Select<AnnotationDataGridViewModel.ColumnData, string>((Func<AnnotationDataGridViewModel.ColumnData, string>) (columnData_0 => columnData_0.Name)).ToHashset<string>().SetEquals(ienumerable_0.Select<AnnotationDataGridViewModel.ColumnData, string>((Func<AnnotationDataGridViewModel.ColumnData, string>) (columnData_0 => columnData_0.Name))))
      return;
    this.Columns = ienumerable_0.ToList<AnnotationDataGridViewModel.ColumnData>();
  }

  public void UpdateColumns()
  {
    this.method_1((IEnumerable<AnnotationDataGridViewModel.ColumnData>) this.method_0(false));
  }

  public class ColumnData
  {
    public string Name { get; set; }

    public DisplayAttribute Display { get; set; }

    public string Context { get; set; } = "";
  }
}
