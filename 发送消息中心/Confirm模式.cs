using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace 发送消息中心
{
    /// <summary>	
    /// 描述：confirm模式
    /// 原理：
    /// 生产者将信道设置成confirm模式，一旦信道进入confirm模式，所有在该信道上面发布的消息都将会被指派一个唯一的ID(从1开始)，
    /// 一旦消息被投递到所有匹配的队列之后，broker就会发送一个确认给生产者(包含消息的唯一ID)，这就使得生产者知道消息已经正确到达目的队列了，
    /// 如果消息和队列是可持久化的，那么确认消息会在将消息写入磁盘之后发出，broker回传给生产者的确认消息中delivery-tag域包含了确认消息的序列号，
    /// 此外broker也可以设置basic.ack的multiple域，表示到这个序列号之前的所有消息都已经得到了处理；
    /// Confirm的三种实现方式：
    /// 方式一：普通confirm模式，每发送一条消息，调用waitForConfirms()方法等待服务端confirm，这实际上是一种串行的confirm，
    /// 每publish一条消息之后就等待服务端confirm，如果服务端返回false或者超时时间内未返回，客户端进行消息重传；
    /// 方式二：批量confirm模式，每发送一批消息之后，调用waitForConfirms()方法，等待服务端confirm，这种批量确认的模式极大的提高了confirm效率，
    /// 但是如果一旦出现confirm返回false或者超时的情况，
    /// 客户端需要将这一批次的消息全部重发，这会带来明显的重复消息，如果这种情况频繁发生的话，效率也会不升反降；
    /// 方式三：channel.addConfirmListener()异步监听发送方确认模式；
    /// 创建人： zhaoyongjie
    /// 创建时间：2019/9/18 15:23:37
    /// </summary>
    public class Confirm模式
    {
        public void jiandanSend()
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
                    queueName = "test-queue-confirm";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存(持久化)  如果队列已经存在 是不能修改的
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //将chnel设置成confirm 模式  ，事务模式和confirm模式不能进行修改，一旦
                    //队列进行了创建，就不能修改模式
                    channel.ConfirmSelect();
                    
                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                        if (!channel.WaitForConfirms())
                        {
                            Console.WriteLine("发送失败");
                        }
                        else
                        {
                            Console.WriteLine("成功发送消息:" + message);
                        }
                    }
                }
            }
        }

        public void piliangSend()
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
                    queueName = "test-queue-pl-confirm";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存(持久化)  如果队列已经存在 是不能修改的
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //将chnel设置成confirm 模式  ，事务模式和confirm模式不能进行修改，一旦
                    //队列进行了创建，就不能修改模式
                    channel.ConfirmSelect();

                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        for (int i = 0; i < 20; i++)
                        {
                            //发送消息
                            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        }

                        if (!channel.WaitForConfirms())
                        {
                            Console.WriteLine("发送失败");
                        }
                        else
                        {
                            Console.WriteLine("成功发送消息:" + message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 目前没有找到.net core 中MQ confirm当中的异步模式
        /// </summary>
        public void yibuSend()
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
                    queueName = "test-queue-pl-confirm";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存(持久化)  如果队列已经存在 是不能修改的
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //将chnel设置成confirm 模式  ，事务模式和confirm模式不能进行修改，一旦
                    //队列进行了创建，就不能修改模式
                    channel.ConfirmSelect();

                    while (true)
                    {
                        Console.WriteLine("请输入消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        for (int i = 0; i < 10; i++)
                        {
                            //发送消息
                            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        }
                        //该方法会等到最后一条消息得到确认或者得到nack才会结束，也就是说在waitForConfirmsOrDie处会造成当前程序的阻塞
                        channel.WaitForConfirmsOrDie();
                        
                    }
                }
            }
        }
    }
}
