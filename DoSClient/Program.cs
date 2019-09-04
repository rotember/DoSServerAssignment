using System;
using System.Threading.Tasks;

namespace DoSClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                ClientsSimulator clientsSimulator = InitSimulator(args);

                if (clientsSimulator == null)
                {
                    Console.WriteLine("Invalid arguments. Please run using arg0 - numOfClients, arg1(optional) - url");
                    return;
                }

                await Task.Run(() => clientsSimulator.Start());
                await Task.Run(() => WaitForUserCancelation());
                await clientsSimulator.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client simulator stopped. Error: {ex}");
            }
        }

        private static ClientsSimulator InitSimulator(string[] args)
        {
            if (!int.TryParse(args[0], out int clientsToSimulate))
            {
               
                return null;
            }

            if (args.Length > 1)
            {
                return new ClientsSimulator(clientsToSimulate, args[1]);
            }
            else
            {
                return new ClientsSimulator(clientsToSimulate);
            }
        }

        private static void WaitForUserCancelation()
        {
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
        }
    }
}
