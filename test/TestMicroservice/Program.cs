using System;
using System.Collections.Generic;
using System.Text;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions.Receivers;
using DataGenies.AspNetCore.DataGeniesCore.Attributes;
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
    public class HtmlSimpleParser : IReceiver, IPublisher, IRunnable
    {
        private readonly BasicDataReceiver _receiver;
        private readonly BasicDataPublisher _publisher;

        public HtmlSimpleParser(BasicDataReceiver receiver, BasicDataPublisher publisher)
        {
            this._receiver = receiver;
            this._publisher = publisher;
        }

        public void Listen(Action<byte[]> onReceive) => _receiver.Listen(onReceive);
        
        public void Publish(byte[] data) => _publisher?.Publish(data);

        public void PublishRange(IEnumerable<byte[]> dataRange) => _publisher?.PublishRange(dataRange);

        public void Run()
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
    public class HttpSimpleUrlGenerator : IPublisher, IRunnable
    {
        private readonly BasicDataPublisher _publisher;

        public HttpSimpleUrlGenerator(BasicDataPublisher publisher) 
        {
            this._publisher = publisher;
        }

        public void Publish(byte[] data) => _publisher?.Publish(data);

        public void PublishRange(IEnumerable<byte[]> dataRange) => _publisher?.PublishRange(dataRange);

        public void Run()
        {
            Console.WriteLine("Download something...");

            var someData = Encoding.UTF8.GetBytes("abcde");

            this.Publish(someData);
        }
    }

}