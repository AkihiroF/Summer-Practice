using System;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Actions
{
    [Serializable]
    public class MoveToTarget : ActionNode
    {
        [SerializeField]private  NavMeshAgent agent;
        [SerializeField]private Vector3 targetPosition;
        [SerializeField]private float stoppingDistance;

        public override void Initialise()
        {
            nodeStatus = NodeStatus.Running;
        }

        public override void SetParameters()
        {
            agent = Container.GetParameter<NavMeshAgent>($"Agent {id}");
            targetPosition = Container.GetParameter<Transform>($"TargetPosition {id}").position;
            stoppingDistance = agent.stoppingDistance;
        }

        public override NodeStatus Tick()
        {
            agent.Move(targetPosition);

            if (agent.remainingDistance <= stoppingDistance)
            {
                nodeStatus = NodeStatus.Success;
            }

            return nodeStatus;
        }

        public override void Terminate(NodeStatus status)
        {
            base.Terminate(status);
        }

        public MoveToTarget(ParameterContainer container, string id) : base(container, id)
        {
        }
    }
}