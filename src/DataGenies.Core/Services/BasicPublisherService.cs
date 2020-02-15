using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;
using DataGenies.Core.Stores;

namespace DataGenies.Core.Services
{
    public class ManagedPublisherService : IPublisher, IRestartable
    {
        protected readonly IPublisher Publisher;
        private readonly IEnumerable<IBasicBehaviour> _basicBehaviours;
        private readonly IEnumerable<IBehaviourOnException> _behaviourOnExceptions;
        private readonly IEnumerable<IWrapperBehaviour> _wrapperBehaviours;

        public ManagedPublisherService(
            IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            Publisher = publisher;
            _basicBehaviours = basicBehaviours;
            _behaviourOnExceptions = behaviourOnExceptions;
            _wrapperBehaviours = wrapperBehaviours;
        }
        
        public virtual void Start()
        {
            
        }

        protected virtual T1 ManagedFunction<T0, T1>(Func<T0, T1> function, T0 arg)
        {
            var retVal = default(T1);
            
            try
            {
                foreach (var beforeStart in this._basicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Func<T0, T1> resultFunction = function;

                retVal = resultFunction(arg);

                foreach (var wrapper in this._wrapperBehaviours)
                {
                    retVal = wrapper.ChainFunction<T0, T1>(arg);
                }
            }
            catch (Exception ex)
            {
                foreach (var onException in this._behaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in this._basicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }

            return retVal;
        }
        
        protected virtual T ManagedFunction<T>(Func<T> function)
        {
            var retVal = default(T);
            
            try
            {
                foreach (var beforeStart in this._basicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }
 
                retVal = function();
            }
            catch (Exception ex)
            {
                foreach (var onException in this._behaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in this._basicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }

            return retVal;
        }

        protected virtual void ManagedAction<T>(Action<T> action, T arg)
        {
            try
            {
                foreach (var beforeStart in this._basicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Action<T> resultAction = action;

                foreach (var wrapper in this._wrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.WrapAction, resultAction);
                }

                resultAction(arg);
            }
            catch (Exception ex)
            {
                foreach (var onException in this._behaviourOnExceptions)
                {
                    onException.Execute(arg, ex);
                }
            }
            finally
            {
                foreach (var afterStart in this._basicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute(arg);
                }
            }
        }

        protected virtual void ManagedAction(Action execute)
        {
            try
            {
                foreach (var beforeStart in this._basicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }

                Action resultAction = execute;

                foreach (var wrapper in this._wrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.WrapAction, resultAction);
                }

                resultAction();
            }
            catch (Exception ex)
            {
                foreach (var onException in this._behaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in this._basicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }
        }

        public virtual void Stop()
        {
            throw new NotImplementedException();
        }

        public void Publish(byte[] data)
        {
            Publisher.Publish(data);
        }

        public void Publish(byte[] data, string routingKey)
        {
            Publisher.Publish(data, routingKey);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            Publisher.Publish(data, routingKeys);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            Publisher.PublishRange(dataRange);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            Publisher.PublishRange(dataRange, routingKey);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            Publisher.PublishTuples(tuples);
        }
    }

    // public class NumbersPublisherService : ManagedPublisherService
    // {
    //    
    //     public override void Start()
    //     {
    //         for (int i = 0; i < 10; i++)
    //         {
    //             this.Publisher.Publish(i.ToBytes());
    //         }
    //     }
    // }
}