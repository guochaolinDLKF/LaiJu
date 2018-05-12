//===================================================
//Author      : DRB
//CreateTime  ：6/16/2017 2:47:03 PM
//Description ：
//===================================================
using System.IO;
using System;

public class ByteBuffer : MemoryStream
{
    private int size;

    private ByteBuffer(int size)
    {
        this.size = size;
    }

    private ByteBuffer(byte[] bytes) : base(bytes)
    {
        this.size = bytes.Length;
    }

    public static ByteBuffer allocate(int size)
    {
        return new ByteBuffer(size);
    }

    public int limit()
    {
        return this.size;
    }

    public static ByteBuffer wrap(byte[] bytes)
    {
        return new ByteBuffer(bytes);
    }

    public byte[] array()
    {
        return base.ToArray();
    }

    public void put(byte value)
    {
        base.WriteByte(value);
    }

    public void put(byte[] bytes)
    {
        base.Write(bytes, 0, bytes.Length);
    }

    public void putFloat(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        base.Write(bytes, 0, bytes.Length);
    }

    public void putShort(short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        base.Write(bytes, 0, bytes.Length);
    }

    public void putInt(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        base.Write(bytes, 0, bytes.Length);
    }

    public void putLong(long value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        base.Write(bytes, 0, bytes.Length);
    }

    public void putDouble(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        base.Write(bytes, 0, bytes.Length);
    }

    public byte get()
    {
        return (byte)base.ReadByte();
    }

    public void get(ref byte[] bytes, int begin, int size)
    {
        base.Read(bytes, begin, size);
    }

    public int getInt()
    {
        byte[] bytes = new byte[4];
        base.Read(bytes, 0, 4);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToInt32(bytes, 0);
    }

    public float getFloat()
    {
        byte[] bytes = new byte[4];
        base.Read(bytes, 0, 4);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToSingle(bytes, 0);
    }

    public long getLong()
    {
        byte[] bytes = new byte[8];
        base.Read(bytes, 0, 8);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToInt64(bytes, 0);
    }

    public double getDouble()
    {
        byte[] bytes = new byte[8];
        base.Read(bytes, 0, 8);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToDouble(bytes, 0);
    }

    public short getShort()
    {
        byte[] bytes = new byte[2];
        base.Read(bytes, 0, 2);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToInt16(bytes, 0);
    }
}
