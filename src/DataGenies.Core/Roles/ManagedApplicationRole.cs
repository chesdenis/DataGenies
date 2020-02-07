using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;

namespace DataGenies.Core.Roles
{
    public class ManagedApplicationRole : IManagedApplicationRole
    {
        private readonly IRestartable component;
        private readonly IEnumerable<IBehaviour> behaviours;

        private IBehaviour[] BeforeStart() => this.behaviours.Where(w => w.Type == BehaviourType.BeforeStart).ToArray();
        private IBehaviour[] AfterStart() => this.behaviours.Where(w => w.Type == BehaviourType.AfterStart).ToArray();
        private IBehaviour[] DuringRunning() => this.behaviours.Where(w => w.Type == BehaviourType.DuringRunning).ToArray();
        private IBehaviour[] OnException() => this.behaviours.Where(w => w.Type == BehaviourType.OnException).ToArray();

        public ManagedApplicationRole(IRestartable component,
            IEnumerable<IBehaviour> behaviours)
        {
            this.component = component;
            this.behaviours = behaviours;
        }
         
        public void Start()
        {
            try
            {
                Array.ForEach(this.BeforeStart(), (t) => t.DoSomethingBeforeStart());

                Action resultAction = this.component.Start;

                foreach (var behaviour in this.DuringRunning())
                {
                    resultAction = behaviour.Wrap(behaviour.DoSomethingDuringRunning, resultAction);
                }

                resultAction();
            }
            catch (Exception ex)
            {
                Array.ForEach(this.OnException(), t => t.DoSomethingOnException(ex));
            }
            finally
            {
                Array.ForEach(this.AfterStart(), t => t.DoSomethingBeforeStart());
            }
        }
 
        public void Stop()
        {
            this.component.Stop();
        }

        public IRestartable GetRootComponent()
        {
            return this.component;
        }
    }
}