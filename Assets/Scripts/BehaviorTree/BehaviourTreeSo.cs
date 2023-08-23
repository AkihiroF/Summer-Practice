using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeSo : ScriptableObject
    {
        public List<string> Nodes;
        public List<string> TypesNode;
        public List<ANode> NodesTree;

        public void Convert()
        {
            NodesTree = new List<ANode>();
            for (int i = 0; i < Nodes.Count(); i++)
            {
                Type type = Type.GetType(TypesNode[i]);
                var node =(ANode)JsonUtility.FromJson(Nodes[i], type);
                NodesTree.Add(node);
            }
        }
    }
}