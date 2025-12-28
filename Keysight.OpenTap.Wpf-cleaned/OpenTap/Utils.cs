// Decompiled with JetBrains decompiler
// Type: OpenTap.Utils
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

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

internal static class Utils
{
  public static Action ActionDefault = (Action) (() => { });
  private static readonly HashSet<Timer> hashSet_0 = new HashSet<Timer>();
  private static HashSet<Utils.OnceLogToken> hashSet_1 = new HashSet<Utils.OnceLogToken>();

  public static Utils.ObjectState SaveState(this AnnotationCollection annotation)
  {
    ICollectionAnnotation icollectionAnnotation = annotation.Get<ICollectionAnnotation>(false, (object) null);
    if (icollectionAnnotation != null)
      return (Utils.ObjectState) new Utils.ListState()
      {
        Elements = icollectionAnnotation.AnnotatedElements.Select<AnnotationCollection, Utils.ObjectState>((Func<AnnotationCollection, Utils.ObjectState>) (annotationCollection_0 => annotationCollection_0.SaveState())).ToArray<Utils.ObjectState>()
      };
    IMembersAnnotation imembersAnnotation = annotation.Get<IMembersAnnotation>(false, (object) null);
    if (imembersAnnotation != null)
    {
      (Utils.ObjectState, string)[] array = imembersAnnotation.Members.Where<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 =>
      {
        IAccessAnnotation iaccessAnnotation = annotationCollection_0.Get<IAccessAnnotation>(false, (object) null);
        return iaccessAnnotation != null && !iaccessAnnotation.IsReadOnly;
      })).Select<AnnotationCollection, (Utils.ObjectState, string)>((Func<AnnotationCollection, (Utils.ObjectState, string)>) (annotationCollection_0 => (annotationCollection_0.SaveState(), ((IReflectionData) annotationCollection_0.Get<IMemberAnnotation>(false, (object) null).Member).Name))).Where<(Utils.ObjectState, string)>((Func<(Utils.ObjectState, string), bool>) (valueTuple_0 => valueTuple_0.State != null)).ToArray<(Utils.ObjectState, string)>();
      return (Utils.ObjectState) new Utils.MembersState()
      {
        Members = ((IEnumerable<(Utils.ObjectState, string)>) array).ToDictionary<(Utils.ObjectState, string), string, Utils.ObjectState>((Func<(Utils.ObjectState, string), string>) (valueTuple_0 => valueTuple_0.Name), (Func<(Utils.ObjectState, string), Utils.ObjectState>) (valueTuple_0 => valueTuple_0.State))
      };
    }
    IStringValueAnnotation istringValueAnnotation = annotation.Get<IStringValueAnnotation>(false, (object) null);
    if (istringValueAnnotation != null)
      return (Utils.ObjectState) new Utils.StringValueState()
      {
        Value = istringValueAnnotation.Value
      };
    IObjectValueAnnotation iobjectValueAnnotation = annotation.Get<IObjectValueAnnotation>(false, (object) null);
    if (iobjectValueAnnotation == null)
      return (Utils.ObjectState) null;
    return (Utils.ObjectState) new Utils.ValueState()
    {
      Value = iobjectValueAnnotation.Value
    };
  }

  public static void LoadState(this AnnotationCollection annotation, Utils.ObjectState state)
  {
    switch (state)
    {
      case Utils.ListState listState:
        ICollectionAnnotation icollectionAnnotation = annotation.Get<ICollectionAnnotation>(false, (object) null);
        AnnotationCollection[] array = icollectionAnnotation.AnnotatedElements.ToArray<AnnotationCollection>();
        List<AnnotationCollection> annotationCollectionList = new List<AnnotationCollection>();
        int index = 0;
        foreach (Utils.ObjectState element in listState.Elements)
        {
          AnnotationCollection annotationCollection = ((IEnumerable<AnnotationCollection>) array).ElementAtOrDefault<AnnotationCollection>(index);
          AnnotationCollection annotation1 = annotationCollection == null ? icollectionAnnotation.NewElement() : annotationCollection;
          annotation1.LoadState(element);
          annotationCollectionList.Add(annotation1);
          ++index;
        }
        icollectionAnnotation.AnnotatedElements = (IEnumerable<AnnotationCollection>) annotationCollectionList;
        break;
      case Utils.MembersState membersState:
        using (IEnumerator<AnnotationCollection> enumerator = annotation.Get<IMembersAnnotation>(false, (object) null).Members.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnnotationCollection current = enumerator.Current;
            string name = ((IReflectionData) current.Get<IMemberAnnotation>(false, (object) null)?.Member).Name;
            Utils.ObjectState state1;
            if (membersState.Members.TryGetValue(name, out state1))
              current.LoadState(state1);
          }
          break;
        }
      case Utils.StringValueState stringValueState:
        annotation.Get<IStringValueAnnotation>(false, (object) null).Value = stringValueState.Value;
        break;
      case Utils.ValueState valueState:
        annotation.Get<IObjectValueAnnotation>(false, (object) null).Value = valueState.Value;
        break;
    }
  }

  public static void Swap<T>(ref T gparam_0, ref T gparam_1)
  {
    T obj = gparam_0;
    gparam_0 = gparam_1;
    gparam_1 = obj;
  }

  public static T Clamp<T>(this T gparam_0, T gparam_1, T gparam_2) where T : IComparable<T>
  {
    if (gparam_0.CompareTo(gparam_1) < 0)
      return gparam_1;
    return gparam_0.CompareTo(gparam_2) > 0 ? gparam_2 : gparam_0;
  }

  public static T Identity<T>(T gparam_0) => gparam_0;

  private static T smethod_0<T, U>(this IEnumerable<T> ienumerable_0, Func<T, U> func_0, int int_0) where U : IComparable
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

  public static T FindMax<T, C>(this IEnumerable<T> ienumerable, Func<T, C> selector) where C : IComparable
  {
    return ienumerable.smethod_0<T, C>(selector, 1);
  }

  public static T FindMin<T, C>(this IEnumerable<T> ienumerable, Func<T, C> selector) where C : IComparable
  {
    return ienumerable.smethod_0<T, C>(selector, -1);
  }

  public static IEnumerable<T> SkipLastN<T>(this IEnumerable<T> source, int int_0)
  {
    List<T> list = source.ToList<T>();
    return list.Count - int_0 > 0 ? list.Take<T>(list.Count - int_0) : Enumerable.Empty<T>();
  }

  public static void RemoveIf<T>(this IList<T> source, Predicate<T> pred)
  {
    if (source is List<T> objList)
    {
      objList.RemoveAll(pred);
    }
    else
    {
      for (int index = source.Count - 1; index >= 0; --index)
      {
        if (pred(source[index]))
          source.RemoveAt(index);
      }
    }
  }

  public static void RemoveIf(this IList source, Predicate<object> pred)
  {
    for (int index = source.Count - 1; index >= 0; --index)
    {
      if (pred(source[index]))
        source.RemoveAt(index);
    }
  }

  private static void smethod_1<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> func_0,
    IList<T> ilist_0)
  {
    Utils.smethod_2<T>(ienumerable_0, func_0, ilist_0, (HashSet<T>) null);
  }

  private static void smethod_2<T>(
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
        Utils.smethod_2<T>(ienumerable_0_1, func_0, ilist_0, hashSet_2);
    }
  }

  public static List<T> FlattenHeirarchy<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> lookup,
    bool distinct = false,
    List<T> buffer = null)
  {
    if (buffer != null)
      buffer.Clear();
    else
      buffer = new List<T>();
    Utils.smethod_2<T>(ienumerable_0, lookup, (IList<T>) buffer, distinct ? new HashSet<T>() : (HashSet<T>) null);
    return buffer;
  }

  public static void FlattenHeirarchyInto<T>(
    IEnumerable<T> ienumerable_0,
    Func<T, IEnumerable<T>> lookup,
    ISet<T> iset_0)
  {
    foreach (T obj in ienumerable_0)
    {
      if (iset_0.Add(obj))
      {
        IEnumerable<T> ienumerable_0_1 = lookup(obj);
        if (ienumerable_0_1 != null)
          Utils.FlattenHeirarchyInto<T>(ienumerable_0_1, lookup, iset_0);
      }
    }
  }

  public static IEnumerable<T> Recurse<T>(T item, Func<T, T> selector)
  {
    yield return item;
    while (true)
    {
      item = selector(item);
      yield return item;
    }
  }

  public static void Evaluate<T>(this IEnumerable<T> source, Action<T> func)
  {
    foreach (T obj in source)
      func(obj);
  }

  public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] newObjects)
  {
    return source.Concat<T>((IEnumerable<T>) newObjects);
  }

  public static int IndexWhen<T>(this IEnumerable<T> source, Func<T, bool> pred)
  {
    int num = 0;
    foreach (T obj in source)
    {
      if (pred(obj))
        return num;
      ++num;
    }
    return -1;
  }

  public static bool IsLongerThan<T>(this IEnumerable<T> source, long count)
  {
    foreach (T obj in source)
    {
      if (--count < 0L)
        return true;
    }
    return false;
  }

  public static void AddRange<T>(this IList<T> ilist_0, IEnumerable<T> values)
  {
    foreach (T obj in values)
      ilist_0.Add(obj);
  }

  public static HashSet<T> ToHashset<T>(this IEnumerable<T> source) => new HashSet<T>(source);

  public static HashSet<T> ToHashset<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
  {
    return new HashSet<T>(source, comparer);
  }

  public static IEnumerable<T> Except<T>(this IEnumerable<T> source, Func<T, bool> selector)
  {
    IEnumerator<T> enumerator = source.GetEnumerator();
    while (enumerator.MoveNext())
    {
      T current = enumerator.Current;
      if (!selector(current))
        yield return current;
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator = (IEnumerator<T>) null;
  }

  public static void Delay(int int_0, Action function)
  {
    lock (Utils.hashSet_0)
    {
      Timer timer = (Timer) null;
      timer = new Timer((TimerCallback) (object_0 =>
      {
        lock (Utils.hashSet_0)
          Utils.hashSet_0.Remove(timer);
        function();
      }), (object) null, int_0, -1);
      Utils.hashSet_0.Add(timer);
    }
  }

  public static void MergeInto<T1, T2>(this Dictionary<T1, T2> srcDict, Dictionary<T1, T2> dstDict)
  {
    foreach (KeyValuePair<T1, T2> keyValuePair in srcDict)
      dstDict[keyValuePair.Key] = keyValuePair.Value;
  }

  public static IEnumerable<string> SplitPreserve(this string string_0, params char[] splitValues)
  {
    HashSet<char> hashset = ((IEnumerable<char>) splitValues).ToHashset<char>();
    int startIndex = 0;
    for (int index = 0; index < string_0.Length; ++index)
    {
      char c = string_0[index];
      if (!hashset.Contains(c))
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

  public static bool ErrorOnce(
    this TraceSource traceSource_0,
    object token,
    string message,
    params object[] args)
  {
    lock (Utils.hashSet_1)
    {
      Utils.OnceLogToken onceLogToken = new Utils.OnceLogToken()
      {
        Token = token,
        Log = traceSource_0
      };
      if (Utils.hashSet_1.Contains(onceLogToken))
        return false;
      Log.Error(traceSource_0, message, args);
      Utils.hashSet_1.Add(onceLogToken);
      return true;
    }
  }

  public static void Rethrow(this Exception exception_0)
  {
    ExceptionDispatchInfo.Capture(exception_0).Throw();
  }

  public static Exception GetInner(this Exception exception_0)
  {
    return exception_0 is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 1 ? exception_0 : exception_0.InnerException;
  }

  public static void RethrowInner(this Exception exception_0)
  {
    if (exception_0 is AggregateException aggregateException && aggregateException.InnerExceptions.Count > 1)
      exception_0.Rethrow();
    else
      (exception_0.InnerException ?? exception_0).Rethrow();
  }

  public static IEnumerable<string> ReadFileLines(string filePath)
  {
    FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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

  public static string ConvertToUnsecureString(this SecureString securePassword)
  {
    if (securePassword == null)
      throw new ArgumentNullException(nameof (securePassword));
    IntPtr num = IntPtr.Zero;
    try
    {
      num = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
      return Marshal.PtrToStringUni(num);
    }
    finally
    {
      Marshal.ZeroFreeGlobalAllocUnicode(num);
    }
  }

  public static SecureString ToSecureString(this string string_0)
  {
    SecureString secureString = new SecureString();
    foreach (char c in string_0)
      secureString.AppendChar(c);
    return secureString;
  }

  public static Type TypeOf(TypeCode typeCode)
  {
    switch (typeCode)
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

  public static bool IsFinite(double value) => !double.IsInfinity(value) && !double.IsNaN(value);

  public static bool Compatible(Version searched, Version referenced)
  {
    return searched == (Version) null || searched.Major == referenced.Major && searched.Minor >= referenced.Minor;
  }

  public static T SetFlag<T>(this T gparam_0, T flag, bool enabled) where T : struct
  {
    int num1 = (ValueType) gparam_0 is Enum ? (int) Convert.ChangeType((object) gparam_0, typeof (int)) : throw new InvalidOperationException("T must be an enum");
    int num2 = (int) Convert.ChangeType((object) flag, typeof (int));
    return (T) Enum.ToObject(typeof (T), !enabled ? num1 & ~num2 : num1 | num2);
  }

  private static double smethod_3(string string_0, ref int int_0)
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

  public static int NaturalCompare(string A, string B)
  {
    if (A == null || B == null)
      return string.Compare(A, B);
    Math.Min(A.Length, B.Length);
    int index1 = 0;
    int index2 = 0;
    while (index1 != A.Length)
    {
      if (index2 == B.Length)
        return 1;
      int int_0_1 = index1;
      double num1 = Utils.smethod_3(A, ref int_0_1);
      int int_0_2 = index2;
      double num2 = Utils.smethod_3(B, ref int_0_2);
      if (int_0_1 != index1 && int_0_2 == index2)
        return -1;
      if (int_0_2 != index2 && int_0_1 == index1)
        return 1;
      if (int_0_1 != index1 && num1 != num2)
        return num1.CompareTo(num2);
      int num3 = A[index1].CompareTo(B[index2]);
      if (num3 != 0)
        return num3;
      ++index1;
      ++index2;
    }
    return index2 == B.Length ? 0 : -1;
  }

  public static void Shuffle<T>(this IList<T> ilist_0)
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

  public static bool IsNull<T>(T gparam_0) where T : class => (object) gparam_0 == null;

  public static bool IsNotNull<T>(T gparam_0) where T : class => (object) gparam_0 != null;

  public class ObjectState
  {
  }

  private class ListState : Utils.ObjectState
  {
    public Utils.ObjectState[] Elements;

    public override string ToString()
    {
      return $"[{string.Join(",", ((IEnumerable<Utils.ObjectState>) this.Elements).Select<Utils.ObjectState, string>((Func<Utils.ObjectState, string>) (objectState_0 => objectState_0.ToString())))}]";
    }
  }

  private class MembersState : Utils.ObjectState
  {
    public Dictionary<string, Utils.ObjectState> Members;

    public override string ToString()
    {
      return $"{{{string.Join(", ", this.Members.Select<KeyValuePair<string, Utils.ObjectState>, string>((Func<KeyValuePair<string, Utils.ObjectState>, string>) (keyValuePair_0 => $"{keyValuePair_0.Key}: {keyValuePair_0.Value}")))}}}";
    }
  }

  private class StringValueState : Utils.ObjectState
  {
    public string Value;

    public override string ToString() => this.Value;
  }

  private class ValueState : Utils.ObjectState
  {
    public object Value;

    public override string ToString()
    {
      object obj = this.Value;
      string str;
      if (obj == null)
      {
        str = (string) null;
      }
      else
      {
        str = obj.ToString();
        if (str != null)
          return str;
      }
      return "";
    }
  }

  private struct OnceLogToken
  {
    public object Token;
    public TraceSource Log;
  }
}
