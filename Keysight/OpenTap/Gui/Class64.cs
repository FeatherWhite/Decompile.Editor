// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Class64
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using OpenTap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace Keysight.OpenTap.Gui;

internal static class Class64
{
  public static string smethod_0(this DockingManager dockingManager_0)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (StreamWriter writer = new StreamWriter((Stream) memoryStream))
      {
        new XmlLayoutSerializer(dockingManager_0).Serialize((TextWriter) writer);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
    }
  }

  public static IEnumerable<LayoutAnchorable> smethod_1(this DockingManager dockingManager_0)
  {
    return dockingManager_0.Layout.smethod_2();
  }

  public static IEnumerable<LayoutAnchorable> smethod_2(this ILayoutContainer ilayoutContainer_0)
  {
    return Class24.smethod_13<ILayoutElement>(ilayoutContainer_0.Children, (Func<ILayoutElement, IEnumerable<ILayoutElement>>) (ilayoutElement_0 => !(ilayoutElement_0 is ILayoutContainer) ? (IEnumerable<ILayoutElement>) new ILayoutElement[0] : ((ILayoutContainer) ilayoutElement_0).Children)).OfType<LayoutAnchorable>();
  }
}
