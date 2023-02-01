using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagicBoard
{
    public class Dice : MonoBehaviour
    {
        #region Exposed_Variables

        [SerializeField] private int maxTorque = 500;
        [SerializeField] private AudioClip rollClip;
        [SerializeField] private DiceSide[] diceSides;

        #endregion

        #region Private_Variables

        private bool _hasLanded = false;
        private bool _rolled = false;
        private int _diceValue;
        private Vector3 _initPosition;
        private Rigidbody _rigidbody = null;

        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _initPosition = transform.position;
            _rigidbody.useGravity = false;
        }

        private void Update()
        {
            if (_rigidbody.IsSleeping() && !_hasLanded && _rolled)
            {
                _hasLanded = true;
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
                
                SideValueCheck();
            }
            else if (_rigidbody.IsSleeping() && _hasLanded && _diceValue == 0)
            {
                RollAgain();
            }
        }

        #endregion

        #region Private_Methods

        private void ResetDice()
        {
            transform.position = _initPosition;
            _rigidbody.isKinematic = false;
            _rolled = false;
            _hasLanded = false;
            _rigidbody.useGravity = false;
        }
        
        private Vector3 RandomTorque()
        {
            var x = Random.Range(0, maxTorque);
            var y = Random.Range(0, maxTorque);
            var z = Random.Range(0, maxTorque);
            return new Vector3(x, y, z);
        }

        private void RollAgain()
        {
            ResetDice();
            _rolled = true;
            _rigidbody.useGravity = true;
            _rigidbody.AddTorque(RandomTorque());
        }

        private void SideValueCheck()
        {
            _diceValue = 0;
            foreach (var side in diceSides)
            {
                if (!side.OnGround) continue;
                _diceValue = side.OppositeSideValue;
                GameManager.Instance.DiceFinishedRolling(_diceValue);
                break;
            }
        }

        #endregion

        #region Public_Methods

        public void RollDice()
        {
            AudioManager.Instance.PlaySfx(rollClip);
            ResetDice();
            if (!_rolled && !_hasLanded)
            {
                _rolled = true;
                _rigidbody.useGravity = true;
                _rigidbody.AddTorque(RandomTorque());
            }
            else if (_rolled && _hasLanded)
            {
                ResetDice();
            }
        }

        public void SetScale(bool lucky)
        {
            transform.localScale = lucky ? transform.localScale = Vector3.one * 2.5f : Vector3.one;
        }

        #endregion


    }
}