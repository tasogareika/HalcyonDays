using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public string name;
    public static int choiceTotal;
    public List<string> sentences, emotions, choices;
    public List<Sprite> sprites;

    public void Start()
    {
        choiceTotal = 0;
    }

    public void parseDialogue(string bookmark)
    {
        sentences.Clear();
        emotions.Clear();
        choices.Clear();
        choiceTotal = 0;
        LoadXMLFile.singleton.label = bookmark;
        LoadXMLFile.singleton.updateDialogue();
        sentences = new List<string>(LoadXMLFile.singleton.textList);
        emotions = new List<string>(LoadXMLFile.singleton.emotionList);
        choices = new List<string>(LoadXMLFile.singleton.buttonList);

        //deals with the trigger stuff (no multiple options)
        if (sentences.Count != emotions.Count)
        {
            //breaking apart the string into individual variables
            string[] triggers = emotions[emotions.Count - 1].Split(',');
            List<string> triggerList = new List<string>();
            foreach (string s in triggers)
            {
                triggerList.Add(s);
            }

            DialogueManager.singleton.nextChar = triggerList[0];
            DialogueManager.singleton.dialogueTrigger = triggerList[1];
            DialogueManager.singleton.goNext = triggerList[2];
        }

        //multiple options enabled
        if (choices.Count != 0)
        {
            choiceTotal = choices.Count;
            //button creation for multiple options
            for (int i = 0; i < choices.Count; i++)
            {
                string[] variables = choices[i].Split(',');
                List<string> variableList = new List<string>();
                foreach (string v in variables)
                {
                    variableList.Add(v);
                }

                DialogueManager.singleton.createChoiceButton(variableList);
            }
        }
    }

    public void setEmotion (string emotion) //setting sprites during dialogue
    {
        switch (emotion) {
            case "normal":
                DialogueManager.singleton.changeSprite(sprites[1]);
                break;

            case "happy":
                DialogueManager.singleton.changeSprite(sprites[2]);
                break;

            case "worry":
                DialogueManager.singleton.changeSprite(sprites[3]);
                break;

            case "fond":
                DialogueManager.singleton.changeSprite(sprites[4]);
                break;

            case "annoy":
                DialogueManager.singleton.changeSprite(sprites[5]);
                break;

            case "coinnormal":
                DialogueManager.singleton.changeSprite(sprites[6]);
                break;

            case "coinhappy":
                DialogueManager.singleton.changeSprite(sprites[7]);
                break;

            case "coinworry":
                DialogueManager.singleton.changeSprite(sprites[8]);
                break;

            case "coinfond":
                DialogueManager.singleton.changeSprite(sprites[9]);
                break;

            case "coinannoy":
                DialogueManager.singleton.changeSprite(sprites[10]);
                break;

            case "null":
                DialogueManager.singleton.clearSprite();
                break;

            default:
                DialogueManager.singleton.maintainSprite();
                break;
        }
    }
}
