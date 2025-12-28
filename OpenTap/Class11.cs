// Decompiled with JetBrains decompiler
// Type: OpenTap.Class11
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class Class11 : IDisposable
{
  private Task task_0;
  private PipeStream pipeStream_0;

  [SpecialName]
  public static bool smethod_0()
  {
    return Environment.GetEnvironmentVariable(Class8.Class9.string_1) != null;
  }

  [SpecialName]
  public static bool smethod_1()
  {
    return Environment.GetEnvironmentVariable(Class8.Class9.string_0) != null;
  }

  [SpecialName]
  public static string smethod_2()
  {
    if (Class11.smethod_0())
      return Environment.GetEnvironmentVariable(Class8.Class9.string_1);
    string location = Assembly.GetEntryAssembly()?.Location;
    if (string.IsNullOrEmpty(location))
      location = Assembly.GetExecutingAssembly().Location;
    return Path.GetDirectoryName(location);
  }

  public Class11()
  {
    string environmentVariable = Environment.GetEnvironmentVariable(Class8.Class9.string_0);
    if (environmentVariable == null)
      return;
    NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", environmentVariable, PipeDirection.Out, PipeOptions.WriteThrough);
    this.task_0 = pipeClientStream.ConnectAsync();
    this.pipeStream_0 = (PipeStream) pipeClientStream;
  }

  public void Dispose() => this.pipeStream_0.Dispose();

  internal void method_0(string string_0)
  {
    this.task_0.Wait();
    byte[] bytes = Encoding.UTF8.GetBytes(string_0);
    this.pipeStream_0.Write(bytes, 0, bytes.Length);
  }
}
