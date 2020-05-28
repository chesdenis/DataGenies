namespace DG.Core.BrokerSynchronizer
{
    using System.Collections.Concurrent;

    public class UpdatesQueue : ConcurrentQueue<BrokerUpdate>
    {
        private object lockObject = new object();

        public int Limit { get; set; }

        public void EnqueueUpdate(BrokerUpdate obj)
        {
            this.Enqueue(obj);
            lock (this.lockObject)
            {
                BrokerUpdate overflow;
                while (this.Count > this.Limit && this.TryDequeue(out overflow))
                {
                }
            }
        }
    }
}