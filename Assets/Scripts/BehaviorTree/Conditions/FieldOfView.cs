using System;
using UnityEngine;

namespace BehaviorTree.Conditions
{
    [Serializable]
    public class FieldOfView : ActionNode
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
            nodeStatus = NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            Debug.Log(origin);
            if (FindVisibleTargets() != Vector3.zero)
            {
                return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }

        public override void SetParameters()
        {
            origin = Container.GetParameter<Transform>($"Origin {id}");
            radiusVision = Container.GetParameter<float>($"Angle Vision {id}");
            angleVision = Container.GetParameter<float>($"Radius Vision {id}");
            playerLayer = Container.GetParameter<LayerMask>($"Target Layer {id}");
            obstacleLayer = Container.GetParameter<LayerMask>($"Obstacle Layer {id}");
        }

        private Vector3 FindVisibleTargets() {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(origin.position, radiusVision, playerLayer);
            Debug.Log(targetsInViewRadius.Length);
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