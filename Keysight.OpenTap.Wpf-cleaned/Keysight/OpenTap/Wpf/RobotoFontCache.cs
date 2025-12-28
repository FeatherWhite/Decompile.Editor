// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.RobotoFontCache
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using System;
using System.IO;
using System.Resources;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class RobotoFontCache : MarkupExtension
{
  private FontFamily fontFamily_0;

  public override object ProvideValue(IServiceProvider serviceProvider) => this.method_0();

  private object method_0()
  {
    if (this.fontFamily_0 != null)
      return (object) this.fontFamily_0;
    string tempPath = Path.GetTempPath();
    string str = Path.Combine(tempPath, "Fonts", "Roboto.ttf");
    if (!File.Exists(str))
    {
      Directory.CreateDirectory(Path.Combine(tempPath, "Fonts"));
      using (UnmanagedMemoryStream stream = new ResourceManager("Keysight.Ccl.Wsl.g", typeof (WslDialog).Assembly).GetStream("ui/resources/fonts/roboto-regular.ttf"))
      {
        using (FileStream destination = File.OpenWrite(str))
          stream.CopyTo((Stream) destination);
      }
    }
    return (object) (this.fontFamily_0 = new FontFamily(new Uri(str, UriKind.Absolute), "./#Roboto"));
  }
}
