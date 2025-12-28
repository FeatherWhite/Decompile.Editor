// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.Callback
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class Callback : Freezable
{
  public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof (EventName), typeof (string), typeof (Callback));
  public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof (Source), typeof (object), typeof (Callback));
  public static readonly DependencyProperty Sync1Property = DependencyProperty.Register(nameof (Sync1), typeof (object), typeof (Callback));
  public static readonly DependencyProperty Sync2Property = DependencyProperty.Register(nameof (Sync2), typeof (object), typeof (Callback));
  private Action action_0;

  public event EventHandler<CallbackEventArgs> Handler;

  private void method_0(object[] object_0)
  {
    // ISSUE: reference to a compiler-generated field
    if (this.eventHandler_0 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    this.eventHandler_0((object) this, new CallbackEventArgs(object_0));
  }

  public string EventName
  {
    get => (string) this.GetValue(Callback.EventNameProperty);
    set => this.SetValue(Callback.EventNameProperty, (object) value);
  }

  public object Source
  {
    get => this.GetValue(Callback.SourceProperty);
    set => this.SetValue(Callback.SourceProperty, value);
  }

  public object Sync1
  {
    get => this.GetValue(Callback.Sync1Property);
    set => this.SetValue(Callback.Sync1Property, value);
  }

  public object Sync2
  {
    get => this.GetValue(Callback.Sync2Property);
    set => this.SetValue(Callback.Sync2Property, value);
  }

  protected override void OnPropertyChanged(
    DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs_0)
  {
    base.OnPropertyChanged(dependencyPropertyChangedEventArgs_0);
    if (dependencyPropertyChangedEventArgs_0.Property == Callback.Sync1Property)
      this.Sync2 = this.Sync1;
    if (dependencyPropertyChangedEventArgs_0.Property == Callback.Sync2Property)
      this.Sync1 = this.Sync2;
    if (dependencyPropertyChangedEventArgs_0.Property != Callback.EventNameProperty && dependencyPropertyChangedEventArgs_0.Property != Callback.SourceProperty)
      return;
    if (this.action_0 != null)
      this.action_0();
    this.action_0 = (Action) null;
    this.method_1();
  }

  private void method_1()
  {
    string eventName = this.EventName;
    object item = this.Source;
    if (eventName == null || item == null)
      return;
    EventInfo eventInfo_0 = item.GetType().GetEvent(eventName);
    if (eventInfo_0 == (EventInfo) null)
      return;
    Delegate action = Callback.smethod_0(eventInfo_0.EventHandlerType, new Action<object[]>(this.method_0));
    eventInfo_0.AddEventHandler(item, action);
    this.action_0 = (Action) (() => eventInfo_0.RemoveEventHandler(item, action));
  }

  private static Delegate smethod_0(Type type_0, Action<object[]> action_1)
  {
    ParameterExpression[] array = ((IEnumerable<ParameterInfo>) type_0.GetMethod("Invoke").GetParameters()).Select<ParameterInfo, ParameterExpression>((Func<ParameterInfo, ParameterExpression>) (parameterInfo_0 => System.Linq.Expressions.Expression.Parameter(parameterInfo_0.ParameterType, parameterInfo_0.Name))).ToArray<ParameterExpression>();
    NewArrayExpression newArrayExpression = System.Linq.Expressions.Expression.NewArrayInit(typeof (object), (System.Linq.Expressions.Expression[]) ((IEnumerable<ParameterExpression>) array).Select<ParameterExpression, UnaryExpression>((Func<ParameterExpression, UnaryExpression>) (parameterExpression_0 => System.Linq.Expressions.Expression.TypeAs((System.Linq.Expressions.Expression) parameterExpression_0, typeof (object)))).ToArray<UnaryExpression>());
    InvocationExpression body = System.Linq.Expressions.Expression.Invoke((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) action_1), (System.Linq.Expressions.Expression) newArrayExpression);
    return System.Linq.Expressions.Expression.Lambda(type_0, (System.Linq.Expressions.Expression) body, array).Compile();
  }

  protected override Freezable CreateInstanceCore() => (Freezable) this;
}
