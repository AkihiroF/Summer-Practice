using System;

namespace BehaviorTree.Editor.SaveSystem
{
    [Serializable]
    public struct EdgeData
    {
        public string fromNodeGuid; // GUID узла, откуда идет связь
        public string toNodeGuid;   // GUID узла, куда идет связь
        public string fromPortName; // Имя порта, откуда идет связь
        public string toPortName;   // Имя порта, куда идет связь
    }
}