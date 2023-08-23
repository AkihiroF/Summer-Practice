using System;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviourTreeComponent : MonoBehaviour
    {
        [SerializeField] private BehaviourTreeSo behaviourTreeSo;
        private ANode startNode;

        private void Awake()
        {
            behaviourTreeSo.Convert();
            startNode = behaviourTreeSo.NodesTree[0];
        }

        private void FixedUpdate()
        {
            Debug.Log(startNode.Tick());
        }
    }
}