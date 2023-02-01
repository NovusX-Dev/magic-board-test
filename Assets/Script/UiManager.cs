using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MagicBoard
{
    public class UiManager : MonoSingleton<UiManager>
    {
        #region Exposed_Variables

        [SerializeField] private Button rollDiceButton;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] private TextMeshProUGUI luckText = null;
        [SerializeField] private TextMeshProUGUI infoTurnsText = null;
        [SerializeField] private TextMeshProUGUI menuTotalTurnsText = null;
        
        [Header("Win Panel")] 
        [SerializeField] private GameObject winPanel = null;
        [SerializeField] private TextMeshProUGUI winnerText = null;
        [SerializeField] private TextMeshProUGUI winTotalTurnsText = null;
        [SerializeField] private string mainMenuScene = "";
        
        #endregion

        #region Private_Variables

        private int _totalTurns = 0;
        private float _gameSpeed;
        
        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Start()
        {
            infoText.text = "Are you lucky enough to win?";
            winPanel.SetActive(false);
            SetSpeed(1);
        }

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods

        public void ActivateDiceButton(bool active, Color playerColor)
        {
            rollDiceButton.gameObject.SetActive(active);
            rollDiceButton.image.color = playerColor;
        }

        public void OnRollDice()
        {
            GameManager.Instance.HumanRollDice();
            rollDiceButton.gameObject.SetActive(false);
        }

        public void UpdateInfoText(string text, Color playerColor)
        {
            infoText.text = text;
            infoText.color = playerColor;
        }

        public void GameWonPanel(string playerName, Color playerColor)
        {
            winPanel.SetActive(true);
            winnerText.text = playerName;
            winnerText.color = playerColor;
        }

        public void UpdateTotalTurns(int turns)
        {
            _totalTurns = turns;
            winTotalTurnsText.text = $"Total Turns Played: {_totalTurns.ToString()}";
            menuTotalTurnsText.text = $"Total Turns Played: {_totalTurns.ToString()}";
            infoTurnsText.text = $"Turns Played: {_totalTurns.ToString()}";
        }
        

        public void OnGoBackToMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public void GotLucky(Color playerColor)
        {
            StartCoroutine(LuckRoutine());
            IEnumerator LuckRoutine()
            {
                luckText.gameObject.SetActive(true);
                luckText.color = playerColor;
                yield return new WaitForSeconds(1.5f);
                luckText.gameObject.SetActive(false);
            }
        }

        public void SetSpeed(float speed)
        {
            _gameSpeed = speed;
            Time.timeScale = speed;
        }

        public void StopGame()
        {
            StartCoroutine(StopRoutine());
            IEnumerator StopRoutine()
            {
                yield return new WaitForSeconds(1.5f);
                Time.timeScale = 0f;
            }
        }

        public void ResumeGame()
        {
            Time.timeScale = _gameSpeed;
        }

        #endregion


    }
}