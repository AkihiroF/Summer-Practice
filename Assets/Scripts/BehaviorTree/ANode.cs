namespace BehaviorTree
{
    public abstract class ANode
    {
        protected NodeStatus NodeStatus;

        public NodeStatus Status => NodeStatus;

        public virtual void Initialise()
        {
            
        }

        public abstract NodeStatus Tick();

        public virtual void Terminate(NodeStatus status)
        {
            
        }
    }
}