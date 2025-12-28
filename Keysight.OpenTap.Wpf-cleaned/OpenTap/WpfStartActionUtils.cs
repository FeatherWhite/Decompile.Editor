// Decompiled with JetBrains decompiler
// Type: OpenTap.WpfStartActionUtils
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap.Cli;
using System;
using System.Diagnostics;
using Tap.Shared;

#nullable disable
namespace OpenTap;

internal static class WpfStartActionUtils
{
  internal static string GenerateArguments(object object_0)
  {
    string arguments = "";
    foreach (IMemberData member in TypeData.GetTypeData(object_0).GetMembers())
    {
      object obj1 = member.GetValue(object_0);
      if (obj1 != null && (!(obj1 is string str) || !string.IsNullOrWhiteSpace(str)) && (!(obj1 is bool flag) || flag))
      {
        if (ReflectionDataExtensions.HasAttribute<CommandLineArgumentAttribute>((IReflectionData) member))
        {
          CommandLineArgumentAttribute attribute = ReflectionDataExtensions.GetAttribute<CommandLineArgumentAttribute>((IReflectionData) member);
          if (member.TypeDescriptor.IsA(typeof (bool)))
            arguments = $"{arguments}--{attribute.Name} ";
          else if (obj1 is Array array)
          {
            foreach (object obj2 in array)
              arguments += $"--{attribute.Name} \"{obj2}\" ";
          }
          else
            arguments += $"--{attribute.Name} \"{obj1}\" ";
        }
        else if (ReflectionDataExtensions.HasAttribute<UnnamedCommandLineArgument>((IReflectionData) member))
        {
          UnnamedCommandLineArgument attribute = ReflectionDataExtensions.GetAttribute<UnnamedCommandLineArgument>((IReflectionData) member);
          arguments += $"--{attribute.Name} \"{obj1}\" ";
        }
      }
    }
    return arguments;
  }

  internal static Process Execute(object object_0)
  {
    return SubProcess.Run(object_0.GetType().Assembly.Location, WpfStartActionUtils.GenerateArguments(object_0)).Process;
  }
}
