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

        public ANode Node
        {
            get;
            protected set;
        }

        public BehaviorNode(string nodeName,ParameterContainer container)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = Guid.NewGuid().ToString();
            Container = container;

            // Здесь можно добавить общие элементы для всех узлов, например, порты или поля ввода
        }
        public BehaviorNode(string nodeName, string guid,ParameterContainer container)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = guid;
            Container = container;
            // Здесь можно добавить общие элементы для всех узлов, например, порты или поля ввода
        }
        protected virtual void AddInputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Vertical, Direction.Input, multiPort, typeof(float));
            port.portName = portName;
            inputContainer.Add(port);
        }

        protected virtual void AddOutputPort(string portName, bool multi = true)
        {
            var multiPort = multi ? Port.Capacity.Multi : Port.Capacity.Single;
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, multiPort, typeof(float));
            port.portName = portName;
            outputContainer.Add(port);
        }
        public MyPort GetPort(string portName, Direction direction)
        {
            // Получите список всех портов в зависимости от направления (вход или выход)
            IEnumerable<MyPort> ports = direction == Direction.Input ? inputContainer.Children().OfType<MyPort>() : outputContainer.Children().OfType<MyPort>();

            // Найдите порт с заданным именем
            return ports.FirstOrDefault(port => port.portName == portName);
        }

        protected new MyPort InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            return MyPort.Create<Edge>(orientation,direction,capacity,type);
        }
    }
}