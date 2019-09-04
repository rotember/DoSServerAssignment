using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoSClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string clientIdInput;
            string url = "http://localhost:8080/";
            HttpClient client = new HttpClient();
            Console.WriteLine("Press q to exit");
            clientIdInput = Console.ReadLine();
           
           
           if (args.Length > 0)
            {
                url = args[0];
            }
            while (clientIdInput != "q")
            {
                try
                {
                    if (!int.TryParse(clientIdInput, out int clientId))
                    {
                        Console.WriteLine("invalid client id");
                        clientIdInput = Console.ReadLine();
                        break;
                    }

                    HttpResponseMessage response = await client.GetAsync($"{url}?clientId={clientId}");
                    Console.WriteLine(response.StatusCode);
                    clientIdInput = Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }

            Console.WriteLine("Stopping...");
        }
    }
}
