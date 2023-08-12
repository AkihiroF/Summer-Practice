using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace BehaviorTree.Editor
{
    public class BehaviorNode : Node
    {
        public string NodeName { get; private set; }
        public string GUID { get; private set; }

        public BehaviorNode(string nodeName)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = Guid.NewGuid().ToString();

            // Здесь можно добавить общие элементы для всех узлов, например, порты или поля ввода
        }
        public BehaviorNode(string nodeName, string guid)
        {
            NodeName = nodeName;
            title = nodeName;
            GUID = guid;

            // Здесь можно добавить общие элементы для всех узлов, например, порты или поля ввода
        }
        public void AddInputPort(string portName)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            inputContainer.Add(port);
        }

        public void AddOutputPort(string portName)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            port.portName = portName;
            outputContainer.Add(port);
        }
        public Port GetPort(string portName, Direction direction)
        {
            // Получите список всех портов в зависимости от направления (вход или выход)
            IEnumerable<Port> ports = direction == Direction.Input ? inputContainer.Children().OfType<Port>() : outputContainer.Children().OfType<Port>();

            // Найдите порт с заданным именем
            return ports.FirstOrDefault(port => port.portName == portName);
        }

    }
}