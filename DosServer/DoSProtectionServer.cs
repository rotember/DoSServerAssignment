using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DosServer
{
    public class DoSProtectionServer
    {
        private readonly string URL;
        private HttpListener Listener;
        private readonly DosProtectionRequestHandler RequestHandler = new DosProtectionRequestHandler();

        public DoSProtectionServer(string url = "http://localhost:8080/")
        {
            URL = url;
        }

        public async Task StartListening()
        {
            try
            {
                Listener = new HttpListener();
                Listener.Prefixes.Add(URL);
                Listener.Start();

                Console.WriteLine($"Started listening, url: {URL}");

                while (Listener.IsListening)
                {
                    HttpListenerContext context = await Listener.GetContextAsync();
                    Task.Run(() => ProcessRequest(context));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while listening: {ex}");
                throw ex;
            }
        }

        public void StopListening()
        {
            Console.WriteLine("Stopping...");
            Listener.Stop();
        }

        private async Task ProcessRequest(HttpListenerContext context)
        {    
            HttpListenerResponse response = context.Response;

            if (!int.TryParse(context.Request.QueryString["clientId"], out int clientId))
            {
                throw new Exception("Cannot process request. invalid clientId");
            }

            bool serviceWasAvilable = await RequestHandler.TryHandleRequest(clientId);
            SetResponseData(response, serviceWasAvilable);
            WriteResponseToConsole(response, clientId);
        }

        private void SetResponseData(HttpListenerResponse response, bool serviceWasAvilable)
        {
            if (serviceWasAvilable)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                response.StatusDescription = "Service Unavailable";
            }

            response.Close();
        }

        private void WriteResponseToConsole(HttpListenerResponse response, int client)
        {
            Console.WriteLine($"{DateTime.Now}: client id: {client} got Response: {response.StatusCode}, {response.StatusDescription}");
        }
    }


}
