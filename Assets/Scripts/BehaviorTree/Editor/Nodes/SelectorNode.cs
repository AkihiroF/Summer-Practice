using System.Collections.Generic;
using BehaviorTree.Editor.Edges;
using Services;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorTree.Editor.Nodes
{
    public class SelectorNode : BehaviorNode
    {
        private List<ANode> _nodes;
        public SelectorNode(ParameterContainer container) : base("ConditionNode",container)
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
            Node = new Selector(GUID, container);
            _nodes = new List<ANode>();
        }
        public SelectorNode(string GUID, ParameterContainer container) : base("ConditionNode",GUID,container)
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
            Node = new Selector(GUID, container);
            _nodes = new List<ANode>();
        }

        protected override void AddOutputPort(string portName, bool multi = true)
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
            if(_nodes.Contains(port.Node))
                return;
            _nodes.Add(port.Node);
            Debug.Log("Add child");
            Container.SetParameter($"ChildNodes {GUID}", (List<ANode>)_nodes);
        }

        private void RemoveChild(BehaviorNode port)
        {
            if(!_nodes.Contains(port.Node))
                return;
            _nodes.Remove(port.Node);
            Container.SetParameter($"ChildNodes {GUID}",(List<ANode>) _nodes);
        }

        private void OnPortConnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.AddChild(childNode);
        }

        private void OnPortDisconnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            parentNode.RemoveChild(childNode);
        }
    }
}