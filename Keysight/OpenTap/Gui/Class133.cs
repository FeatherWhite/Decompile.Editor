// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class133
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using OpenTap;
using System;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal class Class133 : IDisposable
{
  public string String_0 { get; set; }

  public string String_1 { get; set; }

  public ITypeData ITypeData_0 { get; }

  public GuiContext GuiContext_0 { get; set; }

  public Class133(ITypeData itypeData_1, GuiContext guiContext_1)
  {
    this.ITypeData_0 = itypeData_1;
    this.String_0 = itypeData_1.Name;
    this.String_1 = itypeData_1.GetDisplayAttribute().Name;
    this.GuiContext_0 = guiContext_1;
  }

  public UserPanel method_0(MainWindow mainWindow_0)
  {
    try
    {
      DisplayAttribute displayAttribute = this.ITypeData_0.GetDisplayAttribute();
      HelpLinkAttribute attribute = this.ITypeData_0.GetAttribute<HelpLinkAttribute>();
      string str1;
      if (attribute == null)
      {
        str1 = (string) null;
      }
      else
      {
        str1 = attribute.HelpLink;
        if (str1 != null)
          goto label_4;
      }
      str1 = "EditorHelp.chm";
label_4:
      string str2 = str1;
      return new UserPanel(this.ITypeData_0, displayAttribute.Name, this.GuiContext_0, mainWindow_0)
      {
        ToolTip = displayAttribute.Description,
        ContentId = this.String_0,
        HelpLink = str2
      };
    }
    catch (Exception ex)
    {
      TraceSource source = Log.CreateSource("Gui");
      if (source.smethod_27((object) this.ITypeData_0, "Unable to load dock panel plugin {0}", (object) this.ITypeData_0))
        source.Debug(ex);
    }
    return (UserPanel) null;
  }

  public void Dispose()
  {
    if (!(this.GuiContext_0 is IDisposable guiContext0))
      return;
    guiContext0.Dispose();
  }
}
