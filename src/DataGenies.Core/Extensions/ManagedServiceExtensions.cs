using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Services;

namespace DataGenies.Core.Extensions
{
    public static class ManagedServiceExtensions
    {
        public static void ManagedActionWithMessage<T>(this IManagedService managedService, Action<T> execute, T message, BehaviourScope behaviourScope) where T: MqMessage
        {
            try
            {
                foreach (var beforeStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.BeforeRun))
                {
                    beforeStart.Execute(message);
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours
                    .Where(w=>w.BehaviourScope == behaviourScope))
                {
                    resultAction = wrapper.WrapMessageHandling(wrapper.BehaviourActionWithMessage, resultAction, message);
                }

                resultAction(message);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.OnException))
                {
                    onException.Execute(message, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.AfterRun))
                {
                    afterStart.Execute(message);
                }
            }
        }
        
        public static void ManagedActionWithContainer<T>(this IManagedService managedService, Action<T> execute, T container, BehaviourScope behaviourScope) where T: IContainer
        {
            try
            {
                foreach (var beforeStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.BeforeRun))
                {
                    beforeStart.Execute(container);
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours
                    .Where(w=>w.BehaviourScope == behaviourScope))
                {
                    resultAction = wrapper.WrapContainerHandling(wrapper.BehaviourActionWithContainer, resultAction, container);
                }

                resultAction(container);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.OnException))
                {
                    onException.Execute(container, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.AfterRun))
                {
                    afterStart.Execute(container);
                }
            }
        }


        public static ConnectedReceivers ConnectedReceivers(this IManagedService managedService)
        {
            return managedService.BindingNetwork.Receivers;
        }
        
        public static ConnectedReceivers ConnectedReceivers(
            this IManagedService managedService,
            Func<BindingReference, bool> whereArg)
        {
            var filtered = new ConnectedReceivers();

            filtered.AddRange(managedService.BindingNetwork.Receivers.Where(whereArg).ToList());
            
            return filtered;
        }
        
        public static ConnectedReceivers Except(
            this ConnectedReceivers receivers,
            ConnectedReceivers except)
        {
            var filtered = new ConnectedReceivers();

            filtered.AddRange(((IEnumerable<BindingReference>)receivers).Except(except).ToList());
            
            return filtered;
        }
        
        public static ConnectedPublishers Except(
            this ConnectedPublishers publishers,
            ConnectedPublishers except)
        {
            var filtered = new ConnectedPublishers();

            filtered.AddRange(((IEnumerable<BindingReference>)publishers).Except(except).ToList());
            
            return filtered;
        }

        public static ConnectedReceivers ConnectedReceivers(
            this IManagedService managedService,
            string name)
        {
            var filtered = new ConnectedReceivers();
            
            filtered.AddRange(managedService.BindingNetwork.Receivers.Where(w=> w.ReceiverInstanceName == name).ToList());

            return filtered;
        }

        public static ConnectedPublishers ConnectedPublishers(this IManagedService managedService)
        {
            return managedService.BindingNetwork.Publishers;
        }

        public static ConnectedPublishers ConnectedPublishers(
            this IManagedService managedService,
            Func<BindingReference, bool> whereArg)
        {
            var filtered = new ConnectedPublishers();

            var bindingReferences = managedService.BindingNetwork.Publishers;

            filtered.AddRange(bindingReferences.Where(whereArg));
            
            return filtered;
        }

        public static void ManagedPublishUsing(
            this ConnectedReceivers connectedReceivers,
            IManagedService managedService,
            MqMessage message)
        {
            managedService.ManagedActionWithMessage((x) =>
            {
                foreach (var bindingReference in connectedReceivers)
                {
                    managedService.Publish(bindingReference.ExchangeName, x);
                }
            }, message, BehaviourScope.Message);
        }

        public static void ManagedPublishRange(
            this ConnectedReceivers connectedReceivers,
            IManagedService managedService,
            IEnumerable<MqMessage> dataRange)
        {
            managedService.ManagedActionWithContainer(
                (x) =>
                {
                    foreach (var dataEntry in dataRange)
                    {
                        connectedReceivers.ManagedPublishUsing(managedService, dataEntry);
                    }
                },
                managedService.Container,
                BehaviourScope.Service);
        }
        
        public static void ManagedListen(
            this ConnectedPublishers connectedPublishers,
            IManagedService managedService,
            Action<MqMessage> onReceive)
        {
            managedService.ManagedActionWithContainer(
                (x) =>
                {
                    foreach (var bindingReference in connectedPublishers)
                    {
                        managedService.Listen(bindingReference.QueueName,
                            arg =>
                                managedService.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
                    }
                },
                managedService.Container,
                BehaviourScope.Service);
        }
    }

    


}