// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.CsvConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Microsoft.CSharp.RuntimeBinder;
using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class CsvConverter : IValueConverter
{
  public Type ListType;
  public Type PrevType;
  public UnitAttribute UnitAttribute;

  public event EventHandler<Exception> ExceptionRaised;

  public event EventHandler ValueConvertedBack;

  private NumberFormatter method_0(CultureInfo cultureInfo_0)
  {
    return new NumberFormatter(cultureInfo_0, this.UnitAttribute);
  }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return DependencyProperty.UnsetValue;
    Type enumType = value is IEnumerable ? value.GetType() : throw new ArgumentException("value must be an IEnumerable", nameof (value));
    this.PrevType = enumType;
    return enumType.IsGenericType && enumType.GetEnumerableElementType().IsNumeric() ? (object) this.method_0(culture).FormatRange((IEnumerable) value) : (object) string.Join(culture.NumberFormat.NumberGroupSeparator + " ", ((IEnumerable) value).Cast<object>().Select<object, string>((Func<object, string>) (object_0 => object_0 != null ? StringConvertProvider.GetString(object_0, culture) : "NULL")));
  }

  public object ConvertBack(object _value, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      object obj = this.DoConvertBack(_value, targetType, parameter, culture);
      // ISSUE: reference to a compiler-generated field
      if (this.eventHandler_1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.eventHandler_1((object) this, new EventArgs());
      }
      return obj;
    }
    catch (Exception ex)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.eventHandler_0 != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.eventHandler_0((object) this, ex);
      }
      return Binding.DoNothing;
    }
  }

  public object DoConvertBack(
    object _value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    if (!(_value is string str))
      return DependencyProperty.UnsetValue;
    if (targetType == typeof (object))
      targetType = this.ListType;
    Type enumerableElementType = targetType.GetEnumerableElementType();
    if ((object) enumerableElementType == null)
      enumerableElementType = this.PrevType.GetEnumerableElementType();
    Type elementType = enumerableElementType;
    IEnumerable source1;
    if (elementType.IsNumeric())
    {
      source1 = (IEnumerable) ((ICombinedNumberSequence) this.method_0(culture).Parse(str)).CastTo(elementType);
    }
    else
    {
      string[] source2 = str.Split(new string[1]
      {
        culture.NumberFormat.NumberGroupSeparator
      }, StringSplitOptions.RemoveEmptyEntries);
      source1 = !elementType.IsEnum ? (IEnumerable) ((IEnumerable<string>) source2).Select<string, object>((Func<string, object>) (item => System.Convert.ChangeType((object) item, elementType))) : (IEnumerable) ((IEnumerable<string>) source2).Select<string, object>((Func<string, object>) (item => Enum.Parse(elementType, item)));
    }
    Type type1 = (Type) null;
    if (targetType.IsArray)
      elementType = targetType.GetElementType();
    else if (targetType.IsGenericType)
      type1 = targetType.GetGenericTypeDefinition();
    if (typeof (IEnumerable<>) == type1)
    {
      Type genarg = ((IEnumerable<Type>) targetType.GetGenericArguments()).FirstOrDefault<Type>();
      if (genarg != (Type) null && genarg.IsNumeric())
        return source1 is ICombinedNumberSequence icombinedNumberSequence ? (object) icombinedNumberSequence.CastTo(genarg) : (object) source1.Cast<object>().Select<object, object>((Func<object, object>) (object_0 => System.Convert.ChangeType(object_0, genarg)));
    }
    else if (source1.Cast<object>().IsLongerThan<object>(100000L))
      throw new Exception("Sequence is too large. (max number of elements is 100000).");
    if (targetType.IsArray)
    {
      Array instance = Array.CreateInstance(elementType, source1.Cast<object>().Count<object>());
      int num = 0;
      foreach (object obj in source1)
        instance.SetValue(System.Convert.ChangeType(obj, elementType), num++);
      return (object) instance;
    }
    if (targetType.DescendsTo(typeof (ReadOnlyCollection<>)))
    {
      object instance = Activator.CreateInstance(typeof (List<>).MakeGenericType(elementType), (object) source1);
      return Activator.CreateInstance(targetType, instance);
    }
    Type type2 = targetType.IsInterface ? this.ListType : targetType;
    if (type2.IsInterface || type2.IsAbstract)
      type2 = this.PrevType;
    object instance1 = Activator.CreateInstance(type2);
    if (instance1 is IList list)
    {
      foreach (object obj in source1)
        list.Add(obj);
    }
    else
    {
      object obj1 = instance1;
      foreach (object obj2 in source1)
      {
        // ISSUE: reference to a compiler-generated field
        if (CsvConverter.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CsvConverter.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (CsvConverter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CsvConverter.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) CsvConverter.\u003C\u003Eo__12.\u003C\u003Ep__0, obj1, obj2);
      }
    }
    return instance1;
  }
}
