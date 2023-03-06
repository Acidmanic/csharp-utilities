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
        private static string GetCompressionName(this Compressions com)
        {
            if (com == Compressions.Brotli)
            {
                return "Brotli";
            }

            if (com == Compressions.GZip)
            {
                return "GZip";
            }

            return null;
        }

        private static async Task<string> ToCompressedStringAsync(
            string value,
            CompressionLevel level,
            Compressions compression,
            Func<Stream, Stream> createCompressionStream)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = createCompressionStream(output);

            await input.CopyToAsync(stream);
            await stream.FlushAsync();

            var result = output.ToArray();

            return Convert.ToBase64String(result);
        }


        private static async Task<string> FromCompressedStringAsync(string value,
            Func<Stream, Stream> createDecompressionStream)
        {
            var bytes = Convert.FromBase64String(value);
            await using var input = new MemoryStream(bytes);
            await using var output = new MemoryStream();
            await using var stream = createDecompressionStream(input);

            var keepReading = true;

            const int bufferLength = 1024;
            byte[] buffer = new byte[bufferLength];
            
            while (keepReading)
            {
                int read = await input.ReadAsync(buffer, 0, bufferLength);

                await output.WriteAsync(buffer);

                keepReading = read == bufferLength;
            }
            
            await output.FlushAsync();

            return Encoding.Unicode.GetString(output.ToArray());
        }

        /// <summary>
        /// Based of selected Compression type, compresses the input string and returns a base64 string in result.
        /// </summary>
        /// <param name="value">string to be compressed</param>
        /// <param name="compression">Specifies the compression algorithm or if the string must be compressed at all.</param>
        /// <param name="level">Compression level (Vs Performance)</param>
        /// <returns>Compressed string data in base64 format.</returns>
        public static Task<string> CompressAsync(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            if (compression == Compressions.GZip || compression == Compressions.Brotli)
            {
                return ToCompressedStringAsync(value, level, compression, s => new GZipStream(s, level));
            }

            return Task.FromResult(value);
        }
        
        /// <summary>
        /// Takes a base64 string format as input data which is considered to be the result of a compression on a
        /// string data. Returns the de compressed result as string. 
        /// </summary>
        /// <param name="value">base64 string representing the compressed data</param>
        /// <param name="compression">Specifies the compression algorithm or if the string is compressed at all.</param>
        /// <returns>DeCompressed string data as string.</returns>
        public static Task<string> DecompressAsync(this string value, Compressions compression)
        {
            if (compression == Compressions.Brotli)
            {
                return FromCompressedStringAsync(value, s => new BrotliStream(s, CompressionMode.Decompress));
            }

            if (compression == Compressions.GZip)
            {
                return FromCompressedStringAsync(value, s => new GZipStream(s, CompressionMode.Decompress));
            }

            return Task.FromResult(value);
        }
    }
}