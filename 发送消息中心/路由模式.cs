using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 描述：
    ///    P----------------
    /// 
    /// 路由模式是订阅模式的一个变种模式，以路由进行匹配发送，
    /// 例如将消息1发送给A,B两个消息队列，或者将消息2发送给B,C两个消息队列，路由模式的交换机是direct
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 17:44:22
    /// </summary>
    public class 路由模式
    {
        public void Send()
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

                    exchangeName = "test-exchange-direct";
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
