using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class Options
    {

        [VerbOption("list", HelpText = "list resources, more help: list --help")]
        public ListCommand ListCommandOption { get; set; }

        [VerbOption("goto", HelpText = @"goto a local resource that specified by a path, like goto /thing/status")]
        public GotoCommand GotoCommandOption { get; set; }

        [VerbOption("get", HelpText = @"get a resource sample that specified by a path, more help: get --help")]
        public GetCommand GetCommandOption { get; set; }

        [VerbOption("set", HelpText = @"set a resource sample that specified by a path, more help: set --help")]
        public SetCommand SetCommandOption { get; set; }

        [VerbOption("invoke", HelpText = @"invoke a resource sample that specified by a path, more help: invoke --help")]
        public InvokeCommand InvokeCommandOption { get; set; }

        [VerbOption("exit", HelpText = @"Just dispose all things and exit the app")]
        public ExitCommand ExitCommandOption { get; set; }


        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
