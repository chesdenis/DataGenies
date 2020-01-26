using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions.Behaviours;

namespace DataGenies.AspNetCore.DataGeniesCore.RunnableScenarios
{
    public class DefaultStartableScenario : IStartable
    {
        private readonly IStartable component;
        private readonly IEnumerable<IBehaviour> behaviours;
        private readonly IEnumerable<IConverter> converters;

        private IBehaviour[] BeforeRun() => this.behaviours.Where(w => w.Type == BehaviourType.BeforeRun).ToArray();
        private IBehaviour[] AfterRun() => this.behaviours.Where(w => w.Type == BehaviourType.AfterRun).ToArray();
        private IBehaviour[] DuringRun() => this.behaviours.Where(w => w.Type == BehaviourType.DuringRun).ToArray();
        private IBehaviour[] OnException() => this.behaviours.Where(w => w.Type == BehaviourType.OnException).ToArray();

        public DefaultStartableScenario(IStartable component,
            IEnumerable<IBehaviour> behaviours,
            IEnumerable<IConverter> converters)
        {
            this.component = component;
            this.behaviours = behaviours;
            this.converters = converters;
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
    }
}