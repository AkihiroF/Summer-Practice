using System;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class ANode
    {
        [SerializeField]protected NodeStatus NodeStatus;
        [SerializeField]protected readonly ParameterContainer Container;
        [SerializeField]protected string ID;

        protected ANode(ParameterContainer container, string id)
        {
            Container = container;
            ID = id;
        }
        public NodeStatus Status => NodeStatus;

        public virtual void Initialise()
        {
            
        }

        public virtual NodeStatus Tick()
        {
            return NodeStatus.Running;
        }

        public virtual void Terminate(NodeStatus status)
        {
            
        }
    }
}