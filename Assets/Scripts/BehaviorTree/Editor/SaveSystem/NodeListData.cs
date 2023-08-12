using System;
using System.Collections.Generic;

namespace BehaviorTree.Editor.SaveSystem
{
    [Serializable]
    public struct NodeListData
    {
        public List<string> nodes;
        public List<string> nodesType;
        public List<EdgeData> edges;
    }
}