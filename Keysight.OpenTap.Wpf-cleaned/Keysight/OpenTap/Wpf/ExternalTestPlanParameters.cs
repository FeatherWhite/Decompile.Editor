// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.ExternalTestPlanParameters
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class ExternalTestPlanParameters
{
  public static RoutedUICommand ShowCommand = new RoutedUICommand("Show External Test Plan Parameters", nameof (ShowCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.E, ModifierKeys.Alt)
  });
  public static RoutedUICommand AddCommand = new RoutedUICommand("Add External Test Plan Parameters", nameof (AddCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection());
  public static RoutedUICommand UnParameterizeCommand = new RoutedUICommand("Unparameterize", nameof (UnParameterizeCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection());
  public static RoutedUICommand RemoveParameterCommand = new RoutedUICommand("Remove Parameter ", nameof (RemoveParameterCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection());
  public static RoutedUICommand RenameCommand = new RoutedUICommand("Rename Parameter", nameof (RenameCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection());
  public static RoutedUICommand EditCommand = new RoutedUICommand("Edit External Test Plan Parameter", nameof (EditCommand), typeof (ExternalTestPlanParameters), new InputGestureCollection());
}
