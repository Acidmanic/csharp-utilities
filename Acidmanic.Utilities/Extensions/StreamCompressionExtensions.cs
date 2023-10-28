using System;
using System.IO;
using System.IO.Compression;

namespace Acidmanic.Utilities.Extensions;

internal static class StreamCompressionExtensions
{
    public static Stream GetCompressionStream(
        this Stream stream,
        Compressions compression,
        CompressionMode mode,
        CompressionLevel level)
    {
        if (compression == Compressions.Brotli)
        {
            if (mode == CompressionMode.Compress)
            {
                return new BrotliStream(stream, level);
            }

            if (mode == CompressionMode.Decompress)
            {
                return new BrotliStream(stream, CompressionMode.Decompress);
            }
        }

        if (compression == Compressions.GZip)
        {
            if (mode == CompressionMode.Compress)
            {
                return new GZipStream(stream, level);
            }

            if (mode == CompressionMode.Decompress)
            {
                return new GZipStream(stream, CompressionMode.Decompress);
            }
        }

        throw new Exception("You called compression/decompression methods for none compression.");
    }
}