namespace BehaviorTree
{
    public class ActionNode : ANode
    {
        protected ActionNode(ParameterContainer container, string id) : base(container, id)
        {
        }
        
        public virtual void SetParameters(){}
    }
}