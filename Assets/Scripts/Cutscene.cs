using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Cutscene : MonoBehaviour
{
    public GameObject continue_button_obj;
    public List<Sprite> backgrounds;
    [Serializable]
    public struct DialogueEntry
    {
        public string text;
        public AudioClip voice;
        public int bg_image_index;
    }
    public List<DialogueEntry> voice_entries;
    public string next_scene;

    private TypewriterCore typewriter;
    private Image background;
    private CanvasGroup continue_button;
    private int entry_index;

    // Start is called before the first frame update
    void Start()
    {
        entry_index = 0;
        typewriter = GetComponentInChildren<TypewriterCore>();
        background = GetComponent<Image>();
        continue_button = continue_button_obj.GetComponent<CanvasGroup>();

        typewriter.onTextShowed.AddListener(ShowContinue);
        DisplayEntry(entry_index);
    }

    void DisplayEntry(int index)
    {
        typewriter.ShowText(voice_entries[index].text);
        if (voice_entries.Count > 0 && voice_entries[index].voice != null)
            AudioManager.instance.PlaySFX(voice_entries[index].voice);
        background.sprite = backgrounds[voice_entries[index].bg_image_index];
        continue_button.DOFade(0.0f, 0.25f);
    }

    void ShowContinue()
    {
        Sequence continue_sequence = DOTween.Sequence();
        continue_sequence.AppendInterval(0.5f).Append(continue_button.DOFade(1.0f, 0.5f));
    }

void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (typewriter.isShowingText)
            {
                typewriter.SkipTypewriter();
            }
            else if (entry_index < voice_entries.Count)
            {
                if (voice_entries[entry_index].voice != null)
                    AudioManager.instance.StopSFX(voice_entries[entry_index].voice);

                entry_index++;
                if (entry_index >= voice_entries.Count)
                {
                        TransistionsAndLoading.instance.StartSceneLoad(next_scene, next_scene == "Main Menu");
                }
                else
                {
                    DisplayEntry(entry_index);
                }
            }
        }
    }
}
