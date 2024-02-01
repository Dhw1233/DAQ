using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.ExampleThings
{
    public  class UseLessThing:Thing
    {
        private string staticConfig="default";
        private int _base;
        [Cfet2Status("defaultStatus")]
        public string GetDefaultStatus()
        {
            return this.Path + @"/defaultStatus";
        }

        [Cfet2Config]
        public int Base
        {
            get
            {
                return _base;
            }
            set
            {
                if (_base != value)
                {
                    _base = value;
                    //exsample of publis event
                    MyHub.EventHub.Publish(GetPathFor("Base"), "changed", _base);
                }
            }
        }

        [Cfet2Status]
        public int Value(int input) => input* Base;

        [Cfet2Status]
        public int OtherBase(string otherBaseUri)
        {
            var otherBaseValue =(int) MyHub.TryGetResourceSampleWithUri(otherBaseUri).ObjectVal;
            return Base * otherBaseValue;
        }

        [Cfet2Method]
        public void Say(string msg)
        {
            if (msg == "gg")
            {
                throw new Exception("gg, intended exception");
            }
            Console.WriteLine("you said: "+msg);
        }

        public override void TryInit(object initObj)
        {
            base.TryInit(initObj);
            if (initObj != null)
            { 
                staticConfig = initObj.ToString();
            }
            //Console.WriteLine("useless init with : "+ staticConfig);
        }

        public override void Start()
        {
            base.Start();
            //Console.WriteLine("useless start with : " + staticConfig);
        }

    }
}
