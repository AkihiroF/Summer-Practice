using System;

namespace BehaviorTree
{
    [Serializable]
    public struct NextNode
    {
        public string nodeData;
        public string type;
        public ANode node;
    }
}