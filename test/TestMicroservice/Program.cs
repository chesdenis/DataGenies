using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.ApplicationTemplates;
using DataGenies.AspNetCore.DataGeniesCore.Attributes;
using DataGenies.AspNetCore.DataGeniesCore.Converters;
using DataGenies.AspNetCore.DataGeniesCore.Models.InMemory;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace TestMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mqBroker = new InMemoryMqBroker();
            mqBroker.ExchangesAndBoundQueues = new List<Tuple<string, InMemoryQueue>>()
            {
                new Tuple<string, InMemoryQueue>("sampleExchange", new InMemoryQueue(){ Name = "sampleQueue"})
            };
              
            var simpleUrlGenerator = new HttpSimplePageDownloaderGenerator(
                new BasicDataPublisher(
                    new InMemoryPublisherBuilder(mqBroker).WithExchange("sampleExchange").Build(),
                    new List<IConverter>()));

            var simpleParser = new HtmlSimpleParser(
                new BasicDataReceiver(
                    new InMemoryReceiverBuilder(mqBroker).WithQueue("sampleQueue").Build(),
                    new List<IConverter>()),
                new BasicDataPublisher(
                    new InMemoryPublisherBuilder(mqBroker).WithExchange("sampleExchange2").Build(),
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
    public class HtmlSimpleParser : ApplicationReceiverTemplate
    {
        public HtmlSimpleParser(BasicDataReceiver receiver, BasicDataPublisher publisher) 
            : base(receiver, publisher)
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
    public class HttpSimplePageDownloaderGenerator : ApplicationPublisherTemplate
    {
        public HttpSimplePageDownloaderGenerator(BasicDataPublisher publisher) : base(publisher)
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