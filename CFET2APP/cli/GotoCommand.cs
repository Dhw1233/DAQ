using CommandLine;
using CommandLine.Text;
using Jtext103.CFET2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class GotoCommand : CommandBase
    {
        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> Path { get; set; }


        public override void Execute(CliParser parser)
        {
            Setup(parser);
            var path = Path.Single();
            path = GetAbsolutPahtAndQuery(path);
            var resource = MyHub.GetLocalResouce(path);

            if (resource != null)
            {
                MySession.CurrentResource = resource;
                MySession.CurrentPath = path;
                return;
            }
            throw new Exception("Source not found!");


        }

        
    }
}
