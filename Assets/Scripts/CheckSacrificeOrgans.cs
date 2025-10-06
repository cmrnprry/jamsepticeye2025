using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using TMPro;
using AYellowpaper.SerializedCollections;

public class CheckSacrificeOrgans : MonoBehaviour
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public Light2D spotlight, full_screen;
    public TextMeshProUGUI text;
    public GameObject finaltext;

    [SerializedDictionary("name", "data")]
    public SerializedDictionary<Organs, GameObject> organs = new SerializedDictionary<Organs, GameObject>();
    public void Check()
    {
        var isRight = GameManager.instance.CheckSacrifice();
        GameManager.instance.CurrentDay++;
        GameManager.instance.FailedDays += (isRight) ? 0 : 1;

        foreach (var org in GameManager.instance.GetCurrentOrgans())
        {
            organs[org.Type].SetActive(true);
        }

        StartCoroutine(SacrificeOrgans());       
    }

    IEnumerator SacrificeOrgans()
    {
        foreach (var particle in particleSystems)
        {
            particle.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(.5f);

        spotlight.gameObject.SetActive(true);
        full_screen.gameObject.SetActive(true);

        
        DOTween.To(() => spotlight.intensity, x => spotlight.intensity = x, 10, 5);

        yield return new WaitForSeconds(0.25f);

        AudioManager.instance.PlaySFX("tinnitus", 3f);
        Sequence shaking = DOTween.Sequence();
        shaking.Append(this.gameObject.transform.DOShakePosition(0.5f, 1, 10, 90, false, false))
            .Append(this.gameObject.transform.DOShakePosition(1.5f, 5, 15, 90, false, false))
            .Append(this.gameObject.transform.DOShakePosition(1f, 10, 20, 90, false, false))
            .Append(this.gameObject.transform.DOShakePosition(2f, 25, 30, 90, false, false));

        yield return new WaitForSeconds(3f);

        DOTween.To(() => full_screen.intensity, x => full_screen.intensity = x, 200, 4f);

        yield return new WaitForSeconds(2f);

        AudioManager.instance.StopSFX("tinnitus");
        CleanUp();

        finaltext.gameObject.SetActive(true);
        text.text = GameManager.instance.CheckSacrifice() ? "The offering is sufficient." : "I am disappointed in you.";
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("god");

        yield return new WaitForSeconds(2f);
        if (!GameManager.instance.CheckSacrifice())
        {
            GameManager.instance.CurrentDay = 0;
            TransistionsAndLoading.instance.StartCutSceneLoad("Main Game - Cutscene BAD END");
        }
            
        else
            TransistionsAndLoading.instance.StartCutSceneLoad("Main Game - Cutscene " + GameManager.instance.CurrentDay);
    }

    private void CleanUp()
    {
        foreach (KeyValuePair<Organs, GameObject> org in organs)
        {
            org.Value.gameObject.SetActive(false);
        }

        foreach (var particle in particleSystems)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }

        spotlight.gameObject.SetActive(false);
        full_screen.gameObject.SetActive(false);
        spotlight.intensity = 0;
        full_screen.intensity = 0;
    }
}
