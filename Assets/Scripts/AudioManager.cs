using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AYellowpaper.SerializedCollections
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Settings Volume")]
        public AudioMixer mixer;

        [SerializedDictionary("BGM name", "BGM")]
        public SerializedDictionary<string, AudioClip> BGMDictionary;
        [SerializeField] private AudioSource BGMSource;

        [SerializedDictionary("SFX name", "SFX")]
        public SerializedDictionary<string, AudioClip> SFXDictionary;
        [SerializeField] private AudioSource SFXSource;
        [SerializeField] private Transform SFXParent;
        private List<AudioSource> added_sources = new List<AudioSource>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(gameObject);
            SFXSource.Stop();
        }

        private void Start()
        {
            PlayBGMIntro("MainMenu");
        }

        public void PlayBGMIntro(string intro)
        {
            BGMSource.clip = BGMDictionary[intro+"_Intro"];
            BGMSource.loop = false;
            BGMSource.Play();

            StartCoroutine(PlayFullAfterLoop(intro));
        }

        public void PlayBGMLoop(string loop)
        {
            BGMSource.clip = BGMDictionary[loop];
            BGMSource.loop = true;
            BGMSource.Play();
        }

        IEnumerator PlayFullAfterLoop(string loop)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil (() => !BGMSource.isPlaying);
            PlayBGMLoop(loop + "_Loop");
        }

        public static void PlayMenuClick()
        {
			instance.PlaySFX("stab");
            instance.PlaySFX("click");
        }

        public void PlaySFX(string src, float fadeIn = 0, float delay = 0)
        {
            if (SFXDictionary.ContainsKey(src))
            {
                PlaySFX(SFXDictionary[src], fadeIn, delay);
            }
        }

        public void PlaySFX(AudioClip clip, float fadeIn = 0, float delay = 0)
        {
            if (!SFXSource.isPlaying)
            {
                SFXSource.clip = clip;

                if (fadeIn > 0 || delay > 0)
                    StartCoroutine(FadeIn(SFXSource, fadeIn, delay));
                else
                    SFXSource.Play();
            }
            else
            {
                var sfx = Instantiate(SFXSource, SFXParent);
                added_sources.Add(sfx);
                sfx.clip = clip;

                if (fadeIn > 0 || delay > 0)
                    StartCoroutine(FadeIn(sfx, fadeIn, delay));
                else
                    sfx.Play();
            }
        }

		public void Update()
		{
            for (int i = 0; i < added_sources.Count; i++)
            {
                if (!added_sources[i].isPlaying)
                {
					Destroy(added_sources[i].gameObject);
					added_sources.RemoveAt(i);
					--i;
				}
            }
		}

		public void StopSFX(AudioClip src, float delay = 0)
        {
            StopSFX(src.name);
        }

        public void StopSFX(string src, float delay = 0)
        {
            if (SFXSource.isPlaying && SFXSource.clip.name == src)
            {
                SFXSource.Stop();
            }
			for (int i = 0; i < added_sources.Count; i++)
			{
				if (added_sources[i].isPlaying && added_sources[i].clip.name == src)
				{
					added_sources[i].Stop();
					Destroy(added_sources[i].gameObject);
					added_sources.RemoveAt(i);
                    --i;
				}
			}
		}

        private IEnumerator FadeIn(AudioSource src, float duration = 0, float delay = 0)
        {
            src.volume = 0;
            yield return new WaitForSeconds(delay);

            src.Play();
        }

        private IEnumerator FadeOut(AudioSource src, float duration = 0, float delay = 0)
        {
            yield return new WaitForSeconds(delay);

            src.DOFade(0, duration);

            yield return new WaitForSeconds(duration);
            src.Stop();
            added_sources.Remove(src);
            Destroy(src.gameObject);
        }

        public void AdjustMaster(float value)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }

        public void AdjustBGM(float value)
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        }

        public void AdjustSFX(float value)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        }
    }
}