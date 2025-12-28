// Decompiled with JetBrains decompiler
// Type: OpenTap.ReflectionHelper
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace OpenTap;

internal static class ReflectionHelper
{
  private static Dictionary<MemberInfo, DisplayAttribute> dictionary_0 = new Dictionary<MemberInfo, DisplayAttribute>(1024 /*0x0400*/);
  private static object object_0 = new object();
  private static Dictionary<MemberInfo, string> dictionary_1 = new Dictionary<MemberInfo, string>(1024 /*0x0400*/);
  private static readonly ConditionalWeakTable<MemberInfo, object[]> conditionalWeakTable_0 = new ConditionalWeakTable<MemberInfo, object[]>();
  private static Mutex mutex_0;
  private static readonly Dictionary<Type, PropertyInfo[]> dictionary_2 = new Dictionary<Type, PropertyInfo[]>(1024 /*0x0400*/);
  private static readonly Dictionary<Type, MethodInfo[]> dictionary_3 = new Dictionary<Type, MethodInfo[]>(1024 /*0x0400*/);
  private static readonly ConditionalWeakTable<Type, InternalMemberData[]> conditionalWeakTable_1 = new ConditionalWeakTable<Type, InternalMemberData[]>();

  public static Type GetInnerType(this ITypeData typedata)
  {
    if (typedata == null)
      return typeof (ITestStep);
    return typedata is TypeData typeData ? typeData.Load() : typedata.BaseType.GetInnerType();
  }

  public static bool IsA(this ITypeData type, Type basetype)
  {
    return type is TypeData typeData && typeData.Type == basetype;
  }

  public static DisplayAttribute GetDisplayAttribute(this MemberInfo type)
  {
    lock (ReflectionHelper.object_0)
    {
      if (!ReflectionHelper.dictionary_0.ContainsKey(type))
      {
        DisplayAttribute displayAttribute;
        try
        {
          displayAttribute = type.GetAttribute<DisplayAttribute>();
        }
        catch
        {
          displayAttribute = (DisplayAttribute) null;
        }
        if (displayAttribute == null)
          displayAttribute = new DisplayAttribute(type.Name, (string) null, (string) null, -10000.0, false, (string[]) null);
        ReflectionHelper.dictionary_0[type] = displayAttribute;
      }
      return ReflectionHelper.dictionary_0[type];
    }
  }

  internal static string ParseDisplayname(string displayName, out string group)
  {
    group = "";
    string[] source = displayName.Trim().TrimStart('-').Split('\\');
    if (source.Length == 1)
      return displayName;
    if (source.Length < 2)
      return displayName.Trim();
    group = source[0].Trim();
    return ((IEnumerable<string>) source).Last<string>().Trim();
  }

  public static string GetHelpLink(this MemberInfo member)
  {
    lock (ReflectionHelper.dictionary_1)
    {
      if (!ReflectionHelper.dictionary_1.ContainsKey(member))
      {
        string str = (string) null;
        try
        {
          HelpLinkAttribute attribute = member.GetAttribute<HelpLinkAttribute>();
          if (attribute != null)
            str = attribute.HelpLink;
          if (str == null)
          {
            if (member.DeclaringType != (Type) null)
              str = member.DeclaringType.GetHelpLink();
          }
        }
        catch
        {
        }
        ReflectionHelper.dictionary_1[member] = str;
      }
      return ReflectionHelper.dictionary_1[member];
    }
  }

  private static object[] smethod_0(MemberInfo memberInfo_0)
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

  public static object[] GetAllCustomAttributes(this MemberInfo prop)
  {
    return ReflectionHelper.conditionalWeakTable_0.GetValue(prop, new ConditionalWeakTable<MemberInfo, object[]>.CreateValueCallback(ReflectionHelper.smethod_0));
  }

  public static T GetFirstOrDefaultCustomAttribute<T>(this MemberInfo prop) where T : Attribute
  {
    foreach (object allCustomAttribute in prop.GetAllCustomAttributes())
    {
      if (allCustomAttribute is T defaultCustomAttribute)
        return defaultCustomAttribute;
    }
    return default (T);
  }

  public static T GetAttribute<T>(this MemberInfo prop) where T : Attribute
  {
    return prop.GetFirstOrDefaultCustomAttribute<T>();
  }

  public static bool HasAttribute<T>(this MemberInfo prop) where T : Attribute
  {
    return prop.IsDefined(typeof (T), true);
  }

  public static bool HasAttribute<T>(this Type type_0) where T : Attribute
  {
    return type_0.IsDefined(typeof (T), true);
  }

  public static bool IsBrowsable(this MemberInfo memberInfo_0)
  {
    BrowsableAttribute attribute = memberInfo_0.GetAttribute<BrowsableAttribute>();
    return attribute == null || attribute.Browsable;
  }

  public static bool DescendsTo(this Type type_0, Type otherType)
  {
    if (type_0 == otherType)
      return true;
    if (otherType.IsGenericTypeDefinition)
    {
      if (otherType.IsInterface)
      {
        foreach (Type type in type_0.GetInterfaces())
        {
          if (type.IsGenericType && type.GetGenericTypeDefinition() == otherType)
            return true;
        }
        if (type_0.IsInterface && type_0.IsGenericType && type_0.GetGenericTypeDefinition() == otherType)
          return true;
      }
      else
      {
        for (Type type = type_0; type != typeof (object) && type != (Type) null; type = type.BaseType)
        {
          if (type.IsGenericType && type.GetGenericTypeDefinition() == otherType)
            return true;
        }
      }
    }
    return otherType.IsAssignableFrom(type_0);
  }

  public static bool HasInterface<T>(this Type type_0) => typeof (T).IsAssignableFrom(type_0);

  public static bool IsNumeric(this Type type_0)
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

  public static object CreateInstance(this Type type_0) => Activator.CreateInstance(type_0);

  public static Type GetEnumerableElementType(this Type enumType)
  {
    if (enumType.IsArray)
      return enumType.GetElementType();
    Type type1 = enumType.GetInterface("IEnumerable`1");
    if ((object) type1 == null)
      type1 = enumType;
    Type type2 = type1;
    return type2 != (Type) null ? ((IEnumerable<Type>) type2.GetGenericArguments()).FirstOrDefault<Type>() : typeof (object);
  }

  public static void SetTapMutex() => ReflectionHelper.mutex_0 = new Mutex(false, "TapMutex");

  private static PropertyInfo[] smethod_1(Type type_0)
  {
    lock (ReflectionHelper.dictionary_2)
    {
      if (!ReflectionHelper.dictionary_2.ContainsKey(type_0))
      {
        try
        {
          ReflectionHelper.dictionary_2[type_0] = type_0.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
        catch
        {
          ReflectionHelper.dictionary_2[type_0] = Array.Empty<PropertyInfo>();
        }
      }
      return ReflectionHelper.dictionary_2[type_0];
    }
  }

  public static PropertyInfo[] GetPropertiesTap(this Type type) => ReflectionHelper.smethod_1(type);

  private static MethodInfo[] smethod_2(Type type_0)
  {
    lock (ReflectionHelper.dictionary_3)
    {
      if (!ReflectionHelper.dictionary_3.ContainsKey(type_0))
      {
        try
        {
          ReflectionHelper.dictionary_3[type_0] = type_0.GetMethods();
        }
        catch
        {
          ReflectionHelper.dictionary_3[type_0] = Array.Empty<MethodInfo>();
        }
      }
      return ReflectionHelper.dictionary_3[type_0];
    }
  }

  public static MethodInfo[] GetMethodsTap(this Type type) => ReflectionHelper.smethod_2(type);

  public static InternalMemberData[] GetMemberData(this Type type)
  {
    return ReflectionHelper.conditionalWeakTable_1.GetValue(type, new ConditionalWeakTable<Type, InternalMemberData[]>.CreateValueCallback(InternalMemberData.Get));
  }

  public static InternalMemberData GetMemberData(this Type type, string name)
  {
    PropertyInfo property = type.GetProperty(name);
    return property == (PropertyInfo) null ? (InternalMemberData) null : new InternalMemberData((MemberInfo) property);
  }
}
