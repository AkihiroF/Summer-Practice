using System;
using UnityEngine;

namespace BehaviorTree.Editor.SaveSystem
{
    [Serializable]
    public class NodeData
    {
        public string guid;
        public string type;
        public Vector2 position;
        public ANode Node;
    }
}