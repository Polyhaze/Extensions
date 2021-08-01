using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Ultz.Extensions.Logging;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            // Demonstrating the multi-threadedness of our logger, and how it doesn't hold up execution for logging.
            for (var i = 0; i < 100; i++)
            {
                switch (i % 5)
                {
                    case 0:
                        Log.Information($"Message {i}");
                        break;
                    case 1:
                        Log.Warning($"Message {i}");
                        break;
                    case 2:
                        Log.Error($"Message {i}");
                        break;
                    case 3:
                        Log.Trace($"Message {i}");
                        break;
                    case 4:
                        Log.Debug($"Message {i}");
                        break;
                }
            }
            
            Console.WriteLine("Done1");
            //Log.Shutdown();
            Console.WriteLine("Done2");
        }
    }
}