using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    // max's messing around values
    public Transform knife_transform;
    private const float kKnifeLength = 0.5f;
    private const float kAngleOffset = 60.0f;
    private EventSystem EventSystem;

    private void Start()
    {
        Maze.alphaHitTestMinimumThreshold = .9f;
		EventSystem = GetComponent<EventSystem>();
	}
    public void EnableBadTouch()
    {
        TrackBadTouch = true;
    }

    public void Update()
    {
        if (TrackBadTouch)
        {
            // Cursed math
            Vector2 cur_pos = new Vector2(knife_transform.position.x, knife_transform.position.y);
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 to_mouse_norm = (cur_pos - mouse_pos).normalized;
            float angle = Vector2.SignedAngle(to_mouse_norm, Vector2.right);
            float sign = Mathf.Sign(Vector2.Dot(to_mouse_norm, Vector2.left));
            float offset = sign > 0.0f ? kAngleOffset * -sign + 180.0f : kAngleOffset * -sign;

			Vector2 new_pos = (to_mouse_norm * kKnifeLength) + mouse_pos;
            knife_transform.localScale = new Vector3(Mathf.Abs(knife_transform.localScale.x) * sign, knife_transform.localScale.y, knife_transform.localScale.z);
            knife_transform.position = new Vector3(new_pos.x, new_pos.y, knife_transform.position.z);
            knife_transform.rotation = Quaternion.Euler(0.0f, 0.0f, -(angle + offset));

            PointerEventData fake_pointer = new PointerEventData(EventSystem);
            fake_pointer.position = Camera.main.WorldToScreenPoint(knife_transform.position);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(fake_pointer, results);
            RaycastResult top_result = results[0];
			if (top_result.gameObject.name == "Bad Touch")
			{
                BadTouchCollision();
            }
        }
    }

    public void BadTouchCollision()
    {
        if (!TrackBadTouch)
        { return; }

        CurrentBadTouches += 1;

        //TODO: play audio
        TrackBadTouch = false;

        Sequence mySequence = DOTween.Sequence();

        Failure[CurrentBadTouches - 1].DOShakeAnchorPos(1.5f, CurrentBadTouches * 2, 10, 30);
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
        Destroy(this.gameObject);
        Debug.Log("Success :(");
    }

    private void FailMaze()
    {
        TrackBadTouch = false;
        Destroy(this.gameObject);
        Debug.Log("Failure :(");
    }
}
