using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    bool pressed;
    Vector3 pressedScale = new Vector3(0.8f,0.8f,0.8f);
    Vector3 normalScale = Vector3.one;
    RectTransform rect;
    public Canvas myCanvas;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
            transform.localScale = pressedScale;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            transform.localScale = normalScale;
        }
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }
}
