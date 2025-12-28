// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.TestPlanSettingsControl
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class TestPlanSettingsControl : System.Windows.Controls.UserControl, IComponentConnector
{
  public static readonly RoutedUICommand ExportTestPlanParameters = new RoutedUICommand("Export Parameters Values", nameof (ExportTestPlanParameters), typeof (TestPlanSettingsControl));
  public static readonly RoutedUICommand ImportTestPlanParameters = new RoutedUICommand("Import Parameters Values", nameof (ImportTestPlanParameters), typeof (TestPlanSettingsControl));
  private static OpenTap.TraceSource traceSource_0 = OpenTap.Log.CreateSource("TestPlanSettings");
  internal TestPlanSettingsControl testPlanSettingsControl_0;
  internal PropGrid propGrid_0;
  private bool bool_0;

  private TestPlan plan => ((ITapDockContext) this.DataContext).Plan;

  public TestPlanSettingsControl(ITapDockContext context)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TestPlanSettingsControl.Class189 class189 = new TestPlanSettingsControl.Class189();
    // ISSUE: explicit constructor call
    base.\u002Ector();
    // ISSUE: reference to a compiler-generated field
    class189.testPlanSettingsControl_0 = this;
    this.InitializeComponent();
    this.DataContext = (object) context;
    // ISSUE: reference to a compiler-generated field
    class189.imemberData_0 = Array.Empty<IMemberData>();
    MainWindow binder = context as MainWindow;
    binder.Bind("GotLicense", (DependencyObject) this, UIElement.IsEnabledProperty);
    // ISSUE: reference to a compiler-generated method
    binder.PlanChanged += new EventHandler(class189.method_0);
  }

  private static void smethod_0(TestPlan testPlan_0, string string_0)
  {
    IEnumerable<AnnotationCollection> members = AnnotationCollection.Annotate((object) testPlan_0).Get<IMembersAnnotation>().Members;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (AnnotationCollection annotationCollection in members)
    {
      if (annotationCollection.Get<IMemberAnnotation>()?.Member is IParameterMemberData member)
      {
        try
        {
          IStringValueAnnotation stringValueAnnotation = annotationCollection.Get<IStringValueAnnotation>();
          if (stringValueAnnotation == null)
          {
            TestPlanSettingsControl.traceSource_0.Warning("Unable to export {0} since it cannot be converted to/from a string value.", (object) annotationCollection);
          }
          else
          {
            string str = stringValueAnnotation.Value;
            dictionary[member.Name] = str;
          }
        }
        catch (Exception ex)
        {
          TestPlanSettingsControl.traceSource_0.Error("Caught error while exporting {0}", (object) annotationCollection);
          TestPlanSettingsControl.traceSource_0.Debug(ex);
        }
      }
    }
    if (dictionary.Count == 0)
    {
      TestPlanSettingsControl.traceSource_0.Warning("Nothing to export");
    }
    else
    {
      using (FileStream fileStream = File.OpenWrite(string_0 + ".tmp"))
      {
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        streamWriter.WriteLine("External Name, Value");
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
          streamWriter.WriteLine($"{keyValuePair.Key},{keyValuePair.Value}");
        streamWriter.Flush();
      }
      if (File.Exists(string_0))
        File.Delete(string_0);
      File.Move(string_0 + ".tmp", string_0);
    }
  }

  public static void OnExport(TestPlan plan)
  {
    IExternalTestPlanParameterExport[] array = ((IEnumerable<IExternalTestPlanParameterExport>) TestPlanSettingsControl.smethod_1<IExternalTestPlanParameterExport>()).GroupBy<IExternalTestPlanParameterExport, string>((Func<IExternalTestPlanParameterExport, string>) (iexternalTestPlanParameterExport_0 => iexternalTestPlanParameterExport_0.Extension)).Select<IGrouping<string, IExternalTestPlanParameterExport>, IExternalTestPlanParameterExport>((Func<IGrouping<string, IExternalTestPlanParameterExport>, IExternalTestPlanParameterExport>) (igrouping_0 => igrouping_0.First<IExternalTestPlanParameterExport>())).ToArray<IExternalTestPlanParameterExport>();
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.Filter = string.Join("|", ((IEnumerable<IExternalTestPlanParameterExport>) array).Select<IExternalTestPlanParameterExport, string>((Func<IExternalTestPlanParameterExport, string>) (iexternalTestPlanParameterExport_0 => string.Format("{0} (*{1})|*{1}", (object) iexternalTestPlanParameterExport_0.Name, (object) iexternalTestPlanParameterExport_0.Extension))));
    saveFileDialog1.FilterIndex = 1;
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (saveFileDialog2.ShowDialog() != DialogResult.OK)
      return;
    Stopwatch timer = Stopwatch.StartNew();
    string fileName = saveFileDialog2.FileName;
    try
    {
      IExternalTestPlanParameterExport planParameterExport = array[saveFileDialog2.FilterIndex - 1];
      if (planParameterExport.GetType().Name.Contains("CsvFileHandler"))
        TestPlanSettingsControl.smethod_0(plan, fileName);
      else
        planParameterExport.ExportExternalParameters(plan, fileName);
    }
    catch (Exception ex)
    {
      TestPlanSettingsControl.traceSource_0.Error("{0}: {1}", (object) ex.Message, (object) fileName);
      return;
    }
    TestPlanSettingsControl.traceSource_0.Info(timer, "Test plan parameters saved successfully to {0}", (object) fileName);
  }

  public static void OnImport(TestPlan plan)
  {
    IExternalTestPlanParameterImport[] array = ((IEnumerable<IExternalTestPlanParameterImport>) TestPlanSettingsControl.smethod_1<IExternalTestPlanParameterImport>()).GroupBy<IExternalTestPlanParameterImport, string>((Func<IExternalTestPlanParameterImport, string>) (iexternalTestPlanParameterImport_0 => iexternalTestPlanParameterImport_0.Extension)).Select<IGrouping<string, IExternalTestPlanParameterImport>, IExternalTestPlanParameterImport>((Func<IGrouping<string, IExternalTestPlanParameterImport>, IExternalTestPlanParameterImport>) (igrouping_0 => igrouping_0.First<IExternalTestPlanParameterImport>())).ToArray<IExternalTestPlanParameterImport>();
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = string.Join("|", ((IEnumerable<IExternalTestPlanParameterImport>) array).Select<IExternalTestPlanParameterImport, string>((Func<IExternalTestPlanParameterImport, string>) (iexternalTestPlanParameterImport_0 => string.Format("{0} (*{1})|*{1}", (object) iexternalTestPlanParameterImport_0.Name, (object) iexternalTestPlanParameterImport_0.Extension))));
    openFileDialog1.FilterIndex = 1;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog() == DialogResult.OK)
    {
      Stopwatch timer = Stopwatch.StartNew();
      string fileName = openFileDialog2.FileName;
      try
      {
        array[openFileDialog2.FilterIndex - 1].ImportExternalParameters(plan, fileName);
      }
      catch (Exception ex)
      {
        TestPlanSettingsControl.traceSource_0.Error("{0}: {1}", (object) ex.Message, (object) fileName);
        return;
      }
      TestPlanSettingsControl.traceSource_0.Info(timer, "Test plan parameters loaded successfully from {0}", (object) fileName);
    }
    UserInput.NotifyChanged((object) plan, "");
  }

  private void method_0(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = ((IEnumerable<IExternalTestPlanParameterImport>) TestPlanSettingsControl.smethod_1<IExternalTestPlanParameterImport>()).Any<IExternalTestPlanParameterImport>() && this.propGrid_0.ContentEnabled;
  }

  private static T[] smethod_1<T>()
  {
    return TypeData.FromType(typeof (IExternalTestPlanParameterImport)).DerivedTypes.OrderByDescending<TypeData, double>((Func<TypeData, double>) (typeData_0 => typeData_0.GetDisplayAttribute().Order)).Where<TypeData>((Func<TypeData, bool>) (typeData_0 => typeData_0.CanCreateInstance)).Select<TypeData, object>((Func<TypeData, object>) (typeData_0 => typeData_0.CreateInstance())).OfType<T>().ToArray<T>();
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/Editor;component/testplansettingscontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId != 2)
        this.bool_0 = true;
      else
        this.propGrid_0 = (PropGrid) target;
    }
    else
      this.testPlanSettingsControl_0 = (TestPlanSettingsControl) target;
  }
}
