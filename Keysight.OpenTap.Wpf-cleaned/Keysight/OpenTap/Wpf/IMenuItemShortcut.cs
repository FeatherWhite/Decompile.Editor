// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.IMenuItemShortcut
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public interface IMenuItemShortcut : IMenuItem, ITapPlugin
{
  KeyGesture Shortcut { get; }
}
