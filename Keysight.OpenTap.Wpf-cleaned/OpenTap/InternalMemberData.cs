// Decompiled with JetBrains decompiler
// Type: OpenTap.InternalMemberData
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OpenTap;

internal class InternalMemberData
{
  public MemberInfo Info;
  public object[] Attributes;
  private DisplayAttribute displayAttribute_0;

  public DisplayAttribute Display
  {
    get
    {
      if (this.displayAttribute_0 == null)
        this.displayAttribute_0 = this.Info.GetDisplayAttribute();
      return this.displayAttribute_0;
    }
  }

  public bool IsProperty => this.Info is PropertyInfo;

  public PropertyInfo Property => this.Info as PropertyInfo;

  public static InternalMemberData[] Get(Type type)
  {
    PropertyInfo[] propertiesTap = type.GetPropertiesTap();
    type.GetMethodsTap();
    return ((IEnumerable<PropertyInfo>) propertiesTap).Select<PropertyInfo, InternalMemberData>((Func<PropertyInfo, InternalMemberData>) (info => new InternalMemberData((MemberInfo) info))).OrderBy<InternalMemberData, string>((Func<InternalMemberData, string>) (internalMemberData_0 => internalMemberData_0.Info.Name)).ToArray<InternalMemberData>();
  }

  public InternalMemberData(MemberInfo info)
  {
    this.Info = info;
    this.Attributes = info.GetAllCustomAttributes();
  }

  public IEnumerable<T> GetCustomAttributes<T>() => this.Attributes.OfType<T>();

  public bool HasAttribute<T>() where T : Attribute => (object) this.GetAttribute<T>() != null;

  public T GetAttribute<T>() where T : Attribute
  {
    foreach (object attribute1 in this.Attributes)
    {
      if (attribute1 is T attribute2)
        return attribute2;
    }
    return default (T);
  }

  public override string ToString() => $"{this.Info.DeclaringType}.{this.Info.Name}";
}
