using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 公平分发1
{
    /// <summary>	
    /// 描述：自动确认是MQ默认方式,一旦MQ把消息发送给消费者，就会立即从内存当中删除
    /// 坏处：如果这个时候杀死了消费者进程，就会导致数据丢失
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 11:13:34
    /// </summary>
    public class 自动确认
    {
        public void Recv()
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
                    String queueName = String.Empty;
                    queueName = "test-queue";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Thread.Sleep(1000);//等待1秒,
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                    };
                    //消费者开启监听 自动确认
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
