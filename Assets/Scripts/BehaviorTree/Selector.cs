using System;
using System.Collections.Generic;
using BehaviorTree.Actions;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class Selector : ANode
    {
        [SerializeField]private List<NextNode> _childNodes;

        public Selector(string id, ParameterContainer container) : base(container,id)
        {
            _childNodes = new List<NextNode>();
            ID = id;
        }
        public override void Initialise()
        {
            _childNodes = new List<NextNode>();
            var nodes = Container.GetParameter<List<ANode>>($"Selector {ID}");
            foreach (var node in nodes)
            {
                _childNodes.Add(new NextNode()
                {
                    node = node,
                    type = node.GetType().FullName
                });
            }
        }

        public override NodeStatus Tick()
        {
            foreach (var child in _childNodes)
            {
                var status = child.node.Tick();
                if (status == NodeStatus.Success)
                    return NodeStatus.Success;
                if (status == NodeStatus.Running)
                    return NodeStatus.Running;
            }

            return NodeStatus.Failed;
        }
    }
}