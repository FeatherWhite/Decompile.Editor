// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ResourceViewModel
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.ComponentModel;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ResourceViewModel : INotifyPropertyChanged
{
  public string Name
  {
    get => this.Resource.Name;
    set => this.Resource.Name = value;
  }

  public IResource Resource { get; private set; }

  public ResourceViewModel(IResource resource)
  {
    this.Resource = resource;
    ((INotifyPropertyChanged) this.Resource).PropertyChanged += new PropertyChangedEventHandler(this.method_0);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void method_0(object sender, PropertyChangedEventArgs e)
  {
    // ISSUE: reference to a compiler-generated field
    if (!(e.PropertyName == "Name") && !(e.PropertyName == "") || this.propertyChangedEventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.propertyChangedEventHandler_0((object) this, new PropertyChangedEventArgs("Name"));
  }
}
