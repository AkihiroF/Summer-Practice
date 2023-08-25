using System.Collections.Generic;

namespace BehaviorTree
{
    public class ParameterContainer
    {
        // Dictionary to store parameters by name
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        // Sets a parameter by name, adding or updating as necessary
        public void SetParameter(string name, object value)
        {
            parameters[name] = value;
        }

        // Retrieves a parameter by name, returning the default value if not found
        public T GetParameter<T>(string name)
        {
            if (parameters.TryGetValue(name, out object value))
            {
                return (T)value;
            }
            return default;
        }

        // Removes a parameter by name if it exists
        public void RemoveParameter(string name)
        {
            parameters.Remove(name);
        }
    }
}