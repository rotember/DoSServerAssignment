using System;

namespace DosServer
{
    public class ClientRequestsData
    {
        private object clientLock = new object();
        public int Id { get; set; }
        public int CurrentRequests { get; set; }
        public DateTime TimeFrameStart { get; set; }

        public ClientRequestsData(int id)
        {
            TimeFrameStart = DateTime.Now;
            Id = id;
        }

        public void ResetTimeFrame()
        {
            TimeFrameStart = DateTime.Now;
            CurrentRequests = 1;
        }

        public  bool CanGetReply(int timeFrameInSeconds, int maxRequests)
        {
            lock(clientLock)
            {
                int diffInSeconds = (DateTime.Now - TimeFrameStart).Seconds;
                //Console.WriteLine($"Client {Id} started");
                if (diffInSeconds <= timeFrameInSeconds)
                {
                    CurrentRequests++;
                    Console.WriteLine($"Client {Id} ended");
                    return CurrentRequests <= maxRequests;
                }

                ResetTimeFrame();
                //Console.WriteLine($"Client {Id} ended");
                return true;
            }
          
        }


    }
}