using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Acidmanic.Utilities.DataTypes;
using Acidmanic.Utilities.Extensions;
using Acidmanic.Utilities.Reflection.Attributes;
using Acidmanic.Utilities.Reflection.Extensions;
using Acidmanic.Utilities.Reflection.ObjectTree;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd006StringSerializable : TddBase
    {


        class Model
        {
            public long Id { get; set; }
            
            public string Name { get; set; }
            
        }

        class BiggerModel
        {
            [TreatAsLeaf]
            public JsonData<Model> Model { get; set; }
            
            public string Surname { get; set; }
        }
        
        
        public override void Main()
        {


            var myString = "This is the text!";

            var compressed = myString.CompressAsync(Compressions.GZip).Result;

            var reString = compressed.DecompressAsync(Compressions.GZip).Result;
            
            // test 1 pass;

            var bytes = new byte[] {1,2,3,4,5,6,7,8,9,0,9,8,7,6,5,4,3,2,1 };

            var compressedBytes = bytes.Compress();

            var reBytes = compressedBytes.Decompress();

            var smallData = new Model
            {
                Id = 12,
                Name = "Farimehr"
            };
            
            var maData = new BiggerModel
            {
                Model = smallData,
                Surname = "Ayerian"
            };

            StringSerializable<Model> serializable = smallData;

            var modelObject = serializable.ModelObject;

            string stringValue = serializable;

            StringSerializable<Model> reSerialized = stringValue;

            Model model = reSerialized;
            
            
        }
    }
}