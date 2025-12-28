// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ColumnViewProvider
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ColumnViewProvider
{
  private Dictionary<ITypeData, IMemberData> dictionary_0 = new Dictionary<ITypeData, IMemberData>();
  private static bool bool_1;

  public static string GetColumnName(IMemberData member)
  {
    ColumnDisplayNameAttribute displayNameAttribute = member.GetAttribute<ColumnDisplayNameAttribute>() ?? new ColumnDisplayNameAttribute();
    if (displayNameAttribute.ColumnName != null)
      return displayNameAttribute.ColumnName;
    return member.GetDisplayAttribute().GetFullName().Trim('-', '\t', ' ', '\r', '\n');
  }

  public string Name { get; set; }

  public double Priority { get; set; }

  public bool IsReadOnly { get; set; }

  public object GetValue(object context)
  {
    ITypeData key = context != null ? TypeData.GetTypeData(context) : throw new ArgumentNullException(nameof (context));
    if (!this.dictionary_0.ContainsKey(key))
    {
      IMemberData memberData = ((IEnumerable<IMemberData>) GenericGui.GetReflectionDataFromObject2(context)).LastOrDefault<IMemberData>((Func<IMemberData, bool>) (imemberData_0 => ColumnViewProvider.GetColumnName(imemberData_0) == this.Name));
      this.dictionary_0[key] = memberData;
    }
    return this.dictionary_0[key]?.GetValue(context);
  }

  public ItemUi GetItemUi(
    AnnotationCollection baseAnnotation,
    bool isEditable,
    out bool editFormExists)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ColumnViewProvider.Class71 class71 = new ColumnViewProvider.Class71();
    // ISSUE: reference to a compiler-generated field
    class71.columnViewProvider_0 = this;
    editFormExists = false;
    if (this.IsReadOnly & isEditable)
      isEditable = false;
    ITypeData typeData = TypeData.GetTypeData(baseAnnotation.Source);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IMemberData memberData = typeData.GetMembers().FirstOrDefault<IMemberData>(new Func<IMemberData, bool>(class71.method_0)) ?? typeData.GetMembers().FirstOrDefault<IMemberData>(new Func<IMemberData, bool>(class71.method_1));
    if (memberData == null)
    {
      editFormExists = false;
      return (ItemUi) null;
    }
    ColumnDisplayNameAttribute attribute = memberData.GetAttribute<ColumnDisplayNameAttribute>();
    if ((attribute != null ? (attribute.IsReadOnly ? 1 : 0) : 0) != 0)
      isEditable = false;
    // ISSUE: reference to a compiler-generated field
    class71.annotationCollection_0 = baseAnnotation.Get<INamedMembersAnnotation>().GetMember(memberData).Clone();
    if (!isEditable)
    {
      // ISSUE: reference to a compiler-generated field
      class71.annotationCollection_0.Add((IAnnotation) new ReadOnlyViewAnnotation(), (IAnnotation) new ReadOnlyMemberAnnotation());
    }
    // ISSUE: reference to a compiler-generated field
    class71.annotationCollection_0.Add((IAnnotation) new GuiOptions());
    // ISSUE: reference to a compiler-generated field
    ItemUi itemUi = GenericGui.CreateItemUi(class71.annotationCollection_0);
    if (itemUi == null)
    {
      editFormExists = false;
      return (ItemUi) null;
    }
    itemUi.UpdateVisibility();
    itemUi.UpdateErrors();
    ref bool local = ref editFormExists;
    // ISSUE: reference to a compiler-generated field
    IAccessAnnotation accessAnnotation = class71.annotationCollection_0.Get<IAccessAnnotation>();
    int num = (accessAnnotation != null ? (accessAnnotation.IsReadOnly ? 1 : 0) : 0) == 0 ? 1 : 0;
    local = num != 0;
    // ISSUE: reference to a compiler-generated field
    class71.frameworkElement_0 = itemUi.Control;
    if (itemUi.IsScoped)
    {
      // ISSUE: reference to a compiler-generated field
      class71.frameworkElement_0.IsEnabled = false;
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    class71.frameworkElement_0.SourceUpdated += new EventHandler<DataTransferEventArgs>(class71.method_2);
    return itemUi;
  }

  private static void smethod_0(object object_0, AnnotationCollection annotationCollection_0)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ColumnViewProvider.Class72 class72 = new ColumnViewProvider.Class72();
    // ISSUE: reference to a compiler-generated field
    class72.object_0 = object_0;
    // ISSUE: reference to a compiler-generated field
    class72.annotationCollection_0 = annotationCollection_0;
    Dispatcher.CurrentDispatcher.VerifyAccess();
    if (ColumnViewProvider.bool_1)
      return;
    ColumnViewProvider.bool_1 = true;
    // ISSUE: reference to a compiler-generated method
    TapThread.Start(new Action(class72.method_0), "UpdateMonitor Update");
  }

  public static IEnumerable<ColumnViewProvider> GetFor(ITypeData itypeData_0)
  {
    return itypeData_0.GetMembers().Where<IMemberData>((Func<IMemberData, bool>) (imemberData_0 => imemberData_0.HasAttribute<ColumnDisplayNameAttribute>())).Select<IMemberData, ColumnViewProvider>(new Func<IMemberData, ColumnViewProvider>(ColumnViewProvider.GetFor));
  }

  public static ColumnViewProvider GetFor(IMemberData imemberData_0)
  {
    string columnName = ColumnViewProvider.GetColumnName(imemberData_0);
    ColumnDisplayNameAttribute attribute = imemberData_0.GetAttribute<ColumnDisplayNameAttribute>();
    if (attribute != null)
      return new ColumnViewProvider()
      {
        Name = columnName,
        Priority = attribute.Order,
        IsReadOnly = attribute.IsReadOnly
      };
    return new ColumnViewProvider() { Name = columnName };
  }

  public override int GetHashCode() => this.Name.GetHashCode();

  public override bool Equals(object object_0)
  {
    return object_0 is ColumnViewProvider && ((ColumnViewProvider) object_0).Name == this.Name;
  }

  public override string ToString() => $"(p {this.Name})";
}
