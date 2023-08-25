using System.Collections.Generic;
using BehaviorTree.Editor.Edges;
using UnityEditor.Experimental.GraphView;

namespace BehaviorTree.Editor.Nodes
{
    namespace BehaviorTree.Editor.Nodes
{
    public class SelectorNode : BehaviorNode
    {
        private List<ANode> _nodes; // List to store child nodes

        // Constructor for creating a new SelectorNode with a new GUID
        public SelectorNode(ParameterContainer container) : base("SelectorNode", container)
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
            Node = new Selector(GUID, container);
            _nodes = new List<ANode>();
        }

        // Constructor for creating a SelectorNode with an existing GUID
        public SelectorNode(string GUID, ParameterContainer container) : base("SelectorNode", GUID, container)
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
            Node = new Selector(GUID, container);
            _nodes = new List<ANode>();
        }

        // Method to add an output port
        protected override void AddOutputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, multiPort, typeof(float));
            port.portName = portName;
            port.OnConnect += OnPortConnected;
            port.OnDisconnect += OnPortDisconnected;
            port.OnDisconnectAll += OnAllPortDisconnected;

            outputContainer.Add(port);
        }

        // Method to handle all port disconnections
        private void OnAllPortDisconnected()
        {
            _nodes = new List<ANode>();
        }

        // Method to add a child node
        private void AddChild(BehaviorNode port)
        {
            if (_nodes.Contains(port.Node))
                return;
            _nodes.Add(port.Node);
            Container.SetParameter($"ChildNodes {GUID}", (List<ANode>)_nodes);
        }

        // Method to remove a child node
        private void RemoveChild(BehaviorNode port)
        {
            if (!_nodes.Contains(port.Node))
                return;
            _nodes.Remove(port.Node);
            Container.SetParameter($"ChildNodes {GUID}", (List<ANode>)_nodes);
        }

        // Method to handle port connection
        private void OnPortConnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.AddChild(childNode);
        }

        // Method to handle port disconnection
        private void OnPortDisconnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.RemoveChild(childNode);
        }
    }
}

}