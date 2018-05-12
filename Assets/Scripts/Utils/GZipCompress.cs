//===================================================
//Author      : DRB
//CreateTime  ：3/3/2017 4:20:58 PM
//Description ：
//===================================================
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

public class GZipCompress
{

    /// <summary>
    /// 将字节数组压缩，返回压缩后的字节数组
    /// </summary>
    public static byte[] Compress(byte[] bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        MemoryStream ms = new MemoryStream();
        GZipOutputStream stream = new GZipOutputStream(ms);
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();

        byte[] result = ms.ToArray();
        ms.Close();

        return result;
    }

    /// <summary>
    /// 返回被解压的字节数组
    /// </summary>
    public static byte[] DeCompress(byte[] bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        MemoryStream ms = new MemoryStream(bytes);
        GZipInputStream stream = new GZipInputStream(ms);

        MemoryStream buffer = new MemoryStream();
        int count = 0;
        byte[] temp = new byte[1024];
        while ((count = stream.Read(temp, 0, temp.Length)) != 0)
        {
            buffer.Write(temp, 0, count);
        }
        ms.Close();
        stream.Close();

        byte[] result = buffer.ToArray();
        buffer.Close();

        return result;
    }

}
