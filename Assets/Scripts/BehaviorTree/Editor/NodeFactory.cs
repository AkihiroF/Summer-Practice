using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.Nodes.BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem;
using BehaviorTree.Editor.SaveSystem.Nodes;
using UnityEngine;

namespace BehaviorTree.Editor
{
    public class NodeFactory
    {
        private ParameterContainer _container; // Container for parameters

        // Constructor accepting a ParameterContainer
        public NodeFactory(ParameterContainer container)
        {
            _container = container;
        }

        // Create a node based on the given name and position
        public BehaviorNode CreateNode(string nodeName, Vector2 position)
        {
            
            BehaviorNode node;

            switch (nodeName)
            {
                case "MoveToTargetNode":
                    node = new MoveToTargetNode(_container);
                    break;
                case "SelectorNode":
                    node = new SelectorNode(_container);
                    break;
                case "FieldOfViewNode":
                    node = new FieldOfViewNode(_container);
                    break;
                case "StartingNode":
                    node = new StartingNodeEditor(_container);
                    break;
                default:
                    node = new BehaviorNode(nodeName,_container);
                    Debug.Log("create base node");
                    break;
            }

            node.SetPosition(new Rect(position, Vector2.one * 300));
            return node;
        }
        
        // Create a BehaviorNode based on the given NodeData
        public BehaviorNode CreateBehaviorNode(NodeData data)
        {
            var type = data.type;
            switch (type)
            {
                case "FieldOfViewNode":
                    var fieldOfViewNodeData = JsonUtility.FromJson<FieldOfViewNodeData>(JsonUtility.ToJson(data));
                    return CreateNode(fieldOfViewNodeData);
                case "MoveToTargetNode":
                    var moveToTargetNodeData = JsonUtility.FromJson<MoveToTargetNodeData>(JsonUtility.ToJson(data));
                    return CreateNode(moveToTargetNodeData);
                case "SelectorNode":
                    var selectorData = JsonUtility.FromJson<SelectorData>(JsonUtility.ToJson(data));
                    return CreateNode(selectorData);
                case "StartingNode":
                    var startingData = JsonUtility.FromJson<StartingNodeData>(JsonUtility.ToJson(data));
                    return CreateNode(startingData);
                default:
                    return null;
            }
        }
        

        #region CreatingNode

        // Create a FieldOfViewNode based on the given data
        private BehaviorNode CreateNode(FieldOfViewNodeData data)
        {
            var node = new FieldOfViewNode(data.guid, _container);
            node.Origin.value = data.origin;
            node.AngleVision.value = (data.angleVision);
            node.ObstacleLayer.value =(data.obstacleLayer);
            node.PlayerLayer.value =(data.playerLayer);
            node.RadiusVision.value =(data.radiusVision);

            _container.SetParameter($"Origin {data.guid}", (Transform)data.origin);
            _container.SetParameter($"Angle Vision {data.guid}", (float)data.angleVision);
            _container.SetParameter($"Radius Vision {data.guid}", (float)data.radiusVision);
            _container.SetParameter($"Target Layer {data.guid}", (LayerMask)data.playerLayer);
            _container.SetParameter($"Obstacle Layer {data.guid}", (LayerMask)data.obstacleLayer);
            return node;
        }

        // Create a MoveToTargetNode based on the given data
        private BehaviorNode CreateNode(MoveToTargetNodeData data)
        {
            var node = new MoveToTargetNode(data.guid, _container);
            node.targetPosition.SetValueWithoutNotify(data.targetPosition);
            node.agent.SetValueWithoutNotify(data.agent);
            
            _container.SetParameter($"Agent {data.guid}", data.agent);
            _container.SetParameter($"TargetPosition {data.guid}",data.targetPosition);
            return node;
        }

        // Create a SelectorNode based on the given data
        private BehaviorNode CreateNode(SelectorData data)
        {
            return new SelectorNode(data.guid,data.OutPorts, _container);
        }

        // Create a StartingNode based on the given data
        private BehaviorNode CreateNode(StartingNodeData data)
        {
            return new StartingNodeEditor(data.guid, _container);
        }

        #endregion
    }

}