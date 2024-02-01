using CommandLine;
using Jtext103.CFET2.Core.Resource;
using Jtext103.CFET2.Core.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class ListCommand:CommandBase
    {
        [Option('t',"thing",HelpText ="only list things")]
        public bool IsThings { get; set; }


        [Option('d', "detail", HelpText = "show the resource type and etc.")]
        public bool IsDetail { get; set; }


        [Option('a', "all", HelpText = "show all resource on the host, regardless the current location")]
        public bool IsAll { get; set; }


        [Option('c', "child", HelpText = "only show the direct child  of the current thing.")]
        public bool IsChild { get; set; }

        public override void Execute(CliParser parser)
        {
            Setup(parser);
            //get all resource
            var resourceList = MyHub.GetAllLocalResources();
            var resultStrings = new List<string>();
            //check if --all is off then fielter
            if (!IsAll)
            {
                //if current resource is null throw
                if (MySession.CurrentResource == null)
                {
                    throw new Exception("currently not on a legal resource");
                }
                //check if --child, pick child
                if (IsChild)
                {
                    foreach (var resource in resourceList)
                    {
                        if (resource.Key.ToLower()!= MySession.CurrentPath.ToLower() && resource.Key.ToLower().StartsWith(MySession.CurrentPath.ToLower())) //is a child
                        {
                            if (resource.Key.Remove(0, MySession.CurrentPath.Length + 1).Contains(@"/") == false) //it's a direct child
                            {
                                resultStrings.Add(resource.Key);
                            }
                        }
                    }
                }
                else
                {
                    //just myself
                    resultStrings.Add(MySession.CurrentPath);
                }
            }
            else
            {
                resultStrings.AddRange(resourceList.Select(r => r.Key));
            }

            //check if -t is on, filter non things
            if (IsThings)
            {
                var toRemove = new List<string>();
                foreach (string result in resultStrings)
                {
                    if (MyHub.GetLocalResouce(result).ResourceType != ResourceTypes.Thing)
                    {
                        toRemove.Add(result);
                    }
                }
                foreach (var item in toRemove)
                {
                    resultStrings.Remove(item);
                }
            }

            //generate ou put, check -d is on, add detial to out put string
            if (IsDetail)
            {
                for (int i = 0; i < resultStrings.Count; i++)
                {
                    resultStrings[i] = resultStrings[i] + "\t"+
                        Enum.GetName(typeof(ResourceTypes), MyHub.GetLocalResouce(resultStrings[i]).ResourceType);
                }
            }
            //print
            for (int i = 0; i < resultStrings.Count; i++)
            {
                Console.WriteLine(resultStrings[i]);
            }
            if (resultStrings.Count == 0)
            {
                Console.WriteLine("No thing found!");
            }
            
        }
    }
}
