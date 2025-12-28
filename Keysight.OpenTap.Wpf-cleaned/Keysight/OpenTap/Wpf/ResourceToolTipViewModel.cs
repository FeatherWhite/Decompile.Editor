// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ResourceToolTipViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ResourceToolTipViewModel
{
  public string Name { get; }

  public string TypeName { get; }

  public ResourceToolTipViewModel(object resource)
  {
    string str1;
    if (!(resource is IResource iresource))
    {
      if (resource == null)
      {
        str1 = (string) null;
      }
      else
      {
        string str2 = resource.ToString();
        if (str2 == null)
        {
          str1 = str2;
        }
        else
        {
          str1 = str2;
          goto label_6;
        }
      }
    }
    else
    {
      str1 = iresource.Name;
      if (str1 != null)
        goto label_6;
    }
    str1 = "";
label_6:
    this.Name = str1;
    this.TypeName = ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) TypeData.GetTypeData(resource)).GetFullName();
  }

  public override string ToString() => $"{this.Name} {this.TypeName}";
}
