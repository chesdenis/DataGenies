using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;

namespace TestMicroservice
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _containerBag =
            new Dictionary<Type, Dictionary<string, object>>();

        public void Register<T>(object instance) where T : class
        {
            if (_containerBag.ContainsKey(typeof(T)))
            {
                throw new ArgumentException(
                    "Can't add type because it exists already. Please define name for another instance",
                    paramName: "T");
            }
            
            this._containerBag.Add(typeof(T), new Dictionary<string, object>()
            {
                { string.Empty, instance}
            });
        }

        public void Register<T>(object instance, string name) where T : class
        {
            if (_containerBag.ContainsKey(typeof(T)) && _containerBag[typeof(T)].ContainsKey(name))
            {
                throw new ArgumentException("Can't add type because it exists already with this name",
                    paramName: nameof(name));
            }

            if (!_containerBag.ContainsKey(typeof(T)))
            {
                this._containerBag.Add(typeof(T), new Dictionary<string, object>());
            }
            
            this._containerBag[typeof(T)][name] = instance;
        }

        public T Resolve<T>()
        {
            return (T)this._containerBag[typeof(T)][string.Empty];
        }

        public T Resolve<T>(string name)
        {
            return (T)this._containerBag[typeof(T)][name];
        }
    }
    
    public interface IContainer
    {
        void Register<T>(object instance)
            where T : class;

        void Register<T>(object instance, string name)
            where T : class;

        T Resolve<T>();
        
        T Resolve<T>(string name);
    }
    
    public static class DataExtensions
    {
        public static byte[] ToBytes<T>(this T message)
        {
            var jsonData = JsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(jsonData);
        }

        public static T FromBytes<T>(this byte[] messageAsBytes)
        {
            var jsonData = Encoding.UTF8.GetString(messageAsBytes);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
    
    public class BasicBehaviour : IBasicBehaviour
    {
        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(IContainer arg)
        {
            throw new NotImplementedException();
        }
    }

    public class BehaviourOnException : IBehaviourOnException
    {
        public virtual void Execute(Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(IContainer arg, Exception exception)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBasicBehaviour
    {
        void Execute();

        void Execute(IContainer arg);
    }

    public interface IBehaviourOnException
    {
        void Execute(Exception exception);

        void Execute(IContainer arg, Exception exception);
    }
    
    public delegate void WrappedActionWithContainer(Action<IContainer> action, IContainer arg);

    public delegate Action WrappedAction(Action action);

    public interface IWrapperBehaviour
    {
       Action Wrap(Action<Action> wrapperAction, Action executeAction);
       
       void BehaviourAction(Action action);

       Action<IContainer> Wrap(WrappedActionWithContainer wrapperAction, Action<IContainer> executeAction);

       void BehaviourAction(Action<IContainer> action, IContainer container);
    }
    //
    // public interface IConverterBehaviour<in T0, out T1>
    // {
    //     T1 ChainFunction(T0 arg);
    //     T1 ChainFunction();
    // }
    //
    // public abstract class ConverterBehaviour<T0, T1> : IConverterBehaviour<T0, T1>
    // {
    //     public virtual T1 ChainFunction(T0 arg)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public virtual T1 ChainFunction()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }

    public abstract class WrapperBehaviour  : IWrapperBehaviour 
    {
        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }

        public virtual void BehaviourAction(Action action)
        {
            throw new NotImplementedException();
        }

        public Action<IContainer> Wrap(WrappedActionWithContainer wrapperAction, Action<IContainer> executeAction)
        {
            return (arg) => wrapperAction(executeAction, arg);
        }

        public virtual void BehaviourAction(Action<IContainer> action, IContainer container)
        {
            throw new NotImplementedException();
        }
    }
    
    public interface IBehaviorBeforeStart : IBasicBehaviour
    {
        
    }

    public class BehaviorBeforeStart : BasicBehaviour
    {
        
    }

    public interface IBehaviourAfterStart : IBasicBehaviour
    {
        
    }

    public class BehaviourAfterStart : BasicBehaviour
    {
        
    }
    
    public interface IManagedService
    {
        IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
    }
 
    // public interface IManagedConversionService<in T0, out T1> : IManagedService
    // {
    //     IEnumerable<IConverterBehaviour<T0, T1>> ConverterBehaviours { get; }
    // }

    public static class ManagedServiceExtensions
    {
        // public static T1 ManagedFunction<T0, T1>(this IManagedConversionService<T0, T1> managedService, Func<T0, T1> function, T0 arg)
        // {
        //     var retVal = default(T1);
        //     
        //     try
        //     {
        //         foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
        //         {
        //             beforeStart.Execute(arg);
        //         }
        //
        //         Func<T0, T1> resultFunction = function;
        //
        //         retVal = resultFunction(arg);
        //
        //         foreach (var wrapper in managedService.ConverterBehaviours)
        //         {
        //             retVal = wrapper.ChainFunction(arg);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         foreach (var onException in managedService.BehaviourOnExceptions)
        //         {
        //             onException.Execute(ex);
        //         }
        //     }
        //     finally
        //     {
        //         foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
        //         {
        //             afterStart.Execute();
        //         }
        //     }
        //
        //     return retVal;
        // }
        
        // public static T ManagedFunction<T>(this IManagedService managedService, Func<T> function)
        // {
        //     var retVal = default(T);
        //     
        //     try
        //     {
        //         foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
        //         {
        //             beforeStart.Execute();
        //         }
        //
        //         retVal = function();
        //     }
        //     catch (Exception ex)
        //     {
        //         foreach (var onException in managedService.BehaviourOnExceptions)
        //         {
        //             onException.Execute(ex);
        //         }
        //     }
        //     finally
        //     {
        //         foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
        //         {
        //             afterStart.Execute();
        //         }
        //     }
        //
        //     return retVal;
        // }
        
        public static void ManagedAction(this IManagedService managedService, Action<IContainer> action, IContainer arg)
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Action<IContainer> resultAction = action;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.BehaviourAction, resultAction);
                }

                resultAction(arg);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(arg, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute(arg);
                }
            }
        }

        public static void ManagedAction(this IManagedService managedService, Action execute)
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }

                Action resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.BehaviourAction, resultAction);
                }

                resultAction();
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }
        }
    }

    public class ManagedPublisherService : IRestartable, IManagedService
    {
        protected readonly IPublisher Publisher;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        
        public ManagedPublisherService(
            IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            Publisher = publisher;
            
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }
        
        public virtual void Start()
        {
            throw new NotImplementedException();
        }
         
        public virtual void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public class NumbersPublisherService : ManagedPublisherService
    {
        public NumbersPublisherService(IPublisher publisher, IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions, IEnumerable<IWrapperBehaviour> wrapperBehaviours)
            : base(publisher, basicBehaviours, behaviourOnExceptions, wrapperBehaviours)
        {
        }

        public override void Start()
        {
            var container = new Container();
            
            for (int i = 0; i < 10; i++)
            {
                container.Register<ClassA>(new ClassA
                {
                    A = "TestString",
                    B = i
                });
                
                this.ManagedAction(arg => Publisher.Publish(container.Resolve<ClassA>().ToBytes()), container);
            }
        }
 
    }
 
    public class SaveToDeepStorageBehaviour : WrapperBehaviour 
    {
        public override void BehaviourAction(Action<IContainer> action, IContainer container)
        {
            Console.WriteLine($"File was written into deep storage {container.Resolve<ClassA>().A}");
            
            action(container);
        }
    }

    public class ClassA
    {
        public string A { get; set; }
        public int B { get; set; }
    }

    public class ClassB
    {
        public string B { get; set; }
        public int C { get; set; }
    }

    // public class ConverterBehaviour : ConverterBehaviour<ClassA, ClassB>
    // {
    //     public override ClassB ChainFunction(ClassA arg)
    //     {
    //         return new ClassB()
    //         {
    //             B = arg.A
    //         };
    //     }
    // }
    //

    public class SimplePublisher : IPublisher
    {
        public void Publish(byte[] data)
        {
            
        }

        public void Publish(byte[] data, string routingKey)
        {
           
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
             
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
          
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var simplePublisher = new SimplePublisher();

            var basicBehaviours = new List<IBasicBehaviour>(){};
            var behaviourOnExceptions = new List<IBehaviourOnException>();

            var wrapperBehaviours = new List<IWrapperBehaviour>()
            {
                new SaveToDeepStorageBehaviour()
            }; 
            
            var numbers = new NumbersPublisherService(simplePublisher, basicBehaviours, behaviourOnExceptions, wrapperBehaviours);
            
            numbers.Start();
        }
    }
    //
    // [ApplicationTemplate]
    // public class HtmlSimpleParser : ApplicationReceiverAndPublisherRole
    // {
    //     public HtmlSimpleParser(ApplicationReceiverRole receiverRole, ApplicationPublisherRole publisherRole) : base(
    //         receiverRole, publisherRole)
    //     {
    //     }
    //
    //     public override void Start()
    //     {
    //         this.Listen(this.OnReceive);
    //     }
    //
    //     public override void Stop()
    //     {
    //         this.StopListen();
    //     }
    //
    //     private void OnReceive(byte[] data)
    //     {
    //         Console.WriteLine($"Data for parsing: { Encoding.UTF8.GetString(data) }");
    //         Console.WriteLine($"Parsing...");
    //
    //         this.Publish(data);
    //     }
    // }
    
    // [ApplicationTemplate]
    // public class HttpSimplePageDownloaderGenerator : ApplicationPublisherRole
    // {
    //     public HttpSimplePageDownloaderGenerator(IPublisher publisher, IEnumerable<IBehaviour> behaviours,
    //         IEnumerable<IConverter> converters) : base(publisher, behaviours, converters)
    //     {
    //     }
    //
    //     public override void Start()
    //     {
    //         Console.WriteLine("Download something...");
    //
    //         var someData = Encoding.UTF8.GetBytes(DateTime.Now.Second.ToString());
    //
    //         this.Publish(someData);
    //     }
    //
    //     public override void Stop()
    //     {
    //         
    //     }
    // }

}