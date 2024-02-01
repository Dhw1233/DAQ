# CFET2APP 是什么？

* CFET 的入口程序，生成的应用程序类型是控制台应用程序

* CFET 程序的大本营，后面添加的所有 Thing 都是挂载在 CFET2APP 上运行

## 1 如何使用？

### 1.1 运行与调试

* 运行：在 release 中下载运行文件夹

* 调试：如果需要调试代码，则先git clone CFET2APP， 有以下三种运行方式：
  * 打开 sln，把 CFET2App 设置为启动项目，目标框架可以选择 net461 或者 net6，目前 net461 是配置了 ExampleThing 的，点击 vs studio 上面的运行按钮
  * 进入\jtext103_cfet2app\CFET2APP\bin\Debug\net461 点击 exe 文件运行
  * 进入\jtext103_cfet2app\CFET2APP\bin\Debug\net6.0，在命令行输入 dotenet CFET2APP.dll

### 1.2 全局配置

1. 添加GlobalConfig.json文件，在里面配置cfet2app运行的端口号，交互信息的格式等等，具体json文件已经配置好在cfet2app repo的运行路径中，也可以参考release中的json文件
2. 添加Customview文件夹，该文件夹里面配置的是，WidgetUI中用户存下的界面，以及界面对应的哈希表（保存在CustomViewConfig.json文件中）等，具体的使用方式可以在widgetUI的说明文档中查看。
3. 添加thingConfig和thingDll文件夹，这两个文件夹都是作用于动态挂载

### 1.3 动态挂载Thing

将Thing的配置文件与依赖dll文件放到 CFETAPP 指定的地方，它会被自动挂载，步骤如下：

1. 首先在运行目录下找到ThingConfig文件夹，repo中在\jtext103_cfet2app\CFET2APP\bin\Debug\net461下

2. 在thingConfig文件夹下新建一个文件夹，文件夹的命名就是thing的挂载名，文件夹相对thingConfig的路径就是thing的挂载路径。例如，在thingConfig下面新建一个文件夹A，在A下面再新建一个B文件夹，如果在B文件夹中添加配置文件，那么这个thing的挂在路径就是"/A/B"

3. 在最后一层文件夹，添加对应thing的配置文件。

   1. 首先是Config.json，名字与格式统一，用于指定thing的类名（需要包括namespace）和thing的初始化配置文件（可以用[]表示多个）。ExampleThing的Config.json配置如下：

      ```json
      {
          "Type":"Jtext103.CFET2.Things.ExampleThing.ExampleThing",
          "InitObj":"ExampleConfig.json"
      }
      ```

   2. 其次是thing的初始化配置文件，与上述InitObj指定的一致	

4. 把thing所有相关的dll放到thingDll文件夹下

### 1.4 控制台常用指令

* list -a -d：打印挂载的所有 thing 的 CR，即资源路径，-d 显示 CR 类型

* get/set/invoke + CR：控制具体 CR

## 2 如何开发一个 Thing

具体查看文档**HowToMakeCfetThings.md**

### 2.1 ExampleThing

在 CFET2App 中包含了一个 ExampleThing，其中包含以下三个方面的示例：

1. ExampleThing写了不同资源类型的一些方法，引导大家如何编写一个 cfet 的 thing，以及如何使用cfet 事件。在注释中有详细说明

2. ExampleThing 动态挂载的配置已完成，具体路径为\jtext103_cfet2app\CFET2APP\bin\Debug\net461 下的thingConfig和thingDll中
3. ExampleThing 手动挂载的代码以及说明，可查看 Cfet2ProgramAddThingsPartial 中的 ExampleThing（一般不适用手动挂载，默认为动态挂载）

### 2.2 新建或引用一个 Thing的项目

1. 首先，每个thing作为一个单独的VS项目，**路径要在CFET2APP的解决方案目录之外**
2. 创建之后，在CFET2APP的解决方案中引用现有的该项目
3. 运行之前，在CFET2APP的运行路径下完成thing的动态挂载的配置

### 2.3 AddProject2Sln.bat

该脚本用于自动完成项目引用。使用时需要在CFETAPP解决方案所在路径下打开Windows命令行工具，输入脚本名和项目目录的完整路径，如：

```
AddProject2Sln.bat C:\repo\cfet2daqessential
```

脚本会递归搜寻指定目录中的所有.csproj文件，并将其添加引用到CFETAPP解决方案

### 2.4 保持CFET2APP的独立

注意，虽然 Thing 添加在 了CFET2APP 中，但是它不应该属于 CFET2APP 这个 repo。举个例子，你开发了一个采集数据的 Thing，这个 thing 挂载在 app 上运行，标准的流程应该是，采集 Thing 单独一个 repo，而 CFET2APP 是运行入口。为了防止把采集 Thing 也一起 push 到了CFET2APP 的 repo 中，注意做到采集thing的**项目路径要在CFET2APP的解决方案目录之外。

### 2.5 目标框架

生成解决方案时，CFET2APP目前的目标框架有 net461 和 net6，如果本地没有 net6 的环境可以在属性中选择不同的目标框架重新生成

## 3 本地/远程事件怎么订阅

### 3.1 什么是cfet事件？

cfet事件是cfet的资源类型之一，我们知道资源在cfet中用cr表示，可以使用http获取资源。但是如果你想订阅一个资源，当资源发生变化时可以感知，使用http就做不到了，cfet事件向用户提供了资源的发布订阅功能，分为订阅方和发布方，订阅方订阅cr，发布方在需要的时候推送data

`更详细的介绍可以查看cfet core的repo`

### 3.2 本地事件

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

### 3.3 远程事件

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


## 4 如何集成 WidgetUI

* WidgetUI编译之后，会有一个views的文件夹，把里面的App.html改名为index.html，然后把views文件夹放到CFET2APP的运行目录下，在浏览器中输入http://ip+port，注意这里的ip和port都是CFET2APP的，然后就可以访问前端了

* 关于WidgetUI还有Customview的使用，主要是用于界面的保存和恢复，如何使用的详细教程可以查看WidgetUI repo的说明文档