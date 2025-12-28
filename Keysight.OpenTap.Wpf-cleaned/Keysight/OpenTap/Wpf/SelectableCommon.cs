// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SelectableCommon
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SelectableCommon : INotifyPropertyChanged
{
  private IMultiSelectAnnotationProxy imultiSelectAnnotationProxy_0;
  private AnnotationCollection[] annotationCollection_0;
  private AnnotationCollection annotationCollection_1;

  public SelectableCommon.Selectable[] Selectables { get; private set; }

  public event PropertyChangedEventHandler PropertyChanged;

  private void method_0(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, new PropertyChangedEventArgs(string_0));
  }

  private SelectableCommon(AnnotationCollection annotationCollection_2)
  {
    this.annotationCollection_1 = annotationCollection_2;
    this.imultiSelectAnnotationProxy_0 = annotationCollection_2.Get<IMultiSelectAnnotationProxy>(false, (object) null);
    this.annotationCollection_0 = this.imultiSelectAnnotationProxy_0.SelectedValues.ToArray<AnnotationCollection>();
  }

  public static SelectableCommon Create(AnnotationCollection annotation, SelectableCommon update = null)
  {
    IAvailableValuesAnnotationProxy valuesAnnotationProxy = annotation.Get<IAvailableValuesAnnotationProxy>(false, (object) null);
    SelectableCommon common = new SelectableCommon(annotation);
    List<SelectableCommon.Selectable> source = new List<SelectableCommon.Selectable>();
    foreach (AnnotationCollection availableValue in valuesAnnotationProxy.AvailableValues)
      source.Add(new SelectableCommon.Selectable(common, availableValue));
    common.Selectables = source.ToArray();
    return update != null && source.Count == update.Selectables.Length && source.Select<SelectableCommon.Selectable, (object, bool)>((Func<SelectableCommon.Selectable, (object, bool)>) (selectable_0 => (selectable_0.Name, selectable_0.IsSelected))).SequenceEqual<(object, bool)>(((IEnumerable<SelectableCommon.Selectable>) update.Selectables).Select<SelectableCommon.Selectable, (object, bool)>((Func<SelectableCommon.Selectable, (object, bool)>) (selectable_0 => (selectable_0.Name, selectable_0.IsSelected)))) ? update : common;
  }

  public AnnotationCollection Name
  {
    get
    {
      AnnotationCollection name = this.annotationCollection_1.Clone();
      name.Add((IAnnotation) new ReadOnlyMemberAnnotation());
      return name;
    }
  }

  [DebuggerDisplay("Selectable {Name2}")]
  public class Selectable : INotifyPropertyChanged
  {
    private SelectableCommon common;
    private AnnotationCollection item;

    public event PropertyChangedEventHandler PropertyChanged;

    private void method_0(string string_0)
    {
      // ISSUE: reference to a compiler-generated field
      PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
      if (changedEventHandler0 == null)
        return;
      changedEventHandler0((object) this, new PropertyChangedEventArgs(string_0));
    }

    public object Name => (object) this.item;

    internal string Name2
    {
      get
      {
        IStringReadOnlyValueAnnotation onlyValueAnnotation = this.item.Get<IStringReadOnlyValueAnnotation>(false, (object) null);
        string name2;
        if (onlyValueAnnotation == null)
        {
          name2 = (string) null;
        }
        else
        {
          name2 = onlyValueAnnotation.Value;
          if (name2 != null)
            return name2;
        }
        return this.Name.ToString();
      }
    }

    public bool IsSelected
    {
      get
      {
        return ((IEnumerable<AnnotationCollection>) this.common.annotationCollection_0).Contains<AnnotationCollection>(this.item);
      }
      set
      {
        if (value)
        {
          if (!this.IsSelected)
            this.common.imultiSelectAnnotationProxy_0.SelectedValues = (IEnumerable<AnnotationCollection>) this.common.imultiSelectAnnotationProxy_0.SelectedValues.Append<AnnotationCollection>(this.item).ToArray<AnnotationCollection>();
        }
        else if (this.IsSelected)
          this.common.imultiSelectAnnotationProxy_0.SelectedValues = (IEnumerable<AnnotationCollection>) this.common.imultiSelectAnnotationProxy_0.SelectedValues.Where<AnnotationCollection>((Func<AnnotationCollection, bool>) (annotationCollection_0 => !object.Equals((object) annotationCollection_0, (object) this.item))).ToArray<AnnotationCollection>();
        this.common.annotationCollection_0 = this.common.imultiSelectAnnotationProxy_0.SelectedValues.ToArray<AnnotationCollection>();
        GuiHelpers.GuiInvokeAsync((Action) (() => this.common.method_0("Name")));
        this.method_0(nameof (IsSelected));
      }
    }

    public Selectable(SelectableCommon common, AnnotationCollection item)
    {
      this.common = common;
      this.item = item;
    }
  }
}
