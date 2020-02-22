using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedService : IManagedService
    {
        private readonly IPublisher _publisher;
        private readonly IReceiver _receiver;

        public int ApplicationInstanceEntityId { get; set; }
        
        public List<CustomPublishParameter> CustomPublishParameters { get; set; }
        public List<CustomListenParameter> CustomListenParameters { get; set; }

        public ServiceState State { get; set; }
        
        public IContainer Container { get; }
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }

        protected ManagedService(
            IContainer container,
            IPublisher publisher,
            IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            Container = container;

            _receiver = receiver;
            _publisher = publisher;

            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;

            SetLinkToThisServiceInBehaviours();
        }

        private void SetLinkToThisServiceInBehaviours()
        {
            foreach (var behaviour in WrapperBehaviours)
            {
                behaviour.ManagedService = this;
            }

            foreach (var behaviour in BehaviourTemplates)
            {
                behaviour.ManagedService = this;
            }
        }

        public void Publish(MqMessage data)
        {
            this.ManagedActionWithMessage((x) => _publisher.Publish(x), data, BehaviourScope.Message);
        }

        public void Publish(string exchange, MqMessage data)
        {
            this.ManagedActionWithMessage((x) => _publisher.Publish(exchange, x), data, BehaviourScope.Message);
        }

        public void PublishRange(IEnumerable<MqMessage> dataRange)
        {
            this.ManagedActionWithContainer(
                (x) =>
                {
                    foreach (var dataEntry in dataRange)
                    {
                        this.Publish(dataEntry);
                    }
                },
                Container,
                BehaviourScope.Service);
        }

        public void PublishRange(string exchange, IEnumerable<MqMessage> dataRange)
        {
            this.ManagedActionWithContainer(
                (x) =>
                {
                    foreach (var dataEntry in dataRange)
                    {
                        this.Publish(exchange, dataEntry);
                    }
                },
                Container,
                BehaviourScope.Service);
        }

        public void Listen(Action<MqMessage> onReceive)
        {
            this.ManagedActionWithContainer(
                (x) =>
                {
                    _receiver.Listen(
                        arg =>
                            this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
                },
                Container,
                BehaviourScope.Service);
        }

        public void Listen(string queueName, Action<MqMessage> onReceive)
        {
            this.ManagedActionWithContainer(
                (x) =>
                {
                    _receiver.Listen(queueName, 
                        arg =>
                            this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
                },
                Container,
                BehaviourScope.Service);
        }

        public void StopListen()
        {
            this.ManagedActionWithContainer((x) => _receiver.StopListen(), Container, BehaviourScope.Service);
        }

        public void Start()
        {
            this.ManagedActionWithContainer((x) => OnStart(), Container, BehaviourScope.Service);
        }

        public void Stop()
        {
            this.ManagedActionWithContainer((x) => OnStop(), Container, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected T ReadSettings<T>()
        {
            var parametersDictAsJson =
                Container.Resolve<string>("ParametersDictAsJson");
            var configTemplateJson =
                Container.Resolve<string>("ConfigTemplateJson");

            var parametersDict = JsonSerializer.Deserialize<Dictionary<string, string>>(parametersDictAsJson);

            foreach (var parameter in parametersDict)
            {
                configTemplateJson = configTemplateJson.Replace($"[{parameter.Key}]", parameter.Value);
            }

            return JsonSerializer.Deserialize<T>(configTemplateJson);
        }
    }
}