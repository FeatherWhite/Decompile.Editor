// Decompiled with JetBrains decompiler
// Type: OpenTap.MemoryMappedApi
// Assembly: Keysight.OpenTap.Wpf, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DD8C74A-BB26-40BC-BEC9-164CC341DF18
// Assembly location: D:\Software\de4dot-cex\EditorCE\Keysight.OpenTap.Wpf-cleaned.dll

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class MemoryMappedApi : IDisposable
{
  public readonly string Name;
  private MemoryMappedFile memoryMappedFile_0;
  private MemoryStream memoryStream_0 = new MemoryStream();
  private uint uint_0;
  private long long_0 = 4;

  public MemoryMappedApi(string name)
  {
    this.Name = name;
    try
    {
      this.memoryMappedFile_0 = MemoryMappedFile.OpenExisting(this.Name);
      using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
        viewAccessor.Write(0L, this.uint_0 = viewAccessor.ReadUInt32(0L) + 1U);
    }
    catch (FileNotFoundException ex)
    {
      this.uint_0 = 0U;
    }
  }

  public MemoryMappedApi()
    : this(Guid.NewGuid().ToString())
  {
  }

  public void Persist()
  {
    if (this.memoryMappedFile_0 != null)
      this.memoryMappedFile_0.Dispose();
    this.memoryMappedFile_0 = MemoryMappedFile.CreateNew(this.Name, 4L + this.memoryStream_0.Length);
    this.memoryStream_0.Position = 0L;
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(4L, this.memoryStream_0.Length))
    {
      this.memoryStream_0.CopyTo((Stream) viewStream);
      viewStream.Flush();
    }
  }

  public void WaitForHandover()
  {
    while (this.memoryMappedFile_0 != null)
    {
      using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
      {
        if ((int) this.uint_0 != (int) viewAccessor.ReadUInt32(0L))
          break;
        TapThread.Sleep(20);
      }
    }
  }

  public Task WaitForHandoverAsync() => Task.Run(new Action(this.WaitForHandover));

  public void Write<T>(T data) where T : IConvertible
  {
    if (typeof (T) == typeof (string))
    {
      this.Write((string) (object) data);
    }
    else
    {
      TypeCode typeCode = Type.GetTypeCode(data.GetType());
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Object:
          throw new NotSupportedException("Type {0} is not a primitive type. See TypeCode to find supported versions of T[]");
        default:
          this.method_1(typeCode);
          this.method_0(this.method_5((object) data));
          break;
      }
    }
  }

  public void Write<T>(T[] data) where T : IConvertible
  {
    TypeCode typeCode = Type.GetTypeCode(typeof (T));
    switch (typeCode)
    {
      case TypeCode.Empty:
      case TypeCode.Object:
        throw new NotSupportedException("Type {0} is not a primitive type. See TypeCode to find supported versions of T[]");
      default:
        this.method_1((TypeCode) (64 /*0x40*/ + (int) (byte) typeCode));
        this.method_0(this.method_6<T>(data));
        break;
    }
  }

  public void Write(string data)
  {
    this.method_1(TypeCode.String);
    this.method_0(Encoding.UTF8.GetBytes(data));
  }

  public void ReadRewind() => this.long_0 = 4L;

  public Stream ReadStream()
  {
    long long0 = this.long_0;
    int size = 0;
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(this.long_0, 0L, MemoryMappedFileAccess.Read))
    {
      int num = (int) this.method_2((Stream) viewStream);
      size = this.method_4((Stream) viewStream);
      long0 += viewStream.Position;
      this.long_0 += viewStream.Position + (long) size;
    }
    return (Stream) this.memoryMappedFile_0.CreateViewStream(long0, (long) size, MemoryMappedFileAccess.ReadWrite);
  }

  public T Read<T>() => (T) this.Read();

  public virtual void Dispose()
  {
    if (this.memoryMappedFile_0 == null)
      return;
    this.memoryMappedFile_0.Dispose();
    this.memoryMappedFile_0 = (MemoryMappedFile) null;
  }

  public object Read()
  {
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(this.long_0, 0L, MemoryMappedFileAccess.Read))
    {
      TypeCode typeCode1 = this.method_2((Stream) viewStream);
      object obj;
      if ((byte) typeCode1 > (byte) 64 /*0x40*/)
      {
        TypeCode typeCode2 = typeCode1 - 64 /*0x40*/;
        obj = (object) this.method_7(this.method_3((Stream) viewStream), Utils.TypeOf(typeCode2));
      }
      else
        obj = this.method_9(this.method_3((Stream) viewStream), Utils.TypeOf(typeCode1));
      this.long_0 += viewStream.Position;
      return obj;
    }
  }

  private void method_0(byte[] byte_0)
  {
    byte[] bytes = BitConverter.GetBytes(byte_0.Length);
    this.memoryStream_0.Write(bytes, 0, bytes.Length);
    this.memoryStream_0.Write(byte_0, 0, byte_0.Length);
  }

  private void method_1(TypeCode typeCode_0) => this.memoryStream_0.WriteByte((byte) typeCode_0);

  private TypeCode method_2(Stream stream_0) => (TypeCode) stream_0.ReadByte();

  private byte[] method_3(Stream stream_0)
  {
    byte[] buffer1 = new byte[4];
    stream_0.Read(buffer1, 0, buffer1.Length);
    byte[] buffer2 = new byte[BitConverter.ToInt32(buffer1, 0)];
    stream_0.Read(buffer2, 0, buffer2.Length);
    return buffer2;
  }

  private int method_4(Stream stream_0)
  {
    byte[] buffer = new byte[4];
    stream_0.Read(buffer, 0, buffer.Length);
    return BitConverter.ToInt32(buffer, 0);
  }

  private byte[] method_5(object object_0)
  {
    byte[] numArray = new byte[Marshal.SizeOf(object_0)];
    GCHandle gcHandle = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
    Marshal.StructureToPtr(object_0, gcHandle.AddrOfPinnedObject(), false);
    gcHandle.Free();
    return numArray;
  }

  private byte[] method_6<T>(T[] gparam_0)
  {
    if (gparam_0.Length == 0)
      return new byte[0];
    byte[] destination = new byte[Marshal.SizeOf<T>(gparam_0[0]) * gparam_0.Length];
    GCHandle gcHandle = GCHandle.Alloc((object) destination, GCHandleType.Pinned);
    Marshal.Copy(Marshal.UnsafeAddrOfPinnedArrayElement<T>(gparam_0, 0), destination, 0, destination.Length);
    gcHandle.Free();
    return destination;
  }

  private Array method_7(byte[] byte_0, Type type_0)
  {
    if (type_0 == typeof (byte))
      return (Array) byte_0;
    int num = Marshal.SizeOf(type_0);
    Array instance = Array.CreateInstance(type_0, byte_0.Length / num);
    GCHandle.Alloc((object) instance, GCHandleType.Pinned);
    IntPtr destination = Marshal.UnsafeAddrOfPinnedArrayElement(instance, 0);
    Marshal.Copy(byte_0, 0, destination, byte_0.Length);
    return instance;
  }

  private T method_8<T>(byte[] byte_0)
  {
    GCHandle gcHandle = GCHandle.Alloc((object) byte_0, GCHandleType.Pinned);
    T structure = (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof (T));
    gcHandle.Free();
    return structure;
  }

  private object method_9(byte[] byte_0, Type type_0)
  {
    if (type_0 == typeof (string))
      return (object) Encoding.UTF8.GetString(byte_0);
    GCHandle gcHandle = GCHandle.Alloc((object) byte_0, GCHandleType.Pinned);
    object structure = Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), type_0);
    gcHandle.Free();
    return structure;
  }
}
