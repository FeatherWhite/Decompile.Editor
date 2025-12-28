// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.AutomationIdConverter
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class AutomationIdConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is NewStepControl.StepTypeViewModel ? (object) (((NewStepControl.StepViewModel) value).Name + "TreeViewItem") : (object) "";
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
