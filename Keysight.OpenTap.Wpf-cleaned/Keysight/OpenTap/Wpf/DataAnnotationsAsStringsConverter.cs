// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.DataAnnotationsAsStringsConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class DataAnnotationsAsStringsConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    List<string> stringList = new List<string>();
    IEnumerable enumerable = (IEnumerable) value;
    if (value == null)
      return (object) null;
    foreach (AnnotationCollection annotationCollection in enumerable)
      stringList.Add(annotationCollection.Get<IStringValueAnnotation>(false, (object) null).Value ?? "(null)");
    return (object) stringList;
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
