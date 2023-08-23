using System.Collections.Generic;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem;
using Services;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeGraphView : GraphView
    {
        public TextField NameTree;
        private NodeSaver _saver;
        private ParameterContainer _container;
        private CreatorNode _creator;
        public BehaviorTreeGraphView()
        {
            _saver = new NodeSaver();
            _container = new ParameterContainer();
            _creator = new CreatorNode(_container);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            this.style.flexGrow = 1;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = evt.mousePosition;
            
            evt.menu.AppendAction("Create Action Node/Move To Target", (action) => CreateNode("MoveToTargetNode", mousePosition));
            evt.menu.AppendAction("Create Condition Node/Field Of View", (action) => CreateNode("FieldOfViewNode", mousePosition));
            evt.menu.AppendAction("Create Selector Node", (action) => CreateNode("SelectorNode", mousePosition));
            evt.StopPropagation();
        }

        private void CreateNode(string nodeName, Vector2 position)
        {
            if (!StartingExist())
            {
                CreateStartNode(position + Vector2.up * 200);
            }
            
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
            AddElement(node);
        }

        private bool StartingExist()
        {
            foreach (var node in nodes)
            {
                if (node is StartingNodeEditor)
                {
                    return true; // Нода с заданным именем найдена
                }
            }
            return false;
        }

        private void CreateStartNode(Vector2 position)
        {
            var node = new StartingNodeEditor(_container);
            node.SetPosition(new Rect(position, Vector2.one * 300));
            AddElement(node);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port as Port);
                }
            });

            return compatiblePorts;
        }


        public void SaveData()
        {
            _saver.SaveData(NameTree.value,nodes,edges);
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
            BehaviorNode node = _creator.CreateBehaviorNode(data);
            if(node == null)
                return;
            node.SetPosition(new Rect(data.position, Vector2.one * 150));
            AddElement(node);
        }


        public void LoadData()
        {
            ClearGraph();
            var serializedNodes = _saver.LoadNodes(NameTree.value);
            var serializeEdges = _saver.LoadEdge(NameTree.value);

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