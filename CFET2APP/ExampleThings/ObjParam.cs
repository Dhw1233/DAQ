using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.ExampleThings
{
    public  class ObjParam : Thing
    {
        [Cfet2Config]
        public Ract MyRact { get; set; } = new Ract();

        [Cfet2Status("defaultStatus")]
        public string GetDefaultStatus()
        {
            return this.Path + @"/defaultStatus";
        }

        [Cfet2Method]
        public Ract  Replace(float x ,float y, float h, float w)
        {
            MyRact.X = x;
            MyRact.Y = y;
            MyRact.H = h;
            MyRact.W = w;
            return MyRact;

        }

        [Cfet2Method]
        public Ract Scale(float xFactor,float yFactor)
        {
            MyRact.W = xFactor;
            MyRact.H = yFactor;
            return MyRact;
        }

        public override void TryInit(object initObj)
        {
            base.TryInit(initObj);
            if (initObj != null)
            {
                MyRact=initObj as Ract;
            }
            Console.WriteLine($"useless init with : { MyRact.ToString()}");
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"useless start with :  { MyRact.ToString()}");
        }

    }

    public class Ract
    {

        public string Name { get; set; } = "";
        public float W { get; set; }
        public float H { get; set; }


        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, X: {X}, Y: {Y}, H: {H}, W: {W}";
        }

    }
    
}
