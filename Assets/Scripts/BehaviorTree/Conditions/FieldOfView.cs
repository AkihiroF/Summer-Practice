using System;
using UnityEngine;

namespace BehaviorTree.Conditions
{
    [Serializable]
    public class FieldOfView : ActionNode
    {
        [SerializeField] private Transform origin;
        [SerializeField] private float radiusVision;
        [SerializeField] private float angleVision;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask obstacleLayer;

        // Constructor to initialize the FieldOfView condition with a container and ID
        public FieldOfView(ParameterContainer container, string id) : base(container, id)
        {
        }

        // Initialization of the node, setting the status to Running
        public override void Initialise()
        {
            nodeStatus = NodeStatus.Running;
        }

        // Tick logic for checking the field of view, returning Success if a target is found
        public override NodeStatus Tick()
        {
            if (FindVisibleTargets() != Vector3.zero)
            {
                return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }

        // Setting parameters for the condition, retrieving values from the container
        public override void SetParameters()
        {
            origin = Container.GetParameter<Transform>($"Origin {id}");
            radiusVision = Container.GetParameter<float>($"Radius Vision {id}"); // Fixed parameter name
            angleVision = Container.GetParameter<float>($"Angle Vision {id}"); // Fixed parameter name
            playerLayer = Container.GetParameter<LayerMask>($"Target Layer {id}");
            obstacleLayer = Container.GetParameter<LayerMask>($"Obstacle Layer {id}");
        }

        // Finds visible targets within the field of view, considering obstacles
        private Vector3 FindVisibleTargets()
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(origin.position, radiusVision, playerLayer);
            foreach (var collider in targetsInViewRadius)
            {
                Transform target = collider.transform;
                Vector3 dirToTarget = (target.position - origin.position).normalized;
                if (Vector3.Angle(origin.forward, dirToTarget) < angleVision / 2)
                {
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
