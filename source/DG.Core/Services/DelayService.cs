namespace DG.Core.Services
{
    using System.Threading.Tasks;

    public class DelayService : IDelayService
    {
        public void Waitms(int millisecondsDelay)
        {
            Task.Delay(millisecondsDelay);
        }
    }
}
