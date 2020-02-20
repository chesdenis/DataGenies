using System;
using System.Diagnostics;
using DataGenies.Core.Attributes;

namespace DataGenies.Core.Behaviours.BuiltIn
{
    [BehaviourTemplate]
    public class EstimateServiceProcessingTimeBehaviourTemplate : WrapperBehaviourTemplate
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;

        public override void BehaviourActionWithContainer<T>(Action<T> action, T container)
        {
            _sw.Reset();
            
            try
            {
                _sw.Start();
                action(container);
            }
            finally
            {
                _sw.Stop();
            }
        }
    }
}