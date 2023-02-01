using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MagicBoard
{
    public class Stone : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private float straightSpeed = 8f;
        [SerializeField] private float arcSpeed = 2.5f;
        [SerializeField] private float endTurnWaitTime = 0.5f;
        [SerializeField] private RouteGenerator currentRoute = null;
        [SerializeField] private AudioClip hopClip = null;
        [SerializeField] private AudioClip snakeClip = null;
        [SerializeField] private AudioClip ladderClip = null;

        #endregion

        #region Private_Variables

        private List<Node> _nodes = new List<Node>();
        private int _routePosition;
        private int _stoneId;
        private int _stepsToMove;
        private int _stepsDone;
        private float _arcTime = 0f;
        private float _arcHeight = 0.5f;
        private bool _isMoving;
        private WaitForEndOfFrame _waitForEndOfFrame;
        private WaitForSeconds _waitAfterMove;

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
            _waitAfterMove = new WaitForSeconds(endTurnWaitTime);
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
                Vector3.MoveTowards(transform.position, nextPosition, straightSpeed * Time.deltaTime));
        }

        private bool MoveInArcToNextNode(Vector3 startPos, Vector3 nextPos, float speed)
        {
            _arcTime += speed * Time.deltaTime;
            var arcPosition = Vector3.Lerp(startPos, nextPos, _arcTime);
            arcPosition.y += _arcHeight * Mathf.Sin(Mathf.Clamp01(_arcTime) * Mathf.PI);
            return nextPos != (transform.position = Vector3.Lerp(transform.position, arcPosition, _arcTime));
        }

        IEnumerator MoveRoutine()
        {
            if(_isMoving) yield break;
            _isMoving = true;
            _nodes[_routePosition].RemoveStone(this);
            
            while (_stepsToMove > 0)
            {
                _routePosition++;
                Vector3 nextPos = _nodes[_routePosition].transform.position;
                var startPos =_nodes[_routePosition - 1].transform.position;
                while (MoveInArcToNextNode(startPos, nextPos, arcSpeed)) yield return  null;

                yield return _waitForEndOfFrame;
                AudioManager.Instance.PlaySfx(hopClip);

                _arcTime = 0f;
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
                AudioManager.Instance.PlaySfx(_routePosition > connectedNodeId ? ladderClip : snakeClip);
                _stepsDone = connectedNodeId;
                _routePosition = connectedNodeId;
            }
            
            _nodes[_routePosition].AddStone(this);
            yield return _waitAfterMove;
            
            if (_stepsDone == _nodes.Count - 1)
            {
                //Win
                GameManager.Instance.GameWon(this);
                yield break;
            }
            
            GameManager.Instance.SetState(GameManager.States.SwitchPlayer);
            _isMoving = false;
        }

        #endregion

        #region Public_Methods

        public void PlayTurn(int steps)
        {
            _stepsToMove = steps;
             if(_stepsDone + _stepsToMove < _nodes.Count)
                 StartCoroutine(MoveRoutine());
             else
             {
                 Debug.LogWarning($"{gameObject.name} Steps {_stepsDone + _stepsToMove} are higher than nodes");
                 GameManager.Instance.SetState(GameManager.States.SwitchPlayer);
             }
        }

        #endregion


    }
}