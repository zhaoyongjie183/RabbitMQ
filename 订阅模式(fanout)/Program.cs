
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace 订阅模式_fanout_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "192.168.200.112",//IP地址
                UserName = "admin",//账号
                Password = "123456",//密码
                VirtualHost = "test-9825"//虚拟端口
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = String.Empty;
                    exchangeName = "test-exchangequeue";
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                    //队列名称
                    String queueName = String.Empty;
                    queueName = exchangeName+"queue";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //将队列与交换机进行绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                      //  Thread.Sleep(1000);//等待1秒,
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听 自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
