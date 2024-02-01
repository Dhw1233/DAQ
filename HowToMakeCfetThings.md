# 如何做一个 CFET Thing

`可以参照CFET2APP中ExampleThing看具体thing的代码实现`

## CFET 基本概念

> CFET（Control system Framework for Experimental Devices Toolkit）是课题自主研发的用于大型实验设施的控制框架。

* CFET将控制和通讯都抽象成了对资源的访问。在CFET抽象框架下，控制系统中的一切都是资源，每个活动都是在访问资源。

* 资源被定义成了5种类型，分别为Thing（对象），Status（状态），Config（配置），Method（方法）和Event（事件）。
    
    Status只可读。比如Thing指代一台空调，它的当前开关指示灯的亮灭就是该Thing的一个Status
    
    Config是Thing的配置，比如空调的设置温度，是外界希望空调能达到的温度。Config是可读可写的
    
    Method可以理解成外界对Thing的一个指令，Method不一定会返回值。

* Thing在大多数时候指代控制系统中的对象，但是也可以指代某个具有一定功能的控制模块。thing可以是物理上的，也可以是逻辑上存在的。比如控制系统中控制的高压电源，这个高压电源是一个具体的对象，是一个thing。thing也可以指，tcp通讯的功能实现，也可以是一个thing。

* 在CFET代码中，一个thing就是一个类，继承Thing这个父类。

* Thing中需要暴露给外界访问的成员就是资源，Thing中可以包括Status，Config，Method和Event这些资源。

## 什么是 CFET resource（CR）

Thing会对外暴露资源，这些对外暴露的资源会用url的方式暴露出来，用户可以基于http协议访问这些资源。

比如 名叫card的这个thing，有一个status，用来表示card的类型，对应的CR为 /card/type，用户可以通过这个url获取card的类型，这个url就叫CR

CR的类型有Status，Config和Method

在调用CR的时候，分别对应三个action，get,set,invoke

## 如何写一个 Thing

1. 首先要有一个CFET2APP，然后在CFET2APP的sln里面创建一个Net6.0的项目，并在项目的依赖中安装CFET的工具包，即配好开发环境。

2. 创建一个Class，使其继承Thing的基类，这样这个Class就可以使用Thing的所有功能。然后按照指定格式将对外暴露的接口成员标识成属性。

    继承Thing基类之后，可以复写init和start方法，同时基类也会给子类提供Hub，Hub包含很多cfet功能

    thing还需要引用Jtext103.CFET2.Core.Attributes，获得cfet自定义的一些attribute

3. 在开发thing的过程中，需要对外暴露出来的资源，分类型，分别用不同的attribute去修饰函数，这个函数就是要对外暴露的cr，具体可以参照ExampleThing

### Thing 的依赖注入

`cfet基于AutoFac通过构造函数的方式实现依赖注入，在动态加载的时候通过反射拿到对应的类，然后把这个类通过构造函数的方式注入到目标类中`

具体的应用例子就是AIThing，AIThing的构造函数依赖BasicAI，BasicAI有很多子类。所以使用上就变成，在CFET2APP中放对应BasicAI某个子类的dll，在动态加载中，通过反射拿到basicai子类，那么注入的就是这个子类。

### 如何实现 Status

```cs
[Cfet2Status(Name = "StatusName")]   // 或者

[Cfet2Status]
public float col(float y)
{
    return y;
}
```

这里的函数名最后就会变成cr的一部分，比如这个thing名字叫TestThing，这个资源暴露出来之后，对应的cr就是，/TestThing/col，cr类型是Status

参数也是通过cr的方式交互，比如上述函数需要输入参数y=2，用户则调用 /TestThing/col/2

### 如何实现 Config
```cs
// config的装饰器有
[Cfet2Config]
[Cfet2Config(Name = " ConfigName ")]
[Cfet2Config(ConfigActions= ConfigAction.Action,Name = " ConfigName ")]
```
其中Config比较复杂，因为要对应Get和Set两个动作。Config支持修饰方法和属性。ConfigActions中常用的有Get，Set以及GetAndSet。GetAndSet用于默认修饰属性，因为属性本身就可以提供读取和设置的内容。方法则需要指定Get还是Set。

> 具体实现也可以结合ExampleThing一起看

config的attribute也是用来修饰函数，比如
```cs
[Cfet2Config(ConfigActions = ConfigAction.Get, Name = "setNum")]
public float getNum(float x){}

[Cfet2Config(ConfigActions = ConfigAction.Set, Name = "setNum")]
public void setNum(float x,float y){}
```

函数的参数，则是通过cr的方式交互，比如上述x=2，y=4，则调用cr /TestThing/setNum/2/4

### 如何实现 Method
```cs
[Cfet2Method]
[Cfet2Method(Name = " MethodName ")]
```
method在用法上和status相似，但是意义不一样。Status是读取资源状态，method则是对thing下达一些指令，比如 读取 灯的开关状态是Status，而“开灯”、“关灯”这样的指令则用method实现

## 如何访问资源

这里首先讲一下 HUB 怎么用，
然后讲一下怎么用 url 访问 CR，这个关键就是 URL 里面的内容怎么对应到实现这个 CR 的 thing 的方法属性参数上的。

这个 core 那个英文文档里面比较全面，但是可能有些地方过时了，你检重点写，然后可以放个到那个文档的链接。

## 如何发布订阅事件

### 什么是cfet事件？

cfet事件是cfet的资源类型之一，我们知道资源在cfet中用cr表示，可以使用http获取资源。但是如果你想订阅一个资源，当资源发生变化时可以感知，使用http就做不到了，cfet事件向用户提供了资源的发布订阅功能，分为订阅方和发布方，订阅方订阅cr，发布方在需要的时候推送data

`更详细的介绍可以查看cfet core的repo`

### 本地事件

cfet本地事件，说的是，同一个cfet2app内的相互订阅，不需要经过外部的通讯协议，下面简单讲解以下

```CS
// 本地订阅
 MyHub.EventHub.Subscribe(new EventFilter(@"/DummyThing/abc", EventFilter.DefaultEventType, eventLevel), receiveDatehandler);

// 订阅方注册回调函数
 private void receiveDatehandler(EventArg e)
{
    Console.WriteLine(e.Sample);
}

//====================================================================
// 发布方发布
 MyHub.EventHub.Publish(@"/DummyThing/abc","changed", 100);
```

本地事件的订阅，填写相对路径的cr就可以，不需要带有http的完整CR，订阅主要有两个参数，一个是EventFilter（包含订阅的目标资源CR，订阅事件类型，事件级别等信息），另一个是用户自己注册的回调函数，在订阅的事件有消息时，会触发回调函数

### 远程事件

远程事件是发布方和订阅方不在一个cfet2app中，分别运行在不同的端口或者主机上。

`远程事件通过webSocket实现的，默认的ws端口是CFET2APP的端口+1`，比如当前的app跑在8004，那么默认的ws端口号是8005

**`订阅方远程订阅事件的时候，不需要自己+1，+1在订阅函数里面自动处理了，用户只需要输入完全的cr就行`**

```CS
// 比如需要订阅192.168.2.20上跑在8004端口的app里面的cr
 MyHub.EventHub.Subscribe(new EventFilter(@"http://192.168.2.20/DummyThing/abc", EventFilter.DefaultEventType, eventLevel), receiveDatehandler);

//====================================================================
// 192.168.2.20上跑在8004端口的app发布方发布
 MyHub.EventHub.Publish(@"/DummyThing/abc","changed", 100);
```

**发布方只需要填写相对路径就行，这和远程事件的实现原理有关，远程事件的订阅实际上订阅了目标的本地事件，所以不管是远程还是本地事件，发布方的使用都可以保持一致**
