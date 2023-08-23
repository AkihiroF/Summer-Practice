using System;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Actions
{
    [Serializable]
    public class MoveToTarget : ANode
    {
        [SerializeField]private  NavMeshAgent agent;
        [SerializeField]private Vector3 targetPosition;
        [SerializeField]private float stoppingDistance;

        public override void Initialise()
        {
            agent = Container.GetParameter<NavMeshAgent>($"Agent {ID}");
            targetPosition = Container.GetParameter<Transform>($"TargetPosition {ID}").position;
            stoppingDistance = agent.stoppingDistance;
            NodeStatus = NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            agent.Move(targetPosition);

            if (agent.remainingDistance <= stoppingDistance)
            {
                NodeStatus = NodeStatus.Success;
            }

            return NodeStatus;
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