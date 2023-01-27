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

        #endregion

        #region Private_Variables

        private int _nodeId;
        private Node _connectedNode;

        #endregion

        #region Public_Variables

        public int NodeId => _nodeId;

        #endregion

        #region Unity_Calls

        private void OnDrawGizmos()
        {
            if (_connectedNode == null) return;
            var lineColor = _connectedNode.NodeId > _nodeId ? Color.blue : Color.red;
            Debug.DrawLine(_connectedNode.transform.position, transform.position, lineColor);
        }

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods

        public void SetNodeId(int id)
        {
            _nodeId = id;
            numberText.text = id.ToString();
            if (id == 0)
                numberText.text = $"Start";
        }
        

        #endregion


    }
}