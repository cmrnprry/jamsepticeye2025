using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGameManager : MonoBehaviour
{
    private bool TrackBadTouch = false;
    [SerializeField] private int MaxBadTouch;
    public int CurrentBadTouches;

    [Header("Start and End Goals")]
    public GameObject Start_Goal, End_Goal;
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
        if (!TrackBadTouch) { return; }

        CurrentBadTouches += 1; 
        if (CurrentBadTouches >= MaxBadTouch)
        {
            FailMaze();
        }
        else
        {
            ResetMaze();
        }
    }

    private void ResetMaze()
    {
        TrackBadTouch = true;
        Start_Goal.SetActive(true);
        End_Goal.SetActive(false);
    }

    public void SuccessMaze()
    {
        TrackBadTouch = false;
        Debug.Log("Success :(");
    }

    private void FailMaze()
    {
        TrackBadTouch = false;
        this.gameObject.SetActive(false);
        Debug.Log("Failure :(");
    }
}
