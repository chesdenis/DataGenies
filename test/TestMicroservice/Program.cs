using System;
using System.Collections.Generic;
using System.Text;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction.Publisher;
using DataGenies.AspNetCore.DataGeniesCore.Attribute;
using DataGenies.AspNetCore.DataGeniesCore.Publisher;

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