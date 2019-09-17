using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using 公平分发1;

namespace 公平分发1
{
    class Receive1
    {
        static void Main(string[] args)
        {
            能者多劳 neng = new 能者多劳();

            neng.recv();
        }
    }
}
