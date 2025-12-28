// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.FilePathControl
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class FilePathControl : System.Windows.Controls.UserControl, IComponentConnector
{
  private readonly FilePathAttribute attr;
  private readonly DirectoryPathAttribute attr;
  public static RoutedUICommand Browse = new RoutedUICommand("Opens a file browse menu", nameof (Browse), typeof (FilePathControl));
  public static RoutedUICommand Open = new RoutedUICommand("Opens the selected file", nameof (Open), typeof (FilePathControl));
  public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof (Path), typeof (string), typeof (FilePathControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PathsProperty = DependencyProperty.Register(nameof (Paths), typeof (string[]), typeof (FilePathControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PathExampleProperty = DependencyProperty.Register(nameof (PathExample), typeof (string), typeof (FilePathControl), new PropertyMetadata((PropertyChangedCallback) null));
  private readonly bool multiSelect;
  internal FilePathControl This;
  internal System.Windows.Controls.Button browseButton;
  internal System.Windows.Controls.Button openButton;
  private bool bool_0;

  public bool IsDirectory => this.attr != null;

  public bool IsPath => this.attr != null | this.attr != null;

  public string Path
  {
    get => (string) this.GetValue(FilePathControl.PathProperty);
    set => this.SetValue(FilePathControl.PathProperty, (object) value);
  }

  public string[] Paths
  {
    get => (string[]) this.GetValue(FilePathControl.PathsProperty);
    set => this.SetValue(FilePathControl.PathProperty, (object) value);
  }

  public string PathExample
  {
    get => (string) this.GetValue(FilePathControl.PathExampleProperty);
    set => this.SetValue(FilePathControl.PathExampleProperty, (object) value);
  }

  public FilePathControl(FilePathAttribute attr, bool multiSelect)
  {
    this.attr = attr;
    this.multiSelect = multiSelect;
    this.InitializeComponent();
  }

  public FilePathControl(DirectoryPathAttribute attr)
  {
    this.attr = attr;
    this.InitializeComponent();
  }

  public FilePathControl() => this.InitializeComponent();

  public static string[] ExecuteBrowse(
    string path,
    FilePathAttribute.BehaviorChoice behavior,
    bool overwritePrompt,
    string fileExtension,
    bool isDirectory = false,
    bool multiSelect = false)
  {
    string fileName = (string) null;
    if (string.IsNullOrWhiteSpace(path))
      path = ".";
    try
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(path);
      if (directoryInfo1.Exists)
      {
        fileName = directoryInfo1.FullName;
        if (!fileName.EndsWith("\\"))
          fileName += "\\";
      }
      else
      {
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
          fileName = fileInfo.FullName;
        else if (fileInfo.Directory != null)
        {
          DirectoryInfo directoryInfo2 = fileInfo.Directory;
          if (directoryInfo2.Exists)
          {
            fileName = fileInfo.FullName;
          }
          else
          {
            while (directoryInfo2 != null && !directoryInfo2.Exists)
              directoryInfo2 = directoryInfo2.Parent;
            if (directoryInfo2 != null)
              fileName = directoryInfo2.FullName;
          }
        }
      }
    }
    catch (Exception ex)
    {
    }
    if (fileName == null)
      fileName = Directory.GetCurrentDirectory() + "\\";
    if (isDirectory)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = fileName;
      folderBrowserDialog.ShowNewFolderButton = true;
      return folderBrowserDialog.ShowDialog() == DialogResult.OK ? new string[1]
      {
        FileSystemHelper.GetRelativePath(Directory.GetCurrentDirectory(), folderBrowserDialog.SelectedPath)
      } : new string[1]{ path };
    }
    Microsoft.Win32.FileDialog fileDialog;
    if (behavior == 1)
      fileDialog = (Microsoft.Win32.FileDialog) new Microsoft.Win32.SaveFileDialog()
      {
        OverwritePrompt = overwritePrompt
      };
    else
      fileDialog = (Microsoft.Win32.FileDialog) new Microsoft.Win32.OpenFileDialog()
      {
        Multiselect = multiSelect
      };
    FileInfo fileInfo1 = new FileInfo(fileName);
    if (fileInfo1.Exists && fileInfo1.Attributes.HasFlag((Enum) FileAttributes.Directory))
    {
      fileDialog.InitialDirectory = fileInfo1.FullName;
      fileDialog.FileName = string.Empty;
    }
    else
    {
      fileDialog.InitialDirectory = fileInfo1.DirectoryName;
      fileDialog.FileName = fileInfo1.Name;
    }
    if (!string.IsNullOrEmpty(fileExtension))
    {
      if (fileExtension.Contains("*."))
      {
        fileDialog.Filter = fileExtension;
      }
      else
      {
        fileDialog.Filter = string.Format("{0} Files|*.{0}|All Files|*.*", (object) fileExtension);
        fileDialog.DefaultExt = fileExtension;
      }
    }
    return fileDialog.ShowDialog().GetValueOrDefault() ? ((IEnumerable<string>) fileDialog.FileNames).Select<string, string>((Func<string, string>) (string_0 => FileSystemHelper.GetRelativePath(Directory.GetCurrentDirectory(), string_0))).ToArray<string>() : (string[]) null;
  }

  private void method_0(object sender, ExecutedRoutedEventArgs e)
  {
    string path = this.PathExample ?? this.Path;
    if (string.IsNullOrWhiteSpace(path))
      path = ComponentSettings<GuiControlsSettings>.Current.FilePathControlLastBrowsedPath;
    string[] strArray = !this.IsDirectory ? FilePathControl.ExecuteBrowse(path, this.attr.Behavior, this.attr.OverwritePrompt, this.attr.FileExtension, multiSelect: this.multiSelect) : FilePathControl.ExecuteBrowse(path, (FilePathAttribute.BehaviorChoice) 0, false, "", true);
    if (strArray == null || strArray.Length == 0)
      return;
    this.SetCurrentValue(FilePathControl.PathProperty, (object) strArray[0]);
    this.SetCurrentValue(FilePathControl.PathsProperty, (object) strArray);
    ComponentSettings<GuiControlsSettings>.Current.FilePathControlLastBrowsedPath = path;
  }

  private void This_Loaded(object sender, RoutedEventArgs e)
  {
    this.browseButton.Visibility = this.IsPath ? Visibility.Visible : Visibility.Collapsed;
    this.openButton.Visibility = this.IsDirectory || this.multiSelect ? Visibility.Collapsed : Visibility.Visible;
  }

  private void method_1(object sender, ExecutedRoutedEventArgs e)
  {
    TraceSource source = Log.CreateSource("OpenFile");
    string str1 = this.Path;
    if (str1 == null)
    {
      string[] paths = this.Paths;
      string str2;
      if (paths == null)
      {
        str2 = (string) null;
      }
      else
      {
        str2 = ((IEnumerable<string>) paths).FirstOrDefault<string>();
        if (str2 != null)
        {
          str1 = str2;
          goto label_5;
        }
      }
      str1 = "";
    }
label_5:
    string path = str1;
    string fullPath;
    try
    {
      fullPath = System.IO.Path.GetFullPath(path);
    }
    catch (Exception ex)
    {
      Log.Error(source, "Caught error while opening file: {0}", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(source, ex);
      return;
    }
    if (System.IO.Path.GetExtension(fullPath).Contains(".TapPlan"))
    {
      try
      {
        string fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
        if (fileName.Contains("Editor.exe"))
        {
          Process.Start(new ProcessStartInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName), "tap.exe"), $"editor --open \"{System.IO.Path.GetFullPath(fullPath)}\"")
          {
            UseShellExecute = false,
            CreateNoWindow = true
          });
          return;
        }
      }
      catch (Exception ex)
      {
        Log.Error(source, "Caught error while opening file: {0}", new object[1]
        {
          (object) ex.Message
        });
        Log.Debug(source, ex);
      }
    }
    try
    {
      Process.Start(fullPath);
    }
    catch (Exception ex)
    {
      Log.Error(source, "Caught error while opening file: {0}", new object[1]
      {
        (object) ex.Message
      });
      Log.Debug(source, ex);
    }
  }

  private void method_2(object sender, CanExecuteRoutedEventArgs e)
  {
    if (this.IsDirectory)
      return;
    string str1 = this.Path;
    if (str1 == null)
    {
      string[] paths = this.Paths;
      string str2;
      if (paths == null)
      {
        str2 = (string) null;
      }
      else
      {
        str2 = ((IEnumerable<string>) paths).FirstOrDefault<string>();
        if (str2 != null)
        {
          str1 = str2;
          goto label_6;
        }
      }
      str1 = "";
    }
label_6:
    string str3 = str1;
    e.CanExecute = FilePathControl.smethod_0(str3) && File.Exists(str3);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property != FilePathControl.PathProperty)
      return;
    CommandManager.InvalidateRequerySuggested();
  }

  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  [DebuggerNonUserCode]
  public void InitializeComponent()
  {
    if (this.bool_0)
      return;
    this.bool_0 = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/Keysight.OpenTap.Wpf;component/controlproviders/filepathcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("PresentationBuildTasks", "8.0.0.0")]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.This = (FilePathControl) target;
        this.This.Loaded += new RoutedEventHandler(this.This_Loaded);
        break;
      case 2:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_0);
        break;
      case 3:
        ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.method_1);
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.method_2);
        break;
      case 4:
        this.browseButton = (System.Windows.Controls.Button) target;
        break;
      case 5:
        this.openButton = (System.Windows.Controls.Button) target;
        break;
      default:
        this.bool_0 = true;
        break;
    }
  }

  [CompilerGenerated]
  internal static bool smethod_0(string string_0)
  {
    return !string.IsNullOrWhiteSpace(string_0) && string_0.IndexOfAny(System.IO.Path.GetInvalidPathChars()) < 0;
  }
}
