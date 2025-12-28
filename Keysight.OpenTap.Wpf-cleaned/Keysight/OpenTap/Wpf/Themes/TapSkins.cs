// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Themes.TapSkins
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using Keysight.Ccl.Wsl.UI;
using Keysight.Ccl.Wsl.UI.Interfaces;
using Keysight.Ccl.Wsl.UI.Managers;
using Keysight.Ccl.Wsl.UI.Resources.Skins;
using Keysight.OpenTap.Gui;
using MahApps.Metro;
using Microsoft.Win32;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Keysight.OpenTap.Wpf.Themes;

public class TapSkins
{
  [Obsolete("PathWave Hub is no longer supported.")]
  public const string HubModeIndicatorKey = "PathWave.Hub.HubMode";
  private static readonly RegistryKey registryKey_0 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", false);
  private static readonly Dictionary<TapSkins.Theme, string> dictionary_0 = new Dictionary<TapSkins.Theme, string>()
  {
    {
      TapSkins.Theme.Light,
      "TapLight"
    },
    {
      TapSkins.Theme.Dark,
      "TapDark"
    },
    {
      TapSkins.Theme.BistraLight,
      "Light"
    },
    {
      TapSkins.Theme.BistraDark,
      "Dark"
    }
  };
  private static TapSkins.Theme theme_0 = TapSkins.Theme.Light;
  private static TraceSource traceSource_0 = Log.CreateSource("GUI");
  private static TapSkins.Theme? nullable_0;

  [Obsolete("PathWave Hub is no longer supported.")]
  public static bool HubMode { get; }

  public static bool Initialized { get; private set; }

  public static void Initialize()
  {
    HelpManager.Instance.HelpProvider = (IShowHelp) new TapSkins.TapHelpProvider();
    if (TapSkins.Initialized)
      return;
    TapSkins.Initialized = true;
    typeof (AppTheme).ToString();
    SkinManager.Instance.AddSkin((Skin) new LightThemeSkin());
    SkinManager.Instance.AddSkin((Skin) new DarkThemeSkin());
    SkinManager.Instance.AddSkin((Skin) new TapCaranu());
    UXManager.Initialize("Caranu");
    UXManager.ToggleAmbientSkin("Custom Resize Borders", true);
    TapSkins.LoadTheme(TapSkins.theme_0);
    ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    ToolTipService.ShowDurationProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) 120000));
    RenderDispatch.RenderingGlacial += new EventHandler<EventArgs>(TapSkins.smethod_0);
  }

  internal static TapSkins.Theme GetOSTheme()
  {
    object obj = TapSkins.registryKey_0?.GetValue("AppsUseLightTheme");
    return obj is 1 || !(obj is 0) ? TapSkins.Theme.Light : TapSkins.Theme.Dark;
  }

  private static void smethod_0(object sender, EventArgs e)
  {
    if (TapSkins.CurrentTheme == TapSkins.Theme.OsTheme)
    {
      TapSkins.Theme osTheme = TapSkins.GetOSTheme();
      int num = (int) osTheme;
      TapSkins.Theme? nullable0 = TapSkins.nullable_0;
      int valueOrDefault = (int) nullable0.GetValueOrDefault();
      if (num == valueOrDefault & nullable0.HasValue)
        return;
      TapSkins.smethod_1(osTheme);
    }
    else
    {
      int theme0 = (int) TapSkins.theme_0;
      TapSkins.Theme? nullable0 = TapSkins.nullable_0;
      int valueOrDefault = (int) nullable0.GetValueOrDefault();
      if (theme0 == valueOrDefault & nullable0.HasValue || !TapSkins.nullable_0.HasValue)
        return;
      TapSkins.smethod_1(TapSkins.theme_0);
    }
  }

  public static TapSkins.Theme CurrentTheme => TapSkins.SelectedTheme;

  public static void LoadTheme(TapSkins.Theme theme)
  {
    if (!TapSkins.Initialized)
      return;
    Stopwatch stopwatch = Stopwatch.StartNew();
    GuiHelpers.GuiInvoke((Action) (() => TapSkins.smethod_1(TapSkins.theme_0)));
    Log.Debug(TapSkins.traceSource_0, stopwatch, "Loaded {0} Theme.", new object[1]
    {
      (object) theme
    });
  }

  public static TapSkins.Theme SelectedTheme
  {
    get => TapSkins.theme_0;
    set => TapSkins.theme_0 = value;
  }

  private static void smethod_1(TapSkins.Theme theme_1)
  {
    TapSkins.Theme? nullable0 = TapSkins.nullable_0;
    TapSkins.Theme theme = theme_1;
    if (nullable0.GetValueOrDefault() == theme & nullable0.HasValue)
      return;
    if (!TapSkins.Initialized)
      throw new InvalidOperationException("OpenTAP skins system not loaded");
    string str1 = "System";
    switch (theme_1)
    {
      case TapSkins.Theme.Light:
      case TapSkins.Theme.Dark:
        str1 = "Caranu";
        break;
      case TapSkins.Theme.OsTheme:
        TapSkins.smethod_1(TapSkins.GetOSTheme());
        return;
      case TapSkins.Theme.BistraLight:
      case TapSkins.Theme.BistraDark:
        str1 = "Bistra";
        break;
    }
    TapSkins.nullable_0 = new TapSkins.Theme?(theme_1);
    if (SkinManager.Instance.FoundationalSkin != str1)
      SkinManager.Instance.FoundationalSkin = str1;
    if (!TapSkins.dictionary_0.ContainsKey(theme_1))
      return;
    string str2 = TapSkins.dictionary_0[theme_1];
    if (!(UXManager.ColorScheme != str2))
      return;
    UXManager.ColorScheme = str2;
  }

  public enum Theme
  {
    [Display("Light", "Light color theme.", null, -10000.0, false, null)] Light,
    [Display("Dark", "Dark color theme.", null, -10000.0, false, null)] Dark,
    [Display("Automatic (Light/Dark)", "Color theme is Light or Dark based on user defined system settings.", null, -10000.0, false, null)] OsTheme,
    [Display("Legacy Light", "Legacy light color theme.", null, -10000.0, false, null), Browsable(false)] BistraLight,
    [Display("Legacy Dark", "Legacy dark color theme.", null, -10000.0, false, null), Browsable(false)] BistraDark,
    [Browsable(false)] System,
  }

  private class TapHelpProvider : IShowHelp
  {
    public void ShowHelp(object helpSpec)
    {
      if (!(helpSpec is string fileName))
        return;
      if (!fileName.EndsWith(".pdf") && !fileName.StartsWith("http://") && !fileName.StartsWith("https://"))
      {
        string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        if (!(helpSpec is string str))
          return;
        string arguments = (string) null;
        try
        {
          arguments = $"mk:@MSITStore:{directoryName}";
          if (!string.IsNullOrEmpty(str))
            arguments = arguments + Path.DirectorySeparatorChar.ToString() + str;
          Process.Start("hh.exe", arguments);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Failed to launch help file via path - " + arguments, (object) ex);
        }
      }
      else
        Process.Start(fileName);
    }
  }
}
