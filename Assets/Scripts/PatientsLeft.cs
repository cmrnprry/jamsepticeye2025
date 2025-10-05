using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PatientsLeft : MonoBehaviour
{
    private TextMeshProUGUI TMP;
    public int TotalPatients;
    private void Start()
    {
        TMP = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText()
    {
        TotalPatients -= 1;
        TMP.text = $"{TotalPatients} / {GameManager.instance.Patients.Count} Patients Left";
    }
}
