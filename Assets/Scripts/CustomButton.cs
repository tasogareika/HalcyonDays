using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{ 
    //camera needs to have Physics 2D raycaster

    public void OnPointerClick (PointerEventData pointerEventData)
    {
        Debug.Log("kweh");
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

    }

    public void OnPointerExit (PointerEventData pointerEventData)
    {

    }
}
