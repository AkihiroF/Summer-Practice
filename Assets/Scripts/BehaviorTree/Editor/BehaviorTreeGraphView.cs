using System.Collections.Generic;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem;
using BehaviorTree.Editor.SaveSystem.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeGraphView : GraphView
    {
        private NodeSaver _saver;
        public BehaviorTreeGraphView()
        {
            _saver = new NodeSaver();
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            this.style.flexGrow = 1;
        }

        public void CreateNode(string nodeName, Vector2 position)
        {
            BehaviorNode node;

            switch (nodeName)
            {
                case "Action Node":
                    node = new ActionNode();
                    break;
                case "Condition Node":
                    node = new ConditionNode();
                    break;
                default:
                    node = new BehaviorNode(nodeName);
                    Debug.Log("create base node");
                    break;
            }

            node.SetPosition(new Rect(position, Vector2.one * 150));
            AddElement(node);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }


        public void SaveData()
        {
            _saver.SaveData(nodes,edges);
        }

        public void ClearGraph()
        {
            // Удалить все узлы
            foreach (var node in nodes.ToList())
            {
                RemoveElement(node);
            }

            // Удалить все связи
            foreach (var edge in edges.ToList())
            {
                RemoveElement(edge);
            }
        }

        private void CreateNode(NodeData data)
        {
            BehaviorNode node;
            switch (data.type)
            {
                case "Action":
                    var currentActionData = (ActionNodeData) data;
                    node = new ActionNode(currentActionData.guid);
                    var currNode = (ActionNode)node;
                    currNode.targetObj.SetValueWithoutNotify(GameObject.Find(currentActionData.target).transform);
                    break;
                case "Condition":
                    var currentConditionData = JsonUtility.FromJson<ConditionNodeData>(JsonUtility.ToJson(data));
                    var currentConditionNode = new ConditionNode(currentConditionData.guid);
                    currentConditionNode.NameCondition.value = currentConditionData.nameCondition;
                    node = currentConditionNode;
                    break;
                default:
                    return;
            }
            node.SetPosition(new Rect(data.position, Vector2.one * 150));
            AddElement(node);
        }


        public void LoadData()
        {
            ClearGraph();
            var serializedNodes = _saver.LoadNodes();
            var serializeEdges = _saver.LoadEdge();

            foreach (var data in serializedNodes)
            {
                CreateNode(data);
            }
            foreach (var edgeData in serializeEdges)
            {
                var fromNode = FindNodeByGuid(edgeData.fromNodeGuid);
                var toNode = FindNodeByGuid(edgeData.toNodeGuid);
                var fromPort = fromNode.GetPort(edgeData.fromPortName, Direction.Output);
                var toPort = toNode.GetPort(edgeData.toPortName, Direction.Input);

                var edge = fromPort.ConnectTo(toPort);
                AddElement(edge);
            }
        }
        
        private BehaviorNode FindNodeByGuid(string guid)
        {
            return nodes.ToList().Find(node => (node as BehaviorNode).GUID == guid) as BehaviorNode;
        }
    }
}