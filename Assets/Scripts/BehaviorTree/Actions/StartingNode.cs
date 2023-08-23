using System;
using UnityEngine;

namespace BehaviorTree.Actions
{
    [Serializable]
    public class StartingNode : ANode
    {
        [SerializeField]private NextNode nextNode;
        public override void Initialise()
        {
            nextNode = new NextNode();
            nextNode.node = Container.GetParameter<ANode>($"Start {ID}");
            nextNode.type = nextNode.node.GetType().FullName;
            Type type = Type.GetType(nextNode.type);
            nextNode.node = (ANode)JsonUtility.FromJson(JsonUtility.ToJson(nextNode.node), type);
        }

        public override NodeStatus Tick()
        {
            return nextNode.node.Tick();
        }

        public StartingNode(ParameterContainer container, string id) : base(container, id)
        {
        }
    }
    [Serializable]
    public struct NextNode
    {
        public ANode node;
        public string type;
    }
}