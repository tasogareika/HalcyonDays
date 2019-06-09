using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartTrigger : MonoBehaviour {

    public static DialogueStartTrigger singleton;

    void Awake()
    {
        singleton = this;
    }

    public void buttonPress(bool instant)
    {
        dialogueTrigger(instant);
    }

    public void dialogueTrigger(bool instant) //trigger dialogue
    {
        for (int i = 0; i < DialogueManager.singleton.characters.Count; i++) {

            //tl;dr find the character on the list and trigger the start dialogue on that character
            if (DialogueManager.singleton.nextChar == DialogueManager.singleton.characters[i].name)
            {
                DialogueManager.singleton.dialogueButton.SetActive(false);
                if (!instant)
                {
                    if (!DialogueManager.singleton.speakerText.enabled)
                    {
                        DialogueManager.singleton.textBoxControl.Play("TextBoxAppear");
                        StartCoroutine(startDialogue(2.2f, i));
                    }
                    else
                    {
                        StartCoroutine(startDialogue(0.1f, i));
                    }
                } else
                {
                    DialogueManager.singleton.textBoxControl.Play("TextBoxOpen");
                    StartCoroutine(startDialogue(0.1f, i));
                }
                return; //end search once correct character is found
            }
        }
    }

    IEnumerator startDialogue (float waitTime, int i)
    {
        yield return new WaitForSeconds (waitTime);
        DialogueManager.singleton.StartDialogue(DialogueManager.singleton.characters[i].GetComponent<CharacterDialogue>().dialogue);
    }
}
