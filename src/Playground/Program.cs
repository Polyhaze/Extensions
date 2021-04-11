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
            
            var provider = new UltzLoggerProvider();
            var logger = provider.CreateLogger("Hi");
            for (var i = 0; i < 100; i++)
            {
                switch (i % 5)
                {
                    case 0:
                        logger.LogInformation($"Message {i}");
                        break;
                    case 1:
                        logger.LogWarning($"Message {i}");
                        break;
                    case 2:
                        logger.LogError($"Message {i}");
                        break;
                    case 3:
                        logger.LogTrace($"Message {i}");
                        break;
                    case 4:
                        logger.LogDebug($"Message {i}");
                        break;
                }
            }
            
            Console.WriteLine("Done1");
            provider.WaitAndShutdown();
            Console.WriteLine("Done2");
        }
    }
}