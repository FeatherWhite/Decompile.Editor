// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ITapDockContext2
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public interface ITapDockContext2 : ITapDockContext, INotifyPropertyChanged
{
  IEnumerable<object> SelectedItems { get; set; }
}
