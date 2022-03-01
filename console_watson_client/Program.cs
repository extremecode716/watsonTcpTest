using System;
using System.Threading;

namespace console_watson_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000); // 서버 시작 후 실행

            WatsonTcpClientTest watsonTcpClientTest = new WatsonTcpClientTest();
            watsonTcpClientTest.CreateClient("127.0.0.1", 9000);
            watsonTcpClientTest.Connect();
            for (int i = 0; i < 100; ++i)
            {
                watsonTcpClientTest.Send("hi " + i);
            }
            Console.ReadKey();
        }
    }
}
