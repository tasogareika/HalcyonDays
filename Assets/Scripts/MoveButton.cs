using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static MoveButton singleton;
    private Button thisButton;
    private ObjectPulse pulse;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        thisButton = GetComponent<Button>();
        pulse = GetComponent<ObjectPulse>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (gameObject.activeInHierarchy)
        {
            pulse.startPulse();
        }
    }

    public void OnPointerExit (PointerEventData pointerEventData)
    {
        pulse.stopPulse();
    }

    public void toggleMovement()
    {
        pulse.stopPulse();
        thisButton.interactable = false;
        gameObject.SetActive(false);
        GameFlags.singleton.toggleItemButtons();
        GameFlags.singleton.movementMenu.SetActive(true);
    }

    public void closeMoveMenu()
    {
        GameFlags.singleton.movementMenu.SetActive(false);
        gameObject.SetActive(true);
        thisButton.interactable = true;
        StartCoroutine(DialogueManager.singleton.exploreToggle(0.1f));
    }
}
