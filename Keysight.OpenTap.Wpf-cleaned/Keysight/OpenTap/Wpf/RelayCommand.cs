// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.RelayCommand
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class RelayCommand : ICommand
{
  private bool bool_0 = true;
  private readonly Action<object> Execute;

  public bool CanExecute
  {
    get => this.bool_0;
    set
    {
      if (value == this.bool_0)
        return;
      this.bool_0 = value;
      if (this.eventHandler_0 == null)
        return;
      this.eventHandler_0((object) this, new EventArgs());
    }
  }

  public RelayCommand(Action<object> Execute) => this.Execute = Execute;

  bool ICommand.CanExecute(object parameter) => this.CanExecute;

  public event EventHandler CanExecuteChanged;

  void ICommand.Execute(object parameter) => this.Execute(parameter);

  public void UpdateCanExecute()
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new EventArgs());
  }
}
