// Decompiled with JetBrains decompiler
// Type: OpenTap.Package.Ipc.Class60
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace OpenTap.Package.Ipc;

internal class Class60 : IDisposable
{
  protected MemoryMappedFile memoryMappedFile_0;
  private Mutex mutex_0;
  private string string_0;

  public Class60(string string_1, string string_2)
  {
    this.string_0 = string_2 != null ? Path.GetFullPath(Path.Combine(string_2, string_1)).Replace('\\', '/') : throw new ArgumentNullException("dir");
    bool createdNew;
    this.mutex_0 = new Mutex(true, "OpenTap.Package " + BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(this.string_0))).Replace("-", ""), out createdNew);
    if (!createdNew)
      this.mutex_0.WaitOne();
    this.memoryMappedFile_0 = MemoryMappedFile.CreateFromFile(new FileStream(this.string_0, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), (string) null, 1024L /*0x0400*/, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, false);
    if (createdNew)
    {
      using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
        viewAccessor.WriteArray<byte>(0L, new byte[1024 /*0x0400*/], 0, 1024 /*0x0400*/);
    }
    this.mutex_0.ReleaseMutex();
  }

  public void Dispose()
  {
    if (this.memoryMappedFile_0 != null)
      this.memoryMappedFile_0.Dispose();
    this.memoryMappedFile_0 = (MemoryMappedFile) null;
    if (this.mutex_0 != null)
      this.mutex_0.Dispose();
    this.mutex_0 = (Mutex) null;
  }

  protected T method_0<T>(long long_0) where T : struct
  {
    T structure = default (T);
    using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
      viewAccessor.Read<T>(long_0, out structure);
    return structure;
  }

  protected void method_1<T>(long long_0, T gparam_0) where T : struct
  {
    using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
      viewAccessor.Write<T>(long_0, ref gparam_0);
  }
}
