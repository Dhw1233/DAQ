using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class InvokeCommand:GetCommand
    {
        public override void Execute(CliParser parser)
        {
            Setup(parser);
            string resultString = AccessResource(MyHub.TryInvokeSampleResourceWithUri, MyHub.TryInvokeSampleResourceWithUri);
            Console.WriteLine(resultString);
        }
    }
}
