// Decompiled with JetBrains decompiler
// Type: Tap.Shared.Class2
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Tap.Shared;

internal class Class2
{
  private Process process_0;

  [SpecialName]
  public Process method_0() => this.process_0;

  private void method_1(string string_0, IEnumerable<string> ienumerable_0)
  {
    this.method_4(string_0, ienumerable_0);
    this.method_7();
  }

  private void method_2(string string_0, string string_1)
  {
    this.method_3(string_0, string_1);
    this.method_7();
  }

  public void method_3(string string_0, string string_1)
  {
    ProcessStartInfo processStartInfo = new ProcessStartInfo(string_0, string_1)
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

  public void method_4(string string_0_1, IEnumerable<string> ienumerable_0)
  {
    string string_1 = string.Join(" ", ienumerable_0.Select<string, string>((Func<string, string>) (string_0_2 => !string_0_2.Contains<char>(' ') ? string_0_2 : $"\"{string_0_2}\"")));
    this.method_3(string_0_1, string_1);
  }

  [CompilerGenerated]
  [SpecialName]
  public void method_5(Action<string> action_1)
  {
    // ISSUE: reference to a compiler-generated field
    Action<string> action = this.action_0;
    Action<string> comparand;
    do
    {
      comparand = action;
      // ISSUE: reference to a compiler-generated field
      action = Interlocked.CompareExchange<Action<string>>(ref this.action_0, comparand + action_1, comparand);
    }
    while (action != comparand);
  }

  [CompilerGenerated]
  [SpecialName]
  public void method_6(Action<string> action_1)
  {
    // ISSUE: reference to a compiler-generated field
    Action<string> action = this.action_0;
    Action<string> comparand;
    do
    {
      comparand = action;
      // ISSUE: reference to a compiler-generated field
      action = Interlocked.CompareExchange<Action<string>>(ref this.action_0, comparand - action_1, comparand);
    }
    while (action != comparand);
  }

  private void process_0_OutputDataReceived(object sender, DataReceivedEventArgs e)
  {
    // ISSUE: reference to a compiler-generated field
    Action<string> action0 = this.action_0;
    if (action0 == null)
      return;
    action0(e.Data);
  }

  public void method_7()
  {
    this.process_0.Start();
    this.process_0.BeginOutputReadLine();
  }

  public static Class2 smethod_0(string string_0)
  {
    Class2 class2 = new Class2();
    string[] source = string_0.Split(' ');
    class2.method_1(((IEnumerable<string>) source).First<string>(), (IEnumerable<string>) ((IEnumerable<string>) source).Skip<string>(1).ToArray<string>());
    return class2;
  }

  public static Class2 smethod_1(string string_0, string string_1)
  {
    Class2 class2 = new Class2();
    class2.method_2(string_0, string_1);
    return class2;
  }

  public static Class2 smethod_2(string string_0, IEnumerable<string> ienumerable_0)
  {
    Class2 class2 = new Class2();
    class2.method_1(string_0, ienumerable_0);
    return class2;
  }
}
