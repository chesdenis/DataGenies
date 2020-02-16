namespace DataGenies.Core.Behaviours
{
    public interface IBehaviour
    {
        string Name { get; set; }
        string Description { get; set; }
        
        BehaviourScope BehaviourScope { get; set; }

        BehaviourType BehaviourType { get; set; }
    }
}