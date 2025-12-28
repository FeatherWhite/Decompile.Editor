// Decompiled with JetBrains decompiler
// Type: OpenTap.ExecutorClient
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class ExecutorClient : IDisposable
{
  private Task task_0;
  private PipeStream pipeStream_0;

  public static bool IsRunningIsolated
  {
    get
    {
      return Environment.GetEnvironmentVariable(ExecutorSubProcess.EnvVarNames.ParentProcessExeDir) != null;
    }
  }

  public static bool IsExecutorMode
  {
    get
    {
      return Environment.GetEnvironmentVariable(ExecutorSubProcess.EnvVarNames.TpmInteropPipeName) != null;
    }
  }

  public static string ExeDir
  {
    get
    {
      if (ExecutorClient.IsRunningIsolated)
        return Environment.GetEnvironmentVariable(ExecutorSubProcess.EnvVarNames.ParentProcessExeDir);
      string location = Assembly.GetEntryAssembly()?.Location;
      if (string.IsNullOrEmpty(location))
        location = Assembly.GetExecutingAssembly().Location;
      return Path.GetDirectoryName(location);
    }
  }

  public ExecutorClient()
  {
    string environmentVariable = Environment.GetEnvironmentVariable(ExecutorSubProcess.EnvVarNames.TpmInteropPipeName);
    if (environmentVariable == null)
      return;
    NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", environmentVariable, PipeDirection.Out, PipeOptions.WriteThrough);
    this.task_0 = pipeClientStream.ConnectAsync();
    this.pipeStream_0 = (PipeStream) pipeClientStream;
  }

  public void Dispose() => this.pipeStream_0.Dispose();

  internal void MessageServer(string newname)
  {
    this.task_0.Wait();
    byte[] bytes = Encoding.UTF8.GetBytes(newname);
    this.pipeStream_0.Write(bytes, 0, bytes.Length);
  }
}
