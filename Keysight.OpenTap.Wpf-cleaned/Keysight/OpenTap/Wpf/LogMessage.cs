// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.LogMessage
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using OpenTap;
using OpenTap.Diagnostic;
using System;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class LogMessage
{
  public string Message;
  public string Source;

  public virtual LogMessage Clone()
  {
    return new LogMessage(this.Level, this.Message, this.Source, this.Time);
  }

  public DateTime Time { get; set; }

  public string FormattedTime => string.Format("{0:HH:mm:ss.fff}", (object) this.Time);

  public LogEventType Level { get; set; }

  public LogMessage(LogEventType level, string message, string source, DateTime date)
  {
    this.Time = date;
    this.Level = level;
    this.Source = source;
    this.Message = message;
  }

  public LogMessage(Event evnt)
    : this((LogEventType) evnt.EventType, evnt.Message, evnt.Source, new DateTime(evnt.Timestamp))
  {
  }

  public LogMessage()
  {
  }

  public override string ToString()
  {
    DateTime time = this.Time;
    return $"{this.FormattedTime}  {this.Source,-12} {this.Message}";
  }
}
