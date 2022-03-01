using System;

namespace console_watson_server
{
    class Program
    {
        static void Main(string[] args)
        {
            WatsonTcpServerTest watsonTcpServerTest = new WatsonTcpServerTest();
            watsonTcpServerTest.CreateServer("127.0.0.1", 9000);
            watsonTcpServerTest.Start();
            Console.ReadKey();
        }
    }
}
