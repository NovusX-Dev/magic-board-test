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

        [SerializeField] private Dice dice = null;
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

        private void Start()
        {
            foreach (var player in players)
            {
                player.playerName = player.currentStone.gameObject.name;
            }

            _activePlayer = Random.Range(0, players.Count);
            UiManager.Instance.UpdateInfoText($"{players[_activePlayer].playerName}'s Turn!");
        }

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
                        SwitchPlayerState();
                        break;
                }
            }
            
            if (players[_activePlayer].type == Player.PlayerTypes.Human)
            {
                switch (currentState)
                {
                    case States.Waiting:

                        break;
                    
                    case States.RollDice:
                        UiManager.Instance.ActivateDiceButton(true);
                        currentState = States.Waiting;
                        break;
                    
                    case States.SwitchPlayer:
                        SwitchPlayerState();
                        break;
                }
            }
        }
        

        #endregion

        #region Private_Methods

        private void SwitchPlayerState()
        {
            _activePlayer++;
            _activePlayer %= players.Count;
            UiManager.Instance.UpdateInfoText($"{players[_activePlayer].playerName}'s Turn!");
            currentState = States.RollDice;
        }

        IEnumerator RollDiceRoutine()
        {
            yield return new WaitForSeconds(rollDiceWaitTime);
            dice.RollDice();
        }

        #endregion

        #region Public_Methods

        public void SetState(States state)
        {
            currentState = state;
        }

        public void HumanRollDice()
        {
            StartCoroutine(RollDiceRoutine());
        }

        public void DiceFinishedRolling(int value)
        {
            _rolledDiceNumber = value;
            players[_activePlayer].currentStone.PlayTurn(_rolledDiceNumber);
            var infoText = $"{players[_activePlayer].playerName} rolled {_rolledDiceNumber.ToString()}";
            UiManager.Instance.UpdateInfoText(infoText);
        }

        public void GameWon(Stone winner)
        {
            Debug.Log($"Winner is {winner.gameObject.name}");
        }

        #endregion


    }
}