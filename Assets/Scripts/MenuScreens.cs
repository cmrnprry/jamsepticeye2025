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
        public Button StartButton, Quit;
        public void SwitchSceens(GameObject obj)
        {
            TransistionsAndLoading.instance.TransitionToMenu(obj);
        }

        private void Start()
        {
            if (StartButton != null)
            {
                StartButton.onClick.AddListener(() => TransistionsAndLoading.instance.StartCutSceneLoad("Main Game - Cutscene 0"));
            }

            if (Quit != null)
            {
                Quit.onClick.AddListener(() => Application.Quit());
            }
        }
    }
}