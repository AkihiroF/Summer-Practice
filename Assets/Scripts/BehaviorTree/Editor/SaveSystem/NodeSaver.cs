using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BehaviorTree.Editor.Edges;
using BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.Nodes.BehaviorTree.Editor.Nodes;
using BehaviorTree.Editor.SaveSystem.Nodes;
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
            
            GetSerializedEdges(edges);
            GetSerializedNodes(nodes);
            
            SaveToSo(nameTree,nodes);
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
                    if(actionNode.Node is ActionNode actionNodeNode)
                        actionNodeNode.SetParameters();
                    serializedNodes.Add(ConvertToJson(actionNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is FieldOfViewNode conditionNode)
                {
                    if(conditionNode.Node is ActionNode actionNodeNode)
                        actionNodeNode.SetParameters();
                    serializedNodes.Add(ConvertToJson(conditionNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is SelectorNode selectorNode)
                {
                    if(selectorNode.Node is BranchNode branchNode)
                        branchNode.SerializeChildren();
                    serializedNodes.Add(ConvertToJson(selectorNode, out var data));
                    serializedNodesType.Add(data.GetType().FullName);
                }
                else if (node is StartingNodeEditor startingNode)
                {
                    if(startingNode.Node is BranchNode branchNode)
                        branchNode.SerializeChildren();
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
            Dictionary<string, string> nextNodes = new Dictionary<string, string>();
            string path = PathSaveTrees + nameTree + $"/{nameTree}So.asset";
            
            BehaviourTreeSo behaviourTree = AssetDatabase.LoadAssetAtPath<BehaviourTreeSo>(path);
            
            BehaviorNode startingNode = SearchStartingNode(nodes);
            var endingNodes = GetEndingNodes(nodes);
            foreach (BehaviorNode endingNode in endingNodes)
            {
                TraverseNode(endingNode);
            }

            foreach (BehaviorNode node in nodes)
            {
                if (node.Node is BranchNode branchNode)
                {
                    nextNodes.Add(branchNode.ID, JsonUtility.ToJson(branchNode.NextNodes));
                }
            }
            
            if (behaviourTree == null)
            {
                behaviourTree = ScriptableObject.CreateInstance<BehaviourTreeSo>();
                AssetDatabase.CreateAsset(behaviourTree, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            behaviourTree.StartingNodeData = JsonUtility.ToJson(startingNode.Node);
            behaviourTree.TypeNode = startingNode.Node.GetType().FullName;

            behaviourTree.keys = nextNodes.Keys.ToList();
            behaviourTree.nodes = nextNodes.Values.ToList();
            
            EditorUtility.SetDirty(behaviourTree);
            AssetDatabase.SaveAssets();
        }
        
        private void TraverseNode(BehaviorNode node)
        {
            if(node.Node is BranchNode branchNode)
                branchNode.SerializeChildren();

            List<BehaviorNode> parentNodes = GetParentNodes(node);

            foreach (BehaviorNode parentNode in parentNodes)
            {
                TraverseNode(parentNode);
            }
        }

        private List<BehaviorNode> GetParentNodes(BehaviorNode node)
        {
            List<BehaviorNode> parentNodes = new List<BehaviorNode>();

            foreach (var visualElement in node.inputContainer.Children())
            {
                var ports = (MyPort)visualElement;
                foreach (var edge in ports.connections)
                {
                    parentNodes.Add(edge.output.node as BehaviorNode);
                }
            }

            return parentNodes;
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

        private List<BehaviorNode> GetEndingNodes(UQueryState<Node> nodes)
        {
            List<BehaviorNode> endingNodes = new List<BehaviorNode>();
            foreach (var node in nodes)
            {
                if(node.outputContainer.childCount == 0)
                    endingNodes.Add(node as BehaviorNode);
            }

            return endingNodes;
        }
    }
}