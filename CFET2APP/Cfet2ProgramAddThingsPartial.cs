using Jtext103.CFET2.Core;
using Jtext103.CFET2.NancyHttpCommunicationModule;
using Jtext103.CFET2.Core.BasicThings;
using Jtext103.CFET2.Core.Middleware.Basic;
using Jtext103.CFET2.CFET2App.DynamicLoad;
using Jtext103.CFET2.Core.Config;
using Jtext103.CFET2.WebsocketEvent;
using Jtext103.CFET2.Things.ExampleThing;

namespace Jtext103.CFET2.CFET2App
{
    public partial class Cfet2Program : CFET2Host
    {
        private void AddThings()
        {
            //------------------------------Pipeline------------------------------//
            MyHub.Pipeline.AddMiddleware(new ResourceInfoMidware());
            MyHub.Pipeline.AddMiddleware(new NavigationMidware());

            GlobalConfig.Populate("./GlobalConfig.json");

            //------------------------------Nancy HTTP通信模块------------------------------//
            var nancyCM = new NancyCommunicationModule(GlobalConfig.HostUri, GlobalConfig.Accept);
            MyHub.TryAddCommunicationModule(nancyCM);

            //you can add Thing by coding here

            //------------------------------Custom View------------------------------//
            var customView = new CustomViewThing();
            MyHub.TryAddThing(customView, "/", "customView", "./CustomView");

            //---------------------Required Event: ws-------------------------------//
            var remoteHub = new WebsocketEventThing();
            MyHub.TryAddThing(remoteHub, @"/", "WsEvent", "");
            MyHub.EventHub.RemoteEventHubs.Add(remoteHub);

            //If you don't want dynamic load things, please comment out the line below
            //var loader = new DynamicThingsLoader(this);


            //---------------------------------------Example--------------------------------------------//

            // 如果需要手动挂载，则需要CFET2App先引用/依赖该项目，添加using+namespace之后用如下方式挂载
            // 指定CR的路径，以及配置文件所在的路径
            // 这里配置文件对应的路径是\jtext103_cfet2app\CFET2APP\bin\Debug\net461\ExampleConfig.json

            var ExampleThing = new ExampleThing();
            MyHub.TryAddThing(ExampleThing, "/", "ExampleThing", "./ExampleConfig.json");
        }
    }
}
