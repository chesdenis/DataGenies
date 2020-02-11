using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Roles
{
    public class ApplicationReceiverRole : IReceiver, IRestartable
    {
        private readonly IReceiver _receiver;
        private readonly IEnumerable<IBehaviour> _behaviours;
        private readonly IEnumerable<IConverter> _converters;
        
        private IBehaviour[] BeforeStart() => this._behaviours.Where(w => w.Type == BehaviourType.BeforeStart).ToArray();
        private IBehaviour[] AfterStart() => this._behaviours.Where(w => w.Type == BehaviourType.AfterStart).ToArray();
        private IBehaviour[] DuringRunning() => this._behaviours.Where(w => w.Type == BehaviourType.DuringRunning).ToArray();
        private IBehaviour[] OnException() => this._behaviours.Where(w => w.Type == BehaviourType.OnException).ToArray();

        private IConverter[] AfterReceive() => this._converters.Where(w => w.Type == ConverterType.AfterReceive).ToArray();

        public ApplicationReceiverRole(IReceiver receiver, IEnumerable<IBehaviour> behaviours, IEnumerable<IConverter> converters)
        {
            _receiver = receiver;
            _behaviours = behaviours;
            _converters = converters;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            this._receiver.Listen((data) => 
            {
                foreach (var converter in AfterReceive())
                {
                    data = converter.Convert(data);
                }

                try
                {
                    Array.ForEach(this.BeforeStart(), (t) => t.DoSomethingBeforeStart(data));

                    Action<byte[]> resultAction = onReceive;

                    foreach (var behaviour in this.DuringRunning())
                    {
                        resultAction = behaviour.Wrap(behaviour.DoSomethingDuringRunning, resultAction);
                    }

                    resultAction(data);
                }
                catch (Exception ex)
                {
                    Array.ForEach(this.OnException(), t => t.DoSomethingOnException(ex));
                }
                finally
                {
                    Array.ForEach(this.AfterStart(), t => t.DoSomethingBeforeStart());
                }
                
                onReceive(data);
            });
        }

        public void StopListen()
        {
            _receiver.StopListen();
        }

        public virtual void Start() => throw new NotImplementedException();
        public virtual void Stop() => throw new NotImplementedException();
    }
}