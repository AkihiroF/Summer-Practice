using System;
using UnityEngine;

namespace BehaviorTree.Editor.SaveSystem.Nodes
{
    [Serializable]
    public class FieldOfViewNodeData : NodeData
    {
        public Transform origin;
        public float radiusVision;
        public float angleVision;
        public LayerMask playerLayer;
        public LayerMask obstacleLayer; 
    }
}