using System;
using System.Diagnostics;
using DataGenies.Core.Attributes;

namespace DataGenies.Core.Behaviours.BuiltIn
{
    [BehaviourTemplate]
    public class EstimateServiceProcessingTimeBehaviourTemplate : WrapperBehaviourTemplate
    {
        private readonly Stopwatch sw = new Stopwatch();

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;

        public override void BehaviourActionWithContainer<T>(Action<T> action, T container)
        {
            this.sw.Reset();

            try
            {
                this.sw.Start();
                action(container);
            }
            finally
            {
                this.sw.Stop();
            }
        }
    }
}