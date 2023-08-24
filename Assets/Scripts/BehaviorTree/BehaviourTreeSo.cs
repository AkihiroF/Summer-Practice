using System;
using System.Collections.Generic;
using BehaviorTree.Actions;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeSo : ScriptableObject
    {
        public string StartingNodeData;
        public string TypeNode;
        public ANode StartingNode;

        public List<string> keys;
        public List<string> nodes;

        public  Dictionary<string, string> convertingNodes;

        public void Convert()
        {
            convertingNodes = new Dictionary<string, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                convertingNodes.Add(keys[i], nodes[i]);
            }
            Type type = Type.GetType(TypeNode);
            StartingNode = (ANode)JsonUtility.FromJson(StartingNodeData, type);
            if(StartingNode is BranchNode branchNode)
                branchNode.SetChildes(convertingNodes);
        }
    }
}