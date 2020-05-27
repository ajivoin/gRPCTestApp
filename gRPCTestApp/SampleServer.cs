using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using gRPCTestApp.Generated;

namespace gRPCTestApp.Service
{
    public class SampleServer
    {
        private readonly Server server;

        public SampleServer()
        {
            server = new Server()
            {
                Services = { NumberService.BindService(new NumberServiceImpl()) },
                Ports = { new ServerPort("localhost", 56567, ServerCredentials.Insecure) }
            };
        }

        public void Start()
        {
            server.Start();
        }

        public async Task ShutDownAsync()
        {
            await server.ShutdownAsync();
        }
    }
}
