// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.PresetsMenuItem
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using Keysight.OpenTap.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class PresetsMenuItem : MenuItem
{
  public static readonly RoutedCommand SavePreset = new RoutedCommand(nameof (SavePreset), typeof (PresetsMenuItem), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift)
  });
  public static readonly RoutedCommand EditPreset = new RoutedCommand(nameof (EditPreset), typeof (PresetsMenuItem));
  public static RoutedUICommand SetPresetCommandKey1 = new RoutedUICommand(nameof (SetPresetCommandKey1), "ApplyViewPreset_key1", typeof (PresetsMenuItem), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.D1, ModifierKeys.Alt | ModifierKeys.Shift)
  });
  public static RoutedUICommand SetPresetCommandKey2 = new RoutedUICommand(nameof (SetPresetCommandKey2), "ApplyViewPreset_key2", typeof (PresetsMenuItem), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.D2, ModifierKeys.Alt | ModifierKeys.Shift)
  });
  public static RoutedUICommand SetPresetCommandKey3 = new RoutedUICommand(nameof (SetPresetCommandKey3), "ApplyViewPreset_key3", typeof (PresetsMenuItem), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.D3, ModifierKeys.Alt | ModifierKeys.Shift)
  });
  public static RoutedUICommand SetPresetCommandKey4 = new RoutedUICommand(nameof (SetPresetCommandKey4), "ApplyViewPreset_key4", typeof (PresetsMenuItem), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.D4, ModifierKeys.Alt | ModifierKeys.Shift)
  });

  public PresetsMenuItem()
  {
    this.Loaded += new RoutedEventHandler(this.PresetsMenuItem_Loaded);
    this.Header = (object) "Presets";
    ItemCollection items1 = this.Items;
    Separator newItem1 = new Separator();
    newItem1.Name = "PresetsSeparator";
    items1.Add((object) newItem1);
    ItemCollection items2 = this.Items;
    MenuItem newItem2 = new MenuItem();
    newItem2.Name = "NewPresetMenuItem";
    newItem2.Command = (ICommand) PresetsMenuItem.SavePreset;
    newItem2.Header = (object) "Save Preset As...";
    newItem2.ToolTip = (object) "Make a new view preset or overwrite an existing one with the current view configuration.";
    items2.Add((object) newItem2);
    ItemCollection items3 = this.Items;
    MenuItem newItem3 = new MenuItem();
    newItem3.Name = "EditPresetMenuItem";
    newItem3.Command = (ICommand) PresetsMenuItem.EditPreset;
    newItem3.Header = (object) "Edit Presets";
    newItem3.ToolTip = (object) "Edit current view presets.";
    items3.Add((object) newItem3);
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.SavePreset, new ExecutedRoutedEventHandler(this.method_0)));
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.EditPreset, (ExecutedRoutedEventHandler) ((sender, e) => ViewPreset.EditPresets())));
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.SetPresetCommandKey1, new ExecutedRoutedEventHandler(this.method_2)));
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.SetPresetCommandKey2, new ExecutedRoutedEventHandler(this.method_2)));
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.SetPresetCommandKey3, new ExecutedRoutedEventHandler(this.method_2)));
    this.CommandBindings.Add(new CommandBinding((ICommand) PresetsMenuItem.SetPresetCommandKey4, new ExecutedRoutedEventHandler(this.method_2)));
  }

  private void method_0(object sender, ExecutedRoutedEventArgs e)
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler0 = this.eventHandler_0;
    if (eventHandler0 != null)
      eventHandler0((object) this, EventArgs.Empty);
    ViewPreset.SaveAs();
  }

  public event EventHandler OnSaving;

  public event EventHandler OnLoadedPreset;

  private void PresetsMenuItem_Loaded(object sender, RoutedEventArgs e)
  {
    while (!(this.Items[0] is Separator))
      this.Items.RemoveAt(0);
    int num = 0;
    foreach (string listPreset in ViewPreset.ListPresets())
    {
      MenuItem menuItem = new MenuItem();
      menuItem.Header = (object) listPreset;
      menuItem.ToolTip = (object) $"Select view preset '{listPreset}'";
      MenuItem insertItem = menuItem;
      switch (num)
      {
        case 0:
          insertItem.Command = (ICommand) PresetsMenuItem.SetPresetCommandKey1;
          break;
        case 1:
          insertItem.Command = (ICommand) PresetsMenuItem.SetPresetCommandKey2;
          break;
        case 2:
          insertItem.Command = (ICommand) PresetsMenuItem.SetPresetCommandKey3;
          break;
        case 3:
          insertItem.Command = (ICommand) PresetsMenuItem.SetPresetCommandKey4;
          break;
        default:
          insertItem.Click += new RoutedEventHandler(this.method_1);
          break;
      }
      this.Items.Insert(num++, (object) insertItem);
    }
  }

  private void method_1(object sender, RoutedEventArgs e)
  {
    ViewPreset.SelectPreset(((HeaderedItemsControl) sender).Header as string);
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler1 = this.eventHandler_1;
    if (eventHandler1 == null)
      return;
    eventHandler1((object) this, EventArgs.Empty);
  }

  private void method_2(object sender, ExecutedRoutedEventArgs e)
  {
    int index = 0;
    if (e.Command == PresetsMenuItem.SetPresetCommandKey1)
      index = 0;
    else if (e.Command == PresetsMenuItem.SetPresetCommandKey2)
      index = 1;
    else if (e.Command == PresetsMenuItem.SetPresetCommandKey3)
      index = 2;
    else if (e.Command == PresetsMenuItem.SetPresetCommandKey4)
      index = 3;
    string layoutName = ((IEnumerable<string>) ViewPreset.ListPresets()).ElementAtOrDefault<string>(index);
    if (layoutName == null)
      return;
    ViewPreset.SelectPreset(layoutName);
    // ISSUE: reference to a compiler-generated field
    EventHandler eventHandler1 = this.eventHandler_1;
    if (eventHandler1 == null)
      return;
    eventHandler1((object) this, EventArgs.Empty);
  }
}
