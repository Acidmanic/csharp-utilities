using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Acidmanic.Utilities.Extensions;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd003Compression : TddBase
    {
     


        public override void Main()
        {
            
            var original = "This is a very not compressed value\n";

            original += original;
            original += original;
            original += original;

            var method = Compressions.Brotli;
            
            var start = DateTime.Now;
            
            var compressed = original.CompressAsync(method).Result;

            var compressionTime = DateTime.Now.Subtract(start);
            
            start = DateTime.Now;
            
            var decompressed = compressed.DecompressAsync(method).Result;

            var deCompressionTime = DateTime.Now.Subtract(start);
            
            Console.WriteLine($"Compression and Decompression using {method}");
            
            Console.WriteLine($"Original: {original.Length}\n {original}");
            
            Console.WriteLine($"Compressed: {compressed.Length}\n {compressed}, took {compressionTime.TotalMilliseconds} ms");
            
            Console.WriteLine($"Decompressed: {decompressed.Length}\n {decompressed}, took {deCompressionTime.TotalMilliseconds} ms");
                        
        }
    }
}