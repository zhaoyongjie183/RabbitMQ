namespace 发送消息中心
{
    class Program
    {
        static void Main(string[] args)
        {

            //公平分发 gong = new 公平分发();

            //gong.Send();

            //订阅模式 ding = new 订阅模式();

            //ding.Send();

            路由模式 lu = new 路由模式();

            lu.Send();
            
        }
    }
}
