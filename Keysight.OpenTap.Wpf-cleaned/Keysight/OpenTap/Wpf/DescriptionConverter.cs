// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.DescriptionConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class DescriptionConverter : ConverterBase
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (value == null)
      return (object) null;
    if (value is AnnotationCollection annotationCollection)
    {
      DisplayAttribute display = annotationCollection.Get<DisplayAttribute>(false, (object) null);
      IValueDescriptionAnnotation valueDescription = annotationCollection.Get<IValueDescriptionAnnotation>(false, (object) null);
      return (object) new ItemDescription()
      {
        Description = ((Func<string>) (() => display?.Description)),
        ValueDescription = ((Func<string>) (() => valueDescription?.Describe()))
      }.ToString();
    }
    Type type1 = value as Type;
    if ((object) type1 == null)
      type1 = value.GetType();
    Type type2 = type1;
    if (value is Enum)
    {
      string name = Enum.GetName(type2, value);
      MemberInfo memberInfo = ((IEnumerable<MemberInfo>) type2.GetMember(name)).FirstOrDefault<MemberInfo>();
      if (memberInfo != (MemberInfo) null && memberInfo.HasAttribute<DisplayAttribute>())
        return (object) memberInfo.GetDisplayAttribute().Description;
    }
    return (object) type2.GetDisplayAttribute().Description;
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
