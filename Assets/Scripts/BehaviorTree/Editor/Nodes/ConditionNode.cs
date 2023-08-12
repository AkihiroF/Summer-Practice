using BehaviorTree.Conditions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class ConditionNode : BehaviorNode
    {
        public ObjectField origin;
        public FloatField radiusVision;
        public FloatField angleVision;
        public LayerMaskField playerLayer;
        public LayerMaskField obstacleLayer;

        private FieldOfView _fieldOfView;
        public ConditionNode() : base("Condition Node")
        {
            AddInputPort("Input Port");
            InitialiseFields();
        }

        public ConditionNode(string guid) : base("Condition Node", guid)
        {
            AddInputPort("Input Port");
            InitialiseFields();
        }

        private void InitialiseFields()
        {
            origin = new ObjectField("Origin");
            origin.objectType = typeof(Transform);
            origin.allowSceneObjects = true;

            angleVision = new FloatField("Angle Vision");

            radiusVision = new FloatField("Radius Vision");

            playerLayer = new LayerMaskField("Target Layer");

            obstacleLayer = new LayerMaskField("Obstacle Layer");
            
            Add(origin);
            Add(radiusVision);
            Add(angleVision);
            Add(playerLayer);
            Add(obstacleLayer);
        }
    }
}