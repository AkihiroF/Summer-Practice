using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Actions
{
    [Serializable]
    public class MoveToTarget : ActionNode
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private float stoppingDistance;

        // Constructor to initialize the MoveToTarget action with a container and ID
        public MoveToTarget(ParameterContainer container, string id) : base(container, id)
        {
        }

        // Initialization of the node, setting the status to Running
        public override void Initialise()
        {
            nodeStatus = NodeStatus.Running;
        }

        // Setting parameters for the action, retrieving values from the container
        public override void SetParameters()
        {
            agent = Container.GetParameter<NavMeshAgent>($"Agent {id}");
            targetPosition = Container.GetParameter<Transform>($"TargetPosition {id}").position;
            stoppingDistance = agent.stoppingDistance;
        }

        // Tick logic for moving to the target, checking the remaining distance
        public override NodeStatus Tick()
        {
            agent.SetDestination(targetPosition); // Using SetDestination instead of Move

            if (agent.remainingDistance <= stoppingDistance)
            {
                nodeStatus = NodeStatus.Success;
            }

            return nodeStatus;
        }

        // Termination logic, currently calling the base implementation
        public override void Terminate(NodeStatus status)
        {
            base.Terminate(status);
        }
    }
}