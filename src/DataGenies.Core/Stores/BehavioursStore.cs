using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Behaviours;

namespace DataGenies.Core.Stores
{
    public class BehavioursStore : Dictionary<BehaviourType, IEnumerable<IBehaviour>>
    {
        public BehavioursStore(IEnumerable<IBehaviour> behaviours)
        {
            var storeItems = behaviours.ToList();
            
            this.Add(BehaviourType.BeforeStart,  storeItems.Where(w => w.Type == BehaviourType.BeforeStart));
            this.Add(BehaviourType.AfterStart,  storeItems.Where(w => w.Type == BehaviourType.BeforeStart));
            this.Add(BehaviourType.DuringRunning,  storeItems.Where(w => w.Type == BehaviourType.BeforeStart));
            this.Add(BehaviourType.OnException,  storeItems.Where(w => w.Type == BehaviourType.BeforeStart));
        }
    }
}