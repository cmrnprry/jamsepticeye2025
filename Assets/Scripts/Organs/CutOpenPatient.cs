using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutOpenPatient : MonoBehaviour
{
    public GameObject Guts;
    public RectTransform rect;
    public int MaxCuts;

    private int CurrentCuts = 0;
    private bool Dragging = false;

    private Vector3 mousePosition;
    public Vector3 offset = new Vector3(17, -22, 0);
    public float moveSpeed = 0.5f;

    public void UpdateShouldFollow(bool new_state)
    {
        Dragging = new_state;
        //Cursor.visible = !new_state;
    }

    public void CutAlongLine(GameObject line)
    {
        if (!Dragging)
            return;

        CurrentCuts++;
        line.SetActive(false);

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
            mousePosition = Input.mousePosition + offset;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        }
            
    }
}
