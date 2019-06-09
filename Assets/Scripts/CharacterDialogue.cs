using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour {

    public static CharacterDialogue singleton;
    public Dialogue dialogue;

    void Awake()
    {
        singleton = this;
    }
}
