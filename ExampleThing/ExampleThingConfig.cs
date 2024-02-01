using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// 使用和thing一样的namespce
namespace Jtext103.CFET2.Things.ExampleThing
{
    public class ExampleThingConfig
    {
        // 这里随便举例一些需要配置的字段
        public string portName { get; set; } = "COM4";

        public string ServerIp { get; set; } = "192.168.0.11";

        public ExampleThingConfig(string filePath)
        {
            JsonConvert.PopulateObject(File.ReadAllText(filePath, Encoding.Default), this);
        }
    }
}
