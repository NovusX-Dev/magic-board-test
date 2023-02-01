using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicBoard
{
    public class UiManager : MonoSingleton<UiManager>
    {
        #region Exposed_Variables

        [SerializeField] private Button rollDiceButton;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] private TextMeshProUGUI luckText = null;
        [SerializeField] private TextMeshProUGUI turnsText = null;
        
        [Header("Win Panel")] 
        [SerializeField] private GameObject winPanel = null;
        [SerializeField] private TextMeshProUGUI winnerText = null;
        [SerializeField] private TextMeshProUGUI totalTurnsText = null;
        [SerializeField] private string mainMenuScene = "";
        
        #endregion

        #region Private_Variables

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Start()
        {
            infoText.text = "Are you lucky enough to win?";
            winPanel.SetActive(false);
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

        public void GameWonPanel(string playerName, Color playerColor, int totalTurns)
        {
            winPanel.SetActive(true);
            winnerText.text = playerName;
            winnerText.color = playerColor;
            totalTurnsText.text = $"Total Turns Played: {totalTurns.ToString()}";
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

        public void UpdateTurns(int turns)
        {
            turnsText.text = $"Turns Played: {turns.ToString()}";
        }

        #endregion


    }
}