using System;
using System.IO.Compression;
using System.Threading.Tasks;
using Acidmanic.Utilities.Extensions;

namespace Acidmanic.Utilities
{
    

    public static class StringCompressionExtensions
    {
        /// <summary>
        /// Based of selected Compression type, compresses the input string and returns a base64 string in result.
        /// </summary>
        /// <param name="value">string to be compressed</param>
        /// <param name="compression">Specifies the compression algorithm or if the string must be compressed at all.</param>
        /// <param name="level">Compression level (Vs Performance)</param>
        /// <returns>Compressed string data in base64 format.</returns>
        [Obsolete("This extension has been moved to Acidmanic.Utilities.Extensions.StringCompressionExtensions")]
        public static Task<string> CompressAsync(this string value, Compressions compression,
            CompressionLevel level = CompressionLevel.Fastest)
        {
            return Acidmanic.Utilities.Extensions.StringCompressionExtensions
                .CompressAsync(value, compression, level);
        }
        
        /// <summary>
        /// Takes a base64 string format as input data which is considered to be the result of a compression on a
        /// string data. Returns the de compressed result as string. 
        /// </summary>
        /// <param name="value">base64 string representing the compressed data</param>
        /// <param name="compression">Specifies the compression algorithm or if the string is compressed at all.</param>
        /// <returns>DeCompressed string data as string.</returns>
        [Obsolete("This extension has been moved to Acidmanic.Utilities.Extensions.StringCompressionExtensions")]
        public static Task<string> DecompressAsync(this string value, Compressions compression)
        {
            return Acidmanic.Utilities.Extensions.StringCompressionExtensions.DecompressAsync(value, compression);
        }
    }
}