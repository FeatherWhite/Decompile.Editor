// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.UpdateMonitor
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class UpdateMonitor : IOwnedAnnotation, IAnnotation, ICommandHandler
{
  public ICommandHandler CommandHandler;
  private static readonly ConditionalWeakTable<object, object> conditionalWeakTable_0 = new ConditionalWeakTable<object, object>();
  private static readonly object object_1 = new object();
  private bool bool_0;
  private readonly List<Action> list_0 = new List<Action>();
  public Action OnCommit;

  public object Owner { get; set; }

  public static void SetIsConnected(object thing)
  {
    if (thing == null || UpdateMonitor.conditionalWeakTable_0.TryGetValue(thing, out object _))
      return;
    UpdateMonitor.conditionalWeakTable_0.Add(thing, UpdateMonitor.object_1);
  }

  public static bool GetIsConnected(object thing)
  {
    object obj;
    return UpdateMonitor.conditionalWeakTable_0.TryGetValue(thing, out obj) && obj == UpdateMonitor.object_1;
  }

  public static event EventHandler<UpdateEventArgs> Events;

  public static ulong GlobalChangeRevision { get; private set; }

  public static void Update(object sender, AnnotationCollection annotation)
  {
    ++UpdateMonitor.GlobalChangeRevision;
    // ISSUE: reference to a compiler-generated field
    EventHandler<UpdateEventArgs> eventHandler0 = UpdateMonitor.eventHandler_0;
    if (eventHandler0 == null)
      return;
    eventHandler0(sender, new UpdateEventArgs(annotation));
  }

  public void RegisterSourceUpdated(FrameworkElement frameworkElement_0, Action action_0)
  {
    if (frameworkElement_0.IsLoaded)
      this.list_0.Add(action_0);
    ulong startChangeRevision = this.CurrentChangeRevision;
    frameworkElement_0.Loaded += (RoutedEventHandler) ((sender, e) =>
    {
      if ((long) startChangeRevision != (long) this.CurrentChangeRevision)
        action_0();
      this.list_0.Add(action_0);
    });
    frameworkElement_0.Unloaded += (RoutedEventHandler) ((sender, e) =>
    {
      startChangeRevision = this.CurrentChangeRevision;
      this.list_0.Remove(action_0);
    });
  }

  public ulong CurrentChangeRevision { get; private set; }

  public void PushUpdate()
  {
    this.CurrentChangeRevision = UpdateMonitor.GlobalChangeRevision;
    if (this.bool_0)
      return;
    this.bool_0 = true;
    try
    {
      foreach (Action action in this.list_0.ToArray())
        action();
    }
    finally
    {
      this.bool_0 = false;
    }
  }

  internal void Commit()
  {
    Action onCommit = this.OnCommit;
    if (onCommit == null)
      return;
    onCommit();
  }

  public void Read(object source)
  {
    if (source is IEnumerable enumerable)
    {
      foreach (object thing in enumerable)
        UpdateMonitor.SetIsConnected(thing);
    }
    UpdateMonitor.SetIsConnected(source);
  }

  public void Write(object source)
  {
  }

  public bool CanHandleCommand(string commandName, AnnotationCollection context)
  {
    ICommandHandler commandHandler = this.CommandHandler;
    return commandHandler != null && commandHandler.CanHandleCommand(commandName, context);
  }

  public void HandleCommand(string commandName, AnnotationCollection context)
  {
    this.CommandHandler?.HandleCommand(commandName, context);
  }

  public void CommandHandled(string commandName, AnnotationCollection context)
  {
    this.CommandHandler?.CommandHandled(commandName, context);
  }
}
