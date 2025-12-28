// Decompiled with JetBrains decompiler
// Type: OpenTap.Package.Ipc.SharedState
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace OpenTap.Package.Ipc;

internal class SharedState : IDisposable
{
  protected MemoryMappedFile share;
  private Mutex mutex_0;
  private string string_0;

  public SharedState(string name, string string_1)
  {
    this.string_0 = string_1 != null ? Path.GetFullPath(Path.Combine(string_1, name)).Replace('\\', '/') : throw new ArgumentNullException("dir");
    bool createdNew;
    this.mutex_0 = new Mutex(true, "OpenTap.Package " + BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(this.string_0))).Replace("-", ""), out createdNew);
    if (!createdNew)
      this.mutex_0.WaitOne();
    this.share = MemoryMappedFile.CreateFromFile(new FileStream(this.string_0, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), (string) null, 1024L /*0x0400*/, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
    if (createdNew)
    {
      using (MemoryMappedViewAccessor viewAccessor = this.share.CreateViewAccessor())
        viewAccessor.WriteArray<byte>(0L, new byte[1024 /*0x0400*/], 0, 1024 /*0x0400*/);
    }
    this.mutex_0.ReleaseMutex();
  }

  public void Dispose()
  {
    if (this.share != null)
      this.share.Dispose();
    this.share = (MemoryMappedFile) null;
    if (this.mutex_0 != null)
      this.mutex_0.Dispose();
    this.mutex_0 = (Mutex) null;
  }

  protected T Read<T>(long position) where T : struct
  {
    T structure = default (T);
    using (MemoryMappedViewAccessor viewAccessor = this.share.CreateViewAccessor())
      viewAccessor.Read<T>(position, out structure);
    return structure;
  }

  protected void Write<T>(long position, T value) where T : struct
  {
    using (MemoryMappedViewAccessor viewAccessor = this.share.CreateViewAccessor())
      viewAccessor.Write<T>(position, ref value);
  }
}
