using System;
using System.Threading;
using System.Linq;

namespace Server2023
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            int port = 2710;
            int ccu = 10;
            string urlPrefix = "";
            int counter = 0;

            foreach (var arg in args)
            {
                switch(arg)
                {
                    case "-port":
                        port = Int32.Parse(args[counter + 1]);
                    break;

                    case "-ccu":
                        port = Int32.Parse(args[counter + 1]);
                    break;

                    case "-urlprefix":
                        urlPrefix = args[counter + 1];
                    break;

                    default:
                    break;
                }
                counter++;
            }

            Console.Title = "Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(ccu, port, urlPrefix);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started.");
            DateTime _nextLoop = DateTime.Now;

            while(isRunning)
            {
                while(_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}