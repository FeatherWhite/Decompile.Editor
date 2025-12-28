// Decompiled with JetBrains decompiler
// Type: OpenTap.ExecutorSubProcess
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class ExecutorSubProcess : IDisposable
{
  private ProcessStartInfo start;
  private CancellationTokenSource cancellationTokenSource_0 = new CancellationTokenSource();

  private NamedPipeServerStream Pipe { get; set; }

  private Process Process { get; set; }

  public int WaitForExit()
  {
    if (this.Process == null)
      throw new InvalidOperationException("Process has not been started");
    this.Process.WaitForExit();
    return this.Process.ExitCode;
  }

  public ExecutorSubProcess(ProcessStartInfo start) => this.start = start;

  private void method_0()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    new ExecutorSubProcess.\u003C\u003Ec__DisplayClass12_0()
    {
      \u003C\u003E4__this = this,
      buffer = new byte[1024 /*0x0400*/]
    }.method_0();
  }

  public event EventHandler<string> MessageReceived;

  private void method_1(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, string_0);
  }

  public static ExecutorSubProcess Create(string name, string args, bool isolated = false)
  {
    ProcessStartInfo start = new ProcessStartInfo(name, args)
    {
      WorkingDirectory = Path.GetFullPath(Directory.GetCurrentDirectory()),
      UseShellExecute = false,
      RedirectStandardInput = false,
      RedirectStandardOutput = true,
      RedirectStandardError = true
    };
    if (isolated)
      start.Environment[ExecutorSubProcess.EnvVarNames.ParentProcessExeDir] = ExecutorClient.ExeDir;
    return new ExecutorSubProcess(start);
  }

  public static NamedPipeServerStream getStream(out string pipeName)
  {
    while (true)
    {
      ref string local = ref pipeName;
      Guid guid = Guid.NewGuid();
      string str1;
      string str2 = str1 = guid.ToString().Replace("-", "");
      local = str1;
      string pipeName1 = str2;
      try
      {
        return new NamedPipeServerStream(pipeName1, PipeDirection.In, 10, PipeTransmissionMode.Message, PipeOptions.WriteThrough);
      }
      catch (UnauthorizedAccessException ex)
      {
        Thread.Sleep(100);
      }
    }
  }

  public void Start()
  {
    string pipeName;
    this.Pipe = ExecutorSubProcess.getStream(out pipeName);
    this.start.Environment[ExecutorSubProcess.EnvVarNames.TpmInteropPipeName] = pipeName;
    this.Pipe.WaitForConnectionAsync().ContinueWith((Action<Task>) (task_0 => this.method_0()));
    this.Process = Process.Start(this.start);
    this.Process.EnableRaisingEvents = true;
    this.method_2(this.Process.StandardOutput, new Action<string>(Console.Write));
    this.method_2(this.Process.StandardError, new Action<string>(Console.Error.Write));
  }

  private async Task method_2(StreamReader streamReader_0, Action<string> action_0)
  {
    char[] buffer = new char[256 /*0x0100*/];
    TaskAwaiter<int> awaiter;
    while (true)
    {
      awaiter = streamReader_0.ReadAsync(buffer, 0, buffer.Length).GetAwaiter();
      if (awaiter.IsCompleted)
      {
        int result;
        if ((result = awaiter.GetResult()) > 0)
          action_0(new string(buffer, 0, result));
        else
          goto label_5;
      }
      else
        break;
    }
    // ISSUE: explicit reference operation
    // ISSUE: reference to a compiler-generated field
    (^this).\u003C\u003E1__state = 0;
    TaskAwaiter<int> taskAwaiter = awaiter;
    // ISSUE: explicit reference operation
    // ISSUE: reference to a compiler-generated field
    (^this).\u003C\u003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, ExecutorSubProcess.\u003CRedirectOutput\u003Ed__21>(ref awaiter, this);
    return;
label_5:
    buffer = (char[]) null;
  }

  public void Dispose()
  {
    this.cancellationTokenSource_0.Cancel();
    if (this.Pipe != null)
      this.Pipe.Dispose();
    if (this.Process == null)
      return;
    this.Process.Dispose();
  }

  public void Env(string Name, string Value) => this.start.Environment[Name] = Value;

  public class EnvVarNames
  {
    public static string TpmInteropPipeName = "TPM_PIPE";
    public static string ParentProcessExeDir = "TPM_PARENTPROCESSDIR";
  }
}
