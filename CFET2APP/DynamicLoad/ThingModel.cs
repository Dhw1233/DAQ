using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jtext103.CFET2.CFET2App.DynamicLoad
{
    public class ThingModel
    {
        /// <summary>
        /// Thing在CFET下的挂载路径
        /// </summary>
        public string MountPath { get; set; }

        /// <summary>
        /// Thing的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 存在Config.json中的内容
        /// </summary>
        public ConfigModel Config { get; set; }

        public ThingModel()
        {
            Config = new ConfigModel();
        }

        /// <summary>
        /// 读取Thing的文件夹下的Config.json文件内容
        /// </summary>
        /// <param name="configFilePath">Config.json文件的完整路径</param>
        /// <returns>成功返回true失败返回false</returns>
        public bool LoadConfig(string configFilePath)
        {
            bool result = true;

            try
            {
                JsonConvert.PopulateObject(File.ReadAllText(configFilePath, Encoding.Default), Config);

                int startIndex = configFilePath.IndexOf("thingConfig")+ "thingConfig".Length;
                int endIndex = configFilePath.IndexOf("Config.json");

                string fullThingPath = configFilePath.Substring(startIndex, endIndex - startIndex - 1).Replace(Path.DirectorySeparatorChar, '/');
                int breakIndex = fullThingPath.LastIndexOf('/');
                MountPath = fullThingPath.Substring(0, breakIndex + 1);
                Name = fullThingPath.Substring(breakIndex + 1, fullThingPath.Length - breakIndex - 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }
    }

    public class ConfigModel
    {
        /// <summary>
        /// Thing的类型，需要有完整的命名空间+类名
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Thing挂载时传入的参数
        /// </summary>
        public object InitObj { get; set; }
    }
}
