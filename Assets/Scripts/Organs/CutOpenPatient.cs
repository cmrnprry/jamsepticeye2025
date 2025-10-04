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
            rect.position = Input.mousePosition;
    }
}
