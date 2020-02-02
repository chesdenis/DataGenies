using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Roles
{
    [TestClass]
    public class ApplicationPublisherRoleTests
    {
        private MqBroker _inMemoryMqBroker;
        
        private SchemaDataContext _inMemorySchemaContext;

        private SchemaDataBuilder _schemaDataBuilder;

        private ManagedApplicationBuilder _managedApplicationBuilder;
        
        
        [TestInitialize]
        public void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            var receiverBuilder = new ReceiverBuilder(_inMemoryMqBroker);
            var publisherBuilder = new PublisherBuilder(_inMemoryMqBroker);
            
            _managedApplicationBuilder =
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder);
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
        }

        [TestMethod]
        public void PublisherRoleShouldPublishMessagesWithRoutingKey()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver");

            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiver");
            
            // Act
            var appInfo = this._inMemorySchemaContext.ApplicationInstances.First();
            var managedApp = _managedApplicationBuilder.UsingApplicationInstance(appInfo)
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();

            managedApp.Start();
            
            // Assert
            var bindingEntries = _inMemoryMqBroker.Model.Where(s => s.Item1 == "SampleAppPublisher").ToArray();
            Assert.AreEqual(1, bindingEntries.Count());

            var bindingEntry = bindingEntries.First();
            
            var exchangeName = bindingEntry.Item1;
            var queue = bindingEntry.Item2;
            Assert.AreEqual("SampleAppPublisher",exchangeName);
            Assert.AreEqual(1, queue.Count);
            queue.TryDequeue(out MqMessage message);
            Assert.AreEqual("SampleAppReceiver", message.RoutingKey);
            Assert.AreEqual("TestString",  Encoding.UTF8.GetString(message.Body));
        }

        [ApplicationTemplate]
        private class SampleAppPublisher : ApplicationPublisherRole
        {
            public SampleAppPublisher(DataPublisherRole publisherRole) : base(publisherRole)
            {
            }

            public override void Start()
            {
                var testData = Encoding.UTF8.GetBytes("TestString");

                this.Publish(testData);
            }

            public override void Stop()
            {
                
            }
        }
        
        [ApplicationTemplate]
        private class SampleAppReceiver : ApplicationReceiverRole
        {
            public SampleAppReceiver(DataReceiverRole receiverRole) : base(receiverRole)
            {
            }

            public override void Start()
            {
                
            }

            public override void Stop()
            {
                
            }
        }
    }
}