// Decompiled with JetBrains decompiler
// Type: OpenTap.Class55
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap.Cli;
using System;
using System.Diagnostics;
using Tap.Shared;

#nullable disable
namespace OpenTap;

internal static class Class55
{
  internal static string smethod_0(object object_0)
  {
    string str1 = "";
    foreach (IMemberData member in TypeData.GetTypeData(object_0).GetMembers())
    {
      object obj1 = member.GetValue(object_0);
      if (obj1 != null && (!(obj1 is string str2) || !string.IsNullOrWhiteSpace(str2)) && (!(obj1 is bool flag) || flag))
      {
        if (member.HasAttribute<CommandLineArgumentAttribute>())
        {
          CommandLineArgumentAttribute attribute = member.GetAttribute<CommandLineArgumentAttribute>();
          if (member.TypeDescriptor.smethod_1(typeof (bool)))
            str1 = $"{str1}--{attribute.Name} ";
          else if (obj1 is Array array)
          {
            foreach (object obj2 in array)
              str1 += $"--{attribute.Name} \"{obj2}\" ";
          }
          else
            str1 += $"--{attribute.Name} \"{obj1}\" ";
        }
        else if (member.HasAttribute<UnnamedCommandLineArgument>())
        {
          UnnamedCommandLineArgument attribute = member.GetAttribute<UnnamedCommandLineArgument>();
          str1 += $"--{attribute.Name} \"{obj1}\" ";
        }
      }
    }
    return str1;
  }

  internal static Process smethod_1(object object_0)
  {
    return Class2.smethod_1(object_0.GetType().Assembly.Location, Class55.smethod_0(object_0)).method_0();
  }
}
