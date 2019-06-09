using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using RedBlueGames.Tools.TextTyper;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager singleton;
    //QUEUE type - FIRST IN FIRST OUT (FIFO), more restrictive than a list type
    [HideInInspector] public Queue<string> sentences, emotions;
    [HideInInspector] public List<GameObject> characters;
    [HideInInspector] public List<string> SFXList;
    public string dialogueTrigger, nextChar, goNext;
    [HideInInspector] public GameObject moveButton;
    [SerializeField] private GameObject choiceButton, nextButton, nyandroid, tom, hank, narrator, connor, unknown;
    [SerializeField] private Image displayImage, background, transitionBG;
    private string currSentence;
    public TextMeshProUGUI speakerName, speakerText;
    public GameObject buttonHolder, dialogueButton;
    private Dialogue currCharacter;
    private GameObject buttonRef;
    private int sentenceNo;
    private bool hasChoices, instant, audioFade;

    public List<Sprite> backgrounds; //0 empty, 1 kitchen, 2 bedroom
    public Animator textBoxControl;

    void Awake ()
    {
        singleton = this;
    }

	void Start () {

        //variable setting
        hasChoices = false;
        instant = false;
        audioFade = false;
        sentenceNo = 0;
        buttonHolder.SetActive(false);
        moveButton.SetActive(false);

        sentences = new Queue<string>();
        emotions = new Queue<string>();
        SFXList = new List<string>();

        characters = new List<GameObject>();
        GameObject[] charArray = GameObject.FindGameObjectsWithTag("Characters");
        foreach (var c in charArray)
        {
            characters.Add(c);
        }

        //initial stuff
        nextChar = "Narrator";
        dialogueTrigger = "dialogue1";
        background.sprite = backgrounds[6];
        transitionBG.color = new Color(1, 1, 1, 0);
        transitionBG.sprite = backgrounds[2];

        dialogueButton.SetActive(false);

        StartCoroutine(fadeImage(background, transitionBG, backgrounds[2], null, 0, true));
    }

    private void BeginGame()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            DialogueStartTrigger.singleton.buttonPress(instant);
        }
        else
        {
            dialogueButton.SetActive(true);
            dialogueButton.GetComponent<Button>().onClick.AddListener(delegate { DialogueStartTrigger.singleton.buttonPress(instant); });
        }
    }

    private void Update()
    {
        if (CheckInput()) //skip typewriter effect to show current full sentence
        {
            if (!nextButton.GetComponent<Button>().interactable)
            {
                if (speakerText.text.Length > 1)
                {
                    speakerText.GetComponent<TextTyper>().Skip();
                    return;
                }
            }
            else
            {
                DisplayNextSentence();
            }
        }

        //testing only on editor
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LoadXMLFile.singleton.nextXML = "1";
                LoadXMLFile.singleton.switchXML();
                GameFlags.singleton.currState = GameFlags.GAMESTATE.EXPLORING;
                moveButton.SetActive(true);
                nextChar = "Explore";
                //GameFlags.singleton.autoAdvance();
            }
            else if (Input.GetKeyDown(KeyCode.W)) //reload scene
            {
                SceneManager.LoadScene("Demo");
            }
        }
    }

    private bool CheckInput()
    {
        bool canProceed = false;

        if (GameFlags.singleton.currState == GameFlags.GAMESTATE.NIGHT && nextChar == "Window")
        {
            if (Input.GetMouseButtonDown(0))
            {
                canProceed = true;
            }
        } else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                canProceed = true;
            }
        }

        return canProceed;
    }

    public void StartDialogue(Dialogue dialogue) //start dialogue branch
    {
        currCharacter = dialogue;
        speakerName.text = currCharacter.name;
        speakerText.text = string.Empty;
        sentenceNo = 0;

        if (speakerName.text == "Narrator")
        {
            textBoxControl.Play("TextBoxNamelessOpen");
        } else
        {
            textBoxControl.Play("TextBoxOpen");
        }

        dialogue.parseDialogue(dialogueTrigger);

        //music toggle
        if (LoadXMLFile.singleton.BGMNo > 0)
        {
            GameFlags.singleton.startBGM(LoadXMLFile.singleton.BGMNo);
        } else if (LoadXMLFile.singleton.BGMNo == 0)
        {
            audioFade = true;
        }

        if (dialogue.choices.Count != 0) //set enabling of multiple options if defined
        {
            hasChoices = true;
        } else
        {
            hasChoices = false;
        }

        sentences.Clear();
        emotions.Clear();
        SFXList.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (string emotion in dialogue.emotions)
        {
            emotions.Enqueue(emotion);
        }

        if (LoadXMLFile.singleton.soundEffects.Length > 1) //prepare SFX list from XML file
        {
            string[] sounds = LoadXMLFile.singleton.soundEffects.Split('/');
            foreach (string s in sounds)
            {
                SFXList.Add(s);
            }
        }

        DisplayNextSentence();
    }  

    public void DisplayNextSentence() //update to next sentence in dialogue
    {
        sentenceNo++;
        speakerText.text = string.Empty;
        nextButton.GetComponent<ObjectPulse>().stopPulse();
        nextButton.GetComponent<Button>().interactable = false;

        //adjust text box size if there is dialogue or not
        if (speakerName.text == "Narrator")
        {
            speakerText.GetComponent<RectTransform>().SetTop(20);
        }
        else
        {
            speakerText.GetComponent<RectTransform>().SetTop(30);
        }

        if (sentences.Count == 1)
        {
            if (audioFade) //if BGM needs to fade
            {
                audioFade = false;
                GameFlags.singleton.fadeBGM();
            }
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        if (SFXList.Count > 0) //SFX toggling
        {
            for (int i = 0; i < SFXList.Count; i++)
            {
                string val = SFXList[i][0].ToString();
                if (sentenceNo == int.Parse(val))
                {
                    string[] sound = SFXList[i].Split(',');
                    GameFlags.singleton.playSFX(int.Parse(sound[1]));
                    SFXList.RemoveRange(i, 1);
                }
            }
        }

        string sentence = sentences.Dequeue();
        currSentence = sentence;
        string emotion = emotions.Dequeue();
        displayImage.color = Color.clear;
        StopAllCoroutines();
        speakerText.GetComponent<TextTyper>().TypeText(sentence);

        //since connor is only other character, just set him now
        connor.GetComponent<CharacterDialogue>().dialogue.setEmotion(emotion);

        //currCharacter.setEmotion(emotion); //the usual method of sprite setting but edited now due to Connor being the only other character on screen

        //only do special commands after everything else 
        if (LoadXMLFile.singleton.commands.Length > 1)
        {
            string[] vars = LoadXMLFile.singleton.commands.Split(',');
            if (sentenceNo == int.Parse(vars[0]))
            {
                GameFlags.singleton.determineCommand(vars[1]);
            }
        }
    }

    public void clearSprite() //clear sprite
    {
        displayImage.color = Color.clear;
        displayImage.sprite = null;
    }

    public void changeSprite (Sprite thisSprite) //changing sprites
    {
        if (thisSprite == null)
        {
            displayImage.color = Color.clear;
        } else
        {
            if (displayImage.sprite == null)
            {
                displayImage.color = Color.clear;
                displayImage.sprite = thisSprite;
                displayImage.gameObject.GetComponent<Animation>().Play("SpriteEnter");
            }
            else
            {
                displayImage.color = Color.white;
                displayImage.sprite = thisSprite;
            }
        }
    }

    public void maintainSprite() //stay same sprite if change is not pushed
    {
        if (displayImage.sprite == null)
        {
            displayImage.color = Color.clear;
        } else
        {
            displayImage.color = Color.white;
        }
    }

    public void ShowFullSentence()
    {
        StopAllCoroutines();
        speakerText.text = "";
        speakerText.text = currSentence;
        nextButton.GetComponent<ObjectPulse>().startPulse();
        nextButton.GetComponent<Button>().interactable = true;
    }

    public void createChoiceButton(List<string> variables) //creating the buttons when multiple choice happens
    {
        int count = transform.childCount;
        if (buttonHolder.transform.childCount < Dialogue.choiceTotal)
        {
            if (count < 1) //if no more buttons available, make new one. otherwise take from pool.
            {
                buttonRef = Instantiate(choiceButton, Vector3.zero, Quaternion.identity) as GameObject;
                var btnScript = buttonRef.GetComponent<ChoiceButton>();
                btnScript.buttonImage = buttonRef.GetComponent<Image>();
                btnScript.buttonDisplay = buttonRef.transform.GetChild(0).GetComponent<Text>();
            }
            else
            {
                buttonRef = transform.GetChild(0).gameObject;
                buttonRef.SetActive(true);
            }

            buttonRef.transform.SetParent(buttonHolder.transform);
            buttonRef.transform.position = Vector3.zero;

            //assigning variables
            var buttonScript = buttonRef.GetComponent<ChoiceButton>();
            buttonScript.nextChar = variables[0];
            buttonScript.dialogueTrigger = variables[1];
            buttonScript.goNext = variables[2];
            buttonScript.buttonText = variables[3];
            buttonScript.buttonPic = variables[4];
            
            buttonScript.updateButton();

            Dialogue.choiceTotal++; //add number of buttons done (to ensure no multiples are made)
        }
    }

    public void EndDialogue() //end of current dialogue
    {
        //blank out text displays
        speakerName.text = string.Empty;
        speakerText.text = string.Empty;
        //displayImage.color = Color.clear;

        //save choice (if needed)
        if (LoadXMLFile.singleton.decision.Length > 0)
        {
            GameFlags.singleton.choiceFlags.Add(LoadXMLFile.singleton.decision);
        }

        if (LoadXMLFile.singleton.nextXML.Length > 0) //if XMLfile needs to be switched
        {
            LoadXMLFile.singleton.switchXML();
        }

        if (dialogueTrigger.Contains("branchdialogue")) //if need to determine branched dialogue
        {
            dialogueTrigger = determineBranch();
        }

        //to insta trigger next bunch of dialogue
        if (goNext == "true")
        {
            //for multiple choice options
            if (hasChoices)
            {
                buttonHolder.SetActive(true);
                textBoxControl.Play("TextBoxShut");
                displayImage.color = Color.clear;
                displayImage.sprite = null;
            } else
            {
                instant = false;
                DialogueStartTrigger.singleton.dialogueTrigger(false);
            }

        } else if (goNext == "false")
        {
            instant = true;
            textBoxControl.Play("TextBoxVanish");
            if (!hasChoices)
            {
                displayImage.color = Color.clear;
                displayImage.sprite = null;
                if (LoadXMLFile.singleton.location.Length < 1) //check if scene transition needs to happen
                {
                    switch (nextChar)
                    {
                        case "Explore":
                            if (GameFlags.singleton.currState != GameFlags.GAMESTATE.EXPLORING)
                            {
                                GameFlags.singleton.setUpExplore();
                                GameFlags.singleton.toggleLocationButtons(4);
                                moveButton.SetActive(true);
                            } else
                            {
                                if (!GameFlags.singleton.checkExploreDone())
                                {
                                    StartCoroutine(exploreToggle(2.2f));
                                } else
                                {
                                    //explore done
                                    GameFlags.singleton.endExploration(background.sprite.name);
                                }
                            }
                            break;

                        default:
                            StartCoroutine(openDialogueToggle(2.2f));
                            break;
                    }
                } else
                {
                    if (GameFlags.singleton.currState == GameFlags.GAMESTATE.EXPLORING && GameFlags.singleton.checkExploreDone())
                    {
                        GameFlags.singleton.closeItemButtons();
                    }

                    toggleLocationShift(LoadXMLFile.singleton.location);
                }
            } else
            {
                displayImage.color = Color.clear;
                displayImage.sprite = null;
                buttonHolder.SetActive(true);
                textBoxControl.Play("TextBoxShut");
            }
        }
    }

    public void clearDialogue() //clear dialogue box without any auto proceeding etc. (for special commands and such)
    {
        //blank out text displays
        speakerName.text = string.Empty;
        speakerText.text = string.Empty;
        textBoxControl.Play("TextBoxShut");
        displayImage.color = Color.clear;
    }

    private string determineBranch()
    {
        string branch;
        string[] values = dialogueTrigger.Split('/');
        branch = GameFlags.singleton.choiceFlags[int.Parse(values[1])];
        return branch;
    }

    public void toggleLocationShift(string location)
    {
        shiftLocation(location);
    }

    private void shiftLocation(string location)
    {
        //clear sprites as well due to scene transition
        displayImage.sprite = null;
        displayImage.color = Color.clear;

        //change background image
        Sprite newBG, oldBG;
        string[] variables = location.Split(',');
        switch (variables[0])
        {
            case "kitchen":
                newBG = backgrounds[1];
                break;

            case "bedroom":
                newBG = backgrounds[2];
                break;

            case "bathroom":
                newBG = backgrounds[3];
                break;

            case "living":
                newBG = backgrounds[4];
                break;

            case "postit":
                newBG = backgrounds[5];
                break;

            case "blackout":
                newBG = backgrounds[6];
                break;

            case "couchcuddle":
                newBG = backgrounds[7];
                break;

            default:
                newBG = backgrounds[0];
                break;
        }
        oldBG = background.sprite;
        transitionBG.sprite = newBG;

        if (variables.Length > 2)
        {
            string s = variables[2];
            GameFlags.singleton.playSFX(int.Parse(s));
        }

        StartCoroutine(fadeImage(background, transitionBG, newBG, variables[1], backgrounds.IndexOf(newBG), true));
    }

    private void sceneTransit(string check, int location)
    {
        if (GameFlags.singleton.currState != GameFlags.GAMESTATE.EXPLORING)
        {
            if (check != "true")
            {
                StartCoroutine(openDialogueToggle(0.1f));
            }
            else
            {
                instant = false;
                DialogueStartTrigger.singleton.dialogueTrigger(false);
            }
        } else
        {
            switch (nextChar)
            {
                case "Explore":
                    GameFlags.singleton.toggleLocationButtons(location);
                    moveButton.GetComponent<Button>().interactable = true;
                    moveButton.SetActive(true);
                    break;

                case "Narrator":
                    instant = false;
                    DialogueStartTrigger.singleton.dialogueTrigger(false);
                    break;
            }
        }
    }

    IEnumerator fadeImage(Image background, Image trans, Sprite newBG, string check, int location, bool fade)
    {
        if (fade)
        {
            for (float i = 0; i < 1.1; i += (0.5f * Time.deltaTime))
            {
                trans.color = new Color(1, 1, 1, i);
                if (i >= 1)
                {
                    fade = false;
                    StopAllCoroutines();
                    background.sprite = newBG;
                    trans.sprite = null;
                    trans.color = new Color(1, 1, 1, 0);
                    if (GameFlags.singleton.currState == GameFlags.GAMESTATE.MORNING && dialogueTrigger == "dialogue1")
                    {
                        BeginGame();
                    }
                    else
                    {
                        sceneTransit(check, location);
                    }
                }
                yield return null;
            }
        }
    }

    public bool checkLocation(string location)
    {
        bool check;
        if (background.sprite.name.Contains(location))
        {
            check = true;
        } else
        {
            check = false;
        }
        return check;
    }

    public IEnumerator exploreToggle (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moveButton.SetActive(true);
        GameFlags.singleton.toggleLocationButtons(backgrounds.IndexOf(background.sprite));
    }

    IEnumerator openDialogueToggle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        instant = false;
        dialogueButton.SetActive(true);
        dialogueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        dialogueButton.GetComponent<Button>().onClick.AddListener (delegate { DialogueStartTrigger.singleton.buttonPress(instant); });
    }
}