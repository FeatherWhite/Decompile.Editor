// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.ToolMenuEntry
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class ToolMenuEntry : IMenuItem, ITapPlugin, IMenuItemIcon2
{
  private ToolProvider toolProvider_0;
  private ImageSource imageSource_0;

  public string Name => this.toolProvider_0.GetDisplay().Name;

  public string FileName => this.toolProvider_0.FileName;

  public DisplayAttribute GetDisplay()
  {
    return TypeData.GetTypeData((object) this.toolProvider_0).GetDisplayAttribute();
  }

  private ToolMenuEntry(ToolProvider toolProvider_1) => this.toolProvider_0 = toolProvider_1;

  public ImageSource Icon => this.Image;

  public void Invoke() => this.toolProvider_0.Execute();

  public ImageSource Image
  {
    get
    {
      if (this.imageSource_0 != null || this.toolProvider_0.FileName == null || !File.Exists(this.toolProvider_0.FileName))
        return this.imageSource_0;
      this.imageSource_0 = Icons.GetIcon(this.toolProvider_0.FileName);
      return this.imageSource_0;
    }
  }

  internal static IMenuItem smethod_0(ToolProvider toolProvider_1)
  {
    return (IMenuItem) new ToolMenuEntry(toolProvider_1);
  }

  public static IEnumerable<IMenuItem> GetToolMenuEntries()
  {
    IEnumerator<ToolProvider> enumerator = ((IEnumerable<ToolProvider>) ToolProvider.GetToolProviders()).GetEnumerator();
    while (enumerator.MoveNext())
    {
      ToolProvider current = enumerator.Current;
      if (File.Exists(current.FileName))
        yield return ToolMenuEntry.smethod_0(current);
    }
    // ISSUE: reference to a compiler-generated method
    this.method_0();
    enumerator = (IEnumerator<ToolProvider>) null;
  }
}
