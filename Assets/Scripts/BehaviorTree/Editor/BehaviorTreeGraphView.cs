using System.Collections.Generic;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    public class BehaviorTreeGraphView : GraphView
    {
        public PopupField<string> NameTree; // TextField to input the name of the tree
        private NodeSaver _saver; // Responsible for saving the nodes
        private ParameterContainer _container; // Container for parameters
        private NodeFactory _nodeFactory; // Factory for creating nodes

        public BehaviorTreeGraphView()
        {
            _saver = new NodeSaver();
            _container = new ParameterContainer();
            _nodeFactory = new NodeFactory(_container); // Initialize node factory
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            GridBackground grid = new GridBackground();
            Add(grid);
            grid.SendToBack();
            grid.StretchToParentSize();
            this.style.flexGrow = 1;
        }

        // Context menu for creating nodes
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = evt.mousePosition;
            
            evt.menu.AppendAction("Create Action Node/Move To Target", (action) => CreateNode("MoveToTargetNode", mousePosition));
            evt.menu.AppendAction("Create Condition Node/Field Of View", (action) => CreateNode("FieldOfViewNode", mousePosition));
            evt.menu.AppendAction("Create Selector Node", (action) => CreateNode("SelectorNode", mousePosition));
            evt.StopPropagation();
        }

        // Create a node with the given name and position
        private void CreateNode(string nodeName, Vector2 position)
        {
            // Check if a starting node exists and create one if not
            if (!StartingExist())
            {
                AddElement(_nodeFactory.CreateNode("StartingNode", position + Vector2.down * 200));
            }
            
            // Create the node using the factory
            BehaviorNode node = _nodeFactory.CreateNode(nodeName, position);
            AddElement(node);
        }
        
        // Check if a starting node exists in the graph
        private bool StartingExist()
        {
            foreach (var node in nodes)
            {
                if (node is StartingNodeEditor)
                {
                    return true; 
                }
            }
            return false;
        }

        // Get compatible ports for connections
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

        // Save the current state of the graph
        public void SaveData()
        {
            _saver.SaveData(NameTree.value,nodes,edges);
        }

        // Clear the graph
        public void ClearGraph()
        {
            foreach (var node in nodes.ToList())
            {
                RemoveElement(node);
            }
            
            foreach (var edge in edges.ToList())
            {
                RemoveElement(edge);
            }
        }

        private void CreateNode(NodeData data)
        {
            BehaviorNode node = _nodeFactory.CreateBehaviorNode(data);
            if(node == null)
                return;
            node.SetPosition(new Rect(data.position, Vector2.one * 150));
            AddElement(node);
        }


        // Load data from saved state
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
        
        // Find a node by its GUID
        private BehaviorNode FindNodeByGuid(string guid)
        {
            return nodes.ToList().Find(node => (node as BehaviorNode).GUID == guid) as BehaviorNode;
        }
    }
}