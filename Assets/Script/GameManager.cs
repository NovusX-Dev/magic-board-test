using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicBoard
{
    [Serializable]
    public class Player
    {
        public string playerName;
        public Stone currentStone;
        public PlayerTypes type;
        
        public enum PlayerTypes
        {
            CPU = 0,
            Human = 1
        }

    }
    
    public class GameManager : MonoSingleton<GameManager>
    {

        #region Exposed_Variables

        [SerializeField] private List<Player> players = new List<Player>();
        [SerializeField] private States currentState;

        [Header("Delays")] 
        [SerializeField] private float rollDiceWaitTime = 2f;
        #endregion

        #region Private_Variables

        private int _activePlayer;
        private int _rolledDiceNumber;

        #endregion

        #region Public_Variables
        
        public  enum States {Waiting, RollDice, SwitchPlayer}

        #endregion

        #region Unity_Calls

        private void Update()
        {
            if (players[_activePlayer].type == Player.PlayerTypes.CPU)
            {
                switch (currentState)
                {
                    case States.Waiting:

                        break;
                    
                    case States.RollDice:
                        StartCoroutine(RollDiceRoutine());
                        currentState = States.Waiting;
                        break;
                    
                    case States.SwitchPlayer:
                        _activePlayer++;
                        _activePlayer %= players.Count;
                        currentState = States.RollDice;
                        break;
                }
            }
        }

        #endregion

        #region Private_Methods

        IEnumerator RollDiceRoutine()
        {
            yield return new WaitForSeconds(rollDiceWaitTime);
            _rolledDiceNumber = Random.Range(1, 7);
            
            //New Turn
            players[_activePlayer].currentStone.PlayTurn(_rolledDiceNumber);
        }

        #endregion

        #region Public_Methods

        public void SetState(States state)
        {
            currentState = state;
        }

        #endregion


    }
}