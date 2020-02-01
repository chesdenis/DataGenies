using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Attributes;
using DataGenies.AspNetCore.DataGeniesCore.Converters;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;
using DataGenies.AspNetCore.DataGeniesCore.Roles;
using DataGenies.AspNetCore.InMemory;

namespace TestMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mqBroker = new MqBroker();
            mqBroker.ExchangesAndBoundQueues = new List<Tuple<string, Queue>>()
            {
                new Tuple<string, Queue>("sampleExchange", new Queue(){ Name = "sampleQueue"})
            };
              
            var simpleUrlGenerator = new HttpSimplePageDownloaderGenerator(
                new DataPublisherRole(
                    new PublisherBuilder(mqBroker).WithExchange("sampleExchange").Build(),
                    new List<IConverter>()));

            var simpleParser = new HtmlSimpleParser(
                new DataReceiverRole(
                    new ReceiverBuilder(mqBroker).WithQueue("sampleQueue").Build(),
                    new List<IConverter>()),
                new DataPublisherRole(
                    new PublisherBuilder(mqBroker).WithExchange("sampleExchange2").Build(),
                    new List<IConverter>()));

            var t1 = Task.Run(() =>
            {
                for (int i = 0; i < 1020; i++)
                {
                    simpleUrlGenerator.Start();
                }
            });
            var t2 = Task.Run(() => simpleParser.Start());
            var t3 = Task.Run(async () =>
            {
                await Task.Delay(new TimeSpan(0, 0, 30));
                simpleParser.StopListen();
            });

            var tasks = new Task[] {t1,t2, t3 };

            Task.WaitAll(tasks);
            
            Array.ForEach(tasks, t =>
            {
                if (t.IsFaulted) throw t.Exception;
            });
        }
    }

    [ApplicationTemplate]
    public class HtmlSimpleParser : ApplicationReceiverPublisherRole
    {
        public HtmlSimpleParser(DataReceiverRole receiverRole, DataPublisherRole publisherRole) 
            : base(receiverRole, publisherRole)
        {
        }

        public override void Start()
        {
            this.Listen(this.OnReceive);
        }

        private void OnReceive(byte[] data)
        {
            Console.WriteLine($"Data for parsing: { Encoding.UTF8.GetString(data) }");
            Console.WriteLine($"Parsing...");

            this.Publish(data);
        }
    }
    
    [ApplicationTemplate]
    public class HttpSimplePageDownloaderGenerator : ApplicationPublisherRole
    {
        public HttpSimplePageDownloaderGenerator(DataPublisherRole publisherRole) : base(publisherRole)
        {
        }
        
        public override void Start()
        {
            Console.WriteLine("Download something...");

            var someData = Encoding.UTF8.GetBytes(DateTime.Now.Second.ToString());

            this.Publish(someData);
        }
    }

}