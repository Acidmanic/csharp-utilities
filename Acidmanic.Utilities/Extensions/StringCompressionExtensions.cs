using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Acidmanic.Utilities.Extensions
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
        /// <returns>Compressed data as bytes.</returns>
        public static async Task<byte[]> CompressAsync(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            var data = Encoding.Unicode.GetBytes(value);
                
            if (compression == Compressions.GZip || compression == Compressions.Brotli)
            {
                using var sourceStream = new MemoryStream(data);

                using var outStream = new MemoryStream();

                await using var compressionStream = outStream.GetCompressionStream
                    (compression, CompressionMode.Compress, level);

                await sourceStream.CopyToAsync(compressionStream);

                await compressionStream.FlushAsync();

                data = outStream.ToArray();
                
                return data;
            }

            return data;
        }
        
        
        /// <summary>
        /// Based of selected Compression type, compresses the input string and returns a base64 string in result.
        /// </summary>
        /// <param name="value">string to be compressed</param>
        /// <param name="compression">Specifies the compression algorithm or if the string must be compressed at all.</param>
        /// <param name="level">Compression level (Vs Performance)</param>
        /// <returns>Compressed string data in base64 format.</returns>
        public static async Task<string> CompressB64Async(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            if (compression == Compressions.None) return value;

            var data = await CompressAsync(value, compression, level);
            
            var converted = Convert.ToBase64String(data);

            return converted;
        }
        
        /// <summary>
        /// Takes a base64 string format as input data which is considered to be the result of a compression on a
        /// string data. Returns the de compressed result as string. 
        /// </summary>
        /// <param name="data">input byte[] representing the compressed data</param>
        /// <param name="compression">Specifies the compression algorithm or if the string is compressed at all.</param>
        /// <returns>DeCompressed string data as string.</returns>
        public static async Task<string> DecompressAsync(this byte[] data, Compressions compression)
        {
            
            if (compression == Compressions.Brotli || compression == Compressions.GZip)
            {

                using var sourceStream = new MemoryStream(data);

                using var outStream = new MemoryStream();

                await using var decompressStream = sourceStream.GetCompressionStream
                    (compression, CompressionMode.Decompress, CompressionLevel.NoCompression);

                await decompressStream.CopyToAsync(outStream);

                await decompressStream.FlushAsync();

                data = outStream.ToArray();
            }
            
            var reConstructed = Encoding.Unicode.GetString(data);

            return reConstructed;
        }
        
        /// <summary>
        /// Takes a base64 string format as input data which is considered to be the result of a compression on a
        /// string data. Returns the de compressed result as string. 
        /// </summary>
        /// <param name="value">base64 string representing the compressed data</param>
        /// <param name="compression">Specifies the compression algorithm or if the string is compressed at all.</param>
        /// <returns>DeCompressed string data as string.</returns>
        public static async Task<string> DecompressB64Async(this string value, Compressions compression)
        {

            if (compression == Compressions.None) return value;
            
                var data = Convert.FromBase64String(value);

                var reConstructed = await DecompressAsync(data, compression);

                return reConstructed;
            
        }
    }
}