using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public class Stone : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private float speed = 8f;
        [SerializeField] private RouteGenerator currentRoute = null; //to be changed later

        #endregion

        #region Private_Variables

        private List<Node> _nodes = new List<Node>();
        private int _routePosition;
        private int _stoneId;
        private int stepsToMove;
        private int stepsDone;
        private bool isMoving;

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void OnEnable()
        {
            currentRoute.OnSetNodesList += SetNodesList;
        }

        private void OnDisable()
        {
            currentRoute.OnSetNodesList -= SetNodesList;
        }

        private void Start()
        {
            
        }

        #endregion

        #region Private_Methods

        private void SetNodesList(List<Node> nodes)
        {
            _nodes = nodes;
        }

        private bool MoveToNextNode(Vector3 nextPosition)
        {
            return nextPosition != (transform.position =
                Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime));
        }

        IEnumerator MoveRoutine()
        {
            if(isMoving) yield break;
            isMoving = true;

            while (stepsToMove > 0)
            {
                _routePosition++;
                Vector3 nextPos = _nodes[_routePosition].transform.position;
                while (!MoveToNextNode(nextPos)) yield return null;

                yield return new WaitForEndOfFrame();
                stepsToMove--;
                stepsDone++;
            }

            isMoving = false;
        }

        #endregion

        #region Public_Methods

        #endregion


    }
}