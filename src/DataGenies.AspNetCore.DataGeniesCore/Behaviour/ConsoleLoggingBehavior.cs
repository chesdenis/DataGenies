using System;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction.Behaviour;

namespace DataGenies.AspNetCore.DataGeniesCore.Behaviour
{
    public class ConsoleLoggingBehavior : GenericBehaviour
    {
        public override BehaviourType Type { get; set; } = BehaviourType.DuringRun;
         
        public override void ExecuteAction(Action action)
        {
            Console.WriteLine("Start executing...");

            action();

            Console.WriteLine("End execution...");
        }

        public override void ExecuteException(Exception ex = null)
        {
            Console.WriteLine($"Exception occured {ex?.Message}, {ex?.StackTrace}");
        }

        public override void ExecuteScalar()
        {
            throw new NotImplementedException();
        }
    }
}