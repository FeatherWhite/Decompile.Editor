// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.BaseUi
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class BaseUi : INotifyPropertyChanged
{
  private bool bool_0 = true;
  private bool bool_1 = true;

  public virtual bool GroupVisible
  {
    get => this.bool_0;
    set
    {
      if (value == this.bool_0)
        return;
      this.bool_0 = value;
      this.RaisePropertyChanged("CalcVisibility");
    }
  }

  public bool IsVisible
  {
    get => this.bool_1;
    set
    {
      if (value == this.bool_1)
        return;
      this.bool_1 = value;
      this.RaisePropertyChanged(nameof (IsVisible));
      this.RaisePropertyChanged("CalcVisibility");
    }
  }

  public bool CalcVisibility => this.IsVisible & this.GroupVisible;

  public event PropertyChangedEventHandler PropertyChanged;

  public void RaisePropertyChanged(string propertyname)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs(propertyname));
  }

  public virtual double GetOrder() => 0.0;

  public virtual string GetName() => "";
}
