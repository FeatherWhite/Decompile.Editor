// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.FileLink
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System.Windows.Media;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class FileLink
{
  private ImageSource imageSource_0;

  public string FriendlyName { get; set; }

  public string Path { get; set; }

  public ImageSource Image
  {
    get
    {
      if (this.imageSource_0 == null)
        this.imageSource_0 = Icons.GetIcon(this.Path);
      return this.imageSource_0;
    }
  }
}
