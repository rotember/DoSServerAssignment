using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DosServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSProtectionServer server = InitializeServer(args); 
            Task listenerTask = Task.Run(() => server.StartListening());

            Console.ReadKey();
            server.StopListening();
        }

        private static DoSProtectionServer InitializeServer(string[] args)
        {
            if (args.Length > 0)
            {
                return new DoSProtectionServer(args[0]);
            }

            return new DoSProtectionServer();
        }
    }
}
