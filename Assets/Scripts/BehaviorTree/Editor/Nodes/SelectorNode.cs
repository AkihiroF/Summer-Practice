namespace BehaviorTree.Editor.Nodes
{
    public class SelectorNode : BehaviorNode
    {
        public SelectorNode() : base("ConditionNode")
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
        }
        public SelectorNode(string GUID) : base("ConditionNode",GUID)
        {
            AddInputPort("Input Port");
            AddOutputPort("Child");
        }
    }
}