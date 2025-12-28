// Decompiled with JetBrains decompiler
// Type: OpenTap.Class15
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace OpenTap;

internal static class Class15
{
  private static Dictionary<MemberInfo, DisplayAttribute> dictionary_0 = new Dictionary<MemberInfo, DisplayAttribute>(1024 /*0x0400*/);
  private static object object_0 = new object();
  private static Dictionary<MemberInfo, string> dictionary_1 = new Dictionary<MemberInfo, string>(1024 /*0x0400*/);
  private static readonly ConditionalWeakTable<MemberInfo, object[]> conditionalWeakTable_0 = new ConditionalWeakTable<MemberInfo, object[]>();
  private static Mutex mutex_0;
  private static readonly Dictionary<Type, PropertyInfo[]> dictionary_2 = new Dictionary<Type, PropertyInfo[]>(1024 /*0x0400*/);
  private static readonly Dictionary<Type, MethodInfo[]> dictionary_3 = new Dictionary<Type, MethodInfo[]>(1024 /*0x0400*/);
  private static readonly ConditionalWeakTable<Type, Class16[]> conditionalWeakTable_1 = new ConditionalWeakTable<Type, Class16[]>();

  public static Type smethod_0(this ITypeData itypeData_0)
  {
    if (itypeData_0 == null)
      return typeof (ITestStep);
    return itypeData_0 is TypeData typeData ? typeData.Load() : itypeData_0.BaseType.smethod_0();
  }

  public static bool smethod_1(this ITypeData itypeData_0, Type type_0)
  {
    return itypeData_0 is TypeData typeData && typeData.Type == type_0;
  }

  public static DisplayAttribute smethod_2(this MemberInfo memberInfo_0)
  {
    lock (Class15.object_0)
    {
      if (!Class15.dictionary_0.ContainsKey(memberInfo_0))
      {
        DisplayAttribute displayAttribute;
        try
        {
          displayAttribute = memberInfo_0.smethod_8<DisplayAttribute>();
        }
        catch
        {
          displayAttribute = (DisplayAttribute) null;
        }
        if (displayAttribute == null)
          displayAttribute = new DisplayAttribute(memberInfo_0.Name);
        Class15.dictionary_0[memberInfo_0] = displayAttribute;
      }
      return Class15.dictionary_0[memberInfo_0];
    }
  }

  internal static string smethod_3(string string_0, out string string_1)
  {
    string_1 = "";
    string[] source = string_0.Trim().TrimStart('-').Split('\\');
    if (source.Length == 1)
      return string_0;
    if (source.Length < 2)
      return string_0.Trim();
    string_1 = source[0].Trim();
    return ((IEnumerable<string>) source).Last<string>().Trim();
  }

  public static string smethod_4(this MemberInfo memberInfo_0)
  {
    lock (Class15.dictionary_1)
    {
      if (!Class15.dictionary_1.ContainsKey(memberInfo_0))
      {
        string str = (string) null;
        try
        {
          HelpLinkAttribute helpLinkAttribute = memberInfo_0.smethod_8<HelpLinkAttribute>();
          if (helpLinkAttribute != null)
            str = helpLinkAttribute.HelpLink;
          if (str == null)
          {
            if (memberInfo_0.DeclaringType != (Type) null)
              str = memberInfo_0.DeclaringType.smethod_4();
          }
        }
        catch
        {
        }
        Class15.dictionary_1[memberInfo_0] = str;
      }
      return Class15.dictionary_1[memberInfo_0];
    }
  }

  private static object[] smethod_5(MemberInfo memberInfo_0)
  {
    try
    {
      return memberInfo_0.GetCustomAttributes(true);
    }
    catch
    {
      return Array.Empty<object>();
    }
  }

  public static object[] smethod_6(this MemberInfo memberInfo_0)
  {
    return Class15.conditionalWeakTable_0.GetValue(memberInfo_0, new ConditionalWeakTable<MemberInfo, object[]>.CreateValueCallback(Class15.smethod_5));
  }

  public static T smethod_7<T>(this MemberInfo memberInfo_0) where T : Attribute
  {
    foreach (object obj1 in memberInfo_0.smethod_6())
    {
      if (obj1 is T obj2)
        return obj2;
    }
    return default (T);
  }

  public static T smethod_8<T>(this MemberInfo memberInfo_0) where T : Attribute
  {
    return memberInfo_0.smethod_7<T>();
  }

  public static bool smethod_9<T>(this MemberInfo memberInfo_0) where T : Attribute
  {
    return memberInfo_0.IsDefined(typeof (T), true);
  }

  public static bool smethod_10<T>(this Type type_0) where T : Attribute
  {
    return type_0.IsDefined(typeof (T), true);
  }

  public static bool smethod_11(this MemberInfo memberInfo_0)
  {
    BrowsableAttribute browsableAttribute = memberInfo_0.smethod_8<BrowsableAttribute>();
    return browsableAttribute == null || browsableAttribute.Browsable;
  }

  public static bool smethod_12(this Type type_0, Type type_1)
  {
    if (type_0 == type_1)
      return true;
    if (type_1.IsGenericTypeDefinition)
    {
      if (type_1.IsInterface)
      {
        foreach (Type type in type_0.GetInterfaces())
        {
          if (type.IsGenericType && type.GetGenericTypeDefinition() == type_1)
            return true;
        }
        if (type_0.IsInterface && type_0.IsGenericType && type_0.GetGenericTypeDefinition() == type_1)
          return true;
      }
      else
      {
        for (Type type = type_0; type != typeof (object) && type != (Type) null; type = type.BaseType)
        {
          if (type.IsGenericType && type.GetGenericTypeDefinition() == type_1)
            return true;
        }
      }
    }
    return type_1.IsAssignableFrom(type_0);
  }

  public static bool smethod_13<T>(this Type type_0) => typeof (T).IsAssignableFrom(type_0);

  public static bool smethod_14(this Type type_0)
  {
    if (type_0.IsEnum)
      return false;
    switch (Type.GetTypeCode(type_0))
    {
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return true;
      default:
        return false;
    }
  }

  public static object smethod_15(this Type type_0) => Activator.CreateInstance(type_0);

  public static Type smethod_16(this Type type_0)
  {
    if (type_0.IsArray)
      return type_0.GetElementType();
    Type type1 = type_0.GetInterface("IEnumerable`1");
    if ((object) type1 == null)
      type1 = type_0;
    Type type2 = type1;
    return type2 != (Type) null ? ((IEnumerable<Type>) type2.GetGenericArguments()).FirstOrDefault<Type>() : typeof (object);
  }

  public static void smethod_17() => Class15.mutex_0 = new Mutex(false, "TapMutex");

  private static PropertyInfo[] smethod_18(Type type_0)
  {
    lock (Class15.dictionary_2)
    {
      if (!Class15.dictionary_2.ContainsKey(type_0))
      {
        try
        {
          Class15.dictionary_2[type_0] = type_0.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
        catch
        {
          Class15.dictionary_2[type_0] = Array.Empty<PropertyInfo>();
        }
      }
      return Class15.dictionary_2[type_0];
    }
  }

  public static PropertyInfo[] smethod_19(this Type type_0) => Class15.smethod_18(type_0);

  private static MethodInfo[] smethod_20(Type type_0)
  {
    lock (Class15.dictionary_3)
    {
      if (!Class15.dictionary_3.ContainsKey(type_0))
      {
        try
        {
          Class15.dictionary_3[type_0] = type_0.GetMethods();
        }
        catch
        {
          Class15.dictionary_3[type_0] = Array.Empty<MethodInfo>();
        }
      }
      return Class15.dictionary_3[type_0];
    }
  }

  public static MethodInfo[] smethod_21(this Type type_0) => Class15.smethod_20(type_0);

  public static Class16[] smethod_22(this Type type_0)
  {
    return Class15.conditionalWeakTable_1.GetValue(type_0, new ConditionalWeakTable<Type, Class16[]>.CreateValueCallback(Class16.smethod_0));
  }

  public static Class16 smethod_23(this Type type_0, string string_0)
  {
    PropertyInfo property = type_0.GetProperty(string_0);
    return property == (PropertyInfo) null ? (Class16) null : new Class16((MemberInfo) property);
  }
}
