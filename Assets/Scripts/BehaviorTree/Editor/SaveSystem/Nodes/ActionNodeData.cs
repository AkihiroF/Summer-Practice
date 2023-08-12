using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Editor.SaveSystem.Nodes
{
    [Serializable]
    public class ActionNodeData : NodeData
    {
        public NavMeshAgent agent;
        public  Vector3 targetPosition;
        public float stoppingDistance;
    }
}