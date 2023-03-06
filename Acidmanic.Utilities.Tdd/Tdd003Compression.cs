using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd003Compression : TddBase
    {
     


        public override void Main()
        {
            
            var original = "This is a very not compressed value";

            var data = Encoding.Unicode.GetBytes(original);
            
            var sourceStream = new MemoryStream(data);

            var outStream = new MemoryStream();
            
            var compressionStream = new GZipStream(outStream,CompressionLevel.Fastest);

            sourceStream.CopyTo(compressionStream);
            
            compressionStream.Flush();

            data = outStream.ToArray();

            compressionStream.Dispose();
            
            outStream.Dispose();
            
            sourceStream.Dispose();

            var converted = Convert.ToBase64String(data);


            var reBytes = Convert.FromBase64String(converted);
            
            var reSourceStream = new MemoryStream(reBytes);

            var reOutStream = new MemoryStream();

            var decompressStream = new GZipStream(reSourceStream, CompressionMode.Decompress);
            
            decompressStream.CopyTo(reOutStream);
            reOutStream.Flush();

            reBytes = reOutStream.GetBuffer();

            var reConstructed = Encoding.Unicode.GetString(reBytes);
            
            
            var method = Compressions.Brotli;
            
            

            var start = DateTime.Now;
            
            var compressed = original.CompressAsync(method).Result;

            var compressionTime = DateTime.Now.Subtract(start);
            
            start = DateTime.Now;
            
            var decompressed = compressed.DecompressAsync(method).Result;

            var deCompressionTime = DateTime.Now.Subtract(start);
            
            Console.WriteLine($"Compression and Decompression using {method}");
            
            Console.WriteLine($"Original: {original}");
            
            Console.WriteLine($"Compressed: {compressed}, took {compressionTime.TotalMilliseconds} ms");
            
            Console.WriteLine($"Decompressed: {decompressed}, took {deCompressionTime.TotalMilliseconds} ms");
            
        }
    }
}