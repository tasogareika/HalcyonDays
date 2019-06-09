using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlags : MonoBehaviour
{
    public enum GAMESTATE { MORNING, EXPLORING, NIGHT, TEASER };
    public GAMESTATE currState;
    public static GameFlags singleton;
    public List<Text> locationText;
    public List<GameObject> locationButtons;
    [HideInInspector] public GameObject thisLocation, movementMenu;
    [HideInInspector] public List<GameObject> livingRoomStuff, bedroomStuff, kitchenStuff, bathroomStuff;
    [HideInInspector] public List<string> choiceFlags;
    [SerializeField] private AudioSource audioPlayer, SFXPlayer;
    [SerializeField] private GameObject eventSystem, moveTipDisplay;
    public List<AudioClip> BGMList, SFXList;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        currState = GAMESTATE.MORNING;
        thisLocation = null;
        choiceFlags = new List<string>();
        createButtonLists();
        toggleItemButtons();
        UIResize();
    }

    private void UIResize()
    {
        //moveTip
        moveTipDisplay.SetActive(false);

        //movement Menu
        movementMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.7f, Screen.height * 0.6f);
        var menuRect = movementMenu.GetComponent<RectTransform>().rect;
        movementMenu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(menuRect.width * 0.01f, menuRect.height * -0.1f);
        movementMenu.SetActive(false);
    }

    public void moveMenuTextToggle()
    {
        EventSystem system = eventSystem.GetComponent<EventSystem>();
        Text[] textArray = movementMenu.transform.GetChild(0).GetComponentsInChildren<Text>();
        foreach (var t in textArray)
        {
            t.enabled = false;
        }

        if (thisLocation != null && thisLocation.tag == "Map")
        {
            int index = locationButtons.IndexOf(thisLocation);
            locationText[index].enabled = true;
        }
    }

    private void createButtonLists()
    {
        livingRoomStuff = new List<GameObject>();
        bedroomStuff = new List<GameObject>();
        kitchenStuff = new List<GameObject>();
        bathroomStuff = new List<GameObject>();

        foreach (Transform t in GameObject.Find("Living Room Buttons").transform)
        {
            livingRoomStuff.Add(t.gameObject);
        }

        foreach (Transform t in GameObject.Find("Bedroom Buttons").transform)
        {
            bedroomStuff.Add(t.gameObject);
        }

        foreach (Transform t in GameObject.Find("Kitchen Buttons").transform)
        {
            kitchenStuff.Add(t.gameObject);
        }

        foreach (Transform t in GameObject.Find("Bathroom Buttons").transform)
        {
            bathroomStuff.Add(t.gameObject);
        }
    }

    public void dismissMoveTip()
    {
        moveTipDisplay.SetActive(false);
    }

    public void setUpExplore()
    {
        moveTipDisplay.SetActive(true);
        currState = GAMESTATE.EXPLORING;
        startBGM(3);
    }

    public void closeItemButtons()
    {
        List<GameObject> allItems = kitchenStuff.Concat(bedroomStuff).Concat(bathroomStuff).Concat(livingRoomStuff).ToList();
        foreach (var a in allItems)
        {
            a.SetActive(false);
        }
    }

    public void toggleItemButtons()
    {
        GameObject[] itemBtnArray = GameObject.FindGameObjectsWithTag("Items");
        foreach (var b in itemBtnArray)
        {
            b.GetComponent<Button>().interactable = false;
            if (currState != GAMESTATE.EXPLORING)
            {
                b.SetActive(false);
            }
        }
    }

    public void toggleLocationButtons(int location)
    {
        toggleItemButtons();
        switch (location)
        {
            case 1:
                foreach (var k in kitchenStuff)
                {
                    k.SetActive(true);
                    k.GetComponent<Button>().interactable = true;
                }
                break;

            case 2:
                foreach (var b in bedroomStuff)
                {
                    b.SetActive(true);
                    b.GetComponent<Button>().interactable = true;
                }
                break;

            case 3:
                foreach (var b2 in bathroomStuff)
                {
                    b2.SetActive(true);
                    b2.GetComponent<Button>().interactable = true;
                }
                break;

            case 4:
                foreach (var l in livingRoomStuff)
                {
                    l.SetActive(true);
                    l.GetComponent<Button>().interactable = true;
                }
                break;

            default:
                
                break;
        }
    }

    public void autoAdvance()
    {
        List<GameObject> allItems = kitchenStuff.Concat(bedroomStuff).Concat(bathroomStuff).Concat(livingRoomStuff).ToList();
        foreach (var a in allItems)
        {
            a.GetComponent<ItemSelect>().timesClicked++;
        }
    }

    public bool checkExploreDone() //check if everything has been explored
    {
        bool done;
        List<GameObject> allItems = kitchenStuff.Concat(bedroomStuff).Concat(bathroomStuff).Concat(livingRoomStuff).ToList();
        done = allItems.All(obj => obj.GetComponent<ItemSelect>().timesClicked != 0);
        return done;
    }

    public void endExploration(string location)
    {
        DialogueManager.singleton.nextChar = "Narrator";
        if (location == "LivingRoomBG")
        {
            DialogueManager.singleton.dialogueTrigger = "couchdone";
        } else
        {
            DialogueManager.singleton.dialogueTrigger = "explorecomplete";
        }
        DialogueStartTrigger.singleton.dialogueTrigger(false);
    }

    public void determineCommand(string command)
    {
        switch (command)
        {
            case "switchToNight":
                closeItemButtons();
                currState = GAMESTATE.NIGHT;
                break;

            case "stopMusic":
                audioPlayer.Stop();
                break;

            case "messagebox":
                DialogueManager.singleton.clearDialogue();
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        WindowsForm.singleton.CreateMyForm();
                        break;

                    case RuntimePlatform.WindowsPlayer:
                        WindowsForm.singleton.CreateMyForm();
                        break;

                    case RuntimePlatform.OSXPlayer:
                        MacOSEvents.singleton.showEndingBox();
                        break;

                    default:
                        MacOSEvents.singleton.showEndingBox();
                        break;
                }
                
                break;
        }
    }

    public void loadDemoCredits()
    {
        SceneManager.LoadScene("DemoCredits");
    }

    private void messageBoxToggle()
    {
        WindowsForm.singleton.CreateMyForm();
    }

    public void playSFX(int trackNo)
    {
        SFXPlayer.Stop();
        SFXPlayer.clip = SFXList[trackNo];
        SFXPlayer.Play();
    }

    public void startBGM(int trackNo)
    {
        if (trackNo != BGMList.IndexOf(audioPlayer.clip))
        {
            StopAllCoroutines();
            audioPlayer.Stop();
            audioPlayer.clip = BGMList[trackNo];
            audioPlayer.volume = 1;
            audioPlayer.Play();
        }
    }

    public void fadeBGM()
    {
        StartCoroutine(FadeOut(5f));
    }

    IEnumerator FadeOut(float FadeTime)
    {
        float startVolume = audioPlayer.volume;
        while (audioPlayer.volume > 0)
        {
            audioPlayer.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioPlayer.Stop();
    }
}