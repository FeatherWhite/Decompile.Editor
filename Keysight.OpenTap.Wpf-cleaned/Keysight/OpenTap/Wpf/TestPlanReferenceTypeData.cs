// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.TestPlanReferenceTypeData
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using OpenTap.Plugins.BasicSteps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

internal class TestPlanReferenceTypeData : ITypeData, IReflectionData
{
  private const string string_0 = "References a test plan from an external test plan file directly.";
  private object[] object_0;

  public TestPlanReferenceTypeData(string relativeTestPlanPath)
  {
    this.RelativeTestPlanPath = relativeTestPlanPath;
    this.Name = Path.GetFileNameWithoutExtension(this.RelativeTestPlanPath);
    List<string> stringList = new List<string>()
    {
      "Test Plans"
    };
    stringList.AddRange(((IEnumerable<string>) Path.GetDirectoryName(this.RelativeTestPlanPath).Split(Path.DirectorySeparatorChar)).Skip<string>(1));
    this.object_0 = new object[1]
    {
      (object) new DisplayAttribute(this.Name, "References a test plan from an external test plan file directly.", (string) null, -10000.0, false, stringList.ToArray())
    };
  }

  public void UpdateDescription(string description)
  {
    DisplayAttribute displayAttribute = (DisplayAttribute) this.object_0[0];
    this.object_0[0] = (object) new DisplayAttribute(this.Name, description, (string) null, -10000.0, false, displayAttribute.Group);
  }

  public ITypeData BaseType => (ITypeData) TypeData.FromType(typeof (TestPlanReference));

  public bool CanCreateInstance => true;

  public IEnumerable<object> Attributes => (IEnumerable<object>) this.object_0;

  public string Name { get; private set; }

  public string RelativeTestPlanPath { get; }

  public object CreateInstance(object[] arguments)
  {
    MacroString macroString = new MacroString()
    {
      Text = this.RelativeTestPlanPath
    };
    TestPlanReference testPlanReference = new TestPlanReference();
    testPlanReference.Filepath = macroString;
    ((TestStep) testPlanReference).Name = this.Name;
    TestPlanReference instance = testPlanReference;
    instance.LoadTestPlan();
    TypeData.GetTypeData((object) instance).GetMember("OpenTap.Description")?.SetValue((object) instance, (object) ReflectionDataExtensions.GetDisplayAttribute((IReflectionData) this).Description);
    return (object) instance;
  }

  public IMemberData GetMember(string name) => throw new NotImplementedException();

  public IEnumerable<IMemberData> GetMembers() => throw new NotImplementedException();
}
