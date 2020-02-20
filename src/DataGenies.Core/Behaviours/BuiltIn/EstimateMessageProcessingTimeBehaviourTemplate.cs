using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DataGenies.Core.Attributes;

namespace DataGenies.Core.Behaviours.BuiltIn
{
    [BehaviourTemplate]
    public class EstimateMessageProcessingTimeBehaviourTemplate : WrapperBehaviourTemplate
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;

        public override void BehaviourActionWithMessage<T>(Action<T> action, T message)
        {
            _sw.Reset();
            
            try
            {
                _sw.Start();
                action(message);
            }
            finally
            {
                _sw.Stop();
            }
        }
    }
}