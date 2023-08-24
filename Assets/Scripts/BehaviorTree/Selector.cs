using System;
using System.Collections.Generic;
using BehaviorTree.Actions;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class Selector : BranchNode
    {

        public Selector(string id, ParameterContainer container) : base(container,id)
        {
            base.id = id;
        }
        public override void Initialise()
        {
            for (int i = 0; i < nextNodes.Count; i++)
            {
                NextNode nextNode = nextNodes[i];
                Type type = Type.GetType(nextNode.type);
                nextNode.node = (ANode)JsonUtility.FromJson(nextNode.nodeData, type);
                nextNodes[i] = nextNode;
            }
        }

        public override NodeStatus Tick()
        {
            foreach (var child in nextNodes)
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