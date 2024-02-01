using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 一般thing的namespace统一用Jtext103.CFET2.Things+thing名命名
namespace Jtext103.CFET2.Things.ExampleThing
{
    // 继承Thing
    public class ExampleThing : Thing
    {
        private float num = 0;
        private ExampleThingConfig myConfig;

        // 复写两个函数，这这里完全一些初始化和起始工作
        public override void TryInit(object path)
        {
            // 实例化配置文件，配置文件的挂载路径查看Cfet2ProgramAddThingsPartial
            // 如果是动态挂载的配置方法，则需要在运行路径下添加ThingConfig里的配置
            myConfig = new ExampleThingConfig(getConfigFilePath((string)path));
            // 调用配置文件的配置
            var x = myConfig.portName;
        }

        public override void Start()
        {
            // CFET中有事件的发布订阅，用法如下
            // MyHub.EventHub.Subscribe(new EventFilter(@"/echo/callback", EventFilter.DefaultEventType, eventLevel), eventHandler);
            // MyHub.EventHub.Publish("/idtest/" + channel.ToString(), EventFilter.DefaultEventType, id);
        }

        // CFET种有3种资源类型，States，Config，Method
        // 通过以下方式最终会暴露成CR，通过http可以被外界访问

        // 这是STATUS，只能读
        [Cfet2Status]
        public float getnum()
        {
            Random random = new Random();
            return random.Next(0, 100);
        }

        // 这是Config。有两个函数组成一个config，分别是对应读写

        [Cfet2Config(ConfigActions = ConfigAction.Get, Name = "setNum")]

        public float getNum()
        {
            return num;
        }

        [Cfet2Config(ConfigActions = ConfigAction.Set, Name = "setNum")]
        public void setNum(float x, float y)
        {
            num = y;
        }

        // method

        [Cfet2Method]
        public void printA()
        {
            Console.WriteLine("aaaa");
        }

    }
}
