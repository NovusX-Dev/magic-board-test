using System.Collections.Generic;
using UnityEngine;

namespace MagicBoard
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        #region Exposed_Variables

        [SerializeField] private GameObject sfxSourcesContainer;

        #endregion

        #region Private_Variables

        private AudioSource[] _sfxSources;
        
        #endregion

        #region Public_Variables

        #endregion

        #region Unity_Calls

        public override void Init()
        {
            base.Init();
            _sfxSources = sfxSourcesContainer.GetComponentsInChildren<AudioSource>();
        }

        #endregion

        #region Private_Methods

        private AudioSource GetFreeAudioSource()
        {
            if (_sfxSources.Length < 1) return null;
            var freeSource = new AudioSource();
            foreach (var source in _sfxSources)
            {
                if (source.isPlaying) continue;
                freeSource =  source;
                break;
            }

            return freeSource;
        }

        #endregion

        #region Public_Methods

        public void PlaySfx(AudioClip clip)
        {
            var source = GetFreeAudioSource();
            source.clip = clip;
            source.Play();
        }

        #endregion


    }
}