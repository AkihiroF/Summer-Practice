using System.Collections.Generic;

namespace BehaviorTree
{
    public class ParameterContainer
    {
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public void SetParameter(string name, object value)
        {
            if(parameters.ContainsKey(name))
                parameters[name] = value;
            else
                parameters.Add(name,value);
        }

        public T GetParameter<T>(string name)
        {
            if (parameters.TryGetValue(name, out object value))
            {
                return (T)value;
            }
            return default;
        }

        public void RemoveParameter(string name)
        {
            if (parameters.ContainsKey(name))
                parameters.Remove(name);
        }
    }
}