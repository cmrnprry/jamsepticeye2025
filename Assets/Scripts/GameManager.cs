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
    private int CurrentDay;

    private int Current_Patient;
    private GameObject Fire_Particles;

    [Header("Level Data")]
    public List<LevelData> Levels;

    private Transform patient_Parent;
    private Contract Contract;
    private List<GameObject> Patients;
    private List<Stipulations> CurrentOrgans = new List<Stipulations>();

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
            Instantiate(Patients[0], patient_Parent);

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

        int last = Current_Patient;

        if (Current_Patient >= Patients.Count)
            Current_Patient = 0;
        else
            Current_Patient++;

        Patients[last] = Patients[Current_Patient];

        yield return new WaitForSeconds(2.5f);

        for (int ii = 0; ii < Fire_Particles.transform.childCount; ii++)
        {
            Fire_Particles.transform.GetChild(ii).gameObject.SetActive(false);
        }
    }

    public void AddOrganHarvested(Organs o)
    {
        CurrentOrgans.Add(new Stipulations(o, Patients[Current_Patient].GetComponent<Patient>().characteristics));
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