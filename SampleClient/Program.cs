using Grpc.Core;
using gRPCTestApp.Generated;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRPCTestApp.Consumer
{
    class Program
    {
        static NumberService.NumberServiceClient client;
        static async Task<int> NegateNumber(int number)
        {
            var data = new Number() { Content = number };
            var response = await client.NegateNumberAsync(data);

            return response.Content;
        }

        static async Task<int> MultiplyNumbers(int x, int y)
        {
            var data = new NumberPair() { X = x, Y = y };
            var response = await client.MultiplyNumbersAsync(data);

            return response.Content;
        }

        static async Task<List<int>> GetDivisors(int x)
        {
            var list = new List<int>();
            Number num = new Number() { Content = x };

            using (var call = client.GetDivisors(num))
            {
                var responseStream = call.ResponseStream;

                while (await responseStream.MoveNext())
                {
                    Number divisor = call.ResponseStream.Current;
                    Console.Write("{0} ", divisor.Content);
                    list.Add(divisor.Content);
                }
                Console.WriteLine();
            }
            return list;
        }
        static void Main(string[] args)
        {
            var channel = new Channel("10.218.0.6", 56567, ChannelCredentials.Insecure);
            client = new NumberService.NumberServiceClient(channel);

            Random r = new Random();
            int upperLimit = (int)Math.Floor(Math.Sqrt(int.MaxValue));
            int lowerLimit = (int)Math.Floor(Math.Sqrt(int.MinValue));
            int requestNum = r.Next();
            Console.WriteLine(NegateNumber(requestNum).GetAwaiter().GetResult());
            int x = r.Next(lowerLimit, upperLimit);
            int y = r.Next(lowerLimit, upperLimit);
            Console.WriteLine(MultiplyNumbers(x, y).GetAwaiter().GetResult());
            int z = (int) Math.Pow(24, 4);
            var divisors = GetDivisors(z).GetAwaiter().GetResult();

            channel.ShutdownAsync();
        }
    }
}
