using System;
using BehaviorTree.Actions;
using Newtonsoft.Json; // Using Newtonsoft.Json instead of JsonUtility

namespace BehaviorTree
{
    [Serializable]
    public class Selector : BranchNode
    {
        // Constructor to initialize the selector with an ID and parameter container
        public Selector(string id, ParameterContainer container) : base(container, id)
        {
        }

        // Initialization of the node, deserializing child nodes
        public override void Initialise()
        {
            for (int i = 0; i < nextNodes.nextNodes.Count; i++)
            {
                NextNode nextNode = nextNodes.nextNodes[i];
                Type type = Type.GetType(nextNode.type);
                nextNode.node = (ANode)JsonConvert.DeserializeObject(nextNode.nodeData, type); // Using JsonConvert
                nextNodes.nextNodes[i] = nextNode;
            }
        }

        // Executes the selector logic, iterating through child nodes and returning a status
        public override NodeStatus Tick()
        {
            foreach (var child in nextNodes.nextNodes)
            {
                var status = child.node.Tick();
                if (status == NodeStatus.Success)
                    return NodeStatus.Success;
                if (status == NodeStatus.Running)
                    return NodeStatus.Running;
            }

            return NodeStatus.Failed;
        }
    }
}