using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector : ANode
    {
        private List<ANode> childNodes;

        public Selector(List<ANode> childNodes)
        {
            this.childNodes = childNodes;
        }
        public override void Initialise()
        {
            base.Initialise();
        }

        public override NodeStatus Tick()
        {
            foreach (var child in childNodes)
            {
                var status = child.Tick();
                if (status == NodeStatus.Success)
                    return NodeStatus.Success;
                if (status == NodeStatus.Running)
                    return NodeStatus.Running;
            }

            return NodeStatus.Failed;
        }
    }
}