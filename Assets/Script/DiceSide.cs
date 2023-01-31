using System;
using UnityEngine;

namespace MagicBoard
{
    public class DiceSide : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private int oppositeSideValue;
        
        #endregion

        #region Private_Variables

        private const string GroundTag = "Ground";

        #endregion

        #region Public_Variables

        public bool OnGround { get; private set; }
        public int OppositeSideValue => oppositeSideValue;

        #endregion

        #region Unity_Calls

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(GroundTag))
            {
                OnGround = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            OnGround = false;
        }

        #endregion

        #region Private_Methods

        #endregion

        #region Public_Methods


        #endregion


    }
}