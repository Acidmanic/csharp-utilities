using System;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd003Compression : TddBase
    {
     


        public override void Main()
        {

            var method = Compressions.GZip;
            
            var original = "This is a very not compressed value";

            var start = DateTime.Now;
            
            var compressed = original.CompressAsync(method).Result;

            var compressionTime = DateTime.Now.Subtract(start);
            
            start = DateTime.Now;
            
            var decompressed = compressed.DecompressAsync(method).Result;

            var deCompressionTime = DateTime.Now.Subtract(start);
            
            Console.WriteLine($"Compression and Decompression using {method}");
            
            Console.WriteLine($"Original: {original}");
            
            Console.WriteLine($"Compressed: {compressed}, took {compressionTime.TotalMilliseconds} ms");
            
            Console.WriteLine($"Decompressed: {original}, took {deCompressionTime.TotalMilliseconds} ms");
            
        }
    }
}