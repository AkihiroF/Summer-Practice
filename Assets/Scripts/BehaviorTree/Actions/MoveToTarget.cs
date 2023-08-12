using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Actions
{
    public class MoveToTarget : ANode
    {
        private readonly NavMeshAgent agent;
        private readonly Vector3 targetPosition;
        private float stoppingDistance;

        public MoveToTarget(NavMeshAgent agent, Vector3 targetPosition)
        {
            this.agent = agent;
            this.targetPosition = targetPosition;
        }

        public override void Initialise()
        {
            stoppingDistance = agent.stoppingDistance;
            NodeStatus = NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            agent.SetDestination(targetPosition);

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
    }
}