// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Gui.CallbackChannels
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

#nullable disable
namespace Keysight.OpenTap.Gui;

public class CallbackChannels : IDisposable
{
  private bool bool_0;
  private Thread thread_0;
  private MainWindow window;
  private static CallbackChannels callbackChannels_0 = (CallbackChannels) null;
  private static int int_0 = 0;
  private static object object_0 = new object();

  internal static void smethod_0(MainWindow mainWindow_0)
  {
    lock (CallbackChannels.object_0)
    {
      ++CallbackChannels.int_0;
      if (CallbackChannels.callbackChannels_0 != null)
        return;
      CallbackChannels.callbackChannels_0 = new CallbackChannels(mainWindow_0);
    }
  }

  public CallbackChannels(MainWindow window)
  {
    this.window = window;
    this.thread_0 = new Thread(new ThreadStart(this.method_0));
    this.thread_0.SetApartmentState(ApartmentState.STA);
    this.thread_0.IsBackground = true;
    this.thread_0.Start();
  }

  private void method_0()
  {
    try
    {
      NamedPipeServerStream pipeServerStream = new NamedPipeServerStream(CallbackChannelClient.GetPipeName(Process.GetCurrentProcess().Id), PipeDirection.In, -1, PipeTransmissionMode.Message);
      try
      {
        while (!this.bool_0)
        {
          pipeServerStream.WaitForConnection();
          switch (pipeServerStream.ReadByte())
          {
            case 1:
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              CallbackChannels.Class66 class66 = new CallbackChannels.Class66();
              // ISSUE: reference to a compiler-generated field
              class66.callbackChannels_0 = this;
              byte[] buffer1 = new byte[4];
              pipeServerStream.Read(buffer1, 0, 4);
              byte[] numArray1 = new byte[BitConverter.ToInt32(buffer1, 0)];
              pipeServerStream.Read(numArray1, 0, numArray1.Length);
              byte[] buffer2 = new byte[4];
              pipeServerStream.Read(buffer2, 0, 4);
              byte[] numArray2 = new byte[BitConverter.ToInt32(buffer2, 0)];
              pipeServerStream.Read(numArray2, 0, numArray2.Length);
              pipeServerStream.Disconnect();
              string str = Encoding.UTF8.GetString(numArray1);
              // ISSUE: reference to a compiler-generated field
              class66.string_0 = Path.Combine(Path.GetTempPath(), "loadedFromDb.TapPlan");
              try
              {
                if (str.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
                {
                  if (str.Length > 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    class66.string_0 = Path.Combine(Path.GetTempPath(), str + ".TapPlan");
                  }
                }
              }
              catch
              {
              }
              try
              {
                // ISSUE: reference to a compiler-generated field
                File.WriteAllBytes(class66.string_0, numArray2);
                // ISSUE: reference to a compiler-generated method
                GuiHelpers.GuiInvoke((Action) new Action(class66.method_0));
                continue;
              }
              finally
              {
                // ISSUE: reference to a compiler-generated field
                File.Delete(class66.string_0);
              }
            case 2:
              pipeServerStream.Disconnect();
              --CallbackChannels.int_0;
              if (CallbackChannels.int_0 <= 0)
                return;
              continue;
            default:
              pipeServerStream.Disconnect();
              continue;
          }
        }
      }
      finally
      {
        pipeServerStream.Close();
      }
    }
    finally
    {
      lock (CallbackChannels.object_0)
        CallbackChannels.callbackChannels_0 = (CallbackChannels) null;
    }
  }

  public void Dispose()
  {
    this.bool_0 = true;
    this.thread_0.Abort();
  }
}
