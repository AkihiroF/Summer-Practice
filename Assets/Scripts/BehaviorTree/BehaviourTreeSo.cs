using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeSo : ScriptableObject
    {
        public string StartingNodeData; // Serialized data for the starting node
        public string TypeNode; // Type of the node
        public ANode StartingNode; // Reference to the starting node

        public List<string> keys; // List of keys for the nodes
        public List<string> nodes; // List of nodes

        private Dictionary<string, string> convertingNodes; // Dictionary to hold the conversion of nodes

        // Method to convert the nodes
        public void Convert()
        {
            convertingNodes = new Dictionary<string, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                convertingNodes.Add(keys[i], nodes[i]); // Adding keys and nodes to the dictionary
            }
            Type type = Type.GetType(TypeNode); // Getting the type of the node
            StartingNode = (ANode)JsonUtility.FromJson(StartingNodeData, type); // Deserializing the starting node data
            if (StartingNode is BranchNode branchNode)
                branchNode.DeserializeChildren(convertingNodes); // Deserializing the children if the starting node is a branch node
        }
    }
}