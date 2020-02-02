using System.Collections.Generic;
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
    public class ApplicationReceiverAndPublisherRoleTests
    {
        private MqBroker _inMemoryMqBroker;

        private MqConfigurator _mqConfigurator;
        
        private SchemaDataContext _inMemorySchemaContext;

        private SchemaDataBuilder _schemaDataBuilder;

        private ManagedApplicationBuilder _managedApplicationBuilder;
        
        
        [TestInitialize]
        public void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            var receiverBuilder = new ReceiverBuilder(_inMemoryMqBroker);
            var publisherBuilder = new PublisherBuilder(_inMemoryMqBroker);
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
            _mqConfigurator = new MqConfigurator(_inMemoryMqBroker);
            
            _managedApplicationBuilder =
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder, _mqConfigurator);
        }

        [TestMethod]
        public void ReceiverAndPublisherRoleShouldReceiveMessagesFromPublisherAndPushTheyToReceiver()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");

            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverAndPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppReceiverAndPublisher");

            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver");

            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher",
                "SampleAppReceiverAndPublisher", "#");

            _schemaDataBuilder.ConfigureBinding(
                "SampleAppReceiverAndPublisher",
                "SampleAppReceiver", "#");

            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();

            var receiverAndPublisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverAndPublisher"))
                .UsingTemplateType(typeof(SampleAppReceiverAndPublisher)).Build();

            var receiverManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiver"))
                .UsingTemplateType(typeof(SampleAppReceiver)).Build();

            // Act
            var t1 = Task.Run(() => publisherManagedApp.Start());

            var t2 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverAndPublisherManagedApp.Stop();
                });

                receiverAndPublisherManagedApp.Start();
            });

            var t3 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedApp.Stop();
                });

                receiverManagedApp.Start();
            });

            // Assert
            Task.WaitAll(t1, t2, t3);

            Assert.AreEqual("TestString", ((SampleAppPublisher) (publisherManagedApp.GetRootComponent())).State[0]);
            Assert.AreEqual("TestString-changed!", ((SampleAppReceiverAndPublisher) (receiverAndPublisherManagedApp.GetRootComponent())).State[0]);
            Assert.AreEqual("TestString-changed!", ((SampleAppReceiver) (receiverManagedApp.GetRootComponent())).State[0]);
        }

        [ApplicationTemplate]
        private class SampleAppReceiverAndPublisher : ApplicationReceiverAndPublisherRole
        {
            public readonly List<string> State = new List<string>();

            public SampleAppReceiverAndPublisher(DataReceiverRole receiverRole, DataPublisherRole publisherRole) : base(
                receiverRole, publisherRole)
            {
            }

            public override void Start()
            {
                this.Listen((message) =>
                {
                    var testData = Encoding.UTF8.GetString(message);
                    var changedTestData = $"{testData}-changed!";
                     
                    this.Publish( Encoding.UTF8.GetBytes(changedTestData));
                    State.Add(changedTestData);
                });
            }

            public override void Stop()
            {
                 this.StopListen();
            }
        }
        
        [ApplicationTemplate]
        private class SampleAppPublisher : ApplicationPublisherRole
        {
            public readonly List<string> State = new List<string>();
            
            public SampleAppPublisher(DataPublisherRole publisherRole) : base(publisherRole)
            {
            }

            public override void Start()
            {
                var testData = Encoding.UTF8.GetBytes($"TestString");
                this.Publish(testData);
                
                State.Add("TestString");
            }

            public override void Stop()
            {
                
            }
        }
         
        [ApplicationTemplate]
        private class SampleAppReceiver : ApplicationReceiverRole
        {
            public readonly List<string> State = new List<string>();
            
            public SampleAppReceiver(DataReceiverRole receiverRole) : base(receiverRole)
            {
            }

            public override void Start()
            {
                this.Listen((message) =>
                {
                   State.Add(Encoding.UTF8.GetString(message));
                });
            }

            public override void Stop()
            {
                this.StopListen();
            }
        }
    }
    
   

}