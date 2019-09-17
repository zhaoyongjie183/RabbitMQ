
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 描述：
    ///          P--------------------队列-----------------消费者
    /// MQ不允许 从新定义一个已经存在的队列 不同参数  
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 10:50:27
    /// </summary>
    public class 简单队列
    {
        public void send()
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
                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
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
