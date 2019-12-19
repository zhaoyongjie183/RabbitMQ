using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都会被转发到与该交换机绑定的所有队列上
    /// 任何发送到Fanout Exchange的消息都会被转发到与该Exchange绑定(Binding)的所有Queue上。
    ///1.可以理解为路由表的模式
    ///2.这种模式不需要RouteKey
    ///3.这种模式需要提前将Exchange与Queue进行绑定，一个Exchange可以绑定多个Queue，一个Queue可以同多个Exchange进行绑定。
    ///4.如果接受到消息的Exchange没有与任何Queue绑定，则消息会被抛弃。
    /// 描述：              ------------队列1------------消费者1                
    ///         P----------X
    ///                     -------------队列2-----------消费者2
    /// 
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/17 11:49:26
    /// </summary>
    public class 订阅模式
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

                    exchangeName = "test-exchange-fanout";

                    IDictionary<String, Object> args = new Dictionary<String, Object>();

                    //备份交换器是为了实现没有路由到队列的消息，防止消息丢失
                    //备份服务器交换机名称
                    String aeExchangeName = "test-exchange-fanout-ae";

                    //通过alternate-exchange指定备份交换器。备份交换器建议设置成fanout类型，也可以设置成direct或topic的类型
                    //设置成功以后在rabbitmq控制网站上Features字段后面是AE标志
                    args.Add("alternate-exchange", aeExchangeName);

                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout",true,false, args);
                    ////主队列 不绑定队列，这个时候发送消息，消息会丢失，所以声明一个备份交换机，这个时候会把消息发送到备份交换机里面
                    //channel.QueueDeclare("test-exchange-fanout-queue", true, false, false, null);
                    //channel.QueueBind("test-exchange-fanout-queue", "test-exchange-fanout", "", null);
                    //备份队列
                    channel.ExchangeDeclare(exchange: aeExchangeName, type: "fanout", true, false, null);
                    channel.QueueDeclare("test-exchange-fanout-ae-queue", true, false, false, null);
                    channel.QueueBind("test-exchange-fanout-ae-queue", aeExchangeName, "",null);//myAe的类型是fanout，没有路由的


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

