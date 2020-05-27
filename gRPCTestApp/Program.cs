using System;

namespace gRPCTestApp.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleServer server = new SampleServer();
            server.Start();
            Console.WriteLine("Server listening...");
            Console.WriteLine("Press any key to shut down...");
            Console.ReadKey();
        }
    }
}
