using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 公平分发1
{
    /// <summary>	
    /// 描述：手动确认是MQ要接受到消费者的回执，才会从内存当中删除消息
    /// 手动模式的好处 ：如果消费者挂掉，那么MQ会分发给其他消费者，这样即使
    /// 消费者挂掉了，消息也不会丢失
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 11:13:17
    /// </summary>
    public class 手动确认
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
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //将autoAck设置false 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
