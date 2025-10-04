using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MazeGameManager : MonoBehaviour
{
    private bool TrackBadTouch = false;
    private Organs organ;
    [SerializeField] private int MaxBadTouch;
    public int CurrentBadTouches;

    [Header("Failure Indicator")]
    public List<RectTransform> Failure;
    private float time = .5f;


    [Header("Start and End Goals")]
    public GameObject Start_Goal;
    public GameObject End_Goal;
    public Image Maze;

    private void Start()
    {
        Maze.alphaHitTestMinimumThreshold = .75f;
    }
    public void EnableBadTouch()
    {
        TrackBadTouch = true;
    }

    public void BadTouchCollision()
    {
        if (!TrackBadTouch)
        { return; }

        CurrentBadTouches += 1;

        //TODO: play audio
        TrackBadTouch = false;

        Sequence mySequence = DOTween.Sequence();

        Failure[CurrentBadTouches - 1].DOShakeAnchorPos(1.5f, CurrentBadTouches*2, 10, 30);
        mySequence.Append(Failure[CurrentBadTouches - 1].DOScale(2, time))
            .Append(Failure[CurrentBadTouches - 1].DOScale(1, time / 2)).AppendInterval(time).OnComplete(() =>
            {
                if (CurrentBadTouches >= MaxBadTouch)
                    FailMaze();
                else
                    ResetMaze();
            });
    }

    public void SetOrgan(Organs o)
    {
        organ = o;
    }

    private void ResetMaze()
    {
        Start_Goal.SetActive(true);
        End_Goal.SetActive(false);
    }

    public void SuccessMaze()
    {
        TrackBadTouch = false;
        GameManager.instance.AddOrganHarvested(organ);
        Debug.Log("Success :(");
    }

    private void FailMaze()
    {
        TrackBadTouch = false;
        Destroy(this.gameObject);
        Debug.Log("Failure :(");
    }
}
