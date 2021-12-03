using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaiseButton : MonoBehaviour
{
    private float rtz;
    public float offset = 10f;
    private GameObject textObj;

    private void Start()
    {
        rtz = GetComponent<RectTransform>().anchoredPosition3D.z;
        Debug.Log("Got the button's RECT TRANSFORM");
        textObj = GetComponentInChildren<GameObject>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Raise button
        rtz -= offset;
        Debug.Log("we in this button thing");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Lower button
        rtz -= offset;
        Debug.Log("JERG BUTTON!");
    }
}
