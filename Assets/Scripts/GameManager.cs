using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public enum Organs { Heart, Intestines, Stomach, Pancreas, Ribs, Spine, Liver, Lung };

[SerializeField]
public enum Characteristics { Blonde, Bald, Ginger, Man, Woman, Bearded, Short_Hair, Long_Hair, Old, Young, Malnourished, Pink_Cheeks, Wrinkles };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int MaxDays;
    [HideInInspector] public int CurrentDay = 0;
    [HideInInspector] public int FailedDays = 0;

    private GameObject Fire_Particles;

    [Header("Level Data")]
    public List<LevelData> Levels;

    private Transform patient_Parent;
    private Contract Contract;
    private List<GameObject> Patients;
    private int Patient_Index;
    private GameObject Current_Patient;
    private List<Stipulations> CurrentOrgans = new List<Stipulations>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MaxDays = Levels.Count - 1;
    }

    public List<Stipulations> GetCurrentOrgans()
    {
        return CurrentOrgans;
    }

    public void SetDataOnLoad()
    {
        Button toss = GameObject.FindWithTag("TossBody").GetComponent<Button>();
        if (toss != null)
            toss.onClick.AddListener(TossBody);

        Fire_Particles = GameObject.FindWithTag("Fire_Particles");
        if (Fire_Particles != null)
        {
            Patients = Levels[CurrentDay].patients;
            Contract = Levels[CurrentDay].Contract;
        }

        patient_Parent = GameObject.FindWithTag("Patient_Parent").transform;
        if (patient_Parent != null)
        {
            Current_Patient = Instantiate(Patients[0], patient_Parent);
            Patient_Index = 0;
        }

        CurrentOrgans = new List<Stipulations>();
    }

    private void TossBody()
    {
        StartCoroutine(FirePlay());
    }

    IEnumerator FirePlay()
    {
        for (int ii = 0; ii < Fire_Particles.transform.childCount; ii++)
        {
            Fire_Particles.transform.GetChild(ii).gameObject.SetActive(true);

            if (ii % 3 == 0)
                yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2.5f);

        int last = Patient_Index;

        if (Patient_Index >= Patients.Count)
            Patient_Index = 0;
        else
            Patient_Index++;

        Destroy(Current_Patient);

        if (Patient_Index < Patients.Count)
            Current_Patient = Instantiate(Patients[Patient_Index], patient_Parent);
        else
            GameObject.FindWithTag("No_More").transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        for (int ii = 0; ii < Fire_Particles.transform.childCount; ii++)
        {
            Fire_Particles.transform.GetChild(ii).gameObject.SetActive(false);
        }
    }

    public void AddOrganHarvested(Organs o)
    {
        CurrentOrgans.Add(new Stipulations(o, Patients[Patient_Index].GetComponent<Patient>().characteristics));
    }

    public bool CheckSacrifice()
    {
        foreach (Stipulations s in Contract.stipulations_needed)
        {
            bool match_found = false;
            foreach (var o in CurrentOrgans)
            {
                if (o.Type == s.Type)
                {
                    bool chars_match = true;
                    foreach (var Chara in s.characteristics)
                    {
                        if (!o.characteristics.Contains(Chara))
                        {
                            chars_match = false;
                            break;
                        }
                    }
                    if (chars_match)
                    {
                        match_found = true;
                        break;
                    }
                }
            }
            if (!match_found)
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public struct Contract
{
    //NOTE: each contract will live in it's own scene 
    public List<Stipulations> stipulations_needed;
}

[System.Serializable]
public struct Stipulations
{
    public List<Characteristics> characteristics;
    [SerializeField]
    public Organs Type;

    public Stipulations(Organs t, List<Characteristics> c)
    {
        Type = t;
        characteristics = c;
    }
}