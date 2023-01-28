using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicBoard
{
    public class RouteGenerator : MonoBehaviour
    {
        public event Action<List<Node>> OnSetNodesList; 

        #region Exposed_Variables

        [SerializeField] private bool canDestroyRoute = false;

        [Header("Nodes")]
        [SerializeField] private int nodesAmount = 101; 
        [SerializeField] private Transform nodesContainer = null;
        [SerializeField] private Node nodePrefab = null;
        [SerializeField] private int startToCurveId = 14;

        [Header("Nodes Positions")] 
        [SerializeField] private float minHorizontalOffset = 0.5f;
        [SerializeField] private float maxHorizontalOffset = 2f;
        [SerializeField] private float verticalOffset = 1f;
        [SerializeField] private float stepHeight = 2f;
        

        #endregion

        #region Private_Variables

        private List<Node> _nodesList = new List<Node>();

        #endregion

        #region Public_Variables

        public List<Node> NodesList => _nodesList;

        #endregion

        #region Unity_Calls

        private void Awake()
        {
            _nodesList = GetComponentsInChildren<Node>().ToList();
            for (int i = 0; i < _nodesList.Count; i++)
            {
                _nodesList[i].SetNodeId(i);
            }
        }

        private void Start()
        {
            OnSetNodesList?.Invoke(_nodesList);
        }

        private void OnDrawGizmos()
        {
            if (_nodesList.Count < 1) return;
            for (int i = 0; i < _nodesList.Count; i++)
            {
                var start = _nodesList[i].transform.position;
                if (i > 0)
                {
                    var previous = _nodesList[i - 1].transform.position;
                    Debug.DrawLine(previous, start, Color.green);
                }
            }
        }

        #endregion

        #region Private_Methods
        
        [ContextMenu("Fill Nodes")]
        private void FillNodes()
        {
            _nodesList.Clear();
            bool goLeft = false;
            var beforeCurve = 0;
            float nodeLevelHeight = 0f;
            bool skipI = false;

            for (int i = 0; i < nodesAmount + 1; i++)
            {
                var node = Instantiate(nodePrefab, nodesContainer);
                node.gameObject.name = $"Node {i}";
                if (i == 0)
                {
                    node.gameObject.name = $"Starting Node";
                    node.transform.localScale *= 3f;
                }
                else
                {
                    var nodePos = node.transform.localPosition;
                    float randomX = Random.Range(minHorizontalOffset, maxHorizontalOffset);
                    float randomZ = Random.Range(-verticalOffset, verticalOffset);
                    var previousNode = _nodesList[i - 1];
                    if (i == 1)
                    {
                        beforeCurve = i;
                        randomX = 3f;
                    }

                    if (i - beforeCurve == startToCurveId)
                    {
                        beforeCurve = i;
                        goLeft = !goLeft;
                        if (!goLeft) skipI = true;
                    }
                    
                    if (i == beforeCurve + 1 && i != 2)
                    {
                        skipI = false;
                        randomZ = Random.Range(0, verticalOffset) + randomX + stepHeight;
                    }
                    
                    if (i == beforeCurve + 2 && i != 3)
                    {
                        skipI = false;
                        randomZ = _nodesList[i-1].transform.localPosition.z + stepHeight;
                    }
                    
                    if (skipI) randomX *= -1;
                    if(goLeft && i != beforeCurve) randomX *= -1;
                    
                    
                    if (i == beforeCurve + 2 && i != 3)
                    {
                        node.transform.localPosition = new Vector3((nodePos.x * i) + randomX + previousNode.transform.localPosition.x, 0f, randomZ);

                        if (i == beforeCurve + 2) nodeLevelHeight = node.transform.localPosition.z;
                    }
                    else
                    {
                        node.transform.localPosition = new Vector3((nodePos.x * i) + randomX + previousNode.transform.localPosition.x, 0f, randomZ + nodeLevelHeight);
                    }

                }
                
                node.SetNodeId(i);
                _nodesList.Add(node);
            }
        }

        [ContextMenu("Destroy Nodes In Editor")]
        private void DestroyRouteInEditor()
        {
            if(_nodesList.Count < 1 || !canDestroyRoute) return;
            foreach (var node in _nodesList)
            {
                DestroyImmediate(node.gameObject);
            }
            _nodesList.Clear();
        }

        #endregion

        #region Public_Methods

        #endregion


    }
}