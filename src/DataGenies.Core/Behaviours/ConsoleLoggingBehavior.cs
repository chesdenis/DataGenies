using System;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    // public class ConsoleLoggingBehavior : GenericBehaviour
    // {
    //     public override BehaviourType Type { get; set; } = BehaviourType.DuringRunning;
    //
    //     public override void DoSomethingAfterStart()
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public override void DoSomethingDuringRunning(Action action)
    //     {
    //         Console.WriteLine("Start executing...");
    //
    //         action();
    //
    //         Console.WriteLine("End execution...");
    //     }
    //
    //     public override void DoSomethingOnException(Exception ex = null)
    //     {
    //         Console.WriteLine($"Exception occured {ex?.Message}, {ex?.StackTrace}");
    //     }
    //     
    //     public override void DoSomethingBeforeStart()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}