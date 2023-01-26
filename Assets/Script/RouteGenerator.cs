using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public class RouteGenerator : MonoBehaviour
    {
        #region Exposed_Variables

        [Header("Nodes")]
        [SerializeField] private int nodesAmount = 100;
        [SerializeField] private Transform nodesContainer = null;
        [SerializeField] private Node nodePrefab = null;

        #endregion

        #region Private_Variables

        private Transform[] _nodes;
        private List<Node> _nodesList = new List<Node>();

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Start()
        {
            FillNodes();
        }

        #endregion

        #region Private_Methods

        private void FillNodes()
        {
            _nodesList.Clear();

            for (int i = 0; i < nodesAmount; i++)
            {
                var node = Instantiate(nodePrefab, nodesContainer);
                node.gameObject.name = $"Node {i + 1}";
                node.SetNodeId(i + 1);
            }
        }

        #endregion

        #region Public_Methods

        #endregion


    }
}