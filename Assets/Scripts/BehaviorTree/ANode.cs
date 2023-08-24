using System;
using System.Collections.Generic;
using BehaviorTree.Actions;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class ANode
    {
        [SerializeField]protected NodeStatus nodeStatus;
        protected readonly ParameterContainer Container;
        [SerializeField]protected string id;

        protected ANode(ParameterContainer container, string id)
        {
            Container = container;
            this.id = id;
        }
        public NodeStatus Status => nodeStatus;

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