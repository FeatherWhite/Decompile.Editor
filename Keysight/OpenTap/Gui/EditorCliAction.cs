// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.EditorCliAction
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using OpenTap;
using OpenTap.Cli;
using System.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

[Display("editor", "Starts the Editor GUI", null, -10000.0, false, null)]
public class EditorCliAction : ICliAction, ITapPlugin
{
  [CommandLineArgument("search", ShortName = "s", Description = "Allows PluginManager to search the specified folder for installed plugins. Multiple paths can be searched")]
  public string Search { get; set; }

  [CommandLineArgument("open", ShortName = "o", Description = "Opens the specified test plan file")]
  public string Open { get; set; }

  [CommandLineArgument("add", ShortName = "a", Description = "Adds a specific step to the end of the test plan")]
  public string Add { get; set; }

  [CommandLineArgument("enable-callbacks", Visible = false)]
  public bool EnableCallbacks { get; set; }

  [CommandLineArgument("profile", Visible = false)]
  public bool Profile { get; set; }

  [CommandLineArgument("view-preset", Description = "Apply a view preset when the application is loaded.")]
  public string ViewPreset { get; set; }

  [CommandLineArgument("focus-mode", Description = "Loads the application into focus-mode.")]
  public bool FocusMode { get; set; }

  [CommandLineArgument("external", ShortName = "e", Description = "Sets an external test plan parameter. \nUse the syntax parameter=value, e.g. \"-e delay=1.0\". The argument can be used multiple times \nor a .csv file containing sets of parameters can be specified \"-e file.csv\".")]
  public string[] External { get; set; } = new string[0];

  public int Execute(CancellationToken cancellationToken)
  {
    Class55.smethod_1((object) this);
    return 0;
  }
}
