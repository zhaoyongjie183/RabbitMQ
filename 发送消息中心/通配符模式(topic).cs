using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 描述：topic模式 将路由键和某个模式匹配
    /// 　#：匹配0-n个字符语句
    ///   *：匹配一个字符语句
    ///   　注意：RabbitMQ中通配符并不像正则中的单个字符，而是一个以“.”分割的字符串，
    ///   　如 ”topic1.*“匹配的规则以topic1开始并且"."后只有一段语句的路由  例：“topic1.aaa”，“topic1.bb”
    ///   　
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/18 14:45:29
    /// </summary>
    public class 通配符模式_topic_
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

                    exchangeName = "test-exchange-topic";
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息 定义路由routing-key
                        channel.BasicPublish(exchange: exchangeName, routingKey: "test.add", basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
