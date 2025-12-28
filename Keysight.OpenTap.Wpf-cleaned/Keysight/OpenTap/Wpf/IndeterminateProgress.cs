// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.IndeterminateProgress
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class IndeterminateProgress : INotifyPropertyChanged
{
  private bool bool_0;
  private string string_0;

  public string Message
  {
    get => this.string_0;
    set
    {
      if (value == this.string_0)
        return;
      this.string_0 = value;
      this.OnPropertyChanged(nameof (Message));
    }
  }

  public bool Done
  {
    get => this.bool_0;
    set
    {
      if (value == this.bool_0)
        return;
      this.bool_0 = value;
      this.OnPropertyChanged(nameof (Done));
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    // ISSUE: reference to a compiler-generated field
    PropertyChangedEventHandler changedEventHandler0 = this.propertyChangedEventHandler_0;
    if (changedEventHandler0 == null)
      return;
    changedEventHandler0((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
