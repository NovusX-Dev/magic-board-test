using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagicBoard
{
    public class UiManager : MonoSingleton<UiManager>
    {
        #region Exposed_Variables

        [SerializeField] private Button rollDiceButton;
        [SerializeField] private TextMeshProUGUI infoText = null;
        
        #endregion

        #region Private_Variables

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Start()
        {
            infoText.text = "Are you lucky enough to win?";
        }

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods

        public void ActivateDiceButton(bool active)
        {
            rollDiceButton.gameObject.SetActive(active);
        }

        public void OnRollDice()
        {
            GameManager.Instance.HumanRollDice();
            ActivateDiceButton(false);
        }

        public void UpdateInfoText(string text)
        {
            infoText.text = text;
        }
        

        #endregion


    }
}