using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Roles;
using DataGenies.InMemory;

namespace TestMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    [ApplicationTemplate]
    public class HtmlSimpleParser : ApplicationReceiverAndPublisherRole
    {
        public HtmlSimpleParser(ApplicationReceiverRole receiverRole, ApplicationPublisherRole publisherRole) : base(
            receiverRole, publisherRole)
        {
        }

        public override void Start()
        {
            this.Listen(this.OnReceive);
        }

        public override void Stop()
        {
            this.StopListen();
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
        public HttpSimplePageDownloaderGenerator(IPublisher publisher, IEnumerable<IBehaviour> behaviours,
            IEnumerable<IConverter> converters) : base(publisher, behaviours, converters)
        {
        }

        public override void Start()
        {
            Console.WriteLine("Download something...");

            var someData = Encoding.UTF8.GetBytes(DateTime.Now.Second.ToString());

            this.Publish(someData);
        }

        public override void Stop()
        {
            
        }
    }

}