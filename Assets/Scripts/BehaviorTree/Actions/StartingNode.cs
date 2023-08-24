using System;
using UnityEngine;

namespace BehaviorTree.Actions
{
    [Serializable]
    public class StartingNode : BranchNode
    {
        public override void Initialise()
        {
            Debug.Log(nextNodes.Count);
            var nextNode = nextNodes[0];
            Type type = Type.GetType(nextNode.type);
            nextNode.node = (ANode)JsonUtility.FromJson(JsonUtility.ToJson(nextNode.node), type);
            nextNodes[0] = nextNode;
        }

        public override NodeStatus Tick()
        {
            Debug.Log(nextNodes[0].node);
            return nextNodes[0].node.Tick();
        }

        public StartingNode(ParameterContainer container, string id) : base(container, id)
        {
        }
    }
    [Serializable]
    public struct NextNode
    {
        public string nodeData;
        public string type;
        public ANode node;
    }
}