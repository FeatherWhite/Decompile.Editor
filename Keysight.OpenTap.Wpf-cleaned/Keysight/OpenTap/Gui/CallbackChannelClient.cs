// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.CallbackChannelClient
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.IO.Pipes;
using System.Text;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class CallbackChannelClient
{
  public static string GetPipeName(int PID) => "TAPGUI_CLB" + PID.ToString();

  public static bool SendReload(int PID, string planName, string filename)
  {
    try
    {
      NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", CallbackChannelClient.GetPipeName(PID), PipeDirection.Out);
      try
      {
        pipeClientStream.Connect(100);
        byte[] bytes1 = Encoding.UTF8.GetBytes(planName);
        byte[] bytes2 = Encoding.UTF8.GetBytes(filename);
        byte[] bytes3 = BitConverter.GetBytes(bytes1.Length);
        byte[] bytes4 = BitConverter.GetBytes(bytes2.Length);
        pipeClientStream.WriteByte((byte) 1);
        pipeClientStream.Write(bytes3, 0, 4);
        pipeClientStream.Write(bytes1, 0, bytes1.Length);
        pipeClientStream.Write(bytes4, 0, 4);
        pipeClientStream.Write(bytes2, 0, bytes2.Length);
        pipeClientStream.Flush();
        pipeClientStream.Dispose();
        return true;
      }
      finally
      {
        pipeClientStream.Close();
      }
    }
    catch
    {
      return false;
    }
  }

  public static void SendExit(int PID)
  {
    try
    {
      NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", CallbackChannelClient.GetPipeName(PID), PipeDirection.Out);
      try
      {
        pipeClientStream.Connect(100);
        pipeClientStream.WriteByte((byte) 2);
        pipeClientStream.Flush();
        pipeClientStream.Dispose();
      }
      finally
      {
        pipeClientStream.Close();
      }
    }
    catch
    {
    }
  }
}
