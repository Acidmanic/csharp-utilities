using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Acidmanic.Utilities
{
    public enum Compressions
    {
        None = -1,
        GZip = 0,
        Brotli = 1
    }

    public static class StringCompressionExtensions
    {
        /// <summary>
        /// Based of selected Compression type, compresses the input string and returns a base64 string in result.
        /// </summary>
        /// <param name="value">string to be compressed</param>
        /// <param name="compression">Specifies the compression algorithm or if the string must be compressed at all.</param>
        /// <param name="level">Compression level (Vs Performance)</param>
        /// <returns>Compressed string data in base64 format.</returns>
        public static async Task<string> CompressAsync(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            if (compression == Compressions.GZip || compression == Compressions.Brotli)
            {
                var data = Encoding.Unicode.GetBytes(value);

                using var sourceStream = new MemoryStream(data);

                using var outStream = new MemoryStream();

                await using var compressionStream = GetCompressionStream
                    (compression, outStream, CompressionMode.Compress, level);

                await sourceStream.CopyToAsync(compressionStream);

                await compressionStream.FlushAsync();

                data = outStream.ToArray();

                var converted = Convert.ToBase64String(data);

                return converted;
            }

            return value;
        }

        private static Stream GetCompressionStream(Compressions compression,
            Stream stream,
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

        /// <summary>
        /// Takes a base64 string format as input data which is considered to be the result of a compression on a
        /// string data. Returns the de compressed result as string. 
        /// </summary>
        /// <param name="value">base64 string representing the compressed data</param>
        /// <param name="compression">Specifies the compression algorithm or if the string is compressed at all.</param>
        /// <returns>DeCompressed string data as string.</returns>
        public static async Task<string> DecompressAsync(this string value, Compressions compression)
        {
            if (compression == Compressions.Brotli || compression == Compressions.GZip)
            {
                var data = Convert.FromBase64String(value);

                using var sourceStream = new MemoryStream(data);

                using var outStream = new MemoryStream();

                await using var decompressStream = GetCompressionStream
                    (compression, sourceStream, CompressionMode.Decompress, CompressionLevel.NoCompression);

                await decompressStream.CopyToAsync(outStream);

                outStream.Flush();

                data = outStream.GetBuffer();

                var reConstructed = Encoding.Unicode.GetString(data);

                return reConstructed;
            }


            return value;
        }
    }
}