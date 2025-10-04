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
        public void SwitchSceens(GameObject obj)
        {
            TransistionsAndLoading.instance.TransitionToMenu(obj);
        }
    }
}