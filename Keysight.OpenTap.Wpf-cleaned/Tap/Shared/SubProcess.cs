// Decompiled with JetBrains decompiler
// Type: Tap.Shared.SubProcess
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Tap.Shared;

internal class SubProcess
{
  private Process process_0;

  public Process Process => this.process_0;

  private void method_0(string string_0, IEnumerable<string> ienumerable_0)
  {
    this.Load(string_0, ienumerable_0);
    this.Start();
  }

  private void method_1(string string_0, string string_1)
  {
    this.Load(string_0, string_1);
    this.Start();
  }

  public void Load(string fileName, string args)
  {
    ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, args)
    {
      UseShellExecute = false,
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      CreateNoWindow = true
    };
    this.process_0 = new Process()
    {
      StartInfo = processStartInfo,
      EnableRaisingEvents = true
    };
    this.process_0.OutputDataReceived += new DataReceivedEventHandler(this.process_0_OutputDataReceived);
  }

  public void Load(string fileName, IEnumerable<string> args)
  {
    string args1 = string.Join(" ", args.Select<string, string>((Func<string, string>) (string_0 => !string_0.Contains<char>(' ') ? string_0 : $"\"{string_0}\"")));
    this.Load(fileName, args1);
  }

  public event Action<string> DataReceived;

  private void process_0_OutputDataReceived(object sender, DataReceivedEventArgs e)
  {
    // ISSUE: reference to a compiler-generated field
    Action<string> action0 = this.action_0;
    if (action0 == null)
      return;
    action0(e.Data);
  }

  public void Start()
  {
    this.process_0.Start();
    this.process_0.BeginOutputReadLine();
  }

  public static SubProcess Run(string command)
  {
    SubProcess subProcess = new SubProcess();
    string[] source = command.Split(' ');
    subProcess.method_0(((IEnumerable<string>) source).First<string>(), (IEnumerable<string>) ((IEnumerable<string>) source).Skip<string>(1).ToArray<string>());
    return subProcess;
  }

  public static SubProcess Run(string fileName, string args)
  {
    SubProcess subProcess = new SubProcess();
    subProcess.method_1(fileName, args);
    return subProcess;
  }

  public static SubProcess Run(string fileName, IEnumerable<string> args)
  {
    SubProcess subProcess = new SubProcess();
    subProcess.method_0(fileName, args);
    return subProcess;
  }
}
