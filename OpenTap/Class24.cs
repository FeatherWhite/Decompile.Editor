// Decompiled with JetBrains decompiler
// Type: OpenTap.Class24
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

#nullable disable
namespace OpenTap;

internal static class Class24
{
  public static Action action_0 = (Action) (() => { });
  private static readonly HashSet<Timer> hashSet_0 = new HashSet<Timer>();
  private static HashSet<Class24.Struct5> hashSet_1 = new HashSet<Class24.Struct5>();

  public static Class24.Class25 smethod_0(this AnnotationCollection annotationCollection_0_1)
  {
    ICollectionAnnotation collectionAnnotation = annotationCollection_0_1.Get<ICollectionAnnotation>();
    if (collectionAnnotation != null)
      return (Class24.Class25) new Class24.Class26()
      {
        class25_0 = collectionAnnotation.AnnotatedElements.Select<AnnotationCollection, Class24.Class25>((Func<AnnotationCollection, Class24.Class25>) (annotationCollection_0_2 => annotationCollection_0_2.smethod_0())).ToArray<Class24.Class25>()
      };
    IMembersAnnotation membersAnnotation = annotationCollection_0_1.Get<IMembersAnnotation>();
    if (membersAnnotation != null)
    {
      (Class24.Class25, string)[] array = membersAnnotation.Members.Where<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0_3 =>
      {
        IAccessAnnotation accessAnnotation = annotationCollection_0_3.Get<IAccessAnnotation>();
        return accessAnnotation != null && !accessAnnotation.IsReadOnly;
      })).Select<AnnotationCollection, (Class24.Class25, string)>((Func<AnnotationCollection, (Class24.Class25, string)>) (annotationCollection_0_4 => (annotationCollection_0_4.smethod_0(), annotationCollection_0_4.Get<IMemberAnnotation>().Member.Name))).Where<(Class24.Class25, string)>((Func<(Class24.Class25, string), bool>) (valueTuple_0 => valueTuple_0.State != null)).ToArray<(Class24.Class25, string)>();
      return (Class24.Class25) new Class24.Class27()
      {
        dictionary_0 = ((IEnumerable<(Class24.Class25, string)>) array).ToDictionary<(Class24.Class25, string), string, Class24.Class25>((Func<(Class24.Class25, string), string>) (valueTuple_0 => valueTuple_0.Name), (Func<(Class24.Class25, string), Class24.Class25>) (valueTuple_0 => valueTuple_0.State))
      };
    }
    IStringValueAnnotation stringValueAnnotation = annotationCollection_0_1.Get<IStringValueAnnotation>();
    if (stringValueAnnotation != null)
      return (Class24.Class25) new Class24.Class28()
      {
        string_0 = stringValueAnnotation.Value
      };
    IObjectValueAnnotation objectValueAnnotation = annotationCollection_0_1.Get<IObjectValueAnnotation>();
    if (objectValueAnnotation == null)
      return (Class24.Class25) null;
    return (Class24.Class25) new Class24.Class29()
    {
      object_0 = objectValueAnnotation.Value
    };
  }

  public static void smethod_1(
    this AnnotationCollection annotationCollection_0,
    Class24.Class25 class25_0)
  {
    switch (class25_0)
    {
      case Class24.Class26 class26:
        ICollectionAnnotation collectionAnnotation = annotationCollection_0.Get<ICollectionAnnotation>();
        AnnotationCollection[] array = collectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
        List<AnnotationCollection> annotationCollectionList = new List<AnnotationCollection>();
        int index = 0;
        foreach (Class24.Class25 class25_0_1 in class26.class25_0)
        {
          AnnotationCollection annotationCollection = ((IEnumerable<AnnotationCollection>) array).ElementAtOrDefault<AnnotationCollection>(index);
          AnnotationCollection annotationCollection_0_1 = annotationCollection == null ? collectionAnnotation.NewElement() : annotationCollection;
          annotationCollection_0_1.smethod_1(class25_0_1);
          annotationCollectionList.Add(annotationCollection_0_1);
          ++index;
        }
        collectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) annotationCollectionList;
        break;
      case Class24.Class27 class27:
        using (IEnumerator<AnnotationCollection> enumerator = annotationCollection_0.Get<IMembersAnnotation>().Members.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnnotationCollection current = enumerator.Current;
            string name = current.Get<IMemberAnnotation>()?.Member.Name;
            Class24.Class25 class25_0_2;
            if (class27.dictionary_0.TryGetValue(name, out class25_0_2))
              current.smethod_1(class25_0_2);
          }
          break;
        }
      case Class24.Class28 class28:
        annotationCollection_0.Get<IStringValueAnnotation>().Value = class28.string_0;
        break;
      case Class24.Class29 class29:
        annotationCollection_0.Get<IObjectValueAnnotation>().Value = class29.object_0;
        break;
    }
  }

  public static void smethod_2<T>(ref T gparam_0, ref T gparam_1)
  {
    T obj = gparam_0;
    gparam_0 = gparam_1;
    gparam_1 = obj;
  }

  public static T smethod_3<T>(this T gparam_0, T gparam_1, T gparam_2) where T : IComparable<T>
  {
    if (gparam_0.CompareTo(gparam_1) < 0)
      return gparam_1;
    return gparam_0.CompareTo(gparam_2) > 0 ? gparam_2 : gparam_0;
  }

  public static T smethod_4<T>(T gparam_0) => gparam_0;

  private static T smethod_5<T, U>(this IEnumerable<T> ienumerable_0, Func<T, U> func_0, int int_0) where U : IComparable
  {
    if (!ienumerable_0.Any<T>())
      return default (T);
    T obj1 = ienumerable_0.FirstOrDefault<T>();
    U u1 = func_0(obj1);
    foreach (T obj2 in ienumerable_0.Skip<T>(1))
    {
      U u2 = func_0(obj2);
      if (u2.CompareTo((object) u1) * int_0 > 0)
      {
        obj1 = obj2;
        u1 = u2;
      }
    }
    return obj1;
  }

  public static T smethod_6<T, U>(this IEnumerable<T> ienumerable_0, Func<T, U> func_0) where U : IComparable
  {
    return ienumerable_0.smethod_5<T, U>(func_0, 1);
  }

  public static T smethod_7<T, U>(this IEnumerable<T> ienumerable_0, Func<T, U> func_0) where U : IComparable
  {
    return ienumerable_0.smethod_5<T, U>(func_0, -1);
  }

  public static IEnumerable<T> smethod_8<T>(this IEnumerable<T> ienumerable_0, int int_0)
  {
    List<T> list = ienumerable_0.ToList<T>();
    return list.Count - int_0 > 0 ? list.Take<T>(list.Count - int_0) : Enumerable.Empty<T>();
  }

  public static void smethod_9<T>(this IList<T> ilist_0, Predicate<T> predicate_0)
  {
    if (ilist_0 is List<T> objList)
    {
      objList.RemoveAll(predicate_0);
    }
    else
    {
      for (int index = ilist_0.Count - 1; index >= 0; --index)
      {
        if (predicate_0(ilist_0[index]))
          ilist_0.RemoveAt(index);
      }
    }
  }

  public static void smethod_10(this IList ilist_0, Predicate<object> predicate_0)
  {
    for (int index = ilist_0.Count - 1; index >= 0; --index)
    {
      if (predicate_0(ilist_0[index]))
        ilist_0.RemoveAt(index);
    }
  }

  private static void smethod_11<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> func_0,
    IList<T> ilist_0)
  {
    Class24.smethod_12<T>(ienumerable_0, func_0, ilist_0, (HashSet<T>) null);
  }

  private static void smethod_12<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> func_0,
    IList<T> ilist_0,
    HashSet<T> hashSet_2)
  {
    foreach (T obj in ienumerable_0)
    {
      if (hashSet_2 != null)
      {
        if (!hashSet_2.Contains(obj))
          hashSet_2.Add(obj);
        else
          continue;
      }
      ilist_0.Add(obj);
      IEnumerable<T> ienumerable_0_1 = func_0(obj);
      if (ienumerable_0_1 != null)
        Class24.smethod_12<T>(ienumerable_0_1, func_0, ilist_0, hashSet_2);
    }
  }

  public static List<T> smethod_13<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> func_0,
    bool bool_0 = false,
    List<T> list_0 = null)
  {
    if (list_0 != null)
      list_0.Clear();
    else
      list_0 = new List<T>();
    Class24.smethod_12<T>(ienumerable_0, func_0, (IList<T>) list_0, bool_0 ? new HashSet<T>() : (HashSet<T>) null);
    return list_0;
  }

  public static void smethod_14<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> func_0,
    ISet<T> iset_0)
  {
    foreach (T obj in ienumerable_0)
    {
      if (iset_0.Add(obj))
      {
        IEnumerable<T> ienumerable_0_1 = func_0(obj);
        if (ienumerable_0_1 != null)
          Class24.smethod_14<T>(ienumerable_0_1, func_0, iset_0);
      }
    }
  }

  public static IEnumerable<T> smethod_15<T>(T gparam_0, Func<T, T> func_0)
  {
    yield return gparam_0;
    while (true)
    {
      gparam_0 = func_0(gparam_0);
      yield return gparam_0;
    }
  }

  public static void smethod_16<T>(this IEnumerable<T> ienumerable_0, Action<T> action_1)
  {
    foreach (T obj in ienumerable_0)
      action_1(obj);
  }

  public static IEnumerable<T> smethod_17<T>(this IEnumerable<T> ienumerable_0, params T[] gparam_0)
  {
    return ienumerable_0.Concat<T>((IEnumerable<T>) gparam_0);
  }

  public static int smethod_18<T>(this IEnumerable<T> ienumerable_0, Func<T, bool> func_0)
  {
    int num = 0;
    foreach (T obj in ienumerable_0)
    {
      if (func_0(obj))
        return num;
      ++num;
    }
    return -1;
  }

  public static bool smethod_19<T>(this IEnumerable<T> ienumerable_0, long long_0)
  {
    foreach (T obj in ienumerable_0)
    {
      if (--long_0 < 0L)
        return true;
    }
    return false;
  }

  public static void smethod_20<T>(this IList<T> ilist_0, IEnumerable<T> ienumerable_0)
  {
    foreach (T obj in ienumerable_0)
      ilist_0.Add(obj);
  }

  public static HashSet<T> smethod_21<T>(this IEnumerable<T> ienumerable_0)
  {
    return new HashSet<T>(ienumerable_0);
  }

  public static HashSet<T> smethod_22<T>(
    this IEnumerable<T> ienumerable_0,
    IEqualityComparer<T> iequalityComparer_0)
  {
    return new HashSet<T>(ienumerable_0, iequalityComparer_0);
  }

  public static IEnumerable<T> smethod_23<T>(
    this IEnumerable<T> ienumerable_0,
    Func<T, bool> func_0)
  {
    IEnumerator<T> enumerator = ienumerable_0.GetEnumerator();
    while (enumerator.MoveNext())
    {
      T current = enumerator.Current;
      if (!func_0(current))
        yield return current;
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator = (IEnumerator<T>) null;
  }

  public static void smethod_24(int int_0, Action action_1)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Class24.Class33 class33 = new Class24.Class33();
    // ISSUE: reference to a compiler-generated field
    class33.action_0 = action_1;
    lock (Class24.hashSet_0)
    {
      // ISSUE: reference to a compiler-generated field
      class33.timer_0 = (Timer) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      class33.timer_0 = new Timer(new TimerCallback(class33.method_0), (object) null, int_0, -1);
      // ISSUE: reference to a compiler-generated field
      Class24.hashSet_0.Add(class33.timer_0);
    }
  }

  public static void smethod_25<T, U>(
    this Dictionary<T, U> dictionary_0,
    Dictionary<T, U> dictionary_1)
  {
    foreach (KeyValuePair<T, U> keyValuePair in dictionary_0)
      dictionary_1[keyValuePair.Key] = keyValuePair.Value;
  }

  public static IEnumerable<string> smethod_26(this string string_0, params char[] char_0)
  {
    HashSet<char> charSet = ((IEnumerable<char>) char_0).smethod_21<char>();
    int startIndex = 0;
    for (int index = 0; index < string_0.Length; ++index)
    {
      char c = string_0[index];
      if (!charSet.Contains(c))
        continue;
      string str = string_0.Substring(startIndex, index - startIndex);
      if (str.Length > 0)
        yield return str;
      yield return new string(c, 1);
      startIndex = index + 1;
    }
    if (startIndex < string_0.Length)
      yield return string_0.Substring(startIndex, string_0.Length - startIndex);
  }

  public static bool smethod_27(
    this TraceSource traceSource_0,
    object object_0,
    string string_0,
    params object[] object_1)
  {
    lock (Class24.hashSet_1)
    {
      Class24.Struct5 struct5 = new Class24.Struct5()
      {
        object_0 = object_0,
        traceSource_0 = traceSource_0
      };
      if (Class24.hashSet_1.Contains(struct5))
        return false;
      traceSource_0.Error(string_0, object_1);
      Class24.hashSet_1.Add(struct5);
      return true;
    }
  }

  public static void smethod_28(this Exception exception_0)
  {
    ExceptionDispatchInfo.Capture(exception_0).Throw();
  }

  public static Exception smethod_29(this Exception exception_0)
  {
    return exception_0 is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 1 ? exception_0 : exception_0.InnerException;
  }

  public static void smethod_30(this Exception exception_0)
  {
    if (exception_0 is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 1)
      exception_0.smethod_28();
    else
      (exception_0.InnerException ?? exception_0).smethod_28();
  }

  public static IEnumerable<string> smethod_31(string string_0)
  {
    FileStream fileStream = File.Open(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    StreamReader streamReader = new StreamReader((Stream) fileStream);
    string str;
    while ((str = streamReader.ReadLine()) != null)
      yield return str;
    // ISSUE: reference to a compiler-generated method
    this.method_1();
    streamReader = (StreamReader) null;
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    fileStream = (FileStream) null;
  }

  public static string smethod_32(this SecureString secureString_0)
  {
    if (secureString_0 == null)
      throw new ArgumentNullException("securePassword");
    IntPtr num = IntPtr.Zero;
    try
    {
      num = Marshal.SecureStringToGlobalAllocUnicode(secureString_0);
      return Marshal.PtrToStringUni(num);
    }
    finally
    {
      Marshal.ZeroFreeGlobalAllocUnicode(num);
    }
  }

  public static SecureString smethod_33(this string string_0)
  {
    SecureString secureString = new SecureString();
    foreach (char c in string_0)
      secureString.AppendChar(c);
    return secureString;
  }

  public static Type smethod_34(TypeCode typeCode_0)
  {
    switch (typeCode_0)
    {
      case TypeCode.Boolean:
        return typeof (bool);
      case TypeCode.Char:
        return typeof (char);
      case TypeCode.SByte:
        return typeof (sbyte);
      case TypeCode.Byte:
        return typeof (byte);
      case TypeCode.Int16:
        return typeof (short);
      case TypeCode.UInt16:
        return typeof (ushort);
      case TypeCode.Int32:
        return typeof (int);
      case TypeCode.UInt32:
        return typeof (uint);
      case TypeCode.Int64:
        return typeof (long);
      case TypeCode.UInt64:
        return typeof (ulong);
      case TypeCode.Single:
        return typeof (float);
      case TypeCode.Double:
        return typeof (double);
      case TypeCode.Decimal:
        return typeof (Decimal);
      case TypeCode.DateTime:
        return typeof (DateTime);
      case TypeCode.String:
        return typeof (string);
      default:
        return typeof (object);
    }
  }

  public static bool smethod_35(double double_0)
  {
    return !double.IsInfinity(double_0) && !double.IsNaN(double_0);
  }

  public static bool smethod_36(Version version_0, Version version_1)
  {
    return version_0 == (Version) null || version_0.Major == version_1.Major && version_0.Minor >= version_1.Minor;
  }

  public static T smethod_37<T>(this T gparam_0, T gparam_1, bool bool_0) where T : struct
  {
    int num1 = (ValueType) gparam_0 is Enum ? (int) Convert.ChangeType((object) gparam_0, typeof (int)) : throw new InvalidOperationException("T must be an enum");
    int num2 = (int) Convert.ChangeType((object) gparam_1, typeof (int));
    return (T) Enum.ToObject(typeof (T), !bool_0 ? num1 & ~num2 : num1 | num2);
  }

  private static double smethod_38(string string_0, ref int int_0)
  {
    double num1 = 0.0;
    int num2 = 1;
    bool flag1 = false;
    bool flag2 = false;
    double num3 = 1.0;
    while (int_0 < string_0.Length)
    {
      char ch = string_0[int_0];
      switch (ch)
      {
        case '-':
          if (num2 == -1 | flag1)
            return (double) num2 * num1 * num3;
          num2 = -1;
          break;
        case '.':
          if (flag2)
            return (double) num2 * num1 * num3;
          flag2 = true;
          break;
        default:
          int num4 = (int) ch - 48 /*0x30*/;
          switch (num4)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              num1 = num1 * 10.0 + (double) num4;
              if (flag2)
              {
                num3 *= 0.1;
                break;
              }
              break;
            default:
              return (double) num2 * num1 * num3;
          }
          break;
      }
      ++int_0;
    }
    return (double) num2 * num1 * num3;
  }

  public static int smethod_39(string string_0, string string_1)
  {
    if (string_0 == null || string_1 == null)
      return string.Compare(string_0, string_1);
    Math.Min(string_0.Length, string_1.Length);
    int index1 = 0;
    int index2 = 0;
    while (index1 != string_0.Length)
    {
      if (index2 == string_1.Length)
        return 1;
      int int_0_1 = index1;
      double num1 = Class24.smethod_38(string_0, ref int_0_1);
      int int_0_2 = index2;
      double num2 = Class24.smethod_38(string_1, ref int_0_2);
      if (int_0_1 != index1 && int_0_2 == index2)
        return -1;
      if (int_0_2 != index2 && int_0_1 == index1)
        return 1;
      if (int_0_1 != index1 && num1 != num2)
        return num1.CompareTo(num2);
      int num3 = string_0[index1].CompareTo(string_1[index2]);
      if (num3 != 0)
        return num3;
      ++index1;
      ++index2;
    }
    return index2 == string_1.Length ? 0 : -1;
  }

  public static void smethod_40<T>(this IList<T> ilist_0)
  {
    Random random = new Random();
    for (int index1 = 0; index1 < ilist_0.Count; ++index1)
    {
      int index2 = random.Next(0, ilist_0.Count);
      T obj = ilist_0[index1];
      ilist_0[index1] = ilist_0[index2];
      ilist_0[index2] = obj;
    }
  }

  public static bool smethod_41<T>(T gparam_0) where T : class => (object) gparam_0 == null;

  public static bool smethod_42<T>(T gparam_0) where T : class => (object) gparam_0 != null;

  public class Class25
  {
  }

  private class Class26 : Class24.Class25
  {
    public Class24.Class25[] class25_0;

    public override string ToString()
    {
      return $"[{string.Join(",", ((IEnumerable<Class24.Class25>) this.class25_0).Select<Class24.Class25, string>((Func<Class24.Class25, string>) (class25_0 => class25_0.ToString())))}]";
    }
  }

  private class Class27 : Class24.Class25
  {
    public Dictionary<string, Class24.Class25> dictionary_0;

    public override string ToString()
    {
      return $"{{{string.Join(", ", this.dictionary_0.Select<KeyValuePair<string, Class24.Class25>, string>((Func<KeyValuePair<string, Class24.Class25>, string>) (keyValuePair_0 => $"{keyValuePair_0.Key}: {keyValuePair_0.Value}")))}}}";
    }
  }

  private class Class28 : Class24.Class25
  {
    public string string_0;

    public override string ToString() => this.string_0;
  }

  private class Class29 : Class24.Class25
  {
    public object object_0;

    public override string ToString()
    {
      object object0 = this.object_0;
      string str;
      if (object0 == null)
      {
        str = (string) null;
      }
      else
      {
        str = object0.ToString();
        if (str != null)
          return str;
      }
      return "";
    }
  }

  private struct Struct5
  {
    public object object_0;
    public TraceSource traceSource_0;
  }
}
