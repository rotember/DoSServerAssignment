using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DosServer
{
    public class DosProtectionRequestHandler
    {
        private static readonly double TimeFrameInSeconds = 5;
        private static readonly int MaxRequestsInTimeFrame = 5;
        private readonly ConcurrentDictionary<int, ClientRequestsData> ClientsRequestsData = new ConcurrentDictionary<int, ClientRequestsData>();

        public async Task<bool> TryHandleRequest(int clientId)
        {
            if (!ClientsRequestsData.ContainsKey(clientId))
            {
                ClientsRequestsData[clientId] = new ClientRequestsData(clientId);
            }

            ClientRequestsData client = ClientsRequestsData[clientId];

            return await Task.Run(() => IsRequestAccepted(client));
        }

        private bool IsRequestAccepted(ClientRequestsData client)
        {
            lock(client)
            {
                int diffInSeconds = (DateTime.Now - client.TimeFrameStart).Seconds;
                if (diffInSeconds <= TimeFrameInSeconds)
                {
                    client.CurrentRequests++;
                    return client.CurrentRequests <= MaxRequestsInTimeFrame;
                }

                client.ResetTimeFrame();
                return true;
            }
           
        }
    }
}
