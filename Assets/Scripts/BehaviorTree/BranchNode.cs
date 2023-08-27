using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class BranchNode : ANode
    {
        // Unique identifier for the branch node
        public string ID => id;

        // List of next nodes in the behavior tree
        protected ListNextNodes nextNodes;

        // Public accessor for next nodes
        public ListNextNodes NextNodes => nextNodes;

        // Constructor to initialize the branch node with parameters
        public BranchNode(ParameterContainer container, string id) : base(container, id)
        {
        }

        // Serializes the children nodes into JSON format
        public void SerializeChildren()
        {
            nextNodes = new ListNextNodes
            {
                nextNodes = new List<NextNode>()
            };

            var nodes = Container.GetParameter<List<ANode>>($"ChildNodes {id}");
            if(nodes == null)
                return;
            
            foreach (var node in nodes)
            {
                nextNodes.nextNodes.Add(new NextNode()
                {
                    type = node.GetType().FullName,
                    nodeData = JsonUtility.ToJson(node)
                });
            }
        }

        // Deserializes the children nodes from JSON format
        public void DeserializeChildren(Dictionary<string, string> nodes)
        {
            var nodesData = nodes[id];
            nextNodes = JsonUtility.FromJson<ListNextNodes>(nodesData);
            for(int i = 0; i < nextNodes.nextNodes.Count; i++)
            {
                var currentNode = nextNodes.nextNodes[i];
                Type type = Type.GetType(currentNode.type);
                var newNode = (ANode)JsonUtility.FromJson(currentNode.nodeData, type);
                currentNode.node = newNode;
                nextNodes.nextNodes[i] = currentNode;
                if (currentNode.node is BranchNode branchNode)
                {
                    branchNode.DeserializeChildren(nodes);
                }
            }
        }
    }
}
