using CommandLine;
using Jtext103.CFET2.Core.Sample;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class GetCommand:CommandBase
    {
        [OptionArray('p', "params", HelpText = "the input parameters for this ",DefaultValue =new string[] { },MutuallyExclusiveSet ="params")]
        public string[] InputParams { get; set; }


        [Option('d', "dict", HelpText = "the input dictionary for the resource, input must in json form", MutuallyExclusiveSet = "params")]
        public string InputDict { get; set; }


        [Option('s', "sample", HelpText = "show the whole sample instead of just value")]
        public bool IsSample { get; set; }


        [Option('j', "json", HelpText = "indicate the input parameters are json string")]
        public bool IsJson { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> Path { get; set; }

        public override void Execute(CliParser parser)
        {
            Setup(parser);
            string resultString = AccessResource(MyHub.TryGetResourceSampleWithUri, MyHub.TryGetResourceSampleWithUri);
            Console.WriteLine(resultString);

        }

        public string AccessResource(Func<string,object[],ISample> arrayAccessFunc, Func<string, Dictionary<string,object>, ISample> dictAccessFunc)
        {
            
            var path = MySession.CurrentPath;
            if (Path.Count==1)
            {
                path = Path.Single();
            }
                
            if (!(new Uri(path, UriKind.RelativeOrAbsolute)).IsAbsoluteUri) //if not a absolute uri than it's a local path try to make a full path, it can be a relative path
            {
                path = GetAbsolutPahtAndQuery(path);
            }
            var realInputs = new object[] { };
            Dictionary<string, object> realInputDict = null;
            if (InputParams.Count() != 0)
            {
                realInputs = makeInputs(InputParams, IsJson);
            }
            if (!(string.IsNullOrEmpty(InputDict))) //we have dictinary input
            {
                realInputDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(InputDict);
            }
            ISample result = null;
            if (realInputDict != null)
            {
                //result = MyHub.TryGetResourceSampleWithUri(path, realInputDict);
                result = dictAccessFunc(path, realInputDict);
            }
            else
            {
                //result = MyHub.TryGetResourceSampleWithUri(path, realInputs);
                result = arrayAccessFunc(path, realInputs);
            }

            var resultString = result.ToString();
            if (IsSample)
            {
                resultString = JsonConvert.SerializeObject(result, Formatting.Indented);
            }

            return resultString;
        }

        private object[] makeInputs(string[] inputParams, bool isJson)
        {
            if (!IsJson)
            {
                return inputParams.Cast<object>().ToArray();
            }
            var newInputs = new List<object>();
            for (int i = 0; i < inputParams.Length; i++) 
            {
                newInputs.Add(JsonConvert.DeserializeObject(inputParams[i]));
            }
            return newInputs.ToArray();
        }
    }
}
