using System;
using Grpc.Core;
using gRPCTestApp.Generated;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCTestApp.Service
{
    class NumberServiceImpl : NumberService.NumberServiceBase
    {
        public override Task<Number> NegateNumber(Number request, ServerCallContext context)
        {
            var response = new Number() { Content = -request.Content };

            return Task.FromResult(response);
        }

        public override Task<Number> MultiplyNumbers(NumberPair request, ServerCallContext context)
        {
            var response = new Number() { Content = request.X * request.Y };
            return Task.FromResult(response);
        }

        public override Task<BoolResponse> IsPrime(Number request, ServerCallContext context)
        {
            int number = Math.Abs(request.Content);
            for (int i = 0; i < number; i++)
            {
                if (number % i == 0)
                {
                    return Task.FromResult(new BoolResponse() { Result = false });
                }
            }
            return Task.FromResult(new BoolResponse() { Result = true });
        }

        public override async Task GetDivisors(Number request, IServerStreamWriter<Number> responseStream, ServerCallContext context)
        {
            int num = request.Content;
            for (int i = 1; i < Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                {
                    await responseStream.WriteAsync(new Number() { Content = i });
                    await responseStream.WriteAsync(new Number() { Content = num / i });
                }
            }
        }
    }
}
