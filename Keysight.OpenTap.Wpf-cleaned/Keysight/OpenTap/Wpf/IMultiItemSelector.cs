// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.IMultiItemSelector
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public interface IMultiItemSelector
{
  IList ItemsSource { get; set; }

  IList SelectedItems { get; set; }

  event EventHandler SelectionChanged;
}
