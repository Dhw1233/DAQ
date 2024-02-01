using CommandLine;
using Jtext103.CFET2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class CliParser
    {
        public CFET2Host Host { get; set; }

        public CliSession MySesstion { get; set; } = new CliSession();

        public CliParser(CFET2Host host)
        {
            Host = host;
            MySesstion.CurrentPath=@"/";
            try
            {
                MySesstion.CurrentResource = host.MyHub.GetLocalResouce("/");
            }
            catch
            {
                //does nothing
            }

        }

        public void Execute(string command)
        {
            var args = splitArgs(command);
            var options = new Options();
            var optionParser = new CommandLine.Parser(with => { with.MutuallyExclusive = true;with.HelpWriter = Console.Out; });
            //if (!CommandLine.Parser.Default.ParseArguments(args, options,(verd,verbOptions)=>
            if (!optionParser.ParseArguments(args, options, (verd, verbOptions) =>
                {
                    var commandOption = verbOptions as ICliCommand;
                    if (commandOption != null)
                    {
                        commandOption.Execute(this);
                    }
                }))
            {
                //we have a error here
                Console.WriteLine("Wrong command, please read the help.");
            }

        }



        /// <summary>
        /// splite the string into args array, it will make string in quotation as a whole
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string[] splitArgs(string command)
        {

             var result = command.Split('\'')
             .Select((element, index) => index % 2 == 0  // If even index
                                   ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                   : new string[] { element })  // Keep the entire item
             .SelectMany(element => element).ToList();
                return result.ToArray();
        }


            
    }
}
