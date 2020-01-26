using System;
using System.Collections.Generic;
using System.Text;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace TestMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    
    public class HttpSimpleUrlGenerator : IPublisher, IRunnable
    {
        private readonly BasicDataPublisher _publisher;

        public HttpSimpleUrlGenerator(BasicDataPublisher publisher) 
        {
            this._publisher = publisher;
        }

        public void Publish(byte[] data) => _publisher.Publish(data);

        public void PublishRange(IEnumerable<byte[]> dataRange) => _publisher.PublishRange(dataRange);

        public void Run()
        {
            Console.WriteLine("Download something...");

            var someData = Encoding.UTF8.GetBytes("abcde");

            this.Publish(someData);
        }
    }

}