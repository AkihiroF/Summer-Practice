using System;
using UnityEngine; // Using Newtonsoft.Json instead of JsonUtility

namespace BehaviorTree.Actions
{
    [Serializable]
    public class StartingNode : BranchNode
    {
        // Constructor to initialize the starting node with a container and ID
        public StartingNode(ParameterContainer container, string id) : base(container, id)
        {
        }

        // Initialization of the node, deserializing the first child node
        public override void Initialise()
        {
            var nextNode = nextNodes.nextNodes[0];
            Type type = Type.GetType(nextNode.type);
            nextNode.node = (ANode)JsonUtility.FromJson(JsonUtility.ToJson(nextNode.node), type);
            nextNodes.nextNodes[0] = nextNode;
        }

        // Tick logic for the starting node, executing the first child node
        public override NodeStatus Tick()
        {
            return nextNodes.nextNodes[0].node.Tick();
        }
    }
}