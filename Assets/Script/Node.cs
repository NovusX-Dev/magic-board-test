using System;
using System.Collections.Generic;
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
        private List<Stone> _landedStones = new List<Stone>();

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

        private void RearrangeStones()
        {
            if (_landedStones.Count > 1)
            {
                int squareSize = Mathf.CeilToInt(Mathf.Sqrt(_landedStones.Count));
                int stoneIndex = -1;

                for (int x = 0; x < squareSize; x++)
                {
                    for (int y = 0; y < squareSize; y++)
                    {
                        stoneIndex++;
                        if (stoneIndex > _landedStones.Count - 1) break;
                        var newPos = transform.position + new Vector3(-0.25f + x * 0.5f, 0f, -0.25f + y * 0.5f);
                        _landedStones[y].transform.position = newPos;
                    }
                }
            }
            else if (_landedStones.Count == 1)
            {
                _landedStones[0].transform.position = transform.position;
            }
        }

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

        public void AddStone(Stone player)
        {
            _landedStones.Add(player);
            RearrangeStones();
        }

        public void RemoveStone(Stone player)
        {
            if (_landedStones.Count < 1) return;
            _landedStones.Remove(player);
            RearrangeStones();
        }

        #endregion


    }
}