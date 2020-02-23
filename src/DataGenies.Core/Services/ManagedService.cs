using System;
using System.Collections.Generic;
using System.Text.Json;
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
        private readonly IPublisher publisher;
        private readonly IReceiver receiver;

        protected ManagedService(
            IContainer container,
            IPublisher publisher,
            IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours,
            BindingNetwork bindingNetwork)
        {
            this.Container = container;

            this.receiver = receiver;
            this.publisher = publisher;

            this.BehaviourTemplates = behaviourTemplates;
            this.WrapperBehaviours = wrapperBehaviours;
            this.BindingNetwork = bindingNetwork;

            this.SetLinkToThisServiceInBehaviours();
        }

        public int ApplicationInstanceEntityId { get; set; }

        public ServiceState State { get; set; }

        public IContainer Container { get; }

        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }

        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }

        public BindingNetwork BindingNetwork { get; }

        public void Listen(string queueName, Action<MqMessage> onReceive)
        {
            this.receiver.Listen(queueName, onReceive);
        }

        public void StopListen()
        {
            this.ManagedActionWithContainer(x => this.receiver.StopListen(), this.Container, BehaviourScope.Service);
        }

        public void Start()
        {
            this.ManagedActionWithContainer(x => this.OnStart(), this.Container, BehaviourScope.Service);
        }

        public void Stop()
        {
            this.ManagedActionWithContainer(x => this.OnStop(), this.Container, BehaviourScope.Service);
        }

        public void Publish(string exchange, MqMessage data)
        {
            this.publisher.Publish(exchange, data);
        }

        public void PublishRange(string exchange, IEnumerable<MqMessage> dataRange)
        {
            this.publisher.PublishRange(exchange, dataRange);
        }

        private void SetLinkToThisServiceInBehaviours()
        {
            foreach (var behaviour in this.WrapperBehaviours)
            {
                behaviour.ManagedService = this;
            }

            foreach (var behaviour in this.BehaviourTemplates)
            {
                behaviour.ManagedService = this;
            }
        }

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected T ReadSettings<T>()
        {
            var parametersDictAsJson = this.Container.Resolve<string>("ParametersDictAsJson");
            var configTemplateJson = this.Container.Resolve<string>("ConfigTemplateJson");

            var parametersDict = JsonSerializer.Deserialize<Dictionary<string, string>>(parametersDictAsJson);

            foreach (var parameter in parametersDict)
            {
                configTemplateJson = configTemplateJson.Replace($"[{parameter.Key}]", parameter.Value);
            }

            return JsonSerializer.Deserialize<T>(configTemplateJson);
        }
    }
}