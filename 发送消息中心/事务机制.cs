using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 描述：在rabbitmq当中生产者是不知道消息是否已经送达
    /// 在rabbitmq当中有两种或机制 一种是AMPQ  一种是confirm
    /// AMPQ采取事务
    /// txSelect 开启事务
    /// txCommit 提交事务
    /// txRollback 回滚事务
    /// 缺点：降低MQ的吞吐量  而且很耗时
    /// 原因是因为事务的性能是非常差的。事务性能测试：事务模式，结果如下：
    ///事务模式，发送1w条数据，执行花费时间：14197s
    ///事务模式，发送1w条数据，执行花费时间：13597s
    ///事务模式，发送1w条数据，执行花费时间：14216s
    ///
    ///非事务模式，结果如下：
    ///
    ///非事务模式，发送1w条数据，执行花费时间：101s
    ///非事务模式，发送1w条数据，执行花费时间：77s
    ///非事务模式，发送1w条数据，执行花费时间：106s
    ///
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/18 15:10:23
    /// </summary>
    public class 事务机制
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
                    queueName = "test-queue-tx";
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
                        try
                        {
                            Console.WriteLine("请输入消息内容:");
                            String message = Console.ReadLine();
                            //消息内容
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            //开启事务
                            channel.TxSelect();
                            //发送消息
                            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                            Console.WriteLine("成功发送消息:" + message);
                            //提交事务
                            channel.TxCommit();
                        }
                        catch (Exception)
                        {

                            channel.TxRollback();
                            Console.WriteLine("回滚消息");
                        }

                    }
                }
            }
        }
    }
}
