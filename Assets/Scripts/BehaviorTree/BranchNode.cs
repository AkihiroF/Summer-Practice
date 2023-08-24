using System;
using System.Collections.Generic;
using BehaviorTree.Actions;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class BranchNode : ANode
    {
        public string ID => id;
        public ListNextNodes NextNodes => new ListNextNodes()
        {
            nextNodes = this.nextNodes
        };

        protected BranchNode(ParameterContainer container, string id) : base(container, id)
        {
        }
        protected List<NextNode> nextNodes;

        public void SaveChildes()
        {
            nextNodes = new List<NextNode>();
            var nodes = Container.GetParameter<List<ANode>>($"ChildNodes {id}");
            foreach (var node in nodes)
            {
                nextNodes.Add(new NextNode()
                {
                    type = node.GetType().FullName,
                    nodeData = JsonUtility.ToJson(node)
                });
            }
        }

        public void SetChildes(Dictionary<string, string> nodes)
        {
            var nodesData = nodes[id];
            nextNodes = JsonUtility.FromJson<ListNextNodes>(nodesData).nextNodes;
            for(int i = 0; i < nextNodes.Count; i++)
            {
                var currentNode = nextNodes[i];
                Type type = Type.GetType(currentNode.type);
                var newNode = (ANode)JsonUtility.FromJson(currentNode.nodeData, type);
                currentNode.node = newNode;
                nextNodes[i] = currentNode;
                if (currentNode.node is BranchNode branchNode)
                {
                    branchNode.SetChildes(nodes);
                }
            }
        }
    }

    [Serializable]
    public struct ListNextNodes
    {
        public List<NextNode> nextNodes;
    }
}