// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.Icons
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Keysight.OpenTap.Gui;

public static class Icons
{
  private const int int_0 = 4;
  private const int int_1 = 2;
  private const int int_2 = 16 /*0x10*/;
  private const uint uint_0 = 1;
  private const uint uint_1 = 2;
  private const uint uint_2 = 4;
  private const uint uint_3 = 16 /*0x10*/;
  private const uint uint_4 = 32 /*0x20*/;
  private const uint uint_5 = 64 /*0x40*/;
  private const uint uint_6 = 128 /*0x80*/;
  private const uint uint_7 = 256 /*0x0100*/;
  private const uint uint_8 = 512 /*0x0200*/;
  private const uint uint_9 = 1024 /*0x0400*/;
  private const uint uint_10 = 2048 /*0x0800*/;
  private const uint uint_11 = 4096 /*0x1000*/;
  private const uint uint_12 = 8192 /*0x2000*/;
  private const uint uint_13 = 16384 /*0x4000*/;
  private const uint uint_14 = 65536 /*0x010000*/;
  private const uint uint_15 = 256 /*0x0100*/;
  private const uint uint_16 = 512 /*0x0200*/;
  private const uint uint_17 = 1024 /*0x0400*/;
  private const uint uint_18 = 2048 /*0x0800*/;
  private const uint uint_19 = 4096 /*0x1000*/;
  private const uint uint_20 = 8192 /*0x2000*/;
  private const uint uint_21 = 16384 /*0x4000*/;
  private const uint uint_22 = 32768 /*0x8000*/;
  private const uint uint_23 = 65536 /*0x010000*/;
  private const uint uint_24 = 131072 /*0x020000*/;
  private const uint uint_25 = 0;
  private const uint uint_26 = 1;
  private const uint uint_27 = 2;
  private const uint uint_28 = 4;
  private const uint uint_29 = 8;
  private const uint uint_30 = 16 /*0x10*/;

  public static ImageSource GetIcon(string fileName)
  {
    string fullPath = Path.GetFullPath(fileName);
    if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
      return (ImageSource) null;
    if (fileName.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
      return (ImageSource) new BitmapImage(new Uri(fullPath));
    Icon handleFromFilePath = Icons.GetIconHandleFromFilePath(fullPath, Icons.IconSizeEnum.LargeIcon48);
    return (ImageSource) System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(handleFromFilePath.Handle, new Int32Rect(0, 0, handleFromFilePath.Width, handleFromFilePath.Height), BitmapSizeOptions.FromWidthAndHeight(handleFromFilePath.Width, handleFromFilePath.Height));
  }

  public static ImageSource GetIcon(string path, bool smallIcon, bool isDirectory)
  {
    uint uint_33 = 272;
    if (smallIcon)
      uint_33 |= 1U;
    uint uint_31 = 128 /*0x80*/;
    if (isDirectory)
      uint_31 |= 16U /*0x10*/;
    Icons.SHFILEINFO shfileinfo_0;
    return Icons.SHGetFileInfo(path, uint_31, out shfileinfo_0, (uint) Marshal.SizeOf(typeof (Icons.SHFILEINFO)), uint_33) != 0 ? (ImageSource) System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(shfileinfo_0.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) : (ImageSource) null;
  }

  public static List<Icon> GetIcons(string path)
  {
    List<Icon> source = new List<Icon>();
    int int_3 = 1;
    while (true)
    {
      Icon icon_0 = Icons.smethod_0(path, int_3);
      if (icon_0 != null && source.FirstOrDefault<Icon>((Func<Icon, bool>) (icon_1 => icon_1.Width == icon_0.Width && icon_1.Height == icon_0.Height)) == null)
      {
        source.Add(icon_0);
        ++int_3;
      }
      else
        break;
    }
    return source;
  }

  public static Icon GetIconForFile(string filename, uint size)
  {
    Icons.SHFILEINFO shfileinfo_0 = new Icons.SHFILEINFO();
    Icons.SHGetFileInfo(filename, 0U, out shfileinfo_0, (uint) Marshal.SizeOf<Icons.SHFILEINFO>(shfileinfo_0), size);
    return Icon.FromHandle(shfileinfo_0.hIcon);
  }

  [DllImport("shell32")]
  private static extern int SHGetFileInfo(
    string string_0,
    uint uint_31,
    out Icons.SHFILEINFO shfileinfo_0,
    uint uint_32,
    uint uint_33);

  private static Icon smethod_0(string string_0, int int_3)
  {
    Uri uri;
    try
    {
      uri = new Uri(string_0);
    }
    catch (UriFormatException ex)
    {
      string_0 = Path.GetFullPath(string_0);
      uri = new Uri(string_0);
    }
    if (uri.IsFile)
    {
      StringBuilder iconPath = new StringBuilder(260);
      iconPath.Append(string_0);
      IntPtr associatedIcon = Icons.ExtractAssociatedIcon(new HandleRef(), iconPath, ref int_3);
      if (associatedIcon != IntPtr.Zero)
        return Icon.FromHandle(associatedIcon);
    }
    return (Icon) null;
  }

  public static Icon GetIconHandleFromFilePath(string filepath, Icons.IconSizeEnum iconsize)
  {
    Icons.SHFILEINFO shfileinfo_0 = new Icons.SHFILEINFO();
    int int_0 = Icons.SHGetFileInfo(filepath, 128U /*0x80*/, out shfileinfo_0, (uint) Marshal.SizeOf<Icons.SHFILEINFO>(shfileinfo_0), 16384U /*0x4000*/) != 0 ? shfileinfo_0.iIcon : throw new FileNotFoundException();
    Guid guid_0 = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
    Icons.IImageList iimageList_0 = (Icons.IImageList) null;
    Icons.shell32_727((int) iconsize, ref guid_0, ref iimageList_0);
    IntPtr zero = IntPtr.Zero;
    iimageList_0.GetIcon(int_0, 33, ref zero);
    int pclr = 0;
    iimageList_0.GetBkColor(ref pclr);
    return Icon.FromHandle(zero);
  }

  [DllImport("shell32.dll", CharSet = CharSet.Auto, BestFitMapping = false)]
  public static extern IntPtr ExtractAssociatedIcon(
    HandleRef hInst,
    StringBuilder iconPath,
    ref int index);

  [DllImport("shell32.dll", EntryPoint = "#727")]
  private static extern int shell32_727(
    int int_3,
    ref Guid guid_0,
    ref Icons.IImageList iimageList_0);

  private struct SHFILEINFO
  {
    public IntPtr hIcon;
    public int iIcon;
    public uint dwAttributes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szDisplayName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80 /*0x50*/)]
    public string szTypeName;
  }

  public enum IconSizeEnum : uint
  {
    MediumIcon32 = 0,
    SmallIcon16 = 1,
    LargeIcon48 = 2,
    ExtraLargeIcon = 4,
  }

  private struct RECT
  {
    public int left;
    public int int_0;
    public int right;
    public int bottom;
  }

  private struct POINT
  {
    private int int_0;
    private int int_1;
  }

  private struct IMAGELISTDRAWPARAMS
  {
    public int cbSize;
    public IntPtr himl;
    public int int_0;
    public IntPtr hdcDst;
    public int int_1;
    public int int_2;
    public int int_3;
    public int int_4;
    public int xBitmap;
    public int yBitmap;
    public int rgbBk;
    public int rgbFg;
    public int fStyle;
    public int dwRop;
    public int fState;
    public int Frame;
    public int crEffect;
  }

  private struct IMAGEINFO
  {
    public IntPtr hbmImage;
    public IntPtr hbmMask;
    public int Unused1;
    public int Unused2;
    public Icons.RECT rcImage;
  }

  [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  private interface IImageList
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Add(IntPtr hbmImage, IntPtr hbmMask, ref int int_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ReplaceIcon(int int_0, IntPtr hicon, ref int int_1);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetOverlayImage(int iImage, int iOverlay);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Replace(int int_0, IntPtr hbmImage, IntPtr hbmMask);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AddMasked(IntPtr hbmImage, int crMask, ref int int_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Draw(ref Icons.IMAGELISTDRAWPARAMS pimldp);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Remove(int int_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetIcon(int int_0, int flags, ref IntPtr picon);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetImageInfo(int int_0, ref Icons.IMAGEINFO pImageInfo);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Copy(int iDst, Icons.IImageList punkSrc, int iSrc, int uFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Merge(
      int int_0,
      Icons.IImageList punk2,
      int int_1,
      int int_2,
      int int_3,
      ref Guid riid,
      ref IntPtr intptr_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Clone(ref Guid riid, ref IntPtr intptr_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetImageRect(int int_0, ref Icons.RECT rect_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetIconSize(ref int int_0, ref int int_1);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetIconSize(int int_0, int int_1);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetImageCount(ref int int_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetImageCount(int uNewCount);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetBkColor(int clrBk, ref int pclr);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetBkColor(ref int pclr);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EndDrag();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int DragEnter(IntPtr hwndLock, int int_0, int int_1);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int DragLeave(IntPtr hwndLock);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int DragMove(int int_0, int int_1);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetDragCursorImage(ref Icons.IImageList punk, int iDrag, int dxHotspot, int dyHotspot);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int DragShowNolock(int fShow);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetDragImage(
      ref Icons.POINT point_0,
      ref Icons.POINT pptHotspot,
      ref Guid riid,
      ref IntPtr intptr_0);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetItemFlags(int int_0, ref int dwFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetOverlayImage(int iOverlay, ref int piIndex);
  }
}
