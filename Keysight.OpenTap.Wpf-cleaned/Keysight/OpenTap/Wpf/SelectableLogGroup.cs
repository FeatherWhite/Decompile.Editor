// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SelectableLogGroup
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Collections.Generic;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SelectableLogGroup
{
  private HashSet<string> stateLookup;

  public string Name { get; }

  public bool State
  {
    get => !this.stateLookup.Contains(this.Name);
    set
    {
      if (value)
        this.stateLookup.Remove(this.Name);
      else
        this.stateLookup.Add(this.Name);
    }
  }

  public SelectableLogGroup(HashSet<string> stateLookup, string name)
  {
    this.Name = name;
    this.stateLookup = stateLookup;
  }
}
