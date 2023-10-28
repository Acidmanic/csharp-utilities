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

        sourceStream.CopyToAsync(compressionStream);

        compressionStream.FlushAsync();

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

            decompressStream.CopyToAsync(outStream);

            outStream.Flush();

            data = outStream.GetBuffer();
        }

        return data;
    }
}