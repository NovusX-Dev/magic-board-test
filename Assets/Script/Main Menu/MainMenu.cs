using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagicBoard.Main_Menu
{
    [System.Serializable]
    public class MenuToggle
    {
        public Toggle cpu;
        public Toggle human;
    }
    
    public class MainMenu : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private string boardScene = null;
        
        [Header("Toggles")]
        [SerializeField] private MenuToggle redToggle = null;
        [SerializeField] private MenuToggle blueToggle = null;
        [SerializeField] private MenuToggle greenToggle = null;
        [SerializeField] private MenuToggle yellowToggle = null;
        

        #endregion

        #region Private_Variables

        private string _cpu = "";
        private string _human = "";

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Start()
        {
            _cpu = StartSettings.Cpu;
            _human = StartSettings.Human;
        }

        #endregion

        #region Private_Methods

        private void ReadToggles()
        {
            SaveToggle(redToggle, 0);
            SaveToggle(blueToggle, 1);
            SaveToggle(greenToggle, 2);
            SaveToggle(yellowToggle, 3);
        }

        private void SaveToggle(MenuToggle mToggle, int saveIndex)
        {
            if (mToggle.cpu.isOn)
                StartSettings.Players[saveIndex] = _cpu;
            else if (mToggle.human.isOn)
                StartSettings.Players[saveIndex] = _human;
        }

        #endregion

        #region Public_Methods

        public void StartGame()
        {
            StartCoroutine(StartRoutine());
            IEnumerator StartRoutine()
            {
                ReadToggles();
                yield return new WaitForEndOfFrame();
                SceneManager.LoadScene(boardScene);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
            
        }

        #endregion

        
    }
}