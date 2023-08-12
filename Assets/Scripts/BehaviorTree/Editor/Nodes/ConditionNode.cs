using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class ConditionNode : BehaviorNode
    {
        public TextField NameCondition;
        public ConditionNode() : base("ConditionNode")
        {
            AddInputPort("Input Port");
            AddOutputPort("Output Port");
            if(NameCondition != null)
                return;
            NameCondition = new TextField();
            Add(NameCondition);
        }
        public ConditionNode(string GUID) : base("ConditionNode",GUID)
        {
            AddInputPort("Input Port");
            AddOutputPort("Output Port");
            if(NameCondition != null)
                return;
            NameCondition = new TextField();
            Add(NameCondition);
        }
    }
}