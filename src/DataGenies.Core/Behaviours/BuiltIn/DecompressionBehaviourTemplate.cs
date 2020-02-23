using DataGenies.Core.Attributes;
using DataGenies.Core.Extensions;
using DataGenies.Core.Models;

namespace DataGenies.Core.Behaviours.BuiltIn
{
    [BehaviourTemplate]
    public class DecompressionBehaviourTemplate : BehaviourTemplate
    {
        public override void Execute(MqMessage message)
        {
            message.Body = message.Body.Decompress();
            base.Execute(message);
        }
    }
}