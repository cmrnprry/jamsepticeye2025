using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Organs { Heart, Stomach, Pancreas, Ribs, Spine, Liver};
public enum Characteristics { };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int MaxDays;
    private int CurrentDay;
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetDataOnLoad()
    {
        Button toss = GameObject.FindWithTag("TossBody").GetComponent<Button>();
        toss.onClick.AddListener(TossBody);
    }

    private void TossBody()
    {

    }


}


public struct Contract
{ 
    //NOTE: each contract will live in it's own scene 
    public List<Stipulations> stipulations_needed;
}

public struct Stipulations
{
    public List<Characteristics> characteristics;
    public Organ Type;
}