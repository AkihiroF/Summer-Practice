using BehaviorTree.Actions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class MoveToTargetNode : BehaviorNode
    {
        // UI fields for configuring the NavMeshAgent and target position
        public ObjectField agent;
        public ObjectField targetPosition;

        // Constructor for creating a new MoveToTargetNode with a new GUID
        public MoveToTargetNode(ParameterContainer container) : base("MoveToTargetNode", container)
        {
            AddInputPort("Input Port");
            InitialiseField();
            Node = new MoveToTarget(container, GUID);
        }

        // Constructor for creating a MoveToTargetNode with an existing GUID
        public MoveToTargetNode(string guid, ParameterContainer container) : base("MoveToTargetNode", guid, container)
        {
            AddInputPort("Input Port");
            InitialiseField();
            Node = new MoveToTarget(container, guid);
        }

        // Method to initialize the UI fields
        private void InitialiseField()
        {
            // NavMeshAgent field
            agent = new ObjectField("NavMeshAgent");
            agent.objectType = typeof(NavMeshAgent);
            agent.allowSceneObjects = true;
            agent.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Agent {GUID}", (NavMeshAgent)evt.newValue));

            // Target position field
            targetPosition = new ObjectField("Target Transform");
            targetPosition.objectType = typeof(Transform);
            targetPosition.allowSceneObjects = true;
            targetPosition.RegisterValueChangedCallback(evt
                => Container.SetParameter($"TargetPosition {GUID}", (Transform)evt.newValue));
            
            // Adding fields to the node
            Add(agent);
            Add(targetPosition);
        }
    }
}