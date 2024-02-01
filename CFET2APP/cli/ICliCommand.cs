using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jtext103.CFET2.CFET2App.cli
{
    public interface  ICliCommand
    {
        void Execute(CliParser parser);
    }
}
