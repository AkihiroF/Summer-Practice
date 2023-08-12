
namespace BehaviorTree.Editor.Nodes
{
    public class ActionNode : BehaviorNode
    {
        
        public ActionNode() : base("Action Node")
        {
            AddInputPort("Input Port");
            
        }
        public ActionNode(string GUID) : base("Action Node", GUID)
        {
            AddInputPort("Input Port");
            
        }

        private void CheckState()
        {
            
        }
    }
}