using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Editor.SaveSystem.Nodes
{
    [Serializable]
    public class MoveToTargetNodeData : NodeData
    {
        public NavMeshAgent agent;
        public Transform targetPosition;
    }
}