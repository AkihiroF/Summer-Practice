using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.Nodes
{
    public class ActionNode : BehaviorNode
    {
        public ObjectField targetObj;
        public ActionNode() : base("Action Node")
        {
            AddInputPort("Input Port");
            if(targetObj != null)
                return;
            targetObj = new ObjectField();
            targetObj.objectType = typeof(Transform);
            targetObj.allowSceneObjects = true;
            targetObj.RegisterValueChangedCallback(evt =>
            {
                // Обработка выбора GameObject
                Transform selectedGameObject = evt.newValue as Transform;
                RefreshExpandedState();
                RefreshPorts();
            });

            Add(targetObj);

        }
        public ActionNode(string GUID) : base("Action Node", GUID)
        {
            AddInputPort("Input Port");
            targetObj = new ObjectField();
            targetObj.objectType = typeof(Transform);
            targetObj.allowSceneObjects = true;
            targetObj.RegisterValueChangedCallback(evt =>
            {
                // Обработка выбора GameObject
                Transform selectedGameObject = evt.newValue as Transform;
                RefreshExpandedState();
                RefreshPorts();
            });

            Add(targetObj);
        }
    }
}