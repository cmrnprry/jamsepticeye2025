using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace AYellowpaper.SerializedCollections
{
    public class MenuScreens : MonoBehaviour
    {
        [Header("Transition Screen")]
        public Image TransitionScreen;
        public GameObject Settings, MainMenu, SaveLoad;

        private float wait = 0.25f;

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        public void StartGame(GameObject Menu)
        {
            TransitionScreen.gameObject.SetActive(true);
            TransitionScreen.DOFade(1, 0.5f).OnComplete(() =>
            {
                MainMenu.SetActive(false);
                Menu.SetActive(true);
                TransitionScreen.DOFade(0, 0.5f).OnComplete(() =>
                {
                    TransitionScreen.gameObject.SetActive(false);
                });
            });
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ShowScreen(GameObject Menu)
        {
            TransitionScreen.gameObject.SetActive(true);
            TransitionScreen.DOFade(1, 0.5f).OnComplete(() =>
            {
                Menu.SetActive(!Menu.activeSelf);
                TransitionScreen.DOFade(0, 0.5f).OnComplete(() =>
                {
                    TransitionScreen.gameObject.SetActive(false);
                });
            });
        }

        public void ShowSettings()
        {
            TransitionScreen.gameObject.SetActive(true);
            TransitionScreen.DOFade(1, 0.5f).OnComplete(() =>
            {
                Settings.SetActive(true);
                TransitionScreen.DOFade(0, 0.5f).OnComplete(() =>
                {
                    TransitionScreen.gameObject.SetActive(false);
                    Time.timeScale = 0;
                });
            });
        }

        private void ShowScreen(GameObject Menu, bool shouldShow)
        {
            TransitionScreen.gameObject.SetActive(true);
            TransitionScreen.DOFade(1, 0.5f).OnComplete(() =>
            {
                Menu.SetActive(shouldShow);
                TransitionScreen.DOFade(0, 0.5f).OnComplete(() =>
                {
                    TransitionScreen.gameObject.SetActive(false);
                });
            });
        }

        private void CloseSettingsOnLoad()
        {
            StartCoroutine(WaitToHideSettings());
        }

        private IEnumerator WaitToHideSettings()
        {
            yield return new WaitForSeconds(wait);

            if (Settings.activeSelf)
                ShowScreen(Settings, false);

            if (MainMenu.activeSelf)
                ShowScreen(MainMenu, false);
        }
    }
}