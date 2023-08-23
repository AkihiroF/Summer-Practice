using BehaviorTree.Conditions;
using Services;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class FieldOfViewNode : BehaviorNode
    {
        public ObjectField Origin;
        public FloatField RadiusVision;
        public FloatField AngleVision;
        public LayerMaskField PlayerLayer;
        public LayerMaskField ObstacleLayer;
        
        
        public FieldOfViewNode(ParameterContainer container) : base("FieldOfViewNode", container)
        {
            AddInputPort("Input Port");
            InitialiseFields();
            Node = new FieldOfView(container,GUID);
        }

        public FieldOfViewNode(string guid, ParameterContainer container) : base("FieldOfViewNode", guid, container)
        {
            AddInputPort("Input Port");
            InitialiseFields();
            Node = new FieldOfView(container,guid);
        }

        private void InitialiseFields()
        {
            Origin = new ObjectField("Origin");
            Origin.objectType = typeof(Transform);
            Origin.allowSceneObjects = true;
            Origin.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Origin {GUID}", (Transform)evt.newValue));

            AngleVision = new FloatField("Angle Vision");
            AngleVision.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Angle Vision {GUID}",evt.newValue));

            RadiusVision = new FloatField("Radius Vision");
            RadiusVision.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Radius Vision {GUID}", evt.newValue));

            PlayerLayer = new LayerMaskField("Target Layer");
            PlayerLayer.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Target Layer {GUID}", evt.newValue));

            ObstacleLayer = new LayerMaskField("Obstacle Layer");
            ObstacleLayer.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Obstacle Layer {GUID}", evt.newValue));
            
            Add(Origin);
            Add(RadiusVision);
            Add(AngleVision);
            Add(PlayerLayer);
            Add(ObstacleLayer);
        }
    }
}