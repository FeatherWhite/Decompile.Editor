// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.PictureControlProvider
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.OpenTap.Gui;
using OpenTap;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Keysight.OpenTap.Wpf;

[Browsable(false)]
public class PictureControlProvider : IControlProvider, ITapPlugin
{
  private static readonly string[] string_0 = new string[2]
  {
    "png",
    "jpg"
  };

  private FrameworkElement method_0(IPicture ipicture_0)
  {
    ContentPresenter wrapper = new ContentPresenter();
    // ISSUE: variable of a compiler-generated type
    PictureControlProvider.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10_1;
    bool success;
    string error;
    MemoryStream memoryStream_0;
    // ISSUE: variable of a compiler-generated type
    PictureControlProvider.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10;
    TapThread.Start((Action) (async () =>
    {
      cDisplayClass10 = cDisplayClass10_1;
      Stream stream = (Stream) null;
      error = "";
      try
      {
        // ISSUE: reference to a compiler-generated method
        stream = await cDisplayClass10_1.method_0();
        if (stream == null)
          error = "Unable to open image stream.";
      }
      catch (Exception ex)
      {
        error = ex.Message;
      }
      memoryStream_0 = new MemoryStream();
      success = false;
      if (stream != null)
      {
        await stream.CopyToAsync((Stream) memoryStream_0);
        memoryStream_0.Seek(0L, SeekOrigin.Begin);
        success = true;
        stream.Dispose();
      }
      GuiHelpers.GuiInvoke((Action) (() =>
      {
        if (!success)
        {
          // ISSUE: reference to a compiler-generated field
          ContentPresenter wrapper2 = cDisplayClass10.wrapper;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          wrapper2.Content = (object) new TextBlock()
          {
            ToolTip = (object) cDisplayClass10.data.Description,
            Text = $"Picture '{cDisplayClass10.data.Source}' could not be loaded. {error}",
            TextWrapping = TextWrapping.Wrap
          };
        }
        else
        {
          BitmapImage bitmapImage = new BitmapImage();
          bitmapImage.BeginInit();
          bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
          bitmapImage.StreamSource = (Stream) memoryStream_0;
          bitmapImage.EndInit();
          memoryStream_0.Dispose();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cDisplayClass10.wrapper.Content = (object) new Image()
          {
            ToolTip = (object) cDisplayClass10.data.Description,
            Source = (ImageSource) bitmapImage,
            MaxHeight = (double) bitmapImage.PixelHeight,
            MaxWidth = (double) bitmapImage.PixelWidth
          };
        }
      }));
      stream = (Stream) null;
    }), "");
    return (FrameworkElement) wrapper;
  }

  public FrameworkElement CreateControl(AnnotationCollection annotation)
  {
    if (annotation.Get<IPictureAnnotation>(false, (object) null) == null)
      return (FrameworkElement) null;
    return !(annotation.Get<IObjectValueAnnotation>(false, (object) null).Value is IPicture ipicture_0) ? (FrameworkElement) null : this.method_0(ipicture_0);
  }

  public double Order => 21.0;
}
