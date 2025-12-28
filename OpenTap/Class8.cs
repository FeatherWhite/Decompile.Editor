// Decompiled with JetBrains decompiler
// Type: OpenTap.Class8
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class Class8 : IDisposable
{
  private ProcessStartInfo processStartInfo_0;
  private CancellationTokenSource cancellationTokenSource_0 = new CancellationTokenSource();

  public int method_4()
  {
    // ISSUE: reference to a compiler-generated method
    if (this.method_2() == null)
      throw new InvalidOperationException("Process has not been started");
    // ISSUE: reference to a compiler-generated method
    this.method_2().WaitForExit();
    // ISSUE: reference to a compiler-generated method
    return this.method_2().ExitCode;
  }

  public Class8(ProcessStartInfo processStartInfo_1)
  {
    this.processStartInfo_0 = processStartInfo_1;
  }

  private void method_5()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    new Class8.Class10()
    {
      class8_0 = this,
      byte_0 = new byte[1024 /*0x0400*/]
    }.method_0();
  }

  [CompilerGenerated]
  [SpecialName]
  public void method_6(EventHandler<string> eventHandler_1)
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler<string> eventHandler = this.eventHandler_0;
    EventHandler<string> comparand;
    do
    {
      comparand = eventHandler;
      // ISSUE: reference to a compiler-generated field
      eventHandler = Interlocked.CompareExchange<EventHandler<string>>(ref this.eventHandler_0, comparand + eventHandler_1, comparand);
    }
    while (eventHandler != comparand);
  }

  [CompilerGenerated]
  [SpecialName]
  public void method_7(EventHandler<string> eventHandler_1)
  {
    // ISSUE: reference to a compiler-generated field
    EventHandler<string> eventHandler = this.eventHandler_0;
    EventHandler<string> comparand;
    do
    {
      comparand = eventHandler;
      // ISSUE: reference to a compiler-generated field
      eventHandler = Interlocked.CompareExchange<EventHandler<string>>(ref this.eventHandler_0, comparand - eventHandler_1, comparand);
    }
    while (eventHandler != comparand);
  }

  private void method_8(string string_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, string_0);
  }

  public static Class8 smethod_0(string string_0, string string_1, bool bool_0 = false)
  {
    ProcessStartInfo processStartInfo_1 = new ProcessStartInfo(string_0, string_1)
    {
      WorkingDirectory = Path.GetFullPath(Directory.GetCurrentDirectory()),
      UseShellExecute = false,
      RedirectStandardInput = false,
      RedirectStandardOutput = true,
      RedirectStandardError = true
    };
    if (bool_0)
      processStartInfo_1.Environment[Class8.Class9.string_1] = Class11.smethod_2();
    return new Class8(processStartInfo_1);
  }

  public static NamedPipeServerStream smethod_1(out string string_0)
  {
    while (true)
    {
      ref string local = ref string_0;
      Guid guid = Guid.NewGuid();
      string str1;
      string str2 = str1 = guid.ToString().Replace("-", "");
      local = str1;
      string pipeName = str2;
      try
      {
        return new NamedPipeServerStream(pipeName, PipeDirection.In, 10, PipeTransmissionMode.Message, PipeOptions.WriteThrough);
      }
      catch (UnauthorizedAccessException ex)
      {
        Thread.Sleep(100);
      }
    }
  }

  public void method_9()
  {
    string string_0;
    // ISSUE: reference to a compiler-generated method
    this.method_1(Class8.smethod_1(out string_0));
    this.processStartInfo_0.Environment[Class8.Class9.string_0] = string_0;
    // ISSUE: reference to a compiler-generated method
    this.method_0().WaitForConnectionAsync().ContinueWith((Action<Task>) (task_0 => this.method_5()));
    // ISSUE: reference to a compiler-generated method
    this.method_3(Process.Start(this.processStartInfo_0));
    // ISSUE: reference to a compiler-generated method
    this.method_2().EnableRaisingEvents = true;
    // ISSUE: reference to a compiler-generated method
    this.method_10(this.method_2().StandardOutput, new Action<string>(Console.Write));
    // ISSUE: reference to a compiler-generated method
    this.method_10(this.method_2().StandardError, new Action<string>(Console.Error.Write));
  }

  private Task method_10(StreamReader streamReader_0, Action<string> action_0)
  {
    // ISSUE: variable of a compiler-generated type
    Class8.Struct4 stateMachine;
    // ISSUE: reference to a compiler-generated field
    stateMachine.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
    // ISSUE: reference to a compiler-generated field
    stateMachine.streamReader_0 = streamReader_0;
    // ISSUE: reference to a compiler-generated field
    stateMachine.action_0 = action_0;
    // ISSUE: reference to a compiler-generated field
    stateMachine.int_0 = -1;
    // ISSUE: reference to a compiler-generated field
    stateMachine.asyncTaskMethodBuilder_0.Start<Class8.Struct4>(ref stateMachine);
    // ISSUE: reference to a compiler-generated field
    return stateMachine.asyncTaskMethodBuilder_0.Task;
  }

  public void Dispose()
  {
    this.cancellationTokenSource_0.Cancel();
    // ISSUE: reference to a compiler-generated method
    if (this.method_0() != null)
    {
      // ISSUE: reference to a compiler-generated method
      this.method_0().Dispose();
    }
    // ISSUE: reference to a compiler-generated method
    if (this.method_2() == null)
      return;
    // ISSUE: reference to a compiler-generated method
    this.method_2().Dispose();
  }

  public void method_11(string string_0, string string_1)
  {
    this.processStartInfo_0.Environment[string_0] = string_1;
  }

  public class Class9
  {
    public static string string_0 = "TPM_PIPE";
    public static string string_1 = "TPM_PARENTPROCESSDIR";
  }
}
