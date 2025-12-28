// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.GenericGui
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI.Managers;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Serialization;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public static class GenericGui
{
  private static IControlProvider[] icontrolProvider_0 = Array.Empty<IControlProvider>();
  private static HashSet<Type> hashSet_0 = new HashSet<Type>();
  private static TraceSource traceSource_0 = Log.CreateSource("PropertyGrid");

  public static void UpdateHelpLink(
    object objectWithHelpLinkAttribute,
    DependencyObject objectGettingUpdatedHelpLink)
  {
    Type type = objectWithHelpLinkAttribute.GetType();
    if (!ReflectionHelper.HasAttribute<HelpLinkAttribute>(type))
      return;
    HelpLinkAttribute attribute = type.GetAttribute<HelpLinkAttribute>();
    HelpManager.SetHelpLink(objectGettingUpdatedHelpLink, (object) attribute.HelpLink);
  }

  private static IControlProvider[] smethod_0()
  {
    Type[] array = PluginManager.GetPlugins<IControlProvider>().ToArray<Type>();
    if (((IEnumerable<Type>) array).Count<Type>((Func<Type, bool>) (type_0 => !GenericGui.hashSet_0.Contains(type_0))) != GenericGui.icontrolProvider_0.Length)
    {
      GenericGui.icontrolProvider_0 = ((IEnumerable<Type>) array).Select<Type, IControlProvider>((Func<Type, IControlProvider>) (type_0 =>
      {
        try
        {
          return (IControlProvider) Activator.CreateInstance(type_0);
        }
        catch
        {
          GenericGui.hashSet_0.Add(type_0);
          return (IControlProvider) null;
        }
      })).Where<IControlProvider>((Func<IControlProvider, bool>) (icontrolProvider_0 => icontrolProvider_0 != null)).ToArray<IControlProvider>();
      Array.Sort<IControlProvider>(GenericGui.icontrolProvider_0, (Comparison<IControlProvider>) ((icontrolProvider_0, icontrolProvider_1) => icontrolProvider_1.Order.CompareTo(icontrolProvider_0.Order)));
    }
    return GenericGui.icontrolProvider_0;
  }

  public static FrameworkElement LoadDependencyContainer(AnnotationCollection item)
  {
    foreach (IControlProvider controlProvider in GenericGui.smethod_0())
    {
      FrameworkElement control = controlProvider.CreateControl(item);
      if (control != null)
        return control;
    }
    TextBlock textBlock = new TextBlock();
    textBlock.Text = "No Plugin Available";
    textBlock.ToolTip = (object) $"No plugin available to show {item}";
    textBlock.VerticalAlignment = VerticalAlignment.Center;
    return (FrameworkElement) textBlock;
  }

  public static FrameworkElement Create(AnnotationCollection annotation)
  {
    ItemUi itemUi_0 = GenericGui.CreateItemUi(annotation);
    Border border = new Border();
    border.Name = "GenericGuiCreate";
    border.Background = (Brush) Brushes.Transparent;
    Border border_0 = border;
    if (itemUi_0.Item.HasToolTip)
      border_0.SetBinding(FrameworkElement.ToolTipProperty, (BindingBase) new Binding("CompleteDescription")
      {
        Source = (object) itemUi_0.Item
      });
    itemUi_0.Annotation.Get<UpdateMonitor>(true, (object) null)?.RegisterSourceUpdated((FrameworkElement) border_0, (Action) (() =>
    {
      itemUi_0.UpdateVisibility();
      itemUi_0.UpdateErrors();
      border_0?.GetBindingExpression(FrameworkElement.ToolTipProperty)?.UpdateTarget();
    }));
    border_0.Child = (UIElement) itemUi_0.Control;
    border_0.SetBinding(UIElement.IsEnabledProperty, (BindingBase) new Binding("IsEnabled")
    {
      Source = (object) itemUi_0
    });
    return (FrameworkElement) border_0;
  }

  private static string smethod_1(object object_0)
  {
    return object_0 is ITestStep itestStep ? itestStep.Id.ToString() : object_0.GetType().ToString();
  }

  public static IEnumerable<ItemUi> CreateItemUis(IEnumerable<AnnotationCollection> items)
  {
    IEnumerator<AnnotationCollection> enumerator = items.GetEnumerator();
    while (enumerator.MoveNext())
    {
      ItemUi itemUi = GenericGui.CreateItemUi(enumerator.Current);
      if (itemUi != null)
        yield return itemUi;
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator = (IEnumerator<AnnotationCollection>) null;
  }

  public static ItemUi CreateItemUi(AnnotationCollection item)
  {
    ItemUi itemUi = (ItemUi) null;
    try
    {
      FrameworkElement control = GenericGui.LoadDependencyContainer(item);
      itemUi = new ItemUi(item, control);
    }
    catch (Exception ex)
    {
      string name1 = item.ParentAnnotation.Get<DisplayAttribute>(false, (object) null)?.Name;
      string name2 = item.Get<DisplayAttribute>(false, (object) null)?.Name;
      Log.Error(GenericGui.traceSource_0, "Error while generating GUI for '{0}', property '{1}'.", new object[2]
      {
        (object) name1,
        (object) name2
      });
      Log.Debug(GenericGui.traceSource_0, ex);
    }
    return itemUi;
  }

  public static GroupUi CreateGroupUI(
    IEnumerable<AnnotationCollection> items,
    string name,
    int level = 0)
  {
    object declaringType = (object) items.FirstOrDefault<AnnotationCollection>()?.Get<IMemberAnnotation>(false, (object) null).Member.DeclaringType;
    List<BaseUi> source = new List<BaseUi>();
    IEnumerable<IGrouping<string, AnnotationCollection>> groupings = items.GroupBy<AnnotationCollection, string>((Func<AnnotationCollection, string>) (item => ((IEnumerable<string>) item.Get<DisplayAttribute>(false, (object) null).Group).Skip<string>(level).FirstOrDefault<string>() ?? ""));
    bool flag = false;
    foreach (IGrouping<string, AnnotationCollection> items1 in groupings)
    {
      if (items1.Key == "")
      {
        IEnumerable<ItemUi> itemUis = GenericGui.CreateItemUis((IEnumerable<AnnotationCollection>) items1);
        source.AddRange((IEnumerable<BaseUi>) itemUis);
        flag = source.OfType<ItemUi>().Any<ItemUi>((Func<ItemUi, bool>) (itemUi_0 => itemUi_0.Item.Collapsed));
      }
      else
        source.Add((BaseUi) GenericGui.CreateGroupUI((IEnumerable<AnnotationCollection>) items1, items1.Key, level + 1));
    }
    return new GroupUi(name, GenericGui.smethod_1(declaringType), !flag)
    {
      Level = level,
      Items = (IReadOnlyList<BaseUi>) source.OrderBy<BaseUi, double>((Func<BaseUi, double>) (baseUi_0 => baseUi_0.GetOrder())).ThenBy<BaseUi, bool>((Func<BaseUi, bool>) (baseUi_0 => baseUi_0 is GroupUi)).ThenBy<BaseUi, string>((Func<BaseUi, string>) (baseUi_0 => baseUi_0.GetName())).ToList<BaseUi>().AsReadOnly()
    };
  }

  public static IEnumerable<MemberInfo> GetReflectionDataFromType(Type type_0)
  {
    PropertyInfo[] propertiesTap = type_0.GetPropertiesTap();
    MethodInfo[] methodsTap = type_0.GetMethodsTap();
    if (propertiesTap.Length == 0 && methodsTap.Length == 0)
      return (IEnumerable<MemberInfo>) Array.Empty<MemberInfo>();
    MemberInfo[] destinationArray = new MemberInfo[propertiesTap.Length + methodsTap.Length];
    Array.Copy((Array) propertiesTap, (Array) destinationArray, propertiesTap.Length);
    Array.Copy((Array) methodsTap, 0, (Array) destinationArray, propertiesTap.Length, methodsTap.Length);
    return (IEnumerable<MemberInfo>) destinationArray;
  }

  public static IEnumerable<MemberInfo> GetReflectionDataFromObject(object object_0)
  {
    return GenericGui.GetReflectionDataFromType(object_0.GetType());
  }

  public static IEnumerable<IMemberData> GetReflectionDataFromObject2(object object_0)
  {
    return TypeData.GetTypeData(object_0).GetMembers();
  }

  public static bool FilterDefault(MemberInfo memberInfo_0)
  {
    BrowsableAttribute attribute = memberInfo_0.GetAttribute<BrowsableAttribute>();
    if (attribute != null)
      return attribute.Browsable;
    if (memberInfo_0.GetAttribute<XmlIgnoreAttribute>() != null || (object) (memberInfo_0 as PropertyInfo) == null)
      return false;
    if (memberInfo_0.HasAttribute<OutputAttribute>())
      return true;
    PropertyInfo propertyInfo = (PropertyInfo) memberInfo_0;
    return propertyInfo.CanWrite && propertyInfo.CanRead && !(propertyInfo.GetSetMethod() == (MethodInfo) null);
  }

  public static bool FilterDefault2(IReflectionData ireflectionData_0)
  {
    BrowsableAttribute attribute = ReflectionDataExtensions.GetAttribute<BrowsableAttribute>(ireflectionData_0);
    if (attribute != null)
      return attribute.Browsable;
    return ReflectionDataExtensions.GetAttribute<XmlIgnoreAttribute>(ireflectionData_0) == null && ireflectionData_0 is IMemberData imemberData && (ReflectionDataExtensions.HasAttribute<OutputAttribute>(ireflectionData_0) || imemberData.Writable && imemberData.Readable);
  }

  [DebuggerDisplay("{DisplayName}")]
  public class Item
  {
    private AnnotationCollection annotation;
    private DisplayAttribute displayAttribute_0;

    public bool FullRow
    {
      get
      {
        GuiOptions guiOptions = this.annotation.Get<GuiOptions>(false, (object) null);
        return guiOptions != null && guiOptions.FullRow;
      }
    }

    public int RowHeight
    {
      get
      {
        GuiOptions guiOptions = this.annotation.Get<GuiOptions>(false, (object) null);
        return guiOptions == null ? 1 : guiOptions.RowHeight;
      }
    }

    public int MaxRowHeight
    {
      get
      {
        GuiOptions guiOptions = this.annotation.Get<GuiOptions>(false, (object) null);
        return guiOptions == null ? 1000 : guiOptions.MaxRowHeight;
      }
    }

    public IMemberData Member
    {
      get => this.annotation.Get<IMemberAnnotation>(false, (object) null)?.Member;
    }

    public string ValueDescription
    {
      get => this.annotation.Get<IValueDescriptionAnnotation>(false, (object) null)?.Describe();
    }

    public bool HasToolTip
    {
      get
      {
        return !string.IsNullOrEmpty(this.displayAttribute_0.Description) || this.annotation.Get<IValueDescriptionAnnotation>(false, (object) null) != null;
      }
    }

    public string DisplayName => this.displayAttribute_0.Name;

    public string[] GroupName => this.displayAttribute_0.Group;

    public string Description => this.displayAttribute_0.Description;

    public object CompleteDescription
    {
      get
      {
        return (object) new ItemDescription()
        {
          Description = ((Func<string>) (() => this.Description)),
          ValueDescription = ((Func<string>) (() => this.ValueDescription))
        }.ToString();
      }
    }

    public double Order => this.displayAttribute_0.Order;

    public bool ReadOnly { get; set; }

    public string HelpLink
    {
      get => this.annotation.Get<HelpLinkAttribute>(false, (object) null)?.HelpLink;
    }

    public bool Collapsed { get; set; }

    public string TypeDescription
    {
      get
      {
        IReflectionAnnotation ireflectionAnnotation = this.annotation.Get<IReflectionAnnotation>(false, (object) null);
        string typeDescription;
        if (ireflectionAnnotation == null)
        {
          typeDescription = (string) null;
        }
        else
        {
          typeDescription = ireflectionAnnotation.ReflectionInfo.ToString();
          if (typeDescription != null)
            return typeDescription;
        }
        return "";
      }
    }

    public string AnnotationPath => this.annotation.ToString();

    public Item(AnnotationCollection annotation)
    {
      this.annotation = annotation;
      this.displayAttribute_0 = annotation.Get<DisplayAttribute>(false, (object) null) ?? new DisplayAttribute("unnamed", (string) null, (string) null, -10000.0, false, (string[]) null);
      DisplayAttribute displayAttribute0 = this.displayAttribute_0;
      this.Collapsed = displayAttribute0 != null && displayAttribute0.Collapsed;
    }
  }
}
