using System;
using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IApplicationBehavioursScanner
    {
        IEnumerable<BehaviourEntity> ScanBehaviours();

        IEnumerable<IBehaviour> GetBehavioursInstances(IEnumerable<BehaviourEntity> behaviours);
    }
}