using BehaviorTree.Conditions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class FieldOfViewNode : BehaviorNode
    {
        // UI fields for configuring the Field of View
        public ObjectField Origin;
        public FloatField RadiusVision;
        public FloatField AngleVision;
        public LayerMaskField PlayerLayer;
        public LayerMaskField ObstacleLayer;
        
        // Constructor for creating a new FieldOfViewNode with a new GUID
        public FieldOfViewNode(ParameterContainer container) : base("FieldOfViewNode", container)
        {
            AddInputPort("Input Port");
            InitialiseFields();
            Node = new FieldOfView(container, GUID);
        }

        // Constructor for creating a FieldOfViewNode with an existing GUID
        public FieldOfViewNode(string guid, ParameterContainer container) : base("FieldOfViewNode", guid, container)
        {
            AddInputPort("Input Port");
            InitialiseFields();
            Node = new FieldOfView(container, guid);
        }

        // Method to initialize the UI fields
        private void InitialiseFields()
        {
            // Origin field
            Origin = new ObjectField("Origin");
            Origin.objectType = typeof(Transform);
            Origin.allowSceneObjects = true;
            Origin.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Origin {GUID}", (Transform)evt.newValue));

            // Angle vision field
            AngleVision = new FloatField("Angle Vision");
            AngleVision.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Angle Vision {GUID}", (float)evt.newValue));

            // Radius vision field
            RadiusVision = new FloatField("Radius Vision");
            RadiusVision.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Radius Vision {GUID}", (float)evt.newValue));

            // Player layer field
            PlayerLayer = new LayerMaskField("Target Layer");
            PlayerLayer.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Target Layer {GUID}", (LayerMask)evt.newValue));

            // Obstacle layer field
            ObstacleLayer = new LayerMaskField("Obstacle Layer");
            ObstacleLayer.RegisterValueChangedCallback(evt 
                => Container.SetParameter($"Obstacle Layer {GUID}", (LayerMask)evt.newValue));
            
            // Adding fields to the node
            Add(Origin);
            Add(RadiusVision);
            Add(AngleVision);
            Add(PlayerLayer);
            Add(ObstacleLayer);
        }
    }
}
