using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class ANode
    {
        [SerializeField] protected NodeStatus nodeStatus; // Status of the node
        protected readonly ParameterContainer Container; // Container for parameters
        [SerializeField] protected string id; // Unique identifier for the node

        // Constructor to initialize the node with a container and an ID
        [JsonConstructor]
        protected ANode(ParameterContainer container, string id)
        {
            Container = container;
            this.id = id;
        }

        // Property to get the status of the node
        public NodeStatus Status => nodeStatus;

        // Virtual method to initialize the node, can be overridden by derived classes
        public virtual void Initialise()
        {
            
        }

        // Virtual method to update the node's status, can be overridden by derived classes
        public virtual NodeStatus Tick()
        {
            return NodeStatus.Running;
        }

        // Virtual method to terminate the node with a given status, can be overridden by derived classes
        public virtual void Terminate(NodeStatus status)
        {
            
        }
    }
}