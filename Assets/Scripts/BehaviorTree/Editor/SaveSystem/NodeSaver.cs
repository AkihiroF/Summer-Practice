using System;
using System.Collections.Generic;
using System.IO;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.SaveSystem
{
    public class NodeSaver
    {
        public const string PathSaveTrees = "Assets/Behavior Trees";
        private NodeListData _listData;

        public NodeSaver()
        {
            if (!Directory.Exists(PathSaveTrees))
            {
                Directory.CreateDirectory(PathSaveTrees);
                AssetDatabase.Refresh();
            }

            _listData = new NodeListData();
        }

        public List<NodeData> LoadNodes()
        {
            string json = File.ReadAllText(PathSaveTrees +  "/FirstTree");
            List<string> serializedNodes = JsonUtility.FromJson<NodeListData>(json).nodes;
            List<string> serializedNodesType = JsonUtility.FromJson<NodeListData>(json).nodesType;

            List<NodeData> outData = new List<NodeData>();

            for (int i = 0; i < serializedNodes.Count; i++)
            {
                Type type = Type.GetType(serializedNodesType[i]);
                var objData=(NodeData)JsonUtility.FromJson(serializedNodes[i], type);
                outData.Add(objData);
            }

            return outData;
        }

        public List<EdgeData> LoadEdge()
        {
            string json = File.ReadAllText(PathSaveTrees +  "/FirstTree");
            return JsonUtility.FromJson<NodeListData>(json).edges;
        }

        public void SaveData(UQueryState<Node> nodes, UQueryState<Edge> edges)
        {
            GetSerializedEdges(edges);
            GetSerializedNodes(nodes);
            var jsonData = JsonUtility.ToJson(_listData, true);
            File.WriteAllText(PathSaveTrees + "/FirstTree",jsonData);
            AssetDatabase.Refresh();
        }
        private void GetSerializedNodes(UQueryState<Node> nodes)
        {
            List<string> serializedNodes = new List<string>();
            List<string> serializedNodesType = new List<string>();

            foreach (var node in nodes)
            {
                if (node is ActionNode actionNode)
                {
                    var targetTrans = (Transform)actionNode.targetObj.value;
                    var data =new ActionNodeData
                    {
                        guid = actionNode.GUID,
                        type = "Action",
                        position = actionNode.GetPosition().position,
                        target = targetTrans.name
                    };
                        serializedNodes.Add(JsonUtility.ToJson(data,false));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is ConditionNode conditionNode)
                {
                    var data = new ConditionNodeData
                    {
                        guid = conditionNode.GUID,
                        type = "Condition",
                        position = conditionNode.GetPosition().position,
                        nameCondition = conditionNode.NameCondition.value
                    };
                    serializedNodes.Add(JsonUtility.ToJson(data,false));
                    serializedNodesType.Add(data.GetType().FullName);
                }
            }

            _listData.nodes = serializedNodes;
            _listData.nodesType = serializedNodesType;
        }
        
        private void GetSerializedEdges(UQueryState<Edge> edges)
        {
            List<EdgeData> serializedEdges = new List<EdgeData>();

            foreach (var edge in edges)
            {
                var fromNode = edge.output.node as BehaviorNode;
                var toNode = edge.input.node as BehaviorNode;

                if (fromNode != null 
                    || toNode != null)
                    serializedEdges.Add(new EdgeData
                        {
                            fromNodeGuid = fromNode.GUID,
                            toNodeGuid = toNode.GUID,
                            fromPortName = edge.output.portName,
                            toPortName = edge.input.portName
                            
                        });
            }

            _listData.edges = serializedEdges;
        }
    }
}