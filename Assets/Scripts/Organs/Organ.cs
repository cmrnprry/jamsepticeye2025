using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Organ : MonoBehaviour
{
    public GameObject Maze;
    public Organs type;

    //TODO: add difficulty things that turn on depending on day

    private void Start()
    {
        this.gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = .9f;
    }
    private void DetermineDifficulty()
    {

    }


    public void OrganChosen()
    {
        GameObject m = Instantiate(Maze, this.gameObject.transform.parent.parent);
        m.GetComponent<MazeGameManager>().SetOrgan(type);
        DetermineDifficulty();
    }

}
