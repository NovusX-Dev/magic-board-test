using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MagicBoard
{
    public class Node : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private TextMeshProUGUI numberText = null;
        [SerializeField] private Node connectedNode;

        #endregion

        #region Private_Variables

        private int _nodeId;

        #endregion

        #region Public_Variables

        public int NodeId => _nodeId;

        #endregion

        #region Unity_Calls

        private void OnDrawGizmos()
        {
            if (connectedNode == null) return;
            var lineColor = connectedNode.NodeId > _nodeId ? Color.blue : Color.red;
            Debug.DrawLine(connectedNode.transform.position, transform.position, lineColor);
        }

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods

        public void SetNodeId(int id)
        {
            _nodeId = id;
            if (numberText.text != string.Empty) return;
            numberText.text = id.ToString();
            if (id == 0)
                numberText.text = $"Start";
        }

        public bool HasConnectedNode()
        {
            return connectedNode;
        }

        public void GetConnectedNodeId(ref int id)
        {
            if (connectedNode == null) return;
            id =  connectedNode.NodeId;
        }
        

        #endregion


    }
}