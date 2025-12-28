// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.SettingsMenuItem
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class SettingsMenuItem : MenuItem
{
  public List<object> AdditionalItems = new List<object>();

  static SettingsMenuItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SettingsMenuItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SettingsMenuItem)));
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != FrameworkElement.DataContextProperty)
      return;
    this.RearrangeItems();
  }

  public void RearrangeItems()
  {
    if (!(this.DataContext is IEnumerable dataContext))
      return;
    List<object> first = new List<object>();
    foreach (IGrouping<string, ComponentSettings> grouping in (IEnumerable<IGrouping<string, ComponentSettings>>) dataContext.OfType<ComponentSettings>().GroupBy<ComponentSettings, string>((Func<ComponentSettings, string>) (componentSettings_0 => componentSettings_0.GroupName)).OrderBy<IGrouping<string, ComponentSettings>, string>((Func<IGrouping<string, ComponentSettings>, string>) (igrouping_0 => igrouping_0.Key)))
    {
      if (!(grouping.Key == this.Header as string) && !(grouping.Key == ""))
      {
        first.Add((object) new Separator());
        SettingsMenuItem settingsMenuItem1 = new SettingsMenuItem();
        settingsMenuItem1.Header = (object) grouping.Key;
        SettingsMenuItem settingsMenuItem2 = settingsMenuItem1;
        settingsMenuItem2.DataContext = (object) grouping;
        first.Add((object) settingsMenuItem2);
      }
      else
      {
        foreach (ComponentSettings componentSettings in (IEnumerable<ComponentSettings>) grouping)
        {
          List<object> objectList = first;
          SettingsMenuItem settingsMenuItem = new SettingsMenuItem();
          settingsMenuItem.Header = (object) componentSettings.GetType().GetDisplayAttribute().GetFullName();
          settingsMenuItem.DataContext = (object) componentSettings;
          objectList.Add((object) settingsMenuItem);
        }
      }
    }
    this.ItemsSource = (IEnumerable) first.Concat<object>((IEnumerable<object>) this.AdditionalItems);
  }
}
