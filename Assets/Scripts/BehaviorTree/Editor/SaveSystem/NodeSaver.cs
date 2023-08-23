using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BehaviorTree.Actions;
using BehaviorTree.Editor.Edges;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem.Nodes;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor.SaveSystem
{
    public class NodeSaver
    {
        public const string PathSaveTrees = "Assets/Behavior Trees/";
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

        public List<NodeData> LoadNodes(string nameTree)
        {
            string json = File.ReadAllText(PathSaveTrees  + nameTree + "/" + "EditorData");
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

        public List<EdgeData> LoadEdge(string nameTree)
        {
            string json = File.ReadAllText(PathSaveTrees +  nameTree + "/" + "EditorData");
            return JsonUtility.FromJson<NodeListData>(json).edges;
        }

        public void SaveData(string nameTree,UQueryState<Node> nodes, UQueryState<Edge> edges)
        {
            var path = PathSaveTrees + nameTree + "/";
            
            CreateDirectory(path);
            
            SaveToSo(nameTree,nodes);
            
            GetSerializedEdges(edges);
            GetSerializedNodes(nodes);
            var jsonData = JsonUtility.ToJson(_listData, true);
            File.WriteAllText(path + "EditorData",jsonData);
            AssetDatabase.Refresh();
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }
        private void GetSerializedNodes(UQueryState<Node> nodes)
        {
            List<string> serializedNodes = new List<string>();
            List<string> serializedNodesType = new List<string>();

            foreach (var node in nodes)
            {
                if (node is MoveToTargetNode actionNode)
                {
                    serializedNodes.Add(ConvertToJson(actionNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is FieldOfViewNode conditionNode)
                {
                    serializedNodes.Add(ConvertToJson(conditionNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is SelectorNode selectorNode)
                {
                    serializedNodes.Add(ConvertToJson(selectorNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is StartingNodeEditor startingNode)
                {
                    serializedNodes.Add(ConvertToJson(startingNode, out var data));
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

        #region ConvertToJSON

        private string ConvertToJson(FieldOfViewNode node, out FieldOfViewNodeData data)
        {
            data = new FieldOfViewNodeData
            {
                guid = node.GUID,
                type = "FieldOfViewNode",
                position = node.GetPosition().position,
                        
                origin = (Transform)node.Origin.value,
                angleVision = node.AngleVision.value,
                radiusVision = node.RadiusVision.value,
                playerLayer = node.PlayerLayer.value,
                obstacleLayer = node.ObstacleLayer.value
            };
            return JsonUtility.ToJson(data);
        }

        private string ConvertToJson(MoveToTargetNode node, out MoveToTargetNodeData data)
        {
            data = new MoveToTargetNodeData()
            {
                guid = node.GUID,
                type = "MoveToTargetNode",
                position = node.GetPosition().position,
                agent = (NavMeshAgent)node.agent.value,
                targetPosition = (Transform)node.targetPosition.value,
            };
            return JsonUtility.ToJson(data);
        }

        private string ConvertToJson(SelectorNode node, out SelectorData data)
        {
            data = new SelectorData()
            {
                guid = node.GUID,
                position = node.GetPosition().position,
                type = "SelectorNode",
            };
            return JsonUtility.ToJson(data);
        }

        private string ConvertToJson(StartingNodeEditor nodeEditor, out StartingNodeData data)
        {
            data = new StartingNodeData()
            {
                guid = nodeEditor.GUID,
                position = nodeEditor.GetPosition().position,
                type = "StartingNode",
            };
            return JsonUtility.ToJson(data);
        }

        #endregion

        private void SaveToSo(string nameTree,UQueryState<Node> nodes)
        {
            List<string> nodesTree = new List<string>();
            List<string> nodesTreeTypes = new List<string>();
            
            string path = PathSaveTrees + nameTree + $"/{nameTree}So.asset";
            
            BehaviourTreeSo behaviourTree = AssetDatabase.LoadAssetAtPath<BehaviourTreeSo>(path);
            
            BehaviorNode startingNode = SearchStartingNode(nodes);
            
            startingNode.Node.Initialise();
            
            nodesTree.Add(JsonUtility.ToJson(startingNode.Node));
            nodesTreeTypes.Add(startingNode.Node.GetType().FullName);
            foreach (var node1 in nodes)
            {
                var node = (BehaviorNode)node1;
                if(node is StartingNodeEditor)
                    continue;
                var currentNode = node.Node;
                currentNode.Initialise();
                nodesTree.Add(JsonUtility.ToJson(currentNode));
                nodesTreeTypes.Add(currentNode.GetType().FullName);
            }

            if (behaviourTree == null)
            {
                behaviourTree = ScriptableObject.CreateInstance<BehaviourTreeSo>();
                AssetDatabase.CreateAsset(behaviourTree, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            behaviourTree.Nodes = nodesTree;
            behaviourTree.TypesNode = nodesTreeTypes;
            EditorUtility.SetDirty(behaviourTree);
            AssetDatabase.SaveAssets();
        }

        private StartingNodeEditor SearchStartingNode(UQueryState<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is StartingNodeEditor startingNode)
                    return startingNode;
            }

            return null;
        }
    }
}