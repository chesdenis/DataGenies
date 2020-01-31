using System;
using System.Collections.Generic;
using System.Text;
using DataGenies.AspNetCore.DataGeniesCore.Attributes;
using DataGenies.AspNetCore.DataGeniesCore.Components;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace TestMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    [ApplicationType]
    public class HtmlSimpleParser : ApplicationReceiverPublisherType
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
    
    [ApplicationType]
    public class HttpSimpleUrlGenerator : ApplicationPublisherType
    {
        public HttpSimpleUrlGenerator(BasicDataPublisher publisher) : base(publisher)
        {
        }
        
        public override void Start()
        {
            Console.WriteLine("Download something...");

            var someData = Encoding.UTF8.GetBytes("abcde");

            this.Publish(someData);
        }
    }

}