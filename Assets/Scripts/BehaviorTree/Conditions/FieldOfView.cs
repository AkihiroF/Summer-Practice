using UnityEngine;

namespace BehaviorTree.Conditions
{
    public class FieldOfView : ANode
    {
        private Transform origin;
        private float radiusVision;
        private float angleVision;
        private LayerMask playerLayer;
        private LayerMask obstacleLayer;

        public override void Initialise()
        {
            NodeStatus = NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            if (FindVisibleTargets() != Vector3.zero)
            {
                return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }
        private Vector3 FindVisibleTargets() {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(origin.position, radiusVision, playerLayer);

            for (int i = 0; i < targetsInViewRadius.Length; i++) {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - origin.position).normalized;
                if (Vector3.Angle(origin.forward, dirToTarget) < angleVision / 2) {
                    float dstToTarget = Vector3.Distance(origin.position, target.position);
                    if (!Physics.Raycast(origin.position, dirToTarget, dstToTarget, obstacleLayer))
                    {
                        return target.position;
                    }
                }
            }

            return Vector3.zero;
        }

    }
}