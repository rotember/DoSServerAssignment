using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DoSClient
{
    public class ClientsSimulator
    {
        private readonly int ClientsCount;
        public int MinSecondsBetweenRequests = 1;
        public int MaxSecondsBetweenRequests = 10;
        private readonly CancellationTokenSource Ct = new CancellationTokenSource();
        public List<Task> ClientTasks = new List<Task>();
        private readonly string Url;
       
       public ClientsSimulator(int clientsTosimulate, string url = "http://localhost:8080/")
        {
            ClientsCount = clientsTosimulate;
            Url = url;
        }

        public async Task Stop()
        {
            Ct.Cancel();
            await Task.WhenAll(ClientTasks.ToArray());
            Console.WriteLine("Stopping...");
        }

        public void Start()
        {
            for (int i = 0; i < ClientsCount; i++)
            {
                Task clientTask = Task.Run(SimulateClient);
                ClientTasks.Add(clientTask);
            }
        }

        private string GetRequestUrlForClient(int client)
        {
            return $"{Url}?clientId={client}";
        }
        private int GetRandomClientId()
        {
            Random random = new Random();
            int client = random.Next(1, ClientsCount + 1);

            return client;
        }

        private async Task SimulateClient()
        {
            HttpClient httpClient = new HttpClient();

            while (!Ct.IsCancellationRequested)
            {
                try
                {
                    await SendRequest(httpClient);
                    await Task.Delay(GetRandomTimeToWait());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while sending GET request: {ex}");
                    throw ex;
                }
            }
        }

        private async Task SendRequest(HttpClient httpClient)
        {
            int client = GetRandomClientId();
            string url = GetRequestUrlForClient(client);
            HttpResponseMessage response = await httpClient.GetAsync(url);
            WriteResponseToConsole(response, client);
        }

        private int GetRandomTimeToWait()
        {
            Random random = new Random();
            int client = random.Next(MinSecondsBetweenRequests, MaxSecondsBetweenRequests);
            return client * 1000;
        }

        private void WriteResponseToConsole(HttpResponseMessage response, int client)
        {
            Console.WriteLine($"{DateTime.Now}: client id: {client} got Response: {response.StatusCode}");
        }
    }
}