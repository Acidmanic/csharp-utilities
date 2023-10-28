using System.IO;
using System.IO.Compression;
using System.Text;

namespace Acidmanic.Utilities.Extensions;

public static class ByteArrayCompressionExtensions
{
    public static byte[] Compress(
        this byte[] data,
        Compressions compression = Compressions.GZip,
        CompressionLevel level = CompressionLevel.Optimal)
    {
        using var sourceStream = new MemoryStream(data);

        using var outStream = new MemoryStream();

        using var compressionStream = outStream.GetCompressionStream
            (compression, CompressionMode.Compress, level);

        sourceStream.CopyTo(compressionStream);

        compressionStream.Flush();

        data = outStream.ToArray();

        return data;
    }

    public static byte[] Decompress(this byte[] data, Compressions compression = Compressions.GZip)
    {
        if (compression == Compressions.Brotli || compression == Compressions.GZip)
        {
            using var sourceStream = new MemoryStream(data);

            using var outStream = new MemoryStream();

            using var decompressStream = sourceStream.GetCompressionStream
                (compression, CompressionMode.Decompress, CompressionLevel.NoCompression);

            decompressStream.CopyTo(outStream);

            decompressStream.Flush();

            data = outStream.ToArray();
        }

        return data;
    }
}