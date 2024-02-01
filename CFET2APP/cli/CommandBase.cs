using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jtext103.CFET2.Core;

namespace Jtext103.CFET2.CFET2App.cli
{
    /// <summary>
    /// provide some command function that a command may need
    /// </summary>
    public class CommandBase : ICliCommand
    {
        protected Hub MyHub { get;  set; }
        protected CliSession MySession { get;  set; }

        public virtual void Execute(CliParser parser)
        {
            Setup(parser);
        }

        /// <summary>
        /// setup the local property like session and hub
        /// </summary>
        /// <param name="parser"></param>
        public virtual void Setup(CliParser parser)
        {
            MyHub = parser.Host.MyHub;
            MySession = parser.MySesstion;
        }

        /// <summary>
        /// convert a relative uri to a absolte path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetAbsolutPahtAndQuery(string path)
        {
            var currentPathAsDirectory = MySession.CurrentPath;
            if (!MySession.CurrentPath.EndsWith(@"/")) //add a trailing /
            {
                currentPathAsDirectory = MySession.CurrentPath + @"/";
            }
            var baseUri = new Uri(CommonConstants.LocalBaseUri, currentPathAsDirectory);
            var targetUri = new Uri(baseUri, path);
            //path = targetUri.AbsolutePath;
            //i want to keep the query string
            path = targetUri.PathAndQuery;
            if (path.EndsWith(@"/")) //remove the trailing /
            {
                path = path.Substring(0, path.Length - 1);
            }

            return path;
        }
    }
}
