using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem;
using BehaviorTree.Editor.SaveSystem.Nodes;
using UnityEngine;

namespace BehaviorTree.Editor
{
    public class CreatorNode
    {
        private ParameterContainer _container;

        public CreatorNode(ParameterContainer container)
        {
            _container = container;
        }

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

        private BehaviorNode CreateNode(MoveToTargetNodeData data)
        {
            var node = new MoveToTargetNode(data.guid, _container);
            node.targetPosition.SetValueWithoutNotify(data.targetPosition);
            node.agent.SetValueWithoutNotify(data.agent);
            
            _container.SetParameter($"Agent {data.guid}", data.agent);
            _container.SetParameter($"TargetPosition {data.guid}",data.targetPosition);
            return node;
        }

        private BehaviorNode CreateNode(SelectorData data)
        {
            return new SelectorNode(data.guid, _container);
        }

        private BehaviorNode CreateNode(StartingNodeData data)
        {
            return new StartingNodeEditor(data.guid, _container);
        }
    }
}