using BehaviorTree.Actions;
using Services;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class MoveToTargetNode : BehaviorNode
    {
        public ObjectField agent;
        public ObjectField targetPosition;
        public MoveToTargetNode(ParameterContainer container) : base("MoveToTargetNode",container)
        {
            AddInputPort("Input Port");
            InitialiseField();
            Node = new MoveToTarget(container,GUID);
        }
        public MoveToTargetNode(string guid,ParameterContainer container) : base("MoveToTargetNode", guid,container)
        {
            AddInputPort("Input Port");
            InitialiseField();
            Node = new MoveToTarget(container, guid);
        }

        private void InitialiseField()
        {
            agent = new ObjectField("NavMeshAgent");
            agent.objectType = typeof(NavMeshAgent);
            agent.allowSceneObjects = true;

            agent.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Agent {GUID}", (NavMeshAgent)evt.newValue));

            targetPosition = new ObjectField("Target Transform");
            targetPosition.objectType = typeof(Transform);
            targetPosition.allowSceneObjects = true;

            targetPosition.RegisterValueChangedCallback(evt
                => Container.SetParameter($"TargetPosition {GUID}", (Transform)evt.newValue));
            
            Add(agent);
            Add(targetPosition);
        }
    }
}