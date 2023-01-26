using TMPro;
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

        #endregion

        #region Unity_Calls

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods

        public void SetNodeId(int id)
        {
            _nodeId = id;
            numberText.text = id.ToString();
        }

        #endregion


    }
}