using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProject
{
    /// <summary>	
    /// 描述：                          -----------------消费者1
    ///        P---------------消息队列
    ///                                 -----------------消费者2
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 10:35:23
    /// </summary>
    public class 公平分发
    {
      public   void Send()
        {

            Console.WriteLine("Start");
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "192.168.200.112",//IP地址
                UserName = "admin",//账号
                Password = "123456",//密码
                VirtualHost = "test-9825"//虚拟端口
            };
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    String queueName = String.Empty;
                    queueName = "test-queue";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存(持久化)  如果队列已经存在 是不能修改的
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );

                    for (int i = 0; i < 50; i++)
                    {
                        String message = "[发送消息]:"+i;
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
