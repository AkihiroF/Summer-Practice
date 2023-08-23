using BehaviorTree.Editor.Edges;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorTree.Editor.Nodes
{
    public class StartingNodeEditor : BehaviorNode
    {
        private ANode nextNode;
        public StartingNodeEditor(ParameterContainer container) : base("StartingNode", container)
        {
            AddOutputPort("Output", false);
            this.capabilities &= ~Capabilities.Deletable;
            Node = new Actions.StartingNode(container, GUID);
        }

        public StartingNodeEditor(string guid, ParameterContainer container) : base("StartingNode", guid, container)
        {
            AddOutputPort("Output", false);
            this.capabilities &= ~Capabilities.Deletable;
            Node = new Actions.StartingNode(container, guid);
        }

        protected sealed override void AddOutputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, multiPort, typeof(float));
            port.portName = portName;
            port.OnConnect += OnPortConnected;
            port.OnDisconnect += OnPortDisconnected;
        
            outputContainer.Add(port);
        }

        private void AddChild(BehaviorNode port)
        {
            if(nextNode != null)
                return;
            nextNode = port.Node;
            Container.SetParameter($"Start {GUID}", nextNode);
        }

        private void RemoveChild(BehaviorNode port)
        {
            if(nextNode == null)
                return;
            nextNode = null;
            Container.RemoveParameter($"Start {GUID}");
        }
        private void OnPortConnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (StartingNodeEditor)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.AddChild(childNode);
        }

        private void OnPortDisconnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (StartingNodeEditor)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.RemoveChild(childNode);
        }
    }
}