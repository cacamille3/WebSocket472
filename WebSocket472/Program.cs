using System;
using System.Net.WebSockets;
using System.Threading;

namespace WebSocket472
{
  class Program
  {
    static void Main(string[] args)
    {
      //Set the IP and WebSocket port of your camera
      string CAMERA_IP_PORT = "192.168.3.193:888";

      System.Threading.Tasks.Task.Run(async () =>
      {
        using (ClientWebSocket clientws = new ClientWebSocket())
        {
          try
          {
            clientws.Options.AddSubProtocol("tracker-protocol");

            //By default Connection header is Upgrade,Keep-Alive but this is not 
            //working with VIVOTEK camera.
            //.Net 4.7.2 throw an exception when overwritting the Connection Header
            //clientws.Options.SetRequestHeader("Connection", "Upgrade");

            var timerCTS = new CancellationTokenSource(5000);
            await clientws.ConnectAsync(new Uri($"ws://{CAMERA_IP_PORT}/ws/vca"), 
                                        timerCTS.Token);
          }
          catch (Exception ex)
          {
            Console.WriteLine("Exception: {0}", ex);
          }
        }
      }).Wait();
    }
  }
}
