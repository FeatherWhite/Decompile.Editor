// Decompiled with JetBrains decompiler
// Type: OpenTap.Class16
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OpenTap;

internal class Class16
{
  public MemberInfo memberInfo_0;
  public object[] object_0;
  private DisplayAttribute displayAttribute_0;

  public DisplayAttribute DisplayAttribute_0
  {
    get
    {
      if (this.displayAttribute_0 == null)
        this.displayAttribute_0 = this.memberInfo_0.smethod_2();
      return this.displayAttribute_0;
    }
  }

  public bool Boolean_0 => this.memberInfo_0 is PropertyInfo;

  public PropertyInfo PropertyInfo_0 => this.memberInfo_0 as PropertyInfo;

  public static Class16[] smethod_0(Type type_0)
  {
    PropertyInfo[] source = type_0.smethod_19();
    type_0.smethod_21();
    return ((IEnumerable<PropertyInfo>) source).Select<PropertyInfo, Class16>((Func<PropertyInfo, Class16>) (propertyInfo_0 => new Class16((MemberInfo) propertyInfo_0))).OrderBy<Class16, string>((Func<Class16, string>) (class16_0 => class16_0.memberInfo_0.Name)).ToArray<Class16>();
  }

  public Class16(MemberInfo memberInfo_1)
  {
    this.memberInfo_0 = memberInfo_1;
    this.object_0 = memberInfo_1.smethod_6();
  }

  public IEnumerable<T> method_0<T>() => Enumerable.OfType<T>(this.object_0);

  public bool method_1<T>() where T : Attribute => (object) this.method_2<T>() != null;

  public T method_2<T>() where T : Attribute
  {
    foreach (object obj1 in this.object_0)
    {
      if (obj1 is T obj2)
        return obj2;
    }
    return default (T);
  }

  public override string ToString()
  {
    return $"{this.memberInfo_0.DeclaringType}.{this.memberInfo_0.Name}";
  }
}
