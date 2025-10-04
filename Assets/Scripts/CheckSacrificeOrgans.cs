using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSacrificeOrgans : MonoBehaviour
{
   public void CHeck()
    {
        var isRight = GameManager.instance.CheckSacrifice();
        GameManager.instance.CurrentDay++;

            TransistionsAndLoading.instance.StartCutSceneLoad("");

    }
}
