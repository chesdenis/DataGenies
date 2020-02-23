using System;
using System.Diagnostics;
using DataGenies.Core.Attributes;

namespace DataGenies.Core.Behaviours.BuiltIn
{
    [BehaviourTemplate]
    public class EstimateMessageProcessingTimeBehaviourTemplate : WrapperBehaviourTemplate
    {
        private readonly Stopwatch sw = new Stopwatch();

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;

        public override void BehaviourActionWithMessage<T>(Action<T> action, T message)
        {
            this.sw.Reset();
            try
            {
                this.sw.Start();
                action(message);
            }
            finally
            {
                this.sw.Stop();
            }
        }
    }
}