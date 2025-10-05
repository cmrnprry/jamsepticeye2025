using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutOpenPatient : MonoBehaviour
{
    public GameObject Guts;
    public Transform rect;
    public int MaxCuts;

    private int CurrentCuts = 0;
    private bool Dragging = false;

    private Vector3 mousePosition;
    public float moveSpeed = 2f;

    public void UpdateShouldFollow(bool new_state)
    {
        Dragging = new_state;
        AudioManager.instance.PlaySFX("pickup_scalpel");
		Cursor.visible = !new_state;
    }

    public void CutAlongLine(GameObject line)
    {
        if (!Dragging)
            return;

        CurrentCuts++;
        line.SetActive(false);
        AudioManager.instance.PlaySFX("squish");
        AudioManager.instance.PlaySFX("crunch" + Random.Range(1,4));

        if (CurrentCuts >= MaxCuts)
        {
            CurrentCuts = 0;
            Cursor.visible = true;
            Guts.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Dragging)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            rect.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        }
            
    }
}
