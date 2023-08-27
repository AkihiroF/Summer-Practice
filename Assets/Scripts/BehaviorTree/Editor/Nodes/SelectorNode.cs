using System.Collections.Generic;
using System.Linq;
using BehaviorTree.Editor.Edges;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    namespace BehaviorTree.Editor.Nodes
{
    public class SelectorNode : BehaviorNode
    {
        private Dictionary<Port, ANode> _nodes; // List to store child nodes

        private List<Port> activePorts;

        public List<Port> GetActivePorts => activePorts;

        private int countOutPorts;

        // Constructor for creating a new SelectorNode with a new GUID
        public SelectorNode(ParameterContainer container) : base("SelectorNode", container)
        {
            Node = new Selector(GUID, container);
            Construct();
            AddPort();
        }

        // Constructor for creating a SelectorNode with an existing GUID
        public SelectorNode(string GUID, List<string> ports, ParameterContainer container) : base("SelectorNode", GUID, container)
        {
            Node = new Selector(GUID, container);
            foreach (var namePort in ports)
            {
                AddOutputPort(namePort, false);
            }

            countOutPorts = ports.Count+1;
            Construct();
        }
        
        private void Construct()
        {
            Button addButton = new Button(() => { AddPort(); });
            activePorts = new List<Port>();
            addButton.text = "Add Port";
            titleContainer.Add(addButton);
            AddInputPort("Input Port");
            _nodes = new Dictionary<Port, ANode>();
        }

        private void AddPort()
        {
            AddOutputPort($"Child {countOutPorts++}", false);
        }

        // Method to add an output port
        protected override void AddOutputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, multiPort, typeof(float));

            Button deleteButton = new Button(() => DeletePort(port));
            deleteButton.text = "Delete";
            deleteButton.style.width = 50;
            deleteButton.style.height = 30;
            
            port.Add(deleteButton);
            port.portName = portName;
            port.OnConnect += OnPortConnected;
            port.OnDisconnect += OnPortDisconnected;
            port.OnDisconnectAll += OnAllPortDisconnected;

            port.style.width = 180;
            port.style.height = 50;
            
            outputContainer.Add(port);
        }

        // Method to handle all port disconnections
        private void OnAllPortDisconnected()
        {
            _nodes = new Dictionary<Port, ANode>();
            Container.RemoveParameter($"ChildNodes {GUID}");
        }

        // Method to add a child node
        private void AddChild(MyPort outPort,BehaviorNode node)
        {
            _nodes[outPort] = node.Node;
            SaveChildesToContainer();
        }

        // Method to remove a child node
        private void RemoveChild(MyPort outPort)
        {
            _nodes.Remove(outPort);
            SaveChildesToContainer();
        }

        // Method to handle port connection
        private void OnPortConnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            var childNode = (BehaviorNode)inputPort.node;
            AddActivePort(outputPort);
            parentNode.AddChild(outputPort,childNode);
        }

        // Method to handle port disconnection
        private void OnPortDisconnected(MyPort outputPort, Port inputPort)
        {
            var parentNode = (SelectorNode)outputPort.node;
            RemoveActivePort(outputPort);
            parentNode.RemoveChild(outputPort);
        }

        private void DeletePort(Port port)
        {
            var connections = new List<Edge>(port.connections);

            // Шаг 2: Удалить рёбра из GraphView
            foreach (var edge in connections)
            {
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                edge.RemoveFromHierarchy();  // Удаление ребра из GraphView
            }

            // Шаг 3: Удалить порт из ноды
            port.RemoveFromHierarchy();
            countOutPorts--;
            for (int i = 0; i < countOutPorts; i++)
            {
                //outputContainer[i] = $"Child {i+1}";
                if (outputContainer[i] is MyPort myPort)
                    myPort.portName = $"Child {i + 1}";
            }
        }

        private void AddActivePort(MyPort port)
        {
            activePorts.Add(port);
        }

        private void RemoveActivePort(MyPort port)
        {
            activePorts.Remove(port);
        }

        private void SaveChildesToContainer()
        {
            var listActiveNodes = new List<ANode>();
            var sortedPorts = activePorts.OrderBy(port => 
            {
                var parts = port.portName.Split(' ');
                if (parts.Length > 1 && int.TryParse(parts[1], out int number))
                {
                    return number;
                }
                return 0;
            }).ToList();
            foreach (var port in sortedPorts)
            {
                listActiveNodes.Add(_nodes[port]);
            }
            Container.SetParameter($"ChildNodes {GUID}", (List<ANode>)listActiveNodes);
        }
    }
}

}