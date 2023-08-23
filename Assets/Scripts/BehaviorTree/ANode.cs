using System;

namespace BehaviorTree
{
    [Serializable]
    public abstract class ANode
    {
        protected NodeStatus NodeStatus;
        protected readonly ParameterContainer Container;
        protected string ID;

        protected ANode(ParameterContainer container, string id)
        {
            Container = container;
            ID = id;
        }
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