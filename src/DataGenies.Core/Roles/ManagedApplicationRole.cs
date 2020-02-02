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

        private IBehaviour[] BeforeRun() => this.behaviours.Where(w => w.Type == BehaviourType.BeforeRun).ToArray();
        private IBehaviour[] AfterRun() => this.behaviours.Where(w => w.Type == BehaviourType.AfterRun).ToArray();
        private IBehaviour[] DuringRun() => this.behaviours.Where(w => w.Type == BehaviourType.DuringRun).ToArray();
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
                Array.ForEach(this.BeforeRun(), (t) => t.ExecuteScalar());

                Action resultAction = this.component.Start;

                foreach (var behaviour in this.DuringRun())
                {
                    resultAction = behaviour.Wrap(behaviour.ExecuteAction, resultAction);
                }

                resultAction();
            }
            catch (Exception ex)
            {
                Array.ForEach(this.OnException(), t => t.ExecuteException(ex));
            }
            finally
            {
                Array.ForEach(this.AfterRun(), t => t.ExecuteScalar());
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