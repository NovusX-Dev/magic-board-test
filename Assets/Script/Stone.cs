using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private int _stepsToMove;
        private int _stepsDone;
        private bool _isMoving;
        private WaitForEndOfFrame _waitForEndOfFrame;

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

        private void Awake()
        {
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void Update()
        {
            //Debug Only
            if (Input.GetKeyDown(KeyCode.R) && !_isMoving)
            {
                _stepsToMove = Random.Range(1, 7);
                if(_stepsDone + _stepsToMove < _nodes.Count)
                    StartCoroutine(MoveRoutine());
                else
                    Debug.LogWarning($"{gameObject.name} Steps {_stepsDone + _stepsToMove} are higher than nodes");
            }
        }
        

        #endregion

        #region Private_Methods

        private void SetNodesList(List<Node> nodes)
        {
            _nodes = nodes;
        }

        private bool MovingToNextNode(Vector3 nextPosition)
        {
            return nextPosition != (transform.position =
                Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime));
        }

        IEnumerator MoveRoutine()
        {
            if(_isMoving) yield break;
            _isMoving = true;

            while (_stepsToMove > 0)
            {
                _routePosition++;
                Vector3 nextPos = _nodes[_routePosition].transform.position;
                while (MovingToNextNode(nextPos)) yield return null;

                yield return _waitForEndOfFrame;
                _stepsToMove--;
                _stepsDone++;
            }
            yield return _waitForEndOfFrame;
            //Snakes & Ladders
            if (_nodes[_routePosition].HasConnectedNode())
            {
                var connectedNodeId = 0;
                _nodes[_routePosition].GetConnectedNodeId(ref connectedNodeId);
                var nextPos = _nodes[connectedNodeId].transform.position;
                while (MovingToNextNode(nextPos)) yield return null;

                yield return _waitForEndOfFrame;
                _stepsDone = connectedNodeId;
                _routePosition = connectedNodeId;
            }

            yield return _waitForEndOfFrame;
            _isMoving = false;
        }

        #endregion

        #region Public_Methods

        #endregion


    }
}