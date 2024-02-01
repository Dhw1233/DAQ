
using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Attributes;
using Jtext103.CFET2.Core.Event;
using Jtext103.CFET2.Core.Extension;
using Jtext103.CFET2.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.ExampleThings
{
    public class EventListener:Thing
    {
        //the regex expression, if you don't understand you can use exact match, or learn regex
        private ListenerConfig config =new ListenerConfig { Source= @"(\/(\w)+)*\/base", EventType="changed"} ;
        Token token;

        private ICfet2Logger logger;

        public override void TryInit(object initObj)
        {
            base.TryInit(initObj);
            
            //get a logger
            logger = Cfet2LogManager.GetLogger("Event");

            if (initObj != null)
            {
                config = (ListenerConfig)initObj.TryConvertTo(typeof(ListenerConfig));        
            }
            token=MyHub.EventHub.Subscribe(new EventFilter(config.Source, config.EventType), handler);
        }

        private void handler(EventArg e)
        {
            //Console.WriteLine($"{e.Source} has {e.EventType} to {e.Sample}");
            logger.Debug($"{e.Source} has {e.EventType} to {e.Sample}");
        }

        [Cfet2Method]
        public void UnSub()
        {
            token.Dispose();
        }
    }

    class ListenerConfig
    {
        public string Source { get; set; }
        public string EventType { get; set; }

    }
}
