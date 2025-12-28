// Decompiled with JetBrains decompiler
// Type: Keysight.OpenTap.Wpf.WeakEventHandler
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
namespace Keysight.OpenTap.Wpf;

public class WeakEventHandler
{
  private static ConditionalWeakTable<WeakEventHandler, WeakEventHandler.Attacher> conditionalWeakTable_0 = new ConditionalWeakTable<WeakEventHandler, WeakEventHandler.Attacher>();
  private PropertyChangedEventHandler handler;

  public WeakEventHandler(INotifyPropertyChanged notify, PropertyChangedEventHandler handler)
  {
    WeakEventHandler.Attacher attacher = new WeakEventHandler.Attacher(notify, handler);
    WeakEventHandler.conditionalWeakTable_0.Add(this, attacher);
    this.handler = handler;
  }

  ~WeakEventHandler()
  {
    WeakEventHandler.Attacher attacher;
    if (!WeakEventHandler.conditionalWeakTable_0.TryGetValue(this, out attacher))
      return;
    attacher.Unload();
  }

  private class Attacher
  {
    private WeakReference<PropertyChangedEventHandler> weakReference_0;
    private INotifyPropertyChanged notify;
    private static int int_0;

    public Attacher(INotifyPropertyChanged notify, PropertyChangedEventHandler handler)
    {
      this.notify = notify;
      notify.PropertyChanged += new PropertyChangedEventHandler(this.method_0);
      ++WeakEventHandler.Attacher.int_0;
      this.weakReference_0 = new WeakReference<PropertyChangedEventHandler>(handler);
    }

    private void method_0(object sender, PropertyChangedEventArgs e)
    {
      PropertyChangedEventHandler target;
      if (this.weakReference_0.TryGetTarget(out target))
        target(sender, e);
      else
        this.Unload();
    }

    public void Unload()
    {
      --WeakEventHandler.Attacher.int_0;
      this.notify.PropertyChanged -= new PropertyChangedEventHandler(this.method_0);
    }
  }
}
