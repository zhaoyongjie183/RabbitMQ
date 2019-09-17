using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQProject
{
    class Program
    {
        static void Main(string[] args)
        {

            //公平分发 gong = new 公平分发();

            //gong.Send();

            订阅模式 ding = new 订阅模式();

            ding.Send();
        }
    }
}
