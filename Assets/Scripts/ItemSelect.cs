using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] private string dialogueTag, character;
    public int timesClicked;

    private void Start()
    {
        timesClicked = 0;
    }

    public void itemClick()
    {
        GameFlags.singleton.toggleItemButtons();
        DialogueManager.singleton.moveButton.SetActive(false);

        timesClicked++;
        if (dialogueTag == "couch" && timesClicked > 1 && SceneManager.GetActiveScene().name == "Demo")
        {
            dialogueTag = "couchnotdone";
        }
        DialogueManager.singleton.nextChar = character;
        DialogueManager.singleton.dialogueTrigger = dialogueTag;
        DialogueStartTrigger.singleton.dialogueTrigger(false);
    }
}
