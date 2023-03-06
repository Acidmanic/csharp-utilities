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

            await stream.CopyToAsync(output);
            await output.FlushAsync();

            return Encoding.Unicode.GetString(output.ToArray());
        }

        public static Task<string> CompressAsync(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            if (compression == Compressions.GZip || compression == Compressions.Brotli)
            {
                return ToCompressedStringAsync(value, level, compression, s => new GZipStream(s, level));
            }

            return Task.FromResult(value);
        }


        private static Task<string> DecompressAsync(this string value, Compressions compression)
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