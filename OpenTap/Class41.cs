// Decompiled with JetBrains decompiler
// Type: OpenTap.Class41
// Assembly: Editor, Version=9.17.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C70E47C-9445-4F2F-A326-AD7B83F3019A
// Assembly location: D:\Software\de4dot-cex\EditorCE\Editor-cleaned.exe

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace OpenTap;

internal class Class41 : IDisposable
{
  public readonly string string_0;
  private MemoryMappedFile memoryMappedFile_0;
  private MemoryStream memoryStream_0 = new MemoryStream();
  private uint uint_0;
  private long long_0 = 4;

  public Class41(string string_1)
  {
    this.string_0 = string_1;
    try
    {
      this.memoryMappedFile_0 = MemoryMappedFile.OpenExisting(this.string_0);
      using (MemoryMappedViewAccessor viewAccessor = this.memoryMappedFile_0.CreateViewAccessor())
        viewAccessor.Write(0L, this.uint_0 = viewAccessor.ReadUInt32(0L) + 1U);
    }
    catch (FileNotFoundException ex)
    {
      this.uint_0 = 0U;
    }
  }

  public Class41()
    : this(Guid.NewGuid().ToString())
  {
  }

  public void method_0()
  {
    if (this.memoryMappedFile_0 != null)
      this.memoryMappedFile_0.Dispose();
    this.memoryMappedFile_0 = MemoryMappedFile.CreateNew(this.string_0, 4L + this.memoryStream_0.Length);
    this.memoryStream_0.Position = 0L;
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(4L, this.memoryStream_0.Length))
    {
      this.memoryStream_0.CopyTo((Stream) viewStream);
      viewStream.Flush();
    }
  }

  public void method_1()
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

  public Task method_2() => Task.Run(new Action(this.method_1));

  public void method_3<T>(T gparam_0) where T : IConvertible
  {
    if (typeof (T) == typeof (string))
    {
      this.method_5((string) (object) gparam_0);
    }
    else
    {
      TypeCode typeCode = Type.GetTypeCode(gparam_0.GetType());
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Object:
          throw new NotSupportedException("Type {0} is not a primitive type. See TypeCode to find supported versions of T[]");
        default:
          this.method_11(typeCode);
          this.method_10(this.method_15((object) gparam_0));
          break;
      }
    }
  }

  public void method_4<T>(T[] gparam_0) where T : IConvertible
  {
    TypeCode typeCode = Type.GetTypeCode(typeof (T));
    switch (typeCode)
    {
      case TypeCode.Empty:
      case TypeCode.Object:
        throw new NotSupportedException("Type {0} is not a primitive type. See TypeCode to find supported versions of T[]");
      default:
        this.method_11((TypeCode) (64 /*0x40*/ + (int) (byte) typeCode));
        this.method_10(this.method_16<T>(gparam_0));
        break;
    }
  }

  public void method_5(string string_1)
  {
    this.method_11(TypeCode.String);
    this.method_10(Encoding.UTF8.GetBytes(string_1));
  }

  public void method_6() => this.long_0 = 4L;

  public Stream method_7()
  {
    long long0 = this.long_0;
    int size = 0;
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(this.long_0, 0L, MemoryMappedFileAccess.Read))
    {
      int num = (int) this.method_12((Stream) viewStream);
      size = this.method_14((Stream) viewStream);
      long0 += viewStream.Position;
      this.long_0 += viewStream.Position + (long) size;
    }
    return (Stream) this.memoryMappedFile_0.CreateViewStream(long0, (long) size, MemoryMappedFileAccess.ReadWrite);
  }

  public T method_8<T>() => (T) this.method_9();

  public virtual void Dispose()
  {
    if (this.memoryMappedFile_0 == null)
      return;
    this.memoryMappedFile_0.Dispose();
    this.memoryMappedFile_0 = (MemoryMappedFile) null;
  }

  public object method_9()
  {
    using (MemoryMappedViewStream viewStream = this.memoryMappedFile_0.CreateViewStream(this.long_0, 0L, MemoryMappedFileAccess.Read))
    {
      TypeCode typeCode_0_1 = this.method_12((Stream) viewStream);
      object obj;
      if ((byte) typeCode_0_1 > (byte) 64 /*0x40*/)
      {
        TypeCode typeCode_0_2 = typeCode_0_1 - 64 /*0x40*/;
        obj = (object) this.method_17(this.method_13((Stream) viewStream), Class24.smethod_34(typeCode_0_2));
      }
      else
        obj = this.method_19(this.method_13((Stream) viewStream), Class24.smethod_34(typeCode_0_1));
      this.long_0 += viewStream.Position;
      return obj;
    }
  }

  private void method_10(byte[] byte_0)
  {
    byte[] bytes = BitConverter.GetBytes(byte_0.Length);
    this.memoryStream_0.Write(bytes, 0, bytes.Length);
    this.memoryStream_0.Write(byte_0, 0, byte_0.Length);
  }

  private void method_11(TypeCode typeCode_0) => this.memoryStream_0.WriteByte((byte) typeCode_0);

  private TypeCode method_12(Stream stream_0) => (TypeCode) stream_0.ReadByte();

  private byte[] method_13(Stream stream_0)
  {
    byte[] buffer1 = new byte[4];
    stream_0.Read(buffer1, 0, buffer1.Length);
    byte[] buffer2 = new byte[BitConverter.ToInt32(buffer1, 0)];
    stream_0.Read(buffer2, 0, buffer2.Length);
    return buffer2;
  }

  private int method_14(Stream stream_0)
  {
    byte[] buffer = new byte[4];
    stream_0.Read(buffer, 0, buffer.Length);
    return BitConverter.ToInt32(buffer, 0);
  }

  private byte[] method_15(object object_0)
  {
    byte[] numArray = new byte[Marshal.SizeOf(object_0)];
    GCHandle gcHandle = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
    Marshal.StructureToPtr(object_0, gcHandle.AddrOfPinnedObject(), false);
    gcHandle.Free();
    return numArray;
  }

  private byte[] method_16<T>(T[] gparam_0)
  {
    if (gparam_0.Length == 0)
      return new byte[0];
    byte[] destination = new byte[Marshal.SizeOf<T>(gparam_0[0]) * gparam_0.Length];
    GCHandle gcHandle = GCHandle.Alloc((object) destination, GCHandleType.Pinned);
    Marshal.Copy(Marshal.UnsafeAddrOfPinnedArrayElement<T>(gparam_0, 0), destination, 0, destination.Length);
    gcHandle.Free();
    return destination;
  }

  private Array method_17(byte[] byte_0, Type type_0)
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

  private T method_18<T>(byte[] byte_0)
  {
    GCHandle gcHandle = GCHandle.Alloc((object) byte_0, GCHandleType.Pinned);
    T structure = (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof (T));
    gcHandle.Free();
    return structure;
  }

  private object method_19(byte[] byte_0, Type type_0)
  {
    if (type_0 == typeof (string))
      return (object) Encoding.UTF8.GetString(byte_0);
    GCHandle gcHandle = GCHandle.Alloc((object) byte_0, GCHandleType.Pinned);
    object structure = Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), type_0);
    gcHandle.Free();
    return structure;
  }
}
