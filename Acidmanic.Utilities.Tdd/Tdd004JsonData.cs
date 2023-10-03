using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Acidmanic.Utilities.DataTypes;
using Acidmanic.Utilities.Reflection.Attributes;
using Acidmanic.Utilities.Reflection.Extensions;
using Acidmanic.Utilities.Reflection.ObjectTree;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.Tdd
{
    public class Tdd004JsonData : TddBase
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

            var maData = new BiggerModel
            {
                Model = new Model
                {
                    Id = 12,
                    Name = "Farimehr"
                },
                Surname = "Ayerian"
            };
            
            var json = JsonConvert.SerializeObject(maData);
            
            json = json.Replace((char)0 + "", "");
            
            var ev = new ObjectEvaluator(maData);
            
            var falt = ev.ToStandardFlatData(o => o.FullTree());
            
            var reconstruct = JsonConvert.DeserializeObject<BiggerModel>(json);


            var mod = maData.Model;

            var essi = mod.CastTo(typeof(string));

            var dom = essi.CastTo(typeof(JsonData<Model>));
            
        }
    }
}