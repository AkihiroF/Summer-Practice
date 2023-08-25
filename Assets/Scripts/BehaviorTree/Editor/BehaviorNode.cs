using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.Editor.Edges;
using UnityEditor.Experimental.GraphView;

namespace BehaviorTree.Editor
{
    public class BehaviorNode : Node
    {
        public string NodeName { get; private set; }
        public string GUID { get; private set; }
        protected ParameterContainer Container;

        public ANode Node { get; protected set; }

        // Constructor to initialize the BehaviorNode with a name and container
        public BehaviorNode(string nodeName, ParameterContainer container)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = Guid.NewGuid().ToString();
            Container = container;
        }

        // Overloaded constructor to initialize the BehaviorNode with a name, GUID, and container
        public BehaviorNode(string nodeName, string guid, ParameterContainer container)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = guid;
            Container = container;
        }

        // Method to add an input port with a given name and optional multiplicity
        protected virtual void AddInputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Vertical, Direction.Input, multiPort, typeof(float));
            port.portName = portName;
            inputContainer.Add(port);
        }

        // Method to add an output port with a given name and optional multiplicity
        protected virtual void AddOutputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, multiPort, typeof(float));
            port.portName = portName;
            outputContainer.Add(port);
        }

        // Method to get a port by name and direction (Input/Output)
        public MyPort GetPort(string portName, Direction direction)
        {
            IEnumerable<MyPort> ports = direction == Direction.Input ? inputContainer.Children().OfType<MyPort>() : outputContainer.Children().OfType<MyPort>();
            return ports.FirstOrDefault(port => port.portName == portName);
        }

        // Method to instantiate a custom port (MyPort) with given parameters
        protected new MyPort InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            return MyPort.Create<Edge>(orientation, direction, capacity, type);
        }
    }
}
