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
            this.Add(BehaviourType.AfterStart,  storeItems.Where(w => w.Type == BehaviourType.AfterStart));
            this.Add(BehaviourType.StartWrapper,  storeItems.Where(w => w.Type == BehaviourType.StartWrapper));
            this.Add(BehaviourType.OnException,  storeItems.Where(w => w.Type == BehaviourType.OnException));
            this.Add(BehaviourType.OnMessagePublish,  storeItems.Where(w => w.Type == BehaviourType.OnMessagePublish));
            this.Add(BehaviourType.OnMessageReceive,  storeItems.Where(w => w.Type == BehaviourType.OnMessageReceive));
        }
    }
}