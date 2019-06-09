using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour {

    [HideInInspector] public string dialogueTrigger, nextChar, goNext, buttonText, buttonPic, choiceFlag;
    [HideInInspector] public Text buttonDisplay;
    [HideInInspector] public Image buttonImage;
    public List<Sprite> buttonSprites;

    public void updateButton() //update what button shows etc.
    {
        buttonDisplay.text = buttonText; //text on button

        //button graphic
        switch (buttonPic)
        {
            default: //default button BG
                buttonImage.sprite = buttonSprites[0];
                break;
        }
    }

    public void choiceSelect() //selection done, leads to relevant dialogue
    {
        //return all buttons to pool, toggle off button holder
        Button[] buttons = DialogueManager.singleton.buttonHolder.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.gameObject.transform.SetParent(DialogueManager.singleton.gameObject.transform);
            button.gameObject.SetActive(false);
        }
        DialogueManager.singleton.buttonHolder.SetActive(false);

        //assign relevant variables and trigger dialogue
        DialogueManager.singleton.dialogueTrigger = dialogueTrigger;
        DialogueManager.singleton.nextChar = nextChar;
        DialogueManager.singleton.goNext = goNext;
        if (dialogueTrigger.Contains("select") && SceneManager.GetActiveScene().name == "Demo")
        {
            GameFlags.singleton.autoAdvance();
        }
        DialogueStartTrigger.singleton.dialogueTrigger(true);
    }
}
