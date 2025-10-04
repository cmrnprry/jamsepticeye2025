using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public List<GameObject> patients = new List<GameObject>();
    public Contract Contract;
}
