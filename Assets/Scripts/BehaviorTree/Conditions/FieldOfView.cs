using System;
using UnityEngine;

namespace BehaviorTree.Conditions
{
    [Serializable]
    public class FieldOfView : ANode
    {
        [SerializeField]private Transform origin;
        [SerializeField]private float radiusVision;
        [SerializeField]private float angleVision;
        [SerializeField]private LayerMask playerLayer;
        [SerializeField]private LayerMask obstacleLayer;

        public FieldOfView(ParameterContainer container, string id) : base(container, id)
        {
        }
        public override void Initialise()
        {
            origin = Container.GetParameter<Transform>($"Origin {ID}");
            radiusVision = Container.GetParameter<float>($"Angle Vision {ID}");
            angleVision = Container.GetParameter<float>($"Radius Vision {ID}");
            playerLayer = Container.GetParameter<LayerMask>($"Target Layer {ID}");
            obstacleLayer = Container.GetParameter<LayerMask>($"Obstacle Layer {ID}");
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