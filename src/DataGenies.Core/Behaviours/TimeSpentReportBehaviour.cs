using System;
using System.Diagnostics;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    // public class TimeSpentReportBehaviour : GenericBehaviour
    // {
    //     public override BehaviourType Type { get; set; } = BehaviourType.DuringRunning;
    //
    //     private Stopwatch sw = new Stopwatch();
    //
    //     public override void DoSomethingAfterStart()
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public override void DoSomethingDuringRunning(Action action)
    //     {
    //         sw.Restart();
    //         sw.Start();
    //
    //         action();
    //
    //         sw.Stop();
    //         Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds}ms...");
    //     }
    //
    //     public override void DoSomethingOnException(Exception ex = null)
    //     {
    //         throw new NotImplementedException();
    //     }
    //     
    //     public override void DoSomethingBeforeStart()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}