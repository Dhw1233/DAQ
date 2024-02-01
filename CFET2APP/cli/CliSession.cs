using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Resource;

namespace Jtext103.CFET2.CFET2App.cli
{
    public class CliSession
    {
        /// <summary>
        /// the current resource i am on
        /// </summary>
        public ResourceBase CurrentResource { get; set; }

        /// <summary>
        /// the current resource path
        /// </summary>
        public string CurrentPath { get; set; }

        /// <summary>
        /// when this is true the app exit
        /// </summary>
        public bool ShouldExit { get; set; } = false;
    }
}
