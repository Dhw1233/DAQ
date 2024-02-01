using Jtext103.CFET2.CFET2App.cli;
using Jtext103.CFET2.Core;
using Jtext103.CFET2.Core.Log;
using System;

namespace Jtext103.CFET2.CFET2App
{
    partial class Cfet2Program:CFET2Host
    {
        static void Main(string[] args)
        {
            //first thing: init LogManager
            NlogProvider.Config();
            Cfet2LogManager.SetLogProvider(new NlogProvider());

            //inject and init the host app module
            var host = new Cfet2Program();
            HubMaster.InjectHubToModule(host);

            //add things, which defined in xxPartial.cs
            host.AddThings(); 

            //start communication modules
            host.MyHub.StartCommunication();

            //start all thins
            host.MyHub.StartThings();

            //start cli loop 
            var cli = new CliParser(host);
            cli.Host = host;
            Console.WriteLine("Cfet2 host Cli started");
            while (true)
            {
                Console.Write("Cfet2> ");
                var command=Console.ReadLine();
                try
                {
                    cli.Execute(command);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (cli.MySesstion.ShouldExit)
                {
                    //quit the app
                    break;
                }
            }

        }
    }
}
